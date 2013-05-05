using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Game;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe static class PacketBuilder
    {
        /// <summary>
        /// Appends size and type information to a packet pointer.
        /// </summary>
        /// <param name="ptr">Pointer to the packet</param>
        /// <param name="size">Size of the packet, including footer information</param>
        /// <param name="type">Type of the packet</param>
        public static void AppendHeader(byte* ptr, int size, ushort type)
        {
            *((ushort*)ptr) = (ushort)(size - 8);
            *((ushort*)(ptr + 2)) = type;
        }

        /// <summary>
        /// Appends a <see cref="NetStringPacker"/> to a packet pointer.
        /// </summary>
        /// <param name="buffer">Pointer to the packet</param>
        /// <param name="packer">Strings to append</param>
        
    }
}
