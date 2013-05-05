using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Database
{
    class rates
    {
        public static uint KoCount, lastitem,lastentity, GuildWar, PartyDrop, plus13, plus14, plus15, CaptureFlag, SkillTeam, DemonCave, Weather, Night, Garment, VotePrize, Steed, Mount, SoulP6, LevelUp, DamageGarment, DamageTails, TeratoDragon, ThrillingSpook, SnowBanshe, BotJail, TreasureLow, TreasureMax, TreasureMin, SteedRace, RemoveBound, elitepk, MonthlyPk, ChangeName, EliteGw, DailyPk, LastMan, TopSpouse, Riencration, king, prince, housepromete, houseupgrade, itembox, maxcps, minicps, CpsMethodNum, classpk, weeklypk;
        public static string VoteUrl = "";
        public static string serversite = "";
        public static string PopUpURL = "";
        public static string servername = "";
        public static string cryptkey = "";
        public static string coder = "";
        public static string cpsmethod = "";



        public static void LoadRates()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("rates").Where("Coder", "Kimo");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                lastentity = r.ReadUInt32("LastEntity");
                //Program.EntityUID = new ServerBase.Counter(r.ReadUInt32("LastEntity"));
                lastitem = r.ReadUInt32("LastItem");
                KoCount = r.ReadUInt32("KoCount");
                plus13 = r.ReadUInt32("Plus13");
                plus14 = r.ReadUInt32("Plus14");
                plus15 = r.ReadUInt32("Plus15");
                PartyDrop = r.ReadUInt32("PartyDrop");
                CaptureFlag = r.ReadUInt32("CaptureFlag");
                SkillTeam = r.ReadUInt32("SkillTeam");
                DemonCave = r.ReadUInt32("DemonCave");
                VoteUrl = r.ReadString("VoteUrl");
                VotePrize = r.ReadUInt32("VotePrize");
                Weather = r.ReadUInt32("Weather");
                Night = r.ReadUInt32("Night");
                SoulP6 = r.ReadUInt32("SoulP6");
                Garment = r.ReadUInt32("Garment");
                Steed = r.ReadUInt32("Steed");
                Mount = r.ReadUInt32("Mount");
                DamageTails = r.ReadUInt32("DamageTails");
                DamageGarment = r.ReadUInt32("DamageGarment");
                LevelUp = r.ReadUInt32("LevelUp");
                TeratoDragon = r.ReadUInt32("TeratoDragon");
                ThrillingSpook = r.ReadUInt32("ThrillingSpook");
                SnowBanshe = r.ReadUInt32("SnowBanshe");
                TreasureLow = r.ReadUInt32("TreasureLow");
                TreasureMax = r.ReadUInt32("TreasureMax");
                TreasureMin = r.ReadUInt32("TreasureMin");
                GuildWar = r.ReadUInt32("GuildWar");
                BotJail = r.ReadUInt32("BotJail");
                RemoveBound = r.ReadUInt32("RemoveBound");
                elitepk = r.ReadUInt32("elitepk");
                SteedRace = r.ReadUInt32("SteedRace");
                ChangeName = r.ReadUInt32("ChangeName");
                MonthlyPk = r.ReadUInt32("MonthlyPk");
                EliteGw = r.ReadUInt32("EliteGw");
                TopSpouse = r.ReadUInt32("TopSpouse");
                DailyPk = r.ReadUInt32("DailyPk");
                LastMan = r.ReadUInt32("LastMan");
                Riencration = r.ReadUInt32("Riencration");
                king = r.ReadUInt32("kings");
                prince = r.ReadUInt32("prince");
                housepromete = r.ReadUInt32("HousePromete");
                houseupgrade = r.ReadUInt32("HouseUpgrade");
                itembox = r.ReadUInt32("ItemBox");
                maxcps = r.ReadUInt32("MaxCps");
                minicps = r.ReadUInt32("MiniCps");
                CpsMethodNum = r.ReadUInt32("CpsMethodNum");
                classpk = r.ReadUInt32("ClassPk");
                weeklypk = r.ReadUInt32("WeeklyPk");

                cpsmethod = r.ReadString("CpsMethod");
                serversite = r.ReadString("ServerWebsite");
                servername = r.ReadString("ServerName");
                coder = r.ReadString("Coder");
                PopUpURL = r.ReadString("LoginSite");
                ServerBase.Constants.ServerName2 = r.ReadString("ServerName");
                ServerBase.Constants.GameCryptographyKey = r.ReadString("CryptKey");
            }
            Console.WriteLine("Rates Loaded.");
            r.Close();
            r.Dispose();
        }
    }
}
