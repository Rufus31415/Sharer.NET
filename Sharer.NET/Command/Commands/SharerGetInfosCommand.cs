using System;
using System.Collections.Generic;
using System.Text;

namespace Sharer.Command
    {
    /// <summary>
    /// Information about the board. This command is also sent at startup
    /// </summary>
    public class SharerGetInfosCommand : SharerSentCommand
    {
        /// <summary>
        /// Arduino Sharer library major version
        /// </summary>
        public byte MajorVersion;

        /// <summary>
        /// Arduino Sharer library minor version
        /// </summary>
        public byte MinorVersion;

        /// <summary>
        /// Arduino Sharer library fix version
        /// </summary>
        public byte FixVersion;

        /// <summary>
        /// Arduino board name
        /// </summary>
        public string Board;

        /// <summary>
        /// Arduino CPU frequency in Hz
        /// </summary>
        public int CPUFrequency;

        /// <summary>
        /// C++ version used to compile the Arduino sketch
        /// </summary>
        public int CPlusPlusVersion;

        /// <summary>
        /// GCC compiler version used to compile the Arduino sketch
        /// </summary>
        public int GCCVersion;

        /// <summary>
        /// Number of functions exposed by Sharer
        /// </summary>
        public int FunctionCount;

        /// <summary>
        /// Maximum number of function that Sharer can share (see SharerConfig.h)
        /// </summary>
        public int FunctionMaxCount;

        /// <summary>
        /// Number of variables exposed by Sharer
        /// </summary>
        public int VariableCount;

        /// <summary>
        /// Maximum number of variable that Sharer can share (see SharerConfig.h)
        /// </summary>
        public int VariableMaxCount;

        /// <summary>
        /// A human readable string
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("Sharer version: ");
            sb.Append(MajorVersion);
            sb.Append(".");
            sb.Append(MinorVersion);
            sb.Append(".");
            sb.Append(FixVersion);
            sb.AppendLine();

            sb.Append("Board: ");
            sb.AppendLine(Board);

            sb.Append("CPU Frequency: ");
            sb.Append(CPUFrequency);
            sb.AppendLine("Hz");

            sb.Append("C++ version: ");
            sb.Append(CPlusPlusVersion);
            sb.AppendLine();

            sb.Append("Functions shared: ");
            sb.Append(FunctionCount);
            sb.AppendLine();

            sb.Append("Functions max capacity: ");
            sb.Append(FunctionMaxCount);
            sb.AppendLine();

            sb.Append("Variables shared: ");
            sb.Append(VariableCount);
            sb.AppendLine();

            sb.Append("Variables max capacity: ");
            sb.Append(VariableMaxCount);
            sb.AppendLine();

            return sb.ToString();
        }

        internal override SharerCommandID CommandID => SharerCommandID.GetInfo;


        internal override byte[] ArgumentsToSend()
        {
            return null;
        }

        private int _step = 0;

        private List<byte> _buffer = new List<byte>(16);
       
        internal override bool DecodeArgument(byte b)
        {
            switch (_step)
            {
                case 0:
                    MajorVersion = b;
                    _step++;
                    break;
                case 1:
                    MinorVersion = b;
                    _step++;
                    break;
                case 2:
                    FixVersion = b;
                    _step++;
                    break;
                case 3:
                    // if end of string
                    if (b == 0)
                    {
                        Board = Encoding.UTF8.GetString(_buffer.ToArray(), 0, _buffer.Count);
                        _buffer.Clear();
                        _step++;
                    }
                    else
                    {
                        _buffer.Add(b);
                    }

                    break;

                case 4:
                    ReadInt(b, ref CPUFrequency, 32);
                    break;

                case 5:
                    ReadInt(b, ref GCCVersion, 16);
                    break;

                case 6:
                    ReadInt(b, ref CPlusPlusVersion, 32);
                    break;

                case 7:
                    ReadInt(b, ref FunctionCount, 16);
                    break;

                case 8:
                    ReadInt(b, ref FunctionMaxCount, 16);
                    break;

                case 9:
                    ReadInt(b, ref VariableCount, 16);
                    break;

                case 10:
                    if(ReadInt(b, ref VariableMaxCount, 16))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }


        private bool ReadInt(byte b, ref int value, int size)
        {
            _buffer.Add(b);

            if (_buffer.Count * 8 == size)
            {
                if (size == 16)
                {
                    value = BitConverter.ToInt16(_buffer.ToArray(), 0);
                }
                else
                {
                    value = BitConverter.ToInt32(_buffer.ToArray(), 0);
                }

                _buffer.Clear();
                _step++;

                return true;
            }

            return false;
        }
    }
}
