using System;
using System.IO;
using System.Text;

namespace Sharer.UserData
{
    /// <summary>
    /// Describes a received custom user message
    /// </summary>
    public class UserDataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Received raw byte data
        /// </summary>
        public byte[] Data { get; }

        internal UserDataReceivedEventArgs(byte[] data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Returns a reader to facilitate data decoding (string, int, double, ...)
        /// </summary>
        public BinaryReader GetReader()
        {
            return new InternalBinaryReader(Data);
        }

        /// <summary>
        /// A custom reader to properly read strings
        /// </summary>
        private class InternalBinaryReader : BinaryReader
        {
            public InternalBinaryReader(byte[] input) : base(new MemoryStream(input), Encoding.UTF8) { }

            public override string ReadString()
            {
                var sb = new StringBuilder();

                int c;

                while (true)
                {
                    c = Read();

                    // if 0 (end of string) or -1 (end of stream)
                    if (c <= 0) break;

                    sb.Append((char)c);
                }

                return sb.ToString();
            }
        }
    }
}