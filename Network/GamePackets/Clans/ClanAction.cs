using System;

namespace Conquer_Online_Server.Network.GamePackets
{
    public class ClanAction : Writer, Interfaces.IPacket
    {
        byte[] Buffer;

        public ClanAction(String Value, String Name)
        {
            Buffer = new byte[96 + Value.Length + Name.Length];
            WriteUInt16((ushort)(Buffer.Length - 8), 0, Buffer);
            WriteUInt16(1312, 2, Buffer);
            WriteByte(0x02, 16, Buffer);
            WriteByte((byte)Value.Length, 17, Buffer);
            WriteString(Value, 18, Buffer);
            WriteByte((byte)Name.Length, 17 + Value.Length, Buffer);
            WriteString(Name, 18 + Value.Length, Buffer);
        }
        public uint Type
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { WriteUInt32(value, 4, Buffer); }
        }
        public uint UID
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }
        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
        public uint dwParam
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }
        public byte[] ToArray()
        {
            return Buffer;
        }
        public void Send(Client.GameState client)
        {
            client.Send(Buffer);
        }
    }
}
