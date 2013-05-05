using System;
using System.Collections.Generic;
using System.IO;

namespace PhoenixProject.Database
{
    public static class ShopFile
    {
        public static Dictionary<uint, Shop> Shops;
        public class Shop
        {
            public uint UID;
            public MoneyType MoneyType;
            public int Count { get { return Items.Count; } }
            public List<uint> Items;
            public Dictionary<UInt32, HonorShopItem> HonorShopItems;
            public Dictionary<UInt32, SteedShop> SteedShops;
            public class HonorShopItem
            {
                public uint cost;
                public uint itemid;
            }
            public class SteedShop
            {
                public uint cost;
                public uint itemid;
            }
        }
        public static void LoadRaceShop()
        {
            String[] text = File.ReadAllLines("database\\raceshop.ini");
            Shop shop = new Shop();
            shop.UID = 6001;
            shop.MoneyType = MoneyType.RaceShop;
            shop.SteedShops = new Dictionary<uint, Shop.SteedShop>();
            for (int x = 0; x < text.Length; x++)
            {
                String line = text[x];
                String[] split = line.Split(',');
                if (Convert.ToUInt32(split[0]) != 0)
                {
                    if (!shop.SteedShops.ContainsKey(Convert.ToUInt32(split[0])))
                    {
                        Shop.SteedShop shopi = new Shop.SteedShop();
                        shopi.cost = Convert.ToUInt32(split[1]);
                        shopi.itemid = Convert.ToUInt32(split[0]);

                        shop.SteedShops.Add(shopi.itemid, shopi);
                    }
                }
            }
            Shops.Add(6001, shop);

        }
        public static void LoadHonorShop()
        {
            String[] text = File.ReadAllLines("database\\honorshop.ini");
            Shop shop = new Shop();
            shop.UID = 6000;
            shop.MoneyType = MoneyType.HonorPoints;
            shop.HonorShopItems = new Dictionary<uint, Shop.HonorShopItem>();
            for (int x = 0; x < text.Length; x++)
            {
                String line = text[x];
                String[] split = line.Split(',');
                if (Convert.ToUInt32(split[0]) != 0)
                {
                    if (!shop.HonorShopItems.ContainsKey(Convert.ToUInt32(split[0])))
                    {
                        Shop.HonorShopItem shopi = new Shop.HonorShopItem();
                        shopi.cost = Convert.ToUInt32(split[1]);
                        shopi.itemid = Convert.ToUInt32(split[0]);

                        shop.HonorShopItems.Add(shopi.itemid, shopi);
                    }
                }
            }
                Shops.Add(6000, shop);
         
        }
        public static void Load()
        {
            string[] text = File.ReadAllLines(ServerBase.Constants.ShopsPath);
            Shop shop = new Shop();
            for (int x = 0; x < text.Length; x++)
            {
                string line = text[x];
                string[] split = line.Split('=');
               // Console.WriteLine("split[0] " + split[0] + ".");
                //Console.WriteLine("split[1] " + split[1] + ".");
                if (split[0] == "Amount")
                    Shops = new Dictionary<uint, Shop>(int.Parse(split[1]));
                else if (split[0] == "ID")
                {
                    if (shop.UID == 0)
                    {
                        shop.UID = uint.Parse(split[1]);
                        //Console.WriteLine("K split[1] " + split[1] + ".");
                    }
                    else
                    {
                        if (!Shops.ContainsKey(shop.UID))
                        {
                            Shops.Add(shop.UID, shop);
                            shop = new Shop();
                            shop.UID = uint.Parse(split[1]);
                            //Console.WriteLine("split[1] " + shop.UID + ".");
                        }
                    }
                }
                
                    
                
                else if (split[0] == "MoneyType")
                {
                    shop.MoneyType = (MoneyType)byte.Parse(split[1]);
                }
                else if (split[0] == "ItemAmount")
                {
                    shop.Items = new List<uint>(ushort.Parse(split[1]));
                }
                else if (split[0].Contains("Item") && split[0] != "ItemAmount")
                {
                    uint ID = uint.Parse(split[1]);
                    if (!shop.Items.Contains(ID))
                        shop.Items.Add(ID);
                }
            }
            if (!Shops.ContainsKey(shop.UID))
            {
                
                    Shops.Add(shop.UID, shop);
                
            }
            LoadHonorShop();
            LoadRaceShop();
            Console.WriteLine("Shops information loaded.");
        }
        public enum MoneyType
        {
            Gold = 0,
            ConquerPoints = 1,
            BConquerPoints = 2,
            RaceShop = 3,
            HonorPoints = 4
        }
    }
}
