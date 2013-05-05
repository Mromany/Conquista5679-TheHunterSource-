using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe struct Unknown2078Packet
    {
        public uint Data;

        public static Unknown2078Packet Create(uint data)
        {
            var packet = new Unknown2078Packet();
            packet.Data = data;

            return packet;
        }

        public static implicit operator Unknown2078Packet(byte* ptr)
        {
            var packet = new Unknown2078Packet();
            packet.Data = *((uint*)(ptr + 4));
            return packet;
        }

        public static implicit operator byte[](Unknown2078Packet packet)
        {
            var buffer = new byte[264 + 8];
            fixed (byte* ptr = buffer)
            {
                GamePackets.PacketBuilder.AppendHeader(ptr, buffer.Length, 2078);
                *((uint*)(ptr + 4)) = packet.Data;
            }
            return buffer;
        }
    }
}
