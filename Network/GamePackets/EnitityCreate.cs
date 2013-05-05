using System;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class EnitityCreate : Interfaces.IPacket
    {
        public string Name;
        public ushort Body;
        public byte Class;
        public void Deserialize(byte[] buffer)
        {
            Name = Encoding.UTF7.GetString(buffer, 24, 16).Replace("\0", "");
            Body = BitConverter.ToUInt16(buffer, 72);
            Class = (byte)BitConverter.ToUInt16(buffer, 74);
        }
        public byte[] ToArray()
        {
            throw new NotImplementedException();
        }
        public void Send(Client.GameState client)
        {
            throw new NotImplementedException();
        }
    }
}
