using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets
{
    public class Action2
    {
        public uint ArrowID;
        private byte[] mData;

        public Action2()
        {
            this.mData = new byte[40 + 8];
            PacketConstructor.Write(40, 0, this.mData);
            PacketConstructor.Write((ushort)0x3fe, 2, this.mData);
        }

        public Action2(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public static implicit operator byte[](Action2 d)
        {
            return d.mData;
        }

        public uint Character
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

        public AttackEffect Effect
        {
            get
            {
                return (AttackEffect)BitConverter.ToUInt16(this.mData, 0x20);
            }
            set
            {
                PacketConstructor.Write((uint)value, 0x20, this.mData);
            }
        }

        public uint EffectValue
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 0x24);
            }
            set
            {
                PacketConstructor.Write(value, 0x24, this.mData);
            }
        }

        public ushort KOCount
        {
            get
            {
                return BitConverter.ToUInt16(this.mData, 0x1a);
            }
            set
            {
                PacketConstructor.Write(value, 0x1a, this.mData);
            }
        }

        public byte PushBack
        {
            get
            {
                return this.mData[0x1b];
            }
            set
            {
                this.mData[0x1b] = value;
            }
        }

        public uint ResponseValue
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 0x1c);
            }
            set
            {
                PacketConstructor.Write(value, 0x1c, this.mData);
            }
        }

        public uint Target
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
        public enum AttackEffect : ushort
        {
            Block = 1,
            CriticalStrike = 4,
            EarthResist = 0x100,
            FireResist = 0x80,
            MetalResist = 0x10,
            None = 0,
            Penetration = 2,
            StudyPoints = 0x200,
            WaterResist = 0x40,
            WoodResist = 0x20
        }
        public enum ActionType : uint
        {
            AzureShield = 0x37,
            ClaimCP = 0x27,
            CounterKill = 0x2c,
            DragonWhirl = 0x35,
            FatalStrike = 0x2d,
            Kill = 14,
            Magic = 0x18,
            MarriageAccept = 9,
            MarriageRequest = 8,
            Melee = 2,
            MerchantAccept = 40,
            MerchantRefuse = 0x29,
            MonsterHunter = 0x24,
            PushBack = 0x1b,
            Ranged = 0x1c,
            Scapegoat = 0x2b,
            ShowClaimCP = 0x25,
            SpellCast = 0x34,
            StudyPoints = 0x1a,
            Teleport = 0x31,
            WeaponSkill = 0x3e8
        }
        public ActionType Type
        {
            get
            {
                return (ActionType)BitConverter.ToUInt32(this.mData, 20);
            }
            set
            {
                PacketConstructor.Write((uint)value, 20, this.mData);
            }
        }

        public uint Value
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

        public ushort X
        {
            get
            {
                return BitConverter.ToUInt16(this.mData, 0x10);
            }
            set
            {
                PacketConstructor.Write(value, 0x10, this.mData);
            }
        }

        public ushort Y
        {
            get
            {
                return BitConverter.ToUInt16(this.mData, 0x12);
            }
            set
            {
                PacketConstructor.Write(value, 0x12, this.mData);
            }
        }
    }
}
