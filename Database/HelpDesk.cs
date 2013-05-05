using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Database
{
    class HelpDesk
    {
        public static string Register = "";
        public static string Vote = "";
        public static string ChatBox = "";
        public static string Purchase = "";
        public static string Facebook = "";
        public static string ChangePass = "";
        public static void LoadRates()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("helpdesk");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                Register = r.ReadString("Register");
                Vote = r.ReadString("Vote");
                ChatBox = r.ReadString("ChatBox");
                Purchase = r.ReadString("Purchase");
                Facebook = r.ReadString("Facebook");
                ChangePass = r.ReadString("ChangePass");
                
            }
            Console.WriteLine("HelpDesk Table Loaded.");
            r.Close();
            r.Dispose();
        }
    }
}
