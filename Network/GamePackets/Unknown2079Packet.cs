using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe struct Unknown2079Packet
    {
        public DateTime Data;

        public static Unknown2079Packet Create(DateTime data)
        {
            var packet = new Unknown2079Packet();
            packet.Data = data;

            return packet;
        }

        public static implicit operator Unknown2079Packet(byte* ptr)
        {
            var packet = new Unknown2079Packet();
            packet.Data = *((DateTime*)(ptr + 4));
            return packet;
        }

        public static implicit operator byte[](Unknown2079Packet packet)
        {
            var buffer = new byte[8 + 8];
            fixed (byte* ptr = buffer)
            {
                GamePackets.PacketBuilder.AppendHeader(ptr, buffer.Length, 2079);
                *((DateTime*)(ptr + 4)) = packet.Data;
            }
            return buffer;
        }

    }
}
