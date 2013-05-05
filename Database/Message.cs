using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Database
{
    class Messagess
    {
        public static string Sys = "";
        public static string Sys2 = "";
        public static string Sys3 = "";
        public static string Sys4 = "";
        public static string Sys5 = "";
        public static string Sys6 = "";
        public static string Sys7 = "";
        public static string Sys8 = "";
        public static string Sys9 = "";

        public static string SteedRace = "";
        public static string CouplesPk = "";
        public static string DailyPk = "";
        public static string DisEnd = "";
        public static string GuildFlame = "";
        public static string ClassPk = "";
        public static string EliteGW2 = "";



        public static void LoadRates()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("systemmessage");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                EliteGW2 = r.ReadString("EliteGW2");
                ClassPk = r.ReadString("ClassPk");
                GuildFlame = r.ReadString("GuildFlame");
                DisEnd = r.ReadString("DisEnd");
                DailyPk = r.ReadString("DailyPk");
                CouplesPk = r.ReadString("CouplesPk");
                SteedRace = r.ReadString("SteedRace");

                Sys = r.ReadString("Sys");
                Sys2 = r.ReadString("Sys2");
                Sys3 = r.ReadString("Sys3");
                Sys4 = r.ReadString("Sys4");
                Sys5 = r.ReadString("Sys5");
                Sys6 = r.ReadString("Sys6");
                Sys7 = r.ReadString("Sys7");
                Sys8 = r.ReadString("Sys8");
                Sys9 = r.ReadString("Sys9");


                
            }
            Console.WriteLine("System Messages  Loaded.");
            r.Close();
            r.Dispose();
        }
    }
}
