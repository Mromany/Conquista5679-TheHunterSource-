using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets.Quest
{
    public class Quest
    {
        public string ActivityBeginTime;
        public string ActivityEndTime;
        public int ActivityType;
        public bool Auto;
        public int CompleteFlag;
        public string FinishTime;
        public bool First;
        public uint Identifier;
        public List<BoothItem2> ItemPrizes = new List<BoothItem2>();
        public uint Map;
        public ushort MaxLevel;
        public ushort MinLevel;
        public string Name;
        public List<uint> Prerequests = new List<uint>();
        public Dictionary<QuestPrize, uint> Prizes;
        public List<ClassNames> Professions = new List<ClassNames>();
        public ushort Sex;
        public QuestTypes Type;

        
        public Quest()
        {
            Dictionary<QuestPrize, uint> dictionary = new Dictionary<QuestPrize, uint>();
            dictionary.Add(QuestPrize.Exp, 0);
            dictionary.Add(QuestPrize.Gold, 0);
            dictionary.Add(QuestPrize.CP, 0);
            this.Prizes = dictionary;
        }
        public enum ClassNames : byte
        {
            Archer = 0x29,
            ArcherMaster = 0x2d,
            BrassWarrior = 0x16,
            DarkNinja = 0x35,
            DharmaMonk = 0x3f,
            DhyanaMonk = 0x3e,
            DragonArcher = 0x2c,
            DragonTrojan = 14,
            EagleArcher = 0x2a,
            ERROR = 0,
            FireMaster = 0x90,
            FireSaint = 0x91,
            FireTaoist = 0x8e,
            FireWizard = 0x8f,
            GoldWarrior = 0x18,
            InterMonk = 60,
            InternArcher = 40,
            InterNinja = 50,
            InternPirate = 70,
            InternTaoist = 100,
            InternTrojan = 10,
            InternWarrior = 20,
            MiddleNinja = 0x34,
            Monk = 0x3d,
            MysticNinja = 0x36,
            Ninja = 0x33,
            NinjaMaster = 0x37,
            NirvanaMonk = 0x41,
            Pirate = 0x47,
            PirateCaptain = 0x4a,
            PirateGunner = 0x48,
            PirateLord = 0x4b,
            PirateQuartermaster = 0x49,
            PrajnaMonk = 0x40,
            SilverWarrior = 0x17,
            Taoist = 0x65,
            TigerArcher = 0x2b,
            TigerTrojan = 13,
            Trojan = 11,
            TrojanMaster = 15,
            VeteranTrojan = 12,
            Warrior = 0x15,
            WarriorMaster = 0x19,
            WaterMaster = 0x86,
            WaterSaint = 0x87,
            WaterTaoist = 0x84,
            WaterWizard = 0x85
        }
    }
}
