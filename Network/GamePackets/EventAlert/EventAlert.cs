using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets.EventAlert
{
    public class EventAlert
    {
        private byte[] mData;

        public EventAlert()
        {
            this.mData = new byte[20 + 8];
            PacketConstructor.Write(20, 0, this.mData);
            PacketConstructor.Write((ushort)0x466, 2, this.mData);
        }

        public EventAlert(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public static implicit operator byte[](EventAlert d)
        {
            return d.mData;
        }

        public uint Countdown
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

        public uint StrResID
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

        public uint UK12
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
    }
}
