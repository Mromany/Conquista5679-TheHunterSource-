using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Conquer_Online_Server.Network.GamePackets
{
    public class Clan_Arena : Writer, Interfaces.IPacket
    {
        public const byte
            Abandon = 3,
            Register = 4,
            Show = 6,
            ReceiveCall = 7,
            MemCantClaim = 12,
            OnceDay = 13,
            Claimed = 15,
            ReachedMax = 17;

        byte[] Buffer;

        public Clan_Arena(bool Create)
        {
            if (Create)
            {
                Buffer = new byte[140 + 8];
                WriteUInt16(140, 0, Buffer);
                WriteUInt16(1313, 2, Buffer);
                WriteUInt16(398, 13, Buffer);
                Buffer[93] = 0x01;
                Buffer[94] = 0x01;
            }
        }

        public byte Type
        {
            get { return Buffer[4]; }
            set { Buffer[4] = value; }
        }

        public byte Type_Param
        {
            get { return Buffer[12]; }
            set { Buffer[12] = value; }
        }

        public byte Param
        {
            get { return Buffer[16]; }
            set { Buffer[16] = value; }
        }

        public uint Npc
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public string Owner
        {
            get { return Encoding.ASCII.GetString(Buffer, 20, 16); }
            set { WriteString(value, 20, Buffer); }
        }

        public string Arena
        {
            get { return Encoding.ASCII.GetString(Buffer, 56, 16); }
            set { WriteString(value, 56, Buffer); }
        }

        public ushort Occupy
        {
            get { return BitConverter.ToUInt16(Buffer, 96); }
            set { WriteUInt16(value, 96, Buffer); }
        }

        public ulong Fee
        {
            get { return BitConverter.ToUInt64(Buffer, 120); }
            set { WriteUInt64(value, 120, Buffer); }
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
