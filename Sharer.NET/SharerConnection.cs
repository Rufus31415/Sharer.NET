using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sharer.FunctionCall;
using Sharer.Command;
using System.IO;
using Sharer.Variables;

namespace Sharer
{
    /// <summary>
    /// Connection to an Arduino Device
    /// </summary>
    public class SharerConnection
    {
        private Dictionary<SharerCommandID, Type> _notificationCommand = new Dictionary<SharerCommandID, Type>()
        {
            { SharerCommandID.Ready, typeof(SharerReadyCommand)},
        };

        private const Byte SHARER_START_COMMAND_CHAR = 0x92;
        private TimeSpan DEFAULT_TIMEOUT = new TimeSpan(0, 0, 2);

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
                    _currentCommand.EndReceive(false, ex);
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
                        // command without request
                        if (_notificationCommand.ContainsKey(_lastCommandID))
                        {
                            _currentCommand = (SharerSentCommand)Activator.CreateInstance(_notificationCommand[_lastCommandID]);
                            _receiveStep = ReceiveSteps.Body;
                        }
                        else
                        {
                            _currentCommand = _sentCommands.FirstOrDefault((x) => x.CommandID == _lastCommandID && x.SentID == b);

                            if (_currentCommand == null)
                            {
                                _receiveStep = ReceiveSteps.Free;

                                throw new Exception("Command ID " + _lastCommandID + " not found in history");
                            }
                            else
                            {
                                _sentCommands.Remove(_currentCommand);
                                _receiveStep = ReceiveSteps.Body;
                            }
                        }
                    }
                    break;
                case ReceiveSteps.Body:

                    var decodageDone = _currentCommand.DecodeArgument(b);

                    if (decodageDone)
                    {
                        _currentCommand.EndReceive(true); // indicate that the command has been received
                        _receiveStep = ReceiveSteps.Free;

                        switch (_currentCommand.CommandID)
                        {
                            case SharerCommandID.Ready:
                                AsyncNotifyReady();
                                break;
                            case SharerCommandID.Error:

                                break;
                        }
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

        private void AsyncNotifyReady()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    try
                    {
                        if (RefreshFunctionListOnReady) RefreshFunctions();
                        if (RefreshVariableListOnReady) RefreshVariables();
                    }
                    finally
                    {
                        try { Ready?.Invoke(this, EventArgs.Empty); }
                        catch { }
                    }
                }
                catch(Exception ex)
                {
                    RaiseInternalError(ex);
                }
            });
        }

       void RaiseInternalError(Exception ex)
        {
            try { InternalError?.Invoke(this, new ErrorEventArgs(ex)); }
            catch { }
        }

        public Boolean RefreshVariableListOnReady { get; set; } = true;
        public Boolean RefreshFunctionListOnReady { get; set; } = true;

        public event EventHandler Ready;

        public delegate void InternalErrorEventHandler(object o, ErrorEventArgs e);
        public event InternalErrorEventHandler InternalError;


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
        public List<SharerVariable> Variables = new List<SharerVariable>();

        public void RefreshFunctions()
        {
            assertConnected();

            var lst = new List<SharerFunction>();

            var cmd = new SharerGetAllFunctionsPrototypeCommand();

            sendCommand(cmd);

            bool success = cmd.WaitAnswer(DEFAULT_TIMEOUT);

            if (!success)
            {
                throw new Exception("Error while refreshing function list", cmd.Exception);
            }

            Functions.Clear();
            Functions.AddRange(cmd.Functions);
        }

        public void RefreshVariables()
        {
            assertConnected();

            var lst = new List<SharerVariable>();

            var cmd = new SharerGetAllVariablesDefinitionCommand();

            sendCommand(cmd);

            bool success = cmd.WaitAnswer(DEFAULT_TIMEOUT);

            if (!success)
            {
                throw new Exception("Error while refreshing variable list", cmd.Exception);
            }

            Variables.Clear();
            Variables.AddRange(cmd.Variables);
        }
        public List<SharerReadVariableReturn> ReadVariables(IEnumerable<SharerVariable> variables)
        {
            if (variables == null) throw new ArgumentNullException("variables");

            return ReadVariables(variables.Select((x) => x.Name).ToArray());
        }


        public List<SharerReadVariableReturn> ReadVariables(IEnumerable<string> names)
        {
            assertConnected();

            if (names == null) throw new ArgumentNullException("names");
            if (names.Count() == 0) return new List<SharerReadVariableReturn>();

            var ids = new short[names.Count()];

            var types = new SharerType[names.Count()];

            for(int i = 0; i < names.Count(); i++)
            {
                ids[i] = (short)Variables.FindIndex((x) => string.Equals(x.Name, names.ElementAt(i)));
                if (ids[i] < 0) throw new Exception(names.ElementAt(i) + " not found in variables");
                types[i] = Variables[ids[i]].Type;
            }

            byte[] buffer;

            using (MemoryStream memory = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(memory))
                {
                    writer.Write((short)ids.Length);
                    foreach(var id in ids)
                    {
                        writer.Write(id);
                    }
                        buffer = memory.ToArray();
                }
            }


            var cmd = new SharerReadVariablesCommand(buffer, types);

            sendCommand(cmd);

            bool success = cmd.WaitAnswer(DEFAULT_TIMEOUT);

            if (!success)
            {
                throw new Exception("Error while writing variables", cmd.Exception);
            }

            return cmd.Values;
        }


        public bool WriteVariables(IEnumerable<SharerWriteValue> values)
        {
            assertConnected();

            if (values == null) throw new ArgumentNullException("values");
            if (values.Count() == 0) return true;

            var valueToWrite = new List<SharerWriteValue>(values.Count());

            foreach(var value in values)
            {
                var index = Variables.FindIndex((x) => string.Equals(x.Name, value.Name));

                if (index < 0)
                {
                    value.Status = SharerWriteVariableStatus.NotFound;
                    value.Type = SharerType.@void;
                }
                else
                {
                    value.Index = index;
                    value.Type = Variables[index].Type;
                    valueToWrite.Add(value);
                }
            }

            // if all not found
            if (valueToWrite.Count == 0) return false;

            var cmd = new SharerWriteVariablesCommand(valueToWrite);

            sendCommand(cmd);

            bool success = cmd.WaitAnswer(DEFAULT_TIMEOUT);

            if (!success)
            {
                throw new Exception("Error while writting variables", cmd.Exception);
            }


            return values.All((x) => x.Status == SharerWriteVariableStatus.OK);
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

                
                buffer= memory.ToArray();

                }
            }

            var cmd = new SharerCallFunctionCommand<ReturnType>(buffer, function.ReturnType);

            sendCommand(cmd);

            bool success = cmd.WaitAnswer(timeout);

            if (!success)
            {
                throw new Exception("Error while calling function " + functionName, cmd.Exception);
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

public class ErrorEventArgs : EventArgs
{
    public readonly Exception Exception;

    public ErrorEventArgs(Exception ex)
    {
        Exception = ex;
    }
}