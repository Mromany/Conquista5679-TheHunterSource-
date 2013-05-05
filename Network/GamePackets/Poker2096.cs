using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe struct Poker2096
    {
        public uint Type, Subtype, UID;
        public static implicit operator Poker2096(byte* ptr)
        {
            var packet = new Poker2096();
            packet.Type = *((uint*)(ptr + 4));
            packet.Subtype = *((uint*)(ptr + 8));
            packet.UID = *((uint*)(ptr + 12));
            return packet;
        }
        public static implicit operator byte[](Poker2096 packet)
        {
            var buffer = new byte[16 + 8];
            fixed (byte* ptr = buffer)
            {
               // PacketBuilder.AppendHeader(ptr, buffer.Length, 2096);
                *((ushort*)(ptr + 2)) = 2096;
                *((uint*)(ptr + 4)) = packet.Type;
                *((uint*)(ptr + 8)) = packet.Subtype;
                *((uint*)(ptr + 12)) = packet.UID;
            }
            return buffer;
        }

    }
}
