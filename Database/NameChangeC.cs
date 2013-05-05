using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Database
{
    public class NameChange
    {
        public static void UpdateNames()
        {
            Dictionary<String, NameChangeC> UPDATE = new Dictionary<string, NameChangeC>();
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("entities");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            String newname = "", name = "";
            int UID;
            while (r.Read())
            {
                newname = r.ReadString("namechange");
                name = r.ReadString("name");
                UID = (int)r.ReadInt64("UID");
                if (newname != "")
                {
                    MySqlCommand cmdupdate = null;
                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("apprentice").Set("MentorName", newname).Where("MentorID", UID).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("apprentice").Set("ApprenticeName", newname).Where("ApprenticeID", UID).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("elitepk").Set("Name", newname).Where("UID", UID).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopArcher", newname).Where("TopArcher", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopPirate", newname).Where("TopPirate", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopTrojan", newname).Where("TopTrojan", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopGuildLeader", newname).Where("TopGuildLeader", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopNinja", newname).Where("TopNinja", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopMonk", newname).Where("TopMonk", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopWarrior", newname).Where("TopWarrior", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopSpouse", newname).Where("TopSpouse", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopWaterTaoist", newname).Where("TopWaterTaoist", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopFireTaoist", newname).Where("TopFireTaoist", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("MonthlyPkChampion", newname).Where("MonthlyPkChampion", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("WeeklyPkChampion", newname).Where("WeeklyPkChampion", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopDeputyLeader", newname).Where("TopDeputyLeader", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopDeputyLeader2", newname).Where("TopDeputyLeader2", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopDeputyLeader3", newname).Where("TopDeputyLeader3", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopDeputyLeader4", newname).Where("TopDeputyLeader4", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("flags").Set("TopDeputyLeader5", newname).Where("TopDeputyLeader5", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("arena").Set("EntityName", newname).Where("EntityID", UID).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("claimitems").Set("OwnerName", newname).Where("OwnerName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("claimitems").Set("GainerName", newname).Where("GainerName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("detaineditems").Set("OwnerName", newname).Where("OwnerName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("detaineditems").Set("GainerName", newname).Where("GainerName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("enemy").Set("EnemyName", newname).Where("EnemyID", UID).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("friends").Set("FriendName", newname).Where("FriendID", UID).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("guilds").Set("Name", newname).Where("Name", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("guilds").Set("LeaderName", newname).Where("LeaderName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("clans").Set("Leader", newname).Where("Leader", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("nobility").Set("EntityName", newname).Where("EntityUID", UID).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("partners").Set("PartnerName", newname).Where("PartnerID", UID).Execute();

                    if (!UPDATE.ContainsKey(name))
                        UPDATE.Add(name, new NameChangeC() { NewName = newname, OldName = name });
                }
            }
            r.Close();
            r.Dispose();
            if (UPDATE.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(" [NAME CHANGES]");
            }
            foreach (NameChangeC names in UPDATE.Values)
            {
                MySqlCommand cmdupdate2 = new MySqlCommand(MySqlCommandType.UPDATE);
                cmdupdate2.Update("entities").Set("Name", names.NewName).Set("namechange", "").Where("Name", names.OldName).Execute();
                
                Console.WriteLine(" -[" + names.OldName + "] : -[" + names.NewName + "]");
                Console.ForegroundColor = ConsoleColor.White;
            }
            foreach (NameChangeC names in UPDATE.Values)
            {
                MySqlCommand cmdupdate2 = new MySqlCommand(MySqlCommandType.UPDATE);
                cmdupdate2.Update("entities").Set("Spouse", names.NewName).Where("Spouse", names.OldName).Execute();
            }
            UPDATE.Clear();
        }
    }
    public class NameChangeC
    {
        public String NewName;
        public String OldName;
    }
}
