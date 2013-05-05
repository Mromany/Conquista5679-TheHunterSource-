using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Database;

namespace PhoenixProject.Game
{
    class KimoEvents
    {
        public static uint SpouseHour = 0;
        public static uint SpouseMinute = 0;
        public static string SpouseDay = "";
        public static uint SpouseEndHour = 0;

        public static uint ClassHour = 0;
        public static uint ClassMinute = 0;
        public static string ClassDay = "";
        public static uint ClassEndHour = 0;

        public static uint EGHour = 0;
        public static uint EGMinute = 0;
        public static string EGDay = "";
        public static uint EGEndHour = 0;

        public static uint ClanHour = 0;
        public static uint ClanMinute = 0;
        public static string ClanDay = "";
        public static uint ClanEndHour = 0;

        public static uint DWHour = 0;
        public static uint DWMinute = 0;
        public static string DWDay = "";
        public static uint DWEndHour = 0;

        public static uint WHour = 0;
        public static uint WMinute = 0;
        public static string WDay = "";
        public static uint WEndHour = 0;

        public static uint THour = 0;
        public static uint TMinute = 0;
        public static string TDay = "";
        public static uint TEndHour = 0;

        public static uint EBHour = 0;
        public static uint EBMinute = 0;
        public static string EBDay = "";
        public static uint EBEndHour = 0;

        public static uint SKHour = 0;
        public static uint SKMinute = 0;
        public static string SKDay = "";
        public static uint SKEndHour = 0;


        public static uint DCHour = 0;
        public static uint DCMinute = 0;
        public static string DCDay = "";
        public static uint DCEndHour = 0;

        public static uint DisHour = 0;
        public static uint DisMinute = 0;
        public static string DisDay = "";
        public static uint DisEndHour = 0;

        public static uint GWSHour = 0;
        public static uint GWSMinute = 0;
        public static string GWSDay = "";
        public static uint GWSEndHour = 0;

        public static uint GWEHour = 0;
        public static uint GWEMinute = 0;
        public static string GWEDay = "";
        public static uint GWEEndHour = 0;

        public static uint DemonHour = 0;
        public static uint DemonMinute = 0;
        public static string DemonDay = "";
        public static uint DemonEndHour = 0;

        public static uint CFHour = 0;
        public static uint CFMinute = 0;
        public static string CFDay = "";
        public static uint CFEndHour = 0;//LordsWar

        public static uint LordsWarHour = 0;
        public static uint LordsWarMinute = 0;
        public static string LordsWarDay = "";
        public static uint LordsWarEndHour = 0;//LordsWar

        public static void CaptureTeamTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "Flag");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                CFHour = r.ReadUInt32("Hour");
                CFMinute = r.ReadUInt32("Minute");
                CFDay = r.ReadString("Day");
                CFEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("CaptureTeamTime Time Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void DemonCaveTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "DemonCave");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                DemonHour = r.ReadUInt32("Hour");
                DemonMinute = r.ReadUInt32("Minute");
                DemonDay = r.ReadString("Day");
                DemonEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("DemonCaveTime Time Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void GWEndTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "GWEnd");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                GWEHour = r.ReadUInt32("Hour");
                GWEMinute = r.ReadUInt32("Minute");
                GWEDay = r.ReadString("Day");
                GWEEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("GWEndTime Time Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void GWstartTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "GWStart");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                GWSHour = r.ReadUInt32("Hour");
                GWSMinute = r.ReadUInt32("Minute");
                GWSDay = r.ReadString("Day");
                GWSEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("GWstartTime Time Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void DisCityTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "DisCity");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                DisHour = r.ReadUInt32("Hour");
                DisMinute = r.ReadUInt32("Minute");
                DisDay = r.ReadString("Day");
                DisEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("DisCityTime Time Loaded.");
            r.Close();
            r.Dispose();
        }

       
        public static void SkillTeamTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "SkillTeam");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                SKHour = r.ReadUInt32("Hour");
                SKMinute = r.ReadUInt32("Minute");
                SKDay = r.ReadString("Day");
                SKEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("SkillTeamTime Time Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void ElitePKTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "ElitePk");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                EBHour = r.ReadUInt32("Hour");
                EBMinute = r.ReadUInt32("Minute");
                EBDay = r.ReadString("Day");
                EBEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("ElitePK Time Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void LordsWarTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "LordsWar");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                LordsWarHour = r.ReadUInt32("Hour");
                LordsWarMinute = r.ReadUInt32("Minute");
                LordsWarDay = r.ReadString("Day");
                LordsWarEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("LordsWar Time Loaded.");
            r.Close();
            r.Dispose();
        }
       /* public static void TreasureTime()
        {
            //TreasureBox
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "TreasureBox");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                THour = r.ReadUInt32("Hour");
                TMinute = r.ReadUInt32("Minute");
                TDay = r.ReadString("Day");
                TEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("TreasureBox Time Loaded.");
            r.Close();
            r.Dispose();
        }*/
       
        public static void WeeklyTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "WeeklyPk");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                WHour = r.ReadUInt32("Hour");
                WMinute = r.ReadUInt32("Minute");
                WDay = r.ReadString("Day");
                WEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("WeeklyWarTime Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void DonationWarTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "DonationWar");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                DWHour = r.ReadUInt32("Hour");
                DWMinute = r.ReadUInt32("Minute");
                DWDay = r.ReadString("Day");
                DWEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("DonationWarTime Loaded.");
            r.Close();
            r.Dispose();
        }

        public static void ClanWarTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "ClanWar");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                ClanHour = r.ReadUInt32("Hour");
                ClanMinute = r.ReadUInt32("Minute");
                ClanDay = r.ReadString("Day");
                ClanEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("ClanWarTime Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void EliteGWTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "EliteGW");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                EGHour = r.ReadUInt32("Hour");
                EGMinute = r.ReadUInt32("Minute");
                EGDay = r.ReadString("Day");
                EGEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("EliteGW Time Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void SpouseTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "TopSpouse");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                SpouseHour = r.ReadUInt32("Hour");
                SpouseMinute = r.ReadUInt32("Minute");
                SpouseDay = r.ReadString("Day");
                SpouseEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("SpouseTime Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void ClassTime()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("KimoTimes").Where("Type", "ClassPk");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                ClassHour = r.ReadUInt32("Hour");
                ClassMinute = r.ReadUInt32("Minute");
                ClassDay = r.ReadString("Day");
                ClassEndHour = r.ReadUInt32("End");
            }
            Console.WriteLine("ClassTime Loaded.");
            r.Close();
            r.Dispose();
        }
    }
}
