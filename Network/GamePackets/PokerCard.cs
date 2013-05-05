using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe struct PokerCard
    {
        //vars
        public static implicit operator PokerCard(byte* ptr)
        {
            var packet = new PokerCard();
            return packet;
        }
        public static implicit operator byte[](PokerCard packet)
        {
            var buffer = new byte[32 + 8];
            fixed (byte* ptr = buffer)
            {
                *((uint*)(ptr + 4)) = 1012;
            }
            return buffer;
        }

    }
}
