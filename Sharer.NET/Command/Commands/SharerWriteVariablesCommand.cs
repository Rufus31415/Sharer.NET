using Sharer.FunctionCall;
using Sharer.Variables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharer.Command
{
    public enum SharerWriteVariableStatus : byte
    {
        NotYetWritten = 0xff,
        OK = 0,
        VariableIdOutOfRange,
        UnknownType,
        NotFound
    }

    public class SharerWriteVariableReturn
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

    public class SharerWriteValue
    {
        public string Name { get; }
        public object Value { get; }
        public SharerWriteVariableStatus Status { get; internal set; } = SharerWriteVariableStatus.NotYetWritten;
        internal SharerType Type { get; set; }
        internal int Index { get; set; }

        public SharerWriteValue(string name, object value)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");
            this.Name = name;
            this.Value = value;
        }

        public SharerWriteValue(SharerVariable variable, object value)
        {
            if (variable == null) throw new ArgumentNullException("variable");
            if (string.IsNullOrEmpty(variable.Name)) throw new ArgumentNullException("variable.Name");
            if (value == null) throw new ArgumentNullException("value");
            this.Name = variable.Name;
            this.Value = value;
        }
    }


    class SharerWriteVariablesCommand : SharerSentCommand
    {
        private List<SharerWriteValue> _values;
        private byte[] _buffer;

        public SharerWriteVariablesCommand(List<SharerWriteValue> values)
        {
            _values = values;

            using (MemoryStream memory = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(memory))
                {
                    writer.Write((short)values.Count);

                    foreach (var value in values)
                    {
                        writer.Write((short)value.Index);

                        SharerTypeHelper.Encode(value.Type, writer, value.Value);
                    }
                    _buffer = memory.ToArray();
                }
            }
        }

        public override SharerCommandID CommandID
        {
            get
            {
                return SharerCommandID.WriteVariables;
            }
        }

        internal override byte[] ArgumentsToSend()
        {
            return _buffer;
        }

        private int _receiveCount = 0;
        
        internal override bool DecodeArgument(byte b)
        {
            if (_values.Count <= _receiveCount) throw new Exception("Unbalanced stack on write variable command answer");

            _values[_receiveCount].Status = (SharerWriteVariableStatus)b;

            _receiveCount++;
            return _receiveCount >= _values.Count;
        }
    }
}
