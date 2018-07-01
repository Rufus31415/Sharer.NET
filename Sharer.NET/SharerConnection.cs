using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharer.FunctionCall;
using Sharer.Command;
using System.IO;

namespace Sharer
{
    /// <summary>
    /// Connection to an Arduino Device
    /// </summary>
    public class SharerConnection
    {
        private const Byte SHARER_START_COMMAND_CHAR = 0x92;
        private TimeSpan DEFAULT_TIMEOUT = new TimeSpan(0, 0, 5);

        private SerialPort _serialPort = new SerialPort();

        public SharerConnection(string portName, int baudRate, Parity parity = Parity.None, int dataBit=8, StopBits stopBits=StopBits.One, Handshake handShake=Handshake.None)
        {
            _serialPort.PortName = portName;

            _serialPort.BaudRate = baudRate;

            _serialPort.Parity = parity;

            _serialPort.DataBits = dataBit;

            _serialPort.StopBits = stopBits;

            _serialPort.Handshake = handShake;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _receiveStep = ReceiveSteps.Free;

            _serialPort.DataReceived += serialPort_DataReceived;
        }

        private enum ReceiveSteps
        {
            Free,
            DeviceMessageId,
            CommandId,
            SupervisorMessageId,
            Body
        }
        private ReceiveSteps _receiveStep;

        public static String[] GetSerialPortNames()
        {
            return SerialPort.GetPortNames();
        }

       private void serialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
        {
            try
            {
                byte[] data = new byte[_serialPort.BytesToRead];
                int count = _serialPort.Read(data, 0, data.Length);

                if (count > 0)
                {    
                    for (int i = 0; i < count; i++)
                    {
                        ParseReceivedData(data[i]);
                    }
                }
            }
            catch(Exception ex)
            {
                _receiveStep = ReceiveSteps.Free;
                if(_currentCommand != null)
                {
                    _currentCommand.EndReceive(false);
                    _currentCommand = null;
                }
            }
        }

        private byte _lastDeviceMessageId;
        private SharerCommandID _lastCommandID;
        private SharerSentCommand _currentCommand;

        private void ParseReceivedData(byte b)
        {
            switch (_receiveStep)
            {
                case ReceiveSteps.DeviceMessageId:
                    _lastDeviceMessageId = b;
                    _receiveStep = ReceiveSteps.CommandId;
                    break;

                case ReceiveSteps.CommandId:
                    _lastCommandID = (SharerCommandID)b;

                    _receiveStep = ReceiveSteps.SupervisorMessageId;
                    // to do : handle asynchronous messages
                    break;

                case ReceiveSteps.SupervisorMessageId:

                    lock (_sentCommands)
                    {
                        _currentCommand = _sentCommands.FirstOrDefault((x) => x.CommandID == _lastCommandID && x.SentID == b);
                    }

                    if (_currentCommand == null)
                    {
                        _receiveStep = ReceiveSteps.Free;

                        Console.WriteLine("Command not found in history");
                        // to do : handle unfound command in the history
                    }
                    else
                    {
                        _receiveStep = ReceiveSteps.Body;
                    }
                    break;
                case ReceiveSteps.Body:

                    var decodageDone = _currentCommand.DecodeArgument(b);

                    if (decodageDone)
                    {
                        _currentCommand.EndReceive(true); // indicate that the command has been received
                        _receiveStep = ReceiveSteps.Free;
                    }
                    break;
                default:
                    if (b == SHARER_START_COMMAND_CHAR)
                    {
                        _receiveStep = ReceiveSteps.DeviceMessageId;
                    }
                    else
                    {
                        // to do : store value for user stream
                    }
                    break;
            }
        }


        public void Connect()
        {
            Disconnect();

            _serialPort.Open();

        }



        public void Disconnect()
        {
            if(Connected)
            {
                _serialPort.Close();
            }

        }


        public bool Connected
        {
            get
            {
                return _serialPort != null && _serialPort.IsOpen;
            }
        }

        private void assertConnected()
        {
            if (!Connected) throw new Exception("Not connected");
        }

       public List<SharerFunction> Functions = new List<SharerFunction>();

        public void RefreshFunctions()
        {
            assertConnected();

            var lst = new List<SharerFunction>();

            var cmd = new SharerGetAllFunctionsPrototypeCommand();

            sendCommand(cmd);

            bool success = cmd.WaitAnswer(DEFAULT_TIMEOUT);

            if (!success)
            {
                throw new Exception("Error while refreshing function list");
            }

            Functions.Clear();
            Functions.AddRange(cmd.Functions);
        }

        public SharerFunctionReturn<ReturnType> Call<ReturnType>(string functionName,TimeSpan timeout, params object[] arguments)
        {
            assertConnected();

            if (functionName == null)
            {
                throw new ArgumentNullException("functionName");
            }

            Int16 functionId = -1;
            SharerFunction function = null;

            for(int i = 0; i < Functions.Count; i++)
            {
                if(Functions[i].Name == functionName)
                {
                    function = Functions[i];
                    functionId = (Int16)i;
                    break;
                }
            }


            if (function == null || functionId<0)
            {
                throw new Exception(functionName + " not found in function list. Try to update the function List or check if this function has been shared.");
            }
           
            if(arguments == null || arguments.Length == 0)
            {
                if(function.Arguments.Count != 0)
                {
                    throw new Exception("Attempt to call the function " + function.Name + " with no arguments, but the function has " + function.Arguments.Count + " arguments");
                }
            }
            else if(function.Arguments.Count != arguments.Length)
            {
                throw new Exception("Attempt to call the function " + function.Name + " with " + arguments.Length + " argument(s), but the function has " + function.Arguments.Count + " argument(s)");
            }

            byte[] buffer;

            using (MemoryStream memory = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(memory))
                {
                    writer.Write(functionId);

                    if (arguments != null)
                    {
                        for (int i = 0; i < arguments.Length; i++)
                        {
                            if (arguments[i] == null)
                            {
                                throw new ArgumentNullException("arguments[" + i + "]");
                            }

                            try
                            {
                                SharerTypeHelper.Encode(function.Arguments[i].Type, writer, arguments[i]);
                            }
                            catch(Exception ex)
                            {
                                throw new Exception("Error in argument " + function.Arguments[i].Name + " of function " + function.Name, ex);
                            }
                        }
                    }

                
                buffer= memory.GetBuffer().Take((int)memory.Length).ToArray();

                }
            }

            var lst = new List<SharerFunction>();

            var cmd = new SharerCallFunctionCommand<ReturnType>(buffer, function.ReturnType);

            sendCommand(cmd);

            bool success = cmd.WaitAnswer(timeout);

            if (!success)
            {
                throw new Exception("Error while calling function " + functionName);
            }

            return cmd.Return;
        }

        public SharerFunctionReturn<ReturnType> Call<ReturnType>(string functionName, params object[] arguments)
        {
            return Call<ReturnType>(functionName, DEFAULT_TIMEOUT, arguments);
        }

        public SharerFunctionReturn<object> Call(string functionName, TimeSpan timeout, params object[] arguments)
        {
            return Call<object>(functionName, timeout, arguments);
        }

        public SharerFunctionReturn<object> Call(string functionName, params object[] arguments)
        {
            return Call<object>(functionName, DEFAULT_TIMEOUT, arguments);
        }



        // ID of the last sent command (incremented at every new command sent)
        private byte _currentSupervisorCommandID = 0;

        private List<SharerSentCommand> _sentCommands = new List<SharerSentCommand>();

        private void sendCommand(SharerSentCommand cmd)
        {
            lock (_sentCommands)
            {
                _currentSupervisorCommandID++;

            cmd.BeginSend(_currentSupervisorCommandID);

             var header = new byte[] { SHARER_START_COMMAND_CHAR, _currentSupervisorCommandID, (byte)cmd.CommandID };

            // Write header, same for all commands
            _serialPort.Write(header, 0, header.Length);

            // write optionnal arguments
            var buffer = cmd.ArgumentsToSend();
            if (buffer != null && buffer.Length>0)
            {
                _serialPort.Write(buffer, 0, buffer.Length);
            }


      
                _sentCommands.Add(cmd);
            }
        }

    }
}