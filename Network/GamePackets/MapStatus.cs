﻿using System;

namespace PhoenixProject.Network.GamePackets
{
    public class MapStatus : Writer, Interfaces.IPacket
    {
        private byte[] Buffer;
        public MapStatus()
        {
            Buffer = new byte[24];
            WriteUInt16(16, 0, Buffer);
            WriteUInt16(1110, 2, Buffer);
        }
        public uint ID
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { WriteUInt32(value, 4, Buffer); }
        }
        public ushort BaseID
        {
            get { return BitConverter.ToUInt16(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }
        public uint Status
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer);  }
        }
        public uint Weather
        {
            get { return BitConverter.ToUInt32(Buffer, 20); }
            set { WriteUInt32(value, 20, Buffer); }
        }
        public byte[] ToArray()
        {
            return Buffer;
        }
        public void Deserialize(byte[] buffer)
        {
            throw new NotImplementedException();
        }
        public void Send(Client.GameState client)
        {
            client.Send(Buffer);
        }
    }
}
