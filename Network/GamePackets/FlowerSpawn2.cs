using System;
using System.IO;
using System.Text;
using PhoenixProject.Game.ConquerStructures.Society;

namespace PhoenixProject.Network.GamePackets
{
    public class FlowerSpawn : Writer
    {
        byte[] Buffer;
        public FlowerSpawn(string Type, string name, string Flowers, string UID, uint FlowerID)
        {
            string send = Type + " " + Flowers + " " + UID + " " + UID + " " + name + " " + name + "";
            Buffer
                = new byte[88];//18
            WriteUInt16((byte)(80), 0, Buffer);
            WriteUInt16(1151, 2, Buffer);

            Buffer[4] = 2;
            WriteUInt32(FlowerID, 8, Buffer);
            Buffer[16] = 1;
            Buffer[24] = 1;
            Buffer[32] = 1;

            WriteUInt32(uint.Parse(UID), 40, Buffer);
            WriteUInt32(uint.Parse(UID), 44, Buffer);
            //Buffer[17] = 1;//13
            // Buffer[18] = (byte)(send.Length & 255);
            for (int i = 0; i < send.Length; i++)
            {
                try
                {
                    Buffer[48 + i] = Convert.ToByte(send[i]);
                    Buffer[48 + i + 16] = Convert.ToByte(send[i]);

                }
                catch { }
            }
        }
        public byte[] ThePacket()
        {
            return Buffer;
        }
    }
    public class aFlowerSpawn2 : Writer
    {
        byte[] Buffer;
        public aFlowerSpawn2(string UID)
        {
            string send = UID + " 1 0";
            Buffer
                = new byte[21 + send.Length + 8];
            WriteUInt16((byte)(21 + send.Length), 0, Buffer);
            WriteUInt16(1150, 2, Buffer);

            Buffer[4] = 4;
            Buffer[16] = 1;
            Buffer[17] = (byte)(send.Length & 255);
            for (int i = 0; i < send.Length; i++)
            {
                try
                {
                    Buffer[18 + i] = Convert.ToByte(send[i]);

                }
                catch { }
            }
        }
        public byte[] ThePacket()
        {
            return Buffer;
        }
    }

    public class FlowerRank : Writer
    {
        byte[] Buffer;
        public FlowerRank(uint UID)
        {
            int PacketLength = 80;
            uint charamount = 0;
            uint place = 1;
            string[] playernames = new string[100000];
            uint[] playerflowers = new uint[1000000];
            MemoryStream Stream = new MemoryStream();
            BinaryWriter Writer = new BinaryWriter(Stream);
            Database.MySqlCommand cmd = new Database.MySqlCommand(Database.MySqlCommandType.SELECT);
            cmd.Select("flowers").Order("redroses DESC");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            while (r.Read())
            {
                int redroses = r.ReadInt32("redroses");
                if (redroses != 0)
                {
                    uint charuid = r.ReadUInt32("id");
                    PacketLength += (r.ReadString("name").Length * 2) + 36;
                    playernames[charamount] = r.ReadString("name");
                    playerflowers[charamount] = r.ReadUInt32("redroses");
                    charamount++;
                    
                }
            }
            Buffer = new byte[80 +8];
            WriteUInt16(80, 0, Buffer);
            WriteUInt16(1151, 2, Buffer);

            WriteUInt32(1, 4, Buffer);
            WriteUInt32(UID, 8, Buffer);
            WriteUInt32(0, 12, Buffer);
            WriteUInt32(charamount, 16, Buffer);
            int position = 24;
            for(int x = 0; x < charamount; x++)
            {
               // WriteUInt32(1, position, Buffer);
               // position+=4;
                WriteUInt32(place, position, Buffer);
                place++;
                //WriteUInt32(place, position, Buffer);
                position+=4;
                WriteUInt32(playerflowers[1], position, Buffer);
                position += 4;
                WriteUInt32(playerflowers[1], position, Buffer);
                position += 4;
                WriteUInt32(UID, position, Buffer);
                position += 4;
                WriteUInt32(UID, position, Buffer);
                position += 4;
                WriteString(playernames[x], position, Buffer);
                position += 16;
                WriteUInt32(1, position, Buffer);
                position += 4;
                WriteString(playernames[x], position, Buffer);
                position += 8;
                position = position + 16;
                
            }
          
            WriteString("TQServer", position, Buffer);
           

        }

        public byte[] ThePacket()
        {
            return Buffer;
        }

        
        
    }
}
