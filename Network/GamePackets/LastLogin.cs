using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets
{
    public class LastLogin
    {
        private byte[] mData;
        public enum LastLoginTypes
        {
            LastLogin,
            DifferentCity,
            DifferentPlace
        }
        public LastLogin()
        {
            this.mData = new byte[12+8];
            PacketConstructor.Write(12,0, this.mData);
            PacketConstructor.Write((ushort)0x81e, 2, this.mData);
        }

        public LastLogin(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public static implicit operator byte[](LastLogin d)
        {
            return d.mData;
        }

        public uint TotalSeconds
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

        public LastLoginTypes Type
        {
            get
            {
                return (LastLoginTypes)this.mData[8];
            }
            set
            {
                this.mData[8] = (byte)value;
            }
        }

        public byte Unknown
        {
            get
            {
                return this.mData[9];
            }
            set
            {
                this.mData[9] = value;
            }
        }
    }
   

}
