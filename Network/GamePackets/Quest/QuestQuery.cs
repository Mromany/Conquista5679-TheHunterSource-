using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets.Quest
{
    public class QuestQuery
    {
        private byte[] mData;

        public QuestQuery()
        {
            this.mData = new byte[0x2c + 8];
            PacketConstructor.Write(0x2c, 0, this.mData);
            PacketConstructor.Write((ushort)0x46f, 2, this.mData);
        }

        public QuestQuery(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public static implicit operator byte[](QuestQuery d)
        {
            return d.mData;
        }

        public uint Identifier
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 8);
            }
            set
            {
                PacketConstructor.Write(value, 8, this.mData);
            }
        }

        public uint Type
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 4);
            }
            set
            {
                PacketConstructor.Write(value, 4, this.mData);
            }
        }

        public uint Unknown2
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 12);
            }
            set
            {
                PacketConstructor.Write(value, 12, this.mData);
            }
        }

        public uint Unknown3
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 0x10);
            }
            set
            {
                PacketConstructor.Write(value, 0x10, this.mData);
            }
        }
    }
}
