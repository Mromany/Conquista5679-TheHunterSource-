using System;
using System.IO;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class PkExplorer
    {
        public ushort Size;
        public ushort Type;
        public uint SubType;
        public uint Values;
        public uint MaxCount;
        public Client.GameState client;
        Game.PkExpeliate[] PkValues = new Game.PkExpeliate[0];

        public PkExplorer(byte[] packet, Client.GameState _c)
        {
            client = _c;
            MaxCount = (uint)client.Entity.PkExplorerValues.Count;
            if (MaxCount >= 10) { MaxCount = 10; }
            Values = (uint)client.Entity.PkExplorerValues.Count;
            PkValues = new Game.PkExpeliate[client.Entity.PkExplorerValues.Count];
            client.Entity.PkExplorerValues.Values.CopyTo(PkValues, 0);
            BinaryReader Reader = new BinaryReader(new MemoryStream(packet));
            Size = Reader.ReadUInt16();
            Type = Reader.ReadUInt16();
            SubType = Reader.ReadUInt32();
        }

        public byte[] Build()
        {
            MemoryStream Stream = new MemoryStream();
            BinaryWriter Writer = new BinaryWriter(Stream);

            Writer.Write((ushort)0);
            Writer.Write((ushort)2220);
            Writer.Write((uint)SubType);
            Writer.Write((uint)Values);
            Writer.Write((uint)MaxCount);
            foreach (Game.PkExpeliate e in PkValues)
            {
                for (int i = 0; i < 16; i++)
                {
                    if (i < e.Name.Length)
                    {
                        Writer.Write((byte)e.Name[i]);
                    }
                    else
                        Writer.Write((byte)0);
                }
                Writer.Write((uint)e.Times);
                Writer.Write((ushort)e.LostExp);
                Writer.Write((byte)e.Level);
                Writer.Write((byte)0);
                for (int i = 0; i < 16; i++)
                {
                    if (i < e.KilledAt.Length)
                    {
                        Writer.Write((byte)e.KilledAt[i]);
                    }
                    else
                        Writer.Write((byte)0);
                }
                Writer.Write((ulong)0);
                Writer.Write((ulong)0);
                Writer.Write((uint)0);
                Writer.Write((uint)e.Potency);
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
    public class PKExplorer : Writer, Interfaces.IPacket
    {
        byte[] Buffer;
        public PKExplorer()
        {
        }
        public byte[] ToArray()
        {
            Buffer = new byte[20];
            WriteUInt16((ushort)(Buffer.Length - 8), 0, Buffer);
            WriteUInt16(2220, 2, Buffer);
            WriteUInt32(1, 4, Buffer); // Bool to show
            WriteUInt32(1/*Number of kills*/, 8, Buffer);
            WriteUInt32(2, 12, Buffer);
            return Buffer;
        }
        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }
    }
}