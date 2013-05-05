using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Database
{
    class GameUpdatess
    {
        public static string Header = "";
        public static string Body1 = "";
        public static string Body2 = "";
        public static string Body3 = "";
        public static string Body4 = "";
        public static string Body5 = "";
        public static string Body6 = "";
        



        public static void LoadRates()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("gameupdates");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                Body1 = r.ReadString("Body1");
                Body2 = r.ReadString("Body2");
                Body3 = r.ReadString("Body3");
                Body4 = r.ReadString("Body4");
                Body5 = r.ReadString("Body5");
                Body6 = r.ReadString("Body6");
                Header = r.ReadString("Header");


            }
            Console.WriteLine("System GameUpdates  Loaded.");
            r.Close();
            r.Dispose();
        }
    }
}
