using System;
using System.IO;
using System.Text;
using PhoenixProject.ServerBase;
using System.Collections.Generic;

namespace PhoenixProject.Database
{
    public static class EntityTable
    {
        public static void LoadPlayersVots()
        {

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("VoteIp");
            PhoenixProject.Database.MySqlReader d = new PhoenixProject.Database.MySqlReader(cmd);
            while (d.Read())
            {
                PhoenixProject.Game.ConquerStructures.PlayersVot Vot = new PhoenixProject.Game.ConquerStructures.PlayersVot();
                Vot.Uid = d.ReadUInt32("ID");
                Vot.AdressIp = d.ReadString("IP");
                if (!Kernel.VotePool.ContainsKey(Vot.AdressIp))
                    Kernel.VotePool.Add(Vot.AdressIp, Vot);
                if (!Kernel.VotePoolUid.ContainsKey(Vot.Uid))
                    Kernel.VotePoolUid.Add(Vot.Uid, Vot);
               
            }
            d.Close();
            d.Dispose();
        }
        public static void DeletVotes(PhoenixProject.Game.ConquerStructures.PlayersVot PlayerVot)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE).Delete("VoteIp", "ID", PlayerVot.Uid).And("IP", PlayerVot.AdressIp);
            cmd.Execute();
        }
        public static void DeletVotes()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE).Delete("VoteIp", "delete", "1");
            cmd.Execute();
        }
        public static void SavePlayersVot(PhoenixProject.Game.ConquerStructures.PlayersVot PlayerVot)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("VoteIp").Insert("ID", PlayerVot.Uid).Insert("IP", PlayerVot.AdressIp).Execute();

        }
        public static bool LoadEntity(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("entities").Where("UID", client.Account.EntityID);
            MySqlReader r = new MySqlReader(cmd);
            if (r.Read())
            {

                client.WarehousePW = r.ReadString("WarehousePW");
                client.Entity = new Game.Entity(Game.EntityFlag.Player, false);
                client.Entity.Name = r.ReadString("Name");
                client.HeadgearClaim = r.ReadBoolean("HeadgearClaim");
                client.Entity.Spouse = r.ReadString("Spouse");
                client.Entity.NobalityDonation = r.ReadUInt64("Donation");
                client.Entity.Owner = client;
                client.Entity.AddFlower = r.ReadUInt32("Flower");
                client.MoneySave = r.ReadUInt32("MoneySave");
                client.Entity.Experience = r.ReadUInt64("Experience");
                client.Entity.RacePoints = (uint)r.ReadUInt32("RacePoints");
                client.Entity.BConquerPoints = (uint)r.ReadUInt32("BConquerPoints");
                client.Entity.Money = r.ReadUInt32("Money");
                client.Entity.EditeName = r.ReadUInt32("EditeName");
                client.Entity.ConquerPoints = (uint)r.ReadUInt32("ConquerPoints");
                client.Entity.KoKills = (uint)r.ReadUInt32("KoKills");
                client.Entity.ChiPoints = (uint)r.ReadUInt32("ChiPoints");
                client.Entity.UID = r.ReadUInt32("UID");
                //client.Entity.RacePoints = 54321;
                //client.Entity.RacePoints2 = 12345;
                client.Entity.Status = r.ReadUInt32("Status");
                client.Entity.CountryFlag = r.ReadUInt32("Status2");
                client.Entity.Status3 = r.ReadUInt32("Status3");
                client.Entity.Status4 = r.ReadUInt32("Status4");

                client.Entity.Hitpoints = r.ReadUInt32("Hitpoints");
                client.Entity.Quest = r.ReadUInt32("Quest");
                client.Entity.QuizPoints = r.ReadUInt32("QuizPoints");
                client.Entity.Body = r.ReadUInt16("Body");
                client.Entity.Face = r.ReadUInt16("Face");
                client.Entity.Strength = r.ReadUInt16("Strength");
                //client.Entity.ReincarnationLev = (byte)entities[0].ReincarnationLev;

                client.Entity.Agility = r.ReadUInt16("Agility");
                client.Entity.Spirit = r.ReadUInt16("Spirit");
                client.Entity.Vitality = r.ReadUInt16("Vitality");
                client.Entity.Atributes = r.ReadUInt16("Atributes");
                //client.Entity.SubClass = (byte)entities[0].SubClass;
                client.Entity.SubClassLevel = (uint)r.ReadUInt32("SubClassLevel");
               
                client.Entity.SubClasses.Active = (byte)r.ReadByte("SubClass");
                client.Entity.SubClasses.StudyPoints += r.ReadUInt32("StudyPoints");
                client.VirtuePoints = r.ReadUInt32("VirtuePoints");
                client.Entity.Mana = r.ReadUInt16("Mana");
                client.Entity.HairStyle = r.ReadUInt16("HairStyle");
                client.Entity.MapID = r.ReadUInt32("MapID");
                client.VendingDisguise = r.ReadUInt16("VendingDisguise");
                if (client.VendingDisguise == 0)
                    client.VendingDisguise = 223;
                client.Entity.X = r.ReadUInt16("X");
                client.Entity.Y = r.ReadUInt16("Y");
                if (client.Map.BaseID == 1844)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (ServerBase.Constants.PKFreeMaps2.Contains(client.Entity.MapID))
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 1950)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 3333)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 1)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 1225)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 3031)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 2060)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 3)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 2)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 7777)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 1090)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 4021)
                {
                    client.Entity.MapID = 1020;
                    client.Entity.X = 533;
                    client.Entity.Y = 483;
                }
                if (client.Entity.MapID == 4022)
                {
                    client.Entity.MapID = 1020;
                    client.Entity.X = 533;
                    client.Entity.Y = 483;
                }
                if (client.Entity.MapID == 4023)
                {
                    client.Entity.MapID = 1020;
                    client.Entity.X = 533;
                    client.Entity.Y = 483;
                }
                if (client.Entity.MapID == 4024)
                {
                    client.Entity.MapID = 1020;
                    client.Entity.X = 533;
                    client.Entity.Y = 483;
                }
                if (client.Entity.MapID == 4025)
                {
                    client.Entity.MapID = 1020;
                    client.Entity.X = 533;
                    client.Entity.Y = 483;
                }
                if (client.Entity.MapID == 1508)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 1518)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Map.BaseID == 1005 && client.Entity.MapID != 1005)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Map.BaseID == 1730)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 7005)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 7006)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 7008)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (client.Entity.MapID == 1801)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 428;
                    client.Entity.Y = 378;
                }
                if (Game.Flags.WeeklyPKChampion == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.WeeklyPKChampion);
                }
                if (Game.Flags.MonthlyPKChampion == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.MonthlyPKChampion);
                }
                if (Game.Flags.TopTrojan == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopTrojan);
                }
                if (Game.Flags.TopWarrior == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopWarrior);
                }
                if (Game.Flags.TopArcher == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopArcher);
                }
                if (Game.Flags.TopNinja == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopNinja);
                }
                if (Game.Flags.TopMonk == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.Top2Monk);
                }
                if (Game.Flags.TopWaterTaoist == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopWaterTaoist);
                }
                if (Game.Flags.TopFireTaoist == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopFireTaoist);
                }
                if (Game.Flags.TopPirate == client.Entity.Name)
                {
                    client.Entity.AddFlag2(Network.GamePackets.Update.Flags2.TopPirate);
                }
                if (Game.Flags.TopGuildLeader == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopGuildLeader);
                }
                if (Game.Flags.TopDeputyLeader == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopDeputyLeader);
                }
                if (Game.Flags.TopSpouse == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopSpouse);
                }
                if (Game.Flags.TopSpouse == client.Entity.Spouse)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopSpouse);
                }
                if (Game.Flags.TopDeputyLeader2 == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopDeputyLeader);
                }
                if (Game.Flags.TopDeputyLeader3 == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopDeputyLeader);
                }
                if (Game.Flags.TopDeputyLeader4 == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopDeputyLeader);
                }
                if (Game.Flags.TopDeputyLeader5 == client.Entity.Name)
                {
                    client.Entity.AddFlag(Network.GamePackets.Update.Flags.TopDeputyLeader);
                }
                client.BlessTime = r.ReadUInt32("BlessTime");
                //client.HeadgearClaim = entities[0].HeadgearClaim;
                client.NecklaceClaim = r.ReadBoolean("NecklaceClaim");
                client.ArmorClaim = r.ReadBoolean("ArmorClaim");
                client.WeaponClaim = r.ReadBoolean("WeaponClaim");
                client.RingClaim = r.ReadBoolean("RingClaim");
                client.BootsClaim = r.ReadBoolean("BootsClaim");
                client.FanClaim = r.ReadBoolean("FanClaim");
                client.TowerClaim = r.ReadBoolean("TowerClaim");
                client.HeadgearClaim = r.ReadBoolean("HeadgearClaim");
                client.InLottery = r.ReadBoolean("InLottery");
                client.LotteryEntries = r.ReadByte("LotteryEntries");
                client.LastLotteryEntry = DateTime.FromBinary(r.ReadInt64("LastLotteryEntry"));
                if (client.Entity.MapID >= 7008)
                {
                    client.Entity.MapID = 1002;
                    client.Entity.X = 430;
                    client.Entity.Y = 380;
                }
                client.Entity.PreviousMapID = r.ReadUInt16("PreviousMapID");
                client.Entity.PKPoints = r.ReadUInt16("PKPoints");
                client.Entity.Class = r.ReadByte("Class");
                client.Entity.Reborn = r.ReadByte("Reborn");
                client.Entity.Level = r.ReadByte("Level");
                client.Entity.FirstRebornClass = r.ReadByte("FirstRebornClass");
                client.Entity.SecondRebornClass = r.ReadByte("SecondRebornClass");
                client.Entity.FirstRebornLevel = r.ReadByte("FirstRebornLevel");
                client.Entity.SecondRebornLevel = r.ReadByte("SecondRebornLevel");
                if (client.Entity.Reborn == 0)
                {

                }
                else
                {
                    if (client.Entity.Reborn == 1 && client.Entity.FirstRebornLevel == 0)
                    {
                        client.Entity.FirstRebornLevel = 140;
                    }
                    else
                    {
                        if (client.Entity.Reborn == 2 && client.Entity.FirstRebornLevel == 0 || client.Entity.Reborn == 2 && client.Entity.SecondRebornLevel == 0)
                        {
                            client.Entity.FirstRebornLevel = 140;
                            client.Entity.SecondRebornLevel = 140;
                        }
                    }
                }
                client.LastDragonBallUse = DateTime.FromBinary(r.ReadInt64("LastDragonBallUse"));
                client.LastResetTime = DateTime.FromBinary(r.ReadInt64("LastResetTime"));
                client.Entity.EnlightenPoints = r.ReadUInt16("EnlightenPoints");
                client.Entity.EnlightmentTime = r.ReadUInt16("EnlightmentWait");
                if (client.Entity.EnlightmentTime > 0)
                {
                    if (client.Entity.EnlightmentTime % 20 > 0)
                    {
                        client.Entity.EnlightmentTime -= (ushort)(client.Entity.EnlightmentTime % 20);
                        client.Entity.EnlightmentTime += 20;
                    }
                }
                client.Entity.ReceivedEnlightenPoints = r.ReadByte("EnlightsReceived");
                client.Entity.DoubleExperienceTime = r.ReadUInt16("DoubleExpTime");
                client.DoubleExpToday = r.ReadBoolean("DoubleExpToday");
                client.Entity.HeavenBlessing = r.ReadUInt32("HeavenBlessingTime");
                client.Entity.VIPLevel = r.ReadByte("VIPLevel");
                client.Entity.PrevX = r.ReadUInt16("PreviousX");
                client.Entity.PrevY = r.ReadUInt16("PreviousY");
                client.ExpBalls = r.ReadByte("ExpBalls");
                client.Entity.ClanId = r.ReadUInt32("ClanId");
                //client.Entity.ClanRank = (Clan.Ranks)entities[0].ClanRank;

                if (client.Entity.ClanId != 0)
                {
                    if (PhoenixProject.ServerBase.Kernel.ServerClans.ContainsKey(client.Entity.ClanId))
                    {
                        client.Entity.ClanId = r.ReadUInt32("ClanId");
                        client.Entity.Myclan = PhoenixProject.ServerBase.Kernel.ServerClans[client.Entity.ClanId];
                        client.Entity.ClanName = client.Entity.Myclan.ClanName;
                        if (PhoenixProject.ServerBase.Kernel.ServerClans[client.Entity.ClanId].ClanLider == client.Entity.Name)
                        {
                            client.Entity.ClanRank = 100;
                        }
                        else
                        {
                            client.Entity.ClanRank = 10;
                        }
                        //Console.WriteLine("dddddd");
                    }
                }
                UInt64 lastLoginInt = (ulong)r.ReadInt64("LastLogin");
               // Console.WriteLine(" " + lastLoginInt + "");
                if (lastLoginInt != 0)
                    client.Entity.LastLogin = Kernel.FromDateTimeInt(lastLoginInt);
                else
                    client.Entity.LastLogin = DateTime.Now;

                if (client.Entity.MapID == 601)
                    client.OfflineTGEnterTime = DateTime.FromBinary(r.ReadInt64("OfflineTGEnterTime"));

                Game.ConquerStructures.Nobility.Sort(client.Entity.UID);

                if (ServerBase.Kernel.Guilds.ContainsKey(r.ReadUInt16("GuildID")))
                {
                    client.Guild = ServerBase.Kernel.Guilds[r.ReadUInt16("GuildID")];
                    if (client.Guild.Members.ContainsKey(client.Entity.UID))
                    {
                        client.AsMember = client.Guild.Members[client.Entity.UID];
                        if (client.AsMember.GuildID == 0)
                        {
                            client.AsMember = null;
                            client.Guild = null;
                        }
                        else
                        {
                            client.Entity.GuildID = (ushort)client.Guild.ID;
                            client.Entity.GuildRank = (ushort)client.AsMember.Rank;
                        }
                    }
                    else
                        client.Guild = null;
                }
                if (!Game.ConquerStructures.Nobility.Board.TryGetValue(client.Entity.UID, out client.NobilityInformation))
                {
                    client.NobilityInformation = new PhoenixProject.Game.ConquerStructures.NobilityInformation();
                    client.NobilityInformation.EntityUID = client.Entity.UID;
                    client.NobilityInformation.Name = client.Entity.Name;
                    client.NobilityInformation.Donation = 0;
                    client.NobilityInformation.Rank = PhoenixProject.Game.ConquerStructures.NobilityRank.Serf;
                    client.NobilityInformation.Position = -1;
                    client.NobilityInformation.Gender = 1;
                    client.NobilityInformation.Mesh = client.Entity.Mesh;
                    if (client.Entity.Body % 10 >= 3)
                        client.NobilityInformation.Gender = 0;
                }
                else
                    client.Entity.NobilityRank = client.NobilityInformation.Rank;


                if (DateTime.Now.DayOfYear != client.LastResetTime.DayOfYear)
                {
                    if (client.Entity.Level >= 90)
                    {
                        client.Entity.EnlightenPoints = 100;
                        if (client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Knight ||
                            client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Baron)
                            client.Entity.EnlightenPoints += 100;
                        else if (client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Earl ||
                            client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Duke)
                            client.Entity.EnlightenPoints += 200;
                        else if (client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Prince)
                            client.Entity.EnlightenPoints += 300;
                        else if (client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.King)
                            client.Entity.EnlightenPoints += 400;
                        if (client.Entity.VIPLevel != 0)
                        {
                            if (client.Entity.VIPLevel <= 3)
                                client.Entity.EnlightenPoints += 100;
                            else if (client.Entity.VIPLevel <= 5)
                                client.Entity.EnlightenPoints += 200;
                            else if (client.Entity.VIPLevel == 6)
                                client.Entity.EnlightenPoints += 300;
                        }
                    }
                    client.Entity.ReceivedEnlightenPoints = 0;
                    client.DoubleExpToday = false;
                    client.ExpBalls = 0;
                    client.LotteryEntries = 0;
                    client.Entity.Quest = 0;
                    client.Entity.SubClassLevel = 0;
                    client.LastResetTime = DateTime.Now;
                }
                Game.ConquerStructures.Arena.ArenaStatistics.TryGetValue(client.Entity.UID, out client.ArenaStatistic);
                if (client.ArenaStatistic == null || client.ArenaStatistic.EntityID == 0)
                {
                    client.ArenaStatistic = new PhoenixProject.Network.GamePackets.ArenaStatistic(true);
                    client.ArenaStatistic.EntityID = client.Entity.UID;
                    client.ArenaStatistic.Name = client.Entity.Name;
                    client.ArenaStatistic.Level = client.Entity.Level;
                    client.ArenaStatistic.Class = client.Entity.Class;
                    client.ArenaStatistic.Model = client.Entity.Mesh;
                    client.ArenaStatistic.ArenaPoints = ArenaTable.ArenaPointFill(client.Entity.Level);
                    client.ArenaStatistic.LastArenaPointFill = DateTime.Now;
                    ArenaTable.InsertArenaStatistic(client);
                    client.ArenaStatistic.Status = Network.GamePackets.ArenaStatistic.NotSignedUp;
                    Game.ConquerStructures.Arena.ArenaStatistics.Add(client.Entity.UID, client.ArenaStatistic);
                }
                else
                {
                    client.ArenaStatistic.Level = client.Entity.Level;
                    client.ArenaStatistic.Class = client.Entity.Class;
                    client.ArenaStatistic.Model = client.Entity.Mesh;
                    if (DateTime.Now.DayOfYear != client.ArenaStatistic.LastArenaPointFill.DayOfYear)
                    {
                        client.ArenaStatistic.LastSeasonArenaPoints = client.ArenaStatistic.ArenaPoints;
                        client.ArenaStatistic.LastSeasonWin = client.ArenaStatistic.TodayWin;
                        client.ArenaStatistic.LastSeasonLose = client.ArenaStatistic.TodayBattles - client.ArenaStatistic.TodayWin;
                        client.ArenaStatistic.ArenaPoints = ArenaTable.ArenaPointFill(client.Entity.Level);
                        client.ArenaStatistic.LastArenaPointFill = DateTime.Now;
                        client.ArenaStatistic.TodayWin = 0;
                        client.ArenaStatistic.TodayBattles = 0;
                        Game.ConquerStructures.Arena.Sort();
                        Game.ConquerStructures.Arena.YesterdaySort();
                    }
                }
                //PhoenixProject.Database.ConquerItemTable.LoadItems(client);
                //PhoenixProject.Database.SkillTable.LoadProficiencies(client);
                //PhoenixProject.Database.SkillTable.LoadSpells(client);
                //PhoenixProject.Database.KnownPersons.LoadKnownPersons(client);
                //PhoenixProject.Database.ClaimItemTable.LoadClaimableItems(client);
                //PhoenixProject.Database.DetainedItemTable.LoadDetainedItems(client);
                //Database.SubClassTable.Load(client.Entity);
                //PhoenixProject.Database.FlowerSystemTable.Flowers(client);
                //Game.Tournaments.EliteTournament.LoginClient(client);
                client.Entity.FullyLoaded = true;
                r.Close();
                r.Dispose();
            }
            else

                return false;
            return true;      
               

            }
        public static void NextUit()
        {
            if (rates.lastitem == 0)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("items");
                PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
                while (r.Read())
                {
                    uint uid = r.ReadUInt32("UID");
                    if (uid > 0)
                    {
                        if (uid > PhoenixProject.Client.AuthState.nextID)
                            PhoenixProject.Client.AuthState.nextID = uid;
                    }
                    if (uid == 0)
                    {
                        PhoenixProject.Client.AuthState.nextID = 1;
                    }
                }
                r.Close();
                r.Dispose();
                PhoenixProject.Client.AuthState.nextID += 1;
                new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastItem", PhoenixProject.Client.AuthState.nextID).Where("Coder", "kimo").Execute();
                Console.WriteLine("Last Item UID: " + PhoenixProject.Client.AuthState.nextID + "");
            }
            else
            {
                PhoenixProject.Client.AuthState.nextID  = rates.lastitem;
                PhoenixProject.Client.AuthState.nextID += 1;
                Console.WriteLine("Last Item UID: " + PhoenixProject.Client.AuthState.nextID + "");
            }
        }
        public static void NextEntity()
        {
            if (rates.lastentity == 0)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("entities");
                PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);


                uint uidd = 0;
                while (r.Read())
                {
                    uint uid = r.ReadUInt32("UID");
                    if (uid > 0)
                    {
                        if (uid > Program.nextEntityID)
                        {
                            Program.nextEntityID = uid;
                            uidd = uid;
                        }
                    }

                }
                r.Close();
                r.Dispose();


                if (uidd != 0)
                {
                    Database.rates.lastentity = Program.nextEntityID;
                    Program.nextEntityID += 1;
                    new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastEntity", Program.nextEntityID).Where("Coder", "kimo").Execute();
                }
                else
                {
                    Program.nextEntityID = 1000000;
                    Database.rates.lastentity = Program.nextEntityID;
                    new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastEntity", Program.nextEntityID).Where("Coder", "kimo").Execute();
                }

                Console.WriteLine("Last Entity UID: " + Program.nextEntityID + "");
            }
            else
            {
                Program.nextEntityID = rates.lastentity;
                Program.nextEntityID += 1;
                Console.WriteLine("Last Entity UID: " + Program.nextEntityID + "");
            }
        }
        public static void NextGuild()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("guilds");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            while (r.Read())
            {
                uint uid = r.ReadUInt32("ID");
                if (uid > 0)
                {
                    if (uid > Program.nextGuildID)
                        Program.nextGuildID = uid;
                }
            }
            r.Close();
            r.Dispose();
            Program.nextGuildID += 1;
            Console.WriteLine("Last Guild UID: " + Program.nextGuildID + "");
        }
       
        public static void UpdateOnlineStatus(Client.GameState client, bool online, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            if (online || (!online && client.DoSetOffline))
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("entities").Set("Online", online).Where("UID", client.Entity.UID).Execute();
            }
        }
        public static void UpdateOnlineStatus(Client.GameState client, bool online)
        {
            if (online || (!online && client.DoSetOffline))
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("entities").Set("Online", online).Where("UID", client.Entity.UID).Execute();
            }
        }
        public static void UpdateCps(Client.GameState client)
        {
            //UpdateData(client, "ConquerPoints", client.Entity.ConquerPoints);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("ConquerPoints", client.Entity.ConquerPoints).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateMoney(Client.GameState client)
        {
            //UpdateData(client, "Money", client.Entity.Money);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Money", client.Entity.Money).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateLevel(Client.GameState client)
        {
           // UpdateData(client, "Level", client.Entity.Level);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Level", client.Entity.Level).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateTrojanStatus(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateWarriorStatus(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateArcherStatus(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateGuildID(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("GuildID", client.Entity.GuildID).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateClanID(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("ClanId", client.Entity.ClanId).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateClanRank(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("ClanRank", (uint)client.Entity.ClanRank).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateGuildRank(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("GuildRank", client.Entity.GuildRank).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateNinjaStatus(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateMonkStatus(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateWaterStatus(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateFireStatus(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateTrojanStatus1()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("Status", 1).Execute();
        }
        public static void UpdateWarriorStatus1()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("Status", 2).Execute();
        }
        public static void UpdateArcherStatus1()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("Status", 3).Execute();
        }
        public static void UpdateNinjaStatus1()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("Status", 4).Execute();
        }
      
        public static void UpdateMonkStatus1()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("Status", 5).Execute();
        }
        public static void UpdateWaterStatus1()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("Status", 6).Execute();
        }
        public static void UpdateSpouse()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("HeadgearClaim", 0).Where("HeadgearClaim", 1).Execute();
        }
        public static void UpdateFireStatus1()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status", 0).Where("Status", 7).Execute();
        }
        public static void UpdateStatus3(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status3", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateStatus4(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status4", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateDonation(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Donation", client.Entity.NobalityDonation).Where("UID", client.Entity.UID).Execute();
        }
        public static void UpdateStatus33()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status3", 0).Where("Status3", 1).Execute();
        }
        public static void UpdateStatus44()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status4", 0).Where("Status4", 1).Execute();
        }
        public static void Status2()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Status2", 0).Where("Status2", 1).And("Status2", 2).Execute();
        }
        public static void ResetLottery(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("LotteryEntries", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void ResetQuest(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("Quest", 0).Where("UID", client.Entity.UID).Execute();
        }
        public static void ResetExpball(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("entities").Set("ExpBalls", 0).Where("UID", client.Entity.UID).Execute();
        }
        
        static bool InvalidCharacters(string Name)
        {
            foreach (char c in Name)
                if (ServerBase.Kernel.InvalidCharacters.Contains(c) || (byte)c < 48)
                    return true;
            return false;
        }
        public static bool CreateEntity(Network.GamePackets.EnitityCreate eC, Client.GameState client, ref string message)
        {
            if (eC.Name.Length > 16)
                eC.Name = eC.Name.Substring(0, 16);
            if (eC.Name == "")
                return false;

            if (InvalidCharacters(eC.Name))
            {
                message = "Invalid characters inside the name.";
                return false;
            }
            var rdr =new PhoenixProject.Database.MySqlReader(new MySqlCommand(MySqlCommandType.SELECT).Select("entities").Where("name", eC.Name));
            if (rdr.Read())
            {
                rdr.Close();
                rdr.Dispose();
                message = "The chosen name is already in use.";
                return false;
            }
            rdr.Close();
            rdr.Dispose();
            client.Entity = new Game.Entity(Game.EntityFlag.Player, false);
            client.Entity.Name = eC.Name;
            if (eC.Class != 0)
            {
                DataHolder.GetStats(eC.Class, 1, client);
            }
            else
            {
                DataHolder.GetStats(70, 1, client);
                eC.Class = 70;
            }
            client.Entity.UID = Program.nextEntityID;
            Program.nextEntityID++;
            new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastEntity", Program.nextEntityID).Where("Coder", "kimo").Execute();
            client.CalculateStatBonus();
            client.CalculateHPBonus();
            client.Entity.Hitpoints = client.Entity.MaxHitpoints;
            client.Entity.Mana = (ushort)(client.Entity.Spirit * 5);
            client.Entity.Class = eC.Class;
            client.Entity.Body = eC.Body;
            if (eC.Body == 1003 || eC.Body == 1004)
                client.Entity.Face = (ushort)ServerBase.Kernel.Random.Next(1, 50);
            else
                client.Entity.Face = (ushort)ServerBase.Kernel.Random.Next(201, 250);
            byte Color = (byte)ServerBase.Kernel.Random.Next(4, 8);
            client.Entity.HairStyle = (ushort)(Color * 100 + 10 + (byte)ServerBase.Kernel.Random.Next(4, 9));
            client.Account.EntityID = client.Entity.UID;
            client.Account.Save();

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("entities").Insert("Name", eC.Name).Insert("Owner", client.Account.Username).Insert("Class", eC.Class).Insert("UID", client.Entity.UID)
                .Insert("Hitpoints", client.Entity.Hitpoints).Insert("Mana", client.Entity.Mana).Insert("Body", client.Entity.Body)
                .Insert("Face", client.Entity.Face).Insert("HairStyle", client.Entity.HairStyle).Insert("Strength", client.Entity.Strength)
                .Insert("WarehousePW","")
                .Insert("Agility", client.Entity.Agility).Insert("Vitality", client.Entity.Vitality).Insert("Spirit", client.Entity.Spirit);

            cmd.Execute();
            message = "ANSWER_OK";
            return true;
        }
        public static bool SaveEntity(Client.GameState client)
        {
            try
            {
               

                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("entities").Set("WarehousePW", client.WarehousePW)
                    .Set("KoKills", client.Entity.KoKills)
                    .Set("RacePoints", client.Entity.RacePoints)
                    .Set("EditeName", client.Entity.EditeName)
                    .Set("BConquerPoints", client.Entity.BConquerPoints)
                    .Set("Donation", client.Entity.NobalityDonation)
                    .Set("StudyPoints", client.Entity.SubClasses.StudyPoints)
                    .Set("Spouse", client.Entity.Spouse).Set("Money", client.Entity.Money)
                    .Set("ConquerPoints", client.Entity.ConquerPoints).Set("ChiPoints", client.Entity.ChiPoints).Set("Body", client.Entity.Body)
                    .Set("Face", client.Entity.Face).Set("Class", client.Entity.Class).Set("Reborn", client.Entity.Reborn)
                    .Set("Level", client.Entity.Level).Set("HairStyle", client.Entity.HairStyle).Set("EnlightsReceived", client.Entity.ReceivedEnlightenPoints)
                    .Set("PKPoints", client.Entity.PKPoints).Set("QuizPoints", client.Entity.QuizPoints)
                    .Set("Experience", client.Entity.Experience).Set("MoneySave", client.MoneySave)
                    .Set("Hitpoints", client.Entity.Hitpoints).Set("LastDragonBallUse", client.LastDragonBallUse.ToBinary())
                    .Set("Strength", client.Entity.Strength).Set("Agility", client.Entity.Agility)
                    .Set("Spirit", client.Entity.Spirit).Set("Vitality", client.Entity.Vitality)
                    .Set("PreviousX", client.Entity.PrevX).Set("PreviousY", client.Entity.PrevY)
                    .Set("Atributes", client.Entity.Atributes).Set("Mana", client.Entity.Mana).Set("VIPLevel", client.Entity.VIPLevel)
                    .Set("MapID", client.Map.ID).Set("X", client.Entity.X).Set("Y", client.Entity.Y).Set("VirtuePoints", client.VirtuePoints)
                    .Set("PreviousMapID", client.Entity.PreviousMapID).Set("EnlightenPoints", client.Entity.EnlightenPoints)
                    .Set("LastResetTime", client.LastResetTime.ToBinary())
                    .Set("DoubleExpTime", client.Entity.DoubleExperienceTime)
                    .Set("DoubleExpToday", client.DoubleExpToday).Set("HeavenBlessingTime", client.Entity.HeavenBlessing)
                    .Set("InLottery", client.InLottery).Set("LotteryEntries", client.LotteryEntries).Set("LastLotteryEntry", client.LastLotteryEntry.Ticks)
                    .Set("HeadgearClaim", client.HeadgearClaim).Set("NecklaceClaim", client.NecklaceClaim).Set("ArmorClaim", client.ArmorClaim)
                    .Set("WeaponClaim", client.WeaponClaim).Set("RingClaim", client.RingClaim).Set("BootsClaim", client.BootsClaim)
                    .Set("TowerClaim", client.TowerClaim).Set("FanClaim", client.FanClaim).Set("ChatBanTime", client.ChatBanTime.Ticks)
                    .Set("ChatBanLasts", client.ChatBanLasts).Set("ChatBanned", client.ChatBanned).Set("BlessTime", client.BlessTime)
                    .Set("ExpBalls", client.ExpBalls).Set("Status2", client.Entity.CountryFlag).Set("Status4", (uint)client.Entity.Status4).Set("SubClassLevel", (uint)client.Entity.SubClassLevel)


                    
                    .Set("FirstRebornLevel", client.Entity.FirstRebornLevel)
                    .Set("Quest", client.Entity.Quest)
                    .Set("SecondRebornLevel", client.Entity.SecondRebornLevel)
                    .Set("Money", client.Entity.Money)
                    .Set("FirstRebornClass", client.Entity.FirstRebornClass)
                    .Set("SecondRebornClass", client.Entity.SecondRebornClass)
                    .Set("ConquerPoints", client.Entity.ConquerPoints).Set("EnlightmentWait", client.Entity.EnlightmentTime).Set("LastLogin", Kernel.ToDateTimeInt(DateTime.Now));
               
                if (client.Entity.MapID == 601)
                    cmd.Set("OfflineTGEnterTime", client.OfflineTGEnterTime.Ticks);
                else
                    cmd.Set("OfflineTGEnterTime", 0);

                if (client.AsMember != null)
                {
                    cmd.Set("GuildID", client.AsMember.GuildID).
                        Set("GuildRank", (ushort)client.AsMember.Rank).
                        Set("GuildSilverDonation", client.AsMember.SilverDonation).
                        Set("GuildConquerPointDonation", client.AsMember.ConquerPointDonation);
                }
                else
                {
                    cmd.Set("GuildID", 0).
                       Set("GuildRank", 0).
                       Set("GuildSilverDonation", 0).
                       Set("GuildConquerPointDonation", 0);
                }

                if (client.Entity.Myclan != null)
                {
                    cmd.Set("ClanId", client.Entity.Myclan.ClanId).
                        Set("ClanRank", client.Entity.ClanRank);
                        //Set("ClanDonation", client.clan);
                        //Set("GuildConquerPointDonation", client.AsMember.ConquerPointDonation);
                }
                else
                {
                    cmd.Set("ClanId", 0).
                       Set("ClanRank", 0).
                       Set("ClanDonation", 0);
                       //Set("GuildConquerPointDonation", 0);
                }
                cmd.Where("UID", client.Entity.UID).Execute();
            }
            catch (Exception e) { Program.SaveException(e); } return true;
        }
        public static bool SaveEntity(Client.GameState client, MySql.Data.MySqlClient.MySqlConnection conn)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("entities").Set("WarehousePW", client.WarehousePW)
                    .Set("Spouse", client.Entity.Spouse).Set("Money", client.Entity.Money)
                    .Set("ConquerPoints", client.Entity.ConquerPoints).Set("Body", client.Entity.Body)
                    .Set("Face", client.Entity.Face).Set("Class", client.Entity.Class).Set("Reborn", client.Entity.Reborn)
                    .Set("Level", client.Entity.Level).Set("HairStyle", client.Entity.HairStyle).Set("EnlightsReceived", client.Entity.ReceivedEnlightenPoints)
                    .Set("PKPoints", client.Entity.PKPoints).Set("QuizPoints", client.Entity.QuizPoints)
                    .Set("Experience", client.Entity.Experience).Set("MoneySave", client.MoneySave)
                    .Set("Hitpoints", client.Entity.Hitpoints).Set("LastDragonBallUse", client.LastDragonBallUse.ToBinary())
                    .Set("Strength", client.Entity.Strength).Set("Agility", client.Entity.Agility)
                    .Set("Spirit", client.Entity.Spirit).Set("Vitality", client.Entity.Vitality)
                    .Set("PreviousX", client.Entity.PrevX).Set("PreviousY", client.Entity.PrevY)
                    .Set("Atributes", client.Entity.Atributes).Set("Mana", client.Entity.Mana).Set("VIPLevel", client.Entity.VIPLevel)
                    .Set("MapID", client.Map.ID).Set("X", client.Entity.X).Set("Y", client.Entity.Y).Set("VirtuePoints", client.VirtuePoints)
                    .Set("PreviousMapID", client.Entity.PreviousMapID).Set("EnlightenPoints", client.Entity.EnlightenPoints)
                    .Set("LastResetTime", client.LastResetTime.ToBinary())
                    .Set("DoubleExpTime", client.Entity.DoubleExperienceTime)
                    .Set("DoubleExpToday", client.DoubleExpToday).Set("HeavenBlessingTime", client.Entity.HeavenBlessing)
                    .Set("InLottery", client.InLottery).Set("LotteryEntries", client.LotteryEntries).Set("LastLotteryEntry", client.LastLotteryEntry.Ticks)
                    .Set("HeadgearClaim", client.HeadgearClaim).Set("NecklaceClaim", client.NecklaceClaim).Set("ArmorClaim", client.ArmorClaim)
                    .Set("WeaponClaim", client.WeaponClaim).Set("RingClaim", client.RingClaim).Set("BootsClaim", client.BootsClaim)
                    .Set("TowerClaim", client.TowerClaim).Set("FanClaim", client.FanClaim).Set("ChatBanTime", client.ChatBanTime.Ticks)
                    .Set("ChatBanLasts", client.ChatBanLasts).Set("ChatBanned", client.ChatBanned).Set("BlessTime", client.BlessTime)
                    .Set("ExpBalls", client.ExpBalls)

                    .Set("Quest", client.Entity.Quest)
                    .Set("FirstRebornLevel", client.Entity.FirstRebornLevel)
                    .Set("SecondRebornLevel", client.Entity.SecondRebornLevel)
                    .Set("Money", client.Entity.Money)
                    .Set("ConquerPoints", client.Entity.ConquerPoints).Set("EnlightmentWait", client.Entity.EnlightmentTime);
                if (client.Entity.Reborn == 1)
                {
                    cmd.Set("FirstRebornClass", client.Entity.FirstRebornClass);
                }
                if (client.Entity.Reborn == 2)
                {
                    cmd.Set("SecondRebornClass", client.Entity.SecondRebornClass);
                }
                if (client.Entity.MapID == 601)
                    cmd.Set("OfflineTGEnterTime", client.OfflineTGEnterTime.Ticks);
                else
                    cmd.Set("OfflineTGEnterTime", 0);

                if (client.AsMember != null)
                {
                    cmd.Set("GuildID", client.AsMember.GuildID).
                        Set("GuildRank", (ushort)client.AsMember.Rank).
                        Set("GuildSilverDonation", client.AsMember.SilverDonation).
                        Set("GuildConquerPointDonation", client.AsMember.ConquerPointDonation);
                }
                else
                {
                    cmd.Set("GuildID", 0).
                       Set("GuildRank", 0).
                       Set("GuildSilverDonation", 0).
                       Set("GuildConquerPointDonation", 0);
                }

                if (client.Entity.Myclan != null)
                {
                    cmd.Set("ClanId", client.Entity.Myclan.ClanId).
                        Set("ClanRank", client.Entity.ClanRank);
                    //Set("ClanDonation", client.clan);
                    //Set("GuildConquerPointDonation", client.AsMember.ConquerPointDonation);
                }
                else
                {
                    cmd.Set("ClanId", 0).
                       Set("ClanRank", 0).
                       Set("ClanDonation", 0);
                    //Set("GuildConquerPointDonation", 0);
                }
                cmd.Where("UID", client.Entity.UID).Execute();
            }
            catch (Exception e) { Program.SaveException(e); } return true;
        }
        public static void UpdateNames()
        {
            Dictionary<String, NameChangeC> UPDATE = new Dictionary<string, NameChangeC>();
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("nobility");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
           // String newname = "", name = "";
            int UID;
            ulong Donation;
            while (r.Read())
            {
                //newname = r.ReadString("namechange");
                //name = r.ReadString("name");
                UID = (int)r.ReadInt64("EntityUID");
                Donation = (ulong)r.ReadInt64("Donation");
                if (Donation != 0)
                {
                    MySqlCommand cmdupdate = null;
                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("entities").Set("Donation", Donation).Where("UID", UID).Execute();
                    Console.WriteLine("Donation Set.");
                }
            }
            r.Close();
            r.Dispose();
        }
    }
}
