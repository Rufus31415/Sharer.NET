using System;
using System.IO;

namespace Sharer.FunctionCall
{
    /// <summary>
    /// List of all types supported by Sharer for variables and function argument and return
    /// </summary>
    public enum SharerType : byte
    {
        /// <summary>
        /// No type returned
        /// </summary>
        @void,

        /// <summary>
        /// Boolean encoded in a byte (0x00=false, else true)
        /// </summary>
        @bool,

        /// <summary>
        /// Unsigned 8 bits integer
        /// </summary>
        @byte,

        /// <summary>
        /// Signed 8 bits integer
        /// </summary>
        @short,

        /// <summary>
        /// Signed 16 bits integer
        /// </summary>
        @int,

        /// <summary>
        /// Unsigned 16 bits integer
        /// </summary>
        @uint,

        /// <summary>
        /// Signed 32 bits integer
        /// </summary>
        @long,

        /// <summary>
        /// Unsigned 32 bits integer
        /// </summary>
        @ulong,

        /// <summary>
        /// Signed 64 bits integer
        /// </summary>
        int64,

        /// <summary>
        /// unsigned 64 bits integer
        /// </summary>
        uint64,

        /// <summary>
        /// 32 bits float
        /// </summary>
        @float,

        /// <summary>
        /// 64 bits float
        /// </summary>
        @double
    }

    /// <summary>
    /// Static function to hel data manipulation
    /// </summary>
    internal static class SharerTypeHelper
    {
        internal static int Sizeof(SharerType type)
        {
            switch (type)
            {
                case SharerType.@bool:
                case SharerType.@byte:
                case SharerType.@short:
                    return 1;
                case SharerType.@int:
                case SharerType.@uint:
                    return 2;
                case SharerType.@long:
                case SharerType.@ulong:
                case SharerType.@float:
                    return 4;
                case SharerType.int64:
                case SharerType.uint64:
                case SharerType.@double:
                    return 8;
                default:
                    return 0;
            }
        }

        internal static void Encode(SharerType type, BinaryWriter writer, object value)
        {
            if (value == null) throw new ArgumentNullException("value");

            switch (type)
            {
                case SharerType.@bool:
                    var str = value.ToString();
                    bool boolValue;
                    if (Int64.TryParse(str, out long longValue)) boolValue = longValue != 0;
                    else boolValue = Boolean.Parse(str);
                    writer.Write((byte)(boolValue? 1:0));
                    break;
                case SharerType.@byte:
                    writer.Write(Byte.Parse(value.ToString()));
                    break;
                case SharerType.@short:
                    writer.Write(SByte.Parse(value.ToString()));
                    break;
                case SharerType.@int:
                    writer.Write(Int16.Parse(value.ToString()));
                    break;
                case SharerType.@uint:
                    writer.Write(UInt16.Parse(value.ToString()));
                    break;
                case SharerType.@long:
                    writer.Write(Int32.Parse(value.ToString()));
                    break;
                case SharerType.@ulong:
                    writer.Write(UInt32.Parse(value.ToString()));
                    break;
                case SharerType.@float:
                    writer.Write(float.Parse(value.ToString().Replace(",","."), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture));
                    break;
                case SharerType.int64:
                    writer.Write(Int64.Parse(value.ToString()));
                    break;
                case SharerType.uint64:
                    writer.Write(UInt64.Parse(value.ToString()));
                    break;
                case SharerType.@double:
                    writer.Write(double.Parse(value.ToString().Replace(",", "."), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture));
                    break;
                default:
                    throw new Exception("Type " + type.ToString() + " not supported");
            }
        }

        internal static object Decode(SharerType type, byte[] buffer)
        {
            using (MemoryStream memory = new MemoryStream(buffer))
            {
                using (BinaryReader reader = new BinaryReader(memory))
                {
                    switch (type)
                    {
                        case SharerType.@bool:
                            return reader.ReadByte() != 0;
                        case SharerType.@byte:
                            return reader.ReadByte();
                        case SharerType.@short:
                            return reader.ReadSByte();
                        case SharerType.@int:
                            return reader.ReadInt16();
                        case SharerType.@uint:
                            return reader.ReadUInt16();
                        case SharerType.@long:
                            return reader.ReadInt32();
                        case SharerType.@ulong:
                            return reader.ReadUInt32();
                        case SharerType.@float:
                            return reader.ReadSingle();
                        case SharerType.int64:
                            return reader.ReadInt64();
                        case SharerType.uint64:
                            return reader.ReadUInt64();
                        case SharerType.@double:
                            return reader.ReadDouble();
                        default:
                            return null;
                    }
                }
            }
        }
    }
}
