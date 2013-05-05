using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    class testpacket : Writer, Interfaces.IPacket
    {
        public testpacket(bool Create)
        {
            if (Create)
            {
                //Console.WriteLine("s " + Buffer + "");
                //Console.WriteLine("s " + Buffer.Length + "");
                Buffer = new byte[8 + 10];
                WriteUInt16(10, 0, Buffer);
                WriteUInt64(Program.kimo, 2, Buffer);
                WriteUInt64(Program.kimo2, 4, Buffer);
               // WriteUInt64(Program.kimo3, 6, Buffer);
            }
        }
        //24 0 34 5 3- 1 1 0 0 0 3 0 49 0 0 0 255 10 11 0 0 0 0 0 84 81 83 101 114 118 101 114
        byte[] Buffer;
       

        public void Deserialize(byte[] buffer)
        {
            this.Buffer = buffer;
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
