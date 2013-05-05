using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets
{
    public class StatusIconData
    {
        private byte[] mData;

        public StatusIconData()
        {
            this.mData = new byte[28 + 8];
            PacketConstructor.Write(28, 0, this.mData);
            PacketConstructor.Write((ushort)0x96a, 2, this.mData);
        }

        public StatusIconData(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public static implicit operator byte[](StatusIconData d)
        {
            return d.mData;
        }

        public uint AuraLevel
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

        public uint AuraPower
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 0x18);
            }
            set
            {
                PacketConstructor.Write(value, 0x18, this.mData);
            }
        }

        public AuraType AuraType2
        {
            get
            {
                return (AuraType)BitConverter.ToUInt32(this.mData, 12);
            }
            set
            {
                PacketConstructor.Write((uint)value, 12, this.mData);
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
                PacketConstructor.Write(value, 8, this.mData);
            }
        }
        public enum AuraDataTypes
        {
            Add = 3,
            Remove = 2
        }
        public AuraDataTypes Type
        {
            get
            {
                return (AuraDataTypes)BitConverter.ToUInt32(this.mData, 4);
            }
            set
            {
                PacketConstructor.Write((uint)value, 4, this.mData);
            }
        }
        public enum AuraType
        {
            EarthAura = 7,
            FendAura = 2,
            FireAura = 6,
            MagicDefender = 8,
            MetalAura = 3,
            TyrantAura = 1,
            WaterAura = 5,
            WoodAura = 4
        }

        public uint U20
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 20);
            }
            set
            {
                PacketConstructor.Write(value, 20, this.mData);
            }
        }
    }
}
