using System;
using System.IO;
using System.Text;

namespace PhoenixProject.Network.AuthPackets
{
    public class Authentication : Interfaces.IPacket
    {
        public string Username;
        public string Password;//
        public string Server;
        public void Deserialize(byte[] packet)
        {

            if (BitConverter.ToUInt16(packet, 2) == 1124)
            {

                byte[] Packer_Account = new byte[16];

                for (byte x = 0; x < 16; x++)
                    Packer_Account[x] = packet[x + 8];

                Username = Encoding.ASCII.GetString(Packer_Account).Split('\0')[0];




                byte[] Packer_Password = new byte[16];

                for (byte x = 0; x < 16; x++)
                {
                    Packer_Password[x] = packet[x + 72];
                }
                Password = Encoding.ASCII.GetString(Packer_Password).Split('\0')[0];
            }
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