using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe class VipStatus
    {
        byte[] Buffer;
        public byte[] ToArray()
        {
            return Buffer;
        }
        public VipStatus()
        {
            Buffer = new byte[16];
            fixed (byte* ptr = Buffer)
            {
                *((ushort*)(ptr)) = 8;
                *((ushort*)(ptr + 2)) = 1129;
                *((uint*)(ptr + 4)) = 57255;
            }
        }
    }
}
