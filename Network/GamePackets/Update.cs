using System;
using System.Collections.Generic;

namespace PhoenixProject.Network.GamePackets
{
    public class Update : Writer, Interfaces.IPacket
    {
        public struct UpdateStruct
        {
            public uint Type;
            public ulong Value1;
            public ulong Value2;
        }
        public enum CountryID : uint
        {
            Default = 0,
            Australia = 1,
            Brazil = 2,
            Canada = 3,
            Conquer = 4,
            Egypt = 5,
            France = 6,
            Macedonia = 7,
            Germany = 8,
            SaudiArabia = 9,
            Malaysia = 10,
            Mexico = 11,
            Nederlands = 12,
            Philippines = 13,
            Poland = 14,
            Portugal = 15,
            Romania = 16,
            Singapore = 17,
            Sweden = 18,
            Turkey = 19,
            UK = 20,
            USA = 21,
            Vietnam = 22
        }
        public class Effects
        {
            public const ulong
                BlackBeard = 13,
                CannonBarrage = 119,
                EagleEye = 24,
                KrackenRevenge = 29,
                BombUsage = 61,
                GunReloading = 121;

        }
        public class Flags
        {

            public const ulong
                Normal = 0x0,
                FlashingName = 0x1,
                Poisoned = 0x2,
                Invisible = 0x4,
                XPList = 0x10,
                Dead = 0x20,
                TeamLeader = 0x40,
                StarOfAccuracy = 0x80,
                MagicShield = 0x100,
                Stigma = 0x200,
                Ghost = 0x400,
                FadeAway = 0x800,
                RedName = 0x4000,
                BlackName = 0x8000,
                ReflectMelee = 0x20000,
                Superman = 0x40000,
                Ball = 0x80000,
                Ball2 = 0x100000,
                Invisibility = 0x400000,
                Cyclone = 0x800000,
                Dodge = 0x4000000,
                Fly = 0x8000000,
                Intensify = 0x10000000,
                CastPray = 0x40000000,
                Praying = 0x80000000,
                HeavenBlessing = 0x200000000,
                TopGuildLeader = 0x400000000,
                TopDeputyLeader = 0x800000000,
                MonthlyPKChampion = 0x1000000000,
                WeeklyPKChampion = 0x2000000000,
           TopMonk = 0x4000000000000L,
            Top8Monk = 0x8000000000000,
            Top2Monk = 0x10000000000000,
                TopWarrior = 0x4000000000,
                TopTrojan = 0x8000000000,
                TopArcher = 0x10000000000,
                TopWaterTaoist = 0x20000000000,
                TopFireTaoist = 0x40000000000,
                TopNinja = 0x80000000000,
                ShurikenVortex = 0x400000000000,
                FatalStrike = 0x800000000000,
                Flashy = 0x1000000000000,
                Ride = 0x4000000000000,
               TopSpouse = 1UL << 51,
            OrangeSparkles = 1UL << 52,
            PurpleSparkles = 1UL << 53,
            Dazed = 1UL << 54,
            RestoreAura = 1UL << 55,
            MoveSpeedRecovered = 1UL << 56,
            GodlyShield = 1UL << 57,
            ShockDaze = 1UL << 58,
            Freeze = 1UL << 59,
             kimo11 = 1UL << 60,
              kimo12 = 1UL << 61,
               kimo13 = 1UL << 62,
                kimo14 = 1UL << 63,
                /*
                 * halo2 blackspot in middle of body
halo3 black spot in top of head not move player
halo4 stars on the player
halo5 move speed recoverd
halo6 LionShield
halo7 dawama above the head not move
halo8 None
halo9 Move againest*/
             kimo1 = 0x10000000000000,
             kimo2 = 0x20000000000000,
             kimo3 = 0x40000000000000,
             kimo4 = 0x80000000000000,
             kimo5 = 0x100000000000000,
             kimo6 = 0x200000000000000,
             kimo7 = 0x400000000000000,
             kimo8 = 0x800000000000000,
             kimo9 = 0x1000000000000000,
             kimo10 = 0x2000000000000000,
             kimo15 = 0x4000000000000000,
             kimo16 = 0x8000000000000000;

        }
        public class Flags2
        {

            public const ulong
               WeeklyTop8Pk = 0x01,
            WeeklyTop2PkGold = 0x2,
            WeeklyTop2PkBlue = 0x4,
            MonthlyTop8Pk = 0x8,
            MontlyTop2Pk = 0x10,
            MontlyTop3Pk = 0x20,
            Top8Fire = 0x40,
            Top2Fire = 0x80,
            Top3Fire = 0x100,
            Top8Water = 0x200,
            Top2Water = 0x400,
            Top3Water = 0x800,
            Top8Ninja = 0x1000,
            Top2Ninja = 0x2000,
            Top3Ninja = 0x4000,
            Top8Warrior = 0x8000,
            Top2Warrior = 0x10000,
            Top3Warrior = 0x20000,
            Top8Trojan = 0x40000,
            Top2Trojan = 0x80000,
            Top3Trojan = 0x100000,
            Top8Archer = 0x200000,
            Top2Archer = 0x400000,
            Top3Archer = 0x800000,
            Top3SpouseBlue = 0x1000000,
            Top2SpouseBlue = 0x2000000,
            Top3SpouseYellow = 0x4000000,
            Contestant = 0x8000000,
            ChainBoltActive = 0x10000000,
            AzureShield = 0x20000000,
            AzureShieldFade = 0x40000000,
            CaryingFlag = 2147483648,//blank next one?
            TyrantAura = 0x400000000,
            FendAura = 0x1000000000,
            MetalAura = 0x4000000000,
            WoodAura = 0x10000000000,
            WaterAura = 0x40000000000,
            FireAura = 17592186044416,
            EarthAura = 0x400000000000,
            SoulShackle = 140737488355328,
            Oblivion = 0x1000000000000,

            LionShield = 0x200000000000000,
            OrangeHaloGlow = 281474976710656,
            LowVigorUnableToJump = 1125899906842624,
            TopSpouse = 2251799813685248,
            SparkleHalo = 4503599627370496,
            PurpleSparkle = 9007199254740992,
            Dazed = 18014398509481984,//no movement
            BlueRestoreAura = 36028797018963968,
            MoveSpeedRecovered = 72057594037927936,
            SuperShieldHalo = 144115188075855872,
            HUGEDazed = 288230376151711744,//no movement
            IceBlock = 576460752303423488, //no movement
            Confused = 1152921504606846976,//reverses movement
            BlackSpot = 0x80000000000000,
            ScurvySmoke = 36028797018963968UL,
            CannonBraga = 0x100000000000000,
            BlackBread = 0x200000000000000,
            TopPirate = 0x400000000000000,
            Top8Pirate = 0x800000000000000,
            Top2Pirate = 0x1000000000000000,
            Top3Pirate = 0x2000000000000000,
            WarriorWalk = 0x4000000000000000L,
            kimo1 = 0x10000000000000,
             kimo2 = 0x20000000000000,
             kimo3 = 0x40000000000000,
             kimo4 = 0x80000000000000,
             kimo5 = 0x100000000000000,
             kimo6 = 0x200000000000000,
             kimo7 = 0x400000000000000,
             kimo8 = 0x800000000000000,
             kimo9 = 0x1000000000000000,
             kimo10 = 0x2000000000000000,
             kimo15 = 0x4000000000000000,
             kimo16 = 0x8000000000000000;
            
        }
        public class Flags3
        {

            public const ulong
            MagicDefender = 0x01,//MagicDefender for Warrior
            kimo2 = 0x2,
            kimo3 = 0x4,
            kimo4 = 0x8,
            kimo5 = 0x10,
            kimo6 = 0x20,
            kimo7 = 0x40,
            kimo8 = 0x80,
            kimo9 = 0x100,
            kimo10 = 0x200,
            kimo11 = 0x400,
            kimo12 = 0x800,
            kimo13 = 0x1000,
            kimo14 = 0x2000,
            kimo15 = 0x4000,
            kimo16 = 0x8000,
            kimo17 = 0x10000,
            kimo18 = 0x20000,
            kimo19 = 0x40000,
            kimo20 = 0x80000,
            kimo21 = 0x100000,
            kimo22 = 0x200000,
            kimo23 = 0x400000,
            kimo24 = 0x800000,
            kimo25 = 0x1000000,
            kimo26 = 0x2000000,
            kimo27 = 0x4000000,
            kimo28 = 0x8000000,
            kimo29 = 0x10000000,
            kimo30 = 0x20000000,
            kimo31 = 0x40000000,
            kimo32 = 2147483648,//blank next one?
            kimo33 = 0x400000000,
            kimo34 = 0x1000000000,
            kimo35 = 0x4000000000,
            kimo36 = 0x10000000000,
            kimo37 = 0x40000000000,
            kimo38 = 17592186044416,
            kimo39 = 0x400000000000,
            kimo40 = 140737488355328,
            kimo41 = 0x1000000000000,
            kimo42 = 1125899906842624,
            kimo43 = 0x8000000000000,
            kimo44 = 0x10000000000000,
            kimo45 = 0x200000000000000,
            kimo46 = 281474976710656,
            kimo47 = 1125899906842624,
            kimo48 = 2251799813685248,
            kimo49 = 4503599627370496,
            kimo50 = 9007199254740992,
            kimo51 = 18014398509481984,//no movement
            kimo52 = 36028797018963968,
            kimo53 = 72057594037927936,
            kimo54 = 144115188075855872,
            kimo55 = 288230376151711744,//no movement
            kimo56 = 576460752303423488, //no movement
            kimo57 = 1152921504606846976;//reverses movement
           

        }
        public const byte
                Hitpoints = 0,
                MaxHitpoints = 1,
                Mana = 2,
                MaxMana = 3,
                Money = 4,
                Experience = 5,
                PKPoints = 6,
                Class = 7,
                Stamina = 8,
                WHMoney = 9,
                Atributes = 10,
                Mesh = 11,
                Level = 12,
                Spirit = 13,
                Vitality = 14,
                Strength = 15,
                Agility = 16,
                HeavensBlessing = 17,
                DoubleExpTimer = 18,
                CursedTimer = 20,
                Reborn = 22,
                StatusFlag = 25,
                HairStyle = 26,
                XPCircle = 27,
                LuckyTimeTimer = 28,
                ConquerPoints = 29,
                OnlineTraining = 31,
                ExtraBattlePower = 36,
                Merchant = 38,
                VIPLevel = 39,
                QuizPoints = 40,
                EnlightPoints = 41,
                HonorPoints = 42,
                GuildShareBP = 44,
                BoundConquerPoints = 45,
                RacePoints = 47,
                
                MoreSP = 55,
                Flagss1 = 77,
                 Flagss2 = 78,
                  Flagss3 = 79,
                   Flagss4 = 80,
                    Flagss5 = 81,
                     Flagss6 = 82,
                      Flagss7 = 83,
                Flagss8 = 84;

        byte[] Buffer;
        const byte minBufferSize = 84;
        public Update(bool Create)
        {
            if (Create)
            {
                Buffer = new byte[minBufferSize + 8];
                WriteUInt16(minBufferSize, 0, Buffer);
                WriteUInt16(10017, 2, Buffer);
            }
        }

        public uint UID
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { WriteUInt32(value, 4, Buffer); }
        }
        public uint Duration
        {
            get
            {
                return BitConverter.ToUInt32(this.Buffer, 20);
            }
            set
            {
                WriteUInt32(value, 20, this.Buffer);
            }
        }
        public uint UpdateCount
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set
            {
                byte[] buffer = new byte[minBufferSize + 8 + 20 * value];
                Buffer.CopyTo(buffer, 0);
                WriteUInt16((ushort)(minBufferSize + 20 * value), 0, buffer);
                Buffer = buffer;
                WriteUInt32(value, 8, Buffer);
            }
        }
        public void SoulShackle(PhoenixProject.Game.Entity Entity, int Dmg, byte SpellLevel, byte Time_)
        {
            byte[] Buf = new byte[52 + 8];
            WriteUInt16(52, 0, Buf);//Packet Lenght
            WriteUInt16(10017, 2, Buf);//Status Packet Id
            WriteUInt32(Entity.UID, 4, Buf);//Client Id
            WriteUInt32(2, 8, Buf);// Switch
            WriteUInt32(25, 12, Buf); // Status Effect Type
            //WriteUInt64((ulong)Entity.EntityFlag, 16, Buf);//Status flag 1
            WriteUInt64((ulong)Entity.StatusFlag2, 24, Buf);//Status flag 2
            WriteUInt32(54, 32, Buf); //StatusTypes.AzureShield
            WriteUInt32(113, 36, Buf);//i guess Sybtype ,, 
            WriteUInt32(Time_, 40, Buf);//Time duration for the effect
            WriteUInt32((uint)Dmg, 44, Buf);//Damage Coused by the skill
            WriteUInt32(SpellLevel, 48, Buf); //the SoulShackle Skill Level
            if (PhoenixProject.ServerBase.Kernel.GamePool.ContainsKey(Entity.UID))
            {
                Client.GameState Cl = PhoenixProject.ServerBase.Kernel.GamePool[Entity.UID];
                Cl.Send(Buf);
            }
           // Entity.AddFlag2(Update.Flags2.SoulShackle);
        }
        public void AzureShield(PhoenixProject.Game.Entity Entity, int Dmg, byte SpellLevel, byte Time_)
        {
            byte[] Buf = new byte[52 + 8];
            WriteUInt16(52, 0, Buf);//Packet Lenght
            WriteUInt16(10017, 2, Buf);//Status Packet Id
            WriteUInt32(Entity.UID, 4, Buf);//Client Id
            WriteUInt32(2, 8, Buf);// Switch
            WriteUInt32(25, 12, Buf); // Status Effect Type
            //WriteUInt64((ulong)Entity.EntityFlag, 16, Buf);//Status flag 1
            WriteUInt64((ulong)Entity.StatusFlag2, 24, Buf);//Status flag 2
            WriteUInt32(49, 32, Buf); //StatusTypes.AzureShield
            WriteUInt32(93, 36, Buf);//i guess Sybtype ,, 
            WriteUInt32(Time_, 40, Buf);//Time duration for the effect
            WriteUInt32((uint)Dmg, 44, Buf);//Damage Coused by the skill
            WriteUInt32(SpellLevel, 48, Buf); //the Azure Skill Level
            if (PhoenixProject.ServerBase.Kernel.GamePool.ContainsKey(Entity.UID))
            {//3alashan ngeeb el client 
                Client.GameState Cl = PhoenixProject.ServerBase.Kernel.GamePool[Entity.UID];
                Cl.Send(Buf);
            }
            //Entity.AddFlag2(Update.Flags2.AzureShield);
        }
        public void Append(byte type, byte value)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(value, offset + 4, Buffer);
        }
        public void Append(byte type, ushort value)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(value, offset + 4, Buffer);
        }
        public void Add2(UInt32 type, UInt32 value, UInt32 value1, UInt32 value2 = 0)
        {
            UInt16 offset = 12;

            WriteUInt32(type, offset, Buffer);
            WriteUInt32(value, offset + 4, Buffer);
            WriteUInt32(value1, offset + 20, Buffer);
            WriteUInt32(value2, offset + 24, Buffer);

            //32 and 36
        }
        public void AppendFull(byte type, ulong val1, ulong val2)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(val1, offset + 4, Buffer);
            WriteUInt64(val2, offset + 12, Buffer);
          
        }
        public void AppendFull2(byte type, ulong val1, ulong val2,ulong val3)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(val1, offset + 4, Buffer);
            WriteUInt64(val2, offset + 12, Buffer);
            WriteUInt64(val3, offset + 20, Buffer);
        }
        public void Append(byte type, uint value)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(value, offset + 4, Buffer);
        }

        public void Append(byte type, ulong value)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(value, offset + 4, Buffer);
        }
        public void Append2(byte type, byte value)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(value, offset + 12, Buffer);
        }
        public void Append2(byte type, ushort value)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(value, offset + 12, Buffer);
        }
        public void Append2(byte type, uint value)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(value, offset + 12, Buffer);
        }

        public void Append2(byte type, ulong value)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(12 + ((UpdateCount - 1) * 20));
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(value, offset + 12, Buffer);
        }

        public void Clear()
        {
            byte[] buffer = new byte[minBufferSize + 8];
            WriteUInt16(minBufferSize, 0, Buffer);
            WriteUInt16(10017, 2, Buffer);
            WriteUInt32(UID, 4, buffer);
            Buffer = buffer;
        }

        public List<UpdateStruct> Updates
        {
            get
            {
                List<UpdateStruct> structs = new List<UpdateStruct>();
                ushort offset = 12;
                if (UpdateCount > 0)
                {
                    for (int c = 0; c < UpdateCount; c++)
                    {
                        UpdateStruct st = new UpdateStruct();
                        st.Type = BitConverter.ToUInt32(Buffer, offset); offset += 4;
                        st.Value1 = BitConverter.ToUInt64(Buffer, offset); offset += 8;
                        st.Value2 = BitConverter.ToUInt64(Buffer, offset); offset += 8;
                        structs.Add(st);
                    }
                }
                return structs;
            }
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }

        public void Send(Client.GameState client)
        {
            client.Send(Buffer);
        }
        public void Append(byte type, byte value, byte second, byte second2, byte second3, byte second4, byte second5, byte second6, byte second7)
        {
            UpdateCount = UpdateCount + 1;
            ushort offset = (ushort)(UpdateCount * 12);
            WriteUInt32(type, offset, Buffer);
            WriteUInt64(value, offset + 4, Buffer);
            WriteByte(second, offset + 12, Buffer);
            WriteByte(second2, offset + 13, Buffer);
            WriteByte(second3, offset + 14, Buffer);
            WriteByte(second4, offset + 15, Buffer);
            WriteByte(second5, offset + 16, Buffer);
            WriteByte(second6, offset + 17, Buffer);
            WriteByte(second7, offset + 18, Buffer);
        }
    }
}
