using System;
using System.IO;
using PhoenixProject.Network.GamePackets;


namespace PhoenixProject.Database
{
    public class ClaimItemTable
    {
        public static ServerBase.Counter Counter = new PhoenixProject.ServerBase.Counter(1);
        public static void LoadClaimableItems(Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("claimitems").Where("GainerUID", client.Entity.UID);
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                DetainedItem item = new DetainedItem(true);
                item.ItemUID = r.ReadUInt32("ItemUID");
                item.UID = item.ItemUID;
                item.Page = (byte)DetainedItem.ClaimPage;
                item.Item = ConquerItemTable.LoadItem(item.ItemUID);
                item.ConquerPointsCost = r.ReadUInt32("ConquerPointsCost");
                item.OwnerUID = r.ReadUInt32("OwnerUID");
                item.GainerName = r.ReadString("GainerName");
                item.GainerUID = r.ReadUInt32("GainerUID");
                item.OwnerName = r.ReadString("OwnerName");
                item.Date = DateTime.FromBinary(r.ReadInt64("Date"));
                item.DaysLeft = (uint)(TimeSpan.FromTicks(DateTime.Now.Ticks).Days - TimeSpan.FromTicks(item.Date.Ticks).Days);
                if (item.OwnerUID == 500)
                {
                    item.MakeItReadyToClaim();
                    item.GainerUID = r.ReadUInt32("GainerUID");
                    item.OwnerUID = r.ReadUInt32("OwnerUID");
                }
                client.ClaimableItem.Add(item.UID, item);
            }
            r.Close();
            r.Dispose();

            /*ClaimItemCollection items = new ClaimItemCollection();
            items.LoadAndCloseReader(ClaimItem.FetchByParameter("GainerUID", client.Entity.UID));
            for (int x = 0; x < items.Count; x++)
            {
                DetainedItem item = new DetainedItem(true);
                item.ItemUID = items[x].ItemUID;
                item.UID = item.ItemUID - 1;
                item.Page = (byte)DetainedItem.ClaimPage;
                item.Item = ConquerItemTable.LoadItem(item.ItemUID);
                item.ConquerPointsCost = items[x].ConquerPointsCost;
                item.OwnerUID = items[x].OwnerUID;
                item.GainerName = items[x].GainerName;
                item.GainerUID = items[x].GainerUID;
                item.OwnerName = items[x].OwnerName;
                item.Date = DateTime.FromBinary((long)items[x].DateX);
                item.DaysLeft = (uint)(TimeSpan.FromTicks(DateTime.Now.Ticks).Days - TimeSpan.FromTicks(item.Date.Ticks).Days);
                if (item.OwnerUID == 500)
                {
                    item.MakeItReadyToClaim();
                    item.GainerUID = items[x].GainerUID;
                    item.OwnerUID = items[x].OwnerUID;
                }
                client.ClaimableItem.Add(item.UID, item);
            }*/
        }

      

        public static void Redeem(uint kimo, Client.GameState owner)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE)
                .Update("claimitems").Set("OwnerUID", 500).Where("ItemUID", kimo);
            cmd.Execute();
        }

        public static void Claim(uint kimo, Client.GameState owner)
        {
            MySqlCommand command = new MySqlCommand(MySqlCommandType.DELETE);
           
            command.Delete("claimitems", "ItemUID", kimo).And("GainerUID", owner.Entity.UID).Execute();
            //ClaimItem.Delete(item.Item.UID);
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

