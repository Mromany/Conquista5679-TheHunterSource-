using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Interfaces;

namespace PhoenixProject.Network.GamePackets.Quest
{
    [Flags]
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
    public enum HitType
    {
        Hurt,
        HealHP,
        HealMP,
        NoDamage
    }
    public class Target
    {
        public uint Damage;
        public HitType DamageType;
        public AttackEffect Effect;
        public uint EffectValue;
        public bool Hit;
        public IMapObject Obj;
        public ushort X;
        public ushort Y;
    }
}
