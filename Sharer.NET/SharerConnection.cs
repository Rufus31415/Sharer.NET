/*
                ███████╗██╗  ██╗ █████╗ ██████╗ ███████╗██████╗ 
                ██╔════╝██║  ██║██╔══██╗██╔══██╗██╔════╝██╔══██╗
                ███████╗███████║███████║██████╔╝█████╗  ██████╔╝
                ╚════██║██╔══██║██╔══██║██╔══██╗██╔══╝  ██╔══██╗
                ███████║██║  ██║██║  ██║██║  ██║███████╗██║  ██║
                ╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝    

    Sharer is a .NET and Arduino Library to facilitate communication between and Arduino board and a desktop application.
    With Sharer it is easy to remote call a function executed by Arduino, read and write a variable inside Arduino board memory.
    Sharer uses the Serial communication to implement the Sharer protocol and remote execute commands.

    License: MIT
    Author: Rufus31415
    Website: https://rufus31415.github.io
*/

using Sharer.Command;
using Sharer.FunctionCall;
using Sharer.UserData;
using Sharer.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;

namespace Sharer
{
    /// <summary>
    /// Sharer connection to an Arduino Board. Sharer offers features to remote call function and read/write variables on Arduino from a .NET assembly.
    /// </summary>
    public class SharerConnection
    {
        /// <summary>
        /// Static function that returns the list of all available COM port
        /// </summary>
        /// <returns>A string array that contains all serial COM port name you can use in SharerConnection constructor</returns>
        public static String[] GetSerialPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Event raised when user message are received (i.e. Sharer.write(...) or Sharer.print(...) or Sharer.println(...) is called in arduino code)
        /// </summary>
        public event EventHandler<UserDataReceivedEventArgs> UserDataReceived;

        /// <summary>
        /// Create a new Sharer connection. You should then call Connect() open COM Port
        /// </summary>
        /// <param name="portName">Name of the Arduino COM port (example "COM5")</param>
        /// <param name="baudRate">Serial communication baud rate. Should be the same in Arduino code Sharer.init(baudRate). Example : 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 31250, 38400, 57600, or 115200</param>
        /// <param name="parity">Optional: bit parity (default: Parity.None)</param>
        /// <param name="dataBit">Optional: Number of bits per byte (default: 8)</param>
        /// <param name="stopBits">Optional: The serial stop bit (default: StopBits.One)</param>
        /// <param name="handShake">Optional: Hand Shake of the serial communication (default: Handshake.None)</param>
        public SharerConnection(string portName, int baudRate, Parity parity = Parity.None, int dataBit = 8, StopBits stopBits = StopBits.One, Handshake handShake = Handshake.None)
        {
            // Configuration of the serial port
            _serialPort.PortName = portName;
            _serialPort.BaudRate = baudRate;
            _serialPort.Parity = parity;
            _serialPort.DataBits = dataBit;
            _serialPort.StopBits = stopBits;
            _serialPort.Handshake = handShake;
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            // Initialize internal state machine
            _receiveStep = ReceiveSteps.Free;

            _serialPort.DataReceived += serialPort_DataReceived;
        }

        /// <summary>
        /// Connect to Arduino board
        /// </summary>
        /// <param name="waitSharerAvailableTimeout">Optional: Maximum time in milliseconds to initiate a communication with the sharer library that runs on Arduino (0 not to wait for Sharer to answer)</param>
        /// <param name="refreshLists">Optional: Indicates if the variables and function lists should be refreshed</param>
        public void Connect(int waitSharerAvailableTimeout = 10000, bool refreshLists = true)
        {
                Disconnect();

                try
                {
                    _serialPort.Open();

                    if (waitSharerAvailableTimeout > 0)
                    {
                        var cmd = new SharerGetInfosCommand();
                        sendCommand(cmd);

                        if (!cmd.WaitAnswer(TimeSpan.FromMilliseconds(waitSharerAvailableTimeout)))
                        {
                            throw new Exception($"The serial communication is connected but Sharer is not available. Ensure that Sharer.Run() is called in loop() function and that setup() function takes less than {waitSharerAvailableTimeout}ms.");
                        }
                    }

                    if (refreshLists)
                    {
                        RefreshFunctions();
                        RefreshVariables();
                    }
                }
                catch                     
                {
                    Disconnect();
                    throw;
                }
        }

        /// <summary>
        /// Disconnect Sharer and close serial communication
        /// </summary>
        public void Disconnect()
        {
            lock (_serialPort)
            {
                if (Connected)
                {
                    _serialPort.Close();
                }

                _sentCommands.Clear();
            }
        }

        /// <summary>
        /// Indicates if the serial communication is opened
        /// </summary>
        public bool Connected
        {
            get
            {
                return _serialPort != null && _serialPort.IsOpen;
            }
        }

        /// <summary>
        /// List of shared functions. This list is refresh after a call to RefreshFunctions() or Connect()
        /// </summary>
        public readonly List<SharerFunction> Functions = new List<SharerFunction>();

        /// <summary>
        /// List of shared variables. This list is refresh after a call to RefreshVariables() or Connect()
        /// </summary>
        public readonly List<SharerVariable> Variables = new List<SharerVariable>();

        /// <summary>
        /// Refresh functions shared
        /// </summary>
        public void RefreshFunctions()
        {
            AssertConnected();

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

        /// <summary>
        /// Refresh variables shared
        /// </summary>
        public void RefreshVariables()
        {
            AssertConnected();

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

        /// <summary>
        /// Read simultaneously several variables in Arduino
        /// </summary>
        /// <param name="variables">Variable to read. Peek them in Variables field</param>
        /// <returns>Variable values and read status</returns>
        public List<SharerReadVariableReturn> ReadVariables(IEnumerable<SharerVariable> variables)
        {
            if (variables == null) throw new ArgumentNullException("variables");
            
            return ReadVariables(variables.Select((x) => x.Name).ToArray());
        }

        /// <summary>
        /// Read simultaneously several variables in Arduino
        /// </summary>
        /// <param name="names">Names of variable to read</param>
        /// <returns>Variable values and read status</returns>
        public List<SharerReadVariableReturn> ReadVariables(IEnumerable<string> names)
        {
            AssertConnected();

            if (names == null) throw new ArgumentNullException("names");
            if (names.Count() == 0) return new List<SharerReadVariableReturn>();

            var ids = new short[names.Count()];

            var types = new SharerType[names.Count()];

            for (int i = 0; i < names.Count(); i++)
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
                    foreach (var id in ids)
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
                throw new Exception("Error while reading variables", cmd.Exception);
            }

            return cmd.Values;
        }

        /// <summary>
        /// Write variables on Arduino
        /// </summary>
        /// <param name="values">List of values to simultaneously write</param>
        /// <returns>True if all variable has been succesfully written</returns>
        public bool WriteVariables(IEnumerable<SharerWriteValue> values)
        {
            AssertConnected();

            if (values == null) throw new ArgumentNullException("values");
            if (values.Count() == 0) return true;

            var valueToWrite = new List<SharerWriteValue>(values.Count());

            foreach (var value in values)
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

        /// <summary>
        /// Remote call a function by its name with arguments and get the returned value
        /// </summary>
        /// <typeparam name="ReturnType">The .NET type expected. Should be the same as the Arduino function return type</typeparam>
        /// <param name="functionName">Name of the function to call</param>
        /// <param name="timeout">Maximum expected duration of the function execution on Arduino</param>
        /// <param name="arguments">Optional list of argument values.</param>
        /// <returns>Status of the function call with its returned value</returns>
        public SharerFunctionReturn<ReturnType> Call<ReturnType>(string functionName, TimeSpan timeout, params object[] arguments)
        {
            AssertConnected();

            if (functionName == null)
            {
                throw new ArgumentNullException("functionName");
            }

            Int16 functionId = -1;
            SharerFunction function = null;

            for (int i = 0; i < Functions.Count; i++)
            {
                if (Functions[i].Name == functionName)
                {
                    function = Functions[i];
                    functionId = (Int16)i;
                    break;
                }
            }

            if (function == null || functionId < 0)
            {
                throw new Exception(functionName + " not found in function list. Try to update the function List or check if this function has been shared.");
            }

            if (arguments == null || arguments.Length == 0)
            {
                if (function.Arguments.Count != 0)
                {
                    throw new Exception("Attempt to call the function " + function.Name + " with no arguments, but the function has " + function.Arguments.Count + " arguments");
                }
            }
            else if (function.Arguments.Count != arguments.Length)
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
                            catch (Exception ex)
                            {
                                throw new Exception("Error in argument " + function.Arguments[i].Name + " of function " + function.Name, ex);
                            }
                        }
                    }


                    buffer = memory.ToArray();

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

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public SharerFunctionReturn<ReturnType> Call<ReturnType>(string functionName, params object[] arguments)
        {
            return Call<ReturnType>(functionName, DEFAULT_TIMEOUT, arguments);
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public SharerFunctionReturn<object> Call(string functionName, TimeSpan timeout, params object[] arguments)
        {
            return Call<object>(functionName, timeout, arguments);
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public SharerFunctionReturn<object> Call(string functionName, params object[] arguments)
        {
            return Call<object>(functionName, DEFAULT_TIMEOUT, arguments);
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public SharerGetInfosCommand GetInfos()
        {
            var cmd = new Sharer.Command.SharerGetInfosCommand();

            sendCommand(cmd);

            cmd.WaitAnswer(DEFAULT_TIMEOUT);

            return cmd;
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(char[] chars, int index, int count)
        {
            AssertConnected();

            lock (_serialPort)
            {
                _serialPort.Write(chars, index, count);
            }
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(string value)
        {
            AssertConnected();

            lock (_serialPort)
            {
                _serialPort.Write(value);
            }
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(float value)
        {
            WriteUserData(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(ulong value)
        {
            WriteUserData(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(long value)
        {
            WriteUserData(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(uint value)
        {
            WriteUserData(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(int value)
        {
            WriteUserData(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(ushort value)
        {
            WriteUserData(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(short value)
        {
            WriteUserData(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(byte[] buffer, int index, int count)
        {
            AssertConnected();

            lock (_serialPort)
            {
                _serialPort.Write(buffer, index, count);
            }
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(double value)
        {
            WriteUserData(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(char[] chars)
        {
            if (chars == null) throw new ArgumentNullException("chars");

            WriteUserData(chars, 0, chars.Length);
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(char ch)
        {
            WriteUserData(new char[] { ch });
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException("buffer");

            WriteUserData(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(sbyte value)
        {
            WriteUserData(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(byte value)
        {
            WriteUserData(new byte[] { value });
        }

        /// <summary>
        /// Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()
        /// </summary>
        public void WriteUserData(bool value)
        {
            WriteUserData((byte)(value ? 0xFF : 0x00));
        }

#region Private fields
        private readonly Dictionary<SharerCommandID, Type> _notificationCommand = new Dictionary<SharerCommandID, Type>() { /* For future notification commands (unsolicited messages) */ };

        /// <summary>
        /// Every sharer commands starts with this byte
        /// </summary>
        private const Byte SHARER_START_COMMAND_CHAR = 0x92;

        /// <summary>
        /// Default timeout for Sharer commands
        /// </summary>
        private TimeSpan DEFAULT_TIMEOUT = new TimeSpan(0, 0, 2);

        /// <summary>
        /// The serial port object
        /// </summary>
        private readonly SerialPort _serialPort = new SerialPort();

        /// <summary>
        /// internal states of the state machine
        /// </summary>
        private enum ReceiveSteps
        {
            Free,
            DeviceMessageId, 
            CommandId, 
            SupervisorMessageId,
            Body
        }

        /// <summary>
        /// Current state
        /// </summary>
        private ReceiveSteps _receiveStep;

        /// <summary>
        /// User message bytes received
        /// </summary>
        private readonly List<byte> _userData = new List<byte>(100);

        /// <summary>
        /// Last received message id
        /// </summary>
        private byte _lastDeviceMessageId;

        /// <summary>
        /// Last command received
        /// </summary>
        private SharerCommandID _lastCommandID;

        /// <summary>
        /// The sent command associated to the last received answer
        /// </summary>
        private SharerSentCommand _currentCommand;

        // ID of the last sent command (incremented at every new command sent)
        private byte _currentSupervisorCommandID = 0;

        // Buffer of all sent commands that has not receive an answer
        private readonly List<SharerSentCommand> _sentCommands = new List<SharerSentCommand>();
#endregion

#region Private methods
        /// <summary>
        /// Event handler called when serial data are received
        /// </summary>
        private void serialPort_DataReceived(object s, SerialDataReceivedEventArgs e)
        {
            lock (_serialPort)
            {
                try
                {
                    byte[] data = new byte[_serialPort.BytesToRead];
                    int count = _serialPort.Read(data, 0, data.Length);

                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            // Parse received bytes one by one
                            ParseReceivedData(data[i]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _receiveStep = ReceiveSteps.Free;
                    if (_currentCommand != null)
                    {
                        _currentCommand.EndReceive(false, ex);
                        _currentCommand = null;
                    }
                }
            }

            lock (_userData)
            {
                try
                {
                    // Propagate user data if any
                    if (_userData.Count > 0) UserDataReceived?.Invoke(this, new UserDataReceivedEventArgs(_userData.ToArray()));
                }
                catch { /* Do nothing, the user has to catch its own exceptions */ }
                finally { _userData.Clear(); }
            }
        }

        /// <summary>
        /// Parse received bytes one by one to make the internal state machine progress and fill the user message buffer
        /// </summary>
        /// <param name="b">The last byte received</param>
        private void ParseReceivedData(byte b)
        {
            switch (_receiveStep)
            {
                case ReceiveSteps.DeviceMessageId:
                    _lastDeviceMessageId = b;
                    _receiveStep = ReceiveSteps.CommandId;
                    break;

                case ReceiveSteps.CommandId:
                    if (Enum.GetValues(typeof(SharerCommandID)).OfType<SharerCommandID>().Contains((SharerCommandID)b))
                    {
                        _lastCommandID = (SharerCommandID)b;
                        _receiveStep = ReceiveSteps.SupervisorMessageId;
                    }
                    else
                    {
                        lock (_userData)
                        {
                            _userData.Add(SHARER_START_COMMAND_CHAR);
                            _userData.Add(_lastDeviceMessageId);
                            _userData.Add(b);
                        }
                        _receiveStep = ReceiveSteps.Free;
                    }

                    break;

                case ReceiveSteps.SupervisorMessageId:

                    // command without request
                    if (_notificationCommand.ContainsKey(_lastCommandID))
                    {
                        _currentCommand = (SharerSentCommand)Activator.CreateInstance(_notificationCommand[_lastCommandID]);
                        _currentCommand.Timeouted += _currentCommand_Timeouted;

                        _receiveStep = ReceiveSteps.Body;
                    }
                    else
                    {
                        _currentCommand = _sentCommands.FirstOrDefault((x) => x.CommandID == _lastCommandID && x.SentID == b);
                        _currentCommand.Timeouted += _currentCommand_Timeouted;

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
                    break;
                case ReceiveSteps.Body:

                    var decodageDone = _currentCommand.DecodeArgument(b);

                    if (decodageDone)
                    {
                        _currentCommand.EndReceive(true); // indicate that the command has been received
                        _receiveStep = ReceiveSteps.Free;

                        switch (_currentCommand.CommandID)
                        {
                            case SharerCommandID.Error:

                                break;
                        }
                    }
                    break;
                default:
                    if(_currentCommand != null)
                    {
                        _currentCommand.Timeouted -= _currentCommand_Timeouted;
                        _currentCommand = null;
                    }

                    if (b == SHARER_START_COMMAND_CHAR)
                    {
                        _receiveStep = ReceiveSteps.DeviceMessageId;
                    }
                    else
                    {
                        lock (_userData)
                        {
                            _userData.Add(b);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// Handler called when the current command has timeouted. Allow to reset and unlock the state machine
        /// </summary>
        private void _currentCommand_Timeouted(object sender, EventArgs e)
        {
            if(_receiveStep == ReceiveSteps.DeviceMessageId || _receiveStep == ReceiveSteps.Body)
            {
                _sentCommands.Remove(_currentCommand);
                _receiveStep = ReceiveSteps.Free;
            }
        }

        /// <summary>
        /// Throw an exception if the connection is closed
        /// </summary>
        private void AssertConnected()
        {
            if (!Connected) throw new Exception("Sharer interface is not connected.");
        }
                       
        /// <summary>
        /// Convert a command to a byte array and send it on serial port
        /// </summary>
        /// <param name="cmd">The command to send</param>
        private void sendCommand(SharerSentCommand cmd)
        {
            lock (_serialPort)
            {
                cmd.BeginSend(_currentSupervisorCommandID);

                var header = new byte[] { SHARER_START_COMMAND_CHAR, _currentSupervisorCommandID, (byte)cmd.CommandID };

                // Write header, same for all commands
                _serialPort.Write(header, 0, header.Length);

                // write optionnal arguments
                var buffer = cmd.ArgumentsToSend();
                if (buffer != null && buffer.Length > 0)
                {
                    _serialPort.Write(buffer, 0, buffer.Length);
                }

                _sentCommands.Add(cmd);

                _currentSupervisorCommandID++;
            }
        }
#endregion
    }        

}
