using Sharer.FunctionCall;
using Sharer.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharer.Command
{
    public enum SharerReadVariableStatus : byte
    {
        UnknownStatus = 0xff,
        OK = 0,
        VariableIdOutOfRange,
        UnknownType,
    }

    public class SharerReadVariableReturn
    {
        public SharerReadVariableStatus Status = SharerReadVariableStatus.UnknownStatus;
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



    class SharerReadVariablesCommand : SharerSentCommand
    {
        private byte[] _buffer;
        private SharerType[] _types;

        public SharerReadVariablesCommand(byte[] buffer, SharerType[] types)
        {
            _buffer = buffer;
            _types = types;
            Values =  new List<SharerReadVariableReturn>(types.Length);
        }

        public override SharerCommandID CommandID
        {
            get
            {
                return SharerCommandID.ReadVariables;
            }
        }

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
