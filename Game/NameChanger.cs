using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Database;

namespace PhoenixProject.Game
{
    class NameChanger//Coded By Kimo
    {
        public static void KimoUpdateName(Client.GameState client)
        {
            
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("entities");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            String name = "";
            while (r.Read())
            {
                //newname = r.ReadString("namechange");//debug make
                name = r.ReadString("name");
                if (name != "")
                {
                    MySqlCommand cmdupdate = null;//lol i see the problem hold on ,,, hold on what? :$ try now
                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("apprentice").Set("MentorName", client.Entity.NewName).Where("MentorName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("apprentice").Set("ApprenticeName", client.Entity.NewName).Where("ApprenticeName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("arena").Set("EntityName", client.Entity.NewName).Where("EntityName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("claimitems").Set("OwnerName", client.Entity.NewName).Where("OwnerName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("claimitems").Set("GainerName", client.Entity.NewName).Where("GainerName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("detaineditems").Set("OwnerName", client.Entity.NewName).Where("OwnerName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("detaineditems").Set("GainerName", client.Entity.NewName).Where("GainerName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("enemy").Set("EnemyName", client.Entity.NewName).Where("EnemyName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("friends").Set("FriendName", client.Entity.NewName).Where("FriendName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("guilds").Set("Name", client.Entity.NewName).Where("Name", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("guilds").Set("LeaderName", client.Entity.NewName).Where("LeaderName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("nobility").Set("EntityName", client.Entity.NewName).Where("EntityName", name).Execute();

                    cmdupdate = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmdupdate.Update("partners").Set("PartnerName", client.Entity.NewName).Where("PartnerName", name).Execute();
                    UpdateStaff(client);
                    return;
                }
            }
            r.Close();
            r.Dispose();
        }
        public static void UpdateStaff(Client.GameState Kimo)
        {
            Game.ConquerStructures.Nobility.Board.Clear();
            Database.NobilityTable.Load();
            //////////
            Kimo.ClaimableItem.Clear();
            PhoenixProject.Database.ClaimItemTable.LoadClaimableItems(Kimo);
            Kimo.DeatinedItem.Clear();
            PhoenixProject.Database.DetainedItemTable.LoadDetainedItems(Kimo);

            Kimo.Partners.Clear();
            Kimo.Enemy.Clear();
            Kimo.Friends.Clear();
            Kimo.Apprentices.Clear();

            PhoenixProject.Database.KnownPersons.LoadEnemy(Kimo);
            PhoenixProject.Database.KnownPersons.LoaderFriends(Kimo);
            PhoenixProject.Database.KnownPersons.LoadMentor(Kimo);
            PhoenixProject.Database.KnownPersons.LoadPartner(Kimo);

            foreach (Game.ConquerStructures.Society.TradePartner clients in Kimo.Partners.Values)
            {
                if (clients.IsOnline)
                {
                    clients.Client.Partners.Clear();
                    PhoenixProject.Database.KnownPersons.LoadPartner(clients.Client);
                }
            }
            foreach (Game.ConquerStructures.Society.Enemy clients in Kimo.Enemy.Values)
            {
                if (clients.IsOnline)
                {
                    clients.Client.Enemy.Clear();
                    PhoenixProject.Database.KnownPersons.LoadEnemy(clients.Client);
                }
            }
            foreach (Game.ConquerStructures.Society.Friend clients in Kimo.Friends.Values)
            {
                if (clients.IsOnline)
                {
                    clients.Client.Friends.Clear();
                    PhoenixProject.Database.KnownPersons.LoaderFriends(clients.Client);
                }
            }
            foreach (Game.ConquerStructures.Society.Apprentice clients in Kimo.Apprentices.Values)
            {
                if (clients.IsOnline)
                {
                    clients.Client.Apprentices.Clear();
                    PhoenixProject.Database.KnownPersons.LoaderFriends(clients.Client);
                }
            }


           //Console.WriteLine("Warning! : "+Kimo.Entity.Name+" changed his name");
           return;
        }
    }
}
