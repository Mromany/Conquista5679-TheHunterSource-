using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Interfaces;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game
{
    public class LotteryPrize
    {
        public static void GeneratePrize( Client.GameState client)
        {
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
                Item.Plus = item.Plus;
                Item.Color = PhoenixProject.Game.Enums.Color.Blue;
                if (item.Sockets > 0)
                    Item.SocketOne = PhoenixProject.Game.Enums.Gem.EmptySocket;
                if (item.Sockets > 1)
                    Item.SocketTwo = PhoenixProject.Game.Enums.Gem.EmptySocket;
                Item.Durability = Item.MaximDurability = Itemd.Durability;
                client.Entity.LotteryPrize = Item;
            }
            else
            {
                goto tryagain;
            }

        }
    }
}
