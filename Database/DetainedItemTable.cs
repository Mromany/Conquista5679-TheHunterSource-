using System;
using System.IO;
using PhoenixProject.Network.GamePackets;


namespace PhoenixProject.Database
{
    public class DetainedItemTable
    {
        public static ServerBase.Counter Counter = new PhoenixProject.ServerBase.Counter(1);
        public static void LoadDetainedItems(Client.GameState client)
        {

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("detaineditems").Where("OwnerUID", client.Entity.UID);
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                DetainedItem item = new DetainedItem(true);
                item.ItemUID = r.ReadUInt32("ItemUID");
                item.UID = item.ItemUID ;
               // Console.WriteLine("S " + item.UID + "");
                item.Item = ConquerItemTable.LoadItem(item.ItemUID);
                item.ConquerPointsCost = r.ReadUInt32("ConquerPointsCost");
                item.OwnerUID = r.ReadUInt32("OwnerUID");
                item.OwnerName = r.ReadString("OwnerName");
                item.GainerUID = r.ReadUInt32("GainerUID");
                item.GainerName = r.ReadString("GainerName");
                item.Date = DateTime.FromBinary(r.ReadInt64("Date"));
                item.DaysLeft = (uint)(TimeSpan.FromTicks(DateTime.Now.Ticks).Days - TimeSpan.FromTicks(item.Date.Ticks).Days);
                if (DateTime.Now < item.Date.AddDays(7))
                    client.DeatinedItem.Add(item.UID, item);
                else
                    if (item.Bound)
                        Claim(item.UID, client);
            }
            r.Close();

           /* DetaineditemCollection items = new DetaineditemCollection();
            items.LoadAndCloseReader(Detaineditem.FetchByParameter("OwnerUID", client.Entity.UID));
            for (int x = 0; x < items.Count; x++)
            {
                DetainedItem item = new DetainedItem(true);
                item.ItemUID = items[x].ItemUID;
                item.UID = item.ItemUID - 1;
                item.Item = ConquerItemTable.LoadItem(item.ItemUID);
                item.ConquerPointsCost = items[x].ConquerPointsCost;
                item.OwnerUID = items[x].OwnerUID;
                item.GainerName = items[x].OwnerName;
                item.GainerUID = items[x].GainerUID;
                item.OwnerName = items[x].GainerName;
                item.Date = DateTime.FromBinary((long)items[x].DateX);
                item.DaysLeft = (uint)(TimeSpan.FromTicks(DateTime.Now.Ticks).Days - TimeSpan.FromTicks(item.Date.Ticks).Days);
                if (DateTime.Now < item.Date.AddDays(7))
                    client.DeatinedItem.Add(item.UID, item);
                else
                    if (item.Bound)
                        Claim(item, client);
            }*/
        }
   
        public static void DetainItem(Interfaces.IConquerItem item, Client.GameState owner, Client.GameState gainer)
        {
            DetainedItem Item = new DetainedItem(true);
            Item.ItemUID = item.UID;
            Item.Item = item;
            Item.UID = Item.ItemUID ;
            Item.ConquerPointsCost = CalculateCost(item);
            Item.OwnerUID = owner.Entity.UID;
            Item.OwnerName = owner.Entity.Name;
            Item.GainerUID = gainer.Entity.UID;
            Item.GainerName = gainer.Entity.Name;
            Item.Date = DateTime.Now;
            Item.DaysLeft = 0;
            owner.DeatinedItem.Add(Item.UID, Item);
            owner.Send(Item);

            DetainedItem Item2 = new DetainedItem(true);
            Item2.ItemUID = item.UID;
            Item2.UID = Item2.ItemUID ;
            Item2.Item = item;
            Item2.Page = (byte)DetainedItem.ClaimPage;
            Item2.ConquerPointsCost = CalculateCost(item);
            Item.OwnerUID = owner.Entity.UID;
            Item.OwnerName = owner.Entity.Name;
            Item.GainerUID = gainer.Entity.UID;
            Item.GainerName = gainer.Entity.Name;
            Item2.Date = Item.Date;
            Item2.DaysLeft = 0;
            gainer.ClaimableItem.Add(Item2.UID, Item2);
            gainer.Send(Item2);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT)
               .Insert("detaineditems").Insert("ItemUID", item.UID).Insert("ConquerPointsCost", Item.ConquerPointsCost)
               .Insert("Date", Item.Date.Ticks).Insert("OwnerUID", owner.Entity.UID).Insert("OwnerName", owner.Entity.Name)
               .Insert("GainerUID", gainer.Entity.UID).Insert("GainerName", gainer.Entity.Name);
            cmd.Execute();


            MySqlCommand cmd2 = new MySqlCommand(MySqlCommandType.INSERT)
              .Insert("claimitems").Insert("ItemUID", item.UID).Insert("ConquerPointsCost", Item.ConquerPointsCost)
              .Insert("Date", Item.Date.Ticks).Insert("OwnerUID", owner.Entity.UID).Insert("OwnerName", owner.Entity.Name)
              .Insert("GainerUID", gainer.Entity.UID).Insert("GainerName", gainer.Entity.Name);
            cmd2.Execute();

            /*Detaineditem.Insert(item.UID, (ulong)Item.Date.Ticks, Item.ConquerPointsCost, 
            owner.Entity.UID, owner.Entity.Name, gainer.Entity.UID, gainer.Entity.Name);

            ClaimItem.Insert(item.UID, (ulong)Item.Date.Ticks, Item.ConquerPointsCost,
            owner.Entity.UID, owner.Entity.Name, gainer.Entity.UID, gainer.Entity.Name);*/
        }

        public static void Redeem(DetainedItem item, Client.GameState owner)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE)
                .Update("detaineditems").Set("OwnerUID", 500).Where("ItemUID", item.UID);
            cmd.Execute();
        }

        public static void Claim(uint kimo, Client.GameState owner)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE).Delete("detaineditems", "ItemUID", kimo);
            cmd.Execute();
        }

        public static uint CalculateCost(Interfaces.IConquerItem item)
        {
            int basic = 10;
            if (item.ID % 10 == 9)
                basic += 50;
            if (item.ID / 100000 == 4 || item.ID / 100000 == 5)
            {
                if (item.SocketOne != PhoenixProject.Game.Enums.Gem.NoSocket)
                    basic += 100;
                if (item.SocketTwo != PhoenixProject.Game.Enums.Gem.NoSocket)
                    basic += 400;
            }
            else
            {
                if (item.SocketOne != PhoenixProject.Game.Enums.Gem.NoSocket)
                    basic += 400;
                if (item.SocketTwo != PhoenixProject.Game.Enums.Gem.NoSocket)
                    basic += 1600;
            }
            basic += item.Plus * 100;
            return (uint)basic;
        }
    }
}
