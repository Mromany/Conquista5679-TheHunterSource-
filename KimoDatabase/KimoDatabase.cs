using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket.Database;

namespace PhoenixProject.KimoDatabase
{
    public class Database : MySqlEngine
    {
        public Database()
            : base("localhost", Program.DBName, Program.DBUser, Program.DBPass, true, "5", "50")
        {
        }


    }
}
