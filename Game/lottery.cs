using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Interfaces;

namespace PhoenixProject.Game
{
    public class Lottery
    {
       
        public static void LotteryRewardMessage(Client.GameState Hero, Interfaces.IConquerItem i)
        {
            string str = "";
            string str2 = "";
            string str3 = "";
            if ((Game.Enums.ItemQuality)(i.ID % 10) >= PhoenixProject.Game.Enums.ItemQuality.Elite)
            {
                str = "Elite ";
                if ((Game.Enums.ItemQuality)(i.ID % 10) >= PhoenixProject.Game.Enums.ItemQuality.Super)
                {
                    str = "Super ";
                }
                if (Database.ConquerItemInformation.BaseInformations[i.ID].Name.Contains("MoneyBag"))
                {
                    str = "";
                }
            }
            if (i.SocketOne > 0)
            {
                str2 = "1-Socket ";
                if (i.SocketTwo > 0)
                {
                    str2 = "2-Socket ";
                }
            }
            if (i.Plus > 0)
            {
                str3 = "(+" + i.Plus + ")";
            }
            string str4 = str + str2 + Database.ConquerItemInformation.BaseInformations[i.ID].Name + str3;
            string msg = string.Format("{0} won a {1} from the Lottery!", Hero.Entity.Name, str4);

            if ((Game.Enums.ItemQuality)(i.ID % 10) >= PhoenixProject.Game.Enums.ItemQuality.Elite)
            {
                ServerBase.Kernel.SendWorldMessage(new Message(msg, System.Drawing.Color.Red, Network.GamePackets.Message.Talk), ServerBase.Kernel.GamePool.Values);
               
            }
            else
            {
                Hero.Send(new Message(msg, System.Drawing.Color.White, Message.Talk));
                
            }
        }
        public static void GiveLotteryPrize(Client.GameState kimo)
        {
            LotteryRewardMessage(kimo, kimo.Entity.LotteryPrize);
            Database.ConquerItemInformation Itemd = new Database.ConquerItemInformation(kimo.Entity.LotteryItemID, 0);
            //var Itemd = PhoenixProject.Database.ConquerItemInformation.BaseInformations[kimo.Entity.LotteryItemID];
            IConquerItem Item = new ConquerItem(true);
            Item.ID = kimo.Entity.LotteryItemID;
            
            Item.Plus = (byte)kimo.Entity.LotteryItemPlus;
            Item.Color = PhoenixProject.Game.Enums.Color.Blue;
            if (kimo.Entity.LotteryItemSoc1 > 0)
            {
                Item.SocketOne = PhoenixProject.Game.Enums.Gem.EmptySocket;
                kimo.Entity.LotteryItemSoc1 = 0;
            }
            if (kimo.Entity.LotteryItemSoc2 > 0)
            {
                Item.SocketTwo = PhoenixProject.Game.Enums.Gem.EmptySocket;
                kimo.Entity.LotteryItemSoc2 = 0;
            }
            Item.Durability = Item.MaximDurability = Itemd.BaseInformation.Durability;
           
            kimo.Inventory.Add(Item, Game.Enums.ItemUse.CreateAndAdd);
            
           
            kimo.Entity.LotteryItemID = 0;
            kimo.Entity.LotteryJadeAdd = 0;
        }
        public static void LuckyBox(uint npcID, Client.GameState h, bool jade = false)
        {
            if (h.LotteryEntries < 10 || jade)
            {
                if (jade || h.Inventory.Contains(0xadb50, 3))
                {
                    if (!jade)
                    {
                        h.LotteryEntries++;
                       
                    }
                    else
                    {
                        h.Entity.LotteryJadeAdd++;
                    }
                    if ((npcID != 0) && !jade)
                    {
                        _String packet = new _String(true);
                        packet.UID = npcID;
                        packet.TextsCount = 1;
                        packet.Type = _String.Effect;
                        packet.Texts.Add("lottery");
                        h.Send(packet);
                        if (h.Entity.LotteryItemID > 0)
                        {
                            //GiveLotteryPrize(h);
                        }
                    }
                tryagain:
                    int rand = PhoenixProject.ServerBase.Kernel.Random.Next(PhoenixProject.Database.LotteryTable.LotteryItems.Count);
                    var item = PhoenixProject.Database.LotteryTable.LotteryItems[rand];
                    var Itemd = PhoenixProject.Database.ConquerItemInformation.BaseInformations[item.ID];
                    if (Itemd == null)
                        goto tryagain;
                    if (PhoenixProject.ServerBase.Kernel.Rate(item.Rank, item.Chance) && PhoenixProject.ServerBase.Kernel.Rate(item.Rank, 35 - item.Rank))
                    {
                        IConquerItem Item = new ConquerItem(true);
                        Item.ID = item.ID;
                        h.Entity.LotteryItemID = item.ID;
                        h.Entity.LotteryItemPlus = item.Plus;
                        h.Entity.LotteryItemColor = (byte)PhoenixProject.Game.Enums.Color.Blue;
                        Item.Plus = item.Plus;
                        Item.Color = PhoenixProject.Game.Enums.Color.Blue;
                        if (item.Sockets > 0)
                        {
                            Item.SocketOne = PhoenixProject.Game.Enums.Gem.EmptySocket;
                            h.Entity.LotteryItemSoc1 = 255;
                        }
                        if (item.Sockets > 1)
                        {
                            Item.SocketTwo = PhoenixProject.Game.Enums.Gem.EmptySocket;
                            h.Entity.LotteryItemSoc2 = 255;
                           
                        }
                        Item.Durability = Item.MaximDurability = Itemd.Durability;
                       // h.Inventory.Add(Item, Game.Enums.ItemUse.Add);
                        //h.Entity.LotteryPrize = Item;
                        if ((Item != null))
                        {
                            if (!jade)
                            {
                                if (h.Inventory.Contains(0xadb50, 3))
                                {

                                    h.Inventory.Remove(0xadb50, 3);
                                }
                                else
                                {
                                    return;
                                }
                            }
                            else
                            {
                                if (h.Inventory.Contains(0xadb50, 1))
                                {

                                    h.Inventory.Remove(0xadb50, 1);
                                }
                                else
                                {
                                    return;
                                }
                            }

                            h.Entity.LotteryPrize = Item;
                            PhoenixProject.Network.GamePackets.Lottery lottery = new PhoenixProject.Network.GamePackets.Lottery
                            {
                                Chances = (byte)(10 - h.LotteryEntries),
                                Color = Item.Color,
                                Plus = Item.Plus,
                                Prize = Item.ID,
                                SocketOne = (byte)Item.SocketOne,
                                SocketTwo = (byte)Item.SocketTwo,
                                AddJadeChances = h.Entity.LotteryJadeAdd,
                                Type = PhoenixProject.Network.GamePackets.Lottery.LotteryTypes.ShowGUI
                            };
                            h.Send((byte[])lottery);

                        }
                        else
                        {
                            // string msg = string.Format("Error generating lottery prize.", h.Entity.Name, h.Entity.Name);
                            // h.Send(new Message(msg, System.Drawing.Color.White, Message.Talk));

                        }
                    }
                    else
                    {
                        goto tryagain;
                    }

                    
                }
                else
                {
                    string msg = string.Format("You need 3 Small Lottery Tickets to try at the lottery!", h.Entity.Name, h.Entity.Name);
                    h.Send(new Message(msg, System.Drawing.Color.White, Message.Talk));
                   
                }
            }
            else
            {
                string msg = string.Format("You have used up all your lottery attempts today! But if you have a LotteryTicket you can exchange it for another try from Lady Luck!", h.Entity.Name, h.Entity.Name);
                h.Send(new Message(msg, System.Drawing.Color.White, Message.Talk));
               
            }
        }
        public static void Handle(byte[] Data, Client.GameState Client)
        {
            
            PhoenixProject.Network.GamePackets.Lottery lottery = new PhoenixProject.Network.GamePackets.Lottery(Data);
            if (Client != null)
            {
                switch (lottery.Type)
                {
                    case PhoenixProject.Network.GamePackets.Lottery.LotteryTypes.Accept:
                        GiveLotteryPrize(Client);
                        break;

                    case PhoenixProject.Network.GamePackets.Lottery.LotteryTypes.AddJade:
                        if ((Client.Entity.LotteryJadeAdd < 2) && Client.Inventory.Contains(0xadb50, 1))
                        {
                            LuckyBox(0, Client, true);
                        }
                        break;

                    case PhoenixProject.Network.GamePackets.Lottery.LotteryTypes.Continue:
                        LuckyBox(0, Client, false);
                        break;
                }
            }
        }
    }
}
