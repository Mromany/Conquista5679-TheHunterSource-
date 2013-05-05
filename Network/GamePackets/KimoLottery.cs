using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets
{
    internal class Lottery
    {
        private byte[] mData;
        public enum LotteryTypes
        {
            Accept = 0,
            AddJade = 1,
            Continue = 2,
            ShowGUI = 0x103
        }
        public Lottery()
        {
            this.mData = new byte[0x18 + 8];
            PacketConstructor.Write(0x18, 0, this.mData);
            PacketConstructor.Write((ushort)0x522, 2, this.mData);
            PacketConstructor.Write((byte)1, 6, this.mData);
        }

        public Lottery(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public static implicit operator byte[](Lottery d)
        {
            return d.mData;
        }

        public byte AddJadeChances
        {
            get
            {
                return this.mData[11];
            }
            set
            {
                this.mData[11] = value;
            }
        }

        public byte Chances
        {
            get
            {
                return this.mData[12];
            }
            set
            {
                this.mData[12] = value;
            }
        }

        public Game.Enums.Color Color
        {
            get
            {
                return (Game.Enums.Color)((byte)BitConverter.ToUInt16(this.mData, 10));
            }
            set
            {
                PacketConstructor.Write((ushort)value, 10, this.mData);
            }
        }

        public byte Plus
        {
            get
            {
                return this.mData[9];
            }
            set
            {
                PacketConstructor.Write(value, 9, this.mData);
            }
        }

        public uint Prize
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

        public byte SocketOne
        {
            get
            {
                return this.mData[7];
            }
            set
            {
                PacketConstructor.Write(value, 7, this.mData);
            }
        }

        public byte SocketTwo
        {
            get
            {
                return this.mData[8];
            }
            set
            {
                PacketConstructor.Write(value, 8, this.mData);
            }
        }

        public LotteryTypes Type
        {
            get
            {
                return (LotteryTypes)BitConverter.ToUInt16(this.mData, 4);
            }
            set
            {
                PacketConstructor.Write((ushort)value, 4, this.mData);
            }
        }
    }
}
