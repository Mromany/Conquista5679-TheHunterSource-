using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.ServerBase;
using PhoenixProject.Interfaces;

namespace PhoenixProject.Game.Features.Flowers
{
    public class FlowerSystem
    {
        private FlowerPacket Packet;

        public FlowerSystem(byte[] BasePacket, Client.GameState Caller)
        {
           
            Packet = new FlowerPacket(false);
            Packet.Deserialize(BasePacket);

            if (!Kernel.GamePool.ContainsKey(Packet.UID1)) return;
            if (Caller.Entity.Level < 50) return;
            if (Caller.Entity.Body == 2001 || Caller.Entity.Body == 2002)
            {
                if (Kernel.GamePool[Packet.UID1].Entity.Body == 2001 || Kernel.GamePool[Packet.UID1].Entity.Body == 2002)
                {
                    return;
                }
            }
            if (Caller.Entity.Body == 1003 || Caller.Entity.Body == 1004)
            {
                if (Kernel.GamePool[Packet.UID1].Entity.Body == 1003 || Kernel.GamePool[Packet.UID1].Entity.Body == 1004)
                {
                    return;
                }
            } 
            //if (Kernel.GamePool[Packet.UID1].Entity.Body == 1003 || Kernel.GamePool[Packet.UID1].Entity.Body == 1004) return;

            if (Packet.ItemUID == 0)
            {
                if (Caller.Entity.Flowers.LastFlowerSent == null) Caller.Entity.Flowers.LastFlowerSent = DateTime.Now.Subtract(TimeSpan.FromDays(1));
                //Console.WriteLine(" FlowerType " + Packet.FlowerType + "");
                //Console.WriteLine(" Packet.Amount " + Packet.Amount + "");
                if (Packet.FlowerType != FlowerType.RedRoses && Packet.Amount != 1) return;
                if (Caller.Entity.Flowers.LastFlowerSent.AddDays(1) <= DateTime.Now)
                {
                    if (Caller.Entity.Body == 1003 || Caller.Entity.Body == 1004)
                    {
                        Caller.Entity.Flowers.LastFlowerSent = DateTime.Now;

                        FlowerPacket NewPacket = new FlowerPacket(true);
                        NewPacket.SenderName = Caller.Entity.Name;
                        NewPacket.ReceiverName = Kernel.GamePool[Packet.UID1].Entity.Name;
                        NewPacket.SendAmount = 1;
                        NewPacket.SendFlowerType = FlowerType.RedRoses;
                        Kernel.GamePool[Packet.UID1].Send(NewPacket);
                        Kernel.GamePool[Packet.UID1].Entity.Flowers.RedRoses++;
                        Kernel.GamePool[Packet.UID1].Entity.Flowers.RedRoses2day++;
                    }
                    else
                    {
                        Caller.Entity.Flowers.LastFlowerSent = DateTime.Now;

                        FlowerPacket3 NewPacket = new FlowerPacket3(true);
                        NewPacket.SenderName = Caller.Entity.Name;
                        NewPacket.ReceiverName = Kernel.GamePool[Packet.UID1].Entity.Name;
                        NewPacket.SendAmount = 1;
                        NewPacket.SendFlowerType = FlowerType.Kisses;
                        Kernel.GamePool[Packet.UID1].Send(NewPacket);
                        Kernel.GamePool[Packet.UID1].Entity.Flowers.RedRoses++;
                        Kernel.GamePool[Packet.UID1].Entity.Flowers.RedRoses2day++;
                        Kernel.GamePool[Packet.UID1].Send(new FlowerPacket3(Kernel.GamePool[Packet.UID1]));
                    }
                }
                else
                    Caller.Send(Constants.OneFlowerADay);
            }
            else
            {
                if (Caller.Entity.Body == 1003 || Caller.Entity.Body == 1004)
                {
                    if (Kernel.GamePool[Packet.UID1].Entity.Body == 2001 || Kernel.GamePool[Packet.UID1].Entity.Body == 2002)
                    {
                        IConquerItem Item = null;
                        if (Caller.Inventory.TryGetValue(Packet.ItemUID, out Item))
                        {
                            //if (Item.Durability != Packet.Amount) return;
                            FlowerType Flower = FlowerType.Unknown;
                            switch (Item.ID / 1000)
                            {
                                case 751: Flower = FlowerType.RedRoses; break;
                                case 752: Flower = FlowerType.Lilies; break;
                                case 753: Flower = FlowerType.Orchids; break;
                                case 754: Flower = FlowerType.Tulips; break;
                            }
                            if (Flower != FlowerType.Unknown)
                            {
                                switch (Flower)
                                {
                                    case FlowerType.RedRoses: Kernel.GamePool[Packet.UID1].Entity.Flowers.RedRoses += Item.Durability; Kernel.GamePool[Packet.UID1].Entity.Flowers.RedRoses2day += Item.Durability; break;
                                    case FlowerType.Lilies: Kernel.GamePool[Packet.UID1].Entity.Flowers.Lilies += Item.Durability; Kernel.GamePool[Packet.UID1].Entity.Flowers.Lilies2day += Item.Durability; break;
                                    case FlowerType.Orchids: Kernel.GamePool[Packet.UID1].Entity.Flowers.Orchads += Item.Durability; Kernel.GamePool[Packet.UID1].Entity.Flowers.Orchads2day += Item.Durability; break;
                                    case FlowerType.Tulips: Kernel.GamePool[Packet.UID1].Entity.Flowers.Tulips += Item.Durability; Kernel.GamePool[Packet.UID1].Entity.Flowers.Tulips2day += Item.Durability; break;
                                }

                                FlowerPacket3 NewPacket = new FlowerPacket3(true);
                                NewPacket.SenderName = Caller.Entity.Name;
                                NewPacket.ReceiverName = Kernel.GamePool[Packet.UID1].Entity.Name;
                                NewPacket.SendAmount = Item.Durability;
                                NewPacket.SendFlowerType = Flower;
                                Kernel.GamePool[Packet.UID1].Send(NewPacket);
                                Caller.Inventory.Remove(Item, Enums.ItemUse.Remove);
                                Database.ConquerItemTable.RemoveItem(Item.UID);
                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations, " + Caller.Entity.Name + " has sent " + Item.Durability + "" + Flower + " to " + Kernel.GamePool[Packet.UID1].Entity.Name + " what A Love!)", System.Drawing.Color.Black, Message.TopLeft), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            }
                        }
                    }
                }
                else
                {
                    if (Kernel.GamePool[Packet.UID1].Entity.Body == 1003 || Kernel.GamePool[Packet.UID1].Entity.Body == 1004)
                    {
                        IConquerItem Item = null;
                        if (Caller.Inventory.TryGetValue(Packet.ItemUID, out Item))
                        {
                            //if (Item.Durability != Packet.Amount) return;
                            FlowerType Flower = FlowerType.Unknown;
                            switch (Item.ID / 1000)
                            {
                                case 755: Flower = FlowerType.Kisses; break;
                                case 756: Flower = FlowerType.LoveLetters; break;
                                case 757: Flower = FlowerType.TinOfBeer; break;
                                case 758: Flower = FlowerType.Jades; break;
                            }
                            if (Flower != FlowerType.Unknown)
                            {
                                switch (Flower)
                                {
                                    case FlowerType.Kisses: Kernel.GamePool[Packet.UID1].Entity.Flowers.RedRoses += Item.Durability; Kernel.GamePool[Packet.UID1].Entity.Flowers.RedRoses2day += Item.Durability; break;
                                    case FlowerType.LoveLetters: Kernel.GamePool[Packet.UID1].Entity.Flowers.Lilies += Item.Durability; Kernel.GamePool[Packet.UID1].Entity.Flowers.Lilies2day += Item.Durability; break;
                                    case FlowerType.TinOfBeer: Kernel.GamePool[Packet.UID1].Entity.Flowers.Orchads += Item.Durability; Kernel.GamePool[Packet.UID1].Entity.Flowers.Orchads2day += Item.Durability; break;
                                    case FlowerType.Jades: Kernel.GamePool[Packet.UID1].Entity.Flowers.Tulips += Item.Durability; Kernel.GamePool[Packet.UID1].Entity.Flowers.Tulips2day += Item.Durability; break;
                                }

                                FlowerPacket3 NewPacket = new FlowerPacket3(true);
                                NewPacket.SenderName = Caller.Entity.Name;
                                NewPacket.ReceiverName = Kernel.GamePool[Packet.UID1].Entity.Name;
                                NewPacket.SendAmount = Item.Durability;
                                NewPacket.SendFlowerType = Flower;
                                Kernel.GamePool[Packet.UID1].Send(NewPacket);
                                Caller.Inventory.Remove(Item, Enums.ItemUse.Remove);
                                Database.ConquerItemTable.RemoveItem(Item.UID);
                                Kernel.GamePool[Packet.UID1].Send(new FlowerPacket3(Kernel.GamePool[Packet.UID1]));
                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations, " + Caller.Entity.Name + " has sent " + Item.Durability + "" + Flower + " to " + Kernel.GamePool[Packet.UID1].Entity.Name + " She Loves him ;))", System.Drawing.Color.Black, Message.TopLeft), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            }
                        }
                    }
                }
            }
        }
    }
}