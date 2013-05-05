using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets.nobility
{
    public class NobilityIcon
    {
        private byte[] mData;
        private int Offset;
        private int rankOffset;

        public NobilityIcon(int length = 120)
        {
            this.Offset = 120;
            this.rankOffset = 8;
            this.Init(length);
        }

        public NobilityIcon(string s)
        {
            this.Offset = 120;
            this.rankOffset = 8;
            this.Init(0xa8 + s.Length);
            PacketConstructor.Write(new List<string> { s }, 120, this.mData);
        }

        public NobilityIcon(byte[] d)
        {
            this.Offset = 120;
            this.rankOffset = 8;
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }
        public enum NobilityRanks
        {
            Commoner,
            Knight,
            Lady,
            Baron,
            Baroness,
            Earl,
            Countess,
            Duke,
            Dutchess,
            Prince,
            Princess,
            Queen,
            King
        }
        public void AddMinimum(NobilityRanks Rank, ulong amount)
        {
            PacketConstructor.Write(amount, this.rankOffset, this.mData);
            PacketConstructor.Write(uint.MaxValue, this.rankOffset + 8, this.mData);
            PacketConstructor.Write((uint)Rank, this.rankOffset + 12, this.mData);
            this.rankOffset += 0x10;
        }
        public enum Gender : byte
        {
            Female = 2,
            Male = 1
        }
        public static Gender GetGender(uint body)
        {
            if ((body == 0x3eb) || (body == 0x3ec))
            {
                return Gender.Male;
            }
            return Gender.Female;
        }
        public void AddRank(uint Identifier, uint body, NobilityRanks Rank, string Name, ulong Donation, byte Pos)
        {
            byte[] array = new byte[this.mData.Length];
            this.mData.CopyTo(array, 0);
            this.mData = new byte[array.Length + 0x30];
            array.CopyTo(this.mData, 0);
            PacketConstructor.Write((ushort)this.mData.Length, 0, this.mData);
            PacketConstructor.Write(Identifier, this.Offset, this.mData);
            PacketConstructor.Write((uint)GetGender(body), this.Offset + 4, this.mData);
            PacketConstructor.Write(body, this.Offset + 8, this.mData);
            PacketConstructor.WriteNoLength(Name, this.Offset + 12, this.mData);
            PacketConstructor.Write(Donation, this.Offset + 0x20, this.mData);
            PacketConstructor.Write((uint)Rank, this.Offset + 40, this.mData);
            PacketConstructor.Write(Pos, this.Offset + 0x2c, this.mData);
            this.Offset += 0x30;
        }

        private void Init(int len)
        {
            this.mData = new byte[len + 8];
            PacketConstructor.Write((ushort)len, 0, this.mData);
            PacketConstructor.Write((ushort)0x810, 2, this.mData);
        }

        public static implicit operator byte[](NobilityIcon d)
        {
            return d.mData;
        }

        public uint dwParam
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

        public ulong MinimumDonation
        {
            get
            {
                return BitConverter.ToUInt64(this.mData, 8);
            }
            set
            {
                PacketConstructor.Write(value, 8, this.mData);
                this.mData[20] = 60;
                PacketConstructor.Write(uint.MaxValue, 0x18, this.mData);
            }
        }

        public byte ShownMin
        {
            get
            {
                return this.mData[10];
            }
            set
            {
                this.mData[10] = value;
            }
        }

        public string String
        {
            get
            {
                return Encoding.ASCII.GetString(this.mData, 0x22, this.mData[0x21]);
            }
        }
        public enum NobilityTypes : uint
        {
            Donate = 1,
            Icon = 3,
            Minimum = 4,
            Rankings = 2
        }

        public NobilityTypes Type
        {
            get
            {
                return (NobilityTypes)BitConverter.ToUInt32(this.mData, 4);
            }
            set
            {
                PacketConstructor.Write((uint)value, 4, this.mData);
            }
        }

        public uint wParam1
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

        public uint wParam2
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
