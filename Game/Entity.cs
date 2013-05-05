using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PhoenixProject.Network;
using PhoenixProject.Network.GamePackets;
using System.Collections;
using PhoenixProject.Interfaces;
using PhoenixProject.ServerBase;
using PhoenixProject.Database;
using PhoenixProject.Network.GamePackets.EventAlert;

namespace PhoenixProject.Game
{
    public class Entity : Writer, Interfaces.IBaseEntity, Interfaces.IMapObject
    {
        #region Variables
        public uint DetenedorUID = 0;

        public string Detenido = "";

        public string Detenedor = "";

        public uint Dinero = 0;
        public uint kilid = 0;
        public uint LotteryItemID = 0;
        public uint LotteryItemPlus;
        public Time32 BlackSpotTime;
        public uint BlackSpotID = 0;
        public uint BlackSpotCheck = 0;
        public bool BlackSpots = false;
        public bool CanBlade = false;
        public byte BlackSpotTime2;
        public uint LotteryItemColor;
        public uint LotteryItemSoc1;
        public bool UseItem = false;
        public uint LotteryItemSoc2;
        public uint StrResID;
        public uint A;
        public uint B;
        public uint C;
        public uint CPSS;
        public uint METCHANCE;
        public uint SwordChance;
        public byte LotteryJadeAdd;
        public Interfaces.IConquerItem LotteryPrize;
        public int number = 0;
        public Dictionary<uint, Game.PkExpeliate> PkExplorerValues = new Dictionary<uint, PkExpeliate>();
       
        uint _ClanSharedBp;
        public uint ClanSharedBp
        {
            get { return _ClanSharedBp; }
            set
            {
                switch (EntityFlag)
                {
                    case EntityFlag.Player:
                        {
                            //WriteUInt32(value, 38, SpawnPacket);//91
                           // WriteUInt32(value, 56, SpawnPacket);//91
                            Owner.SendScreen(SpawnPacket, false);
                        }
                        break;
                }
                _ClanSharedBp = value;
            }
        }
        uint _GuildSharedBp;
        public uint GuildSharedBp
        {
            get { return _GuildSharedBp; }
            set
            {
                switch (EntityFlag)
                {
                    case EntityFlag.Player:
                        {
                            //UpdateDatabase("TotalBP", value);
                            Update(Network.GamePackets.Update.GuildShareBP, value, false);
                        }
                        break;
                }
                _GuildSharedBp = value;
            }
        }
        public PhoenixProject.Game.Features.Flowers.Flowers Flowers;
        public uint LastXLocation, LastYLocation;
        public bool InSteedRace = false;
        public bool invite = false;
        public Time32 InviteSendStamp;
        public uint ScreenColor = 0;
        public Database.MonsterInformation MonsterInfo;
        public Time32 CounterKillStamp;
        public Time32 DeathStamp, BlackBeardStamp, CannonBarageStamp, VortexAttackStamp, ChainBoltStamp, AttackStamp, StaminaStamp, FlashingNameStamp, CycloneStamp, SupermanStamp,
                      MagicDefenderStamp,DefensiveStanceStamp, StigmaStamp, InvisibilityStamp, StarOfAccuracyStamp, MagicShieldStamp, DodgeStamp, EnlightmentStamp,
                      AccuracyStamp, ShieldStamp, FlyStamp, NoDrugsStamp, ToxicFogStamp, FatalStrikeStamp,FreezeStamp, DoubleExpStamp,
                      ShurikenVortexStamp, IntensifyStamp, TransformationStamp,ReflectStamp, PKPointDecreaseStamp, LastPopUPCheck,
                      HeavenBlessingStamp, OblivionStamp,AuraStamp, ShackleStamp, AzureStamp, StunStamp, WhilrwindKick, Confuse, LastTeamLeaderLocationSent = Time32.Now;
        public bool KillTheTerrorist_IsTerrorist = false;
        public bool Tournament_Signed = false;
        public Time32 MagikAttackTimeAtaque;
        public int MagikAttackTime = 0;

        public Time32 MaleeAttackTimeAtaque;
        public int MaleeAttackTime = 0;

        public Time32 RangedAttackTimeAtaque;
        public int RangedAttackTime = 0;
        public bool SpawnProtection = false;
        public bool TeamDeathMatch_Signed = false;
        public bool TeamDeathMatch_RedCaptain = false;
        public bool TeamDeathMatch_BlackCaptain = false;
        public bool TeamDeathMatch_WhiteCaptain = false;
        public bool TeamDeathMatch_BlueCaptain = false;
        public bool TeamDeathMatch_RedTeam = false;
        public bool TeamDeathMatch_BlackTeam = false;
        public bool TeamDeathMatch_WhiteTeam = false;
        public uint InteractionType = 0;
        public uint InteractionWith = 0;
        public bool InteractionInProgress = false;
        public ushort InteractionX = 0;
        public ushort InteractionY = 0;
        public uint PokerSeat = 0;
        public uint TableID = 0;
        public PhoenixProject.Generated.Interfaces.Table Table = null;
        public bool InteractionSet = false;
        public uint Points = 0;
        //public uint UID = 0;
        //public ushort Avatar = 0;
        //public ushort Mesh = 0;
        //public string Name = "";
        public ushort Postion = 0;
        public byte MyTitle = 0;
        public Tournaments.Elite_client Elite;
        public UInt32 ActivePOPUP;
        public byte TitleActivated
        {
            get { return SpawnPacket[179]; }
            set { SpawnPacket[179] = value; }
        }
        uint _CountryFlag;
        public uint CountryFlag
        {
            get { return _CountryFlag; }
            set
            {
                
                _CountryFlag = value;
                WriteUInt32(value, 225, SpawnPacket);//91
                Owner.SendScreen(SpawnPacket, false);
              
            }
        }
       
        public byte SubClassesActive
        {
            get { return SpawnPacket[218]; }
            set { SpawnPacket[218] = value; }
        }
        public TitlePacket TitlePacket = null;
        public Updating.Offset1 UpdateOffset1 = Updating.Offset1.None;
        public Updating.Offset2 UpdateOffset2 = Updating.Offset2.None;
        public Updating.Offset3 UpdateOffset3 = Updating.Offset3.None;
        public Updating.Offset4 UpdateOffset4 = Updating.Offset4.None;
        public Updating.Offset5 UpdateOffset5 = Updating.Offset5.None;
        public Updating.Offset6 UpdateOffset6 = Updating.Offset6.None;
        public Updating.Offset7 UpdateOffset7 = Updating.Offset7.None;
        public int DisKO = 0;
        public int DemonCave1 = 0;
        public int DemonCave2 = 0;
        public int DemonCave3 = 0;
        public static byte ScreenDistance = 0;
        public bool DisQuest = false;

        public static bool dis = false;
        public static bool dis2 = false;
        public static byte DisMax1 = 0;
        public static byte DisMax2 = 0;
        public static byte DisMax3 = 0;
        public Client.GameState ClientStats;
        public StatusStatics Statistics;
        public double DragonGems;
        public double PhoenixGems;
        public ushort Detoxication;
        public ushort Immunity;
        public ushort Breaktrough;
        public ushort CriticalStrike;
        public ushort SkillCStrike;
        public ushort Intensification;
        public ushort Block;
        public ushort FinalMagicDmgPlus;
        public ushort FinalMagicDmgReduct;
        public ushort FinalDmgPlus;
        public ushort FinalDmgReduct;
        public ushort Penetration;
        public ushort Counteraction;
        public ushort MetalResistance;
        public ushort WoodResistance;
        public ushort WaterResistance;
        public ushort FireResistance;
        public ushort EarthResistance;
        public bool TeamDeathMatch_BlueTeam = false;
        public int TeamDeathMatch_Kills = 0;
        public byte _SubClass;
        public Statement.ClientClasses SubClasses = new Statement.ClientClasses();
        public bool Stunned = false, Confused = false;
        public bool Companion;
        public bool CauseOfDeathIsMagic = false;
        public bool OnIntensify;
        private DateTime mLastLogin;
        uint f_flower;
        public uint ActualMyTypeFlower
        {
            get { return f_flower; }
            set
            {
                //30010202 orchids
                //30010002 rouse
                //30010102 lilyes
                //30010302 orchids

                f_flower = value;
                WriteUInt32(value, 127, SpawnPacket);//91
            }
        }
        private uint flower_R;
        public uint AddFlower
        {
            get { return flower_R; }
            set
            {
                flower_R = value;
            }
        }
        public short KOSpellTime
        {
            get
            {
                if (KOSpell == 1110)
                {
                    if (ContainsFlag(Network.GamePackets.Update.Flags.Cyclone))
                    {
                        return CycloneTime;
                    }
                }
                else if (KOSpell == 1025)
                {
                    if (ContainsFlag(Network.GamePackets.Update.Flags.Superman))
                    {
                        return SupermanTime;
                    }
                }
                return 0;
            }
            set
            {
                if (KOSpell == 1110)
                {
                    if (ContainsFlag(Network.GamePackets.Update.Flags.Cyclone))
                    {
                        int Seconds = CycloneStamp.AddSeconds(value).AllSeconds() - Time32.Now.AllSeconds();
                        if (Seconds >= 20)
                        {
                            CycloneTime = 20;
                            CycloneStamp = Time32.Now;
                        }
                        else
                        {
                            CycloneTime = (short)Seconds;
                            CycloneStamp = Time32.Now;
                        }
                    }
                }
                if (KOSpell == 1025)
                {
                    if (ContainsFlag(Network.GamePackets.Update.Flags.Superman))
                    {
                        int Seconds = SupermanStamp.AddSeconds(value).AllSeconds() - Time32.Now.AllSeconds();
                        if (Seconds >= 20)
                        {
                            SupermanTime = 20;
                            SupermanStamp = Time32.Now;
                        }
                        else
                        {
                            SupermanTime = (short)Seconds;
                            SupermanStamp = Time32.Now;
                        }
                    }
                }
            }
        }
        public short CycloneTime = 0, Cannonbarage = 0, Blackbeard = 0, SupermanTime = 0, NoDrugsTime = 0, FatalStrikeTime = 0, ShurikenVortexTime = 0, OblivionTime = 0, AuraTime = 0, ShackleTime = 0, AzureTime = 0;
        public ushort KOSpell = 0;
        public int AzureDamage = 0;
        public short SoulShackleTime = 0;
        public string NewName = "";
        private ushort _enlightenPoints;
        private byte _receivedEnlighenPoints;
        private ushort _enlightmenttime;
        public float ToxicFogPercent, StigmaIncrease, MagicDefenderIncrease,DefensiveStanceIncrease, MagicShieldIncrease, DodgeIncrease, ShieldIncrease;
        public byte ToxicFogLeft, FlashingNameTime, FreezeTime, ChainBoltTime, FlyTime, StigmaTime, MagicDefenderTime,DefensiveStanceTime, InvisibilityTime, StarOfAccuracyTime, MagicShieldTime, DodgeTime, AccuracyTime, ShieldTime;
        public ushort KOCount = 0;
        public bool CounterKillSwitch = false;
        public Network.GamePackets.Attack AttackPacket;
        public Network.GamePackets.Attack VortexPacket;
        public byte[] SpawnPacket;
        private string _Name, _Spouse;
        private ushort _Defence, _MDefence, _MDefencePercent;
        private Client.GameState _Owner;
        public ushort ItemHP = 0, ItemMP = 0, ItemBless = 0, PhysicalDamageDecrease = 0, PhysicalDamageIncrease = 0, MagicDamageDecrease = 0, MagicDamageIncrease = 0, AttackRange = 1, Vigor = 0, ExtraVigor = 0;
        public ushort[] Gems = new ushort[10];
        public ushort PhoenixGem = 0, DragonGem = 0, TortisGem = 0, RainbowGem = 0;
        private uint _MinAttack, _MaxAttack, _MagicAttack;
        public uint BaseMinAttack, BaseMaxAttack, BaseMagicAttack, BaseDefence, BaseMagicDefence;
        private uint _TransMinAttack, _TransMaxAttack, _TransDodge, _TransPhysicalDefence, _TransMagicDefence;
        public bool Killed = false;
        public bool Quizz = false;
        public bool Transformed
        {
            get
            {
                return TransformationID != 98 && TransformationID != 99 && TransformationID != 0;
            }
        }
        public uint TransformationAttackRange = 0;
        public int TransformationTime = 0;
        public uint TransformationMaxHP = 0;
        private byte _Dodge;
        private Enums.PKMode _PKMode;
        private EntityFlag _EntityFlag;
        public uint scatter = 0;
        private MapObjectType _MapObjectType;
        public Enums.Mode Mode;
        private ulong _experience, _NobalityDonation;
        public ushort xx, yy;
        private uint _heavenblessing, _money, _uid, _hitpoints, _maxhitpoints, _quizpoints;
        private uint _RacePoints, _RacePoints2, _SubClassLevel, _ChiPoints, _KoKills, _Bconquerpoints, _conquerpoints, _status, _status2, _status3, _status4, _Quest;
        private ushort  _doubleexp, _body, _transformationid, _face, _strength, _agility, _spirit, _vitality, _atributes, _mana, _maxmana, _hairstyle, _x, _y, _pkpoints;
        private byte _stamina, _class, _reborn, _level;
        private ulong _mapid, _previousmapid;
        byte cls, secls;
        public byte FirstRebornClass
        {
            get
            {
                return cls;
            }
            set
            {
                cls = value;
                SpawnPacket[219] = value;
            }
        }
        public byte SecondRebornClass
        {
            get
            {
                return secls;
            }
            set
            {
                secls = value;
                SpawnPacket[221] = value;
            }
        }
       /* public byte ThirdRebornClass
        {
            get
            {
                return seclss;
            }
            set
            {
                seclss = value;
                SpawnPacket[215] = value;
            }
        }*/
        public byte FirstRebornLevel, SecondRebornLevel;
        public bool FullyLoaded = false, SendUpdates = false, HandleTiming = false;
        private Network.GamePackets.Update update;

        #endregion
        #region Acessors
        #region Fan/Tower Acessor
        public int getFan(bool Magic)
        {
            if (Owner.Equipment.Free(10))
                return 0;

            ushort magic = 0;
            ushort physical = 0;
            ushort gemVal = 0;

            #region Get
            Interfaces.IConquerItem Item = this.Owner.Equipment.TryGetItem(10);

            if (Item != null)
            {
                if (Item.ID > 0)
                {
                    switch (Item.ID % 10)
                    {
                        case 3:
                        case 4:
                        case 5: physical += 300; magic += 150; break;
                        case 6: physical += 500; magic += 200; break;
                        case 7: physical += 700; magic += 300; break;
                        case 8: physical += 900; magic += 450; break;
                        case 9: physical += 1200; magic += 750; break;
                    }

                    switch (Item.Plus)
                    {
                        case 0: break;
                        case 1: physical += 200; magic += 100; break;
                        case 2: physical += 400; magic += 200; break;
                        case 3: physical += 600; magic += 300; break;
                        case 4: physical += 800; magic += 400; break;
                        case 5: physical += 1000; magic += 500; break;
                        case 6: physical += 1200; magic += 600; break;
                        case 7: physical += 1300; magic += 700; break;
                        case 8: physical += 1400; magic += 800; break;
                        case 9: physical += 1500; magic += 900; break;
                        case 10: physical += 1600; magic += 950; break;
                        case 11: physical += 1700; magic += 1000; break;
                        case 12: physical += 1800; magic += 1050; break;
                    }
                    switch (Item.SocketOne)
                    {
                        case Enums.Gem.NormalThunderGem: gemVal += 100; break;
                        case Enums.Gem.RefinedThunderGem: gemVal += 300; break;
                        case Enums.Gem.SuperThunderGem: gemVal += 500; break;
                    }
                    switch (Item.SocketTwo)
                    {
                        case Enums.Gem.NormalThunderGem: gemVal += 100; break;
                        case Enums.Gem.RefinedThunderGem: gemVal += 300; break;
                        case Enums.Gem.SuperThunderGem: gemVal += 500; break;
                    }
                }
            }
            #endregion

            magic += gemVal;
            physical += gemVal;

            if (Magic)
                return (int)magic;
            else
                return (int)physical;
        }

        public int getTower(bool Magic)
        {
            if (Owner.Equipment.Free(11))
                return 0;

            ushort magic = 0;
            ushort physical = 0;
            ushort gemVal = 0;

            #region Get
            Interfaces.IConquerItem Item = this.Owner.Equipment.TryGetItem(11);

            if (Item != null)
            {
                if (Item.ID > 0)
                {
                    switch (Item.ID % 10)
                    {
                        case 3:
                        case 4:
                        case 5: physical += 250; magic += 100; break;
                        case 6: physical += 400; magic += 150; break;
                        case 7: physical += 550; magic += 200; break;
                        case 8: physical += 700; magic += 300; break;
                        case 9: physical += 1100; magic += 600; break;
                    }

                    switch (Item.Plus)
                    {
                        case 0: break;
                        case 1: physical += 150; magic += 50; break;
                        case 2: physical += 350; magic += 150; break;
                        case 3: physical += 550; magic += 250; break;
                        case 4: physical += 750; magic += 350; break;
                        case 5: physical += 950; magic += 450; break;
                        case 6: physical += 1100; magic += 550; break;
                        case 7: physical += 1200; magic += 625; break;
                        case 8: physical += 1300; magic += 700; break;
                        case 9: physical += 1400; magic += 750; break;
                        case 10: physical += 1500; magic += 800; break;
                        case 11: physical += 1600; magic += 850; break;
                        case 12: physical += 1700; magic += 900; break;
                    }
                    switch (Item.SocketOne)
                    {
                        case Enums.Gem.NormalGloryGem: gemVal += 100; break;
                        case Enums.Gem.RefinedGloryGem: gemVal += 300; break;
                        case Enums.Gem.SuperGloryGem: gemVal += 500; break;
                    }
                    switch (Item.SocketTwo)
                    {
                        case Enums.Gem.NormalGloryGem: gemVal += 100; break;
                        case Enums.Gem.RefinedGloryGem: gemVal += 300; break;
                        case Enums.Gem.SuperGloryGem: gemVal += 500; break;
                    }
                }
            }
            #endregion

            magic += gemVal;
            physical += gemVal;

            if (Magic)
                return (int)magic;
            else
                return (int)physical;
        }
        #endregion
        public float GetAttack(bool Magic)
        {
            float val = 1.0f;
            switch (Magic)
            {
                case true:
                    {
                        foreach (ConquerItem items in Owner.Equipment.Objects)
                        {
                            if (items != null)
                            {
                                if (items.SocketOne != Enums.Gem.NoSocket)
                                    if (items.SocketOne != Enums.Gem.EmptySocket)
                                        val += ((int)(items.SocketOne) / 10);
                                if (items.SocketTwo != Enums.Gem.NoSocket)
                                    if (items.SocketTwo != Enums.Gem.EmptySocket)
                                        val += ((int)(items.SocketTwo) / 10);
                            }
                        }
                        val = val / 100;
                        break;
                    }
            }
            return val;
        }
        private Region.Region mRegion;
        public Region.Region MapRegion
        {
            get
            {
                return this.mRegion;
            }
            set
            {
                if (this.mRegion != value)
                {
                    string str = (this.mRegion != null) ? this.mRegion.Name : string.Empty;
                    this.mRegion = value;
                    if (this.MapRegion != null)
                    {
                      
                        if (this.MapRegion.Name != str)
                        {
                            if (this.MapRegion.Name != string.Empty)
                            {
                                Owner.Send(new Message("You entered the region "+this.MapRegion.Name+" (Lineage: "+this.MapRegion.Lineage+").", System.Drawing.Color.BurlyWood, PhoenixProject.Network.GamePackets.Message.TopLeft));
                               
                            }
                            else
                            {
                                Owner.Send(new Message("You left the region " + str + "", System.Drawing.Color.BurlyWood, PhoenixProject.Network.GamePackets.Message.TopLeft));
                               
                            }
                        }
                        if (ContainsFlag(Network.GamePackets.Update.Flags.Ride))
                        {
                            if (!Owner.Equipment.Free(12))
                                if (Owner.Map.ID == 1036 || Owner.Equipment.TryGetItem((byte)12).Plus < this.MapRegion.Lineage)
                                    RemoveFlag(Network.GamePackets.Update.Flags.Ride);
                           
                        }
                    }
                }
            }
        }
        public uint MaxVigor
        {
            get
            {
                uint __Vigor = 0;
                Interfaces.IConquerItem IT = Owner.Equipment.TryGetItem(12);

                if (IT != null)
                {
                    switch (IT.Plus)
                    {
                        case 1: __Vigor = 50; break;
                        case 2: __Vigor = 120; break;
                        case 3: __Vigor = 200; break;
                        case 4: __Vigor = 350; break;
                        case 5: __Vigor = 650; break;
                        case 6: __Vigor = 1000; break;
                        case 7: __Vigor = 1400; break;
                        case 8: __Vigor = 2000; break;
                        case 9: __Vigor = 2800; break;
                        case 10: __Vigor = 3100; break;
                        case 11: __Vigor = 3500; break;
                        case 12: __Vigor = 4000; break;
                    }
                }

                return (uint)(30 + __Vigor);
            }
        }
        public int BattlePower
        {
            get
            {
                return BattlePowerCalc(this);
            }
        }
        public int _TopTrojan = 0;
        public int TopTrojan
        {
            get { return _TopTrojan; }
            set
            {
                _TopTrojan = value;

            }
        }
        public ushort _TopWarrior = 0;
        public ushort TopWarrior
        {
            get { return _TopWarrior; }
            set
            {
                _TopWarrior = value;

            }
        }
        public int _TopNinja = 0;
        public int TopNinja
        {
            get { return _TopNinja; }
            set
            {
                _TopNinja = value;

            }
        }
        public int _TopWaterTaoist = 0;
        public int TopWaterTaoist
        {
            get { return _TopWaterTaoist; }
            set
            {
                _TopWaterTaoist = value;

            }
        }
        public int _TopArcher = 0;
        public int TopArcher
        {
            get { return _TopArcher; }
            set
            {
                _TopArcher = value;

            }
        }
        public int _TopGuildLeader = 0;
        public int TopGuildLeader
        {
            get { return _TopGuildLeader; }
            set
            {
                _TopGuildLeader = value;

            }
        }
        public int _TopFireTaoist = 0;
        public int TopFireTaoist
        {
            get { return _TopFireTaoist; }
            set
            {
                _TopFireTaoist = value;

            }
        }
        public int _TopDeputyLeader = 0;
        public int TopDeputyLeader
        {
            get { return _TopDeputyLeader; }
            set
            {
                _TopDeputyLeader = value;

            }
        }
        public int _WeeklyPKChampion = 0;
        public int WeeklyPKChampion
        {
            get { return _WeeklyPKChampion; }
            set
            {
                _WeeklyPKChampion = value;

            }
        }
        public int _MonthlyPKChampion = 0;
        public int MonthlyPKChampion
        {
            get { return _MonthlyPKChampion; }
            set
            {
                _MonthlyPKChampion = value;

            }
        }
        public int _Topmonk2 = 0;
        public int TopMonk
        {
            get { return _Topmonk2; }
            set
            {
                _Topmonk2 = value;

            }
        }
        public int _TopSpouse = 0;
        public int TopSpouse
        {
            get { return _TopSpouse; }
            set
            {
                _TopSpouse = value;

            }
        }
        public DateTime LastLogin
        {
            get { return this.mLastLogin; }
            set { this.mLastLogin = value; }
        }
       
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {

                _Name = value;
                if (ClanName != "")
                {
                    SpawnPacket = new byte[8 + 234 + Name.Length + ClanName.Length + 2];
                    WriteUInt16((ushort)(234 + Name.Length + ClanName.Length + 2), 0, SpawnPacket);
                    WriteUInt16(10014, 2, SpawnPacket);
                    // WriteUInt16(1871, 155, SpawnPacket);//clan id
                    SpawnPacket[232] = 4;
                    SpawnPacket[233] = (byte)_Name.Length;//basta = 5 = 217 + 5 + 2= 
                    WriteString(_Name, 234, SpawnPacket);
                    SpawnPacket[233 + SpawnPacket[233] + 2] = (byte)ClanName.Length;
                    WriteString(ClanName, 233 + SpawnPacket[231] + 3, SpawnPacket);
                }
                else
                {
                    SpawnPacket = new byte[8 + 234 + Name.Length];
                    WriteUInt16((ushort)(234 + Name.Length), 0, SpawnPacket);
                    WriteUInt16(10014, 2, SpawnPacket);
                    SpawnPacket[232] = 4;
                    SpawnPacket[233] = (byte)_Name.Length;
                    WriteString(_Name, 234, SpawnPacket);
                }
            }
        }
        public string Spouse
        {
            get
            {
                return _Spouse;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets._String.Spouse, value, false);
                }

                _Spouse = value;
            }
        }
        public uint Money
        {
            get
            {
                return _money;
            }
            set
            {
                _money = value;
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Money, value, false);


            }
        }
        private byte _vipLevel;
        public byte VIPLevel
        {
            get
            {
                return _vipLevel;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets.Update.VIPLevel, value, false);
                }
                _vipLevel = value;
            }
        }
        public byte reinc;
        public byte ReincarnationLev
        {
            get
            {
                return reinc;
            }
            set
            {
                reinc = value;
            }
        }
        public ulong NobalityDonation
        {
            get
            {
                return _NobalityDonation;
            }
            set
            {
                _NobalityDonation = value;
            }
        }
        public uint ChiPoints
        {
            get
            {
                return _ChiPoints;
            }
            set
            {
                if (value <= 0 || value > 999999999)
                {
                    value = 0;
                }

                _ChiPoints = value;
                if (EntityFlag == EntityFlag.Player)
                {
                    //Update(Network.GamePackets.Update.ConquerPoints, (uint)value, false);
                }
            }
        }
        public uint KoKills
        {
            get
            {
                return _KoKills;
            }
            set
            {
                if (value <= 0 || value > 999999999)
                {
                    value = 0;
                }

                _KoKills = value;
               
            }
        }
        public uint ConquerPoints
        {
            get
            {
                return _conquerpoints;
            }
            set
            {
                if (value <= 0 || value > 999999999)
                {
                    value = 0;
                }

                _conquerpoints = value;
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets.Update.ConquerPoints, (uint)value, false);
                }
            }
        }
        public uint BConquerPoints
        {
            get
            {
                return _Bconquerpoints;
            }
            set
            {
                if (value <= 0 || value > 999999999)
                {
                    value = 0;
                }

                _Bconquerpoints = value;
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets.Update.BoundConquerPoints, (uint)value, false);
                }
            }
        }
        public uint RacePoints
        {
            get
            {
                return _RacePoints;
            }
            set
            {
                if (value <= 0)
                    value = 0;

                _RacePoints = value;
                if (EntityFlag == EntityFlag.Player)
                {

                    Update(Network.GamePackets.Update.RacePoints, (uint)value, false);
                }
            }
        }
       
        public ushort Body
        {
            get
            {
                return _body;
            }
            set
            {
                WriteUInt32((uint)(TransformationID * 10000000 + Face * 10000 + value), 4, SpawnPacket);
                _body = value;
                if (EntityFlag == EntityFlag.Player)
                {
                    if (Owner != null)
                    {
                        Owner.ArenaStatistic.Model = (uint)(Face * 10000 + value);
                        Update(Network.GamePackets.Update.Mesh, Mesh, true);
                    }
                }
            }
        }
        public ushort DoubleExperienceTime
        {
            get
            {
                return _doubleexp;
            }
            set
            {
                ushort oldVal = DoubleExperienceTime;
                _doubleexp = value;
                if (FullyLoaded)
                    if (oldVal <= _doubleexp)
                        if (EntityFlag == EntityFlag.Player)
                        {
                            if (Owner != null)
                            {
                                Update(Network.GamePackets.Update.DoubleExpTimer, DoubleExperienceTime, false);
                                SyncPacket packet = new SyncPacket
                                {
                                    Identifier = UID,
                                    Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.DoubleExpTimer,
                                    Duration = DoubleExperienceTime,
                                    Multiple = 1 * 100
                                };
                                Owner.Send((byte[])packet);
                            }
                        }
            }
        }

        public uint HeavenBlessing
        {
            get
            {
                return _heavenblessing;
            }
            set
            {
                uint oldVal = HeavenBlessing;
                _heavenblessing = value;
                if (FullyLoaded)
                    if (value > 0)
                        if (!ContainsFlag(Network.GamePackets.Update.Flags.HeavenBlessing) || oldVal <= _heavenblessing)
                        {
                            AddFlag(Network.GamePackets.Update.Flags.HeavenBlessing);
                            Update(Network.GamePackets.Update.HeavensBlessing, HeavenBlessing, false);
                            Update(Network.GamePackets._String.Effect, "bless", true);
                        }
            }
        }

        public byte Stamina
        {
            get
            {
                return _stamina;
            }
            set
            {
                _stamina = value;
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Stamina, value, false);
            }
        }
        public ushort TransformationID
        {
            get
            {
                return _transformationid;
            }
            set
            {
                _transformationid = value;
                WriteUInt32((uint)(value * 10000000 + Face * 10000 + Body), 4, SpawnPacket);
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Mesh, Mesh, true);
            }
        }
        public ushort Face
        {
            get
            {
                return _face;
            }
            set
            {
                WriteUInt32((uint)(TransformationID * 10000000 + value * 10000 + Body), 4, SpawnPacket);
                _face = value;
                if (EntityFlag == EntityFlag.Player)
                {
                    if (Owner != null)
                    {
                        Owner.ArenaStatistic.Model = (uint)(value * 10000 + Body);
                        Update(Network.GamePackets.Update.Mesh, Mesh, true);
                    }
                }
            }
        }
        public uint Mesh
        {
            get
            {
                return BitConverter.ToUInt32(SpawnPacket, 4);
            }
        }
        public byte Class
        {
            get
            {
                return _class;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                {
                    if (Owner != null)
                    {
                        Update(Network.GamePackets.Update.Class, value, false);
                        if (Owner.ArenaStatistic != null)
                            Owner.ArenaStatistic.Class = value;
                        
                    }
                }
                _class = value;
                SpawnPacket[223] = value;
                //SpawnPacket[219] = value;
                //SpawnPacket[221] = value;
                //SpawnPacket[218] = value;
            }
        }
        public byte Reborn
        {
            get
            {
                SpawnPacket[106] = _reborn;
                return _reborn;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets.Update.Reborn, value, true);
                }
                _reborn = value;
                SpawnPacket[106] = value;
            }
        }
        public byte Level
        {
            get
            {
                SpawnPacket[107] = _level;
                return _level;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets.Update.Level, value, true);
                    Data update = new Data(true);
                    update.UID = UID;
                    update.ID = Data.Leveled;
                    if (Owner != null)
                    {
                        (Owner as Client.GameState).SendScreen(update, true);
                        Owner.ArenaStatistic.Level = value;
                        Owner.ArenaStatistic.ArenaPoints = 1000;
                    }
                    if (Owner != null)
                    {
                        if (Owner.AsMember != null)
                        {
                            Owner.AsMember.Level = value;
                        }
                    }
                    SpawnPacket[107] = value;
                    //if (FullyLoaded)
                        //UpdateDatabase("Level", value);
                }
                else
                {
                    SpawnPacket[107] = value;
                }
                _level = value;

            }
        }

        public uint ExtraBattlePower
        {
            get
            {
                return BitConverter.ToUInt32(SpawnPacket, 111);
            }
            set
            {
                if (value > 200)
                    value = 0;
                if (ExtraBattlePower > 1000)
                    WriteUInt32(0, 111, SpawnPacket);
                if (ExtraBattlePower > 0 && value == 0 || value > 0)
                {
                    if (value == 0 && ExtraBattlePower == 0)
                        return;
                    Update(Network.GamePackets.Update.ExtraBattlePower, value, false);
                    WriteUInt32(value, 111, SpawnPacket);
                }
            }
        }

        public byte Away
        {
            get
            {
                return SpawnPacket[110];
            }
            set
            {
                SpawnPacket[110] = value;
            }
        }
        public byte Boss
        {
            get
            {
                return SpawnPacket[189];
            }
            set
            {
                SpawnPacket[189] = 1;
                SpawnPacket[190] = 2;
                SpawnPacket[191] = 3;
            }
        }
        private uint _EditeName;
        public uint EditeName
        {
            get
            {

                return _EditeName;
            }
            set
            {
                _EditeName = value;
            }
        }
        public uint UID
        {
            get
            {
                if (SpawnPacket != null)
                    return BitConverter.ToUInt32(SpawnPacket, 8);
                else
                    return _uid;
            }
            set
            {
                _uid = value;
                WriteUInt32(value, 8, SpawnPacket);
            }
        }

        public ushort GuildID
        {
            get
            {
                return BitConverter.ToUInt16(SpawnPacket, 12);
            }
            set
            {
                WriteUInt32(value, 12, SpawnPacket);
            }
        }

        public ushort GuildRank
        {
            get
            {
                return BitConverter.ToUInt16(SpawnPacket, 16);
            }
            set
            {
                WriteUInt16(value, 16, SpawnPacket);
            }
        }
        public ushort Strength
        {
            get
            {
                return _strength;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets.Update.Strength, value, false);
                }
                _strength = value;
            }
        }
        public ushort Agility
        {
            get
            {
                if (OnCyclone())
                    return (ushort)(_agility);
                return _agility;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Agility, value, false);
                _agility = value;
            }
        }
        public ushort Spirit
        {
            get
            {
                return _spirit;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Spirit, value, false);
                _spirit = value;
            }
        }
        public ushort Vitality
        {
            get
            {
                return _vitality;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Vitality, value, false);
                _vitality = value;
            }
        }
        public ushort Atributes
        {
            get
            {
                return _atributes;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Atributes, value, false);
                _atributes = value;
            }
        }
        public uint Hitpoints
        {
            get
            {
                return _hitpoints;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Hitpoints, value, false);
                _hitpoints = value;

                if (Boss > 0)
                {
                    uint key = (uint)(MaxHitpoints / 10000);
                    if (key != 0)
                        WriteUInt16((ushort)(value / key), 84, SpawnPacket);
                    else
                        WriteUInt16((ushort)(value * MaxHitpoints / 1000 / 1.09), 84, SpawnPacket);
                }
                else
                    WriteUInt16((ushort)value, 84, SpawnPacket);
            }
        }
        public ushort Mana
        {
            get
            {
                return _mana;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Mana, value, false);
                _mana = value;
            }
        }
        public ushort MaxMana
        {
            get
            {
                return _maxmana;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.MaxMana, value, false);
                _maxmana = value;
            }
        }
        public ushort HairStyle
        {
            get
            {
                return _hairstyle;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets.Update.HairStyle, value, true);
                }
                _hairstyle = value;
                WriteUInt16(value, 96, SpawnPacket);
            }
        }
        public Byte SubClass
        {
            get { return _SubClass; }
            set
            {
                _SubClass = value;
                SpawnPacket[210] =
                    EntityFlag != Game.EntityFlag.Monster ? _SubClass : (Byte)0;
            }
        }

        public uint SubClassLevel
        {
            get { return _SubClassLevel; }
            set
            {
                _SubClassLevel = value;
               
            }
        }
        public ConquerStructures.NobilityRank NobilityRank
        {
            get
            {
                return (PhoenixProject.Game.ConquerStructures.NobilityRank)SpawnPacket[131];
            }
            set
            {
                SpawnPacket[131] = (byte)value;
                if (Owner != null)
                {
                    if (Owner.AsMember != null)
                    {
                        Owner.AsMember.NobilityRank = value;
                    }
                }
            }
        }

        public byte HairColor
        {
            get
            {
                return (byte)(HairStyle / 100);
            }
            set
            {
                HairStyle = (ushort)((value * 100) + (HairStyle % 100));
            }
        }
        public ulong MapID
        {
            get
            {
                return _mapid;
            }
            set
            {
                _mapid = value;
            }
        }
        public uint Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public uint Status2
        {
            get
            {
                return _status2;
            }
            set
            {
                _status2 = value;
            }
        }

        public uint Status3
        {
            get
            {
                return _status3;
            }
            set
            {
                _status3 = value;
            }
        }

        public uint Status4
        {
            get
            {
                return _status4;
            }
            set
            {
                _status4 = value;
            }
        }
        public uint Quest
        {
            get
            {
                return _Quest;
            }
            set
            {
                _Quest = value;
            }
        }
        public ulong PreviousMapID
        {
            get
            {
                return _previousmapid;
            }
            set
            {
                _previousmapid = value;
            }
        }
        public ushort X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                WriteUInt16(value, 92, SpawnPacket);
            }
        }
        public ushort Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                WriteUInt16(value, 94, SpawnPacket);
            }
        }
        public ushort PX
        {
            get;
            set;
        }
        public ushort PY
        {
            get;
            set;
        }
        public bool Dead
        {
            get
            {
                return Hitpoints < 1;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public ushort Defence
        {
            get
            {
                return _Defence;
            }
            set { _Defence = value; }
        }
        public ushort TransformationDefence
        {
            get
            {
                if (ContainsFlag(Network.GamePackets.Update.Flags.MagicShield))
                {
                    if (ShieldTime > 0)
                        return (ushort)(_TransPhysicalDefence * ShieldIncrease);
                    else
                        return (ushort)(_TransPhysicalDefence * MagicShieldIncrease);
                }
                return (ushort)_TransPhysicalDefence;
            }
            set { _TransPhysicalDefence = value; }
        }
        public ushort MagicDefencePercent
        {
            get { return _MDefencePercent; }
            set { _MDefencePercent = value; }
        }
        public ushort TransformationMagicDefence
        {
            get { return (ushort)_TransMagicDefence; }
            set { _TransMagicDefence = value; }
        }
        public ushort MagicDefence
        {
            get { return _MDefence; }
            set { _MDefence = value; }
        }
        public Client.GameState Owner
        {
            get { return _Owner; }
            set { _Owner = value; }
        }
        public uint TransformationMinAttack
        {
            get
            {
                if (ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                    return (uint)(_TransMinAttack * StigmaIncrease);
                return _TransMinAttack;
            }
            set { _TransMinAttack = value; }
        }
        public uint TransformationMaxAttack
        {
            get
            {
                if (ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                    return (uint)(_TransMaxAttack * StigmaIncrease);
                return _TransMaxAttack;
            }
            set { _TransMaxAttack = value; }
        }
        public uint MinAttack
        {
            get
            {
                return _MinAttack;
            }
            set { _MinAttack = value; }
        }
        public uint MaxAttack
        {
            get
            {
                return _MaxAttack;
            }
            set { _MaxAttack = value; }
        }
        public uint MaxHitpoints
        {
            get
            {
                return _maxhitpoints;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                    if (TransformationID != 0 && TransformationID != 98)
                        Update(Network.GamePackets.Update.MaxHitpoints, value, true);
                _maxhitpoints = value;
            }
        }
        public uint MagicAttack
        {
            get
            {
                return _MagicAttack;
            }
            set { _MagicAttack = value; }
        }
        public byte Dodge
        {
            get
            {
                if (ContainsFlag(Network.GamePackets.Update.Flags.Dodge))
                {
                    //Console.WriteLine("Calc Dodge =" + (_Dodge * DodgeIncrease).ToString());
                    return (byte)(_Dodge * DodgeIncrease);
                }
                return _Dodge;
            }
            set { _Dodge = value; }
        }
        public byte TransformationDodge
        {
            get
            {
                if (ContainsFlag(Network.GamePackets.Update.Flags.Dodge))
                    return (byte)(_TransDodge * DodgeIncrease);
                return (byte)_TransDodge;
            }
            set { _TransDodge = value; }
        }
        public MapObjectType MapObjType
        {
            get { return _MapObjectType; }
            set { _MapObjectType = value; }
        }

        public EntityFlag EntityFlag
        {
            get { return _EntityFlag; }
            set { _EntityFlag = value; }
        }
        public ulong Experience
        {
            get
            {
                return _experience;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                    Update(Network.GamePackets.Update.Experience, value, false);
                _experience = value;
            }
        }

        public ushort EnlightenPoints
        {
            get
            {
                return _enlightenPoints;
            }
            set
            {
                _enlightenPoints = value;
            }
        }

        public byte ReceivedEnlightenPoints
        {
            get
            {
                return _receivedEnlighenPoints;
            }
            set
            {
                _receivedEnlighenPoints = value;
            }
        }

        public ushort EnlightmentTime
        {
            get
            {
                return _enlightmenttime;
            }
            set
            {
                _enlightmenttime = value;
            }
        }

        public ushort PKPoints
        {
            get
            {
                return _pkpoints;
            }
            set
            {
                _pkpoints = value;
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets.Update.PKPoints, value, false);
                    if (PKPoints > 99)
                    {
                        RemoveFlag(Network.GamePackets.Update.Flags.RedName);
                        AddFlag(Network.GamePackets.Update.Flags.BlackName);
                    }
                    else if (PKPoints > 29)
                    {
                        AddFlag(Network.GamePackets.Update.Flags.RedName);
                        RemoveFlag(Network.GamePackets.Update.Flags.BlackName);
                    }
                    else if (PKPoints < 30)
                    {
                        RemoveFlag(Network.GamePackets.Update.Flags.RedName);
                        RemoveFlag(Network.GamePackets.Update.Flags.BlackName);
                    }
                }
            }
        }
        public uint QuizPoints
        {
            get
            {
                return _quizpoints;
            }
            set
            {
                if (EntityFlag == EntityFlag.Player)
                {
                    Update(Network.GamePackets.Update.QuizPoints, value , true);
                }
                _quizpoints = value;
                if (value >= 0 && value < 901)
                {
                    WriteUInt32(value, 140, SpawnPacket);
                }
                if (value >= 901 && value < 9001)//Master
                {
                    WriteUInt32(value + 500000, 140, SpawnPacket);
                }
                if (value >= 9001 && value < 27001)//Scholar
                {
                    WriteUInt32(value + 5000000, 140, SpawnPacket);
                }
                if (value >= 27001)//Professor
                {
                    WriteUInt32(value + 50000000, 140, SpawnPacket);
                }
               // WriteUInt32(value, 140, SpawnPacket);
            }
        }
       
        public uint ClanId
        {
            get { return BitConverter.ToUInt32(SpawnPacket, 167); }
            set { WriteUInt32(value, 167, SpawnPacket); }
        }
        public Game.Clans Myclan;
        public byte ClanRank
        {
            get { return SpawnPacket[171]; }
            set { SpawnPacket[171] = value; /*100 is for clan lider*/ }
        }
      
        string clan = "";
        public string ClanName
        {
            get { return clan; }
            set
            {
                string oldclan = clan;
                clan = value;
                if (value != null)
                {

                    if (value != "")
                    {
                        byte[] dd33 = new byte[8 + 234 + Name.Length + value.Length + 2];
                        for (int i = 2; i < SpawnPacket.Length - 7; i++)
                        {
                            dd33[i] = SpawnPacket[i];
                        }

                        SpawnPacket = new byte[8 + 234 + Name.Length + value.Length + 2];
                        WriteUInt16((ushort)(234 + Name.Length + value.Length + 2), 0, SpawnPacket);

                        for (int i = 2; i < dd33.Length; i++)
                        {
                            SpawnPacket[i] = dd33[i];
                        }

                        WriteUInt16(10014, 2, SpawnPacket);

                        // WriteUInt16(1871, 155, SpawnPacket);

                        // SpawnPacket[151] = 2;


                        SpawnPacket[232] = 4;
                        SpawnPacket[233] = (byte)_Name.Length;
                        WriteString(_Name, 234, SpawnPacket);
                        SpawnPacket[233 + SpawnPacket[233] + 2] = (byte)value.Length;
                        WriteString(value, 233 + SpawnPacket[233] + 3, SpawnPacket);
                    }
                    else
                    {
                        byte[] dd33 = new byte[8 + 234 + Name.Length + 2];
                        for (int i = 2; i < SpawnPacket.Length - 8; i++)
                        {
                            if (i < dd33.Length)
                                dd33[i] = SpawnPacket[i];
                        }

                        SpawnPacket = new byte[8 + 234 + Name.Length + 2];
                        WriteUInt16((ushort)(232 + Name.Length + 2), 0, SpawnPacket);

                        for (int i = 2; i < dd33.Length; i++)
                        {
                            SpawnPacket[i] = dd33[i];
                        }

                        WriteUInt16(10014, 2, SpawnPacket);

                        // WriteUInt16(1871, 155, SpawnPacket);



                        SpawnPacket[232] = 4;
                        SpawnPacket[233] = (byte)_Name.Length;
                        WriteString(_Name, 234, SpawnPacket);
                        SpawnPacket[233 + SpawnPacket[233] + 2] = (byte)value.Length;
                        WriteString(value, 233 + SpawnPacket[233] + 3, SpawnPacket);
                        //SpawnPacket[212 + SpawnPacket[212] + 2] = (byte)value.Length;
                        //WriteString(value, 212 + SpawnPacket[212] + 3, SpawnPacket);
                    }
                }
            }
        }
        private UInt32 mClanJoinTarget;
        public UInt32 ClanJoinTarget
        {
            get { return this.mClanJoinTarget; }
            set { this.mClanJoinTarget = value; }
        }
        public Enums.PKMode PKMode
        {
            get { return _PKMode; }
            set { _PKMode = value; }
        }
        public ushort Action
        {
            get { return BitConverter.ToUInt16(SpawnPacket, 99); }
            set
            {
                WriteUInt16(value, 99, SpawnPacket);
            }
        }
        public Enums.ConquerAngle Facing
        {
            get { return (Enums.ConquerAngle)SpawnPacket[98]; }
            set
            {
                SpawnPacket[98] = (byte)value;
            }
        }
        public ulong StatusFlag
        {
            get
            {
                return BitConverter.ToUInt64(SpawnPacket, 22);
            }
            set
            {
                ulong OldV = StatusFlag;
                if (value != OldV)
                {
                    WriteUInt64(value, 22, SpawnPacket);
                    UpdateEffects(true);
                    //Update(Network.GamePackets.Update.StatusFlag, value, !ContainsFlag(Network.GamePackets.Update.Flags.XPList));
                }
            }
        }
        private ulong _Stateff2 = 0;
        public ulong StatusFlag2
        {
            get { return _Stateff2; }
            set
            {
                ulong OldV = StatusFlag2;
                if (value != OldV)
                {
                    _Stateff2 = value;
                    WriteUInt64(value, 30, SpawnPacket);

                    UpdateEffects(true);
                    // Update2(Network.GamePackets.Update.StatusFlag, value, true);// !ContainsFlag(Network.GamePackets.Update.Flags.XPList));//you need to update the SECOND value of stateff
                }
            }
        }
        private ulong _Stateff3 = 0;
        public ulong StatusFlag3
        {
            get { return _Stateff3; }
            set
            {
                ulong OldV = StatusFlag3;
                if (value != OldV)
                {
                    _Stateff3 = value;
                    WriteUInt64(value, 38, SpawnPacket);

                    UpdateEffects(true);
                    // Update2(Network.GamePackets.Update.StatusFlag, value, true);// !ContainsFlag(Network.GamePackets.Update.Flags.XPList));//you need to update the SECOND value of stateff
                }
            }
        }
        public void Save(String row, String value)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.UPDATE);
            Command.Update("entities")
                .Set(row, value)
                .Where("uid", UID)
                .Execute();
        }
        public void Save(String row, UInt16 value)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.UPDATE);
            Command.Update("entities")
                .Set(row, value)
                .Where("uid", UID)
                .Execute();
        }
        public void Save(String row, Boolean value)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.UPDATE);
            Command.Update("entities")
                .Set(row, value)
                .Where("uid", UID)
                .Execute();
        }
        public void Save(String row, UInt32 value)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.UPDATE);
            Command.Update("entities")
                .Set(row, value)
                .Where("uid", UID)
                .Execute();
        }
        #endregion
        #region Send Screen Acessor
        public void SendScreen(Interfaces.IPacket Data)
        {
            Client.GameState[] Chars = new Client.GameState[ServerBase.Kernel.GamePool.Count];
            ServerBase.Kernel.GamePool.Values.CopyTo(Chars, 0);
            foreach (Client.GameState C in Chars)
                if (C != null)
                    if (C.Entity != null)
                        if (Game.Calculations.PointDistance(X, Y, C.Entity.X, C.Entity.Y) <= 20)
                            C.Send(Data);
            Chars = null;

        }
        #endregion
        public void DieString()
        {
            _String str = new _String(true);
            str.UID = this.UID;
            str.Type = _String.Effect;
            str.Texts.Add("ghost");
            str.Texts.Add("1ghost");
            str.TextsCount = 1;
            if (EntityFlag == Game.EntityFlag.Player)
            {
                this.SendScreen(str);
            }
        }
        public double interval = 1000;
        public System.Timers.Timer MyTimer;
        #region Functions
        private uint Guild_______Points;

        public uint Guild_points
        {
            get { return Guild_______Points; }
            set
            {
                Guild_______Points = value;
            }
        }
        public UInt16 BattlePowerCalc(Entity e)
        {
            UInt16 BP = e.Level;

            if (e == null) return 0;
            if (e.Owner == null) return 0;
            foreach (IConquerItem i in e.Owner.Equipment.Objects)
            {
                if (!Owner.AlternateEquipment)
                {
                    if (i == null) continue;
                    if (i.Position < 20)
                    {
                        if (i.Position != ConquerItem.Bottle &&
                            i.Position != ConquerItem.Garment && i.Position != ConquerItem.RightWeaponAccessory && i.Position != ConquerItem.LeftWeaponAccessory && i.Position != ConquerItem.SteedArmor)
                        {
                            Byte Multiplier = 1;
                            if (i.Position == ConquerItem.RightWeapon && i.IsTwoHander())
                            {
                                Multiplier = 2;
                                IConquerItem left = e.Owner.Equipment.TryGetItem(ConquerItem.LeftWeapon);
                                if (left != null)
                                {

                                    Multiplier = 1;

                                }
                            }

                            Byte quality = (Byte)(i.ID % 10);
                            if (quality >= 6)
                            {
                                BP += (Byte)((quality - 5) * Multiplier);
                            }
                            if (i.SocketOne != 0)
                            {
                                BP += (Byte)(1 * Multiplier);
                                if ((Byte)i.SocketOne % 10 == 3)
                                    BP += (Byte)(1 * Multiplier);
                                if (i.SocketTwo != 0)
                                {
                                    BP += (Byte)(1 * Multiplier);
                                    if ((Byte)i.SocketTwo % 10 == 3)
                                        BP += (Byte)(1 * Multiplier);
                                }
                            }
                            BP += (Byte)(i.Plus * Multiplier);
                        }
                    }
                }
                else
                {
                    if (i == null) continue;
                    if (i.Position > 9)
                    {
                        if (i.Position != ConquerItem.AltBottle &&
                            i.Position != ConquerItem.AltGarment && i.Position != ConquerItem.RightWeaponAccessory && i.Position != ConquerItem.LeftWeaponAccessory && i.Position != ConquerItem.SteedArmor)
                        {
                            Byte Multiplier = 1;
                            if (i.Position == ConquerItem.AltRightHand && i.IsTwoHander())
                            {
                                Multiplier = 2;
                                IConquerItem left = e.Owner.Equipment.TryGetItem(ConquerItem.AltLeftHand);
                                if (left != null)
                                {

                                    Multiplier = 1;

                                }
                            }

                            Byte quality = (Byte)(i.ID % 10);
                            if (quality >= 6)
                            {
                                BP += (Byte)((quality - 5) * Multiplier);
                            }
                            if (i.SocketOne != 0)
                            {
                                BP += (Byte)(1 * Multiplier);
                                if ((Byte)i.SocketOne % 10 == 3)
                                    BP += (Byte)(1 * Multiplier);
                                if (i.SocketTwo != 0)
                                {
                                    BP += (Byte)(1 * Multiplier);
                                    if ((Byte)i.SocketTwo % 10 == 3)
                                        BP += (Byte)(1 * Multiplier);
                                }
                            }
                            BP += (Byte)(i.Plus * Multiplier);
                        }
                    }
                }
            }

            BP += (Byte)e.NobilityRank;
            BP += (Byte)(e.Reborn * 5);
            BP += (Byte)GuildSharedBp;
            BP += (Byte)ClanSharedBp;

            return BP;
        }
        public Entity(EntityFlag Flag, bool companion)
        {
            Statistics = new StatusStatics();
            Companion = companion;
            this.EntityFlag = Flag;
            Mode = Enums.Mode.None;
            update = new PhoenixProject.Network.GamePackets.Update(true);
            update.UID = UID;
            switch (Flag)
            {
                case EntityFlag.Player:
                   /* MyTimer = new System.Timers.Timer(200);
                    MyTimer.AutoReset = true;
                    MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack);
                    MyTimer.Start();

                    MyTimer = new System.Timers.Timer(1000);
                    MyTimer.AutoReset = true;
                    MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack2);
                    MyTimer.Start();

                    MyTimer = new System.Timers.Timer(1000);
                    MyTimer.AutoReset = true;
                    MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack3);
                    MyTimer.Start();

                      MyTimer = new System.Timers.Timer(1000);
                    MyTimer.AutoReset = true;
                    MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack4);
                    MyTimer.Start();

                    MyTimer = new System.Timers.Timer(1000);
                    MyTimer.AutoReset = true;
                    MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack5);
                    MyTimer.Start();

                    MyTimer = new System.Timers.Timer(100);
                    MyTimer.AutoReset = true;
                    MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack6);
                    MyTimer.Start();*/

                    MapObjType = Game.MapObjectType.Player;
                    break;
                case EntityFlag.Monster:
                   /*  MyTimer = new System.Timers.Timer(200);
                    MyTimer.AutoReset = true;
                    MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack);
                    MyTimer.Start();*/
                    MapObjType = Game.MapObjectType.Monster;
                    break;
            }
        }
        #region  TimerPool
        public void _timerCallBack6(object obj, System.Timers.ElapsedEventArgs arg)
        {
            try
            {
                Time32 Now = Time32.Now;
                if (this == null || Owner == null ||  Owner.Account == null)
                {
                    Owner.Disconnect();
                    return;
                }
                if (this == null || Owner == null)
                    return;
                if (Owner.Screen == null || Owner.Entity == null)
                {
                    Owner.Disconnect();
                }
                if (Owner != null)
                {


                    if (this.HandleTiming)
                    {
                      /*  MapStatus mbox = new MapStatus();
                        mbox.BaseID = (ushort)Owner.Map.BaseID;
                       // mbox.ID = Owner.Map.ID;
                        Owner.Send(mbox);*/
                        if (DateTime.Now.Minute == 42 && DateTime.Now.Second == 58)
                        {
                            if (Database.rates.Weather == 1)
                            {
                               
                                    Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                                    weather.WeatherType = (uint)Program.WeatherType;
                                    weather.Intensity = 100;
                                    weather.Appearence = 2;
                                    weather.Direction = 4;
                                    Owner.Send(weather);
                               
                            }
                        }
                        if (DateTime.Now.Minute == 57 && DateTime.Now.Second == 58)
                        {
                            if (Database.rates.Weather == 1)
                            {
                               
                                    Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                                    weather.WeatherType = (uint)Program.WeatherType;
                                    weather.Intensity = 100;
                                    weather.Appearence = 2;
                                    weather.Direction = 4;
                                    Owner.Send(weather);
                                
                            }
                        }
                        if (DateTime.Now.Minute == 12 && DateTime.Now.Second == 58)
                        {
                            if (Database.rates.Weather == 1)
                            {
                               
                                    Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                                    weather.WeatherType = (uint)Program.WeatherType;
                                    weather.Intensity = 100;
                                    weather.Appearence = 2;
                                    weather.Direction = 4;
                                    Owner.Send(weather);
                                
                            }
                        }
                        if (DateTime.Now.Minute == 27 && DateTime.Now.Second == 58)
                        {
                            if (Database.rates.Weather == 1)
                            {
                              
                                   
                                    Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                                    weather.WeatherType = (uint)Program.WeatherType;
                                    weather.Intensity = 100;
                                    weather.Appearence = 2;
                                    weather.Direction = 4;
                                    Owner.Send(weather);
                                
                            }
                        }
                        if (MapID == 1036)
                        {
                            if (ServerBase.Kernel.GetDistance(X, Y, 184, 205) < 17 && !Owner.Effect)
                            {
                                Owner.Effect = true;
                                if (MapID == 1036)
                                {
                                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                    // floorItem.MapObjType = Game.MapObjectType.Item;
                                    floorItem.ItemID = 812;//794
                                    floorItem.MapID = 1036;
                                    floorItem.X = 184;
                                    floorItem.Y = 205;
                                    floorItem.Type = Network.GamePackets.FloorItem.Effect;
                                    Owner.Send(floorItem);
                                }
                            }
                            else
                            {
                                if (ServerBase.Kernel.GetDistance(X, Y, 184, 205) > 17)
                                {
                                    Owner.Effect = false;
                                }
                            }
                        }
                        else
                        {
                            if (MapID == 1002)
                            {
                                if (ServerBase.Kernel.GetDistance(X,Y, 438, 377) < 17 && !Owner.Effect)
                                {
                                    Owner.Effect = true;
                                    if (MapID == 1002)
                                    {
                                        Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                        // floorItem.MapObjType = Game.MapObjectType.Item;
                                        floorItem.ItemID = 23;//746
                                        floorItem.MapID = 1002;
                                        floorItem.X = 438;
                                        floorItem.Y = 377;
                                        floorItem.Type = 10;
                                        Owner.Send(floorItem);
                                        floorItem.ItemID = 31;//794
                                        floorItem.MapID = 1002;
                                        floorItem.X = 438;
                                        floorItem.Y = 377;
                                        floorItem.Type = 10;
                                        Owner.Send(floorItem);
                                    }
                                }
                                else
                                {
                                    if (ServerBase.Kernel.GetDistance(X, Y, 438, 377) > 17)
                                    {
                                        Owner.Effect = false;
                                    }
                                }
                                if (ServerBase.Kernel.GetDistance(X, Y, 436, 444) < 17 && !Owner.Effect3)
                                {
                                    Owner.Effect3 = true;
                                    if (MapID == 1002)
                                    {
                                        Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                        // floorItem.MapObjType = Game.MapObjectType.Item;
                                        floorItem.ItemID = 765;//794
                                        floorItem.MapID = 1002;
                                        floorItem.X = 436;
                                        floorItem.Y = 444;
                                        floorItem.Type = Network.GamePackets.FloorItem.Effect;

                                        Owner.Send(floorItem);
                                    }
                                }
                                else
                                {
                                    if (ServerBase.Kernel.GetDistance(X, Y, 436, 444) > 17)
                                    {
                                        Owner.Effect3 = false;
                                    }
                                }
                            }
                            else
                            {
                                if (MapID == 1005)
                                {
                                    if (ServerBase.Kernel.GetDistance(X, Y, 42, 48) < 17 && !Owner.Effect2)
                                    {
                                        Owner.Effect2 = true;
                                        if (MapID == 1005)
                                        {//1005 42 50 790
                                            Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                            // floorItem.MapObjType = Game.MapObjectType.Item;
                                            floorItem.ItemID = 797;//794
                                            floorItem.MapID = 1005;
                                            floorItem.X = 42;
                                            floorItem.Y = 48;
                                            floorItem.Type = Network.GamePackets.FloorItem.Effect;
                                            Owner.Send(floorItem);
                                            // floorItem.MapObjType = Game.MapObjectType.Item;
                                            floorItem.ItemID = 731;//794
                                            floorItem.MapID = 1005;
                                            floorItem.X = 42;
                                            floorItem.Y = 51;
                                            floorItem.Type = Network.GamePackets.FloorItem.Effect;
                                            Owner.Send(floorItem);
                                        }
                                    }
                                    else
                                    {
                                        if (ServerBase.Kernel.GetDistance(X, Y, 42, 48) > 17)
                                        {
                                            Owner.Effect2 = false;
                                        }
                                    }

                                }
                            }
                        }
                        // Console.WriteLine(" " + UID + " ");
                        #region Auto attack
                        if (Owner.Entity.AttackPacket != null || Owner.Entity.VortexAttackStamp != null)
                        {
                            try
                            {
                                if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                                {
                                    if (Owner.Entity.VortexPacket != null && Owner.Entity.VortexPacket.ToArray() != null)
                                    {
                                        if (Time32.Now > Owner.Entity.VortexAttackStamp.AddMilliseconds(1400))
                                        {
                                            Owner.Entity.VortexAttackStamp = Time32.Now;
                                            new Game.Attacking.Handle(Owner.Entity.VortexPacket, Owner.Entity, null);
                                        }
                                    }
                                }
                                else
                                {
                                    Owner.Entity.VortexPacket = null;
                                    var AttackPacket = Owner.Entity.AttackPacket;
                                    if (AttackPacket != null && AttackPacket.ToArray() != null)
                                    {
                                        uint AttackType = AttackPacket.AttackType;
                                        if (AttackType == Network.GamePackets.Attack.Magic || AttackType == Network.GamePackets.Attack.Melee || AttackType == Network.GamePackets.Attack.Ranged)
                                        {
                                            if (AttackType == Network.GamePackets.Attack.Magic)
                                            {
                                                if (Time32.Now > Owner.Entity.AttackStamp.AddSeconds(1))
                                                {

                                                    new Game.Attacking.Handle(AttackPacket, Owner.Entity, null);
                                                }
                                            }
                                            else
                                            {
                                                int decrease = -300;
                                                if (Owner.Entity.OnCyclone())
                                                    decrease = 700;
                                                if (Owner.Entity.OnSuperman())
                                                    decrease = 200;
                                                if (Time32.Now > Owner.Entity.AttackStamp.AddMilliseconds((1000 - Owner.Entity.Agility - decrease) * (int)(AttackType == Network.GamePackets.Attack.Ranged ? 1 : 1)))
                                                {
                                                    new Game.Attacking.Handle(AttackPacket, Owner.Entity, null);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Program.SaveException(e);
                                AttackPacket = null;
                                VortexPacket = null;
                            }
                        }
                        #endregion


                    }
                }
                //AutoAttack();
            }
            catch { }
            {
            }
        }
        public void _timerCallBack5(object obj, System.Timers.ElapsedEventArgs arg)
        {
            try
            {
                Time32 Now = Time32.Now;
                if (this == null || Owner == null)
                {
                    Owner.Disconnect();
                    return;
                }
                if (this == null || Owner == null)
                    return;
                if (Owner.Screen == null || Owner.Entity == null)
                {
                    Owner.Disconnect();
                }
                if (Owner != null)
                {


                    if (this.HandleTiming)
                    {

                        #region BlessThread_Execute
                        if (Owner.Screen == null || Owner.Entity == null)
                        {
                            Owner.Disconnect();
                        }
                        if (Owner.Socket.Connected)
                        {

                            for (int c = 0; c < Owner.Screen.Objects.Count; c++)
                            {
                                if (c >= Owner.Screen.Objects.Count)
                                    break;
                                Interfaces.IMapObject ClientObj = Owner.Screen.Objects[c];
                                if (ClientObj != null)
                                {
                                    if (ClientObj.MapObjType == PhoenixProject.Game.MapObjectType.Player)
                                    {
                                        if (ClientObj is Game.Entity)
                                        {
                                            if (ClientObj.Owner.Entity.BlackSpotTime2 > 0)
                                            {
                                                BlackSpot spot = new BlackSpot
                                                {
                                                    Remove = 0,
                                                    Identifier = ClientObj.UID
                                                };
                                                Owner.Send(spot);
                                            }
                                            else
                                            {
                                                BlackSpot spot = new BlackSpot
                                                {
                                                    Remove = 1,
                                                    Identifier = ClientObj.UID
                                                };
                                                Owner.Send(spot);
                                            }
                                        }
                                    }
                                }
                            }
                            if (!Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Praying) && Owner.Entity.Reborn < 2)
                            {
                                for (int c = 0; c < Owner.Screen.Objects.Count; c++)
                                {
                                    if (c >= Owner.Screen.Objects.Count)
                                        break;
                                    Interfaces.IMapObject ClientObj = Owner.Screen.Objects[c];
                                    if (ClientObj != null)
                                    {
                                        if (ClientObj is Game.Entity)
                                        {
                                            if (ClientObj.MapObjType == PhoenixProject.Game.MapObjectType.Player)
                                            {
                                                var Client = ClientObj.Owner;
                                                if (Client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.CastPray))
                                                {
                                                    if (ServerBase.Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, ClientObj.X, ClientObj.Y) <= 3)
                                                    {
                                                        Owner.Entity.AddFlag(Network.GamePackets.Update.Flags.Praying);
                                                        Owner.PrayLead = Client;
                                                        Owner.Entity.Action = Client.Entity.Action;
                                                        Client.Prayers.Add(Owner);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Owner.PrayLead != null)
                                {
                                    if (ServerBase.Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, Owner.PrayLead.Entity.X, Owner.PrayLead.Entity.Y) > 4)
                                    {
                                        Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                                        Owner.PrayLead.Prayers.Remove(Owner);
                                        Owner.PrayLead = null;
                                    }
                                }
                            }
                            if (!Owner.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.kimo4))
                            {
                                for (int c = 0; c < Owner.Screen.Objects.Count; c++)
                                {
                                    if (c >= Owner.Screen.Objects.Count)
                                        break;
                                    Interfaces.IMapObject ClientObj = Owner.Screen.Objects[c];
                                    if (ClientObj != null)
                                    {
                                        if (ClientObj is Game.Entity)
                                        {
                                            if (ClientObj.MapObjType == PhoenixProject.Game.MapObjectType.Player)
                                            {
                                                var Client = ClientObj.Owner;

                                                if (Client.Entity.ContainsFlag3(Network.GamePackets.Update.Flags3.MagicDefender))
                                                {
                                                    if (ServerBase.Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, ClientObj.X, ClientObj.Y) <= 3)
                                                    {
                                                        Owner.Entity.AddFlag2(Network.GamePackets.Update.Flags.kimo4);
                                                        Owner.MagicLead = Client;
                                                        Owner.Entity.Action = PhoenixProject.Game.Enums.ConquerAction.Sit;
                                                        Client.MagicDef.Add(Owner);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (Owner.MagicLead != null)
                                {
                                    if (ServerBase.Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, Owner.MagicLead.Entity.X, Owner.MagicLead.Entity.Y) > 4)
                                    {
                                        Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.kimo4);
                                        Owner.MagicLead.MagicDef.Remove(Owner);
                                        Owner.MagicLead = null;
                                    }
                                }
                            }
                        }
                        #endregion



                    }
                }
                //AutoAttack();
            }
            catch { }
            {
            }
        }
        public void _timerCallBack4(object obj, System.Timers.ElapsedEventArgs arg)
        {
            try
            {
                Time32 Now = Time32.Now;
                if (this == null || Owner == null)
                {
                    Owner.Disconnect();
                    return;
                }
                if (this == null || Owner == null)
                    return;
                if (Owner.Screen == null || Owner.Entity == null)
                {
                    Owner.Disconnect();
                }
                if (Owner != null)
                {


                    if (this.HandleTiming)
                    {

                        #region StatusFlagChange_Execute

                        #region Bless
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.CastPray))
                        {
                            if (Owner.BlessTime <= 358500)
                                Owner.BlessTime += 1500;
                            else
                                Owner.BlessTime = 360000;
                        }
                        else if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Praying))
                        {
                            if (Owner.PrayLead != null)
                            {
                                if (Owner.PrayLead.Socket.Connected)
                                {
                                    if (Owner.BlessTime <= 359500)
                                        Owner.BlessTime += 500;
                                    else
                                        Owner.BlessTime = 360000;
                                }
                                else
                                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                            }
                            else
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                        }
                        else
                        {
                            if (Owner.BlessTime > 0)
                            {
                                if (Owner.BlessTime >= 500)
                                {
                                    Owner.BlessTime -= 500;
                                    Owner.Entity.Update(Network.GamePackets.Update.LuckyTimeTimer, Owner.BlessTime, false);
                                }
                                else
                                {
                                    Owner.BlessTime = 0;
                                    Owner.Entity.Update(Network.GamePackets.Update.LuckyTimeTimer, Owner.BlessTime, false);
                                }
                            }
                        }

                        #endregion
                        #region Flashing name
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.FlashingName))
                        {
                            if (Now > Owner.Entity.FlashingNameStamp.AddSeconds(Owner.Entity.FlashingNameTime))
                            {
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.FlashingName);
                            }
                        }
                        #endregion
                        #region XPList
                        if (!Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.XPList))
                        {
                            if (Now > Owner.XPCountStamp.AddSeconds(3))
                            {
                                #region Arrows
                                if (Owner.Equipment != null)
                                {
                                    if (!Owner.Equipment.Free(5))
                                    {
                                        if (Network.PacketHandler.IsArrow(Owner.Equipment.TryGetItem(5).ID))
                                        {
                                            Database.ConquerItemTable.UpdateDurabilityItem(Owner.Equipment.TryGetItem(5));
                                        }
                                    }
                                }
                                #endregion
                                Owner.XPCountStamp = Now;
                                Owner.XPCount++;
                                if (Owner.XPCount >= 100)
                                {
                                    Owner.Entity.AddFlag(Network.GamePackets.Update.Flags.XPList);
                                    Owner.XPCount = 0;
                                    Owner.XPListStamp = Now;
                                }
                            }
                        }
                        else
                        {
                            if (Now > Owner.XPListStamp.AddSeconds(20))
                            {
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.XPList);
                            }
                        }
                        #endregion
                        #region KOSpell
                        if (Owner.Entity.OnKOSpell())
                        {
                            if (Owner.Entity.OnCyclone())
                            {
                                int Seconds = Now.AllSeconds() - Owner.Entity.CycloneStamp.AddSeconds(Owner.Entity.CycloneTime).AllSeconds();
                                if (Seconds >= 1)
                                {
                                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Cyclone);
                                    if (Owner.Entity.KOCount > rates.KoCount)
                                    {
                                        rates.KoCount = Owner.Entity.KOCount;
                                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations," + Name + " has Killed " + Owner.Entity.KOCount + " monsters with an Xp Skill and he/she is now ranked #1 on KoRank!", System.Drawing.Color.White, Network.GamePackets.Message.Talk), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                    }
                                    if (Owner.Entity.KOCount > Owner.Entity.KoKills)
                                    {
                                        Owner.Entity.KoKills = Owner.Entity.KOCount;
                                    }
                                }
                            }
                            if (Owner.Entity.OnOblivion())
                            {
                                if (Now > Owner.Entity.OblivionStamp.AddSeconds(Owner.Entity.OblivionTime))
                                {
                                    Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.Oblivion);
                                    if (Owner.Entity.KOCount > rates.KoCount)
                                    {
                                        rates.KoCount = Owner.Entity.KOCount;
                                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations," + Name + " has Killed " + Owner.Entity.KOCount + " monsters with an Xp Skill and he/she is now ranked #1 on KoRank!", System.Drawing.Color.White, Network.GamePackets.Message.Talk), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                    }
                                    if (Owner.Entity.KOCount > Owner.Entity.KoKills)
                                    {
                                        Owner.Entity.KoKills = Owner.Entity.KOCount;
                                    }
                                }
                            }
                            if (Owner.Entity.OnSuperman())
                            {
                                int Seconds = Now.AllSeconds() - Owner.Entity.SupermanStamp.AddSeconds(Owner.Entity.SupermanTime).AllSeconds();
                                if (Seconds >= 1)
                                {
                                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Superman);
                                    if (Owner.Entity.KOCount > rates.KoCount)
                                    {
                                        rates.KoCount = Owner.Entity.KOCount;
                                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations," + Name + " has Killed " + Owner.Entity.KOCount + " monsters with an Xp Skill and he/she is now ranked #1 on KoRank!", System.Drawing.Color.White, Network.GamePackets.Message.Talk), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                    }
                                    if (Owner.Entity.KOCount > Owner.Entity.KoKills)
                                    {
                                        Owner.Entity.KoKills = Owner.Entity.KOCount;
                                    }
                                }
                            }
                            if (!Owner.Entity.OnKOSpell())
                            {
                                //Record KO
                                Owner.Entity.KOCount = 0;
                            }
                        }
                        #endregion
                        #region Buffers

                        if (Owner.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.WarriorWalk))
                        {
                            if (Now >= Owner.Entity.DefensiveStanceStamp.AddSeconds(Owner.Entity.DefensiveStanceTime))
                            {
                                Owner.Entity.DefensiveStanceTime = 0;
                                Owner.Entity.DefensiveStanceIncrease = 0;
                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.WarriorWalk);
                            }
                        }
                        if (Owner.Entity.ContainsFlag3(Network.GamePackets.Update.Flags3.MagicDefender))
                        {
                            if (Now >= Owner.Entity.MagicDefenderStamp.AddSeconds(Owner.Entity.MagicDefenderTime))
                            {
                                Owner.Entity.MagicDefenderTime = 0;
                                Owner.Entity.MagicDefenderIncrease = 0;
                                Owner.Entity.RemoveFlag3(Network.GamePackets.Update.Flags3.MagicDefender);
                                SyncPacket packet = new SyncPacket
                                {
                                    Identifier = Owner.Entity.UID,
                                    Count = 2,
                                    Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                                    StatusFlag1 = (ulong)Owner.Entity.StatusFlag,
                                    StatusFlag2 = (ulong)Owner.Entity.StatusFlag2,
                                    Unknown1 = 0x31,
                                    StatusFlagOffset = 0x80,
                                    Time = 0,
                                    Value = 0,
                                    Level = 0
                                };
                                Owner.Entity.Owner.Send((byte[])packet);

                                foreach (var Client in Owner.MagicDef)
                                {
                                    if (Client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.kimo4))
                                    {
                                        Client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.kimo4);
                                    }
                                }
                                Owner.MagicDef.Clear();
                            }
                        }


                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                        {
                            if (Now >= Owner.Entity.StigmaStamp.AddSeconds(Owner.Entity.StigmaTime))
                            {
                                Owner.Entity.StigmaTime = 0;
                                Owner.Entity.StigmaIncrease = 0;
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Stigma);
                            }
                        }
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Dodge))
                        {
                            if (Now >= Owner.Entity.DodgeStamp.AddSeconds(Owner.Entity.DodgeTime))
                            {
                                Owner.Entity.DodgeTime = 0;
                                Owner.Entity.DodgeIncrease = 0;
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Dodge);
                            }
                        }
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Invisibility))
                        {
                            if (Now >= Owner.Entity.InvisibilityStamp.AddSeconds(Owner.Entity.InvisibilityTime))
                            {
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Invisibility);
                            }
                        }
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.StarOfAccuracy))
                        {
                            if (Owner.Entity.StarOfAccuracyTime != 0)
                            {
                                if (Now >= Owner.Entity.StarOfAccuracyStamp.AddSeconds(Owner.Entity.StarOfAccuracyTime))
                                {
                                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.StarOfAccuracy);
                                }
                            }
                            else
                            {
                                if (Now >= Owner.Entity.AccuracyStamp.AddSeconds(Owner.Entity.AccuracyTime))
                                {
                                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.StarOfAccuracy);
                                }
                            }
                        }
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.MagicShield))
                        {
                            if (Owner.Entity.MagicShieldTime != 0)
                            {
                                if (Now >= Owner.Entity.MagicShieldStamp.AddSeconds(Owner.Entity.MagicShieldTime))
                                {
                                    Owner.Entity.MagicShieldIncrease = 0;
                                    Owner.Entity.MagicShieldTime = 0;
                                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.MagicShield);
                                }
                            }
                            else
                            {
                                if (Now >= Owner.Entity.ShieldStamp.AddSeconds(Owner.Entity.ShieldTime))
                                {
                                    Owner.Entity.ShieldIncrease = 0;
                                    Owner.Entity.ShieldTime = 0;
                                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.MagicShield);
                                }
                            }
                        }
                        #endregion
                        #region Fly
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                        {
                            if (Now >= Owner.Entity.FlyStamp.AddSeconds(Owner.Entity.FlyTime))
                            {
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Fly);
                                Owner.Entity.FlyTime = 0;
                            }
                        }
                        #endregion
                        #region PoisonStar
                        if (Owner.Entity.NoDrugsTime > 0)
                        {
                            if (Now > Owner.Entity.NoDrugsStamp.AddSeconds(Owner.Entity.NoDrugsTime))
                            {
                                Owner.Entity.NoDrugsTime = 0;
                            }
                        }
                        #endregion
                        #region ToxicFog
                        if (Owner.Entity.ToxicFogLeft > 0)
                        {
                            if (Now >= Owner.Entity.ToxicFogStamp.AddSeconds(2))
                            {
                                float Percent = Owner.Entity.ToxicFogPercent;
                                //Remove this line if you want it normal
                                Percent = Math.Min(0.1F, Owner.Entity.ToxicFogPercent);
                                Owner.Entity.ToxicFogLeft--;
                                Owner.Entity.ToxicFogStamp = Now;
                                if (Owner.Entity.Hitpoints > 1)
                                {
                                    uint damage = Game.Attacking.Calculate.Percent(Owner.Entity, Percent);
                                    Owner.Entity.Hitpoints -= damage;
                                    Network.GamePackets.SpellUse suse = new PhoenixProject.Network.GamePackets.SpellUse(true);
                                    suse.Attacker = Owner.Entity.UID;
                                    suse.SpellID = 10010;
                                    suse.Targets.Add(Owner.Entity.UID, damage);
                                    Owner.SendScreen(suse, true);
                                    if (Owner.QualifierGroup != null)
                                        Owner.QualifierGroup.UpdateDamage(ServerBase.Kernel.GamePool[Owner.ArenaStatistic.PlayWith], damage);
                                }
                            }
                        }
                        #endregion
                        if (Owner.Entity.OnBlackBread())
                        {
                            //int Seconds = Now.AllSeconds() - client.Entity.BlackBeardStamp.AddSeconds(client.Entity.Blackbeard).AllSeconds();
                            if (Now > Owner.Entity.BlackBeardStamp.AddSeconds(Owner.Entity.Blackbeard))
                            {
                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.BlackBread);
                            }
                        }
                        if (Owner.Entity.OnChainBolt())
                        {
                            //int Seconds = Now.AllSeconds() - client.Entity.BlackBeardStamp.AddSeconds(client.Entity.Blackbeard).AllSeconds();
                            if (Now > Owner.Entity.ChainBoltStamp.AddSeconds(Owner.Entity.ChainBoltTime))
                            {
                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.ChainBoltActive);
                            }
                        }
                        if (Owner.Entity.OnCannonBrag())
                        {
                            // int Seconds = Now.AllSeconds() - client.Entity.CannonBarageStamp.AddSeconds(client.Entity.Cannonbarage).AllSeconds();
                            if (Now > Owner.Entity.CannonBarageStamp.AddSeconds(Owner.Entity.Cannonbarage))
                            {
                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.CannonBraga);
                            }
                        }
                        #region FatalStrike
                        if (Owner.Entity.OnFatalStrike())
                        {
                            if (Now > Owner.Entity.FatalStrikeStamp.AddSeconds(Owner.Entity.FatalStrikeTime))
                            {
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.FatalStrike);
                            }
                        }
                        #endregion
                       
                        #region ShurikenVortex
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                        {
                            if (Now > Owner.Entity.ShurikenVortexStamp.AddSeconds(Owner.Entity.ShurikenVortexTime))
                            {
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.ShurikenVortex);
                            }
                        }
                        #endregion
                        #region Transformations
                        if (Owner.Entity.Transformed)
                        {
                            if (Now > Owner.Entity.TransformationStamp.AddSeconds(Owner.Entity.TransformationTime))
                            {
                                Owner.Entity.Untransform();
                            }
                        }
                        #endregion
                        #endregion



                    }
                }
                //AutoAttack();
            }
            catch { }
            {
            }
        }
        public void _timerCallBack3(object obj, System.Timers.ElapsedEventArgs arg)
        {
            try
            {
                Time32 Now = Time32.Now;
                if (this == null || Owner == null)
                {
                    Owner.Disconnect();
                    return;
                }
                if (this == null || Owner == null)
                    return;
                if (Owner.Screen == null || Owner.Entity == null)
                {
                    Owner.Disconnect();
                }
                if (Owner != null)
                {


                    if (this.HandleTiming)
                    {

                       // Console.WriteLine(" " + UID + " ");
                       
                        #region CompanionThread_Execute
                        if (Owner.Companion != null)
                        {
                            short distance = ServerBase.Kernel.GetDistance(Owner.Companion.X, Owner.Companion.Y, Owner.Entity.X, Owner.Entity.Y);
                            if (distance >= 8)
                            {
                                ushort X = (ushort)(Owner.Entity.X + ServerBase.Kernel.Random.Next(2));
                                ushort Y = (ushort)(Owner.Entity.Y + ServerBase.Kernel.Random.Next(2));
                                if (!Owner.Map.SelectCoordonates(ref X, ref Y))
                                {
                                    X = Owner.Entity.X;
                                    Y = Owner.Entity.Y;
                                }
                                Owner.Companion.X = X;
                                Owner.Companion.Y = Y;
                                Network.GamePackets.Data data = new PhoenixProject.Network.GamePackets.Data(true);
                                data.ID = Network.GamePackets.Data.Jump;
                                data.dwParam = (uint)((Y << 16) | X);
                                data.wParam1 = X;
                                data.wParam2 = Y;
                                data.UID = Owner.Companion.UID;
                                Owner.Companion.MonsterInfo.SendScreen(data);
                            }
                            else if (distance > 4)
                            {
                                Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(Owner.Companion.X, Owner.Companion.Y, Owner.Companion.Owner.Entity.X, Owner.Companion.Owner.Entity.Y);
                                if (!Owner.Companion.Move(facing))
                                {
                                    facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                    if (Owner.Companion.Move(facing))
                                    {
                                        Owner.Companion.Facing = facing;
                                        Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                        move.Direction = facing;
                                        move.UID = Owner.Companion.UID;
                                        move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                        Owner.Companion.MonsterInfo.SendScreen(move);
                                    }
                                }
                                else
                                {
                                    Owner.Companion.Facing = facing;
                                    Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                    move.Direction = facing;
                                    move.UID = Owner.Companion.UID;
                                    move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                    Owner.Companion.MonsterInfo.SendScreen(move);
                                }
                            }
                            else
                            {
                                var monster = Owner.Companion;

                                if (monster.MonsterInfo.InSight == 0)
                                {
                                    if (AttackPacket != null)
                                    {
                                        if (AttackPacket.AttackType == Network.GamePackets.Attack.Magic)
                                        {
                                            if (AttackPacket.Decoded)
                                            {
                                                if (SpellTable.SpellInformations.ContainsKey((ushort)AttackPacket.Damage))
                                                {
                                                    var info = Database.SpellTable.SpellInformations[(ushort)AttackPacket.Damage][Owner.Spells[(ushort)AttackPacket.Damage].Level];
                                                    if (info.CanKill == 1)
                                                    {
                                                        monster.MonsterInfo.InSight = AttackPacket.Attacked;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            monster.MonsterInfo.InSight = AttackPacket.Attacked;
                                        }
                                    }
                                }
                                else
                                {
                                    if (monster.MonsterInfo.InSight > 400000 && monster.MonsterInfo.InSight < 600000 || monster.MonsterInfo.InSight > 800000 && monster.MonsterInfo.InSight != monster.UID)
                                    {
                                        Entity attacked = null;

                                        if (Owner.Screen.TryGetValue(monster.MonsterInfo.InSight, out attacked))
                                        {
                                            if (Now > monster.AttackStamp.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                            {
                                                monster.AttackStamp = Now;
                                                if (attacked.Dead)
                                                {
                                                    monster.MonsterInfo.InSight = 0;
                                                }
                                                else
                                                    new Game.Attacking.Handle(null, monster, attacked);
                                            }
                                        }
                                        else
                                            monster.MonsterInfo.InSight = 0;
                                    }
                                }

                            }
                        }
                        #endregion


                    }
                }
                //AutoAttack();
            }
            catch { }
            {
            }
        }
        public void _timerCallBack2(object obj, System.Timers.ElapsedEventArgs arg)
        {
            try
            {
                Time32 Now = Time32.Now;
                if (this == null || Owner == null)
                {
                    Owner.Disconnect();
                    return;
                }
                if (this == null || Owner == null)
                    return;
                if (Owner.Screen == null || Owner.Entity == null)
                {
                    Owner.Disconnect();
                }
                if (Owner != null)
                {


                    if (this.HandleTiming)
                    {

                        
                        #region CharacterThread_Execute
                        if (BlackSpots)
                        {
                            if (Now >= BlackSpotTime.AddSeconds(BlackSpotTime2))
                            {

                                BlackSpot spot = new BlackSpot
                                {
                                    Remove = 1,
                                    Identifier = UID
                                };
                                Owner.Send((byte[])spot);
                                BlackSpots = false;
                                BlackSpotTime2 = 0;
                                BlackSpotCheck = 0;
                            }
                            else
                            {
                                if (BlackSpotCheck == 0)
                                {
                                    BlackSpot spot = new BlackSpot
                                    {
                                        Remove = 0,
                                        Identifier = UID
                                    };
                                    Owner.Send((byte[])spot);
                                    BlackSpotCheck = 1;
                                }

                            }

                        }
                        /* if (ActivePOPUP == 99995)
                         {
                             if (Now > LastPopUPCheck.AddSeconds(20))
                             {
                                 Owner.Disconnect();
                             }
                         }*/
                        /* if (Owner.popups == 0)
                         {//kimo
                             Owner.popups = 1;
                             Owner.Send(new Network.GamePackets.Message("" + PhoenixProject.Database.rates.PopUpURL + "", System.Drawing.Color.Red, Network.GamePackets.Message.Website));
            
                         }*/
                        if (Database.rates.Night == 1)
                        {
                            if (MapID == 701)
                            {
                                Random disco = new Random();
                                uint discocolor = (uint)disco.Next(50000, 999999999);
                                //Program.ScreenColor = discocolor;
                                //ScreenColor = Program.ScreenColor;
                                PhoenixProject.Network.GamePackets.Data datas = new PhoenixProject.Network.GamePackets.Data(true);
                                datas.UID = UID;
                                datas.ID = 104;
                                datas.dwParam = discocolor;
                                Owner.Send(datas);
                            }
                            else
                            {
                                if (DateTime.Now.Minute >= 40 && DateTime.Now.Minute <= 45)// Program.ScreenColor = 5855577
                                {
                                    PhoenixProject.Network.GamePackets.Data datas = new PhoenixProject.Network.GamePackets.Data(true);
                                    datas.UID = UID;
                                    datas.ID = 104;
                                    datas.dwParam = 5855577;
                                    Owner.Send(datas);
                                }
                                else
                                {
                                    PhoenixProject.Network.GamePackets.Data datas = new PhoenixProject.Network.GamePackets.Data(true);
                                    datas.UID = UID;
                                    datas.ID = 104;
                                    datas.dwParam = 0;
                                    Owner.Send(datas);
                                }
                            }
                        }
                        if (DateTime.Now.DayOfYear > 365)
                        {
                            Owner.Disconnect();
                            return;
                        }
                        if (DateTime.Now.Hour == 16 && DateTime.Now.Minute >= 20 && DateTime.Now.Second == 00)
                        {
                            if (MapID == 7777)
                            {
                                Teleport(1002, 391, 371);
                            }
                        }
                        if (Now > InviteSendStamp.AddSeconds(5) && invite)
                        {

                            Game.ClanWar.ScoreSendStamp = Time32.Now;
                            invite = false;

                            //Console.WriteLine("a7a");

                        }
                        /* if (Now > LastPopUPCheck.AddMinutes(30))
                         {
                             if (!ServerBase.Constants.PKForbiddenMaps.Contains(Owner.Map.BaseID))
                             {
                                 if (!ServerBase.Constants.PKFreeMaps.Contains(MapID))
                                 {
                                     if (MapID < 1000000)
                                     {
                                         ActivePOPUP = 99995;
                                         Owner.Send(new Network.GamePackets.NpcReply(6, "Are You Here? Please Press OK or Cancel To verrify You are Not Using any sort of Bots."));
                                         LastPopUPCheck = Time32.Now;
                                     }
                                 }
                             }
                         }*/
                        if (DateTime.Now.Hour == Game.KimoEvents.EBHour && DateTime.Now.Minute == 05 && DateTime.Now.Second == 15)
                        {
                            if (DateTime.Now.Hour == Game.KimoEvents.EBHour && DateTime.Now.Minute == 05 && DateTime.Now.Second == 15)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10533,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10533;
                                    Owner.Send((byte[])alert);
                                    //return;
                                }
                            }
                        }
                        if (DateTime.Now.Hour == Game.KimoEvents.DWHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                        {
                            if (DateTime.Now.Hour == Game.KimoEvents.DWHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                            {
                                Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "DonationCps War has Started! You Wana Join?");
                                npc.OptionID = 237;
                                Owner.Send(npc.ToArray());
                                //return;
                            }
                            Program.DemonCave3 = 0;
                        }
                        if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                        {
                            if (DateTime.Now.Hour == Game.KimoEvents.ClanHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                            {
                                Program.kimo = 1313;
                                Program.kimo2 = 7;
                                testpacket str = new testpacket(true);
                                Owner.Send(str);
                                //ClanWar;
                            }
                        }
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == (Game.KimoEvents.GWEEndHour - 1) && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                        {
                            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == (Game.KimoEvents.GWEEndHour - 1) && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10515,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10515;
                                    Owner.Send((byte[])alert);
                                    //return;
                                }

                            }
                        }
                        if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                        {
                            if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10541,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10541;
                                    Owner.Send((byte[])alert);
                                }
                                //return;

                            }
                        }
                        if (DateTime.Now.Hour == Game.KimoEvents.CFHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                        {
                            if (DateTime.Now.Hour == Game.KimoEvents.CFHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10535,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10535;
                                    Owner.Send((byte[])alert);
                                    //return;
                                }

                            }
                        }
                        if (DateTime.Now.Hour == Game.KimoEvents.DemonHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                        {
                            if (DateTime.Now.Hour == Game.KimoEvents.DemonHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                            {
                                Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "DemonCave Event  has Started! You Wana Join?");
                                npc.OptionID = 235;
                                Owner.Send(npc.ToArray());
                                //return;

                            }
                        }
                        if (DateTime.Now.Hour == Game.KimoEvents.LordsWarHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                        {
                            if (DateTime.Now.Hour == Game.KimoEvents.LordsWarHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                            {
                                Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "LordsWar Tourment  has Started! You Wana Join?");
                                npc.OptionID = 233;
                                Owner.Send(npc.ToArray());
                                //return;

                            }
                        }
                        /*if (DateTime.Now.Hour == Game.KimoEvents.THour && DateTime.Now.Minute == 30 && DateTime.Now.Second == 30)
                        {
                            if (DateTime.Now.Hour == Game.KimoEvents.THour && DateTime.Now.Minute == 30 && DateTime.Now.Second == 30)
                            {
                                Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "TreasureBox Event  has Started! You Wana Join?");
                                npc.OptionID = 238;
                                Owner.Send(npc.ToArray());
                                //return;

                            }
                         * //TreasureBox
                        }*/

                        if (DateTime.Now.Hour == Game.KimoEvents.THour && DateTime.Now.Minute == 45 && DateTime.Now.Second == 00)
                        {
                            if (MapID == 1225)
                            {
                                Teleport(1002, 428, 243);
                                //return;

                            }
                        }

                        if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 59 && DateTime.Now.Second == 30)
                        {
                            if (MapID == 4023 || MapID == 4024 || MapID == 4025)
                            {
                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("DisCity has finished come Next Day it Start at 21:00 EveryDay!", System.Drawing.Color.White, Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                Teleport(1002, 430, 378);
                                //return;
                            }
                        }
                        if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 45 && DateTime.Now.Second == 00)
                        {
                            if (MapID == 4023 || MapID == 4024)
                            {
                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("All Players in DisCity Stage3 has been Teleported to FinalStage Goodluck!", System.Drawing.Color.White, Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                Teleport(4025, 150, 286);
                                Owner.Inventory.Add(723087, 0, 1);
                                //return;
                            }
                        }


                        if (DateTime.Now.Second == 00 && DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == Game.KimoEvents.WHour)
                        {
                            if (DateTime.Now.Second == 00 && DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == Game.KimoEvents.WHour && DateTime.Now.Minute == 00)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10529,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10529;
                                    Owner.Send((byte[])alert);
                                    //return;
                                }
                            }
                        }
                        if (DateTime.Now.Hour == Game.KimoEvents.ClassHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                        {
                            if (Class >= 41 && Class <= 45)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10519,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10519;
                                    Owner.Send((byte[])alert);
                                }
                                //return;
                            }
                            if (Class >= 61 && Class <= 65)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10519,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10519;
                                    Owner.Send((byte[])alert);
                                }
                            }
                            if (Class >= 11 && Class <= 15)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10519,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10519;
                                    Owner.Send((byte[])alert);
                                }
                            }
                            if (Class >= 21 && Class <= 25)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10519,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10519;
                                    Owner.Send((byte[])alert);
                                }
                            }
                            if (Class >= 142 && Class <= 145)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10519,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10519;
                                    Owner.Send((byte[])alert);
                                }
                            }
                            if (Class >= 51 && Class <= 55)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10519,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10519;
                                    Owner.Send((byte[])alert);
                                }
                            }
                            if (Class >= 132 && Class <= 135)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10519,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10519;
                                    Owner.Send((byte[])alert);
                                }
                            }
                            if (Class >= 70 && Class <= 75)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10519,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10519;
                                    Owner.Send((byte[])alert);
                                }
                            }

                        }

                        if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                        {
                            if (DateTime.Now.Minute == 00 && DateTime.Now.Hour == Game.KimoEvents.EGHour && DateTime.Now.Second == 15)
                            {
                                Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "Elite GuildWar has Started! You Wana Join?");
                                npc.OptionID = 239;
                                Owner.Send(npc.ToArray());
                                //return;
                            }
                        }
                        if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00 && DateTime.Now.Hour == Game.KimoEvents.SpouseHour)
                        {
                            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00 && DateTime.Now.Hour == Game.KimoEvents.SpouseHour)
                            {
                                Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "Couples Pk War has Started! You Wana Join?");
                                npc.OptionID = 241;
                                //Owner.HeadgearClaim = false;
                                Owner.Send(npc.ToArray());
                                //return;
                            }
                        }
                        if (DateTime.Now.DayOfYear != Owner.LastResetTime.DayOfYear)
                        {
                            if (Level >= 90)
                            {
                                EnlightenPoints = 100;
                                if (NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Knight ||
                                    NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Baron)
                                    EnlightenPoints += 100;
                                else if (NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Earl ||
                                    NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Duke)
                                    EnlightenPoints += 200;
                                else if (NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Prince)
                                    EnlightenPoints += 300;
                                else if (NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.King)
                                    EnlightenPoints += 400;
                                if (VIPLevel != 0)
                                {
                                    if (VIPLevel <= 3)
                                        EnlightenPoints += 100;
                                    else if (VIPLevel <= 5)
                                        EnlightenPoints += 200;
                                    else if (VIPLevel == 6)
                                        EnlightenPoints += 300;
                                }
                            }
                            ReceivedEnlightenPoints = 0;
                            Owner.DoubleExpToday = false;
                            Owner.ExpBalls = 0;
                            Owner.LotteryEntries = 0;
                            Quest = 0;
                            SubClassLevel = 0;
                            Owner.LastResetTime = DateTime.Now;
                            Owner.Send(new FlowerPacket(Flowers));
                        }
                        if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 01)
                        {
                            if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 01)
                            {
                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("DisCity has been started Go to Ape City to signup at SolarSaint!", System.Drawing.Color.White, Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                PhoenixProject.Game.Features.DisCity.dis = true;
                                Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "DisCity has Started! You Wana Join?");
                                npc.OptionID = 245;
                                Owner.Send(npc.ToArray());
                                //return;
                            }
                        }
                        if (DateTime.Now.Minute == 44 && DateTime.Now.Second == 00)
                        {
                            if (DateTime.Now.Minute == 44 && DateTime.Now.Second == 00)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10525,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10525;
                                    Owner.Send((byte[])alert);
                                }
                            }
                        }



                        if (DateTime.Now.Minute == 30 && DateTime.Now.Second == 00 && !Game.Tournaments.EliteTournament.Start)
                        {
                            if (DateTime.Now.Minute == 30 && DateTime.Now.Second == 00 && !Game.Tournaments.EliteTournament.Start)
                            {
                                Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "LastManStanding has Started! You Wana Join?");
                                npc.OptionID = 240;
                                Owner.Send(npc.ToArray());
                                //return;
                            }
                        }
                        if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00 && !Game.Tournaments.EliteTournament.Start)
                        {
                            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00 && !Game.Tournaments.EliteTournament.Start)
                            {
                                if (Owner.Map.BaseID != 6001 && Owner.Map.BaseID != 6000 && !Dead)
                                {

                                    EventAlert alert = new EventAlert
                                    {
                                        StrResID = 10531,
                                        Countdown = 30,
                                        UK12 = 1
                                    };
                                    StrResID = 10531;
                                    Owner.Send((byte[])alert);
                                }
                            }
                        }
                        #endregion


                    }
                }
                //AutoAttack();
            }
            catch { }
            {
            }
        }

        public void _timerCallBack(object obj, System.Timers.ElapsedEventArgs arg)
        {
            try
            {
                Time32 Now = Time32.Now;
                if (this == null || Owner == null)
                {
                    Owner.Disconnect();
                    return;
                }
                if (this == null || Owner == null)
                    return;
                if (Owner.Screen == null || Owner.Entity == null)
                {
                    Owner.Disconnect();
                }
                if (Owner != null)
                {


                    if (this.HandleTiming)
                    {

                      #region Training points
                        if (HeavenBlessing > 0 && !Dead)
                        {
                            if (Now > Owner.LastTrainingPointsUp.AddMinutes(10))
                            {
                                Owner.OnlineTrainingPoints += 10;
                                if (Owner.OnlineTrainingPoints >= 30)
                                {
                                    Owner.OnlineTrainingPoints -= 30;
                                    Owner.IncreaseExperience(Owner.ExpBall / 100, false);
                                }
                                Owner.LastTrainingPointsUp = Now;
                                Update(Network.GamePackets.Update.OnlineTraining, Owner.OnlineTrainingPoints, false);
                            }
                        }
                        #endregion
                        #region Minning
                        if (Owner.Mining && !Dead)
                        {
                            if (Now >= Owner.MiningStamp.AddSeconds(2))
                            {
                                Owner.MiningStamp = Now;
                                Game.ConquerStructures.Mining.Mine(Owner);
                            }
                        }
                        #endregion
                        #region MentorPrizeSave
                        if (Now > Owner.LastMentorSave.AddSeconds(5))
                        {
                            Database.KnownPersons.SaveApprenticeInfo(Owner.AsApprentice);
                            Owner.LastMentorSave = Now;
                        }
                        #endregion
                        #region Attackable
                        if (Owner.JustLoggedOn)
                        {
                            Owner.JustLoggedOn = false;
                            Owner.ReviveStamp = Now;
                        }
                        if (!Owner.Attackable)
                        {
                            if (Now > Owner.ReviveStamp.AddSeconds(5))
                            {
                                Owner.Attackable = true;
                            }
                        }
                        #endregion
                        #region DoubleExperience
                        if (Owner.Entity.DoubleExperienceTime > 0)
                        {
                            if (Now > Owner.Entity.DoubleExpStamp.AddMilliseconds(1000))
                            {
                                Owner.Entity.DoubleExpStamp = Now;
                                Owner.Entity.DoubleExperienceTime--;
                            }
                        }
                        #endregion
                        #region HeavenBlessing
                        if (Owner.Entity.HeavenBlessing > 0)
                        {
                            if (Now > Owner.Entity.HeavenBlessingStamp.AddMilliseconds(1000))
                            {
                                Owner.Entity.HeavenBlessingStamp = Now;
                                Owner.Entity.HeavenBlessing--;
                            }
                        }
                        #endregion
                        #region Enlightment
                        if (Owner.Entity.EnlightmentTime > 0)
                        {
                            if (Now >= Owner.Entity.EnlightmentStamp.AddMinutes(1))
                            {
                                Owner.Entity.EnlightmentStamp = Now;
                                Owner.Entity.EnlightmentTime--;
                                if (Owner.Entity.EnlightmentTime % 10 == 0 && Owner.Entity.EnlightmentTime > 0)
                                    Owner.IncreaseExperience(Game.Attacking.Calculate.Percent((int)Owner.ExpBall, .10F), false);
                            }
                        }
                        #endregion
                        #region PKPoints
                        if (Now >= Owner.Entity.PKPointDecreaseStamp.AddMinutes(5))
                        {
                            Owner.Entity.PKPointDecreaseStamp = Now;
                            if (Owner.Entity.PKPoints > 0)
                            {
                                Owner.Entity.PKPoints--;
                            }
                            else
                                Owner.Entity.PKPoints = 0;
                        }
                        #endregion
                        #region OverHP
                        if (Owner.Entity.FullyLoaded)
                        {
                            if (Owner.Entity.Hitpoints > Owner.Entity.MaxHitpoints && Owner.Entity.MaxHitpoints > 1 && !Owner.Entity.Transformed)
                            {
                                Owner.Entity.Hitpoints = Owner.Entity.MaxHitpoints;
                            }
                        }
                        #endregion
                        #region Stamina
                        if (Now > this.StaminaStamp.AddMilliseconds(500))
                        {
                            if (Owner.Entity.Vigor < Owner.Entity.MaxVigor)
                            {
                                if (Owner.Entity.Vigor + 3 < Owner.Entity.MaxVigor)
                                {
                                    Owner.Entity.Vigor += (ushort)(3 + (Owner.Entity.Action == Game.Enums.ConquerAction.Sit ? 2 : 0));

                                    {
                                        Network.GamePackets.Vigor vigor = new Network.GamePackets.Vigor(true);
                                        vigor.VigorValue = Owner.Entity.Vigor;
                                        vigor.Send(Owner);
                                    }
                                }
                                else
                                {
                                    Owner.Entity.Vigor = (ushort)Owner.Entity.MaxVigor;

                                    {
                                        Network.GamePackets.Vigor vigor = new Network.GamePackets.Vigor(true);
                                        vigor.VigorValue = Owner.Entity.Vigor;
                                        vigor.Send(Owner);
                                    }
                                }
                            }
                            if (!this.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                            {
                                int limit = 0;
                                if (this.HeavenBlessing > 0)
                                    limit = 50;
                                if (this.Stamina != 100 + limit)
                                {
                                    if (this.Action == Game.Enums.ConquerAction.Sit || !this.Owner.Equipment.Free(18))
                                    {
                                        if (this.Stamina <= 93 + limit)
                                        {
                                            this.Stamina += 7;
                                        }
                                        else
                                        {
                                            if (this.Stamina != 100 + limit)
                                                this.Stamina = (byte)(100 + limit);
                                        }
                                    }
                                    else
                                    {
                                        if (this.Stamina <= 97 + limit)
                                        {
                                            this.Stamina += 3;
                                        }
                                        else
                                        {
                                            if (this.Stamina != 100 + limit)
                                                this.Stamina = (byte)(100 + limit);
                                        }
                                    }
                                }
                                this.StaminaStamp = Now;
                            }
                        }
                        #endregion
                        #region SoulShackle
                        if (Owner.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.SoulShackle))
                        {
                            if (Now > Owner.Entity.ShackleStamp.AddSeconds(Owner.Entity.ShackleTime))
                            {
                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.SoulShackle);
                            }
                        }
                        #endregion
                        #region Freeze
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags2.IceBlock))
                        {
                            if (Now > Owner.Entity.FreezeStamp.AddSeconds(Owner.Entity.FreezeTime))
                            {
                                Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags2.IceBlock);
                            }
                        }
                        #endregion
                        #region AzureShield
                        if (Owner.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.AzureShield))
                        {
                            if (Now > Owner.Entity.MagicShieldStamp.AddSeconds(Owner.Entity.MagicShieldTime))
                            {
                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.AzureShield);
                            }
                        }
                        #endregion
                        #region Die Delay
                        if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Dead) && !Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Ghost))
                        {
                            if (Now > Owner.Entity.DeathStamp.AddSeconds(2))
                            {
                                Owner.Entity.AddFlag(Network.GamePackets.Update.Flags.Ghost);
                                if (Owner.Entity.Body % 10 < 3)
                                    Owner.Entity.TransformationID = 99;
                                else
                                    Owner.Entity.TransformationID = 98;

                                Owner.SendScreenSpawn(Owner.Entity, true);
                            }
                        }
                        #endregion
                        #region SkillTeam
                        if (Owner.Entity.MapID == 7009)
                        {
                            if (Owner.Entity.MapID == 7009)
                            {
                                Game.KimoSkillWar.SkillTeamRes(Owner);
                            }
                        }
                        #endregion
                        #region CaptureFlag
                        if (Owner.Entity.MapID == 2060)
                        {
                            if (Owner.Entity.MapID == 2060)
                            {
                                Game.Team.CaptureRes(Owner);
                            }
                        }
                        #endregion
                     
                    }
                }
                //AutoAttack();
            }
            catch { }
            {
            }
        }
       /* public void AutoAttack()
        {
            try
            {
                if (Owner == null)
                    return;
                foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
                {
                    if (client.Socket.Connected)
                    {
                        if (client.Entity.HandleTiming)
                        {
                            #region Auto attack
                            if (client.Entity.AttackPacket != null || client.Entity.VortexAttackStamp != null)
                            {
                                try
                                {
                                    if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                                    {
                                        if (client.Entity.VortexPacket != null && client.Entity.VortexPacket.ToArray() != null)
                                        {
                                            if (Time32.Now > client.Entity.VortexAttackStamp.AddMilliseconds(1400))
                                            {
                                                client.Entity.VortexAttackStamp = Time32.Now;
                                                new Game.Attacking.Handle(client.Entity.VortexPacket, client.Entity, null);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        client.Entity.VortexPacket = null;
                                        var AttackPacket = client.Entity.AttackPacket;
                                        if (AttackPacket != null && AttackPacket.ToArray() != null)
                                        {
                                            uint AttackType = AttackPacket.AttackType;
                                            if (AttackType == Network.GamePackets.Attack.Magic || AttackType == Network.GamePackets.Attack.Melee || AttackType == Network.GamePackets.Attack.Ranged)
                                            {
                                                if (AttackType == Network.GamePackets.Attack.Magic)
                                                {
                                                    if (Time32.Now > client.Entity.AttackStamp.AddSeconds(1))
                                                    {
                                                        new Game.Attacking.Handle(AttackPacket, client.Entity, null);
                                                    }
                                                }
                                                else
                                                {
                                                    int decrease = -300;
                                                    if (client.Entity.OnCyclone())
                                                        decrease = 700;
                                                    if (client.Entity.OnSuperman())
                                                        decrease = 200;
                                                    if (Time32.Now > client.Entity.AttackStamp.AddMilliseconds((1000 - client.Entity.Agility - decrease) * (int)(AttackType == Network.GamePackets.Attack.Ranged ? 1 : 1)))
                                                    {
                                                        new Game.Attacking.Handle(AttackPacket, client.Entity, null);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    //SaveException(e);
                                    client.Entity.AttackPacket = null;
                                    client.Entity.VortexPacket = null;
                                }
                            }
                            #endregion
                        }
                    }
                    //else
                    //    client.Disconnect();
                }
            }
            catch { }
            {
            }
        }*/
        #endregion
        public void Ressurect()
        {
            if (EntityFlag == EntityFlag.Player)
            {
                Owner.Send(new MapStatus() { BaseID = (ushort)Database.MapsTable.MapInformations[Owner.Map.ID].BaseID, ID = (uint)Owner.Map.ID, Status = Database.MapsTable.MapInformations[Owner.Map.ID].Status, Weather = Database.MapsTable.MapInformations[Owner.Map.ID].Weather });
                Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                weather.WeatherType = (uint)Program.WeatherType;
                weather.Intensity = 100;
                weather.Appearence = 2;
                weather.Direction = 4;
                Owner.Send(weather);
            }
        }
        public void Ressurect2()
        {
            Hitpoints = MaxHitpoints;
            TransformationID = 0;
            Stamina = 100;
            FlashingNameTime = 0;
            FlashingNameStamp = Time32.Now;
            RemoveFlag(Network.GamePackets.Update.Flags.FlashingName);
            RemoveFlag(Network.GamePackets.Update.Flags.Dead | Network.GamePackets.Update.Flags.Ghost);
            if (EntityFlag == EntityFlag.Player)
            {
                Owner.Send(new MapStatus() { BaseID = (ushort)Database.MapsTable.MapInformations[Owner.Map.ID].BaseID, ID = (uint)Owner.Map.ID, Status = Database.MapsTable.MapInformations[Owner.Map.ID].Status });
                Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                weather.WeatherType = (uint)Program.WeatherType;
                weather.Intensity = 100;
                weather.Appearence = 2;
                weather.Direction = 4;
                Owner.Send(weather);
            }
        }
        public void DropRandomStuff(Entity KillerName)
        {
            #region DropMoney
            if (Money > 100)
            {
                int amount = (int)(Money / 2);
                amount = ServerBase.Kernel.Random.Next(amount);
                if (ServerBase.Kernel.Rate(40))
                {
                    uint ItemID = Network.PacketHandler.MoneyItemID((uint)amount);
                    ushort x = X, y = Y;
                    Game.Map Map = ServerBase.Kernel.Maps[MapID];
                    if (Map.SelectCoordonates(ref x, ref y))
                    {
                        Money -= (uint)amount;
                        Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                        floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Money;
                        floorItem.Value = (uint)amount;
                        floorItem.ItemID = ItemID;
                        floorItem.MapID = MapID;
                        floorItem.MapObjType = Game.MapObjectType.Item;
                        floorItem.X = x;
                        floorItem.Y = y;
                        floorItem.Type = Network.GamePackets.FloorItem.Drop;
                        floorItem.OnFloor = Time32.Now;
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                        while (Map.Npcs.ContainsKey(floorItem.UID))
                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                        Map.AddFloorItem(floorItem);
                        Owner.SendScreenSpawn(floorItem, true);
                    }
                }
            }
            #endregion

            #region DropItems
            if (Owner != null)
            {
                Owner.Inventory.Update();
                if (Owner.Inventory.Count > 0)
                {
                    uint count = (uint)(Owner.Inventory.Count / 4);
                    //int startfrom = (byte)ServerBase.Kernel.Random.Next((int)Owner.Inventory.Objects.Length);
                    for (int c = 0; c < count; c++)
                    {
                        int startfrom = (byte)ServerBase.Kernel.Random.Next((int)Owner.Inventory.Objects.Length);
                        if (Owner.Inventory.Count < startfrom)
                        {
                            Owner.Inventory.Update();
                            //return;
                        }
                        if (Owner.Inventory.Objects[startfrom] != null)
                        {
                            if (PKPoints > 0)
                            {
                                if (Owner.Inventory.Objects[startfrom].Lock == 0)
                                {
                                    if (Owner.Inventory.Objects[startfrom].UnlockEnd < DateTime.Now)
                                    {
                                        if (!Owner.Inventory.Objects[startfrom].Bound && !Owner.Inventory.Objects[startfrom].Inscribed)
                                        {
                                            if (!Owner.Inventory.Objects[startfrom].Suspicious && Owner.Inventory.Objects[startfrom].Lock != 1)
                                            {
                                                if (!ServerBase.Constants.NoDrop.Contains((Owner.Inventory.Objects[startfrom].ID)))
                                                {
                                                    var Item = Owner.Inventory.Objects[startfrom];
                                                    Item.Lock = 0;
                                                    var infos = Database.ConquerItemInformation.BaseInformations[(uint)Item.ID];
                                                    ushort x = X, y = Y;
                                                    Game.Map Map = ServerBase.Kernel.Maps[MapID];
                                                    if (Map.SelectCoordonates(ref x, ref y))
                                                    {
                                                        Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                                        Owner.Inventory.Remove(Item, Enums.ItemUse.Remove);
                                                        floorItem.Item = Item;
                                                        floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                                                        floorItem.ItemID = (uint)Item.ID;
                                                        floorItem.MapID = MapID;
                                                        floorItem.MapObjType = Game.MapObjectType.Item;
                                                        floorItem.X = x;
                                                        floorItem.Y = y;
                                                        floorItem.Type = Network.GamePackets.FloorItem.Drop;
                                                        floorItem.OnFloor = Time32.Now;
                                                        floorItem.ItemColor = floorItem.Item.Color;
                                                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                                                        while (Map.Npcs.ContainsKey(floorItem.UID))
                                                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                                                        Map.AddFloorItem(floorItem);
                                                        Owner.SendScreenSpawn(floorItem, true);
                                                        Owner.Inventory.Update();
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region DropDetain items
            if (PKPoints > 29 && Killer != null && Killer.Owner != null)
            {
                int itemcount = 0;

                foreach (var Item in Owner.Equipment.Objects)
                {
                    if (Item != null && itemcount == 0)
                    {
                        if (Item.Position != 9 && Item.Position != 12)
                        {
                            if (!Item.ID.ToString().StartsWith("105"))
                            {
                                if (ServerBase.Kernel.Rate(35 + (int)(PKPoints > 99 ? 75 : 0)))
                                {
                                    ushort x = X, y = Y;
                                    Game.Map Map = ServerBase.Kernel.Maps[MapID];
                                    if (Map.SelectCoordonates(ref x, ref y))
                                    {
                                        Owner.Equipment.RemoveToGround(Item.Position);
                                        var infos = Database.ConquerItemInformation.BaseInformations[(uint)Item.ID];

                                        Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                        floorItem.Item = Item;
                                        floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                                        floorItem.ItemID = (uint)Item.ID;
                                        floorItem.MapID = MapID;
                                        floorItem.MapObjType = Game.MapObjectType.Item;
                                        floorItem.X = x;
                                        floorItem.Y = y;
                                        floorItem.Type = Network.GamePackets.FloorItem.DropDetain;
                                        floorItem.OnFloor = Time32.Now;
                                        floorItem.ItemColor = floorItem.Item.Color;
                                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                                        while (Map.Npcs.ContainsKey(floorItem.UID))
                                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                                        Owner.SendScreenSpawn(floorItem, true);

                                        Database.DetainedItemTable.DetainItem(Item, Owner, Killer.Owner);
                                        itemcount++;
                                        
                                        //return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            if (PKPoints > 99)
            {
                if (KillerName.EntityFlag == EntityFlag.Player)
                {
                    ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message(Name + " has been captured by " + KillerName.Name + " and sent in jail! The world is now safer!", System.Drawing.Color.Red, Message.Talk), ServerBase.Kernel.GamePool.Values);
                    Teleport(6000, 30, 76);
                }
                else
                {
                    ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message(Name + " has been captured and sent in jail by A Monster ["+KillerName.Name+"]! Funny!", System.Drawing.Color.Red, Message.Talk), ServerBase.Kernel.GamePool.Values);
                    Teleport(6000, 30, 76);
                }
            }
        }
        public static double GetAngle(ushort x, ushort y, ushort x2, ushort y2)
        {
            double xf1 = x, xf2 = x2, yf1 = y, yf2 = y2;
            double result = 90 - Math.Atan((xf1 - xf2) / (yf1 - yf2)) * 180 / Math.PI;
            if (xf1 - xf2 < 0 && yf1 - yf2 < 0)
                result += 180;
            else if (xf1 - xf2 == 0 && yf1 - yf2 < 0)
                result += 180;
            else if (yf1 - yf2 < 0 && xf1 - xf2 > 0)
                result -= 180;
            return result;
        }
        public class Vector { public ushort X, Y; }
        public static Vector GetBorderCoords(ushort old_x, ushort old_y, ushort Target_x, ushort Target_y)
        {
            double Θ = GetAngle(old_x, old_y, Target_x, Target_y);
            double w, h;
            Vector v = new Vector();
            byte quadrant = 1;
            if (Θ < 0)
                Θ += 360;
            else if (Θ == 360)
                Θ = 0;
            while (Θ >= 90)
            {
                Θ -= 90;
                quadrant++;
            }
            double screendistance = ScreenDistance;
            if (quadrant == 1)
            {
                screendistance = ScreenDistance / (Math.Cos(Θ * Math.PI / 180));
                if (screendistance > 25)
                    screendistance = ScreenDistance / (Math.Sin(Θ * Math.PI / 180));
                else if (Θ != 0)
                    v.Y++;
                h = screendistance * (Math.Sin(Θ * Math.PI / 180));
                w = screendistance * (Math.Cos(Θ * Math.PI / 180));
                v.X += (ushort)(Target_x + Math.Round(w));
                if (Θ == 90)
                    v.Y += (ushort)(Target_y - Math.Round(h));
                else
                    v.Y += (ushort)(Target_y + Math.Round(h));
            }
            else if (quadrant == 2)
            {
                screendistance = ScreenDistance / (Math.Cos(Θ * Math.PI / 180));
                if (screendistance > 25)
                {
                    screendistance = ScreenDistance / (Math.Sin(Θ * Math.PI / 180));
                    v.Y++;
                }
                w = screendistance * (Math.Sin(Θ * Math.PI / 180));
                h = screendistance * (Math.Cos(Θ * Math.PI / 180));
                v.X += (ushort)(Target_x - w);
                v.Y += (ushort)(Target_y + h);
            }
            else if (quadrant == 3)
            {
                screendistance = ScreenDistance / (Math.Cos(Θ * Math.PI / 180));
                if (screendistance > 25)
                    screendistance = ScreenDistance / (Math.Sin(Θ * Math.PI / 180));
                h = screendistance * (Math.Sin(Θ * Math.PI / 180));
                w = screendistance * (Math.Cos(Θ * Math.PI / 180));
                v.X += (ushort)(Target_x - w);
                v.Y += (ushort)(Target_y - h);
            }
            else if (quadrant == 4)
            {
                screendistance = ScreenDistance / (Math.Cos(Θ * Math.PI / 180));
                if (screendistance > 25)
                    screendistance = ScreenDistance / (Math.Sin(Θ * Math.PI / 180));
                else if (Θ > 0)
                    v.X++;
                w = screendistance * (Math.Sin(Θ * Math.PI / 180));
                h = screendistance * (Math.Cos(Θ * Math.PI / 180));
                v.X += (ushort)(Target_x + w);
                v.Y += (ushort)(Target_y - h);
            }
            return v;
        }
      
        public Entity Killer;
        public void Die(Entity killer)//just replace this whole void for die delay
        {

            if (Time32.Now > DeathStamp)
            {
                if (AuraTime != 0)
                {
                    AuraTime = 0;
                    RemoveFlag2(Network.GamePackets.Update.Flags2.TyrantAura);
                    RemoveFlag2(Network.GamePackets.Update.Flags2.FendAura);
                    RemoveFlag2(Network.GamePackets.Update.Flags2.MetalAura);
                    RemoveFlag2(Network.GamePackets.Update.Flags2.WoodAura);
                    RemoveFlag2(Network.GamePackets.Update.Flags2.WaterAura);
                    RemoveFlag2(Network.GamePackets.Update.Flags2.EarthAura);
                    RemoveFlag2(Network.GamePackets.Update.Flags2.FireAura);
                    if (!Owner.AlternateEquipment)
                    {
                        Owner.LoadItemStats(this);
                    }
                    else
                    {
                        Owner.LoadItemStats2(this);
                    }

                }
                if (killer.EntityFlag == EntityFlag.Player)
                {

                    BlackSpot spot = new BlackSpot
                    {
                        Remove = 1,
                        Identifier = UID
                    };
                    killer.Owner.Send((byte[])spot);
                    BlackSpots = false;
                    BlackSpotTime2 = 0;
                    BlackSpotCheck = 0;
                }
                
                if (killer.EntityFlag == Game.EntityFlag.Player)
                {
                    if (EntityFlag == Game.EntityFlag.Player)
                    {
                       /* Game.PkExpeliate pk = new Game.PkExpeliate();
                        pk.UID = killer.UID;
                        pk.Name = Name;
                        pk.KilledAt = "KimoConquer";
                        pk.LostExp = 0;
                        pk.Times = 0;
                        pk.Potency = (uint)BattlePower;
                        pk.Level = Level;
                        PkExpelTable.PkExploitAdd(killer.Owner, UID, pk);*/
                    }
                }
                if (killer.MapID == 3031)
                {
                    if (this.ConquerPoints >= 20000)
                    {

                        uint ItemID = 729911;
                        this.ConquerPoints -= 20000;
                        ushort X = this.X, Y = this.Y;
                        Game.Map Map = ServerBase.Kernel.Maps[this.MapID];
                        if (Map.SelectCoordonates(ref X, ref Y))
                        {
                            Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                            floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                            floorItem.Value = 20000;
                            floorItem.ItemID = ItemID;
                            floorItem.MapID = this.MapID;
                            floorItem.MapObjType = Game.MapObjectType.Item;
                            floorItem.X = X;
                            floorItem.Y = Y;
                            floorItem.Owner = killer.Owner;
                            floorItem.Type = Network.GamePackets.FloorItem.Drop;
                            floorItem.OnFloor = Time32.Now;
                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            while (Map.Npcs.ContainsKey(floorItem.UID))
                                floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            Map.AddFloorItem(floorItem);
                            this.Owner.SendScreenSpawn(floorItem, true);
                        }
                    }
                    else
                    {
                        this.Teleport(1002, 428, 378);
                        ServerBase.Kernel.SendWorldMessage(new Message("Ops! " + this.Name + " has losed all his/her cps in the LordsWar and teleported back to TwinCity.", System.Drawing.Color.White, Message.TopLeft), ServerBase.Kernel.GamePool.Values);
                    }
                }
                if (killer.MapID == 7777)
                {
                    if (killer.MapID == 7777)
                    {
                        killer.Owner.elitepoints += 1;
                        if (killer.Owner.elitepoints >= 20)
                        {
                            Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "Congratulations, You Have Now " + killer.Owner.elitepoints + " ElitePk Points you can claim your prize now!");
                            npc.OptionID = 255;
                            killer.Owner.Send(npc.ToArray());
                        }
                        else
                        {
                            Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "You Have Now " + killer.Owner.elitepoints + " ElitePk Points Congratz you still need " + (20 - killer.Owner.elitepoints) + " more!");
                            npc.OptionID = 255;
                            killer.Owner.Send(npc.ToArray());
                        }
                    }
                }
                if (EntityFlag == EntityFlag.Player)
                    Owner.XPCount = 0;
                Killer = killer;
                Hitpoints = 0;
                DeathStamp = Time32.Now;
                //DieString();
                ToxicFogLeft = 0;
                if (Companion)
                {
                    AddFlag(Network.GamePackets.Update.Flags.Ghost | Network.GamePackets.Update.Flags.Dead | Network.GamePackets.Update.Flags.FadeAway);
                    Network.GamePackets.Attack zattack = new Network.GamePackets.Attack(true);
                    zattack.Attacked = UID;
                    zattack.AttackType = Network.GamePackets.Attack.Kill;
                    zattack.X = X;
                    zattack.Y = Y;
                    MonsterInfo.SendScreen(zattack);
                    Owner.Map.RemoveEntity(this);
                    Owner.Companion = null;
                }
                if (EntityFlag == EntityFlag.Player)
                {
                    if (killer.EntityFlag == EntityFlag.Player)

                    {
                        #region Cheack Pk Map

                        if (Owner.Entity.MapID == 2555 || Owner.Entity.MapID == 5530 || Owner.Entity.MapID == 5531 || Owner.Entity.MapID == 5532 || Owner.Entity.MapID == 1452 || Owner.Entity.MapID == 5560 || Owner.Entity.MapID == 5570 || Owner.Entity.MapID == 5580)
                        {
                            Owner.Entity.Teleport(1002, 438, 382);
                            // Console.WriteLine("Done");
                        }

                        #endregion Cheack Pk Map
                        if (ServerBase.Constants.PKFreeMaps.Contains(killer.MapID))
                            goto Over;
                        if (ServerBase.Constants.Damage1Map.Contains(killer.MapID))
                            goto Over;
                        if (killer.Owner.Map.BaseID == 700)
                            goto Over;
                        if (!ContainsFlag(Network.GamePackets.Update.Flags.FlashingName) && !ContainsFlag(Network.GamePackets.Update.Flags.BlackName))
                        {
                            killer.AddFlag(Network.GamePackets.Update.Flags.FlashingName);
                            killer.FlashingNameStamp = Time32.Now;
                            killer.FlashingNameTime = 60;
                            if (killer.GuildID != 0)
                            {
                                if (killer.Owner.Guild.Enemy.ContainsKey(GuildID))
                                {
                                    killer.PKPoints += 3;
                                }
                                else
                                {
                                    if (!killer.Owner.Enemy.ContainsKey(UID))
                                        killer.PKPoints += 10;
                                    else
                                        killer.PKPoints += 5;
                                }
                            }
                            else
                            {
                                if (!killer.Owner.Enemy.ContainsKey(UID))
                                    killer.PKPoints += 10;
                                else
                                    killer.PKPoints += 5;
                            }
                            if (!this.Owner.Enemy.ContainsKey(killer.UID))
                            {
                                Network.PacketHandler.AddEnemy(this.Owner, killer.Owner);

                            }

                        }
                    }

                }

                if (killer.EntityFlag == EntityFlag.Monster)
                {
                    DropRandomStuff(Killer);
                }
                else
                {
                    DropRandomStuff(Killer);
                }

                RemoveFlag(Network.GamePackets.Update.Flags.FlashingName);
            Over:

                Network.GamePackets.Attack attack = new Attack(true);
                attack.Attacker = killer.UID;
                attack.Attacked = UID;
                attack.AttackType = Network.GamePackets.Attack.Kill;
                attack.X = X;
                attack.Y = Y;

                if (EntityFlag == EntityFlag.Player)
                {
                    AddFlag(Network.GamePackets.Update.Flags.Dead);
                    RemoveFlag(Network.GamePackets.Update.Flags.Fly);
                    RemoveFlag(Network.GamePackets.Update.Flags.Ride);
                    RemoveFlag(Network.GamePackets.Update.Flags.Cyclone);
                    RemoveFlag(Network.GamePackets.Update.Flags.Superman);
                    RemoveFlag(Network.GamePackets.Update.Flags.FatalStrike);
                    RemoveFlag(Network.GamePackets.Update.Flags.FlashingName);
                    RemoveFlag(Network.GamePackets.Update.Flags.ShurikenVortex);
                    RemoveFlag2(Network.GamePackets.Update.Flags2.Oblivion);

                    //  if (Body % 10 < 3)
                    //    TransformationID = 99;
                    //else
                    //  TransformationID = 98;

                    Owner.SendScreen(attack, true);
                    Owner.Send(new MapStatus() { BaseID = (ushort)Database.MapsTable.MapInformations[Owner.Map.ID].BaseID, ID = (uint)Owner.Map.ID, Status = Database.MapsTable.MapInformations[Owner.Map.ID].Status, Weather = Database.MapsTable.MapInformations[Owner.Map.ID].Weather });
                    Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                    weather.WeatherType = (uint)Program.WeatherType;
                    weather.Intensity = 100;
                    weather.Appearence = 2;
                    weather.Direction = 4;
                    Owner.Send(weather);
                    if (Owner.QualifierGroup != null)
                    {
                        Owner.QualifierGroup.End(Owner);
                    }
                }
                else
                {

                    if (!Companion)
                    {
                        try
                        {
                            if (MonsterInfo != null)
                                if (killer != null)
                                    MonsterInfo.Drop(killer);
                        }
                        catch { }
                    }
                    ServerBase.Kernel.Maps[MapID].Floor[X, Y, MapObjType, this] = true;
                    if (killer.EntityFlag == EntityFlag.Player)
                    {
                        killer.Owner.IncreaseExperience(MaxHitpoints, true);
                        if (killer.Owner.Team != null)
                        {
                            foreach (Client.GameState teammate in killer.Owner.Team.Teammates)
                            {
                                if (ServerBase.Kernel.GetDistance(killer.X, killer.Y, teammate.Entity.X, teammate.Entity.Y) <= ServerBase.Constants.pScreenDistance)
                                {
                                    if (killer.UID != teammate.Entity.UID)
                                    {
                                        uint extraExperience = MaxHitpoints / 2;
                                        if (killer.Spouse == teammate.Entity.Name)
                                            extraExperience = MaxHitpoints * 2;
                                        byte TLevelN = teammate.Entity.Level;
                                        if (killer.Owner.Team.CanGetNoobExperience(teammate))
                                        {
                                            if (teammate.Entity.Level < 137)
                                            {
                                                extraExperience *= 2;
                                                teammate.IncreaseExperience(extraExperience, false);
                                                teammate.Send(ServerBase.Constants.NoobTeamExperience(extraExperience));
                                            }
                                        }
                                        else
                                        {
                                            if (teammate.Entity.Level < 137)
                                            {
                                                teammate.IncreaseExperience(extraExperience, false);
                                                teammate.Send(ServerBase.Constants.TeamExperience(extraExperience));
                                            }
                                        }
                                        byte TLevelNn = teammate.Entity.Level;
                                        byte newLevel = (byte)(TLevelNn - TLevelN);
                                        if (newLevel != 0)
                                        {
                                            if (TLevelN < 70)
                                            {
                                                for (int i = TLevelN; i < TLevelNn; i++)
                                                {
                                                    teammate.Team.Teammates[0].VirtuePoints += (uint)(i * 3.83F);
                                                    teammate.Team.SendMessage(new Message("The leader, " + teammate.Team.Teammates[0].Entity.Name + ", has gained " + (uint)(i * 7.7F) + " virtue points for power leveling the rookies.", System.Drawing.Color.Red, Message.Team));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (killer.Level < 137)
                        {
                            uint extraExp = MaxHitpoints;
                            extraExp *= ServerBase.Constants.ExtraExperienceRate;
                            extraExp += extraExp * killer.Gems[3] / 100;
                            extraExp += (uint)(extraExp * ((float)killer.BattlePower / 100));
                            if (killer.DoubleExperienceTime > 0)
                                extraExp *= 2;
                            if (killer.HeavenBlessing > 0)
                                extraExp += (uint)(extraExp * 20 / 100);
                            if (killer.Reborn >= 2)
                                extraExp /= 3;
                            killer.Owner.Send(ServerBase.Constants.ExtraExperience(extraExp));
                        }
                        killer.Owner.XPCount++;
                        if (killer.OnKOSpell())
                            killer.KOSpellTime++;
                    }

                }
            }
        }

        public void Update(byte type, byte value, bool screen)
        {
            if (!SendUpdates)
                return;
            if (this.Owner == null)
                return;
            update = new Update(true);
            update.UID = UID;
            update.Append(type, value, (byte)UpdateOffset1, (byte)UpdateOffset2, (byte)UpdateOffset3, (byte)UpdateOffset4, (byte)UpdateOffset5, (byte)UpdateOffset6, (byte)UpdateOffset7);
            if (!screen)
                update.Send(Owner);
            else
                Owner.SendScreen(update, true);
        }
        public void Update(byte type, ushort value, bool screen)
        {
            if (!SendUpdates)
                return;
            update = new Update(true);
            update.UID = UID;
            update.Append(type, value);
            if (!screen)
                update.Send(Owner as Client.GameState);
            else
                (Owner as Client.GameState).SendScreen(update, true);
        }
        public void Update(byte type, uint value, bool screen)
        {
            if (!SendUpdates)
                return;
            update = new Update(true);
            update.UID = UID;
            update.Append(type, value);
            if (!screen)
                update.Send(Owner as Client.GameState);
            else
                (Owner as Client.GameState).SendScreen(update, true);
        }
        public void Update(byte type, ulong value, bool screen)
        {
            if (!SendUpdates)
                return;
            update = new Update(true);
            update.UID = UID;
            update.Append(type, value);
            if (EntityFlag == EntityFlag.Player)
            {
                if (!screen)
                    update.Send(Owner as Client.GameState);
                else
                    (Owner as Client.GameState).SendScreen(update, true);
            }
            else
            {
                MonsterInfo.SendScreen(update);
            }
        }
        public void UpdateEffects(bool screen)
        {
            if (!SendUpdates)
                return;
            update = new Update(true);
            update.UID = UID;
            update.AppendFull2(25, StatusFlag, StatusFlag2, StatusFlag3);
            if (EntityFlag == EntityFlag.Player)
            {
                if (EntityFlag == EntityFlag.Player)
                {
                    if (!screen)
                        update.Send(Owner as Client.GameState);
                    else
                        (Owner as Client.GameState).SendScreen(update, true);
                }
                else
                {
                    MonsterInfo.SendScreen(update);
                }
            }
            else
            {
                MonsterInfo.SendScreen(update);
            }
        }
        public void Update2(byte type, ulong value, bool screen)
        {
            if (!SendUpdates)
                return;
            update = new Update(true);
            update.UID = UID;
            update.Append2(type, value);
            if (EntityFlag == EntityFlag.Player)
            {
                if (!screen)
                    update.Send(Owner as Client.GameState);
                else
                    (Owner as Client.GameState).SendScreen(update, true);
            }
            else
            {
                MonsterInfo.SendScreen(update);
            }
        }
        public void Update(byte type, string value, bool screen)
        {
            if (!SendUpdates)
                return;
            Network.GamePackets._String update = new _String(true);
            update.UID = this.UID;
            update.Type = type;
            update.TextsCount = 1;
            update.Texts.Add(value);
            if (EntityFlag == EntityFlag.Player)
            {
                if (!screen)
                    update.Send(Owner as Client.GameState);
                else
                    (Owner as Client.GameState).SendScreen(update, true);
            }
            else
            {
                MonsterInfo.SendScreen(update);
            }
        }
        private void UpdateDatabase(string column, byte value)
        {
            new Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE).Update("entities").Set(column, value).Where("UID", UID).Execute();
        }
        private void UpdateDatabase(string column, long value)
        {
            new Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE).Update("entities").Set(column, value).Where("UID", UID).Execute();
        }
        private void UpdateDatabase(string column, ulong value)
        {
            new Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE).Update("entities").Set(column, value).Where("UID", UID).Execute();
        }
        private void UpdateDatabase(string column, bool value)
        {
            new Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE).Update("entities").Set(column, value).Where("UID", UID).Execute();
        }
        private void UpdateDatabase(string column, string value)
        {
            new Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE).Update("entities").Set(column, value).Where("UID", UID).Execute();
        }
        public bool Move(Enums.ConquerAngle Direction)
        {
            ushort _X = X, _Y = Y;
            Facing = Direction;
            sbyte xi = 0, yi = 0;
            switch (Direction)
            {
                case Enums.ConquerAngle.North: xi = -1; yi = -1; break;
                case Enums.ConquerAngle.South: xi = 1; yi = 1; break;
                case Enums.ConquerAngle.East: xi = 1; yi = -1; break;
                case Enums.ConquerAngle.West: xi = -1; yi = 1; break;
                case Enums.ConquerAngle.NorthWest: xi = -1; break;
                case Enums.ConquerAngle.SouthWest: yi = 1; break;
                case Enums.ConquerAngle.NorthEast: yi = -1; break;
                case Enums.ConquerAngle.SouthEast: xi = 1; break;
            }
            _X = (ushort)(X + xi);
            _Y = (ushort)(Y + yi);
            Game.Map Map = null;
            if (ServerBase.Kernel.Maps.TryGetValue(MapID, out Map))
            {
                if (Map.Floor[_X, _Y, MapObjType, this])
                {
                    if (MapObjType == MapObjectType.Monster)
                    {
                        Map.Floor[_X, _Y, MapObjType, this] = false;
                        Map.Floor[X, Y, MapObjType, this] = true;
                    }

                    X = _X;
                    Y = _Y;
                    return true;
                }
                else
                {
                    if (Mode == Enums.Mode.None)
                    {
                        if (EntityFlag != EntityFlag.Monster)
                            Teleport(MapID,X, Y);
                        else
                            return false;
                    }
                }
            }
            else
            {
                if (EntityFlag != EntityFlag.Monster)
                    Teleport(MapID,X, Y);
                else
                    return false;
            }
            return true;
        }
        public bool Move2(Enums.ConquerAngle Direction)
        {
            ushort _X = X, _Y = Y;
            Facing = Direction;
            sbyte xi = 0, yi = 0;
            switch (Direction)
            {
                case Enums.ConquerAngle.North: xi = -1; yi = -1; break;
                case Enums.ConquerAngle.South: xi = 1; yi = 1; break;
                case Enums.ConquerAngle.East: xi = 1; yi = -1; break;
                case Enums.ConquerAngle.West: xi = -1; yi = 1; break;
                case Enums.ConquerAngle.NorthWest: xi = -1; break;
                case Enums.ConquerAngle.SouthWest: yi = 1; break;
                case Enums.ConquerAngle.NorthEast: yi = -1; break;
                case Enums.ConquerAngle.SouthEast: xi = 1; break;
            }
            _X = (ushort)(X + xi);
            _Y = (ushort)(Y + yi);
            Game.Map Map = null;
            if (ServerBase.Kernel.Maps.TryGetValue(MapID, out Map))
            {
                if (Map.Floor[_X, _Y, MapObjType, this])
                {
                    Map.Floor[_X, _Y, MapObjType, this] = false;
                    Map.Floor[X, Y, MapObjType, this] = true;
                    X = _X;
                    Y = _Y;

                    return true;
                }
                else
                {
                    if (Mode == Enums.Mode.None)
                    {
                        if (EntityFlag != EntityFlag.Monster)
                            Teleport(MapID, X, Y);
                        else
                            return false;
                    }
                }
            }
            else
            {
                if (EntityFlag != EntityFlag.Monster)
                    Teleport(MapID, X, Y);
                else
                    return false;
            }
            return true;
        }
        public void SendSpawn(Client.GameState client)
        {
            SendSpawn(client, true);
        }
        public void SendSpawn(Client.GameState client, bool checkScreen = true)
        {
            if (client.Screen.Add(this) || !checkScreen)
            {
                client.Send(SpawnPacket);
                byte[] array = new byte[this.SpawnPacket.Length];
                this.SpawnPacket.CopyTo(array, 0);
                array[0x6d] = 0;
                client.Send(array);
                if (EntityFlag == EntityFlag.Player)
                {
                    if (Owner.Booth != null)
                    {
                        client.Send(Owner.Booth);
                        if (Owner.Booth != null)
                        {
                            if (Owner.Booth.HawkMessage != null)
                                client.Send(Owner.Booth.HawkMessage);
                        }
                    }
                }
                
               
            }
        }

        public void AddFlag(ulong flag)
        {
            //if (!ContainsFlag(Network.GamePackets.Update.Flags.Dead) && !ContainsFlag(Network.GamePackets.Update.Flags.Ghost))
            StatusFlag |= flag;
        }

        public bool ContainsFlag(ulong flag)
        {
            ulong aux = StatusFlag;
            aux &= ~flag;
            return !(aux == StatusFlag);
        }
        public void RemoveFlag(ulong flag)
        {
            if (ContainsFlag(flag))
            {
                StatusFlag &= ~flag;
            }
        }
        public void AddFlag2(ulong flag)
        {
            if (flag == Network.GamePackets.Update.Flags2.SoulShackle) { StatusFlag2 |= flag ; return; }
            if (!ContainsFlag(Network.GamePackets.Update.Flags.Dead) && !ContainsFlag(Network.GamePackets.Update.Flags.Ghost))
                StatusFlag2 |= flag;
        }
        public bool ContainsFlag2(ulong flag)
        {
            ulong aux = StatusFlag2;
            aux &= ~flag;
            return !(aux == StatusFlag2);
        }
        public void RemoveFlag2(ulong flag)
        {
            if (ContainsFlag2(flag))
            {
                StatusFlag2 &= ~flag;
            }
        }

        public void AddFlag3(ulong flag)
        {
            if (flag == Network.GamePackets.Update.Flags2.SoulShackle) { StatusFlag3 |= flag; return; }
            if (!ContainsFlag(Network.GamePackets.Update.Flags.Dead) && !ContainsFlag(Network.GamePackets.Update.Flags.Ghost))
                StatusFlag3 |= flag;
        }
        public bool ContainsFlag3(ulong flag)
        {
            ulong aux = StatusFlag3;
            aux &= ~flag;
            return !(aux == StatusFlag3);
        }
        public void RemoveFlag3(ulong flag)
        {
            if (ContainsFlag3(flag))
            {
                StatusFlag3 &= ~flag;
               // Console.WriteLine("ss");
            }
        }
        public void Shift(ushort X, ushort Y)
        {
            if (EntityFlag == EntityFlag.Player)
            {
                if (!Database.MapsTable.MapInformations.ContainsKey(MapID))
                    return;
                this.X = X;
                this.Y = Y;
                Network.GamePackets.Data Data = new Network.GamePackets.Data(true);
                Data.UID = UID;
                Data.ID = Network.GamePackets.Data.FlashStep;
                Data.dwParam = (uint)MapID;
                Data.wParam1 = X;
                Data.wParam2 = Y;
                Owner.SendScreen(Data, true);
                Owner.Screen.Reload(null);
            }
        }
        public void Teleportxx(ushort X, ushort Y)
        {
            if (EntityFlag == EntityFlag.Player)
            {
              
                if (!Database.MapsTable.MapInformations.ContainsKey(MapID))
                    return;
                if (Owner.Entity.ContainsFlag3(Network.GamePackets.Update.Flags3.MagicDefender))
                {

                    Owner.Entity.MagicDefenderTime = 0;
                    Owner.Entity.MagicDefenderIncrease = 0;
                    Owner.Entity.RemoveFlag3(Network.GamePackets.Update.Flags3.MagicDefender);
                    UpdateEffects(true);
                    SyncPacket packet = new SyncPacket
                    {
                        Identifier = Owner.Entity.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)Owner.Entity.StatusFlag,
                        StatusFlag2 = (ulong)Owner.Entity.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x80,
                        Time = 0,
                        Value = 0,
                        Level = 0
                    };
                    Owner.Entity.Owner.Send((byte[])packet);
                    foreach (var Client in Owner.MagicDef)
                    {
                        if (Client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.kimo4))
                        {
                            Client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.kimo4);
                        }
                    }
                    Owner.MagicDef.Clear();
                }
                if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.CastPray))
                {
                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.CastPray);

                    foreach (var Client in Owner.Prayers)
                    {
                        if (Client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Praying))
                        {
                            Client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                        }
                    }
                    Owner.Prayers.Clear();
                }
                this.X = X;
                this.Y = Y;
                Network.GamePackets.Data Data = new Network.GamePackets.Data(true);
                Data.UID = UID;
                Data.ID = Network.GamePackets.Data.Jump;
                Data.dwParam = (uint)Database.MapsTable.MapInformations[MapID].BaseID;
                Data.wParam1 = X;
                Data.wParam2 = Y;
                Owner.Send(Data);
            }
        }
        public void Teleport(ushort X, ushort Y)
        {
            if (EntityFlag == EntityFlag.Player)
            {
               
                if (!Database.MapsTable.MapInformations.ContainsKey(MapID))
                    return;
                if (Owner.Entity.ContainsFlag3(Network.GamePackets.Update.Flags3.MagicDefender))
                {

                    Owner.Entity.MagicDefenderTime = 0;
                    Owner.Entity.MagicDefenderIncrease = 0;
                    Owner.Entity.RemoveFlag3(Network.GamePackets.Update.Flags3.MagicDefender);
                    UpdateEffects(true);
                    SyncPacket packet = new SyncPacket
                    {
                        Identifier = Owner.Entity.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)Owner.Entity.StatusFlag,
                        StatusFlag2 = (ulong)Owner.Entity.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x80,
                        Time = 0,
                        Value = 0,
                        Level = 0
                    };
                    Owner.Entity.Owner.Send((byte[])packet);
                    foreach (var Client in Owner.MagicDef)
                    {
                        if (Client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.kimo4))
                        {
                            Client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.kimo4);
                        }
                    }
                    Owner.MagicDef.Clear();
                }
                if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.CastPray))
                {
                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.CastPray);

                    foreach (var Client in Owner.Prayers)
                    {
                        if (Client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Praying))
                        {
                            Client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                        }
                    }
                    Owner.Prayers.Clear();
                }
                this.X = X;
                this.Y = Y;
                Network.GamePackets.Data Data = new Network.GamePackets.Data(true);
                Data.UID = UID;
                Data.ID = Network.GamePackets.Data.Teleport;
                Data.dwParam = (uint)Database.MapsTable.MapInformations[MapID].BaseID;
                Data.wParam1 = X;
                Data.wParam2 = Y;
                Owner.Send(Data);
                Owner.Entity.MapRegion = Region.Region.FindRegion((uint)Owner.Map.BaseID, Owner.Entity.X, Owner.Entity.Y);
            }
        }
        public void SetLocation(ulong MapID, ushort X, ushort Y)
        {
            if (EntityFlag == EntityFlag.Player)
            {
                this.X = X;
                this.Y = Y;
                this.MapID = MapID;
                Owner.Entity.MapRegion = Region.Region.FindRegion((uint)Owner.Map.BaseID, Owner.Entity.X, Owner.Entity.Y);
            }
        }
        public void TeleportHouse(ulong MapID, ushort X, ushort Y)
        {
            if (EntityFlag == EntityFlag.Player)
            {
               
                if (!ServerBase.Kernel.Maps.ContainsKey(MapID))
                    return;
                if (Owner.Entity.ContainsFlag3(Network.GamePackets.Update.Flags3.MagicDefender))
                {

                    Owner.Entity.MagicDefenderTime = 0;
                    Owner.Entity.MagicDefenderIncrease = 0;
                    Owner.Entity.RemoveFlag3(Network.GamePackets.Update.Flags3.MagicDefender);
                    UpdateEffects(true);
                    SyncPacket packet = new SyncPacket
                    {
                        Identifier = Owner.Entity.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)Owner.Entity.StatusFlag,
                        StatusFlag2 = (ulong)Owner.Entity.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x80,
                        Time = 0,
                        Value = 0,
                        Level = 0
                    };
                    Owner.Entity.Owner.Send((byte[])packet);
                    foreach (var Client in Owner.MagicDef)
                    {
                        if (Client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.kimo4))
                        {
                            Client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.kimo4);
                        }
                    }
                    Owner.MagicDef.Clear();
                }
                if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.CastPray))
                {
                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.CastPray);

                    foreach (var Client in Owner.Prayers)
                    {
                        if (Client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Praying))
                        {
                            Client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                        }
                    }
                    Owner.Prayers.Clear();
                }
                if (EntityFlag == EntityFlag.Player)
                {
                    if (Owner.Companion != null)
                    {
                        Owner.Map.RemoveEntity(Owner.Companion);
                        Data data = new Data(true);
                        data.UID = Owner.Companion.UID;
                        data.ID = Network.GamePackets.Data.RemoveEntity;
                        Owner.Companion.MonsterInfo.SendScreen(data);
                        Owner.Companion = null;
                    }
                }
                if (MapID == this.MapID)
                {
                    Teleport(X, Y);
                    return;
                }
                this.X = X;
                this.Y = Y;
                PX = 0;
                PY = 0;
                this.PreviousMapID = this.MapID;
                this.MapID = MapID;
                Network.GamePackets.Data Data = new Network.GamePackets.Data(true);
                Data.UID = UID;
                Data.ID = Network.GamePackets.Data.Teleport;
                Data.dwParam = (uint)ServerBase.Kernel.Maps[UID].BaseID;
                Data.wParam1 = X;
                Data.wParam2 = Y;
                Owner.Send(Data);
                Owner.Send(new MapStatus() { BaseID = (ushort)Database.MapsTable.MapInformations[Owner.Map.ID].BaseID, ID = (uint)Owner.Map.ID, Status = Database.DMapsTablesss.HouseInfo[Owner.Map.ID].Status, Weather = Database.MapsTable.MapInformations[Owner.Map.ID].Weather });
                Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                weather.WeatherType = (uint)Program.WeatherType;
                weather.Intensity = 100;
                weather.Appearence = 2;
                weather.Direction = 4;
                Owner.Send(weather);
                Owner.Entity.MapRegion = Region.Region.FindRegion((uint)Owner.Map.BaseID, Owner.Entity.X, Owner.Entity.Y);
            }
        }
        public void Teleport(ulong MapID, ushort X, ushort Y)
        {
            if (EntityFlag == EntityFlag.Player)
            {
               
                if (Owner.WatchingGroup != null)
                {
                    PhoenixProject.Game.ConquerStructures.Arena.QualifyEngine.DoLeave(Owner);
                }
                if (Owner.QualifierGroup != null)
                {
                    PhoenixProject.Game.ConquerStructures.Arena.QualifyEngine.DoQuit(Owner);
                    //Owner.ArenaStatistic.Status = Network.GamePackets.ArenaStatistic.NotSignedUp;
                }


               


                if (!Database.MapsTable.MapInformations.ContainsKey(MapID))
                {
                    if (MapID != UID)
                    {

                        return;
                    }
                }
                if (Owner.Entity.ContainsFlag3(Network.GamePackets.Update.Flags3.MagicDefender))
                {

                    Owner.Entity.MagicDefenderTime = 0;
                    Owner.Entity.MagicDefenderIncrease = 0;
                    Owner.Entity.RemoveFlag3(Network.GamePackets.Update.Flags3.MagicDefender);
                    UpdateEffects(true);
                    SyncPacket packet = new SyncPacket
                    {
                        Identifier = Owner.Entity.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)Owner.Entity.StatusFlag,
                        StatusFlag2 = (ulong)Owner.Entity.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x80,
                        Time = 0,
                        Value = 0,
                        Level = 0
                    };
                    Owner.Entity.Owner.Send((byte[])packet);
                    foreach (var Client in Owner.MagicDef)
                    {
                        if (Client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.kimo4))
                        {
                            Client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.kimo4);
                        }
                    }
                    Owner.MagicDef.Clear();
                }
                if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.CastPray))
                {
                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.CastPray);

                    foreach (var Client in Owner.Prayers)
                    {
                        if (Client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Praying))
                        {
                            Client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                        }
                    }
                    Owner.Prayers.Clear();
                }
                
                if (EntityFlag == EntityFlag.Player)
                {
                    if (Owner.Companion != null)
                    {
                        Owner.Map.RemoveEntity(Owner.Companion);
                        Data data = new Data(true);
                        data.UID = Owner.Companion.UID;
                        data.ID = Network.GamePackets.Data.RemoveEntity;
                        Owner.Companion.MonsterInfo.SendScreen(data);
                        Owner.Companion = null;
                    }
                }
                if (MapID == this.MapID)
                {
                    Teleport(X, Y);
                    return;
                }
                this.X = X;
                this.Y = Y;
                this.PreviousMapID = this.MapID;
                this.MapID = MapID;
                Network.GamePackets.Data Data = new Network.GamePackets.Data(true);
                Data.UID = UID;
                Data.ID = Network.GamePackets.Data.Teleport;
                Data.dwParam = (uint)Database.MapsTable.MapInformations[MapID].BaseID;
                Data.wParam1 = X;
                Data.wParam2 = Y;
                Owner.Send(Data);
                Owner.Send(new MapStatus() { BaseID = (ushort)Database.MapsTable.MapInformations[Owner.Map.ID].BaseID, ID = (uint)Owner.Map.ID, Status = Database.MapsTable.MapInformations[Owner.Map.ID].Status, Weather = Database.MapsTable.MapInformations[Owner.Map.ID].Weather });
                Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                weather.WeatherType = (uint)Program.WeatherType;
                weather.Intensity = 100;
                weather.Appearence = 2;
                weather.Direction = 4;
                Owner.Send(weather);
                Owner.Entity.Action = PhoenixProject.Game.Enums.ConquerAction.None;
                Owner.ReviveStamp = Time32.Now;
                Owner.Attackable = false;
                Owner.Entity.MapRegion = Region.Region.FindRegion((uint)Owner.Map.BaseID, Owner.Entity.X, Owner.Entity.Y);
            }
        }
        public ushort PrevX, PrevY;
        public void Teleport(ulong MapID, ulong DynamicID, ushort X, ushort Y)
        {
            if (EntityFlag == EntityFlag.Player)
            {
               
                if (!Database.DMaps.MapPaths.ContainsKey(MapID))
                {
                    Console.WriteLine(" ss ");
                    return;
                }
                if (Owner.Entity.ContainsFlag3(Network.GamePackets.Update.Flags3.MagicDefender))
                {

                    Owner.Entity.MagicDefenderTime = 0;
                    Owner.Entity.MagicDefenderIncrease = 0;
                    Owner.Entity.RemoveFlag3(Network.GamePackets.Update.Flags3.MagicDefender);
                    UpdateEffects(true);
                    SyncPacket packet = new SyncPacket
                    {
                        Identifier = Owner.Entity.UID,
                        Count = 2,
                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                        StatusFlag1 = (ulong)Owner.Entity.StatusFlag,
                        StatusFlag2 = (ulong)Owner.Entity.StatusFlag2,
                        Unknown1 = 0x31,
                        StatusFlagOffset = 0x80,
                        Time = 0,
                        Value = 0,
                        Level = 0
                    };
                    Owner.Entity.Owner.Send((byte[])packet);
                    foreach (var Client in Owner.MagicDef)
                    {
                        if (Client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.kimo4))
                        {
                            Client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.kimo4);
                        }
                    }
                    Owner.MagicDef.Clear();
                }
                if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.CastPray))
                {
                    Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.CastPray);

                    foreach (var Client in Owner.Prayers)
                    {
                        if (Client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Praying))
                        {
                            Client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                        }
                    }
                    Owner.Prayers.Clear();
                }
                this.PrevX = this.X;
                this.PrevY = this.Y;
                this.X = X;
                this.Y = Y;
                this.PreviousMapID = this.MapID;
                this.MapID = DynamicID;
                Network.GamePackets.Data Data = new Network.GamePackets.Data(true);
                Data.UID = UID;
                Data.ID = Network.GamePackets.Data.Teleport;
                Data.dwParam = (ushort)MapID;
                Data.wParam1 = X;
                Data.wParam2 = Y;
                Owner.Send(Data);
                Owner.Entity.Action = PhoenixProject.Game.Enums.ConquerAction.None;
                Owner.ReviveStamp = Time32.Now;
                Owner.Attackable = false;
                Owner.Send(new MapStatus() { BaseID = (ushort)Database.MapsTable.MapInformations[Owner.Map.ID].BaseID, ID = (uint)Owner.Map.ID, Status = Database.MapsTable.MapInformations[Owner.Map.ID].Status, Weather = Database.MapsTable.MapInformations[Owner.Map.ID].Weather });
                Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                weather.WeatherType = (uint)Program.WeatherType;
                weather.Intensity = 100;
                weather.Appearence = 2;
                weather.Direction = 4;
                Owner.Send(weather);
                Owner.Entity.MapRegion = Region.Region.FindRegion((uint)Owner.Map.BaseID, Owner.Entity.X, Owner.Entity.Y);
            }
        }

        public bool OnKOSpell()
        {
            return OnCyclone() || OnSuperman() || OnOblivion();
        }
        public bool OnOblivion()
        {
            return ContainsFlag2(Network.GamePackets.Update.Flags2.Oblivion);
        }
        public bool OnCyclone()
        {
            return ContainsFlag(Network.GamePackets.Update.Flags.Cyclone);
        }
        public bool OnCannonBrag()
        {
            return ContainsFlag2(Network.GamePackets.Update.Flags2.CannonBraga);
        }//OnChainBolt
        public bool OnBlackBread()
        {
            return ContainsFlag2(Network.GamePackets.Update.Flags2.BlackBread);
        }
        public bool OnChainBolt()
        {
            return ContainsFlag2(Network.GamePackets.Update.Flags2.ChainBoltActive);
        }
        public bool OnSuperman()
        {
            return ContainsFlag(Network.GamePackets.Update.Flags.Superman);
        }
        public bool OnFatalStrike()
        {
            return ContainsFlag(Network.GamePackets.Update.Flags.FatalStrike);
        }

        public void Untransform()
        {
            if (MapID == 1036 && TransformationTime == 3600)
                return;
            this.TransformationID = 0;

            double maxHP = TransformationMaxHP;
            double HP = Hitpoints;
            double point = HP / maxHP;

            Hitpoints = (uint)(MaxHitpoints * point);
            Update(Network.GamePackets.Update.MaxHitpoints, MaxHitpoints, false);
        }
        public byte[] WindowSpawn()
        {
            byte[] buffer = new byte[SpawnPacket.Length];
            SpawnPacket.CopyTo(buffer, 0);
            buffer[95] = 1;
            return buffer;
        }
        #endregion
        public void SetVisible()
        {
            SpawnPacket[99] = 0;
        }
    }
}