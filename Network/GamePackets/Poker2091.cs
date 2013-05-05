using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe struct Poker2091
    {
        //vars

        public static implicit operator Poker2091(byte* ptr)
        {
            var packet = new Poker2091();
            return packet;
        }
        public static implicit operator byte[](Poker2091 packet)
        {
            var buffer = new byte[60 + 8];
            fixed (byte* ptr = buffer)
            {
                //PacketBuilder.AppendHeader(ptr, buffer.Length, 1012);
                *((ushort*)(ptr + 2)) = 1012;
            }
            return buffer;
        }

    }
}
