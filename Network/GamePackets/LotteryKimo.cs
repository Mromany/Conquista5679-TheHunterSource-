using System;
using PhoenixProject.Game;

namespace PhoenixProject.Network.GamePackets
{
    public class KimoLotterys : Writer, Interfaces.IPacket
    {
       

        public KimoLotterys(bool Create)
        {
            if (Create)
            {
               
                Buffer = new byte[8 + 24];
                WriteUInt16(24, 0, Buffer);
                WriteUInt16(0, 1, Buffer);
                WriteUInt16(1314, 2, Buffer);
                WriteUInt16(1, 6, Buffer);//fast or slow

                // Buffer = new byte[8 + 24];
                //WriteUInt16(24, 0, Buffer);
               // WriteUInt16(0, 1, Buffer);
              //  WriteUInt16(1314, 2, Buffer);
                WriteUInt16(5, 3, Buffer);
                WriteUInt16(3, 4, Buffer);//Jades
                WriteUInt16(1, 5, Buffer);
               // WriteUInt16(1, 6, Buffer);//fast or slow
               // WriteUInt16(3, 7, Buffer);//Gem1
               // WriteUInt16(13, 8, Buffer);//Gem2
                //WriteUInt16(11, 9, Buffer);//plus
                WriteUInt16(3, 10, Buffer);
                WriteUInt16(0, 11, Buffer);
                //WriteUInt16(8, 12, Buffer);//lottery times


                //WriteUInt16(0, 13, Buffer);
                //WriteUInt16(0, 14, Buffer);
               // WriteUInt16(0, 15, Buffer);
               // WriteInt32((int)Program.lotterytype, 16, Buffer);//itemchange
              //  WriteInt32((int)Program.lotteryprize, 17, Buffer);//item kind
                WriteUInt16(11, 18, Buffer);
                WriteUInt16(0, 19, Buffer);
                WriteUInt16(0, 20, Buffer);
                WriteUInt16(0, 22, Buffer);
                //WriteInt32((int)480339, 24, Buffer);
            }
        }
        //24 0 34 5 3- 1 1 0 0 0 3 0 49 0 0 0 255 10 11 0 0 0 0 0 84 81 83 101 114 118 101 114
        byte[] Buffer;
        public byte AddJadeChances
        {
            get
            {
                return Buffer[11];
            }
            set
            {
                Buffer[11] = value;
            }
        }

        public byte Chances
        {
            get
            {
                return Buffer[12];
            }
            set
            {
                Buffer[12] = value;
            }
        }

        public Enums.Color Color
        {
            get
            {
                return (Enums.Color)(byte)System.BitConverter.ToUInt16(Buffer, 10);
            }
            set
            {
                WriteUInt16((ushort)value, 10, Buffer);
            }
        }

        public byte Plus
        {
            get
            {
                return Buffer[9];
            }
            set
            {
                WriteUInt16(value, 9, Buffer);
            }
        }

        public uint Prize
        {
            get
            {
                return System.BitConverter.ToUInt32(Buffer, 0x10);
            }
            set
            {
                WriteUInt32(value, 0x10, Buffer);
            }
        }

        public byte SocketOne
        {
            get
            {
                return Buffer[7];
            }
            set
            {
                WriteUInt16(value, 7, Buffer);
            }
        }

        public byte SocketTwo
        {
            get
            {
                return Buffer[8];
            }
            set
            {
                WriteUInt16(value, 8, Buffer);
            }
        }

        public byte Type
        {
            get
            {
                return Buffer[4];
            }
            set
            {
                WriteUInt16(value, 4, Buffer);
            }
        }
       /* public Enums.LotteryTypes Type
        {
            get
            {
                return (Enums.LotteryTypes)System.BitConverter.ToUInt16(Buffer, 4);
            }
            set
            {
                WriteUInt16((ushort)value, 4, Buffer);
            }
        }*/
       

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
