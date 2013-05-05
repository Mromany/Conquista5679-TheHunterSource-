using System;
using System.Collections.Generic;
using System.IO;
namespace PhoenixProject.Database
{
    public class ConquerItemInformation
    {
        public static SafeDictionary<uint, SafeDictionary<byte, ConquerItemPlusInformation>> PlusInformations;
        public static SafeDictionary<uint, ConquerItemBaseInformation> BaseInformations;
        public static SafeDictionary<string, SafeDictionary<int, ConquerItemBaseInformation>> GradeInformations;
        public static SafeDictionary<string, SafeDictionary<uint, int>> GradeInformations2;
        public static Dictionary<uint, ConquerItemBaseInformation> BaseInfos = new Dictionary<uint, ConquerItemBaseInformation>();
        public static void Load()
        {
            BaseInformations = new SafeDictionary<uint, ConquerItemBaseInformation>(10000);
            PlusInformations = new SafeDictionary<uint, SafeDictionary<byte, ConquerItemPlusInformation>>(10000);
            GradeInformations = new SafeDictionary<string, SafeDictionary<int, ConquerItemBaseInformation>>(10000);
            GradeInformations2 = new SafeDictionary<string, SafeDictionary<uint, int>>(10000);
            string[] baseText = File.ReadAllLines(ServerBase.Constants.ItemBaseInfosPath);
            int count = 0;
            int gkey = 0;
            int lastlevel = 0;
            string lastgr = "";
            foreach (string line in baseText)
            {
                count++;
                string _item_ = line.Trim();
                if (_item_.Length > 11)
                {
                    if (_item_.IndexOf("//", 0, 2) != 0)
                    {
                        ConquerItemBaseInformation CIBI = new ConquerItemBaseInformation();
                        CIBI.Parse(_item_);

                        var Grades = GradeInformations[CIBI.Description];
                        BaseInformations.Add(CIBI.ID, CIBI);
                        if (GradeInformations.ContainsKey(CIBI.Description) == false)
                        {
                            GradeInformations2.Add(CIBI.Description, new SafeDictionary<uint, int>(1000));
                            GradeInformations2[CIBI.Description].Add((uint)(CIBI.ID / 10), 0);
                            lastlevel = CIBI.Level;
                            GradeInformations.Add(CIBI.Description, new SafeDictionary<int, ConquerItemBaseInformation>(1000));
                            gkey = 0;
                        }
                        else
                        {
                            if (lastgr != CIBI.Description)
                                gkey = GradeInformations2[CIBI.Description].Count - 1;

                            if (GradeInformations2[CIBI.Description].ContainsKey(CIBI.ID / 10) && CIBI.Level == lastlevel)
                            {
                                CIBI.GradeKey = gkey;
                                continue;
                            }
                            else
                            {
                                GradeInformations2[CIBI.Description].Add((uint)(CIBI.ID / 10), 0);
                                lastlevel = CIBI.Level;
                                gkey = gkey + 1;
                            }
                        }
                        lastgr = CIBI.Description;
                        CIBI.GradeKey = gkey;
                        GradeInformations[CIBI.Description].Add(gkey, CIBI);
                    }
                }
            }
            GradeInformations2.Base.Clear();
            baseText = File.ReadAllLines(ServerBase.Constants.ItemPlusInfosPath);

            foreach (string line in baseText)
            {
                try
                {
                    string _item_ = line.Trim();
                    ConquerItemPlusInformation CIPI = new ConquerItemPlusInformation();
                    CIPI.Parse(_item_);
                    SafeDictionary<byte, ConquerItemPlusInformation> info = null;
                    if (PlusInformations.TryGetValue(CIPI.ID, out info))
                    {
                        info.Add(CIPI.Plus, CIPI);
                    }
                    else
                    {
                        PlusInformations.Add(CIPI.ID, new SafeDictionary<byte, ConquerItemPlusInformation>(1000));
                        if (PlusInformations.TryGetValue(CIPI.ID, out info))
                        {
                            info.Add(CIPI.Plus, CIPI);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine(line);
                }
            }
            Console.WriteLine("Item Base and Plus information loaded.");
        }
        public ConquerItemInformation(uint ID, byte Plus)
        {
            _BaseInformation = null;
            if (BaseInformations.TryGetValue(ID, out _BaseInformation))
            {
                byte itemType = (byte)(ID / 10000);
                ushort itemType2 = (ushort)(ID / 1000);
                if (itemType == 14)//armors
                {
                    ID = (uint)(
                                (((uint)(ID / 1000)) * 1000) + // [3] = 0
                                ((ID % 100) - (ID % 10)) // [5] = 0
                            );
                }
                else if (itemType == 13 || itemType == 11 || itemType2 == 123 || itemType == 30 || itemType == 20 || itemType == 12 || itemType == 15 || itemType == 16 || itemType == 50 || itemType2 == 421 || itemType2 == 601)//Necky bow bag
                {
                    ID = (uint)(
                                ID - (ID % 10) // [5] = 0
                            );
                }
                else
                {
                    byte head = (byte)(ID / 100000);
                    ID = (uint)(
                            ((head * 100000) + (head * 10000) + (head * 1000)) + // [1] = [0], [2] = [0]
                            ((ID % 1000) - (ID % 10)) // [5] = 0
                        );
                }
                _PlusInformation = new ConquerItemPlusInformation();
                if (Plus > 0)
                {
                    SafeDictionary<byte, ConquerItemPlusInformation> pInfo = null;
                    PlusInformations.TryGetValue(ID, out pInfo);
                    if (pInfo == null)
                        return;
                    if (!pInfo.TryGetValue(Plus, out _PlusInformation))
                        _PlusInformation = new ConquerItemPlusInformation();
                }
            }
            else
            {
                return;
            }
        }
        private ConquerItemPlusInformation _PlusInformation;
        private ConquerItemBaseInformation _BaseInformation;
        public ConquerItemPlusInformation PlusInformation
        {
            get
            {
                if (_PlusInformation == null)
                    return new ConquerItemPlusInformation();
                return _PlusInformation;
            }
        }
        public ConquerItemBaseInformation BaseInformation
        {
            get
            {
                if (_BaseInformation == null)
                    return new ConquerItemBaseInformation();
                return _BaseInformation;
            }
        }
        public uint CalculateUplevel()
        {
            var grades = GradeInformations[this.BaseInformation.Description];
            if (grades == null) return BaseInformation.ID;
            if (grades[BaseInformation.GradeKey + 1] == null)
                return BaseInformation.ID;
            else
                return (uint)((grades[BaseInformation.GradeKey + 1].ID / 10) * 10 + BaseInformation.ID % 10);
        }
        public uint CalculateDownlevel()
        {
            var grades = GradeInformations[this.BaseInformation.Description];
            if (grades == null) return BaseInformation.ID;
            if (grades[BaseInformation.GradeKey - 1] == null)
                return BaseInformation.ID;
            else
                return (uint)((grades[BaseInformation.GradeKey - 1].ID / 10) * 10 + BaseInformation.ID % 10);
        }
        public uint LowestID(byte Level)
        {
            var grades = GradeInformations[this.BaseInformation.Description];
            if (grades == null) return BaseInformation.ID;
            for (byte gr = 0; gr < grades.Count; gr++)
                if (grades[gr].Level == Level)
                    return (uint)((grades[gr + 1].ID / 10) * 10 + BaseInformation.ID % 10);
            return grades[0].ID;
        }
        public uint LowestID2(byte Level)
        {
            var grades = GradeInformations[this.BaseInformation.Description];
            if (grades == null) return BaseInformation.ID;
            for (byte gr = 0; gr < grades.Count; gr++)
                if (grades[gr].Level == Level)
                    return (uint)((grades[gr + 1].ID / 10) * 10 + BaseInformation.ID % 10);
            return grades[0].ID;
        }
    }
    public class ConquerItemPlusInformation
    {
        public uint ID;
        public byte Plus;
        public ushort ItemHP;
        public uint MinAttack;
        public uint MaxAttack;
        public ushort PhysicalDefence;
        public ushort MagicAttack;
        public ushort MagicDefence;
        public ushort Agility;
        public ushort Vigor { get { return Agility; } }
        public byte Dodge;
        public ushort SpeedPlus { get { return Dodge; } }
        public void Parse(string Line)
        {
            string[] Info = Line.Split(' ');
            ID = uint.Parse(Info[0]);
            Plus = byte.Parse(Info[1]);
            ItemHP = ushort.Parse(Info[2]);
            MinAttack = uint.Parse(Info[3]);
            MaxAttack = uint.Parse(Info[4]);
            PhysicalDefence = ushort.Parse(Info[5]);
            MagicAttack = ushort.Parse(Info[6]);
            MagicDefence = ushort.Parse(Info[7]);
            Agility = ushort.Parse(Info[8]);
            Dodge = byte.Parse(Info[9]);
        }
        public ConquerItemPlusInformation()
        {
            ID = MinAttack = MaxAttack = 0;
            Plus = Dodge = 0;
            PhysicalDefence = MagicAttack = MagicDefence = Agility = 0;
        }
    }
    public class ConquerItemBaseInformation
    {
        public uint ID;
        public string Name;
        public byte Class;
        public byte Proficiency;
        public byte Level;
        public byte Gender;
        public ushort Strength;
        public ushort Agility;
        public uint GoldWorth;
        public ushort MinAttack;
        public ushort MaxAttack;
        public ushort PhysicalDefence;
        public ushort MagicDefence;
        public ushort MagicAttack;
        public byte Dodge;
        public byte Frequency;
        public uint ConquerPointsWorth;
        public ushort Durability;
        public ushort StackSize;
        public ushort ItemHP;
        public ushort ItemMP;
        public ushort AttackRange;
        public ItemType Type;
        public string Description;
        public int GradeKey;
        public string FinalDescription;

        public ushort PurificationLevel;
        public ushort PurificationMeteorNeed;

        public UInt16 EarthResist,
            FireResist, WaterResist, WoodResist, Block, Detoxication,
            CounterAction, Penetration, Immunity, BreakThrough, CriticalStrike;

        public void Parse(string Line)
        {
            string[] data = Line.Split(new string[] { "@@", " " }, StringSplitOptions.RemoveEmptyEntries);
            ID = Convert.ToUInt32(data[0]);
            Name = data[1].Trim();
            Class = Convert.ToByte(data[2]);
            Proficiency = Convert.ToByte(data[3]);
            Level = Convert.ToByte(data[4]);
            Gender = Convert.ToByte(data[5]);
            Strength = Convert.ToUInt16(data[6]);
            Agility = Convert.ToUInt16(data[7]);
            Type = Convert.ToUInt32(data[10]) == 0 ? ItemType.Dropable : ItemType.Others;
            GoldWorth = Convert.ToUInt32(data[12]);
            MaxAttack = Convert.ToUInt16(data[14]);
            MinAttack = Convert.ToUInt16(data[15]);
            PhysicalDefence = Convert.ToUInt16(data[16]);
            Frequency = Convert.ToByte(data[17]);
            Dodge = Convert.ToByte(data[18]);
            ItemHP = Convert.ToUInt16(data[19]);
            ItemMP = Convert.ToUInt16(data[20]);
            Durability = Convert.ToUInt16(data[22]);
            MagicAttack = Convert.ToUInt16(data[30]);
            MagicDefence = Convert.ToUInt16(data[31]);
            AttackRange = Convert.ToUInt16(data[32]);
            ConquerPointsWorth = Convert.ToUInt32(data[37]);
            StackSize = Convert.ToUInt16(data[47]);

            EarthResist = Convert.ToUInt16(data[52]);
            FireResist = Convert.ToUInt16(data[51]);
            WaterResist = Convert.ToUInt16(data[50]);
            WoodResist = Convert.ToUInt16(data[49]);
            Block = Convert.ToUInt16(data[44]);
            //Detoxication = Convert.ToUInt16(data[43]);
            CounterAction = Convert.ToUInt16(data[46]);
            Penetration = Convert.ToUInt16(data[43]);
            Immunity = Convert.ToUInt16(data[42]);
            BreakThrough = Convert.ToUInt16(data[45]);
            CriticalStrike = Convert.ToUInt16(data[40]);


            Description = data[53].Replace("`s", "");
            if (Description == "NinjaKatana")
                Description = "NinjaWeapon";
            if (Description == "Earrings")
                Description = "Earring";
            if (Description == "Bow")
                Description = "ArcherBow";
            if (Description == "Backsword")
                Description = "TaoistBackSword";
            Description = Description.ToLower();

            if (data.Length >= 58)
            {
                if (ID <= 824017 && ID >= 800000)
                {
                    PurificationLevel = Convert.ToUInt16(data[56]);
                    PurificationMeteorNeed = Convert.ToUInt16(data[57]);
                }
            }

        }
        public enum ItemType : byte
        {
            Dropable = 0,
            Others
        }
    }
}