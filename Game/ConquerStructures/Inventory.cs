using System;
using System.Linq;
using System.Collections.Generic;

namespace PhoenixProject.Game.ConquerStructures
{
    public class Inventory
    {
        Dictionary<uint, Interfaces.IConquerItem> inventory;
        Interfaces.IConquerItem[] objects;
        Client.GameState Owner;
        public Inventory(Client.GameState client)
        {
            Owner = client;
            inventory = new Dictionary<uint, Interfaces.IConquerItem>(40);
            objects = new Interfaces.IConquerItem[0];
        }
        public bool Add(uint id, byte plus, byte times)
        {
            try {
            Database.ConquerItemInformation infos = new Database.ConquerItemInformation(id, plus);
            while (times > 0)
            {
                if (Count <= 39)
                {
                    Interfaces.IConquerItem item = new Network.GamePackets.ConquerItem(true);
                    item.ID = id;
                    item.Plus = plus;
                    item.Durability = item.MaximDurability = infos.BaseInformation.Durability;
                    Add(item, Enums.ItemUse.CreateAndAdd);
                }
                else
                {
                    return false;
                }
                times--;
            }
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
            return true;
        }
        public bool AddandWear(uint id, byte plus, byte times, Client.GameState client)
        {
            try
            {
                Database.ConquerItemInformation infos = new Database.ConquerItemInformation(id, plus);
                while (times > 0)
                {
                    if (Count <= 39)
                    {
                        Interfaces.IConquerItem item = new Network.GamePackets.ConquerItem(true);
                        item.ID = id;
                        item.Plus = plus;
                        item.Color = PhoenixProject.Game.Enums.Color.Red;
                        item.Durability = item.MaximDurability = infos.BaseInformation.Durability;
                        Add(item, Enums.ItemUse.CreateAndAdd);
                        client.Inventory.Remove(item, Game.Enums.ItemUse.Move, true);
                        PhoenixProject.Network.PacketHandler.Positions pos = PhoenixProject.Network.PacketHandler.GetPositionFromID(item.ID);
                        item.Position = (byte)pos;
                        client.Equipment.Add(item);
                        item.Mode = Game.Enums.ItemMode.Update;
                        item.Send(client);
                        client.CalculateStatBonus();
                        client.CalculateHPBonus();
                        client.LoadItemStats(client.Entity);
                        PhoenixProject.Network.GamePackets.ClientEquip equips = new PhoenixProject.Network.GamePackets.ClientEquip();
                        equips.DoEquips(client);
                        client.Send(equips);
                        Database.ConquerItemTable.UpdateLocation(item, client);
                    }
                    else
                    {
                        return false;
                    }
                    times--;
                }
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
            return true;
        }
        public bool Add35(uint id, byte plus, byte times)
        {
            Database.ConquerItemInformation infos = new Database.ConquerItemInformation(id, plus);
            while (times > 0)
            {
                if (Count <= 39)
                {
                    Interfaces.IConquerItem item = new Network.GamePackets.ConquerItem(true);
                    item.ID = id;
                    item.Plus = 12;
                    item.Enchant = 245;
                    item.Bless = 7;

                    item.SocketOne = Game.Enums.Gem.SuperDragonGem;
                    item.SocketTwo = Game.Enums.Gem.SuperDragonGem;
                    item.Durability = item.MaximDurability = infos.BaseInformation.Durability;
                    Add(item, Enums.ItemUse.CreateAndAdd);
                }
                else
                {
                    return false;
                }
                times--;
            }
            return true;
        }
        public bool Add(uint id, Game.Enums.ItemEffect effect)
        {
            try {
            Interfaces.IConquerItem item = new Network.GamePackets.ConquerItem(true);
            item.ID = id;
            item.Effect = effect;
            Database.ConquerItemInformation infos = new Database.ConquerItemInformation(id, 0);
            item.Durability = item.MaximDurability = infos.BaseInformation.Durability;
            if (Count <= 39)
            {
                Add(item, Enums.ItemUse.CreateAndAdd);
            }
            else
            {
                return false;
            }
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
            return true;
        }

        public bool Add(Interfaces.IConquerItem item, Enums.ItemUse use)
        {try {
            if (!Database.ConquerItemInformation.BaseInformations.ContainsKey(item.ID))
                return true;
            if (Count == 40)
            {
                Owner.Send(ServerBase.Constants.FullInventory);
                return false;
            }
            if (!inventory.ContainsKey(item.UID))
            {
                item.Position = 0;
                Interfaces.IConquerItem _ExistingItem;
                Database.ConquerItemInformation iteminfo = new PhoenixProject.Database.ConquerItemInformation(item.ID, 0);
                if (Owner.Inventory.Contains(iteminfo.BaseInformation.ID, iteminfo.BaseInformation.StackSize, out _ExistingItem) && Owner.SpiltStack && use !=Enums.ItemUse.None)
                {

                    if (_ExistingItem.StackSize == 0)
                        _ExistingItem.StackSize = 1;
                    ushort _StackCount = iteminfo.BaseInformation.StackSize;
                    _StackCount -= (ushort)_ExistingItem.StackSize;

                    if (_StackCount >= 1)
                        _StackCount += 1;
                    _ExistingItem.StackSize += 1;

                    Database.ConquerItemTable.UpdateStack(_ExistingItem);
                    _ExistingItem.Mode = Game.Enums.ItemMode.Update;
                    _ExistingItem.Send(Owner);
                    _ExistingItem.Mode = Game.Enums.ItemMode.Default;
                    switch (use)
                    {

                        case Enums.ItemUse.Add:
                            Database.ConquerItemTable.DeleteItem(item.UID);
                            break;
                        case Enums.ItemUse.Move:
                            Database.ConquerItemTable.DeleteItem(item.UID);
                            break;
                    }
                    // Owner.SpiltStack = false;
                    return true;

                }
                else
                {
                    switch (use)
                    {
                        case Enums.ItemUse.CreateAndAdd:
                            item.UID = PhoenixProject.Client.AuthState.nextID;
                            PhoenixProject.Client.AuthState.nextID++;
                            Database.ConquerItemTable.AddItem(ref item, Owner);
                            item.MobDropped = false;
                            break;
                        case Enums.ItemUse.Add:
                            Database.ConquerItemTable.UpdateLocation(item, Owner);
                            break;
                        case Enums.ItemUse.Move:
                            item.Position = 0;
                            item.StatsLoaded = false;
                            Database.ConquerItemTable.UpdateLocation(item, Owner);
                            break;
                    }
                    inventory.Add(item.UID, item);
                    objects = inventory.Values.ToArray();
                    item.Mode = Enums.ItemMode.Default;
                    if (use != Enums.ItemUse.None)
                        item.Send(Owner);
                    return true;
                }
            }
        }
        catch (Exception e)
        {
            Program.SaveException(e);
        }
            return false;
        }
        public void Update()
        {
            objects = inventory.Values.ToArray();
        }
        public bool Remove(Interfaces.IConquerItem item, Enums.ItemUse use)
        {
            try
            {
                if (inventory.ContainsKey(item.UID))
                {
                    if (Owner.Entity.UseItem && item.StackSize > 1)
                    {
                        Remove(item.ID, 1);
                        Owner.Entity.UseItem = false;
                        return true;
                    }
                    else
                    {
                        switch (use)
                        {
                            case Enums.ItemUse.Remove: Database.ConquerItemTable.DeleteItem(item.UID); break;
                            case Enums.ItemUse.Delete: Database.ConquerItemTable.DeleteItem(item.UID); break;
                            case Enums.ItemUse.Move: Database.ConquerItemTable.UpdateLocation(item, Owner); break;
                        }
                        //Database.ItemLog.LogItem(item.UID, Owner.Entity.UID, PhoenixProject.Database.ItemLog.ItemLogAction.Remove);

                        inventory.Remove(item.UID);
                        objects = inventory.Values.ToArray();
                        Network.GamePackets.ItemUsage iu = new Network.GamePackets.ItemUsage(true);
                        iu.UID = item.UID;
                        iu.ID = Network.GamePackets.ItemUsage.RemoveInventory;
                        Owner.Send(iu);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
            return false;
        }
        public bool Remove(Interfaces.IConquerItem item, Enums.ItemUse use, bool equipment)
        {
            try
            {
                if (inventory.ContainsKey(item.UID))
                {
                    //  Database.ItemLog.LogItem(item.UID, Owner.Entity.UID, PhoenixProject.Database.ItemLog.ItemLogAction.Remove);
                    if (inventory.ContainsKey(item.UID))
                        inventory.Remove(item.UID);
                    else
                        return false;
                    if (objects == null)
                        objects = new Interfaces.IConquerItem[objects.Length];
                    if (inventory.Values.Count >= 0)
                        objects = inventory.Values.ToArray();
                    else
                        return false;
                    Network.GamePackets.ItemUsage iu = new Network.GamePackets.ItemUsage(true);
                    iu.UID = item.UID;
                    iu.ID = Network.GamePackets.ItemUsage.RemoveInventory;
                    Owner.Send(iu);
                    return true;
                }
            }
            catch (Exception e)
            {
                Program.SaveException(e);

            }
            return false;
        }
        public bool Remove(uint UID, Enums.ItemUse use, bool sendRemove)
        {
            try {
            if (inventory.ContainsKey(UID))
            {
                switch (use)
                {
                    case Enums.ItemUse.Remove: Database.ConquerItemTable.RemoveItem(UID); break;
                    case Enums.ItemUse.Move: Database.ConquerItemTable.UpdateLocation(inventory[UID], Owner); break;
                }
                Database.ItemLog.LogItem(UID, Owner.Entity.UID, PhoenixProject.Database.ItemLog.ItemLogAction.Remove);
                inventory.Remove(UID);
                objects = inventory.Values.ToArray();
                if (sendRemove)
                {
                    Network.GamePackets.ItemUsage iu = new Network.GamePackets.ItemUsage(true);
                    iu.UID = UID;
                    iu.ID = Network.GamePackets.ItemUsage.RemoveInventory;
                    Owner.Send(iu);
                }
                return true;
            }
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
            return false;
        }
        public bool Remove(string name)
        {
            try {
            foreach (var item in inventory.Values)
            {
                if (Database.ConquerItemInformation.BaseInformations[item.ID].Name.ToLower() == name.ToLower())
                {
                   // Database.ItemLog.LogItem(item.UID, Owner.Entity.UID, PhoenixProject.Database.ItemLog.ItemLogAction.Remove);
                    Remove(item, Enums.ItemUse.Remove);
                    Network.GamePackets.ItemUsage iu = new Network.GamePackets.ItemUsage(true);
                    iu.UID = item.UID;
                    iu.ID = Network.GamePackets.ItemUsage.RemoveInventory;
                    Owner.Send(iu);
                    return true;
                }
            }
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
            return false;
        }
        public Interfaces.IConquerItem[] Objects
        {
            get
            {
                return objects;
            }
        }
        public byte Count { get { return (byte)Objects.Length; } }

        public bool TryGetItem(uint UID, out Interfaces.IConquerItem item)
        {
            return inventory.TryGetValue(UID, out item);
        }

        public bool ContainsUID(uint UID)
        {
            return inventory.ContainsKey(UID);
        }
        public bool Contains(uint ID, ushort maxstackamount, out Interfaces.IConquerItem Item)
        {
            Item = null;
            if (ID == 0)
                return false;
            foreach (Interfaces.IConquerItem item in Objects)
            {
                if (item.ID == ID && item.StackSize < maxstackamount)
                {
                    Item = item;
                    return true;
                }
            }
            return false;
        }
        public bool Contains(uint ID, ushort times)
        {
            if (ID == 0)
                return true;
            ushort has = 0;
            foreach (Interfaces.IConquerItem item in Objects)
                if (item.ID == ID)
                {
                    if (item.StackSize == 0)
                        has++;
                    else
                        has += (byte)item.StackSize;
                }
            return has >= times;
        }
        public Interfaces.IConquerItem GetItemByID(uint ID)
        {
            foreach (Interfaces.IConquerItem item in Objects)
                if (item.ID == ID)
                    return item;
            return null;
        }
        public bool Remove(uint ID, byte times)
        {
            if (ID == 0)
                return true;
            List<Interfaces.IConquerItem> items = new List<Interfaces.IConquerItem>();
            byte has = 0;
            foreach (Interfaces.IConquerItem item in Objects)
                if (item.ID == ID)
                {
                    if (item.StackSize > 1)
                    {
                        //Console.WriteLine(" " + item.StackSize + "");
                        if (item.StackSize >= times)
                        {
                            item.StackSize-= times;
                            if (item.StackSize != 0)
                            {
                                Database.ConquerItemTable.UpdateStack(item);
                                item.Mode = Enums.ItemMode.Update;
                                item.Send(Owner);
                                item.Mode = Enums.ItemMode.Default;
                                return true;
                            }
                            else
                            {
                                Database.ConquerItemTable.DeleteItem(item.UID);
                                inventory.Remove(item.UID);
                                objects = inventory.Values.ToArray();
                                Network.GamePackets.ItemUsage iu = new Network.GamePackets.ItemUsage(true);
                                iu.UID = item.UID;
                                iu.ID = Network.GamePackets.ItemUsage.RemoveInventory;
                                Owner.Send(iu);
                                return true;
                            }
                        }
                        else
                        {
                            has += (byte)item.StackSize; items.Add(item); if (has >= times) break;
                        }
                    }
                    else
                    {
                        has++; items.Add(item); if (has >= times) break;
                    }
                }
            if (has >= times)
                foreach (Interfaces.IConquerItem item in items)
                    Remove(item, Enums.ItemUse.Remove);
            return has >= times;
        }

        public bool TryGetValue(uint UID, out PhoenixProject.Interfaces.IConquerItem Info)
        {
            Info = null;
            lock (inventory)
            {
                if (inventory.ContainsKey(UID))
                { return inventory.TryGetValue(UID, out Info); }
                else
                    return false;
            }
        }
    }
}
