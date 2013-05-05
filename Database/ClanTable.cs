using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Database
{
    public static class Clans
    {
        public static void DeleteClan(uint ID)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("Clans", "ClanID", ID).Execute();
           // Console.WriteLine("6666");
        }
        public static void TransferClan(string name)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd3.Update("entities")
                .Set("ClanRank", 100).Where("Name", name).Execute();
           // Console.WriteLine("7777");
        }
        public static void KickClan(string name)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd3.Update("entities").Set("ClanDonation", 0)
                .Set("ClanRank", 0)
                .Set("ClanId", 0).Where("Name", name).Execute();
        }
        public static void SaveClientDonation(Client.GameState client)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd3.Update("entities").Set("ClanDonation", client.Entity.Myclan.Members[client.Entity.UID].Donation).Where("ClanId", client.Entity.Myclan.ClanId)
                .And("UID", client.Entity.UID).Execute();
           // Console.WriteLine("9999");
        }
        public static void SaveClan(Game.Clans clan)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("Clans").Set("Fund", clan.ClanDonation).Set("Level", clan.ClanLevel)
                .Set("Bulletin", clan.ClanBuletion).Set("Leader", clan.ClanLider).Where("ClanID", clan.ClanId).Execute();
            //Console.WriteLine("1010");
        }
        public static void JoinClan(Client.GameState client)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd3.Update("entities").Set("ClanId", client.Entity.Myclan.ClanId).Set("ClanRank", client.Entity.Myclan.Members[client.Entity.UID].Rank)
                .Set("ClanDonation", client.Entity.Myclan.Members[client.Entity.UID].Donation).Where("UID", client.Entity.UID).Execute();

            Network.GamePackets.Clan cl = new PhoenixProject.Network.GamePackets.Clan(client, 1);
            client.Send(cl.ToArray());
            //Console.WriteLine("111100");

        }
        public static void NextClan()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("clans");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            while (r.Read())
            {
                uint uid = r.ReadUInt32("ClanID");
                if (uid > 0)
                {
                    if (uid > Program.nextClanid)
                        Program.nextClanid = uid;
                }
            }
            r.Close();
            //r.Close();
            r.Dispose();

            Program.nextClanid += 1;
            Console.WriteLine("Last Clan ID: " + Program.nextClanid + "");
        }
        public static void CreateClan(Client.GameState client)
        {
            try
            {
                uint clanid = Program.nextClanid;
                Program.nextClanid++;
                client.Entity.Myclan.ClanId = clanid;
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("Clans").Insert("Name", client.Entity.Myclan.ClanName).Insert("ClanID", clanid)
                    .Insert("Leader", client.Entity.Name).Insert("Fund", 500000).Execute();

                MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd3.Update("entities").Set("ClanId", clanid).Set("ClanRank", "100")
                    .Set("ClanDonation", "500000").Where("UID", client.Entity.UID).Execute();
                client.Entity.ClanRank = 100;
                client.Entity.ClanName = client.Entity.Myclan.ClanName;
                client.Entity.ClanId = clanid;
                Network.GamePackets.Clan cl = new PhoenixProject.Network.GamePackets.Clan(client, 1);
                client.Send(cl.ToArray());
                PhoenixProject.ServerBase.Kernel.ServerClans.Add(clanid, client.Entity.Myclan);
                Network.GamePackets.Clan cls = new PhoenixProject.Network.GamePackets.Clan(client, 1);
                client.Send(cls.ToArray());
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
        }
        public static void LoadAllClans()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("Clans");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            while (r.Read())
            {
                Game.Clans clan = new PhoenixProject.Game.Clans();
                clan.ClanLider = r.ReadString("Leader");
                clan.ClanId = r.ReadUInt32("ClanID");
                clan.ClanName = r.ReadString("Name");
                clan.ClanBuletion = r.ReadString("Bulletin");
                clan.ClanDonation = r.ReadUInt32("Fund");
                clan.ClanLevel = r.ReadByte("Level");
                if (!PhoenixProject.ServerBase.Kernel.ServerClans.ContainsKey(clan.ClanId))
                    PhoenixProject.ServerBase.Kernel.ServerClans.Add(clan.ClanId, clan);
            }
            r.Close();
            //r.Close();
            r.Dispose();

            Console.WriteLine("Clans Loading " + PhoenixProject.ServerBase.Kernel.ServerClans.Count);
            GetMembers();
           // Console.WriteLine("1515");
            foreach (Game.Clans c in ServerBase.Kernel.ServerClans.Values)
            {
                c.LoadAssociates();
            }
        }
        public static void GetMembers()
        {
            foreach (KeyValuePair<uint, Game.Clans> G in PhoenixProject.ServerBase.Kernel.ServerClans)
            {
                Game.Clans clan = G.Value;
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("entities").Where("ClanId", clan.ClanId);
                PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
                while (r.Read())
                {

                    Game.ClanMembers member = new PhoenixProject.Game.ClanMembers();
                    member.Donation = r.ReadUInt32("ClanDonation");
                    //member.Rank = r.ReadByte("ClanRank");
                    member.UID = r.ReadUInt32("UID");
                    member.Name = r.ReadString("Name");
                    member.Class = r.ReadUInt16("Class");
                    member.Level = r.ReadByte("Level");

                    if (clan.ClanLider == member.Name)
                    {
                        member.Rank = 100;
                    }
                    else
                    {
                        member.Rank = 10;
                    }

                    if (!clan.Members.ContainsKey(member.UID))
                        clan.Members.Add(member.UID, member);
                }
                r.Close();
                //r.Close();
                r.Dispose();
                //Console.WriteLine("1818");
            }
        }
    }
}
