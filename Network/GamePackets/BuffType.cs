﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public enum BuffType : byte
    {
        Absorb = 0x41,
        AttackDown = 2,
        AttackSpeedDown = 8,
        AttackSpeedUp = 7,
        AttackUp = 1,
        BlockDown = 0x22,
        BlockUp = 0x21,
        CritDown = 0x10,
        CritSpellDown = 0x12,
        CritSpellUp = 0x11,
        CritUp = 15,
        DefenceAdd = 0x66,
        DefenseDown = 4,
        DefenseUp = 3,
        DexterityAdd = 0x67,
        DodgeAdd = 0x68,
        DontUseArrows = 0x5b,
        EarthResistDown = 30,
        EarthResistUp = 0x1d,
        ExpDown = 6,
        ExpUp = 5,
        FireResistDown = 0x1c,
        FireResistUp = 0x1b,
        Fly = 90,
        HitpointsAdd = 0x69,
        ImmuneToMagic = 0x42,
        ImmunityDown = 20,
        ImmunityUp = 0x13,
        Invulnerable = 70,
        MagicDamageAdd = 0x6a,
        MagicDefenceAdd = 0x6b,
        MaxAttackUpAdd = 0x65,
        MDefenseDown = 0x24,
        MDefenseUp = 0x23,
        MetalResistDown = 0x18,
        MetalResistUp = 0x17,
        MinAttackUpAdd = 100,
        MoveRemove = 0x3d,
        MoveSpeedDown = 14,
        MoveSpeedUp = 13,
        Poisioned = 50,
        PreventAttack = 0x37,
        PreventDrugs = 0x33,
        PreventJump = 0x34,
        PreventRevive = 0x36,
        PreventWalk = 0x35,
        SpellSpeedDown = 10,
        SpellSpeedUp = 9,
        StaminaCostDown = 0x26,
        StaminaCostUp = 0x25,
        StaminaDown = 12,
        StaminaUp = 11,
        Transformation = 60,
        WaterResistDown = 0x20,
        WaterResistUp = 0x1f,
        WoodResistDown = 0x1a,
        WoodResistUp = 0x19
    }
}
