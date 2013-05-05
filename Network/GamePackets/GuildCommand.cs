using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class GuildCommand : Writer, Interfaces.IPacket
    {
        public const uint
                    JoinRequest = 1,
                    InviteRequest = 2,
                    Quit = 3,
                    Info = 6,
                    Allied = 7,
                    Neutral1 = 8,
                    Enemied = 9,
                    Neutral2 = 10,
                    DonateSilvers = 11,
                    Refresh = 12,
                    Disband = 19,
                    DonateConquerPoints = 20,
                    Bulletin = 27,
                    Discharge = 30,
                    Promote = 37;

        private byte[] Buffer;
        public GuildCommand(bool Create)
        {
            if (Create)
            {
                Buffer = new byte[8 + 28];
                WriteUInt16(28, 0, Buffer);
                WriteUInt16(1107, 2, Buffer);
            }
        }

        public uint Type
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { WriteUInt32(value, 4, Buffer); }
        }

        public uint dwParam
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public void Deserialize(byte[] Data)
        {
            Buffer = Data;
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
        public class GuildProfilePacket : Writer, Interfaces.IPacket
    {
        private byte[] Packet;
        public GuildProfilePacket() { }
        public uint Silver
        {
            get { return BitConverter.ToUInt32(Packet, 8); }
            set { WriteUInt32(value, 8, Packet); }
        }

        public uint Pk
        {
            get { return BitConverter.ToUInt32(Packet, 20); }
            set { WriteUInt32(value, 20, Packet); }
        }

        public uint Cps
        {
            get { return BitConverter.ToUInt32(Packet, 12); }
            set { WriteUInt32(value, 12, Packet); }
        }

        public uint Guide
        {
            get { return BitConverter.ToUInt32(Packet, 16); }
            set { WriteUInt32(value, 16, Packet); }
        }

        public uint Arsenal
        {
            get { return BitConverter.ToUInt32(Packet, 24); }
            set { WriteUInt32(value, 24, Packet); }
        }

        public uint Rose
        {
            get { return BitConverter.ToUInt32(Packet, 28); }
            set { WriteUInt32(value, 28, Packet); }
        }

        public uint Orchid
        {
            get { return BitConverter.ToUInt32(Packet, 32); }
            set { WriteUInt32(value, 32, Packet); }
        }

        public uint Lily
        {
            get { return BitConverter.ToUInt32(Packet, 36); }
            set { WriteUInt32(value, 36, Packet); }
        }

        public uint Tulip
        {
            get { return BitConverter.ToUInt32(Packet, 40); }
            set { WriteUInt32(value, 40, Packet); }
        }

        public uint HistorySilvers
        {
            get { return BitConverter.ToUInt32(Packet, 44); }
            set { WriteUInt32(value, 44, Packet); }
        }

        public uint HistoryCps
        {
            get { return BitConverter.ToUInt32(Packet, 48); }
            set { WriteUInt32(value, 48, Packet); }
        }

        public uint HistoryGuide
        {
            get { return BitConverter.ToUInt32(Packet, 52); }
            set { WriteUInt32(value, 52, Packet); }
        }

        public uint HistoryPk
        {
            get { return BitConverter.ToUInt32(Packet, 56); }
            set { WriteUInt32(value, 56, Packet); }
        }

        public void Send(Client.GameState client)
        {
            client.Send(Packet);
        }
        public void Deserialize(byte[] Data)
        {
            this.Packet = Data;
        }

        public byte[] ToArray()
        {
            return Packet;
        }
    }
}

