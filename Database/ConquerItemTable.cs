using System;
using System.IO;

using System.Linq;
using System.Text;
namespace PhoenixProject.Database
{
    public class ConquerItemTable
    {
        public static Interfaces.IConquerItem GetSingleItem(uint UID)
        {
            
            Interfaces.IConquerItem item = new Network.GamePackets.ConquerItem(true);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("items").Where("UID", UID);
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                item.ID = r.ReadUInt32("ID");
                item.UID = r.ReadUInt32("UID");
                item.Durability = r.ReadUInt16("Durability");
                item.MaximDurability = r.ReadUInt16("MaximDurability");
                item.Position = r.ReadUInt16("Position");
                item.SocketProgress = r.ReadUInt32("SocketProgress");
                item.PlusProgress = r.ReadUInt32("PlusProgress");
                item.SocketOne = (Game.Enums.Gem)r.ReadByte("SocketOne");
                item.SocketTwo = (Game.Enums.Gem)r.ReadByte("SocketTwo");
                item.Effect = (Game.Enums.ItemEffect)r.ReadByte("Effect");
                item.Mode = Game.Enums.ItemMode.Default;
                item.Plus = r.ReadByte("Plus");
                item.Bless = r.ReadByte("Bless");
                item.Bound = r.ReadBoolean("Bound");
                item.Enchant = r.ReadByte("Enchant");
                item.Lock = r.ReadByte("Locked");
                item.UnlockEnd = DateTime.FromBinary(r.ReadInt64("UnlockEnd"));
                item.Suspicious = r.ReadBoolean("Suspicious");
                
                item.SuspiciousStart = DateTime.FromBinary(r.ReadInt64("SuspiciousStart"));
                item.Color = (Game.Enums.Color)r.ReadByte("Color");
                item.Warehouse = r.ReadUInt16("Warehouse");
                if (item.Lock == 2)
                    if (DateTime.Now >= item.UnlockEnd)
                        item.Lock = 0;
            }
            r.Close();
            //r.Close();
            r.Dispose();

            return item;
        }
        public static void UpdateInscre3(uint UID)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Inscribed", 0).Where("EntityID", UID).And("Inscribed", 1).Execute();
        }
        public static void LoadItems(Client.GameState client)
        {
            client.SpiltStack = true;
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("items").Where("EntityID", client.Entity.UID);
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                Interfaces.IConquerItem item = new Network.GamePackets.ConquerItem(true);
                item.ID = r.ReadUInt32("ID");
                item.UID = r.ReadUInt32("UID");

                item.Durability = r.ReadUInt16("Durability");
                if (item.ID == 750000)
                {
                    item.MaximDurability = r.ReadUInt16("MaximDurability");
                    client.Entity.kilid = item.MaximDurability;
                    //item.Durability = item.MaximDurability;
                }
                else
                {
                    item.MaximDurability = r.ReadUInt16("MaximDurability");
                    item.Durability = item.MaximDurability;
                }
                item.Position = r.ReadUInt16("Position");
                item.SocketProgress = r.ReadUInt32("SocketProgress");
                item.PlusProgress = r.ReadUInt32("PlusProgress");
                item.SocketOne = (Game.Enums.Gem)r.ReadByte("SocketOne");
                item.SocketTwo = (Game.Enums.Gem)r.ReadByte("SocketTwo");
                item.Effect = (Game.Enums.ItemEffect)r.ReadByte("Effect");
                item.Mode = Game.Enums.ItemMode.Default;
                item.Plus = r.ReadByte("Plus");
                item.Bless = r.ReadByte("Bless");
                item.Bound = r.ReadBoolean("Bound");
                item.Enchant = r.ReadByte("Enchant");
                item.Lock = r.ReadByte("Locked");
                item.UnlockEnd = DateTime.FromBinary(r.ReadInt64("UnlockEnd"));
                item.Suspicious = r.ReadBoolean("Suspicious");
                item.SuspiciousStart = DateTime.FromBinary(r.ReadInt64("SuspiciousStart"));
                item.Color = (Game.Enums.Color)r.ReadByte("Color");
                item.Warehouse = r.ReadUInt16("Warehouse");

                item.Inscribed = (r.ReadByte("Inscribed") == 1 ? true : false);
               
                item.StackSize = r.ReadUInt16("StackSize");
                item.MaxStackSize = r.ReadUInt16("MaxStackSize");
                item.RefineItem = r.ReadUInt32("RefineryItem");
                if (item.ID == 730001)
                {
                    item.Plus = 1;
                }
                if (item.ID == 730002)
                {
                    item.Plus = 2;
                }
                if (item.ID == 730003)
                {
                    item.Plus = 3;
                }
                if (item.ID == 730004)
                {
                    item.Plus = 4;
                }
                if (item.ID == 730005)
                {
                    item.Plus = 5;
                }
                if (item.ID == 730006)
                {
                    item.Plus = 6;
                }
                if (item.ID == 730007)
                {
                    item.Plus = 7;
                }
                if (item.ID == 730008)
                {
                    item.Plus = 8;
                }
                if (item.ID == 730009)
                {
                    item.Plus = 9;
                }
                /*if (item.ID == 0x493e0)
                {
                    byte rr = 0;
                    byte g = 0;
                    byte b = 0;
                    rr = (byte)item.kimo1;
                    g = (byte)item.kimo2;
                    b = (byte)item.kimo3;
                    item.Color = (PhoenixProject.Game.Enums.Color)b;
                    item.Bless = g;
                    item.Unknown40 = rr;

                }*/
                ulong rTime = r.ReadUInt64("RefineryTime");
               
                if (item.RefineItem > 0 && rTime != 0)
                {
                    item.RefineryTime = ServerBase.Kernel.FromDateTimeInt(rTime);
                    if (DateTime.Now > item.RefineryTime)
                    {
                        item.RefineryTime = new DateTime(0);
                        item.RefineItem = 0;
                        Database.ConquerItemTable.UpdateRefineryItem(item);
                        Database.ConquerItemTable.UpdateRefineryTime(item);
                    }
                }

                if (item.Lock == 2)
                    if (DateTime.Now >= item.UnlockEnd)
                        item.Lock = 0;
                ItemAddingTable.GetAddingsForItem(item);
                if (item.Warehouse == 0)
                {
                    switch (item.Position)
                    {
                        case 0: client.Inventory.Add(item, Game.Enums.ItemUse.None); break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                            if (client.Equipment.Free((byte)item.Position))
                            {

                                client.Equipment.Add(item, Game.Enums.ItemUse.None);
                            }
                            else
                            {

                                if (client.Inventory.Count < 40)
                                {
                                    item.Position = 0;
                                    client.Inventory.Add(item, Game.Enums.ItemUse.None);
                                    if (client.Warehouses[PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.StoneCity].Count < 60)
                                        client.Warehouses[PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.StoneCity].Add(item);
                                    UpdatePosition(item);
                                }
                            }
                            break;
                    }
                }
                else
                {
                    client.Warehouses[(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID)item.Warehouse].Add(item);
                }


                if (item.ID == 720828)
                {
                    string agate = r.ReadString("agate");
                    uint count = 0;
                    string[] maps = agate.Split('#');
                    foreach (string one in maps)
                    {
                        if (one.Length > 6)
                        {
                            item.Agate_map.Add(count, one);
                            count++;
                        }
                    }
                }
                 if (item.Inscribed)
                {
                    if (client.Guild == null)
                    {
                        item.Inscribed = false;
                        UpdateInscre1(item);
                    }

                }
               
            }
            //client.SpiltStack = true;
            r.Close();
            r.Dispose();
        }
        public static Interfaces.IConquerItem LoadItem(uint UID)
        {
            Interfaces.IConquerItem item = new Network.GamePackets.ConquerItem(true);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("items").Where("UID", UID);
            MySqlReader r = new MySqlReader(cmd);
            if (r.Read())
            {
                item.ID = r.ReadUInt32("ID");
                item.UID = r.ReadUInt32("UID");
                item.Durability = r.ReadUInt16("Durability");
                item.MaximDurability = r.ReadUInt16("MaximDurability");
                item.Position = r.ReadUInt16("Position");
                item.SocketProgress = r.ReadUInt32("SocketProgress");
                item.PlusProgress = r.ReadUInt32("PlusProgress");
                item.SocketOne = (Game.Enums.Gem)r.ReadByte("SocketOne");
                item.SocketTwo = (Game.Enums.Gem)r.ReadByte("SocketTwo");
                item.Effect = (Game.Enums.ItemEffect)r.ReadByte("Effect");
                item.Mode = Game.Enums.ItemMode.Default;
                item.Plus = r.ReadByte("Plus");
                item.Bless = r.ReadByte("Bless");
                item.Bound = r.ReadBoolean("Bound");
                item.Enchant = r.ReadByte("Enchant");
                item.Lock = r.ReadByte("Locked");
                item.UnlockEnd = DateTime.FromBinary(r.ReadInt64("UnlockEnd"));
                item.Suspicious = r.ReadBoolean("Suspicious");
                item.SuspiciousStart = DateTime.FromBinary(r.ReadInt64("SuspiciousStart"));
                item.Color = (Game.Enums.Color)r.ReadByte("Color");
                item.Inscribed = (r.ReadByte("Inscribed") == 1 ? true : false);
                item.StackSize = r.ReadUInt16("StackSize");
                item.Warehouse = r.ReadUInt16("Warehouse");
                if (item.Lock == 2)
                    if (DateTime.Now >= item.UnlockEnd)
                        item.Lock = 0;
                ItemAddingTable.GetAddingsForItem(item);
            }
            r.Close();
            r.Dispose();
            return item;
        }
        public static void AddItem(ref Interfaces.IConquerItem Item, Client.GameState client)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("items").Insert("EntityID", client.Entity.UID).Insert("UID", Item.UID)
                    .Insert("ID", Item.ID).Insert("Plus", Item.Plus).Insert("Bless", Item.Bless)
                    .Insert("Enchant", Item.Enchant).Insert("SocketOne", (byte)Item.SocketOne)
                    .Insert("SocketTwo", (byte)Item.SocketTwo).Insert("Durability", Item.Durability)
                    .Insert("MaximDurability", Item.MaximDurability).Insert("SocketProgress", Item.SocketProgress)
                    .Insert("PlusProgress", Item.PlusProgress).Insert("Effect", (ushort)Item.Effect)
                    .Insert("Bound", Item.Bound).Insert("Locked", Item.Lock).Insert("Suspicious", Item.Suspicious)
                    .Insert("Color", (uint)Item.Color).Insert("Position", Item.Position).Insert("Warehouse", Item.Warehouse)
                    .Insert("UnlockEnd", Item.UnlockEnd.ToBinary()).Insert("SuspiciousStart", Item.SuspiciousStart.ToBinary())
                    .Insert("StackSize", Item.StackSize);
                cmd.Execute();
                new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastItem", PhoenixProject.Client.AuthState.nextID).Where("Coder", "kimo").Execute();
            }
            catch
            {
            again:
                PhoenixProject.Client.AuthState.nextID++; 
                Item.UID = PhoenixProject.Client.AuthState.nextID;
                if (IsThere(Item.UID))
                    goto again;
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("items").Insert("EntityID", client.Entity.UID).Insert("UID", Item.UID)
                    .Insert("ID", Item.ID).Insert("Plus", Item.Plus).Insert("Bless", Item.Bless)
                    .Insert("Enchant", Item.Enchant).Insert("SocketOne", (byte)Item.SocketOne)
                    .Insert("SocketTwo", (byte)Item.SocketTwo).Insert("Durability", Item.Durability)
                    .Insert("MaximDurability", Item.MaximDurability).Insert("SocketProgress", Item.SocketProgress)
                    .Insert("PlusProgress", Item.PlusProgress).Insert("Effect", (ushort)Item.Effect)
                    .Insert("Bound", Item.Bound).Insert("Locked", Item.Lock).Insert("Suspicious", Item.Suspicious)
                    .Insert("Color", (uint)Item.Color).Insert("Position", Item.Position).Insert("Warehouse", Item.Warehouse)
                    .Insert("UnlockEnd", Item.UnlockEnd.ToBinary()).Insert("SuspiciousStart", Item.SuspiciousStart.ToBinary())
                    .Insert("StackSize", Item.StackSize);
                cmd.Execute();
                new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastItem", PhoenixProject.Client.AuthState.nextID).Where("Coder", "kimo").Execute();
            }
        }
        public static void PonerDurabilidad(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Durability", Item.Durability).Where("UID", Item.UID).Execute();
        }
        public static bool IsThere(uint uid)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("items").Where("UID", uid);
            var r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                r.Close();
                r.Dispose();

                return true;
            }
            r.Close();
            //r.Close();
            r.Dispose();

            return false;
        }
       
        public static void UpdateBless(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Bless", Item.Bless).Where("UID", Item.UID).Execute();
        }
        public static void UpdateEnchant(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Enchant", Item.Enchant).Where("UID", Item.UID).Execute();
        }
        public static void UpdateItemAgate(Interfaces.IConquerItem Item)
        {
            string agate = "";
            if (Item.ID == 720828)
            {
                foreach (string coord in Item.Agate_map.Values)
                {
                    agate += coord + "#";
                    MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
                    cmd.Update("items").Set("agate", agate).Where("UID", Item.UID).Execute();
                   // UpdateData(Item, "agate", agate);
                }
            }
        }
        public static void UpdateColor(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
           cmd.Update("items").Set("Color", (uint)Item.Color).Where("UID", Item.UID).Execute();
        }
        public static void UpdateStack(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("StackSize", Item.StackSize).Where("UID", Item.UID).Execute();
        }
        public static void UpdateEnchant(Interfaces.IConquerItem Item, Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Enchant", Item.Enchant).Where("UID", Item.UID).Execute();
        }
        public static void UpdateLock(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Locked", Item.Lock).Set("UnlockEnd", Item.UnlockEnd.ToBinary()).Where("UID", Item.UID).Execute();
        }
        public static void UpdateSockets(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("SocketOne", (byte)Item.SocketOne)
                .Set("SocketTwo", (byte)Item.SocketTwo).Where("UID", Item.UID).Execute();
        }
        public static void UpdateSocketProgress(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("SocketProgress", Item.SocketProgress).Where("UID", Item.UID).Execute();
        }
        public static void UpdateRefineryItem(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("RefineryItem", Item.RefineItem).Where("UID", Item.UID).Execute();
            //UpdateData(Item, "RefineryItem", Item.RefineItem);
        }
        public static void UpdateRefineryTime(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("RefineryTime", ServerBase.Kernel.ToDateTimeInt(Item.RefineryTime)).Where("UID", Item.UID).Execute();
            //UpdateData(Item, "RefineryTime", ServerBase.Kernel.ToDateTimeInt(Item.RefineryTime));
        }
        public static void UpdateDurabilityItem(Interfaces.IConquerItem Item)
        {
        }
        public static void UpdateLocation(Interfaces.IConquerItem Item, Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("EntityID", client.Entity.UID).Set("Position", Item.Position).Set("Warehouse", Item.Warehouse).Where("UID", Item.UID).Execute();
        }
        public static void UpdatePosition(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Position", Item.Position).Set("Warehouse", Item.Warehouse).Where("UID", Item.UID).Execute();
        }
        public static void UpdateInscre1(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Inscribed", true).Where("UID", Item.UID).Execute();
        }
        public static void UpdateInscre2(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Inscribed", false).Where("UID", Item.UID).Execute();
        }
        /*public static void deleteallguildins(Client.GameState client)
        {
            ItemCollection items = new ItemCollection();
            items.LoadAndCloseReader(Conquer.Database.Item.FetchByParameter("EntityID", client.Entity.UID));
            for (int x = 0; x < items.Count; x++)
            {
                if (items[x].Inscribed == true)
                {
                    items[x].Inscribed = false;
                    items[x].Save();
                }
            }
            PtArsenalInscribed.Delete("uid", client.Entity.UID);
        }*/
        public static void UpdatePlus(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Plus", Item.Plus).Where("UID", Item.UID).Execute();
        }
        public static void UpdateBound(Interfaces.IConquerItem Item)
        {
             MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Bound", 0).Where("UID", Item.UID).Execute();
        }
        public static void UpdatePlusProgress(Interfaces.IConquerItem Item)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("PlusProgress", Item.PlusProgress).Where("UID", Item.UID).Execute();
        }
        public static void UpdateItemID(Interfaces.IConquerItem Item, Client.GameState client)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("ID", Item.ID).Where("UID", Item.UID).Execute();
        }
        public static void RemoveItem(uint UID)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("EntityID", 0).Set("Position", 0).Where("UID", UID).Execute();
        }
        public static void RemoveItem2(uint UID)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("EntityID", 1).Set("Position", 0).Where("UID", UID).Execute();
        }
        public static void DeleteItem(uint UID)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
            cmd.Delete("items", "UID", UID).Execute();
        }
        public static void ClearPosition(uint EntityID, byte position)
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd.Update("items").Set("Position", 0).Where("EntityID", EntityID).And("Position", position).Execute();
        }
        public static void RefineryUpdate(Interfaces.IConquerItem Item, Client.GameState client)
        {
        }
    }
}
