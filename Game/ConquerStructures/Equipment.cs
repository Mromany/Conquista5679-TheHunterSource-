using System;
using System.Collections.Generic;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Interfaces;
using KinSocket;

namespace PhoenixProject.Game.ConquerStructures
{
    public class Equipment
    {
        Interfaces.IConquerItem[] objects;
        Client.GameState Owner;
        public Equipment(Client.GameState client)
        {
            Owner = client;
            objects = new Interfaces.IConquerItem[29];
        }

        public void UpdateEntityPacket()
        {
            for (byte Position = 1; Position < 20; Position++)
            {
                if (Free(Position))
                {
                    ClearItemview(Position);
                }
                else
                {
                    var item = TryGetItem(Position);
                    
                    UpdateItemview(item);
                }
            }
            Owner.SendScreen(Owner.Entity.SpawnPacket, false);
        }
        public void UpdateEntityPacket2()
        {
            for (byte Position = 10; Position < 30; Position++)
            {
                if (Free(Position))
                {
                    ClearItemview2(Position);
                }
                else
                {
                    var item = TryGetItem(Position);
                    UpdateItemview2(item);
                }
            }
            Owner.SendScreen(Owner.Entity.SpawnPacket, false);
        }
        public uint GetGear(byte Position, Client.GameState C)
        {
            IConquerItem I = C.Equipment.TryGetItem(Position);
            if (I == null)
            {
                return 0;
            }
            return I.UID;
        }
        public bool Add(Interfaces.IConquerItem item)
        {
            if (objects[item.Position - 1] == null)
            {
                UpdateItemview(item);
                objects[item.Position - 1] = item;
                item.Position = item.Position;
                item.Send(Owner);

                Owner.SendScreenSpawn(Owner.Entity, false);

                return true;
            }
            else return false;
        }
        public bool Add600(Interfaces.IConquerItem item)
        {
            if (objects[item.Position - 1] == null)
            {
                UpdateItemview2(item);
                objects[item.Position - 1] = item;
                item.Position = item.Position;
                item.Send(Owner);

                Owner.LoadItemStats2(Owner.Entity);
                Owner.SendScreenSpawn(Owner.Entity, false);

                return true;
            }
            else return false;
        }
        public bool Add505(Interfaces.IConquerItem item)
        {
            if (objects[item.Position - 1] == null)
            {
                //UpdateItemview2(item);
                objects[item.Position - 1] = item;
                item.Position = item.Position;
                item.Send(Owner);

               // Owner.LoadItemStats(Owner.Entity);
                Owner.SendScreenSpawn(Owner.Entity, false);

                return true;
            }
            else return false;
        }
        public bool Add2(Interfaces.IConquerItem item)
        {
            if (objects[item.Position - 1] == null)
            {
                UpdateItemview(item);
                objects[item.Position  - 1] = item;
                item.Position = item.Position;
                item.Send(Owner);

                Owner.LoadItemStats(Owner.Entity);
                Owner.SendScreenSpawn(Owner.Entity, false);

                return true;
            }
            else return false;
        }
        public bool Add(Interfaces.IConquerItem item, Enums.ItemUse use)
        {
            if (objects[item.Position - 1] == null)
            {
                objects[item.Position - 1] = item;
                item.Mode = Enums.ItemMode.Default;

                if (use != Enums.ItemUse.None)
                {
                    UpdateItemview(item);

                    item.Send(Owner);
                    Owner.LoadItemStats(Owner.Entity);
                }
                return true;
            }
            else return false;
        }


        public void ClearItemview(uint Position)
        {
            switch ((ushort)Position)
            {
                case Network.GamePackets.ConquerItem.Head:
                    Network.Writer.WriteUInt32(0, 194, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 44, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16(0, 139, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.Garment:
                    Network.Writer.WriteUInt32(0, 48, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.Armor:
                    Network.Writer.WriteUInt32(0, 198, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 52, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16(0, 135, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.RightWeapon:
                    Network.Writer.WriteUInt32(0, 206, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 60, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.LeftWeapon:
                    Network.Writer.WriteUInt32(0, 202, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 56, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16(0, 137, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.RightWeaponAccessory:
                    Network.Writer.WriteUInt32(0, 68, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.LeftWeaponAccessory:
                    Network.Writer.WriteUInt32(0, 64, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.Steed:
                    Network.Writer.WriteUInt32(0, 72, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16(0, 145, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 151, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.SteedArmor:
                    if (!Free(Network.GamePackets.ConquerItem.Steed))
                    {
                        var item = TryGetItem(Network.GamePackets.ConquerItem.Steed);
                        Network.Writer.WriteUInt32(item.ID, 76, Owner.Entity.SpawnPacket);
                    }
                    break;
            }


        }

        public void UpdateItemview(Interfaces.IConquerItem item)
        {
            switch ((ushort)item.Position)
            {
                case Network.GamePackets.ConquerItem.Head:
                    if (item.Purification.Available)
                        Network.Writer.WriteUInt32(item.Purification.PurificationItemID, 194, Owner.Entity.SpawnPacket);

                    Network.Writer.WriteUInt32(item.ID, 44, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16((byte)item.Color, 139, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.Garment:
                    Network.Writer.WriteUInt32(item.ID, 48, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.Armor:
                    if (item.Purification.Available)
                        Network.Writer.WriteUInt32(item.Purification.PurificationItemID, 198, Owner.Entity.SpawnPacket);

                    Network.Writer.WriteUInt32(item.ID, 52, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16((byte)item.Color, 135, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.RightWeapon:
                    if (item.Purification.Available)
                        Network.Writer.WriteUInt32(item.Purification.PurificationItemID, 206, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(item.ID, 60, Owner.Entity.SpawnPacket);
                    break;
               
                case Network.GamePackets.ConquerItem.LeftWeapon:
                    if (item.Purification.Available)
                        Network.Writer.WriteUInt32(item.Purification.PurificationItemID, 202, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16((byte)item.Color, 115, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(item.ID, 56, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.LeftWeaponAccessory:
                    Network.Writer.WriteUInt32(item.ID, 64, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.RightWeaponAccessory:
                    Network.Writer.WriteUInt32(item.ID, 68, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.Steed:
                    Network.Writer.WriteUInt32(item.ID, 72, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16((byte)item.Plus, 145, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(item.SocketProgress, 151, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.SteedArmor:
                    Network.Writer.WriteUInt32(item.ID, 76, Owner.Entity.SpawnPacket);
                    break;
            }
        }
        public void UpdateItemview2(Interfaces.IConquerItem item)
        {
            switch ((ushort)item.Position)
            {
                case Network.GamePackets.ConquerItem.AltHead:
                    if (item.Purification.Available)
                        Network.Writer.WriteUInt32(item.Purification.PurificationItemID, 194, Owner.Entity.SpawnPacket);

                    Network.Writer.WriteUInt32(item.ID, 44, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16((byte)item.Color, 139, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.AltGarment:
                    Network.Writer.WriteUInt32(item.ID, 48, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.AltArmor:
                    if (item.Purification.Available)
                        Network.Writer.WriteUInt32(item.Purification.PurificationItemID, 198, Owner.Entity.SpawnPacket);

                    Network.Writer.WriteUInt32(item.ID, 52, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16((byte)item.Color, 135, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.AltRightHand:
                    if (item.Purification.Available)
                        Network.Writer.WriteUInt32(item.Purification.PurificationItemID, 206, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(item.ID, 60, Owner.Entity.SpawnPacket);
                    break;
              
                case Network.GamePackets.ConquerItem.AltLeftHand:
                    if (item.Purification.Available)
                        Network.Writer.WriteUInt32(item.Purification.PurificationItemID, 202, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16((byte)item.Color, 115, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(item.ID, 56, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.LeftWeaponAccessory:
                    Network.Writer.WriteUInt32(item.ID, 64, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.RightWeaponAccessory:
                    Network.Writer.WriteUInt32(item.ID, 68, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.Steed:
                    Network.Writer.WriteUInt32(item.ID, 72, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16((byte)item.Plus, 145, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(item.SocketProgress, 151, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.SteedArmor:
                    Network.Writer.WriteUInt32(item.ID, 76, Owner.Entity.SpawnPacket);
                    break;
            }
        }
        public void ClearItemview2(uint Position)
        {
            switch ((ushort)Position)
            {
                case Network.GamePackets.ConquerItem.AltHead:
                    Network.Writer.WriteUInt32(0, 194, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 44, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16(0, 139, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.AltGarment:
                    Network.Writer.WriteUInt32(0, 48, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.AltArmor:
                    Network.Writer.WriteUInt32(0, 198, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 52, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16(0, 135, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.AltRightHand:
                    Network.Writer.WriteUInt32(0, 206, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 60, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.AltLeftHand:
                    Network.Writer.WriteUInt32(0, 202, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 56, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16(0, 137, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.RightWeaponAccessory:
                    Network.Writer.WriteUInt32(0, 68, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.LeftWeaponAccessory:
                    Network.Writer.WriteUInt32(0, 64, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.Steed:
                    Network.Writer.WriteUInt32(0, 72, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt16(0, 145, Owner.Entity.SpawnPacket);
                    Network.Writer.WriteUInt32(0, 151, Owner.Entity.SpawnPacket);
                    break;
                case Network.GamePackets.ConquerItem.SteedArmor:
                    if (!Free(Network.GamePackets.ConquerItem.Steed))
                    {
                        var item = TryGetItem(Network.GamePackets.ConquerItem.Steed);
                        Network.Writer.WriteUInt32(item.ID, 76, Owner.Entity.SpawnPacket);
                    }
                    break;
            }


        }
        public bool Remove(byte Position)
        {
            if (objects[Position - 1] != null)
            {
                if (Owner.Inventory.Count <= 39)
                {
                    if (Owner.Inventory.Add(objects[Position - 1], Enums.ItemUse.Move))
                    {
                        objects[Position - 1].Position = Position;
                        //Owner.UnloadItemStats(objects[Position - 1], false);
                        objects[Position - 1].Position = 0;
                        if (Position == 12)
                            Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Ride);
                        if (Position == 4)
                            Owner.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Fly);
                        Network.GamePackets.ItemUsage iu = new Network.GamePackets.ItemUsage(true);
                        iu.UID = objects[Position - 1].UID;
                        iu.dwParam = Position;
                        iu.ID = Network.GamePackets.ItemUsage.UnequipItem;
                        Owner.Send(iu);
                        ClearItemview(Position);
                        objects[Position - 1] = null;
                        Owner.SendScreenSpawn(Owner.Entity, false);
                        return true;
                    }
                }
                else
                {
                    Owner.Send(new Network.GamePackets.Message("Not enough room in your inventory.", System.Drawing.Color.Red, Network.GamePackets.Message.TopLeft));
                }
            }
            return false;
        }
        public bool DestroyArrow(uint Position)
        {
            if (objects[Position - 1] != null)
            {
                objects[Position - 1].Position = (ushort)Position;
                if (objects[Position - 1].ID == 0)
                {
                    objects[Position - 1].Position = 0;
                    Database.ConquerItemTable.DeleteItem(objects[Position - 1].UID);
                    objects[Position - 1] = null;
                    return true;
                }
                /*if (!Network.PacketHandler.IsArrow(objects[Position - 1].ID))
                    return false;*/

                //Owner.UnloadItemStats(objects[Position - 1], false);
                Database.ConquerItemTable.DeleteItem(objects[Position - 1].UID);
                Network.GamePackets.ItemUsage iu = new Network.GamePackets.ItemUsage(true);
                iu.UID = objects[Position - 1].UID;
                iu.dwParam = Position;
                iu.ID = Network.GamePackets.ItemUsage.UnequipItem;
                Owner.Send(iu);
                iu.dwParam = 0;
                iu.ID = Network.GamePackets.ItemUsage.RemoveInventory;
                Owner.Send(iu);
                ClearItemview2(Position);
                objects[Position - 1].Position = 0;
                objects[Position - 1] = null;
                return true;
            }
            return false;
        }
        public bool RemoveToGround(uint Position)
        {
            if (Position == 0 || Position > 29)
                return true;
            if (objects[Position - 1] != null)
            {
                objects[Position - 1].Position = (ushort)Position;
                objects[Position - 1].Position = 0;
                Database.ConquerItemTable.RemoveItem2(objects[Position - 1].UID);
                Network.GamePackets.ItemUsage iu = new Network.GamePackets.ItemUsage(true);
                iu.UID = objects[Position - 1].UID;
                iu.dwParam = Position;
                iu.ID = Network.GamePackets.ItemUsage.UnequipItem;
                Owner.Send(iu);
                iu.dwParam = 0;
                iu.ID = Network.GamePackets.ItemUsage.RemoveInventory;
                Owner.Send(iu);

                ClearItemview(Position);
                objects[Position - 1] = null;
                return true;
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
        public byte Count
        {
            get
            {
                byte count = 0; foreach (Interfaces.IConquerItem i in objects)
                    if (i != null)
                        count++; return count;
            }
        }
        public bool Free(byte Position)
        {
            return TryGetItem(Position) == null;
        }
        public bool Free(uint Position)
        {
            return TryGetItem((byte)Position) == null;
        }
        public Interfaces.IConquerItem TryGetItem(byte Position)
        {
            Interfaces.IConquerItem item = null;
            if (Position < 1 || Position > 29)
                return item;
            item = objects[Position - 1];
            return item;
        }
        public Interfaces.IConquerItem TryGetItem(uint uid)
        {
            try
            {
                foreach (Interfaces.IConquerItem item in objects)
                {
                    if (item != null)
                        if (item.UID == uid)
                            return item;
                }
            }
            catch (Exception e)
            {
                Program.SaveException(e);
                Console.WriteLine(e);
            }
            return TryGetItem((byte)uid);
        }

        public bool IsArmorSuper()
        {
            if (TryGetItem(3) != null)
                return TryGetItem(3).ID % 10 == 9;
            return false;
        }
        public bool IsAllSuper()
        {
            for (byte count = 1; count < 12; count++)
            {
                if (count == 5)
                {
                    if (Owner.Entity.Class > 100)
                        continue;
                    if (TryGetItem(count) != null)
                    {
                        if (Network.PacketHandler.IsArrow(TryGetItem(count).ID))
                            continue;
                        if (Network.PacketHandler.IsTwoHand(TryGetItem(4).ID))
                            continue;
                        if (TryGetItem(count).ID % 10 != 9)
                            return false;
                    }
                }
                else
                {
                    if (TryGetItem(count) != null)
                    {
                        if (count != Network.GamePackets.ConquerItem.Bottle && count != Network.GamePackets.ConquerItem.Garment)
                            if (TryGetItem(count).ID % 10 != 9)
                                return false;
                    }
                    else
                        if (count != Network.GamePackets.ConquerItem.Bottle && count != Network.GamePackets.ConquerItem.Garment)
                            return false;
                }
            }
            return true;
        }
    }
}
