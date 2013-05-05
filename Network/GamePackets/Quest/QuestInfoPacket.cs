using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets.Quest
{
    public class QuestInfoPacket
    {
        private byte[] mData;
        private int offset;

        public QuestInfoPacket(byte[] d)
        {
            this.offset = 8;
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public QuestInfoPacket(int size = 8)
        {
            this.offset = 8;
            this.mData = new byte[size + 8];
            PacketConstructor.Write((ushort)size, 0, this.mData);
            PacketConstructor.Write((ushort)0x46e, 2, this.mData);
        }

        public void AddQuest(uint identifier, QuestCompleteTypes type)
        {
            byte[] array = new byte[this.mData.Length];
            this.mData.CopyTo(array, 0);
            this.mData = new byte[array.Length + 12];
            Buffer.BlockCopy(array, 0, this.mData, 0, array.Length);
            this.Count = (ushort)(this.Count + 1);
            PacketConstructor.Write((ushort)this.mData.Length, 0, this.mData);
            PacketConstructor.Write(identifier, this.offset, this.mData);
            this.offset += 4;
            PacketConstructor.Write((ushort)type, this.offset, this.mData);
            this.offset += 8;
        }

        public static implicit operator byte[](QuestInfoPacket d)
        {
            return d.mData;
        }

        public ushort Count
        {
            get
            {
                return BitConverter.ToUInt16(this.mData, 6);
            }
            private set
            {
                PacketConstructor.Write(value, 6, this.mData);
            }
        }

        public uint QuestIdentifier
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 8);
            }
            set
            {
                PacketConstructor.Write(value, 8, this.mData);
                this.Count = 1;
            }
        }

        public QuestCompleteTypes QuestType
        {
            get
            {
                return (QuestCompleteTypes)((ushort)BitConverter.ToUInt32(this.mData, 12));
            }
            set
            {
                PacketConstructor.Write((uint)value, 12, this.mData);
            }
        }

        public ushort Type
        {
            get
            {
                return BitConverter.ToUInt16(this.mData, 4);
            }
            set
            {
                PacketConstructor.Write(value, 4, this.mData);
            }
        }
    }
}
