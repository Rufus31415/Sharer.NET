using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharer.FunctionCall
{
    public enum SharerType : byte
    {
        @void,
        @int,
        @float
    }

    public static class SharerTypeHelper
    {
        public static int Sizeof(SharerType type)
        {
            switch (type)
            {
                case SharerType.@int:
                    return 2;
                case SharerType.@float:
                    return 4;
                default:
                    return 0;
            }
        }

        public static object Decode(SharerType type, byte[] buffer)
        {
            using (MemoryStream memory = new MemoryStream(buffer))
            {
                using (BinaryReader reader = new BinaryReader(memory))
                {
                    switch (type)
                    {
                        case SharerType.@int:
                            return reader.ReadInt16();
                        case SharerType.@float:
                            return reader.ReadSingle();
                        default:
                            return null;
                    }
                }
            }
        }
    }
}
