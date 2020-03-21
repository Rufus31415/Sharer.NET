using Sharer.FunctionCall;
using System.Collections.Generic;

namespace Sharer.Command
{
    /// <summary>
    /// Status of the reading
    /// </summary>
    public enum SharerReadVariableStatus : byte
    {
        NotYedRead = 0xff,
        OK = 0,
        VariableIdOutOfRange,
        UnknownType,
    }

    /// <summary>
    /// Variable value and status of the reading
    /// </summary>
    public class SharerReadVariableReturn
    {
        /// <summary>
        /// Status of the reading
        /// </summary>
        public SharerReadVariableStatus Status = SharerReadVariableStatus.NotYedRead;

        /// <summary>
        /// Value of the variable
        /// </summary>
        public object Value;

        public override string ToString()
        {
            if (Status == SharerReadVariableStatus.OK)
            {
                if (Value == null) return "";
                else return Value.ToString();
            }
            else
            {
                return Status.ToString();
            }
        }
    }


    /// <summary>
    /// Sharer command that encode/decode the reading of a variable
    /// </summary>
    internal class SharerReadVariablesCommand : SharerSentCommand
    {
        private byte[] _buffer;
        private SharerType[] _types;

        public SharerReadVariablesCommand(byte[] buffer, SharerType[] types)
        {
            _buffer = buffer;
            _types = types;
            Values =  new List<SharerReadVariableReturn>(types.Length);
        }

        internal override SharerCommandID CommandID => SharerCommandID.ReadVariables;

        internal override byte[] ArgumentsToSend()
        {
            return _buffer;
        }

        private enum Steps
        {
            Status,
            Value,
            End
        }

        // reception step
        private Steps _receivedStep = Steps.Status;

        public List<SharerReadVariableReturn> Values ;

        private SharerReadVariableReturn _lastValue;

        private int _lastValueSize;
        private SharerType _lastValueType;

        private List<byte> _lastValueBytes =new List<byte>(16);
        
        internal override bool DecodeArgument(byte b)
        {
            switch (_receivedStep)
            {
                case Steps.Status: // receive status
                    _lastValue = new SharerReadVariableReturn();
                    Values.Add(_lastValue);

                    _lastValue.Status = (SharerReadVariableStatus)b;

                    // if the we have a value, go to next step to decode, else stay to have next variable
                    if (_lastValue.Status == SharerReadVariableStatus.OK)
                    {
                        _lastValueType = _types[Values.Count - 1];
                        _lastValueSize = SharerTypeHelper.Sizeof(_lastValueType);
                        _lastValueBytes.Clear();
                        _receivedStep = Steps.Value;
                    }
                    else if (Values.Count >= _types.Length)
                    {
                        _receivedStep = Steps.End;
                    }

                    break;
                case Steps.Value:
                    _lastValueBytes.Add(b); // add received byte

                    // if enought returned byte, decode it 
                    if (_lastValueBytes.Count >= _lastValueSize)
                    {
                        _lastValue.Value = SharerTypeHelper.Decode(_lastValueType, _lastValueBytes.ToArray());

                        if (Values.Count >= _types.Length)
                        {
                            _receivedStep = Steps.End;
                        }
                        else
                        {
                            _receivedStep = Steps.Status;
                        }
                    }
                    break;
            }

            return _receivedStep == Steps.End;
        }
    }
}
