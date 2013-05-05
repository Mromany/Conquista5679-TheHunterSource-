using System;
using System.IO;
using System.Text;
using PhoenixProject.ServerBase;


namespace PhoenixProject.Database
{
    public class AccountTable
    {
        public enum AccountState : byte
        {
            NotActivated = 100,
            ProjectManager = 4,
            GameHelper = 5,
            GameMaster = 3,
            Player = 2,
            Banned = 1,
            Aimbot = 50,
            BadWords = 60,
            Spam = 70,
            Cheat = 80,
            Coder = 6,
            DoesntExist = 0
        }
        public string Username;
        public string Password;
        public string Email;
        public string IP;
        public string OldIP;
        public DateTime LastCheck;
        public AccountState State;
        public uint EntityID;
        public uint TempID;
        public bool exists = false;
        public AccountTable(string username)
        {
            this.Username = username;
            this.Password = "";
            this.IP = "";
            this.LastCheck = DateTime.Now;
            this.State = AccountState.DoesntExist;
            this.EntityID = 0;
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("accounts").Where("Username", username);
            MySqlReader r = new MySqlReader(cmd);
            if (r.Read())
            {
                exists = true;
                this.Password = r.ReadString("Password");
                this.IP = r.ReadString("IP");
                this.EntityID = r.ReadUInt32("EntityID");
                this.LastCheck = DateTime.FromBinary(r.ReadInt64("LastCheck"));
                this.State = (AccountState)r.ReadByte("State");
                this.Email = r.ReadString("Email");
            }
            r.Close();
            r.Dispose();
        }
        public void Save()
         {
             if (exists)
             {
                 MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                 cmd.Update("accounts").Set("Password", Password).Set("IP", IP).Set("EntityID", EntityID).Set("LastCheck", (ulong)DateTime.Now.ToBinary()).Where("Username", Username).Execute();
             }
             else
             {
                 try
                 {
                     MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                     cmd.Insert("accounts").Insert("Username", Username).Insert("Password", Password).Insert("State", (byte)State).Execute();
                 }
                 catch (Exception e) { Program.SaveException(e); }
             }
         }
        public void Savekimo()
        {
            if (exists)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd.Update("accounts").Set("State", 80).Where("Username", Username).Execute();
            }
            else
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                    cmd.Insert("accounts").Insert("Username", Username).Insert("Password", Password).Insert("State", (byte)State).Execute();
                }
                catch (Exception e) { Program.SaveException(e); }
            }
        }
    }
}
