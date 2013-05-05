using System;
using System.Text;

namespace Conquer_Online_Server.Network.GamePackets
{
    public class Clan : Writer
    {
        byte[] Bufffer;
        public byte[] ToArray()
        {
            return Bufffer;
        }
        public Clan(Client.GameState client, byte ClanType)
        {
            int Position = 18;
            //ID  Member Unknow donation Level rank Unknow Unknow Unknow Unknow Unknow Mydonation

            string name1 = client.Entity.Clan.ID
                + " " + client.Entity.Clan.Members.Count + " 0 "
                + client.Entity.Clan.Donation + " "
                + client.Entity.Clan.Level + " "
                + client.Entity.ClanMember.Rank + " 0 0 0 0 0 "
                + client.Entity.ClanMember.Donation;

            string clanname = client.Entity.Clan.Name;
            string LiderName = client.Entity.Clan.Leader;
            string Unknow = "0 0 0 0 0 0 0";

            Bufffer = new byte[8 + 141 + (byte)(clanname.Length + LiderName.Length)];
            WriteUInt16((ushort)(Bufffer.Length - 8), 0, Bufffer);
            WriteUInt16(1312, 2, Bufffer);

            Bufffer[16] = (byte)6;
            Bufffer[17] = (byte)name1.Length;

            WriteByte(ClanType, 4, Bufffer);
            WriteUInt16((ushort)client.Entity.ClanUID, 8, Bufffer);
            
            for (int i = 0; i < name1.Length; i++)
            {
                try
                {
                    Bufffer[Position + i] = Convert.ToByte(name1[i]);
                }
                catch { }
            }
            Position += (byte)name1.Length;

            WriteByte((byte)clanname.Length, Position, Bufffer);
            Position += 1;

            for (int i = 0; i < clanname.Length; i++)
            {
                try
                {
                    Bufffer[Position + i] = Convert.ToByte(clanname[i]);
                }
                catch { }
            }

            Position += (byte)clanname.Length;

            WriteByte((byte)LiderName.Length, Position, Bufffer);
            Position += 1;
            for (int i = 0; i < LiderName.Length; i++)
            {
                try
                {
                    Bufffer[Position + i] = Convert.ToByte(LiderName[i]);
                }
                catch { }
            }

            Position += (byte)LiderName.Length;
            WriteByte((byte)Unknow.Length, Position, Bufffer);
            Position += 1;
            for (int i = 0; i < Unknow.Length; i++)
            {
                try
                {
                    Bufffer[Position + i] = Convert.ToByte(Unknow[i]);
                }
                catch { }
            }
        }
    }
}
