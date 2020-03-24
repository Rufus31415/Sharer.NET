<a name='assembly'></a>
# Sharer

## Contents

- [InternalBinaryReader](#T-Sharer-UserData-UserDataReceivedEventArgs-InternalBinaryReader 'Sharer.UserData.UserDataReceivedEventArgs.InternalBinaryReader')
- [ReceiveSteps](#T-Sharer-SharerConnection-ReceiveSteps 'Sharer.SharerConnection.ReceiveSteps')
- [SharerCallFunctionCommand\`1](#T-Sharer-Command-SharerCallFunctionCommand`1 'Sharer.Command.SharerCallFunctionCommand`1')
- [SharerCallFunctionStatus](#T-Sharer-Command-SharerCallFunctionStatus 'Sharer.Command.SharerCallFunctionStatus')
  - [FunctionIdOutOfRange](#F-Sharer-Command-SharerCallFunctionStatus-FunctionIdOutOfRange 'Sharer.Command.SharerCallFunctionStatus.FunctionIdOutOfRange')
  - [NotYetCalled](#F-Sharer-Command-SharerCallFunctionStatus-NotYetCalled 'Sharer.Command.SharerCallFunctionStatus.NotYetCalled')
  - [OK](#F-Sharer-Command-SharerCallFunctionStatus-OK 'Sharer.Command.SharerCallFunctionStatus.OK')
  - [UnknownType](#F-Sharer-Command-SharerCallFunctionStatus-UnknownType 'Sharer.Command.SharerCallFunctionStatus.UnknownType')
- [SharerCommandID](#T-Sharer-Command-SharerCommandID 'Sharer.Command.SharerCommandID')
- [SharerConnection](#T-Sharer-SharerConnection 'Sharer.SharerConnection')
  - [#ctor(portName,baudRate,parity,dataBit,stopBits,handShake)](#M-Sharer-SharerConnection-#ctor-System-String,System-Int32,System-IO-Ports-Parity,System-Int32,System-IO-Ports-StopBits,System-IO-Ports-Handshake- 'Sharer.SharerConnection.#ctor(System.String,System.Int32,System.IO.Ports.Parity,System.Int32,System.IO.Ports.StopBits,System.IO.Ports.Handshake)')
  - [DEFAULT_TIMEOUT](#F-Sharer-SharerConnection-DEFAULT_TIMEOUT 'Sharer.SharerConnection.DEFAULT_TIMEOUT')
  - [Functions](#F-Sharer-SharerConnection-Functions 'Sharer.SharerConnection.Functions')
  - [SHARER_START_COMMAND_CHAR](#F-Sharer-SharerConnection-SHARER_START_COMMAND_CHAR 'Sharer.SharerConnection.SHARER_START_COMMAND_CHAR')
  - [Variables](#F-Sharer-SharerConnection-Variables 'Sharer.SharerConnection.Variables')
  - [_currentCommand](#F-Sharer-SharerConnection-_currentCommand 'Sharer.SharerConnection._currentCommand')
  - [_lastCommandID](#F-Sharer-SharerConnection-_lastCommandID 'Sharer.SharerConnection._lastCommandID')
  - [_lastDeviceMessageId](#F-Sharer-SharerConnection-_lastDeviceMessageId 'Sharer.SharerConnection._lastDeviceMessageId')
  - [_receiveStep](#F-Sharer-SharerConnection-_receiveStep 'Sharer.SharerConnection._receiveStep')
  - [_serialPort](#F-Sharer-SharerConnection-_serialPort 'Sharer.SharerConnection._serialPort')
  - [_userData](#F-Sharer-SharerConnection-_userData 'Sharer.SharerConnection._userData')
  - [Connected](#P-Sharer-SharerConnection-Connected 'Sharer.SharerConnection.Connected')
  - [AssertConnected()](#M-Sharer-SharerConnection-AssertConnected 'Sharer.SharerConnection.AssertConnected')
  - [Call()](#M-Sharer-SharerConnection-Call-System-String,System-TimeSpan,System-Object[]- 'Sharer.SharerConnection.Call(System.String,System.TimeSpan,System.Object[])')
  - [Call()](#M-Sharer-SharerConnection-Call-System-String,System-Object[]- 'Sharer.SharerConnection.Call(System.String,System.Object[])')
  - [Call\`\`1(functionName,timeout,arguments)](#M-Sharer-SharerConnection-Call``1-System-String,System-TimeSpan,System-Object[]- 'Sharer.SharerConnection.Call``1(System.String,System.TimeSpan,System.Object[])')
  - [Call\`\`1()](#M-Sharer-SharerConnection-Call``1-System-String,System-Object[]- 'Sharer.SharerConnection.Call``1(System.String,System.Object[])')
  - [Connect(waitSharerAvailableTimeout,refreshLists)](#M-Sharer-SharerConnection-Connect-System-Int32,System-Boolean- 'Sharer.SharerConnection.Connect(System.Int32,System.Boolean)')
  - [Disconnect()](#M-Sharer-SharerConnection-Disconnect 'Sharer.SharerConnection.Disconnect')
  - [GetInfos()](#M-Sharer-SharerConnection-GetInfos 'Sharer.SharerConnection.GetInfos')
  - [GetSerialPortNames()](#M-Sharer-SharerConnection-GetSerialPortNames 'Sharer.SharerConnection.GetSerialPortNames')
  - [ParseReceivedData(b)](#M-Sharer-SharerConnection-ParseReceivedData-System-Byte- 'Sharer.SharerConnection.ParseReceivedData(System.Byte)')
  - [ReadVariable(name)](#M-Sharer-SharerConnection-ReadVariable-System-String- 'Sharer.SharerConnection.ReadVariable(System.String)')
  - [ReadVariables(variables)](#M-Sharer-SharerConnection-ReadVariables-System-Collections-Generic-IEnumerable{Sharer-Variables-SharerVariable}- 'Sharer.SharerConnection.ReadVariables(System.Collections.Generic.IEnumerable{Sharer.Variables.SharerVariable})')
  - [ReadVariables(names)](#M-Sharer-SharerConnection-ReadVariables-System-Collections-Generic-IEnumerable{System-String}- 'Sharer.SharerConnection.ReadVariables(System.Collections.Generic.IEnumerable{System.String})')
  - [RefreshFunctions()](#M-Sharer-SharerConnection-RefreshFunctions 'Sharer.SharerConnection.RefreshFunctions')
  - [RefreshVariables()](#M-Sharer-SharerConnection-RefreshVariables 'Sharer.SharerConnection.RefreshVariables')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Char[],System-Int32,System-Int32- 'Sharer.SharerConnection.WriteUserData(System.Char[],System.Int32,System.Int32)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-String- 'Sharer.SharerConnection.WriteUserData(System.String)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Single- 'Sharer.SharerConnection.WriteUserData(System.Single)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-UInt64- 'Sharer.SharerConnection.WriteUserData(System.UInt64)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Int64- 'Sharer.SharerConnection.WriteUserData(System.Int64)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-UInt32- 'Sharer.SharerConnection.WriteUserData(System.UInt32)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Int32- 'Sharer.SharerConnection.WriteUserData(System.Int32)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-UInt16- 'Sharer.SharerConnection.WriteUserData(System.UInt16)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Int16- 'Sharer.SharerConnection.WriteUserData(System.Int16)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Byte[],System-Int32,System-Int32- 'Sharer.SharerConnection.WriteUserData(System.Byte[],System.Int32,System.Int32)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Double- 'Sharer.SharerConnection.WriteUserData(System.Double)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Char[]- 'Sharer.SharerConnection.WriteUserData(System.Char[])')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Char- 'Sharer.SharerConnection.WriteUserData(System.Char)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Byte[]- 'Sharer.SharerConnection.WriteUserData(System.Byte[])')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-SByte- 'Sharer.SharerConnection.WriteUserData(System.SByte)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Byte- 'Sharer.SharerConnection.WriteUserData(System.Byte)')
  - [WriteUserData()](#M-Sharer-SharerConnection-WriteUserData-System-Boolean- 'Sharer.SharerConnection.WriteUserData(System.Boolean)')
  - [WriteVariable(value)](#M-Sharer-SharerConnection-WriteVariable-Sharer-Command-SharerWriteValue- 'Sharer.SharerConnection.WriteVariable(Sharer.Command.SharerWriteValue)')
  - [WriteVariable(name,value)](#M-Sharer-SharerConnection-WriteVariable-System-String,System-Object- 'Sharer.SharerConnection.WriteVariable(System.String,System.Object)')
  - [WriteVariables(values)](#M-Sharer-SharerConnection-WriteVariables-System-Collections-Generic-IEnumerable{Sharer-Command-SharerWriteValue}- 'Sharer.SharerConnection.WriteVariables(System.Collections.Generic.IEnumerable{Sharer.Command.SharerWriteValue})')
  - [_currentCommand_Timeouted()](#M-Sharer-SharerConnection-_currentCommand_Timeouted-System-Object,System-EventArgs- 'Sharer.SharerConnection._currentCommand_Timeouted(System.Object,System.EventArgs)')
  - [sendCommand(cmd)](#M-Sharer-SharerConnection-sendCommand-Sharer-Command-SharerSentCommand- 'Sharer.SharerConnection.sendCommand(Sharer.Command.SharerSentCommand)')
  - [serialPort_DataReceived()](#M-Sharer-SharerConnection-serialPort_DataReceived-System-Object,System-IO-Ports-SerialDataReceivedEventArgs- 'Sharer.SharerConnection.serialPort_DataReceived(System.Object,System.IO.Ports.SerialDataReceivedEventArgs)')
- [SharerFunction](#T-Sharer-FunctionCall-SharerFunction 'Sharer.FunctionCall.SharerFunction')
  - [Arguments](#F-Sharer-FunctionCall-SharerFunction-Arguments 'Sharer.FunctionCall.SharerFunction.Arguments')
  - [Name](#F-Sharer-FunctionCall-SharerFunction-Name 'Sharer.FunctionCall.SharerFunction.Name')
  - [ReturnType](#F-Sharer-FunctionCall-SharerFunction-ReturnType 'Sharer.FunctionCall.SharerFunction.ReturnType')
  - [Prototype](#P-Sharer-FunctionCall-SharerFunction-Prototype 'Sharer.FunctionCall.SharerFunction.Prototype')
  - [ToString()](#M-Sharer-FunctionCall-SharerFunction-ToString 'Sharer.FunctionCall.SharerFunction.ToString')
- [SharerFunctionArgument](#T-Sharer-FunctionCall-SharerFunctionArgument 'Sharer.FunctionCall.SharerFunctionArgument')
  - [Name](#F-Sharer-FunctionCall-SharerFunctionArgument-Name 'Sharer.FunctionCall.SharerFunctionArgument.Name')
  - [Type](#F-Sharer-FunctionCall-SharerFunctionArgument-Type 'Sharer.FunctionCall.SharerFunctionArgument.Type')
- [SharerFunctionReturn\`1](#T-Sharer-Command-SharerFunctionReturn`1 'Sharer.Command.SharerFunctionReturn`1')
  - [Status](#F-Sharer-Command-SharerFunctionReturn`1-Status 'Sharer.Command.SharerFunctionReturn`1.Status')
  - [Type](#F-Sharer-Command-SharerFunctionReturn`1-Type 'Sharer.Command.SharerFunctionReturn`1.Type')
  - [Value](#F-Sharer-Command-SharerFunctionReturn`1-Value 'Sharer.Command.SharerFunctionReturn`1.Value')
  - [ToString()](#M-Sharer-Command-SharerFunctionReturn`1-ToString 'Sharer.Command.SharerFunctionReturn`1.ToString')
- [SharerGetAllFunctionsPrototypeCommand](#T-Sharer-Command-SharerGetAllFunctionsPrototypeCommand 'Sharer.Command.SharerGetAllFunctionsPrototypeCommand')
- [SharerGetAllVariablesDefinitionCommand](#T-Sharer-Command-SharerGetAllVariablesDefinitionCommand 'Sharer.Command.SharerGetAllVariablesDefinitionCommand')
- [SharerGetInfosCommand](#T-Sharer-Command-SharerGetInfosCommand 'Sharer.Command.SharerGetInfosCommand')
  - [Board](#F-Sharer-Command-SharerGetInfosCommand-Board 'Sharer.Command.SharerGetInfosCommand.Board')
  - [CPUFrequency](#F-Sharer-Command-SharerGetInfosCommand-CPUFrequency 'Sharer.Command.SharerGetInfosCommand.CPUFrequency')
  - [CPlusPlusVersion](#F-Sharer-Command-SharerGetInfosCommand-CPlusPlusVersion 'Sharer.Command.SharerGetInfosCommand.CPlusPlusVersion')
  - [FixVersion](#F-Sharer-Command-SharerGetInfosCommand-FixVersion 'Sharer.Command.SharerGetInfosCommand.FixVersion')
  - [FunctionCount](#F-Sharer-Command-SharerGetInfosCommand-FunctionCount 'Sharer.Command.SharerGetInfosCommand.FunctionCount')
  - [FunctionMaxCount](#F-Sharer-Command-SharerGetInfosCommand-FunctionMaxCount 'Sharer.Command.SharerGetInfosCommand.FunctionMaxCount')
  - [GCCVersion](#F-Sharer-Command-SharerGetInfosCommand-GCCVersion 'Sharer.Command.SharerGetInfosCommand.GCCVersion')
  - [MajorVersion](#F-Sharer-Command-SharerGetInfosCommand-MajorVersion 'Sharer.Command.SharerGetInfosCommand.MajorVersion')
  - [MinorVersion](#F-Sharer-Command-SharerGetInfosCommand-MinorVersion 'Sharer.Command.SharerGetInfosCommand.MinorVersion')
  - [VariableCount](#F-Sharer-Command-SharerGetInfosCommand-VariableCount 'Sharer.Command.SharerGetInfosCommand.VariableCount')
  - [VariableMaxCount](#F-Sharer-Command-SharerGetInfosCommand-VariableMaxCount 'Sharer.Command.SharerGetInfosCommand.VariableMaxCount')
  - [ToString()](#M-Sharer-Command-SharerGetInfosCommand-ToString 'Sharer.Command.SharerGetInfosCommand.ToString')
- [SharerReadVariableReturn](#T-Sharer-Command-SharerReadVariableReturn 'Sharer.Command.SharerReadVariableReturn')
  - [Status](#F-Sharer-Command-SharerReadVariableReturn-Status 'Sharer.Command.SharerReadVariableReturn.Status')
  - [Value](#F-Sharer-Command-SharerReadVariableReturn-Value 'Sharer.Command.SharerReadVariableReturn.Value')
  - [ToString()](#M-Sharer-Command-SharerReadVariableReturn-ToString 'Sharer.Command.SharerReadVariableReturn.ToString')
- [SharerReadVariableStatus](#T-Sharer-Command-SharerReadVariableStatus 'Sharer.Command.SharerReadVariableStatus')
  - [NotYedRead](#F-Sharer-Command-SharerReadVariableStatus-NotYedRead 'Sharer.Command.SharerReadVariableStatus.NotYedRead')
  - [OK](#F-Sharer-Command-SharerReadVariableStatus-OK 'Sharer.Command.SharerReadVariableStatus.OK')
  - [UnknownType](#F-Sharer-Command-SharerReadVariableStatus-UnknownType 'Sharer.Command.SharerReadVariableStatus.UnknownType')
  - [VariableIdOutOfRange](#F-Sharer-Command-SharerReadVariableStatus-VariableIdOutOfRange 'Sharer.Command.SharerReadVariableStatus.VariableIdOutOfRange')
- [SharerReadVariablesCommand](#T-Sharer-Command-SharerReadVariablesCommand 'Sharer.Command.SharerReadVariablesCommand')
- [SharerSentCommand](#T-Sharer-Command-SharerSentCommand 'Sharer.Command.SharerSentCommand')
  - [Exception](#P-Sharer-Command-SharerSentCommand-Exception 'Sharer.Command.SharerSentCommand.Exception')
  - [WaitAnswer(timeout)](#M-Sharer-Command-SharerSentCommand-WaitAnswer-System-TimeSpan- 'Sharer.Command.SharerSentCommand.WaitAnswer(System.TimeSpan)')
- [SharerType](#T-Sharer-FunctionCall-SharerType 'Sharer.FunctionCall.SharerType')
  - [bool](#F-Sharer-FunctionCall-SharerType-bool 'Sharer.FunctionCall.SharerType.bool')
  - [byte](#F-Sharer-FunctionCall-SharerType-byte 'Sharer.FunctionCall.SharerType.byte')
  - [double](#F-Sharer-FunctionCall-SharerType-double 'Sharer.FunctionCall.SharerType.double')
  - [float](#F-Sharer-FunctionCall-SharerType-float 'Sharer.FunctionCall.SharerType.float')
  - [int](#F-Sharer-FunctionCall-SharerType-int 'Sharer.FunctionCall.SharerType.int')
  - [int64](#F-Sharer-FunctionCall-SharerType-int64 'Sharer.FunctionCall.SharerType.int64')
  - [long](#F-Sharer-FunctionCall-SharerType-long 'Sharer.FunctionCall.SharerType.long')
  - [short](#F-Sharer-FunctionCall-SharerType-short 'Sharer.FunctionCall.SharerType.short')
  - [uint](#F-Sharer-FunctionCall-SharerType-uint 'Sharer.FunctionCall.SharerType.uint')
  - [uint64](#F-Sharer-FunctionCall-SharerType-uint64 'Sharer.FunctionCall.SharerType.uint64')
  - [ulong](#F-Sharer-FunctionCall-SharerType-ulong 'Sharer.FunctionCall.SharerType.ulong')
  - [void](#F-Sharer-FunctionCall-SharerType-void 'Sharer.FunctionCall.SharerType.void')
- [SharerTypeHelper](#T-Sharer-FunctionCall-SharerTypeHelper 'Sharer.FunctionCall.SharerTypeHelper')
- [SharerVariable](#T-Sharer-Variables-SharerVariable 'Sharer.Variables.SharerVariable')
  - [Name](#F-Sharer-Variables-SharerVariable-Name 'Sharer.Variables.SharerVariable.Name')
  - [Type](#F-Sharer-Variables-SharerVariable-Type 'Sharer.Variables.SharerVariable.Type')
- [SharerWriteValue](#T-Sharer-Command-SharerWriteValue 'Sharer.Command.SharerWriteValue')
  - [#ctor(name,value)](#M-Sharer-Command-SharerWriteValue-#ctor-System-String,System-Object- 'Sharer.Command.SharerWriteValue.#ctor(System.String,System.Object)')
  - [#ctor(variable,value)](#M-Sharer-Command-SharerWriteValue-#ctor-Sharer-Variables-SharerVariable,System-Object- 'Sharer.Command.SharerWriteValue.#ctor(Sharer.Variables.SharerVariable,System.Object)')
  - [Name](#P-Sharer-Command-SharerWriteValue-Name 'Sharer.Command.SharerWriteValue.Name')
  - [Status](#P-Sharer-Command-SharerWriteValue-Status 'Sharer.Command.SharerWriteValue.Status')
  - [Value](#P-Sharer-Command-SharerWriteValue-Value 'Sharer.Command.SharerWriteValue.Value')
- [SharerWriteVariableReturn](#T-Sharer-Command-SharerWriteVariableReturn 'Sharer.Command.SharerWriteVariableReturn')
  - [Status](#F-Sharer-Command-SharerWriteVariableReturn-Status 'Sharer.Command.SharerWriteVariableReturn.Status')
  - [Value](#F-Sharer-Command-SharerWriteVariableReturn-Value 'Sharer.Command.SharerWriteVariableReturn.Value')
  - [ToString()](#M-Sharer-Command-SharerWriteVariableReturn-ToString 'Sharer.Command.SharerWriteVariableReturn.ToString')
- [SharerWriteVariableStatus](#T-Sharer-Command-SharerWriteVariableStatus 'Sharer.Command.SharerWriteVariableStatus')
  - [NotFound](#F-Sharer-Command-SharerWriteVariableStatus-NotFound 'Sharer.Command.SharerWriteVariableStatus.NotFound')
  - [NotYetWritten](#F-Sharer-Command-SharerWriteVariableStatus-NotYetWritten 'Sharer.Command.SharerWriteVariableStatus.NotYetWritten')
  - [OK](#F-Sharer-Command-SharerWriteVariableStatus-OK 'Sharer.Command.SharerWriteVariableStatus.OK')
  - [UnknownType](#F-Sharer-Command-SharerWriteVariableStatus-UnknownType 'Sharer.Command.SharerWriteVariableStatus.UnknownType')
  - [VariableIdOutOfRange](#F-Sharer-Command-SharerWriteVariableStatus-VariableIdOutOfRange 'Sharer.Command.SharerWriteVariableStatus.VariableIdOutOfRange')
- [Steps](#T-Sharer-Command-SharerGetAllVariablesDefinitionCommand-Steps 'Sharer.Command.SharerGetAllVariablesDefinitionCommand.Steps')
- [UserDataReceivedEventArgs](#T-Sharer-UserData-UserDataReceivedEventArgs 'Sharer.UserData.UserDataReceivedEventArgs')
  - [Data](#P-Sharer-UserData-UserDataReceivedEventArgs-Data 'Sharer.UserData.UserDataReceivedEventArgs.Data')
  - [GetReader()](#M-Sharer-UserData-UserDataReceivedEventArgs-GetReader 'Sharer.UserData.UserDataReceivedEventArgs.GetReader')

<a name='T-Sharer-UserData-UserDataReceivedEventArgs-InternalBinaryReader'></a>
## InternalBinaryReader `type`

##### Namespace

Sharer.UserData.UserDataReceivedEventArgs

##### Summary

A custom reader to properly read strings

<a name='T-Sharer-SharerConnection-ReceiveSteps'></a>
## ReceiveSteps `type`

##### Namespace

Sharer.SharerConnection

##### Summary

internal states of the state machine

<a name='T-Sharer-Command-SharerCallFunctionCommand`1'></a>
## SharerCallFunctionCommand\`1 `type`

##### Namespace

Sharer.Command

##### Summary

Command that allow to encode and decode a function call

##### Generic Types

| Name | Description |
| ---- | ----------- |
| ReturnType | Expected .NET type returned by the function |

<a name='T-Sharer-Command-SharerCallFunctionStatus'></a>
## SharerCallFunctionStatus `type`

##### Namespace

Sharer.Command

##### Summary

Status of the remote call

<a name='F-Sharer-Command-SharerCallFunctionStatus-FunctionIdOutOfRange'></a>
### FunctionIdOutOfRange `constants`

##### Summary

The id of the function is out of the range of function array on Arduino

<a name='F-Sharer-Command-SharerCallFunctionStatus-NotYetCalled'></a>
### NotYetCalled `constants`

##### Summary

The function has not been yet called

<a name='F-Sharer-Command-SharerCallFunctionStatus-OK'></a>
### OK `constants`

##### Summary

The function has been successfully camled

<a name='F-Sharer-Command-SharerCallFunctionStatus-UnknownType'></a>
### UnknownType `constants`

##### Summary

Returned type ou argument type is unknown, please check version of Sharer

<a name='T-Sharer-Command-SharerCommandID'></a>
## SharerCommandID `type`

##### Namespace

Sharer.Command

##### Summary

List of all commands available in Sharer

<a name='T-Sharer-SharerConnection'></a>
## SharerConnection `type`

##### Namespace

Sharer

##### Summary

Sharer connection to an Arduino Board. Sharer offers features to remote call function and read/write variables on Arduino from a .NET assembly.

<a name='M-Sharer-SharerConnection-#ctor-System-String,System-Int32,System-IO-Ports-Parity,System-Int32,System-IO-Ports-StopBits,System-IO-Ports-Handshake-'></a>
### #ctor(portName,baudRate,parity,dataBit,stopBits,handShake) `constructor`

##### Summary

Create a new Sharer connection. You should then call Connect() open COM Port

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| portName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Name of the Arduino COM port (example "COM5") |
| baudRate | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Serial communication baud rate. Should be the same in Arduino code Sharer.init(baudRate). Example : 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 31250, 38400, 57600, or 115200 |
| parity | [System.IO.Ports.Parity](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IO.Ports.Parity 'System.IO.Ports.Parity') | Optional: bit parity (default: Parity.None) |
| dataBit | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Optional: Number of bits per byte (default: 8) |
| stopBits | [System.IO.Ports.StopBits](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IO.Ports.StopBits 'System.IO.Ports.StopBits') | Optional: The serial stop bit (default: StopBits.One) |
| handShake | [System.IO.Ports.Handshake](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.IO.Ports.Handshake 'System.IO.Ports.Handshake') | Optional: Hand Shake of the serial communication (default: Handshake.None) |

<a name='F-Sharer-SharerConnection-DEFAULT_TIMEOUT'></a>
### DEFAULT_TIMEOUT `constants`

##### Summary

Default timeout for Sharer commands

<a name='F-Sharer-SharerConnection-Functions'></a>
### Functions `constants`

##### Summary

List of shared functions. This list is refresh after a call to RefreshFunctions() or Connect()

<a name='F-Sharer-SharerConnection-SHARER_START_COMMAND_CHAR'></a>
### SHARER_START_COMMAND_CHAR `constants`

##### Summary

Every sharer commands starts with this byte

<a name='F-Sharer-SharerConnection-Variables'></a>
### Variables `constants`

##### Summary

List of shared variables. This list is refresh after a call to RefreshVariables() or Connect()

<a name='F-Sharer-SharerConnection-_currentCommand'></a>
### _currentCommand `constants`

##### Summary

The sent command associated to the last received answer

<a name='F-Sharer-SharerConnection-_lastCommandID'></a>
### _lastCommandID `constants`

##### Summary

Last command received

<a name='F-Sharer-SharerConnection-_lastDeviceMessageId'></a>
### _lastDeviceMessageId `constants`

##### Summary

Last received message id

<a name='F-Sharer-SharerConnection-_receiveStep'></a>
### _receiveStep `constants`

##### Summary

Current state

<a name='F-Sharer-SharerConnection-_serialPort'></a>
### _serialPort `constants`

##### Summary

The serial port object

<a name='F-Sharer-SharerConnection-_userData'></a>
### _userData `constants`

##### Summary

User message bytes received

<a name='P-Sharer-SharerConnection-Connected'></a>
### Connected `property`

##### Summary

Indicates if the serial communication is opened

<a name='M-Sharer-SharerConnection-AssertConnected'></a>
### AssertConnected() `method`

##### Summary

Throw an exception if the connection is closed

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-Call-System-String,System-TimeSpan,System-Object[]-'></a>
### Call() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-Call-System-String,System-Object[]-'></a>
### Call() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-Call``1-System-String,System-TimeSpan,System-Object[]-'></a>
### Call\`\`1(functionName,timeout,arguments) `method`

##### Summary

Remote call a function by its name with arguments and get the returned value

##### Returns

Status of the function call with its returned value

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| functionName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Name of the function to call |
| timeout | [System.TimeSpan](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.TimeSpan 'System.TimeSpan') | Maximum expected duration of the function execution on Arduino |
| arguments | [System.Object[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object[] 'System.Object[]') | Optional list of argument values. |

##### Generic Types

| Name | Description |
| ---- | ----------- |
| ReturnType | The .NET type expected. Should be the same as the Arduino function return type |

<a name='M-Sharer-SharerConnection-Call``1-System-String,System-Object[]-'></a>
### Call\`\`1() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-Connect-System-Int32,System-Boolean-'></a>
### Connect(waitSharerAvailableTimeout,refreshLists) `method`

##### Summary

Connect to Arduino board

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| waitSharerAvailableTimeout | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Optional: Maximum time in milliseconds to initiate a communication with the sharer library that runs on Arduino (0 not to wait for Sharer to answer) |
| refreshLists | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') | Optional: Indicates if the variables and function lists should be refreshed |

<a name='M-Sharer-SharerConnection-Disconnect'></a>
### Disconnect() `method`

##### Summary

Disconnect Sharer and close serial communication

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-GetInfos'></a>
### GetInfos() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-GetSerialPortNames'></a>
### GetSerialPortNames() `method`

##### Summary

Static function that returns the list of all available COM port

##### Returns

A string array that contains all serial COM port name you can use in SharerConnection constructor

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-ParseReceivedData-System-Byte-'></a>
### ParseReceivedData(b) `method`

##### Summary

Parse received bytes one by one to make the internal state machine progress and fill the user message buffer

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| b | [System.Byte](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Byte 'System.Byte') | The last byte received |

<a name='M-Sharer-SharerConnection-ReadVariable-System-String-'></a>
### ReadVariable(name) `method`

##### Summary

Read a variable by its name

##### Returns

Variable value and read status

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Name of the variable to read |

<a name='M-Sharer-SharerConnection-ReadVariables-System-Collections-Generic-IEnumerable{Sharer-Variables-SharerVariable}-'></a>
### ReadVariables(variables) `method`

##### Summary

Read simultaneously several variables in Arduino

##### Returns

Variable values and read status

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| variables | [System.Collections.Generic.IEnumerable{Sharer.Variables.SharerVariable}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{Sharer.Variables.SharerVariable}') | Variable to read. Peek them in Variables field |

<a name='M-Sharer-SharerConnection-ReadVariables-System-Collections-Generic-IEnumerable{System-String}-'></a>
### ReadVariables(names) `method`

##### Summary

Read simultaneously several variables in Arduino

##### Returns

Variable values and read status

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| names | [System.Collections.Generic.IEnumerable{System.String}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{System.String}') | Names of variable to read |

<a name='M-Sharer-SharerConnection-RefreshFunctions'></a>
### RefreshFunctions() `method`

##### Summary

Refresh functions shared

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-RefreshVariables'></a>
### RefreshVariables() `method`

##### Summary

Refresh variables shared

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Char[],System-Int32,System-Int32-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-String-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Single-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-UInt64-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Int64-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-UInt32-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Int32-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-UInt16-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Int16-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Byte[],System-Int32,System-Int32-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Double-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Char[]-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Char-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Byte[]-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-SByte-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Byte-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteUserData-System-Boolean-'></a>
### WriteUserData() `method`

##### Summary

Send a custom user message to Arduino. The message can be read in Arduino code with Sharer.read()

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-WriteVariable-Sharer-Command-SharerWriteValue-'></a>
### WriteVariable(value) `method`

##### Summary

Write an Arduino variable

##### Returns

True if all variable has been succesfully written

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| value | [Sharer.Command.SharerWriteValue](#T-Sharer-Command-SharerWriteValue 'Sharer.Command.SharerWriteValue') | Value to write |

<a name='M-Sharer-SharerConnection-WriteVariable-System-String,System-Object-'></a>
### WriteVariable(name,value) `method`

##### Summary

Write an Arduino variable

##### Returns

True if all variable has been succesfully written

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Variable name to write |
| value | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | New value to write |

<a name='M-Sharer-SharerConnection-WriteVariables-System-Collections-Generic-IEnumerable{Sharer-Command-SharerWriteValue}-'></a>
### WriteVariables(values) `method`

##### Summary

Write variables on Arduino

##### Returns

True if all variable has been succesfully written

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| values | [System.Collections.Generic.IEnumerable{Sharer.Command.SharerWriteValue}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.IEnumerable 'System.Collections.Generic.IEnumerable{Sharer.Command.SharerWriteValue}') | List of values to simultaneously write |

<a name='M-Sharer-SharerConnection-_currentCommand_Timeouted-System-Object,System-EventArgs-'></a>
### _currentCommand_Timeouted() `method`

##### Summary

Handler called when the current command has timeouted. Allow to reset and unlock the state machine

##### Parameters

This method has no parameters.

<a name='M-Sharer-SharerConnection-sendCommand-Sharer-Command-SharerSentCommand-'></a>
### sendCommand(cmd) `method`

##### Summary

Convert a command to a byte array and send it on serial port

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| cmd | [Sharer.Command.SharerSentCommand](#T-Sharer-Command-SharerSentCommand 'Sharer.Command.SharerSentCommand') | The command to send |

<a name='M-Sharer-SharerConnection-serialPort_DataReceived-System-Object,System-IO-Ports-SerialDataReceivedEventArgs-'></a>
### serialPort_DataReceived() `method`

##### Summary

Event handler called when serial data are received

##### Parameters

This method has no parameters.

<a name='T-Sharer-FunctionCall-SharerFunction'></a>
## SharerFunction `type`

##### Namespace

Sharer.FunctionCall

##### Summary

Description of a function shared with Sharer

<a name='F-Sharer-FunctionCall-SharerFunction-Arguments'></a>
### Arguments `constants`

##### Summary

Arguments of the function

<a name='F-Sharer-FunctionCall-SharerFunction-Name'></a>
### Name `constants`

##### Summary

Function name

<a name='F-Sharer-FunctionCall-SharerFunction-ReturnType'></a>
### ReturnType `constants`

##### Summary

Return type of the function

<a name='P-Sharer-FunctionCall-SharerFunction-Prototype'></a>
### Prototype `property`

##### Summary

Get the prototype of the function. For example: int myFunction(int arg1, bool arg2)

<a name='M-Sharer-FunctionCall-SharerFunction-ToString'></a>
### ToString() `method`

##### Summary

Human readable string

##### Parameters

This method has no parameters.

<a name='T-Sharer-FunctionCall-SharerFunctionArgument'></a>
## SharerFunctionArgument `type`

##### Namespace

Sharer.FunctionCall

##### Summary

Description of a shared function argument

<a name='F-Sharer-FunctionCall-SharerFunctionArgument-Name'></a>
### Name `constants`

##### Summary

Argument name

<a name='F-Sharer-FunctionCall-SharerFunctionArgument-Type'></a>
### Type `constants`

##### Summary

Argument type

<a name='T-Sharer-Command-SharerFunctionReturn`1'></a>
## SharerFunctionReturn\`1 `type`

##### Namespace

Sharer.Command

##### Summary

Status of the function call

##### Generic Types

| Name | Description |
| ---- | ----------- |
| ReturnType | Expected .NET type returned by the function |

<a name='F-Sharer-Command-SharerFunctionReturn`1-Status'></a>
### Status `constants`

##### Summary

Status of the success of  function call

<a name='F-Sharer-Command-SharerFunctionReturn`1-Type'></a>
### Type `constants`

##### Summary

Type of the returned value

<a name='F-Sharer-Command-SharerFunctionReturn`1-Value'></a>
### Value `constants`

##### Summary

Returned value converted in .NET type

<a name='M-Sharer-Command-SharerFunctionReturn`1-ToString'></a>
### ToString() `method`

##### Summary

A human readable string

##### Returns



##### Parameters

This method has no parameters.

<a name='T-Sharer-Command-SharerGetAllFunctionsPrototypeCommand'></a>
## SharerGetAllFunctionsPrototypeCommand `type`

##### Namespace

Sharer.Command

##### Summary

Command that allows to encode and decode the list of all function shared

<a name='T-Sharer-Command-SharerGetAllVariablesDefinitionCommand'></a>
## SharerGetAllVariablesDefinitionCommand `type`

##### Namespace

Sharer.Command

##### Summary

Command that allows to encode and decode the list of all variables shared

<a name='T-Sharer-Command-SharerGetInfosCommand'></a>
## SharerGetInfosCommand `type`

##### Namespace

Sharer.Command

##### Summary

Information about the board. This command is also sent at startup

<a name='F-Sharer-Command-SharerGetInfosCommand-Board'></a>
### Board `constants`

##### Summary

Arduino board name

<a name='F-Sharer-Command-SharerGetInfosCommand-CPUFrequency'></a>
### CPUFrequency `constants`

##### Summary

Arduino CPU frequency in Hz

<a name='F-Sharer-Command-SharerGetInfosCommand-CPlusPlusVersion'></a>
### CPlusPlusVersion `constants`

##### Summary

C++ version used to compile the Arduino sketch

<a name='F-Sharer-Command-SharerGetInfosCommand-FixVersion'></a>
### FixVersion `constants`

##### Summary

Arduino Sharer library fix version

<a name='F-Sharer-Command-SharerGetInfosCommand-FunctionCount'></a>
### FunctionCount `constants`

##### Summary

Number of functions exposed by Sharer

<a name='F-Sharer-Command-SharerGetInfosCommand-FunctionMaxCount'></a>
### FunctionMaxCount `constants`

##### Summary

Maximum number of function that Sharer can share (see SharerConfig.h)

<a name='F-Sharer-Command-SharerGetInfosCommand-GCCVersion'></a>
### GCCVersion `constants`

##### Summary

GCC compiler version used to compile the Arduino sketch

<a name='F-Sharer-Command-SharerGetInfosCommand-MajorVersion'></a>
### MajorVersion `constants`

##### Summary

Arduino Sharer library major version

<a name='F-Sharer-Command-SharerGetInfosCommand-MinorVersion'></a>
### MinorVersion `constants`

##### Summary

Arduino Sharer library minor version

<a name='F-Sharer-Command-SharerGetInfosCommand-VariableCount'></a>
### VariableCount `constants`

##### Summary

Number of variables exposed by Sharer

<a name='F-Sharer-Command-SharerGetInfosCommand-VariableMaxCount'></a>
### VariableMaxCount `constants`

##### Summary

Maximum number of variable that Sharer can share (see SharerConfig.h)

<a name='M-Sharer-Command-SharerGetInfosCommand-ToString'></a>
### ToString() `method`

##### Summary

A human readable string

##### Parameters

This method has no parameters.

<a name='T-Sharer-Command-SharerReadVariableReturn'></a>
## SharerReadVariableReturn `type`

##### Namespace

Sharer.Command

##### Summary

Variable value and status of the reading

<a name='F-Sharer-Command-SharerReadVariableReturn-Status'></a>
### Status `constants`

##### Summary

Status of the reading

<a name='F-Sharer-Command-SharerReadVariableReturn-Value'></a>
### Value `constants`

##### Summary

Value of the variable

<a name='M-Sharer-Command-SharerReadVariableReturn-ToString'></a>
### ToString() `method`

##### Summary

A human readable string

##### Parameters

This method has no parameters.

<a name='T-Sharer-Command-SharerReadVariableStatus'></a>
## SharerReadVariableStatus `type`

##### Namespace

Sharer.Command

##### Summary

Status of the reading

<a name='F-Sharer-Command-SharerReadVariableStatus-NotYedRead'></a>
### NotYedRead `constants`

##### Summary

The variable has not been yet read

<a name='F-Sharer-Command-SharerReadVariableStatus-OK'></a>
### OK `constants`

##### Summary

The variable has been successfully read

<a name='F-Sharer-Command-SharerReadVariableStatus-UnknownType'></a>
### UnknownType `constants`

##### Summary

Variable type is unknown by the board, please check Sharer version

<a name='F-Sharer-Command-SharerReadVariableStatus-VariableIdOutOfRange'></a>
### VariableIdOutOfRange `constants`

##### Summary

Id of the variable is out of range of the Arduino variable array

<a name='T-Sharer-Command-SharerReadVariablesCommand'></a>
## SharerReadVariablesCommand `type`

##### Namespace

Sharer.Command

##### Summary

Sharer command that encode/decode the reading of a variable

<a name='T-Sharer-Command-SharerSentCommand'></a>
## SharerSentCommand `type`

##### Namespace

Sharer.Command

##### Summary

Abstract class to describe a Sharer command, encode and decode it

<a name='P-Sharer-Command-SharerSentCommand-Exception'></a>
### Exception `property`

##### Summary

Internal exception thrown during command execution

<a name='M-Sharer-Command-SharerSentCommand-WaitAnswer-System-TimeSpan-'></a>
### WaitAnswer(timeout) `method`

##### Summary

Wait command to finish call and answer

##### Returns

True if success

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| timeout | [System.TimeSpan](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.TimeSpan 'System.TimeSpan') | Maximum blocking time |

<a name='T-Sharer-FunctionCall-SharerType'></a>
## SharerType `type`

##### Namespace

Sharer.FunctionCall

##### Summary

List of all types supported by Sharer for variables and function argument and return

<a name='F-Sharer-FunctionCall-SharerType-bool'></a>
### bool `constants`

##### Summary

Boolean encoded in a byte (0x00=false, else true)

<a name='F-Sharer-FunctionCall-SharerType-byte'></a>
### byte `constants`

##### Summary

Unsigned 8 bits integer

<a name='F-Sharer-FunctionCall-SharerType-double'></a>
### double `constants`

##### Summary

64 bits float

<a name='F-Sharer-FunctionCall-SharerType-float'></a>
### float `constants`

##### Summary

32 bits float

<a name='F-Sharer-FunctionCall-SharerType-int'></a>
### int `constants`

##### Summary

Signed 16 bits integer

<a name='F-Sharer-FunctionCall-SharerType-int64'></a>
### int64 `constants`

##### Summary

Signed 64 bits integer

<a name='F-Sharer-FunctionCall-SharerType-long'></a>
### long `constants`

##### Summary

Signed 32 bits integer

<a name='F-Sharer-FunctionCall-SharerType-short'></a>
### short `constants`

##### Summary

Signed 8 bits integer

<a name='F-Sharer-FunctionCall-SharerType-uint'></a>
### uint `constants`

##### Summary

Unsigned 16 bits integer

<a name='F-Sharer-FunctionCall-SharerType-uint64'></a>
### uint64 `constants`

##### Summary

unsigned 64 bits integer

<a name='F-Sharer-FunctionCall-SharerType-ulong'></a>
### ulong `constants`

##### Summary

Unsigned 32 bits integer

<a name='F-Sharer-FunctionCall-SharerType-void'></a>
### void `constants`

##### Summary

No type returned

<a name='T-Sharer-FunctionCall-SharerTypeHelper'></a>
## SharerTypeHelper `type`

##### Namespace

Sharer.FunctionCall

##### Summary

Static function to hel data manipulation

<a name='T-Sharer-Variables-SharerVariable'></a>
## SharerVariable `type`

##### Namespace

Sharer.Variables

##### Summary

Describes a Shared variable that can be read and written by Sharer

<a name='F-Sharer-Variables-SharerVariable-Name'></a>
### Name `constants`

##### Summary

Variable name

<a name='F-Sharer-Variables-SharerVariable-Type'></a>
### Type `constants`

##### Summary

Variable type

<a name='T-Sharer-Command-SharerWriteValue'></a>
## SharerWriteValue `type`

##### Namespace

Sharer.Command

##### Summary

Variable to write

<a name='M-Sharer-Command-SharerWriteValue-#ctor-System-String,System-Object-'></a>
### #ctor(name,value) `constructor`

##### Summary

Create a command to write a variable on Arduino

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Name of the variable to write |
| value | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | Value to write |

<a name='M-Sharer-Command-SharerWriteValue-#ctor-Sharer-Variables-SharerVariable,System-Object-'></a>
### #ctor(variable,value) `constructor`

##### Summary

Create a command to write a variable on Arduino

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| variable | [Sharer.Variables.SharerVariable](#T-Sharer-Variables-SharerVariable 'Sharer.Variables.SharerVariable') | Variable to write |
| value | [System.Object](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Object 'System.Object') | Value to write |

<a name='P-Sharer-Command-SharerWriteValue-Name'></a>
### Name `property`

##### Summary

Name of the variable to write

<a name='P-Sharer-Command-SharerWriteValue-Status'></a>
### Status `property`

##### Summary

Status of the writting

<a name='P-Sharer-Command-SharerWriteValue-Value'></a>
### Value `property`

##### Summary

Value of the variable to write

<a name='T-Sharer-Command-SharerWriteVariableReturn'></a>
## SharerWriteVariableReturn `type`

##### Namespace

Sharer.Command

##### Summary

Status of the variable writting

<a name='F-Sharer-Command-SharerWriteVariableReturn-Status'></a>
### Status `constants`

##### Summary

Status of the writting

<a name='F-Sharer-Command-SharerWriteVariableReturn-Value'></a>
### Value `constants`

##### Summary

Value written

<a name='M-Sharer-Command-SharerWriteVariableReturn-ToString'></a>
### ToString() `method`

##### Summary

A human readable string

##### Returns



##### Parameters

This method has no parameters.

<a name='T-Sharer-Command-SharerWriteVariableStatus'></a>
## SharerWriteVariableStatus `type`

##### Namespace

Sharer.Command

##### Summary

Status of the variable writting

<a name='F-Sharer-Command-SharerWriteVariableStatus-NotFound'></a>
### NotFound `constants`

##### Summary

The variable to write has not been found in variable list

<a name='F-Sharer-Command-SharerWriteVariableStatus-NotYetWritten'></a>
### NotYetWritten `constants`

##### Summary

The variable has not been yet written

<a name='F-Sharer-Command-SharerWriteVariableStatus-OK'></a>
### OK `constants`

##### Summary

The variable has successfullt been written

<a name='F-Sharer-Command-SharerWriteVariableStatus-UnknownType'></a>
### UnknownType `constants`

##### Summary

Variable type is unknown by the board, please check Sharer version

<a name='F-Sharer-Command-SharerWriteVariableStatus-VariableIdOutOfRange'></a>
### VariableIdOutOfRange `constants`

##### Summary

Id of the variable is out of range of the Arduino variable array

<a name='T-Sharer-Command-SharerGetAllVariablesDefinitionCommand-Steps'></a>
## Steps `type`

##### Namespace

Sharer.Command.SharerGetAllVariablesDefinitionCommand

##### Summary

internal state machine for decoding the list

<a name='T-Sharer-UserData-UserDataReceivedEventArgs'></a>
## UserDataReceivedEventArgs `type`

##### Namespace

Sharer.UserData

##### Summary

Describes a received custom user message

<a name='P-Sharer-UserData-UserDataReceivedEventArgs-Data'></a>
### Data `property`

##### Summary

Received raw byte data

<a name='M-Sharer-UserData-UserDataReceivedEventArgs-GetReader'></a>
### GetReader() `method`

##### Summary

Returns a reader to facilitate data decoding (string, int, double, ...)

##### Parameters

This method has no parameters.
