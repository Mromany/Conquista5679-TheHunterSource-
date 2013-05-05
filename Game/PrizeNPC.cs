using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Database;

namespace PhoenixProject.Game
{
    class PrizeNPC//Coded By Kimo
    {
        public struct PrizeNpcInfo
        {
            public long Owner;
            public uint type;//1 cps , 2 item,
            public uint amount;
            public uint itemid;
        }
        public static SafeDictionary<long, PrizeNpcInfo> PrizeNpcInformations = new SafeDictionary<long, PrizeNpcInfo>(50000);
        public static void Load()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("prizenpc");
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                PrizeNpcInfo info = new PrizeNpcInfo();
                info.Owner = r.ReadUInt32("Owner");
                info.type = r.ReadUInt32("type");
                info.amount = r.ReadUInt32("amount");
                info.itemid = r.ReadUInt32("itemid");
                PrizeNpcInformations.Add(info.Owner, info);                
            }
            Console.WriteLine("PrizeNpc Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void AddCps(Client.GameState client, uint Amount)
        {
            PrizeNpcInfo info = new PrizeNpcInfo();
            info.Owner = client.Entity.UID;
            info.type = 1;
            info.amount = Amount;
            info.itemid = 0;
            PrizeNpcInformations.Add(info.Owner, info); 
        }
        public static void AddItem(Client.GameState client, uint itemid)
        {
            PrizeNpcInfo info = new PrizeNpcInfo();
            info.Owner = client.Entity.UID;
            info.type = 2;
            info.amount = 0;
            info.itemid = itemid;
            PrizeNpcInformations.Add(info.Owner, info); 
        }
        public static void RemoveCps(Client.GameState client)
        {
            MySqlCommand command = new MySqlCommand(MySqlCommandType.DELETE);
            command.Delete("prizenpc", "Owner", client.Entity.UID).And("type", "1").Execute();

            PrizeNpcInformations.Remove(client.Entity.UID);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("prizenpc").Where("Owner", client.Entity.UID);
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                PrizeNpcInfo info = new PrizeNpcInfo();
                info.Owner = r.ReadUInt32("Owner");
                info.type = r.ReadUInt32("type");
                info.amount = r.ReadUInt32("amount");
                info.itemid = r.ReadUInt32("itemid");
                PrizeNpcInformations.Add(info.Owner, info);
            }
            //Console.WriteLine("PrizeNpc Loaded.");
            r.Close();
            r.Dispose();
        }
        public static void RemoveItem(Client.GameState client)
        {
            MySqlCommand command = new MySqlCommand(MySqlCommandType.DELETE);
            command.Delete("prizenpc", "Owner", client.Entity.UID).And("type", "2").Execute();

            PrizeNpcInformations.Remove(client.Entity.UID);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("prizenpc").Where("Owner", client.Entity.UID);
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                PrizeNpcInfo info = new PrizeNpcInfo();
                info.Owner = r.ReadUInt32("Owner");
                info.type = r.ReadUInt32("type");
                info.amount = r.ReadUInt32("amount");
                info.itemid = r.ReadUInt32("itemid");
                PrizeNpcInformations.Add(info.Owner, info);
            }
            //Console.WriteLine("PrizeNpc Loaded.");
            r.Close();
            r.Dispose();
        }
    }
}
