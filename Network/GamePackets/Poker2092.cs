using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe struct Poker2092
    {
        //vars
        public static implicit operator Poker2092(byte* ptr)
        {
            var packet = new Poker2092();
            return packet;
        }
        public static implicit operator byte[](Poker2092 packet)
        {
            var buffer = new byte[32 + 8];
            fixed (byte* ptr = buffer)
            {
                *((ushort*)(ptr + 2)) = 1012;
            }
            return buffer;
        }

    }
}
