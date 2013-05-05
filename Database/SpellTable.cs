using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using KinSocket;

namespace PhoenixProject.Database
{

    public enum LeftToAdd : ushort
    {
        Bless = 9876,
        //----------
        SummonGuard = 4000,
        SummonBat = 4010,
        SummonBatBoss = 4020,
        BloodyBat = 4050,
        FireEvil = 4060,
        Skeleton = 4070
    }
    public class SpellSort
    {
        public const byte
            Damage = 1,
            Heal = 2,
            MultiWeaponSkill = 4,
            Circle = 5,
            XP = 6,
            Revive = 7,
            XPIncrease = 11,
            Dash = 12,
            Linear = 14,
            SingleWeaponSkill = 16,
            Specials = 19,
            ManaAdd = 20,
            Summon = 23,
            HPPercentDecrease = 26,
            Spook = 30,
            WarCry = 31,
            Ride = 32;
    }
    public class SpellTarget
    {
        public const byte
            Magic = 0,
            EntitiesOnly = 1,
            Self = 2,
            AroundCoordonates = 4,
            Sector = 8,//power % 1000 = sector angle
            AutoAttack = 16,
            PlayersOnly = 32;
    }
   
    public class SpellInformation
    {
        public ushort ID;
        public byte Level;
        private System.Collections.Concurrent.ConcurrentDictionary<ushort, Native.TIME> Cooldowns;
        public bool RemoveCooldown(ushort skillid)
        {
            KinSocket.Native.TIME time;
            return this.Cooldowns.TryRemove(skillid, out time);
        }
        public void AddCooldown(ushort skillid, int miliseconds)
        {
            if (this.Cooldowns.ContainsKey(skillid))
            {
                Native.TIME time;
                this.Cooldowns.TryRemove(skillid, out time);
            }
            this.Cooldowns.TryAdd(skillid, Native.TIME.Now.AddMilliseconds(miliseconds));
        }
        public bool CanUseSkill(ushort skillid)
        {
            if (this.Cooldowns.ContainsKey(skillid))
            {
                return (Native.TIME.Now >= this.Cooldowns[skillid]);
            }
            return true;
        }
        public byte CanKill;
        public byte Sort;
        public byte OnlyGround;
        public byte Multi;
        public byte Target;

        public ushort UseMana;
        public byte UseStamina;
        public byte UseArrows;

        public byte Percent;
        public int Sector;

        public uint Duration;

        public ushort Range;
        public ushort Distance;

        public ushort Power;
        public float PowerPercent;

        public ulong Status;

        public uint NeedExperience;
        public byte NeedLevel;
        public byte NeedXP;

        public uint WeaponSubtype;
        public uint OnlyWithThisWeaponSubtype;
        public ushort NextSpellID;
        public uint  UnknownPush;
        public uint CoolDown;
        public uint CPUpgradeRatio;
    }
    public class SpellTable
    {
        public static SafeDictionary<ushort, SafeDictionary<byte, SpellInformation>> SpellInformations = new SafeDictionary<ushort, SafeDictionary<byte, SpellInformation>>(100000);
        public static SafeDictionary<ushort, ushort> WeaponSpells = new SafeDictionary<ushort, ushort>(100);

        public static void Parse(string Line)
        {
            string[] data = Line.Split(new string[] { "@@", " " }, StringSplitOptions.RemoveEmptyEntries);
            SpellInformation spell = new SpellInformation();
            spell.ID = Convert.ToUInt16(data[1]);
            spell.Level = Convert.ToByte(data[8]);
            spell.CanKill = Convert.ToByte(data[4]);
            spell.Sort = Convert.ToByte(data[2]);
            spell.OnlyGround = Convert.ToByte(data[5]);
            spell.Multi = Convert.ToByte(data[6]);
            spell.Target = Convert.ToByte(data[7]);
            spell.UseMana = Convert.ToUInt16(data[9]);
            spell.Power = Convert.ToUInt16(data[10]);
            spell.PowerPercent = (float)(spell.Power % 1000) / 100;
            if (spell.Power > 13000)
                spell.Power = 0;
            spell.Percent = Convert.ToByte(data[12]);
            spell.Duration = Convert.ToUInt32(data[13]);
            spell.Range = Convert.ToUInt16(data[14]);
            spell.Sector = spell.Range * 20;
            spell.Distance = Convert.ToUInt16(data[15]);
            if (spell.Distance >= 4) spell.Distance--;
            spell.Status = Convert.ToUInt64(data[16]);
            spell.NeedExperience = Convert.ToUInt32(data[18]);
            spell.NeedLevel = Convert.ToByte(data[20]);
            spell.WeaponSubtype = Convert.ToUInt32(data[22]);
            spell.OnlyWithThisWeaponSubtype = Convert.ToUInt32(data[22]);
            spell.NextSpellID = Convert.ToUInt16(data[32]);
            spell.NeedXP = Convert.ToByte(data[21]);
            spell.UseStamina = Convert.ToByte(data[29]);
            spell.UseArrows = Convert.ToByte(data[34]);
            spell.CoolDown = Convert.ToUInt32(data[47]);
            spell.CPUpgradeRatio = Convert.ToUInt32(data[48]);
            spell.UnknownPush = Convert.ToUInt32(data[44]);
            if (SpellInformations.ContainsKey(spell.ID))
            {
                SpellInformations[spell.ID].Add(spell.Level, spell);
            }
            else
            {
                SpellInformations.Add(spell.ID, new SafeDictionary<byte, SpellInformation>(10));
                SpellInformations[spell.ID].Add(spell.Level, spell);
            }
            if (spell.Distance > 17)
                spell.Distance = 17;
            
            if (spell.WeaponSubtype != 0)
            {
                switch (spell.ID)
                {
                    case 5010:
                    case 7020:
                    case 1290:
                    case 1260:
                    case 5030:
                    case 5040:
                    case 7000:
                    case 7010:
                    case 7030:
                    case 7040:
                    case 1250:
                    case 5050:
                    case 5020:
                    case 10490:
                    case 11140:
                    case 11230:
                    case 1300:
                    case 11120:
                        if (spell.Distance >= 3)
                            spell.Distance = 3;
                        if (spell.Range > 3)
                            spell.Range = 3;
                      
                        
                        if (!WeaponSpells.ContainsKey((ushort)spell.WeaponSubtype))
                        {
                            WeaponSpells.Add((ushort)spell.WeaponSubtype, spell.ID);
                        }
                        break;
                }
            }

          

        }
        public static void Load()
        {

            string[] baseText = File.ReadAllLines(ServerBase.Constants.MagicTypeFile);
            foreach (string line in baseText)
            {
                string item = line.Trim();
                Parse(item);
            }

        }
    }
}
