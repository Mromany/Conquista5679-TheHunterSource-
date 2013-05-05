using System;

namespace PhoenixProject.Network.GamePackets
{
    public class KimoChi : Writer, Interfaces.IPacket
    {


        public KimoChi(bool Create)
        {
            if (Create)
            {
                //Console.WriteLine("s " + Buffer + "");
                //Console.WriteLine("s " + Buffer.Length + "");
                Buffer = new byte[8 + 24];
                WriteUInt16(24, 0, Buffer);
                WriteUInt16(0, 1, Buffer);
                WriteUInt16(2533, 2, Buffer);
               
            }
        }
        //24 0 34 5 3- 1 1 0 0 0 3 0 49 0 0 0 255 10 11 0 0 0 0 0 84 81 83 101 114 118 101 114
        byte[] Buffer;
        /*
         * ushort length
ushort 1314
Byte 3
Byte boxid
Byte boxid
Byte socket 1
Byte socket 2
Byte plus
Byte 3
Byte JadesAdded
ushort how many times you can still use the lotto today
ushort 0
uint ItemID
         * */


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
