using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Database;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game
{
    class Flags//Coded By Kimo
    {
        public static string TopArcher = "";
        public static string TopWarrior = "";
        public static string TopNinja = "";
        public static string TopMonk = "";
        public static string TopTrojan = "";
        public static string TopWaterTaoist = "";
        public static string TopFireTaoist = "";
        public static string TopPirate = "";

        public static string TopGuildLeader = "";
        public static string TopDeputyLeader = "";
        public static string TopDeputyLeader2 = "";
        public static string TopDeputyLeader3 = "";
        public static string TopDeputyLeader4 = "";
        public static string TopDeputyLeader5 = "";

        public static string MonthlyPKChampion = "";
        public static string WeeklyPKChampion = "";

        public static string TopSpouse = "";
        
        public static void LoadFlags()
        {
           
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("flags");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                TopSpouse = r.ReadString("TopSpouse");
                TopArcher = r.ReadString("TopArcher");
                TopWarrior = r.ReadString("TopWarrior");
                TopNinja = r.ReadString("TopNinja");
                TopWaterTaoist = r.ReadString("TopWaterTaoist");
                TopFireTaoist = r.ReadString("TopFireTaoist");
                TopTrojan = r.ReadString("TopTrojan");
                TopGuildLeader = r.ReadString("TopGuildLeader");
                TopDeputyLeader = r.ReadString("TopDeputyLeader");
                WeeklyPKChampion = r.ReadString("WeeklyPKChampion");
                MonthlyPKChampion = r.ReadString("MonthlyPKChampion");
                TopMonk = r.ReadString("TopMonk");
                TopDeputyLeader2 = r.ReadString("TopDeputyLeader2");
                TopDeputyLeader3 = r.ReadString("TopDeputyLeader3");
                TopDeputyLeader4 = r.ReadString("TopDeputyLeader4");
                TopDeputyLeader5 = r.ReadString("TopDeputyLeader5");
                TopPirate = r.ReadString("TopPirate");

            }
            r.Close();
            r.Dispose();
        }
        public static void AddTopTrojan(Client.GameState client)
        {

            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopTrojan", client.Entity.Name).Execute();
            TopTrojan = client.Entity.Name;
            return;
        }
        public static void AddTopWarrior(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopWarrior", client.Entity.Name).Execute();
            TopWarrior = client.Entity.Name;
            return;
        }
        public static void AddTopArcher(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopArcher", client.Entity.Name).Execute();
            TopArcher = client.Entity.Name;
            return;
        }
        public static void AddTopNinja(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopNinja", client.Entity.Name).Execute();
            TopNinja = client.Entity.Name;

            return;
        }
        public static void AddTopMonk(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopMonk", client.Entity.Name).Execute();
            TopMonk = client.Entity.Name;
            return;
        }
        public static void AddTopWater(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopWaterTaoist", client.Entity.Name).Execute();
            TopWaterTaoist = client.Entity.Name;

            return;
        }
        public static void AddTopFire(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopFireTaoist", client.Entity.Name).Execute();
            TopFireTaoist = client.Entity.Name;
            return;
        }
        public static void AddTopPirate(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopPirate", client.Entity.Name).Execute();
            TopPirate = client.Entity.Name;
            return;
        }
        public static void AddGuildLeader(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopGuildLeader", client.Entity.Name).Execute();
            TopGuildLeader = client.Entity.Name;
            return;
        }
        public static void AddGuildDeaputy(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopDeputyLeader", client.Entity.Name).Execute();
            TopDeputyLeader = client.Entity.Name;
            return;
        }
        public static void AddWeekly(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("WeeklyPKChampion", client.Entity.Name).Execute();
            WeeklyPKChampion = client.Entity.Name;
            return;
        }
        public static void AddMonthly(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("MonthlyPKChampion", client.Entity.Name).Execute();
            MonthlyPKChampion = client.Entity.Name;
            return;
        }
        public static void AddSpouse(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopSpouse", client.Entity.Name).Execute();
            TopSpouse = client.Entity.Name;
            return;
        }
        public static void AddGuildDeaputy2(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopDeputyLeader2", client.Entity.Name).Execute();
            TopDeputyLeader2 = client.Entity.Name;
            return;
        }
        public static void AddGuildDeaputy3(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopDeputyLeader3", client.Entity.Name).Execute();
            TopDeputyLeader3 = client.Entity.Name;
            return;
        }
        public static void AddGuildDeaputy4(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopDeputyLeader4", client.Entity.Name).Execute();
            TopDeputyLeader4 = client.Entity.Name;
            return;
        }
        public static void AddGuildDeaputy5(Client.GameState client)
        {
            new MySqlCommand(MySqlCommandType.UPDATE).Update("flags").Set("TopDeputyLeader5", client.Entity.Name).Execute();
            TopDeputyLeader5 = client.Entity.Name;
            return;
        }
       
    }
}
