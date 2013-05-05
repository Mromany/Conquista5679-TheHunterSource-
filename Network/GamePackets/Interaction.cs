using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace PhoenixProject.Network.GamePackets
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Interaction
    {
        /// <summary> Offset 8</summary>
        [FieldOffset(8)]
        public uint Identity;
        /// <summary> Offset 12</summary>
        [FieldOffset(12)]
        public uint Target;
        /// <summary> Offset 16</summary>
        [FieldOffset(16)]
        public ushort X;
        /// <summary> Offset 18</summary>
        [FieldOffset(18)]
        public ushort Y;
        /// <summary> Offset 20</summary>
        [FieldOffset(20)]
        public uint Action;
        /// <summary> Offset 24</summary>
        [FieldOffset(24)]
        public uint Effect;
        /// <summary> Offset 28</summary>
        [FieldOffset(28)]
        public uint Parameter;

        public Interaction(byte[] buffer)
        {
            Identity = BitConverter.ToUInt32(buffer, 8);
            Target = BitConverter.ToUInt32(buffer, 12);
            X = BitConverter.ToUInt16(buffer, 16);
            Y = BitConverter.ToUInt16(buffer, 18);
            Action = BitConverter.ToUInt32(buffer, 20);
            Effect = BitConverter.ToUInt32(buffer, 24);
            Parameter = BitConverter.ToUInt32(buffer, 28);
        }
        public Interaction(uint identity, uint target)
        {
            Identity = identity;
            Target = target;
            X = 0;
            Y = 0;
            Action = 0;
            Effect = 0;
            Parameter = 0;
        }
        public unsafe byte[] ToArray()
        {
            byte[] buffer = new byte[48];
            fixed (byte* ptr = buffer)
            {
                *(Interaction*)ptr = this;
                *((ushort*)(ptr)) = 40;
                *((ushort*)(ptr + 2)) = 1022;
            }
            return buffer;
        }
    }
}
