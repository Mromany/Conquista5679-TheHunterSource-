using System;
using System.Collections.Generic;
using PhoenixProject.Game.ConquerStructures.Society;

using SubSonic;

namespace PhoenixProject.Database
{
    public class KnownPersons
    {
        public static void LoadEnemy(Client.GameState client)
        {
            client.Enemy = new SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.Enemy>(50);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd = new MySqlCommand(MySqlCommandType.SELECT);
            
            cmd.Select("enemy").Where("EntityID", client.Entity.UID);
            MySqlReader reader = new MySqlReader(cmd);
            reader = new MySqlReader(cmd);
            while (reader.Read())
            {
                Game.ConquerStructures.Society.Enemy enemy = new Game.ConquerStructures.Society.Enemy();
                enemy.ID = reader.ReadUInt32("EnemyID");
                enemy.Name = reader.ReadString("EnemyName");
                client.Enemy.Add(enemy.ID, enemy);
            }
            reader.Close();
            reader.Dispose();
        }
        public static void LoadPartner(Client.GameState client)
        {
            client.Partners = new SafeDictionary<uint, TradePartner>(40);

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            
            
            cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("partners").Where("EntityID", client.Entity.UID);
            MySqlReader reader = new MySqlReader(cmd);
            reader = new MySqlReader(cmd);
            while (reader.Read())
            {
                Game.ConquerStructures.Society.TradePartner partner = new Game.ConquerStructures.Society.TradePartner();
                partner.ID = reader.ReadUInt32("PartnerID");
                partner.Name = reader.ReadString("PartnerName");
                partner.ProbationStartedOn = DateTime.FromBinary(reader.ReadInt64("ProbationStartedOn"));
                client.Partners.Add(partner.ID, partner);
            }
            reader.Close();
            reader.Dispose();
        }
        public static void LoadMentor(Client.GameState client)
        {
            client.Apprentices = new SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.Apprentice>(10);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);

           
            cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("apprentice").Where("MentorID", client.Entity.UID);
            MySqlReader reader = new MySqlReader(cmd);
            //reader = new MySqlReader(cmd);
            while (reader.Read())
            {
                Game.ConquerStructures.Society.Apprentice app = new Game.ConquerStructures.Society.Apprentice();
                app.ID = reader.ReadUInt32("ApprenticeID");
                app.Name = reader.ReadString("ApprenticeName");
                app.EnroleDate = reader.ReadUInt32("EnroleDate");
                app.Actual_Experience = reader.ReadUInt64("Actual_Experience");
                app.Total_Experience = reader.ReadUInt64("Total_Experience");
                app.Actual_Plus = reader.ReadUInt16("Actual_Plus");
                app.Total_Plus = reader.ReadUInt16("Total_Plus");
                app.Actual_HeavenBlessing = reader.ReadUInt16("Actual_HeavenBlessing");
                app.Total_HeavenBlessing = reader.ReadUInt16("Total_HeavenBlessing");
                client.PrizeExperience += app.Actual_Experience;
                client.PrizePlusStone += app.Actual_Plus;
                client.PrizeHeavenBlessing += app.Actual_HeavenBlessing;
                client.Apprentices.Add(app.ID, app);

                if (client.PrizeExperience > 50 * 606)
                    client.PrizeExperience = 50 * 606;
            }
            reader.Close();
            reader.Dispose();

            cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("apprentice").Where("ApprenticeID", client.Entity.UID);
            reader = new MySqlReader(cmd);
            while (reader.Read())
            {
                client.Mentor = new Mentor();
                client.Mentor.ID = reader.ReadUInt32("MentorID");
                client.Mentor.Name = reader.ReadString("MentorName");
                client.Mentor.EnroleDate = reader.ReadUInt32("EnroleDate");
                client.AsApprentice = new Game.ConquerStructures.Society.Apprentice();
                client.AsApprentice.ID = client.Entity.UID;
                client.AsApprentice.Name = client.Entity.Name;
                client.AsApprentice.EnroleDate = client.Mentor.EnroleDate;
                client.AsApprentice.Actual_Experience = reader.ReadUInt64("Actual_Experience");
                client.AsApprentice.Total_Experience = reader.ReadUInt64("Total_Experience");
                client.AsApprentice.Actual_Plus = reader.ReadUInt16("Actual_Plus");
                client.AsApprentice.Total_Plus = reader.ReadUInt16("Total_Plus");
                client.AsApprentice.Actual_HeavenBlessing = reader.ReadUInt16("Actual_HeavenBlessing");
                client.AsApprentice.Total_HeavenBlessing = reader.ReadUInt16("Total_HeavenBlessing");
            }
            reader.Close();
            reader.Dispose();
        }
       /* public static void LoadKnownPersons(Client.GameState client)
        {
            client.Friends = new SafeDictionary<uint, Friend>(50);
            client.Enemy = new SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.Enemy>(10);
            client.Partners = new SafeDictionary<uint, TradePartner>(40);
            client.Apprentices = new SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.Apprentice>(10);
            FriendXCollection entities = new FriendXCollection();
            entities.LoadAndCloseReader(FriendX.FetchByParameter("EntityID", client.Entity.UID));
            for (int x = 0; x < entities.Count; x++)
            {
                Friend friend = new Friend();
                friend.ID = (uint)entities[x].FriendID;
                friend.Name = entities[x].FriendName;
                friend.Message = entities[x].Message;
                client.Friends.Add(friend.ID, friend);
            }

            EnemyCollection entities2 = new EnemyCollection();
            entities2.LoadAndCloseReader(Conquer.Database.Enemy.FetchByParameter("EntityID", client.Entity.UID));
            for (int x = 0; x < entities2.Count; x++)
            {
                PhoenixProject.Game.ConquerStructures.Society.Enemy enemy = new PhoenixProject.Game.ConquerStructures.Society.Enemy();
                enemy.ID = (uint)entities2[x].EnemyID;
                enemy.Name = entities2[x].EnemyName;
                client.Enemy.Add(enemy.ID, enemy);
            }

            PartnerCollection entities3 = new PartnerCollection();
            entities3.LoadAndCloseReader(Conquer.Database.Partner.FetchByParameter("EntityID", client.Entity.UID));
            for (int x = 0; x < entities3.Count; x++)
            {
                TradePartner partner = new TradePartner();
                partner.ID = (uint)entities3[x].PartnerID;
                partner.Name = entities3[x].PartnerName;
                partner.ProbationStartedOn = DateTime.FromBinary(entities3[x].ProbationStartedOn);
                client.Partners.Add(partner.ID, partner);
            }

            ApprenticeCollection entities4 = new ApprenticeCollection();
            entities4.LoadAndCloseReader(Conquer.Database.Apprentice.FetchByParameter("MentorID", client.Entity.UID));
            for (int x = 0; x < entities4.Count; x++)
            {
                PhoenixProject.Game.ConquerStructures.Society.Apprentice app = new PhoenixProject.Game.ConquerStructures.Society.Apprentice();
                app.ID = (uint)entities4[x].ApprenticeID;
                app.Name = entities4[x].ApprenticeName;
                app.EnroleDate = (uint)entities4[x].EnroleDate;
                app.Actual_Experience = (ulong)entities4[x].ActualExperience;
                app.Total_Experience = (ulong)entities4[x].TotalExperience;
                app.Actual_Plus = (ushort)entities4[x].ActualPlus;
                app.Total_Plus = (ushort)entities4[x].TotalPlus;
                app.Actual_HeavenBlessing = (ushort)entities4[x].ActualHeavenBlessing;
                app.Total_HeavenBlessing = (ushort)entities4[x].TotalHeavenBlessing;
                client.PrizeExperience += app.Actual_Experience;
                client.PrizePlusStone += app.Actual_Plus;
                client.PrizeHeavenBlessing += app.Actual_HeavenBlessing;
                client.Apprentices.Add(app.ID, app);
                client.apprtnum += 1;


                if (client.PrizeExperience > 50 * 606)
                    client.PrizeExperience = 50 * 606;
            }

            ApprenticeCollection entities5 = new ApprenticeCollection();
            entities5.LoadAndCloseReader(Conquer.Database.Apprentice.FetchByParameter("ApprenticeID", client.Entity.UID));
            for (int x = 0; x < entities5.Count; x++)
            {
                client.Mentor = new Mentor();
                client.Mentor.ID = (uint)entities5[x].MentorID;
                client.Mentor.Name = entities5[x].MentorName;
                client.Mentor.EnroleDate = (uint)entities5[x].EnroleDate;
                client.AsApprentice = new PhoenixProject.Game.ConquerStructures.Society.Apprentice();
                client.AsApprentice.ID = client.Entity.UID;
                client.AsApprentice.Name = client.Entity.Name;
                client.AsApprentice.EnroleDate = client.Mentor.EnroleDate;
                client.AsApprentice.Actual_Experience = (ulong)entities5[x].ActualExperience;
                client.AsApprentice.Total_Experience = (ulong)entities5[x].TotalExperience;
                client.AsApprentice.Actual_Plus = (ushort)entities5[x].ActualPlus;
                client.AsApprentice.Total_Plus = (ushort)entities5[x].TotalPlus;
                client.AsApprentice.Actual_HeavenBlessing = (ushort)entities5[x].ActualHeavenBlessing;
                client.AsApprentice.Total_HeavenBlessing = (ushort)entities5[x].TotalHeavenBlessing;
            }
        }*/
        public static void LoaderFriends(Client.GameState client)
        {
            client.Friends = new SafeDictionary<uint, Friend>(50);
           
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("friends").Where("EntityID", client.Entity.UID);
            MySqlReader reader = new MySqlReader(cmd);
            while (reader.Read())
            {
                Friend friend = new Friend();
                friend.ID = reader.ReadUInt32("FriendID");
                friend.Name = reader.ReadString("FriendName");
                friend.Message = reader.ReadString("Message");
                client.Friends.Add(friend.ID, friend);
            }
            reader.Close();
            reader.Dispose();

        }
        public static void SaveApprenticeInfo(PhoenixProject.Game.ConquerStructures.Society.Apprentice app)
        {
            if (app != null)
            {
                MySqlCommand mysqlcmd = new MySqlCommand(MySqlCommandType.UPDATE);
                mysqlcmd.Update("apprentice")
                    .Set("Actual_Experience", app.Actual_Experience.ToString())
                    .Set("Total_Experience", app.Total_Experience.ToString())
                    .Set("Actual_Plus", app.Actual_Plus.ToString())
                    .Set("Total_Plus", app.Total_Plus.ToString())
                    .Set("Actual_HeavenBlessing", app.Actual_HeavenBlessing.ToString())
                    .Set("Total_HeavenBlessing", app.Total_HeavenBlessing.ToString()).Where("ApprenticeID", app.ID).Execute();
            }
        }
        public static void AddMentor(Mentor mentor, PhoenixProject.Game.ConquerStructures.Society.Apprentice appr)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("apprentice").Insert("mentorid", mentor.ID).Insert("apprenticeid", appr.ID)
                .Insert("mentorname", mentor.Name).Insert("apprenticename", appr.Name).Insert("enroledate", appr.EnroleDate).Execute();
        }
        public static void RemoveMentor(uint apprenticeuid)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("apprentice", "apprenticeid", apprenticeuid).Execute();
        }
        public static void RemoveApprentice(Client.GameState client, uint apprenticeID)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("apprentice", "apprenticeid", apprenticeID).Execute();
        }
        public static void RemoveFriend(Client.GameState client, uint friendID)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("friends", "friendid", friendID).And("entityid", client.Entity.UID).Execute();
            cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("friends", "entityid", friendID).And("friendid", client.Entity.UID).Execute();
        }
        public static void RemovePartner(Client.GameState client, uint partnerID)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("partners", "partnerid", partnerID).And("entityid", client.Entity.UID).Execute();
            cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("partners", "entityid", partnerID).And("partnerid", client.Entity.UID).Execute();
        }
        public static void RemoveEnemy(Client.GameState client, uint enemyID)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("enemy", "enemyid", enemyID).And("entityid", client.Entity.UID).Execute();
        }

        public static void AddFriend(Client.GameState client, Friend friend)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("friends").Insert("entityid", client.Entity.UID).Insert("friendid", friend.ID)
                .Insert("friendname", friend.Name).Execute();
            cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("friends").Insert("entityid", friend.ID).Insert("friendid", client.Entity.UID)
                .Insert("friendname", client.Entity.Name).Execute();
        }
        public static void AddPartner(Client.GameState client, TradePartner partner)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("partners").Insert("entityid", client.Entity.UID).Insert("partnerid", partner.ID)
                .Insert("partnername", partner.Name).Insert("probationstartedon", partner.ProbationStartedOn.Ticks).Execute();
            cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("partners").Insert("entityid", partner.ID).Insert("partnerid", client.Entity.UID)
                .Insert("partnername", client.Entity.Name).Insert("probationstartedon", partner.ProbationStartedOn.Ticks).Execute();
        }
        public static void AddEnemy(Client.GameState client, Game.ConquerStructures.Society.Enemy enemy)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("enemy").Insert("entityid", client.Entity.UID).Insert("enemyid", enemy.ID)
                .Insert("enemyname", enemy.Name).Execute();
        }
        public static void UpdateMessageOnFriend(uint entityID, uint friendID, string message)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            message = message.Replace("\'", "");
            cmd.Update("friends").Set("Message", message).Where("EntityID", friendID).And("FriendID", entityID).Execute();
        }
    }
}
