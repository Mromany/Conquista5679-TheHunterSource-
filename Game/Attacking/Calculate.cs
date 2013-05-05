using System;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game.Attacking
{
    public class Calculate//Coded By Kimo
    {
        public static Boolean ChanceSuccess(Double Chance, Double offset = 0)
        {
            Random Rand = new Random();
            if (Chance <= 0) return false;
            else if (Chance >= 100) return true;
            return ((float)Rand.Next(1, 1000000) / 10000d >= (100d + offset) - Chance);
        }

        //Orignal Hammmy Mele
        public static uint Melee(Entity attacker, Entity attacked, ref Attack Packet)
        {

            
            if (attacked.EntityFlag == EntityFlag.Player)
            {
                BlessEffect.Effect(attacked);
            }
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                GemEffect.Effect(attacker);
            }
             

            int Damage = 0;
            Boolean CritImmune = false;
            Boolean canBT = false;

            if (attacker.EntityFlag == EntityFlag.Monster)
                if (attacked.EntityFlag == EntityFlag.Player)
                    if (ServerBase.Kernel.Rate(Math.Min(60, attacked.Dodge + 30)))
                        return 0;

            Durability(attacker, attacked, null);

            if (attacked.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                return 1;
            if (!attacker.Transformed)
                Damage = ServerBase.Kernel.Random.Next(Math.Min((int)attacker.MinAttack, (int)attacker.MaxAttack), Math.Max((int)attacker.MinAttack, (int)attacker.MaxAttack) + 1);
            else
                Damage = ServerBase.Kernel.Random.Next((int)attacker.TransformationMinAttack, (int)attacker.TransformationMaxAttack + 1);
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (attacker.BattlePower < attacked.BattlePower)
                    canBT = true;
            }
            else canBT = false;
            if (canBT)
            {
                if (canBT)
                {
                    if (ChanceSuccess((float)attacked.Counteraction / 100f))
                        canBT = false;
                }
                if (canBT)
                {
                    if (ChanceSuccess((float)attacker.Breaktrough / 100f))
                    {
                        Damage = (Int32)attacker.MaxAttack + 3000;
                        Packet.Effect1 |= Attack.AttackEffects1.Penetration;
                    }
                }
            }
            if (attacker.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                if (!attacker.Transformed && Damage > 1)
                    Damage = (int)(Damage * 1.30);

            if (attacked.EntityFlag == EntityFlag.Monster)
            {
                if (attacked.MapID < 1351 || attacked.MapID > 1354)
                    Damage = (int)(Damage * (1 + (GetLevelBonus(attacker.Level, attacked.Level) * 0.08)));
            }
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (attacked.EntityFlag == EntityFlag.Monster)
                {
                    if (attacked.MapID < 1351 || attacked.MapID > 1354)
                        if (!attacker.Owner.Equipment.Free(4) && !attacker.Owner.Equipment.Free(5))
                            Damage = (int)(Damage * 1.5);
                }
                if (attacked.EntityFlag == EntityFlag.Monster)
                    if (attacked.MapID < 1351 || attacked.MapID > 1354)
                        Damage = (int)(Damage * AttackMultiplier(attacker, attacked));
                if (attacker.OnSuperman())
                    if (attacked.EntityFlag == EntityFlag.Monster)
                        Damage *= 10;
                    
                if (attacker.OnFatalStrike())
                    if (attacked.EntityFlag == EntityFlag.Monster)
                        Damage *= 5;
            }
            if (!attacked.Transformed)
            {
                if (attacked.ContainsFlag(Network.GamePackets.Update.Flags.MagicShield))
                {
                    if (attacked.ShieldTime > 0)
                        Damage -= 1000;
                    else
                        Damage -= (ushort)(attacked.Defence * attacked.MagicShieldIncrease);
                }
                else
                {
                    Damage -= attacked.Defence;
                }
            }
            else
                Damage -= attacked.TransformationDefence;

            
            if (ServerBase.Kernel.Rate(5))
            {
                if (attacker.EntityFlag == EntityFlag.Player)
                {
                    if (attacker.Owner.BlessTime > 0)
                    {
                        Damage *= 2;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacker.Owner.SendScreen(str, true);
                    }
                }
            }

            if (ServerBase.Kernel.Rate(5))
            {
                if (attacked.EntityFlag == EntityFlag.Player)
                {
                    if (attacked.Owner.BlessTime > 0)
                    {
                        Damage = 1;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacked.Owner.SendScreen(str, true);
                    }
                }
            }

            Damage = RemoveExcessDamage(Damage, attacker, attacked);

            Damage += attacker.PhysicalDamageIncrease;
            Damage -= attacked.PhysicalDamageDecrease;
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (!CritImmune)
                {
                    if (ChanceSuccess((float)attacker.CriticalStrike / 100f))
                    {
                        Packet.Effect1 |= Attack.AttackEffects1.CriticalStrike;
                        Damage = (Int32)Math.Floor((float)Damage * 1.1);
                    }
                }
            }
            if (attacked.EntityFlag == EntityFlag.Player)
            {
                if (ChanceSuccess((float)attacked.Block / 100f))
                {
                    Packet.Effect1 |= Attack.AttackEffects1.Block;
                    Damage = (Int32)Math.Floor((float)Damage / 2);
                }
            }
            try
            {
                if (attacked.EntityFlag == EntityFlag.Player && (attacker.BattlePower < attacked.BattlePower))
                {
                    int sub = attacked.BattlePower - attacker.BattlePower;
                    
                    if (sub == 1)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .50);
                    }
                    if (sub == 2)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .49);
                    }
                    if (sub == 3)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .48);
                    }
                    if (sub == 4)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .47);
                    }
                    if (sub == 5)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .46);
                    }
                    if (sub == 6)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .45);
                    }
                    if (sub == 7)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .44);
                    }
                    if (sub == 8)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .43);
                    }
                    if (sub == 9)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .42);
                    }
                    if (sub == 10)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .41);
                    }
                    if (sub == 11)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .40);
                    }
                    if (sub == 12)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .39);
                    }
                    if (sub == 13)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .38);
                    }
                    if (sub == 14)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .37);
                    }
                    if (sub == 15)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .36);
                    }
                    if (sub == 16)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .35);
                    }
                    if (sub == 17)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .34);
                    }
                    if (sub == 18)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .33);
                    }
                    if (sub == 19)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .32);
                    }
                    if (sub == 20)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .31);
                    }
                    if (sub == 21)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .30);
                    }
                    if (sub == 22)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .29);
                    }
                    if (sub == 23)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .28);
                    }
                    if (sub == 24)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .27);
                    }
                    if (sub == 25)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .26);
                    }
                    if (sub == 26)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .25);
                    }
                    if (sub == 27)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .24);
                    }
                    if (sub == 28)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .23);
                    }
                    if (sub == 29)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .22);
                    }
                    if (sub == 30)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .21);
                    }
                    if (sub == 31)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .20);
                    }
                    if (sub == 32)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .19);
                    }
                    if (sub == 33)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .18);
                    }
                    if (sub == 34)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .17);
                    }
                    if (sub == 35)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .16);
                    }
                    if (sub == 36)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .15);
                    }
                    if (sub == 37)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .14);
                    }
                    if (sub == 38)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .13);
                    }
                    if (sub == 39)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .12);
                    }
                    if (sub == 40)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .11);
                    }
                    if (sub == 41)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .10);
                    }
                    if (sub > 41)
                    {
                        Damage = 1;
                    }
                }
            }
            catch (Exception ex) { Program.SaveException(ex); }

            
            if (attacked.ContainsFlag2(Network.GamePackets.Update.Flags2.AzureShield))
            {

                if (attacked.AzureDamage >= Damage)
                {
                    // Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    attacked.AzureDamage -= Damage;
                    int sec = 60 - (Time32.Now - attacked.MagicShieldStamp).AllSeconds();
                   // attacked.Owner.Send(ServerBase.Constants.Shield(attacked.AzureDamage, sec));
                    SyncPacket packet4 = new SyncPacket
                    {
                        Identifier = attacked.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)attacked.StatusFlag,
                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x5d,
                        Time = (uint)sec,
                        Value = (uint)attacked.AzureDamage,
                        Level = 4
                    };
                    attacked.Owner.Send((byte[])packet4);
                    // Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    return 0;
                }
                else
                {
                    //Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    Damage -= attacked.AzureDamage;
                    attacked.AzureDamage = 0;
                    attacked.RemoveFlag2(Update.Flags2.AzureShield);
                    SyncPacket packet4 = new SyncPacket
                    {
                        Identifier = attacked.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)attacked.StatusFlag,
                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x5d,
                        Time = 0,
                        Value = 0,
                        Level = 4
                    };
                    attacked.Owner.Send((byte[])packet4);
                    //Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    return (uint)Damage;

                }

            }

            ///////////////////////////////

            if (attacked.EntityFlag == EntityFlag.Monster)
            {
                if (Damage >= 700 * attacked.MaxHitpoints)
                    Damage = (int)(700 * attacked.MaxHitpoints);
            }
            
            Damage = (int)(Damage*1.8);//kimoz
            if (Damage <= 0)
                Damage = 1;

           
           // if (attacker.Class > 140)
                //Damage = 1;

            AutoRespone(attacker, attacked, ref Damage);
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;
            return (uint)Damage;
        }
        public static uint Melee(Entity attacker, Entity attacked, Database.SpellInformation spell, ref Attack Packet)
        {
           
            int Damage = 0;
            Boolean CritImmune = false;
            Boolean canBT = false;
            if (attacker.EntityFlag == EntityFlag.Monster)
                if (attacked.EntityFlag == EntityFlag.Player)
                    if (ServerBase.Kernel.Rate(Math.Min(60, attacked.Dodge + 30)))
                        return 0;
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                GemEffect.Effect(attacker);
            }
            Durability(attacker, attacked, null);

            if (attacked.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                return 1;
            if (!attacker.Transformed)
                Damage = ServerBase.Kernel.Random.Next(Math.Min((int)attacker.MinAttack, (int)attacker.MaxAttack), Math.Max((int)attacker.MinAttack, (int)attacker.MaxAttack) + 1);
            else
                Damage = ServerBase.Kernel.Random.Next((int)attacker.TransformationMinAttack, (int)attacker.TransformationMaxAttack + 1);
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (attacker.BattlePower < attacked.BattlePower)
                    canBT = true;
            }
            else canBT = false;
            if (canBT)
            {
                if (canBT)
                {
                    if (ChanceSuccess((float)attacked.Counteraction / 100f))
                        canBT = false;
                }
                if (canBT)
                {
                    if (ChanceSuccess((float)attacker.Breaktrough / 100f) && spell.ID != 1115)
                    {
                        Damage = (Int32)attacker.MaxAttack + 3000;
                        Packet.Effect1 |= Attack.AttackEffects1.Penetration;
                    }
                }
            }
            if (attacker.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                if (!attacker.Transformed && Damage > 1)
                    Damage = (int)(Damage * 1.30);

            if (attacked.EntityFlag == EntityFlag.Monster)
            {
                if (attacked.MapID < 1351 || attacked.MapID > 1354)
                    Damage = (int)(Damage * (1 + (GetLevelBonus(attacker.Level, attacked.Level) * 0.08)));
            }
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (attacked.EntityFlag == EntityFlag.Monster)
                {
                    if (attacked.MapID < 1351 || attacked.MapID > 1354)
                        if (!attacker.Owner.Equipment.Free(4) && !attacker.Owner.Equipment.Free(5))
                            Damage = (int)(Damage * 1.5);
                }
                if (attacked.EntityFlag == EntityFlag.Monster)
                    if (attacked.MapID < 1351 || attacked.MapID > 1354)
                        Damage = (int)(Damage * AttackMultiplier(attacker, attacked));
                if (attacker.OnSuperman())
                    if (attacked.EntityFlag == EntityFlag.Monster)
                        Damage *= 10;
                  

                if (attacker.OnFatalStrike())
                    if (attacked.EntityFlag == EntityFlag.Monster)
                        Damage *= 5;
            }
            if (!attacked.Transformed)
            {
                if (attacked.ContainsFlag(Network.GamePackets.Update.Flags.MagicShield))
                {
                    if (attacked.ShieldTime > 0)
                        Damage -= 1000;
                    else
                        Damage -= (ushort)(attacked.Defence * attacked.MagicShieldIncrease);
                }
                else
                {
                    Damage -= attacked.Defence;
                }
            }
            else
                Damage -= attacked.TransformationDefence;

            if (ServerBase.Kernel.Rate(5))
            {
                if (attacker.EntityFlag == EntityFlag.Player)
                {
                    if (attacker.Owner.BlessTime > 0)
                    {
                        Damage *= 2;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacker.Owner.SendScreen(str, true);
                    }
                }
            }

            if (ServerBase.Kernel.Rate(5))
            {
                if (attacked.EntityFlag == EntityFlag.Player)
                {
                    if (attacked.Owner.BlessTime > 0)
                    {
                        Damage = 1;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacked.Owner.SendScreen(str, true);
                    }
                }
            }
            if (spell.ID == 6000 && attacked.EntityFlag == EntityFlag.Monster)
            {
                if (spell.PowerPercent != 0)
                    Damage = (int)(Damage * spell.PowerPercent);
            }
            else if (spell.ID != 6000)
            {
                if (spell.PowerPercent != 0)
                    Damage = (int)(Damage * spell.PowerPercent);
            }

            Damage = RemoveExcessDamage(Damage, attacker, attacked);

            Damage += attacker.PhysicalDamageIncrease;
            Damage -= attacked.PhysicalDamageDecrease;
            if (attacker.EntityFlag == EntityFlag.Player && spell.ID != 1115)
            {
                if (!CritImmune)
                {
                    if (ChanceSuccess((float)attacker.CriticalStrike / 200f) && spell.ID != 1115)
                    {
                        Packet.Effect1 |= Attack.AttackEffects1.CriticalStrike;
                        Damage = (Int32)Math.Floor((float)Damage * 1.2);
                    }
                }
            }
            if (attacked.EntityFlag == EntityFlag.Monster)
            {
                if (Damage >= 700 * attacked.MaxHitpoints)
                    Damage = (int)(700 * attacked.MaxHitpoints);
            }

            //Damage += Damage;
           Damage = (int)(Damage*1.8);//kimoz
            if (spell.ID == 1115 )
                Damage = Damage / 2;
            if (spell.ID == 11110)
                Damage = (int)(Damage / 1.5);

            /*if (spell.ID == 10315 || spell.ID == 11190)
                Damage = Damage / 2;*/
            

            if (Damage <= 0)
                Damage = 1;
           
            AutoRespone(attacker, attacked, ref Damage);
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;
            try
            {
                if (attacked.EntityFlag == EntityFlag.Player && (attacker.BattlePower < attacked.BattlePower))
                {
                    int sub = attacked.BattlePower - attacker.BattlePower;
                    
                    if (sub == 1)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .50);
                    }
                    if (sub == 2)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .49);
                    }
                    if (sub == 3)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .48);
                    }
                    if (sub == 4)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .47);
                    }
                    if (sub == 5)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .46);
                    }
                    if (sub == 6)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .45);
                    }
                    if (sub == 7)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .44);
                    }
                    if (sub == 8)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .43);
                    }
                    if (sub == 9)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .42);
                    }
                    if (sub == 10)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .41);
                    }
                    if (sub == 11)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .40);
                    }
                    if (sub == 12)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .39);
                    }
                    if (sub == 13)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .38);
                    }
                    if (sub == 14)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .37);
                    }
                    if (sub == 15)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .36);
                    }
                    if (sub == 16)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .35);
                    }
                    if (sub == 17)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .34);
                    }
                    if (sub == 18)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .33);
                    }
                    if (sub == 19)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .32);
                    }
                    if (sub == 20)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .31);
                    }
                    if (sub == 21)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .30);
                    }
                    if (sub == 22)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .29);
                    }
                    if (sub == 23)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .28);
                    }
                    if (sub == 24)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .27);
                    }
                    if (sub == 25)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .26);
                    }
                    if (sub == 26)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .25);
                    }
                    if (sub == 27)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .24);
                    }
                    if (sub == 28)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .23);
                    }
                    if (sub == 29)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .22);
                    }
                    if (sub == 30)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .21);
                    }
                    if (sub == 31)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .20);
                    }
                    if (sub == 32)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .19);
                    }
                    if (sub == 33)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .18);
                    }
                    if (sub == 34)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .17);
                    }
                    if (sub == 35)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .16);
                    }
                    if (sub == 36)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .15);
                    }
                    if (sub == 37)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .14);
                    }
                    if (sub == 38)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .13);
                    }
                    if (sub == 39)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .12);
                    }
                    if (sub == 40)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .11);
                    }
                    if (sub == 41)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .10);
                    }
                    if (sub > 41)
                    {
                        Damage = 1;
                    }
                }
            }
            catch (Exception ex) { Program.SaveException(ex); }
            
            if (attacked.ContainsFlag2(Network.GamePackets.Update.Flags2.AzureShield))
            {

                if (attacked.AzureDamage >= Damage)
                {
                    // Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    attacked.AzureDamage -= Damage;
                    int sec = 60 - (Time32.Now - attacked.MagicShieldStamp).AllSeconds();
                    //attacked.Owner.Send(ServerBase.Constants.Shield(attacked.AzureDamage, sec));
                    SyncPacket packet4 = new SyncPacket
                    {
                        Identifier = attacked.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)attacked.StatusFlag,
                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x5d,
                        Time = (uint)sec,
                        Value = (uint)attacked.AzureDamage,
                        Level = 4
                    };
                    attacked.Owner.Send((byte[])packet4);
                    //Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    return 0;
                }
                else
                {
                    // Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    Damage -= attacked.AzureDamage;
                    attacked.AzureDamage = 0;
                    attacked.RemoveFlag2(Update.Flags2.AzureShield);
                    SyncPacket packet4 = new SyncPacket
                    {
                        Identifier = attacked.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)attacked.StatusFlag,
                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x5d,
                        Time = (uint)0,
                        Value = (uint)0,
                        Level = 4
                    };
                    attacked.Owner.Send((byte[])packet4);
                    // Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    return (uint)Damage;

                }

            }
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;
            return (uint)Damage;
        }
        public static uint Melee(Entity attacker, SobNpcSpawn attacked, ref Attack Packet)
        {

            int Damage = 0;
            Boolean CritImmune = false;

            Durability(attacker, null, null);
            if (!attacker.Transformed)
                Damage = ServerBase.Kernel.Random.Next((int)attacker.MinAttack, (int)attacker.MaxAttack + 1);
            else
                Damage = ServerBase.Kernel.Random.Next(Math.Min((int)attacker.MinAttack, (int)attacker.MaxAttack), Math.Max((int)attacker.MinAttack, (int)attacker.MaxAttack) + 1);
            if (attacker.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                if (!attacker.Transformed)
                    Damage = (int)(Damage * 1.30);
            Damage += attacker.PhysicalDamageIncrease;
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                GemEffect.Effect(attacker);
            }
            if (ServerBase.Kernel.Rate(5))
            {
                if (attacker.EntityFlag == EntityFlag.Player)
                {
                    if (attacker.Owner.BlessTime > 0)
                    {
                        Damage *= 2;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacker.Owner.SendScreen(str, true);
                    }
                }
            }
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (!CritImmune)
                {
                    if (ChanceSuccess((float)attacker.CriticalStrike / 100f))
                    {
                        Packet.Effect1 |= Attack.AttackEffects1.CriticalStrike;
                        Damage = (Int32)Math.Floor((float)Damage * 1.2);
                    }
                }
            }

           
           Damage = (int)(Damage*1.8);//kimoz

            if (Damage <= 0)
                Damage = 1;
            
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;


            return (uint)Damage;
        }
        public static bool Miss(int Percent)
        {
            if (Percent >= 100)
                return false;

            return ChanceSuccess(Percent, 100);
        }
        public static uint Magic(Entity Attacker, Entity Attacked, Database.SpellInformation SInfo, ref Attack Packet)
        {
            if (Attacked.EntityFlag == EntityFlag.Player)
            {
                BlessEffect.Effect(Attacked);
            }
            if (Attacker.EntityFlag == EntityFlag.Player)
            {
                GemEffect.Effect(Attacker);
            }
            Boolean CritImmune = false;
            switch (Attacked.EntityFlag)
            {
                case EntityFlag.Player:
                    {
                        if (Attacker.EntityFlag == EntityFlag.Monster)
                        {
                            if (!Attacked.ContainsFlag(Update.Flags.Stigma) && Attacker.Name == "StigGuard")
                            {
                                Attacked.AddFlag(Update.Flags.Stigma);
                                Attacked.StigmaStamp = Time32.Now;
                                Attacked.StigmaIncrease = 25;
                                Attacked.StigmaTime = (byte)20;
                                if (Attacked.EntityFlag == EntityFlag.Player)
                                    Attacked.Owner.Send(ServerBase.Constants.Stigma(30, 20));
                                return 0;
                            }
                            if (Attacked.ContainsFlag(Update.Flags.Stigma) && Attacker.Name == "StigGuard")
                            {

                                return 0;
                            }
                            if (Attacked.Dead && Attacker.Name == "RevGuard")
                            {
                                Attacked.Owner.Entity.Action = PhoenixProject.Game.Enums.ConquerAction.None;
                                Attacked.Owner.ReviveStamp = Time32.Now;
                                Attacked.Owner.Attackable = false;

                                Attacked.Owner.Entity.TransformationID = 0;
                                Attacked.Owner.Entity.RemoveFlag(Update.Flags.Dead);
                                Attacked.Owner.Entity.RemoveFlag(Update.Flags.Ghost);
                                Attacked.Owner.Entity.Hitpoints = Attacked.Owner.Entity.MaxHitpoints;

                                Attacked.Ressurect();
                                Attacked.Owner.Send(new MapStatus() { BaseID = (ushort)Attacked.Owner.Map.BaseID, ID = (uint)Attacked.Owner.Map.ID, Status = Database.MapsTable.MapInformations[Attacked.Owner.Map.ID].Status, Weather = Database.MapsTable.MapInformations[Attacked.Owner.Map.ID].Weather });
                                //Attacked.Owner.Entity.Teleport(Attacked.Owner.Entity.MapID, Attacked.Owner.Entity.X, Attacked.Owner.Entity.Y);
                                return 0;
                            }
                        }
                        Int32 Damage = 0;
                        Int32 Defence = 0;
                        if (ChanceSuccess((float)Attacked.Immunity / 100f))
                            CritImmune = true;
                        Int32 Fan = 0, Tower = 0;
                        if (Attacker.EntityFlag == EntityFlag.Player)
                            Fan = Attacker.getFan(true);
                        if (Attacked.EntityFlag == EntityFlag.Player)
                            Tower = Attacked.getTower(true);
                        Damage = (int)Attacker.MagicAttack;
                        
                        if (Attacked.EntityFlag == EntityFlag.Player)
                            if (Attacked.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                                return 1;

                        if (Miss(SInfo.Percent))
                            return 0;

                        Defence = (Int32)Attacked.MagicDefence;
                        Defence = (Int32)Attacked.MagicDamageDecrease;

                      
                       
                        if (Attacked.EntityFlag == EntityFlag.Player)
                        {
                            if (Attacked.Reborn == 1)
                                Damage = (Int32)(Damage * 0.69);
                            if (Attacked.Reborn == 2)
                                Damage = (Int32)(Damage * 0.49);
                        }
                        Damage += Fan;
                        if (Attacker.EntityFlag == EntityFlag.Player)
                            Damage += Attacker.MagicDamageIncrease;
                        //UInt32 bpdmage = 0, bpdefence = 0;
                        //if (Attacked.EntityFlag == EntityFlag.Player)
                            //bpdefence = (UInt32)(Attacked.BattlePower * 3.5);
                        //Damage -= (int)bpdefence;
                        //Damage -= Attacked.MagicDamageDecrease;
                        Damage -= Defence;
                        Damage -= Tower;
                        //if (Damage <= 1) Damage = 1;

                        //return (UInt32)Damage;

                        
                        //if (Damage <= 1) Damage = 1;
                        //return (UInt32)Damage;
                        //the new block offers us the behavior of the melee response.

                        if (!CritImmune)
                        {
                            if (ChanceSuccess((float)Attacker.SkillCStrike / 100f))
                            {
                                Packet.Effect1 |= Attack.AttackEffects1.CriticalStrike;
                                Damage = (Int32)Math.Floor((float)Damage * 1.3);
                            }
                        }
                        if (Attacked.EntityFlag == EntityFlag.Player)
                        {
                            if (ChanceSuccess((float)Attacked.Block / 100f))
                            {
                                Packet.Effect1 |= Attack.AttackEffects1.Block;
                                Damage = (Int32)Math.Floor((float)Damage / 2);
                            }
                        }

                        if (Damage <= 0)
                            Damage = 1;
                        Damage = Damage / 3;

                        AutoRespone(Attacker, Attacked, ref Damage);
                        if (ServerBase.Constants.Damage1Map.Contains(Attacker.MapID))
                            Damage = 1;
                        
                        //Console.WriteLine(Damage.ToString() + "       7");
                        if (Attacked.ContainsFlag2(Network.GamePackets.Update.Flags2.AzureShield))
                        {

                            if (Attacked.AzureDamage >= Damage)
                            {
                                // Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + Attacked.AzureDamage.ToString());
                                Attacked.AzureDamage -= Damage;
                                int sec = 60 - (Time32.Now - Attacked.MagicShieldStamp).AllSeconds();
                               // Attacked.Owner.Send(ServerBase.Constants.Shield(Attacked.AzureDamage, sec));
                                SyncPacket packet4 = new SyncPacket
                                {
                                    Identifier = Attacked.UID,
                                    Count = 2,
                                    Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                                    StatusFlag1 = (ulong)Attacked.StatusFlag,
                                    StatusFlag2 = (ulong)Attacked.StatusFlag2,
                                    Unknown1 = 0x31,
                                    StatusFlagOffset = 0x5d,
                                    Time = (uint)sec,
                                    Value = (uint)Attacked.AzureDamage,
                                    Level = 4
                                };
                                Attacked.Owner.Send((byte[])packet4);
                                //Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + Attacked.AzureDamage.ToString());
                                return 0;
                            }
                            else
                            {
                                //Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + Attacked.AzureDamage.ToString());
                                Damage -= Attacked.AzureDamage;
                                Attacked.AzureDamage = 0;
                                Attacked.RemoveFlag2(Update.Flags2.AzureShield);
                                SyncPacket packet4 = new SyncPacket
                                {
                                    Identifier = Attacked.UID,
                                    Count = 2,
                                    Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                                    StatusFlag1 = (ulong)Attacked.StatusFlag,
                                    StatusFlag2 = (ulong)Attacked.StatusFlag2,
                                    Unknown1 = 0x31,
                                    StatusFlagOffset = 0x5d,
                                    Time = 0,
                                    Value = 0,
                                    Level = 4
                                };
                                Attacked.Owner.Send((byte[])packet4);
                                // Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + Attacked.AzureDamage.ToString());
                                //Console.WriteLine(Damage.ToString() + "       8");
                                return (uint)Damage;

                            }

                        }
                        
                        if (ServerBase.Constants.Damage1Map.Contains(Attacker.MapID))
                            Damage = 1;
                        return (uint)Damage;
                    }
                case EntityFlag.Monster:
                    {
                        Int32 Damage = 0;
                        Int32 Defence = 0;

                        Int32 Fan = 0;
                        if (Attacker.EntityFlag == EntityFlag.Player)
                            Fan = Attacker.getFan(true);

                        if (Miss(SInfo.Percent))
                            return 0;
                        Damage = (int)Attacker.MagicAttack;
                        if (SInfo != null)
                            Damage += SInfo.Power;
                        Defence = Attacked.MonsterInfo.Level * 20;

                        if (Attacked.MonsterInfo != null)
                            if (Attacked.Name == "Guard1")
                                Damage = Damage/30;
                      

                        Damage += Fan;
                        if (Attacker.EntityFlag == EntityFlag.Player)
                            Damage += Attacker.MagicDamageIncrease;

                       

                        Damage *= (int)(2.5);

                        //Damage += (int)bpdamage;
                        Damage -= Defence;
                        if (!CritImmune)
                        {
                            if (ChanceSuccess((float)Attacker.SkillCStrike / 100f))
                            {
                                Packet.Effect1 |= Attack.AttackEffects1.CriticalStrike;
                                Damage = (Int32)Math.Floor((float)Damage * 1.2);
                            }
                        }


                        if (Damage <= 1) Damage = 1;
                        if (ServerBase.Constants.Damage1Map.Contains(Attacker.MapID))
                            Damage = 1;
                        return (UInt32)Damage;
                    }
            }
            return 0;
        }
        public static uint Magic(Entity attacker, Entity attacked, ushort spellID, byte spellLevel, ref Attack Packet)
        {
            Database.SpellInformation spell = Database.SpellTable.SpellInformations[spellID][spellLevel];
            return Magic(attacker, attacked, spell, ref Packet);
        }
        public static uint Magic(Entity attacker, SobNpcSpawn attacked, Database.SpellInformation spell, ref Attack Packet)
        {
            if (spell != null)
                if (!ServerBase.Kernel.Rate(spell.Percent))
                    return 0;
            if (spell != null)
                Durability(attacker, null, spell);
            if (attacker.Transformed)
                return 0;
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                GemEffect.Effect(attacker);
            }
            int Damage = 0;
            Damage = (int)attacker.MagicAttack;
            if (spell != null)
                Damage += spell.Power;
            if (ServerBase.Kernel.Rate(5))
            {
                if (attacker.EntityFlag == EntityFlag.Player)
                {
                    if (attacker.Owner.BlessTime > 0)
                    {
                        Damage *= 2;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacker.Owner.SendScreen(str, true);
                    }
                }
            }


            Damage += attacker.MagicDamageIncrease;
            if (ChanceSuccess((float)attacker.SkillCStrike / 100f))
            {
                Packet.Effect1 |= Attack.AttackEffects1.CriticalStrike;
                Damage = (Int32)Math.Floor((float)Damage * 1.2);
            }

            if (Damage <= 0)
                Damage = 1;
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;
            if (attacker.MonsterInfo != null)
                if (attacker.MonsterInfo.Name == "Guard1")
                    Damage = (int)attacked.MaxHitpoints + 1;
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;
            return (uint)Damage;
        }

        public static uint Ranged(Entity attacker, Entity attacked, ref Attack Packet)
        {
            if (attacked.EntityFlag == EntityFlag.Player)
            {
                BlessEffect.Effect(attacked);
            }
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                GemEffect.Effect(attacker);
            }
            int Damage = 0;
            Boolean CritImmune = false;
            Boolean canBT = true;
            Durability(attacker, attacked, null);

            if (attacked.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                return 1;
            if (attacker.Transformed)
                return 0;

            Damage = ServerBase.Kernel.Random.Next(Math.Min((int)attacker.MinAttack, (int)attacker.MaxAttack), Math.Max((int)attacker.MinAttack, (int)attacker.MaxAttack) + 1);
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (attacker.BattlePower > attacked.BattlePower)
                    canBT = false;
            }
            else canBT = false;
            if (canBT)
            {
                if (canBT)
                {
                    if (ChanceSuccess((float)attacked.Counteraction / 100f))
                        canBT = false;
                }
                if (canBT)
                {
                    if (ChanceSuccess((float)attacker.Breaktrough / 100f))
                    {
                        Damage = (Int32)attacker.MaxAttack + 3000;
                        Packet.Effect1 |= Attack.AttackEffects1.Penetration;
                    }
                }
            }
            if (attacker.OnSuperman())
                if (attacked.EntityFlag == EntityFlag.Monster)
                    Damage *= 10;
               

            if (attacker.OnFatalStrike())
                if (attacked.EntityFlag == EntityFlag.Monster)
                    Damage *= 5;

            if (!attacked.Transformed)
                Damage -= attacked.Defence;
            else
                Damage -= attacked.TransformationDefence;

            Damage -= Damage * attacked.ItemBless / 100;

            byte dodge = attacked.Dodge;
            if (dodge > 100)
                dodge = 99;
            if (!attacked.Transformed)
                Damage -= Damage * dodge / 100;
            else
                Damage -= Damage * attacked.TransformationDodge / 100;

            if (attacker.OnIntensify && Time32.Now >= attacker.IntensifyStamp.AddSeconds(4))
            {
                Damage *= 2;
                attacker.OnIntensify = false;
            }

            if (attacker.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                if (!attacker.Transformed)
                    Damage = (int)(Damage * 1.30);

            if (attacked.EntityFlag == EntityFlag.Monster)
            {
                if (attacked.MapID < 1351 || attacked.MapID > 1354)
                    Damage = (int)(Damage * (1 + (GetLevelBonus(attacker.Level, attacked.Level) * 0.08)));

                if (attacked.MapID < 1351 || attacked.MapID > 1354)
                    Damage = (int)(Damage * AttackMultiplier(attacker, attacked));
            }
            if (ServerBase.Kernel.Rate(5))
            {
                if (attacker.EntityFlag == EntityFlag.Player)
                {
                    if (attacker.Owner.BlessTime > 0)
                    {
                        Damage *= 2;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacker.Owner.SendScreen(str, true);
                    }
                }
            }

            if (ServerBase.Kernel.Rate(5))
            {
                if (attacked.EntityFlag == EntityFlag.Player)
                {
                    if (attacked.Owner.BlessTime > 0)
                    {
                        Damage = 1;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacked.Owner.SendScreen(str, true);
                    }
                }
            }
            if (attacked.EntityFlag == EntityFlag.Monster)
            {
                if (Damage >= 700 * attacked.MaxHitpoints)
                    Damage = (int)(700 * attacked.MaxHitpoints);
            }

            Damage += attacker.PhysicalDamageIncrease;
            Damage -= attacked.PhysicalDamageDecrease;
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (!CritImmune)
                {
                    if (ChanceSuccess((float)attacker.CriticalStrike / 100f))
                    {
                        Packet.Effect1 |= Attack.AttackEffects1.CriticalStrike;
                        Damage = (Int32)Math.Floor((float)Damage * 1.2);
                    }
                }
            }
            if (attacked.EntityFlag == EntityFlag.Player)
            {
                if (ChanceSuccess((float)attacked.Block / 100f))
                {
                    Packet.Effect1 |= Attack.AttackEffects1.Block;
                    Damage = (Int32)Math.Floor((float)Damage / 2);
                }
            }
            try
            {
                if (attacked.EntityFlag == EntityFlag.Player && (attacker.BattlePower < attacked.BattlePower))
                {
                    int sub = attacked.BattlePower - attacker.BattlePower;
                    
                    if (sub == 1)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .50);
                    }
                    if (sub == 2)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .49);
                    }
                    if (sub == 3)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .48);
                    }
                    if (sub == 4)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .47);
                    }
                    if (sub == 5)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .46);
                    }
                    if (sub == 6)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .45);
                    }
                    if (sub == 7)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .44);
                    }
                    if (sub == 8)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .43);
                    }
                    if (sub == 9)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .42);
                    }
                    if (sub == 10)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .41);
                    }
                    if (sub == 11)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .40);
                    }
                    if (sub == 12)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .39);
                    }
                    if (sub == 13)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .38);
                    }
                    if (sub == 14)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .37);
                    }
                    if (sub == 15)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .36);
                    }
                    if (sub == 16)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .35);
                    }
                    if (sub == 17)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .34);
                    }
                    if (sub == 18)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .33);
                    }
                    if (sub == 19)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .32);
                    }
                    if (sub == 20)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .31);
                    }
                    if (sub == 21)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .30);
                    }
                    if (sub == 22)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .29);
                    }
                    if (sub == 23)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .28);
                    }
                    if (sub == 24)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .27);
                    }
                    if (sub == 25)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .26);
                    }
                    if (sub == 26)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .25);
                    }
                    if (sub == 27)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .24);
                    }
                    if (sub == 28)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .23);
                    }
                    if (sub == 29)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .22);
                    }
                    if (sub == 30)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .21);
                    }
                    if (sub == 31)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .20);
                    }
                    if (sub == 32)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .19);
                    }
                    if (sub == 33)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .18);
                    }
                    if (sub == 34)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .17);
                    }
                    if (sub == 35)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .16);
                    }
                    if (sub == 36)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .15);
                    }
                    if (sub == 37)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .14);
                    }
                    if (sub == 38)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .13);
                    }
                    if (sub == 39)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .12);
                    }
                    if (sub == 40)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .11);
                    }
                    if (sub == 41)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .10);
                    }
                    if (sub > 41)
                    {
                        Damage = 1;
                    }
                }
            }
            catch (Exception ex) { Program.SaveException(ex); }
            //Damage += Damage;
           Damage = (int)(Damage*1.8);//kimoz

            if (Damage <= 0)
                Damage = 1;
            if (attacker.MonsterInfo != null)
                if (attacker.MonsterInfo.Name == "Guard1")
                    Damage = (int)attacked.Hitpoints + 1;
            //if (Damage > 65000)
            //Damage = 65000;
            AutoRespone(attacker, attacked, ref Damage);
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;


            // Console.WriteLine("Damage is :" + Damage.ToString() + " The Dodge is " + attacked.Dodge.ToString() +"         16");
            
            if (attacked.ContainsFlag2(Network.GamePackets.Update.Flags2.AzureShield))
            {

                if (attacked.AzureDamage >= Damage)
                {
                    ////Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    attacked.AzureDamage -= Damage;
                    int sec = 60 - (Time32.Now - attacked.MagicShieldStamp).AllSeconds();
                   // attacked.Owner.Send(ServerBase.Constants.Shield(attacked.AzureDamage, sec));
                    SyncPacket packet4 = new SyncPacket
                    {
                        Identifier = attacked.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)attacked.StatusFlag,
                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x5d,
                        Time = (uint)sec,
                        Value = (uint)attacked.AzureDamage,
                        Level = 4
                    };
                    attacked.Owner.Send((byte[])packet4);
                    //Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    return 0;
                }
                else
                {
                    //Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    Damage -= attacked.AzureDamage;
                    attacked.AzureDamage = 0;
                    attacked.RemoveFlag2(Update.Flags2.AzureShield);
                    SyncPacket packet4 = new SyncPacket
                    {
                        Identifier = attacked.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)attacked.StatusFlag,
                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x5d,
                        Time = 0,
                        Value = 0,
                        Level = 4
                    };
                    attacked.Owner.Send((byte[])packet4);
                    //Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    return (uint)Damage;

                }

            }
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;
            return (uint)Damage;

        }
        public static uint Ranged(Entity attacker, Entity attacked, Database.SpellInformation spell, ref Attack Packet)
        {
            if (attacked.EntityFlag == EntityFlag.Player)
            {
                BlessEffect.Effect(attacked);
            }
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                GemEffect.Effect(attacker);
            }
            int Damage = 0;
            Boolean CritImmune = false;
            Boolean canBT = true;
            Durability(attacker, attacked, null);

            if (attacked.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                return 1;
            if (attacker.Transformed)
                return 0;

            Damage = ServerBase.Kernel.Random.Next(Math.Min((int)attacker.MinAttack, (int)attacker.MaxAttack), Math.Max((int)attacker.MinAttack, (int)attacker.MaxAttack) + 1);
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (attacker.BattlePower > attacked.BattlePower)
                    canBT = false;
            }
            else canBT = false;
            if (canBT)
            {
                if (canBT)
                {
                    if (ChanceSuccess((float)attacked.Counteraction / 100f) && spell.ID != 1115)
                        canBT = false;
                }
                if (canBT)
                {
                    if (ChanceSuccess((float)attacker.Breaktrough / 100f) && spell.ID != 1115)
                    {
                        Damage = (Int32)attacker.MaxAttack + 3000;
                        Packet.Effect1 |= Attack.AttackEffects1.Penetration;
                    }
                }
            }
            if (attacker.OnSuperman())
                if (attacked.EntityFlag == EntityFlag.Monster)
                    Damage *= 10;
              

            if (attacker.OnFatalStrike())
                if (attacked.EntityFlag == EntityFlag.Monster)
                    Damage *= 5;

            if (!attacked.Transformed)
                Damage -= attacked.Defence;
            else
                Damage -= attacked.TransformationDefence;

            Damage -= Damage * attacked.ItemBless / 100;

            byte dodge = attacked.Dodge;
            if (dodge > 100)
                dodge = 99;
            if (!attacked.Transformed)
                Damage -= Damage * dodge / 100;
            else
                Damage -= Damage * attacked.TransformationDodge / 100;

            if (attacker.OnIntensify && Time32.Now >= attacker.IntensifyStamp.AddSeconds(4))
            {
                Damage *= 2;
                attacker.OnIntensify = false;
            }

            if (attacker.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                if (!attacker.Transformed)
                    Damage = (int)(Damage * 1.30);

            if (attacked.EntityFlag == EntityFlag.Monster)
            {
                if (attacked.MapID < 1351 || attacked.MapID > 1354)
                    Damage = (int)(Damage * (1 + (GetLevelBonus(attacker.Level, attacked.Level) * 0.08)));

                if (attacked.MapID < 1351 || attacked.MapID > 1354)
                    Damage = (int)(Damage * AttackMultiplier(attacker, attacked));
            }
            if (ServerBase.Kernel.Rate(5))
            {
                if (attacker.EntityFlag == EntityFlag.Player)
                {
                    if (attacker.Owner.BlessTime > 0)
                    {
                        Damage *= 2;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacker.Owner.SendScreen(str, true);
                    }
                }
            }

            if (ServerBase.Kernel.Rate(5))
            {
                if (attacked.EntityFlag == EntityFlag.Player)
                {
                    if (attacked.Owner.BlessTime > 0)
                    {
                        Damage = 1;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacked.Owner.SendScreen(str, true);
                    }
                }
            }
            if (attacked.EntityFlag == EntityFlag.Monster)
            {
                if (Damage >= 700 * attacked.MaxHitpoints)
                    Damage = (int)(700 * attacked.MaxHitpoints);
            }

            if (spell.PowerPercent != 0)
                Damage = (int)(Damage * spell.PowerPercent);

            Damage += attacker.PhysicalDamageIncrease;
            Damage -= attacked.PhysicalDamageDecrease;
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (!CritImmune)
                {
                    if (ChanceSuccess((float)attacker.CriticalStrike / 100f) && spell.ID != 1115)
                    {
                        Packet.Effect1 |= Attack.AttackEffects1.CriticalStrike;
                        Damage = (Int32)Math.Floor((float)Damage * 1.2);
                    }
                }
            }

            if (attacked.EntityFlag == EntityFlag.Player)
            {
                if (ChanceSuccess((float)attacked.Block / 100f))
                {
                    Packet.Effect1 |= Attack.AttackEffects1.Block;
                    Damage = (Int32)Math.Floor((float)Damage / 2);
                }
            }
            try
            {
                if (attacked.EntityFlag == EntityFlag.Player && (attacker.BattlePower < attacked.BattlePower))
                {
                    int sub = attacked.BattlePower - attacker.BattlePower;
                    
                    if (sub == 1)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .50);
                    }
                    if (sub == 2)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .49);
                    }
                    if (sub == 3)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .48);
                    }
                    if (sub == 4)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .47);
                    }
                    if (sub == 5)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .46);
                    }
                    if (sub == 6)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .45);
                    }
                    if (sub == 7)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .44);
                    }
                    if (sub == 8)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .43);
                    }
                    if (sub == 9)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .42);
                    }
                    if (sub == 10)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .41);
                    }
                    if (sub == 11)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .40);
                    }
                    if (sub == 12)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .39);
                    }
                    if (sub == 13)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .38);
                    }
                    if (sub == 14)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .37);
                    }
                    if (sub == 15)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .36);
                    }
                    if (sub == 16)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .35);
                    }
                    if (sub == 17)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .34);
                    }
                    if (sub == 18)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .33);
                    }
                    if (sub == 19)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .32);
                    }
                    if (sub == 20)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .31);
                    }
                    if (sub == 21)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .30);
                    }
                    if (sub == 22)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .29);
                    }
                    if (sub == 23)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .28);
                    }
                    if (sub == 24)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .27);
                    }
                    if (sub == 25)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .26);
                    }
                    if (sub == 26)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .25);
                    }
                    if (sub == 27)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .24);
                    }
                    if (sub == 28)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .23);
                    }
                    if (sub == 29)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .22);
                    }
                    if (sub == 30)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .21);
                    }
                    if (sub == 31)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .20);
                    }
                    if (sub == 32)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .19);
                    }
                    if (sub == 33)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .18);
                    }
                    if (sub == 34)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .17);
                    }
                    if (sub == 35)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .16);
                    }
                    if (sub == 36)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .15);
                    }
                    if (sub == 37)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .14);
                    }
                    if (sub == 38)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .13);
                    }
                    if (sub == 39)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .12);
                    }
                    if (sub == 40)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .11);
                    }
                    if (sub == 41)
                    {
                        Damage = (Int32)Math.Floor((float)Damage * .10);
                    }
                    if (sub > 41)
                    {
                        Damage = 1;
                    }
                }
            }
            catch (Exception ex) { Program.SaveException(ex); }
            //Damage += Damage;
           Damage = (int)(Damage*1.8);//kimoz

            if (Damage <= 0)
                Damage = 1;
            if (attacker.MonsterInfo != null)
                if (attacker.MonsterInfo.Name == "Guard1")
                    Damage = (int)attacked.Hitpoints + 1;
            //if (Damage > 65000)
            //Damage = 65000;
            AutoRespone(attacker, attacked, ref Damage);
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;

            //Console.WriteLine("Damage is :" + Damage.ToString() + " The Dodge is " + attacked.Dodge.ToString() + "         16");


            
            if (attacked.ContainsFlag2(Network.GamePackets.Update.Flags2.AzureShield))
            {

                if (attacked.AzureDamage >= Damage)
                {
                    //Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    attacked.AzureDamage -= Damage;
                    int sec = 60 - (Time32.Now - attacked.MagicShieldStamp).AllSeconds();
                   // attacked.Owner.Send(ServerBase.Constants.Shield(attacked.AzureDamage, sec));
                    SyncPacket packet4 = new SyncPacket
                    {
                        Identifier = attacked.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)attacked.StatusFlag,
                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x5d,
                        Time = (uint)sec,
                        Value = (uint)attacked.AzureDamage,
                        Level = 4
                    };
                    attacked.Owner.Send((byte[])packet4);
                    //Console.WriteLine("^^^^Damage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    return 0;
                }
                else
                {
                    //Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    Damage -= attacked.AzureDamage;
                    attacked.AzureDamage = 0;
                    attacked.RemoveFlag2(Update.Flags2.AzureShield);
                    SyncPacket packet4 = new SyncPacket
                    {
                        Identifier = attacked.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)attacked.StatusFlag,
                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x5d,
                        Time = 0,
                        Value = 0,
                        Level = 4
                    };
                    attacked.Owner.Send((byte[])packet4);
                    //Console.WriteLine("XXXXDamage is " + Damage.ToString() + " Azure is : " + attacked.AzureDamage.ToString());
                    return (uint)Damage;

                }

            }
            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;
            return (uint)Damage;
        }
        public static uint Ranged(Entity attacker, SobNpcSpawn attacked, ref Attack Packet)
        {
            int Damage = 0;
            Boolean CritImmune = false;
            Boolean canBT = true;
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                GemEffect.Effect(attacker);
            }
            Durability(attacker, null, null);
            if (attacker.Transformed)
                return 0;

            Damage = ServerBase.Kernel.Random.Next(Math.Min((int)attacker.MinAttack, (int)attacker.MaxAttack), Math.Max((int)attacker.MinAttack, (int)attacker.MaxAttack) + 1);

           if (canBT)
            {
                if (ChanceSuccess((float)attacker.Breaktrough / 100f))
                {
                    Damage = (Int32)attacker.MaxAttack + 3000;
                    Packet.Effect1 |= Attack.AttackEffects1.Penetration;
                }
            }
            if (attacker.OnIntensify && Time32.Now >= attacker.IntensifyStamp.AddSeconds(4))
            {
                Damage *= 2;

                attacker.OnIntensify = false;
            }

            if (ServerBase.Kernel.Rate(5))
            {
                if (attacker.EntityFlag == EntityFlag.Player)
                {
                    if (attacker.Owner.BlessTime > 0)
                    {
                        Damage *= 2;
                        _String str = new _String(true);
                        str.UID = attacker.UID;
                        str.TextsCount = 1;
                        str.Type = _String.Effect;
                        str.Texts.Add("LuckyGuy");
                        attacker.Owner.SendScreen(str, true);
                    }
                }
            }
            if (attacker.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                if (!attacker.Transformed)
                    Damage = (int)(Damage * 1.30);

            Damage += attacker.PhysicalDamageIncrease;
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (!CritImmune)
                {
                    if (ChanceSuccess((float)attacker.CriticalStrike / 100f))
                    {
                        Packet.Effect1 |= Attack.AttackEffects1.CriticalStrike;
                        Damage = (Int32)Math.Floor((float)Damage * 1.2);
                    }
                }
            }
            //Damage += Damage;
           Damage = (int)(Damage*1.8);//kimoz

            if (Damage <= 0)
                Damage = 1;
            //if (Damage > 65000)
            //Damage = 65000;

            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                Damage = 1;
            return (uint)Damage;
        }

        public static int RemoveExcessDamage(int CurrentDamage, Entity Attacker, Entity Opponent)
        {
            if (Opponent.EntityFlag != EntityFlag.Player)
                return CurrentDamage;
            if (Opponent.Reborn == 1)
                CurrentDamage = (int)Math.Round((double)(CurrentDamage * 0.7));
            else if (Opponent.Reborn == 2)
                CurrentDamage = (int)Math.Round((double)(CurrentDamage * 0.5));
            CurrentDamage = (int)Math.Round((double)(CurrentDamage * (1.00 - (Opponent.ItemBless * 0.01))));

            return CurrentDamage;
        }
        public static bool ChanceSuccess(int c, int inchance)
        {
            Random rand = new Random();
            int e = rand.Next(inchance);

            if (e != 0)
                if (e <= c)
                    return true;

            return false;
        }

        public static uint Percent(Entity attacked, float percent)
        {
            return (uint)(attacked.Hitpoints * percent);
        }

        public static uint Percent(SobNpcSpawn attacked, float percent)
        {
            return (uint)(attacked.Hitpoints * percent);
        }

        public static uint Percent(int target, float percent)
        {
            return (uint)(target * percent);
        }

        private static void Durability(Entity attacker, Entity attacked, Database.SpellInformation spell)
        {
           // return;

            if (spell != null)
                if (spell.CanKill == 0)
                    return;
            if (attacker.EntityFlag == EntityFlag.Player)
                if (attacker.Owner.Map.ID == 1039)
                    return;
            #region Attack
            if (attacker != null)
                if (attacker.EntityFlag == EntityFlag.Player)
                {
                    for (byte i = 4; i <= 6; i++)
                    {
                        if (!attacker.Owner.Equipment.Free(i))
                        {
                            var item = attacker.Owner.Equipment.TryGetItem(i);
                            if (i == 5)
                            {
                                if (Network.PacketHandler.IsArrow(item.ID))
                                {
                                    continue;
                                }
                            }
                            if (ServerBase.Kernel.Rate(20, 100))
                            {
                                if (item.Durability != 0)
                                {
                                    item.Durability--;
                                    if (item.Durability == 0)
                                        //attacker.Owner.UnloadItemStats(item, true);
                                        Database.ConquerItemTable.UpdateDurabilityItem(item);
                                    item.Mode = Enums.ItemMode.Update;
                                    item.Send(attacker.Owner);
                                    item.Mode = Enums.ItemMode.Default;
                                }
                            }
                        }
                        if (i == 6)
                            break;
                    }
                    if (!attacker.Owner.Equipment.Free(10))
                    {
                        var item = attacker.Owner.Equipment.TryGetItem(10);
                        if (ServerBase.Kernel.Rate(20, 100))
                        {
                            if (item.Durability != 0)
                            {
                                item.Durability--;
                                if (item.Durability == 0)
                                    //attacker.Owner.UnloadItemStats(item, true);
                                    Database.ConquerItemTable.UpdateDurabilityItem(item);
                                item.Mode = Enums.ItemMode.Update;
                                item.Send(attacker.Owner);
                                item.Mode = Enums.ItemMode.Default;
                            }
                        }
                    }
                }
            #endregion
            #region Defence
            if (attacked != null)
                if (attacked.EntityFlag == EntityFlag.Player)
                {
                    for (byte i = 1; i <= 8; i++)
                    {
                        if (i == 4 || i == 6 || i == 7)
                            continue;
                        if (!attacked.Owner.Equipment.Free(i))
                        {
                            var item = attacked.Owner.Equipment.TryGetItem(i);
                            if (i == 5)
                            {
                                if (Network.PacketHandler.ItemPosition(item.ID) != 5 && Network.PacketHandler.IsArrow(item.ID))
                                {
                                    continue;
                                }
                            }
                            if (ServerBase.Kernel.Rate(30, 100))
                            {
                                if (item.Durability != 0)
                                {
                                    item.Durability--;
                                    if (item.Durability == 0)
                                        //attacked.Owner.UnloadItemStats(item, true);
                                        Database.ConquerItemTable.UpdateDurabilityItem(item);

                                    item.Mode = Enums.ItemMode.Update;
                                    item.Send(attacked.Owner);
                                    item.Mode = Enums.ItemMode.Default;
                                }
                            }
                        }
                        if (i == 8)
                            break;
                    }
                    if (!attacked.Owner.Equipment.Free(11) && ServerBase.Kernel.Rate(30, 100))
                    {
                        var item = attacked.Owner.Equipment.TryGetItem(11);
                        if (ServerBase.Kernel.Rate(30, 100))
                        {
                            if (item.Durability != 0)
                            {
                                item.Durability--;
                                if (item.Durability == 0)
                                    //attacked.Owner.UnloadItemStats(item, true);
                                    Database.ConquerItemTable.UpdateDurabilityItem(item);
                                item.Mode = Enums.ItemMode.Update;
                                item.Send(attacked.Owner);
                                item.Mode = Enums.ItemMode.Default;
                            }
                        }
                    }
                }

            #endregion
        }

        private static void AutoRespone(Entity attacker, Entity attacked, ref int Damage)
        {
            try
            {
                if (attacked.EntityFlag == EntityFlag.Player)
                {
                    if (attacked.CounterKillSwitch  && !attacker.ContainsFlag(Update.Flags.Fly) && Time32.Now > attacked.CounterKillStamp.AddSeconds(15))
                    {
                        attacked.CounterKillStamp = Time32.Now;
                        Network.GamePackets.Attack attack = new PhoenixProject.Network.GamePackets.Attack(true);
                        attack.Effect1 = Attack.AttackEffects1.None;
                        uint damage = Melee(attacked, attacker, ref attack);
                        //Database.SpellInformation information = Database.SpellTable.SpellInformations[6003][attacked.Owner.Spells[6003].Level];
                        damage = damage/3;
                        attack.Attacked = attacker.UID;
                        attack.Attacker = attacked.UID;
                        attack.AttackType = Network.GamePackets.Attack.Scapegoat;
                        attack.Damage = 0;
                        attack.ResponseDamage = damage;
                        attack.X = attacked.X;
                        attack.Y = attacked.Y;

                        if (attacker.Hitpoints <= damage)
                        {
                            if (attacker.EntityFlag == EntityFlag.Player)
                            {
                                if (attacked.Owner.QualifierGroup != null)
                                    attacked.Owner.QualifierGroup.UpdateDamage(attacked.Owner, attacker.Hitpoints);
                                attacker.Owner.SendScreen(attack, true);
                                attacked.AttackPacket = null;
                            }
                            else
                            {
                                attacker.MonsterInfo.SendScreen(attack);
                            }
                            attacker.Die(attacked);
                        }
                        else
                        {
                            attacker.Hitpoints -= damage;
                            if (attacker.EntityFlag == EntityFlag.Player)
                            {
                                if (attacked.Owner.QualifierGroup != null)
                                    attacked.Owner.QualifierGroup.UpdateDamage(attacked.Owner, damage);
                                attacker.Owner.SendScreen(attack, true);
                            }
                            else
                            {
                                attacker.MonsterInfo.SendScreen(attack);
                            }
                        }
                        Damage = 0;
                    }
                    else if (attacked.Owner.Spells.ContainsKey(3060) && ServerBase.Kernel.Rate(30) && Time32.Now > attacked.ReflectStamp.AddSeconds(10))
                    {
                        attacked.ReflectStamp = Time32.Now;
                        uint damage = (uint)(Damage / 10);
                        if (damage <= 0)
                            damage = 1;
                        Network.GamePackets.Attack attack = new PhoenixProject.Network.GamePackets.Attack(true);
                        attack.Attacked = attacker.UID;
                        attack.Attacker = attacked.UID;
                        attack.AttackType = Network.GamePackets.Attack.Reflect;
                        attack.Damage = damage;
                        attack.ResponseDamage = damage;
                        attack.X = attacked.X;
                        attack.Y = attacked.Y;

                        if (attacker.Hitpoints <= damage)
                        {
                            if (attacker.EntityFlag == EntityFlag.Player)
                            {
                                if (attacked.Owner.QualifierGroup != null)
                                    attacked.Owner.QualifierGroup.UpdateDamage(attacked.Owner, attacker.Hitpoints);
                                attacker.Owner.SendScreen(attack, true);
                                attacked.AttackPacket = null;
                            }
                            else
                            {
                                attacker.MonsterInfo.SendScreen(attack);
                            }
                            attacker.Die(attacked);
                        }
                        else
                        {
                            attacker.Hitpoints -= damage;
                            if (attacker.EntityFlag == EntityFlag.Player)
                            {
                                if (attacked.Owner.QualifierGroup != null)
                                    attacked.Owner.QualifierGroup.UpdateDamage(attacked.Owner, damage);
                                attacker.Owner.SendScreen(attack, true);
                            }
                            else
                            {
                                attacker.MonsterInfo.SendScreen(attack);
                            }
                        }
                        Damage = 0;
                    }
                }
            }
            catch (Exception e) { Program.SaveException(e); }
        }
        public static int GetLevelBonus(int l1, int l2)
        {
            int num = l1 - l2;
            int bonus = 0;
            if (num >= 3)
            {
                num -= 3;
                bonus = 1 + (num / 5);
            }
            return bonus;
        }
        private static double AttackMultiplier(Entity attacker, Entity attacked)
        {
            if (attacked.Level > attacker.Level)
                return 1;
            return ((double)(attacker.Level - attacked.Level)) / 10 + 1;
        }
        public static ulong CalculateExpBonus(ushort Level, ushort MonsterLevel, ulong Experience)
        {
            int leveldiff = (2 + Level - MonsterLevel);
            if (leveldiff < -5)
                Experience = (ulong)(Experience * 1.3);
            else if (leveldiff < -1)
                Experience = (ulong)(Experience * 1.2);
            else if (leveldiff == 4)
                Experience = (ulong)(Experience * 0.8);
            else if (leveldiff == 5)
                Experience = (ulong)(Experience * 0.3);
            else if (leveldiff > 5)
                Experience = (ulong)(Experience * 0.1);
            return Experience;
        }
    }
}
