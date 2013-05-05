using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe class HoldingHands
    {
        public byte[] Buffer;
        public HoldingHands(byte direction, byte speed)
        {
            Buffer = new byte[32];
            fixed (byte* ptr = Buffer)
            {
                *((ushort*)(ptr)) = 24;
                *((ushort*)(ptr + 2)) = 1114;
                *((ushort*)(ptr + 4)) = 1;
                *(ptr + 6) = direction;
                *(ptr + 7) = speed;
                *(ptr + 12) = 2;
            }
        }
        public HoldingHands(ushort x, ushort y)
        {
            Buffer = new byte[32];
            fixed (byte* ptr = Buffer)
            {
                *((ushort*)(ptr)) = 24;
                *((ushort*)(ptr + 2)) = 1114;
                *((ushort*)(ptr + 4)) = 2;
                *((ushort*)(ptr + 6)) = x;
                *((ushort*)(ptr + 8)) = y;
                *(ptr + 12) = 2;
            }
        }
        public HoldingHands(byte direction)
        {
            Buffer = new byte[32];
            fixed (byte* ptr = Buffer)
            {
                *((ushort*)(ptr)) = 24;
                *((ushort*)(ptr + 2)) = 1114;
                *((ushort*)(ptr + 6)) = direction;
                *(ptr + 12) = 2;
            }
        }
        public uint Identity
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { fixed (byte* ptr = Buffer) *((uint*)(ptr + 16)) = value; }
        }
        public uint Target
        {
            get { return BitConverter.ToUInt32(Buffer, 20); }
            set { fixed (byte* ptr = Buffer) *((uint*)(ptr + 20)) = value; }
        }
    }
}
