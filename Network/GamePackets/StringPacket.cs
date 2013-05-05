using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets
{
    public class StringPacket
    {
        private byte[] mData;
        private byte offset;

        public StringPacket()
        {
            this.offset = 10;
            this.mData = new byte[11 + 8];
            PacketConstructor.Write(11, 0, this.mData);
            PacketConstructor.Write((ushort)0x3f7, 2, this.mData);
        }

        public StringPacket(string s)
        {
            this.offset = 10;
            this.mData = new byte[11 + 8];
            PacketConstructor.Write(11, 0, this.mData);
            PacketConstructor.Write((ushort)0x3f7, 2, this.mData);
            this.AddString(s);
        }

        public StringPacket(byte[] d)
        {
            this.offset = 10;
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public void AddString(string s)
        {
            byte[] array = new byte[this.mData.Length];
            this.mData.CopyTo(array, 0);
            this.mData = new byte[(array.Length + s.Length) + 1];
            Buffer.BlockCopy(array, 0, this.mData, 0, array.Length);
            PacketConstructor.Write((ushort)this.mData.Length, 0, this.mData);
            this.Count = (byte)(this.Count + 1);
            PacketConstructor.Write(s, this.offset, this.mData);
            this.offset = (byte)(this.offset + ((byte)(s.Length + 1)));
        }

        public static implicit operator byte[](StringPacket d)
        {
            return d.mData;
        }

        public byte Count
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

        public string String
        {
            get
            {
                return Encoding.ASCII.GetString(this.mData, 11, this.mData[10]);
            }
        }
        public enum StringType
        {
            Effect = 10,
            EffectPossibly = 9,
            EndGamble = 0x12,
            GuildAllies = 0x15,
            GuildEnemies = 0x16,
            GuildList = 11,
            GuildName = 3,
            Sound = 20,
            Spouse = 6,
            StartGamble = 0x11,
            Unknown = 13,
            ViewEquipSpouse = 0x10,
            WhisperDetails = 0x1a
        }
        public StringType Type
        {
            get
            {
                return (StringType)this.mData[8];
            }
            set
            {
                this.mData[8] = (byte)value;
            }
        }

        public ushort X
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

        public ushort Y
        {
            get
            {
                return BitConverter.ToUInt16(this.mData, 6);
            }
            set
            {
                PacketConstructor.Write(value, 6, this.mData);
            }
        }
    }
}
