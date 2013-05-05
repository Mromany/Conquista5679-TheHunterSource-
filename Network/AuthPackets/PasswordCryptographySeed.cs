using System;
using System.Text;
namespace PhoenixProject.Network.AuthPackets
{
    public class PasswordCryptographySeed : Interfaces.IPacket
    {
        byte[] Buffer;
        public PasswordCryptographySeed()
        {
            Buffer = new byte[8];
            Network.Writer.WriteUInt16(8, 0, Buffer);
            Network.Writer.WriteUInt32(0xDEADBEEF, 2, Buffer);
            Network.Writer.WriteUInt32(1059, 2, Buffer);
        }
        public int Seed
        {
            get
            {
                return BitConverter.ToInt32(Buffer, 4);
            }
            set
            {
                Network.Writer.WriteInt32(value, 4, Buffer);
            }
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
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
