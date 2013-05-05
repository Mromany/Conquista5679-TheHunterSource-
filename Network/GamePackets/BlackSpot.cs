using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class BlackSpot
    {
        private byte[] mData;

        public uint Remove
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 4);
            }
            set
            {
                Writer.WriteUInt32(value, 4, this.mData);
            }
        }

        public uint Identifier
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 8);
            }
            set
            {
                Writer.WriteUInt32(value, 8, this.mData);
            }
        }

        public BlackSpot(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo((Array)this.mData, 0);
        }

        public BlackSpot()
        {
            this.mData = new byte[12+8];
            Writer.WriteUInt16(12, 0, this.mData);
            Writer.WriteUInt16(0x821, 2, this.mData);
        }

        public static implicit operator byte[](BlackSpot d)
        {
            return d.mData;
        }
    }
}
