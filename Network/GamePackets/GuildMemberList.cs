using System;
using System.IO;
using System.Text;
using PhoenixProject.Game.ConquerStructures.Society;

namespace PhoenixProject.Network.GamePackets
{
    public class GuildMemberList
    {
        public ushort Size;
        public ushort Type;
        public ushort SubType;
        public ushort PageNumber;
        public Guild g;

        public GuildMemberList(byte[] packet)
        {
            BinaryReader Reader = new BinaryReader(new MemoryStream(packet));
            Size = Reader.ReadUInt16();
            Type = Reader.ReadUInt16();
            SubType = Reader.ReadUInt16();
            PageNumber = Reader.ReadUInt16();
        }

        public byte[] Build()
        {
            MemoryStream Stream = new MemoryStream();
            BinaryWriter Writer = new BinaryWriter(Stream);

            Writer.Write((ushort)0);
            Writer.Write((ushort)2102);
            Writer.Write((ushort)0);
            Writer.Write((ushort)1);//page
            Writer.Write((ushort)0);
            Writer.Write((ushort)0);
            Writer.Write((ushort)g.Members.Count);//count
            Writer.Write((ushort)0);
            foreach (PhoenixProject.Game.ConquerStructures.Society.Guild.Member m in g.Members.Values)
            {
                for (int i = 0; i < 16; i++)//16 offsets
                {
                    if (i < m.Name.Length)
                    {
                        Writer.Write((byte)m.Name[i]);
                    }
                    else
                        Writer.Write((byte)0);
                }
                Writer.Write((ushort)m.NobilityRank);
                Writer.Write((ushort)0);
                Writer.Write((ushort)1);
                Writer.Write((ushort)0);
                Writer.Write((uint)m.Level);
                Writer.Write((uint)m.Rank);
                Writer.Write((uint)0);
                Writer.Write((uint)m.SilverDonation);
                if (m.Client != null)
                { Writer.Write((byte)1); }
                else
                { Writer.Write((byte)0); }
                Writer.Write((byte)0);
                Writer.Write((ushort)0);
                Writer.Write((ushort)0);
                Writer.Write((ushort)0);
            }
            int packetlength = (int)Stream.Length;
            Stream.Position = 0;
            Writer.Write((ushort)packetlength);
            Stream.Position = Stream.Length;
            Writer.Write(ASCIIEncoding.ASCII.GetBytes("TQServer"));
            Stream.Position = 0;
            byte[] buf = new byte[Stream.Length];
            Stream.Read(buf, 0, buf.Length);
            Writer.Close();
            Stream.Close();
            return buf;
        }
    }
    public class GuildDonationList
    {
        public ushort Size;
        public ushort Type;
        public ushort SubType;
        public ushort PageNumber;
        public Guild g;

        public GuildDonationList(byte[] packet)
        {
            BinaryReader Reader = new BinaryReader(new MemoryStream(packet));
            Size = Reader.ReadUInt16();
            Type = Reader.ReadUInt16();
            SubType = Reader.ReadUInt16();
            PageNumber = Reader.ReadUInt16();
        }

        public byte[] Build()
        {
            MemoryStream Stream = new MemoryStream();
            BinaryWriter Writer = new BinaryWriter(Stream);

            Writer.Write((ushort)0);
            Writer.Write((ushort)2102);
            Writer.Write((ushort)SubType);
            Writer.Write((ushort)PageNumber);//page
            Writer.Write((ushort)0);
            Writer.Write((ushort)0);
            Writer.Write((ushort)g.Members.Count);//count
            Writer.Write((ushort)0);
            for (int i = 0; i < 16; i++)//16 offsets
            {
                if (i < "TestName".Length)
                {
                    Writer.Write((byte)"TestName"[i]);
                }
                else
                    Writer.Write((byte)0);
            }
            Writer.Write((ulong)0);
            Writer.Write((uint)130);//level
            Writer.Write((uint)1000);//guildrank
            Writer.Write((uint)0);//unknown
            Writer.Write((uint)10000);//donation
            Writer.Write((byte)1);//online-offline
            Writer.Write((byte)0);
            Writer.Write((ushort)0);
            Writer.Write((ushort)0);
            Writer.Write((ushort)0);
            int packetlength = (int)Stream.Length;
            Stream.Position = 0;
            Writer.Write((ushort)packetlength);
            Stream.Position = Stream.Length;
            Writer.Write(ASCIIEncoding.ASCII.GetBytes("TQServer"));
            Stream.Position = 0;
            byte[] buf = new byte[Stream.Length];
            Stream.Read(buf, 0, buf.Length);
            Writer.Close();
            Stream.Close();
            return buf;
        }
    }
}