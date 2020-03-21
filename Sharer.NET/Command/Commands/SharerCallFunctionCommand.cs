using Sharer.FunctionCall;
using System.Collections.Generic;

namespace Sharer.Command
{
    /// <summary>
    /// Status of the remote call
    /// </summary>
    public enum SharerCallFunctionStatus : byte
    {
        /// <summary>
        /// The function has not been yet called
        /// </summary>
        NotYetCalled = 0xff,

        /// <summary>
        /// The function has been successfully camled
        /// </summary>
        OK = 0,

        /// <summary>
        /// The id of the function is out of the range of function array on Arduino
        /// </summary>
        FunctionIdOutOfRange,

        /// <summary>
        /// Returned type ou argument type is unknown, please check version of Sharer
        /// </summary>
        UnknownType,
    }

    /// <summary>
    /// Status of the function call
    /// </summary>
    /// <typeparam name="ReturnType">Expected .NET type returned by the function</typeparam>
    public class SharerFunctionReturn<ReturnType>
    {
        /// <summary>
        /// Status of the success of  function call
        /// </summary>
        public SharerCallFunctionStatus Status = SharerCallFunctionStatus.NotYetCalled;

        /// <summary>
        /// Type of the returned value
        /// </summary>
        public SharerType Type;

        /// <summary>
        /// Returned value converted in .NET type
        /// </summary>
        public ReturnType Value;

         /// <summary>
         /// A human readable string
         /// </summary>
         /// <returns></returns>
        public override string ToString()
        {
            if (Status == SharerCallFunctionStatus.OK)
            {
                if (Value == null) return "OK";
                else return Value.ToString();
            }
            else
            {
                return Status.ToString();
            }
        }
    }

    /// <summary>
    /// Command that allow to encode and decode a function call
    /// </summary>
    /// <typeparam name="ReturnType">Expected .NET type returned by the function</typeparam>
    internal class SharerCallFunctionCommand<ReturnType> : SharerSentCommand
    {
        private byte[] _buffer;
        SharerType _returnType;

        public SharerCallFunctionCommand(byte[] buffer, SharerType returnType)
        {
            _buffer = buffer;
            _returnType = returnType;
            Return.Type = returnType;
        }

        internal override SharerCommandID CommandID => SharerCommandID.CallFunction;

        internal override byte[] ArgumentsToSend()
        {
            return _buffer;
        }

        private enum Steps
        {
            Status,
            ReturnValue,
            End
        }

        // reception step
        private Steps _receivedStep = Steps.Status;

        public SharerFunctionReturn<ReturnType> Return = new SharerFunctionReturn<ReturnType>();

        private List<byte> _returnBytes = new List<byte>();

        private int _returnSize;

        internal override bool DecodeArgument(byte b)
        {
            switch (_receivedStep)
            {
                case Steps.Status: // receive status
                    Return.Status = (SharerCallFunctionStatus)b;

                    // if the function has a returned value
                    if(Return.Status == SharerCallFunctionStatus.OK && _returnType != SharerType.@void)
                    {
                        _returnBytes.Clear();
                        _returnSize = SharerTypeHelper.Sizeof(_returnType);
                        _receivedStep = Steps.ReturnValue;
                    }
                    else
                    {
                        _receivedStep = Steps.End;
                    }

                    break;
                case Steps.ReturnValue:
                    _returnBytes.Add(b); // add received byte

                    // if enought returned byte, decode it and end
                    if(_returnBytes.Count>= _returnSize)
                    {
                        Return.Value = (ReturnType)SharerTypeHelper.Decode(_returnType, _returnBytes.ToArray());

                        _receivedStep = Steps.End;
                    }
                    break;
            }

            return _receivedStep == Steps.End;
        }
    }
}
