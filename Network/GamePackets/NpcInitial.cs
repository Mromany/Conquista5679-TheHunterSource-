using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets
{
    public class NpcInitial
    {
        private byte[] mData;

        public NpcInitial()
        {
            this.mData = new byte[0x10 + 8];
            PacketConstructor.Write(0x10, 0, this.mData);
            PacketConstructor.Write((ushort)0x7ef, 2, this.mData);
        }

        public NpcInitial(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public static implicit operator byte[](NpcInitial d)
        {
            return d.mData;
        }

        public uint Furniture
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

        public uint Identifier
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

        public ushort Lookface
        {
            get
            {
                return BitConverter.ToUInt16(this.mData, 14);
            }
            set
            {
                PacketConstructor.Write(value, 14, this.mData);
            }
        }
        public enum NpcModes
        {
            Close = 6,
            Delete = 3,
            Place = 5,
            Talk = 0
        }
        public NpcModes Mode
        {
            get
            {
                return (NpcModes)BitConverter.ToUInt16(this.mData, 12);
            }
            set
            {
                PacketConstructor.Write((ushort)value, 12, this.mData);
            }
        }
    }
}
