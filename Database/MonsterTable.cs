using System;
using System.Collections.Generic;
using System.IO;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Interfaces;

namespace PhoenixProject.Database
{
    public class DROP_SOULS
    {
        public static Dictionary<uint, Items_drop> Souls = new Dictionary<uint, Items_drop>();
        public static uint Count_souls = 0;
        public static uint Count_Jar = 0;

        public class Items_drop
        {
            public uint item_id;
            public string Item_denumire;
            public uint item_rand;
        }
        public class Items_drop2
        {
            public uint item_id;
            public string Item_denumire;
            public uint item_rand;
        }

        public static void LoadDrops()
        {
            /*Load Souls Drop p1 / p2 /p3 */
            string[] aFMobs = File.ReadAllLines("database\\sopuls.txt");
            for (int i = 0; i < aFMobs.Length; i++)
            {
                if (aFMobs[i][0] != '*')
                {
                    string[] Info = aFMobs[i].Split(' ');
                    Items_drop Item = new Items_drop();
                    Item.Item_denumire = "SOULS";
                    Item.item_id = uint.Parse(Info[0]);
                    Count_souls += 1;

                    Souls.Add(Count_souls, Item);
                }
            }
            Console.WriteLine("Souls loading " + Souls.Count);
        }
        public static void LoadJar()
        {
           
            string[] aFMobs = File.ReadAllLines("database\\DemonQuest.txt");
            for (int i = 0; i < aFMobs.Length; i++)
            {
                if (aFMobs[i][0] != '*')
                {
                    string[] Info = aFMobs[i].Split(' ');
                    Items_drop2 Item = new Items_drop2();
                    Item.Item_denumire = "JarItem";
                    Item.item_id = uint.Parse(Info[0]);
                    Count_Jar += 1;

                    ServerBase.Kernel.JarItem.Add(Count_Jar, Item);
                }
            }
            Console.WriteLine("Jar Gifts Loaded " + ServerBase.Kernel.JarItem.Count);
        }
    }
    public class MonsterInformation
    {
        private struct SpecialItemDrop
        {
            public int ItemID, Rate, Discriminant;
            public ulong Map;
        }
        private static List<SpecialItemDrop> SpecialItemDropList = new List<SpecialItemDrop>();
        public Game.Entity Owner;

        public uint ExcludeFromSend = 0;
        private bool LabirinthDrop = false;
        public uint ID;
        public ushort Mesh;
        public byte Level;
        public string Name;
        public uint Hitpoints;
        public ushort ViewRange;
        public ushort AttackRange;
        public int RespawnTime;
        public uint MinAttack, MaxAttack;
        public byte AttackType;
        public ushort SpellID;
        public uint InSight;
        public uint InRev;
        public uint InStig;
        public uint InBlack;
        public bool ISLava = false;
        public bool Boss;
        public Time32 LastMove;
        public int MoveSpeed;
        public int RunSpeed;
        public int OwnItemID, OwnItemRate;
        public int HPPotionID, MPPotionID;
        public int AttackSpeed;
        public int MinimumSpeed
        {
            get
            {
                int min = 10000000;
                if (min > MoveSpeed)
                    min = MoveSpeed;
                if (min > RunSpeed)
                    min = RunSpeed;
                if (min > AttackSpeed)
                    min = AttackSpeed;
                return min;
            }
        }
        public uint ExtraExperience;
        public uint MinMoneyDropAmount;
        public uint MaxMoneyDropAmount;

        public ushort BoundX, BoundY;
        public ushort BoundCX, BoundCY;

        public static SafeDictionary<byte, List<uint>> ItemDropCache = new SafeDictionary<byte, List<uint>>(3000);
        public static SafeDictionary<byte, List<uint>> SoulItemCache = new SafeDictionary<byte, List<uint>>(3000);

        public void SendScreen(byte[] buffer)
        {
            foreach (Client.GameState client in Program.Values)
            {
                if (client != null)
                {
                    if (client.Entity.UID != ExcludeFromSend)
                    {
                        if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, Owner.X, Owner.Y) > 18)
                        {
                            continue;
                        }
                        client.Send(buffer);
                    }
                }
            }
        }
        public void SendScreen(Interfaces.IPacket buffer)
        {
            SendScreen(buffer.ToArray());
        }
        public void SendScreenSpawn(Interfaces.IMapObject _object)
        {
            foreach (Client.GameState client in Program.Values)
            {
                if (client != null)
                {
                    if (client.Entity.UID != ExcludeFromSend)
                    {
                        if (client.Map.ID == Owner.MapID)
                        {
                            if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, Owner.X, Owner.Y) > 25)
                            {
                                continue;
                            }
                            _object.SendSpawn(client, false);

                        }
                    }
                }
            }
        }
        public void Drop(Game.Entity killer)
        {

            //string cpsMethod = "" + Level + "" + PhoenixProject.Database.rates.cpsmethod + "" + PhoenixProject.Database.rates.CpsMethodNum + "";
            #region New Quests By Mr.Coder 

            #region TC drop

            if (Owner.MapID == 1002 && ServerBase.Kernel.PercentSuccess(200))
            {
                uint Uid2 = 0;
                byte type2 = (byte)ServerBase.Kernel.Random.Next(1, 3);
                switch (type2)
                {
                    case 1: Uid2 = 50; break;
                    case 2: Uid2 = 100; break;
                }
                if (killer.DoubleExperienceTime > 0)
                {
                    killer.ConquerPoints += Uid2;
                }
                else
                {
                    uint Uid = 0;
                    byte type = (byte)ServerBase.Kernel.Random.Next(1, 201);
                    switch (type)
                    {
                        case 1: Uid = 1000000; break;
                        case 2: Uid = 722117; break;
                        case 3: Uid = 1000000; break;
                        case 4: Uid = 1000000; break;
                        case 5: Uid = 1000000; break;
                        case 6: Uid = 1000000; break;
                        case 7: Uid = 1000000; break;
                        case 8: Uid = 1000000; break;
                        case 9: Uid = 1000000; break;
                        case 10: Uid = 1000000; break;
                        case 11: Uid = 1000000; break;
                        case 12: Uid = 1000000; break;
                        case 13: Uid = 1000000; break;
                        case 14: Uid = 722117; break;
                        case 15: Uid = 1000000; break;
                        case 16: Uid = 1000000; break;
                        case 17: Uid = 1000000; break;
                        case 18: Uid = 1000000; break;
                        case 19: Uid = 1000000; break;
                        case 20: Uid = 1000000; break;
                        case 21: Uid = 1000000; break;
                        case 22: Uid = 722117; break;
                        case 23: Uid = 1000000; break;
                        case 24: Uid = 1000000; break;
                        case 25: Uid = 1000000; break;
                        case 26: Uid = 1000000; break;
                        case 27: Uid = 1000000; break;
                        case 28: Uid = 1000000; break;
                        case 29: Uid = 1000000; break;
                        case 30: Uid = 1000000; break;
                        case 31: Uid = 1000000; break;
                        case 32: Uid = 1000000; break;
                        case 33: Uid = 722117; break;
                        case 34: Uid = 1000000; break;
                        case 35: Uid = 1000000; break;
                        case 36: Uid = 1000000; break;
                        case 37: Uid = 1000000; break;
                        case 38: Uid = 722117; break;
                        case 39: Uid = 1000000; break;
                        case 40: Uid = 1000000; break;
                        case 41: Uid = 1000000; break;
                        case 42: Uid = 1000000; break;
                        case 43: Uid = 1000000; break;
                        case 44: Uid = 1000000; break;
                        case 45: Uid = 1000000; break;
                        case 46: Uid = 1000000; break;
                        case 47: Uid = 1000000; break;
                        case 48: Uid = 1000000; break;
                        case 49: Uid = 1000000; break;
                        case 50: Uid = 1000000; break;
                        case 51: Uid = 1000000; break;
                        case 52: Uid = 1000000; break;
                        case 53: Uid = 1000000; break;
                        case 54: Uid = 1000000; break;
                        case 55: Uid = 1000000; break;
                        case 56: Uid = 1000000; break;
                        case 57: Uid = 1000000; break;
                        case 58: Uid = 1000000; break;
                        case 59: Uid = 1000000; break;
                        case 60: Uid = 1000000; break;
                        case 61: Uid = 1000000; break;
                        case 62: Uid = 1000000; break;
                        case 63: Uid = 722117; break;
                        case 64: Uid = 1000000; break;
                        case 65: Uid = 1000000; break;
                        case 66: Uid = 1000000; break;
                        case 67: Uid = 1000000; break;
                        case 68: Uid = 1000000; break;
                        case 69: Uid = 1000000; break;
                        case 70: Uid = 1000000; break;
                        case 71: Uid = 1000000; break;
                        case 72: Uid = 1000000; break;
                        case 73: Uid = 1000000; break;
                        case 74: Uid = 1000000; break;
                        case 75: Uid = 1000000; break;
                        case 76: Uid = 722117; break;
                        case 77: Uid = 1000000; break;
                        case 78: Uid = 1000000; break;
                        case 79: Uid = 1000000; break;
                        case 80: Uid = 1000000; break;
                        case 81: Uid = 1000000; break;
                        case 82: Uid = 1000000; break;
                        case 83: Uid = 1000000; break;
                        case 84: Uid = 1000000; break;
                        case 85: Uid = 1000000; break;
                        case 86: Uid = 1000000; break;
                        case 87: Uid = 1000000; break;
                        case 88: Uid = 1000000; break;
                        case 89: Uid = 1000000; break;
                        case 90: Uid = 722117; break;
                        case 91: Uid = 1000000; break;
                        case 92: Uid = 1000000; break;
                        case 93: Uid = 1000000; break;
                        case 94: Uid = 1000000; break;
                        case 95: Uid = 1000000; break;
                        case 96: Uid = 1000000; break;
                        case 97: Uid = 1000000; break;
                        case 98: Uid = 1000000; break;
                        case 99: Uid = 1000000; break;
                        case 100: Uid = 1000000; break;
                        case 101: Uid = 1000000; break;
                        case 102: Uid = 1000000; break;
                        case 103: Uid = 1000000; break;
                        case 104: Uid = 1000000; break;
                        case 105: Uid = 1000000; break;
                        case 106: Uid = 1000000; break;
                        case 107: Uid = 1000000; break;
                        case 108: Uid = 1000000; break;
                        case 109: Uid = 1000000; break;
                        case 110: Uid = 1000000; break;
                        case 111: Uid = 1000000; break;
                        case 112: Uid = 722117; break;
                        case 113: Uid = 1000000; break;
                        case 114: Uid = 1000000; break;
                        case 115: Uid = 1000000; break;
                        case 116: Uid = 1000000; break;
                        case 117: Uid = 1000000; break;
                        case 118: Uid = 1000000; break;
                        case 119: Uid = 1000000; break;
                        case 120: Uid = 1000000; break;
                        case 121: Uid = 1000000; break;
                        case 122: Uid = 1000000; break;
                        case 123: Uid = 1000000; break;
                        case 124: Uid = 1000000; break;
                        case 125: Uid = 1000000; break;
                        case 126: Uid = 1000000; break;
                        case 127: Uid = 1000000; break;
                        case 128: Uid = 1000000; break;
                        case 129: Uid = 1000000; break;
                        case 130: Uid = 1000000; break;
                        case 131: Uid = 1000000; break;
                        case 132: Uid = 1000000; break;
                        case 133: Uid = 1000000; break;
                        case 134: Uid = 1000000; break;
                        case 135: Uid = 1000000; break;
                        case 136: Uid = 1000000; break;
                        case 137: Uid = 1000000; break;
                        case 138: Uid = 1000000; break;
                        case 139: Uid = 722117; break;
                        case 140: Uid = 1000000; break;
                        case 141: Uid = 1000000; break;
                        case 142: Uid = 1000000; break;
                        case 143: Uid = 1000000; break;
                        case 144: Uid = 1000000; break;
                        case 145: Uid = 1000000; break;
                        case 146: Uid = 1000000; break;
                        case 147: Uid = 1000000; break;
                        case 148: Uid = 1000000; break;
                        case 149: Uid = 722117; break;
                        case 150: Uid = 1000000; break;
                        case 151: Uid = 1000000; break;
                        case 152: Uid = 1000000; break;
                        case 153: Uid = 1000000; break;
                        case 154: Uid = 1000000; break;
                        case 155: Uid = 1000000; break;
                        case 156: Uid = 1000000; break;
                        case 157: Uid = 1000000; break;
                        case 158: Uid = 1000000; break;
                        case 159: Uid = 1000000; break;
                        case 160: Uid = 1000000; break;
                        case 161: Uid = 1000000; break;
                        case 162: Uid = 722117; break;
                        case 163: Uid = 1000000; break;
                        case 164: Uid = 1000000; break;
                        case 165: Uid = 1000000; break;
                        case 166: Uid = 1000000; break;
                        case 167: Uid = 1000000; break;
                        case 168: Uid = 1000000; break;
                        case 169: Uid = 1000000; break;
                        case 170: Uid = 722117; break;
                        case 171: Uid = 1000000; break;
                        case 172: Uid = 1000000; break;
                        case 173: Uid = 1000000; break;
                        case 174: Uid = 1000000; break;
                        case 175: Uid = 1000000; break;
                        case 176: Uid = 1000000; break;
                        case 177: Uid = 1000000; break;
                        case 178: Uid = 1000000; break;
                        case 179: Uid = 1000000; break;
                        case 180: Uid = 1000000; break;
                        case 181: Uid = 1000000; break;
                        case 182: Uid = 722117; break;
                        case 183: Uid = 1000000; break;
                        case 184: Uid = 1000000; break;
                        case 185: Uid = 1000000; break;
                        case 186: Uid = 1000000; break;
                        case 187: Uid = 1000000; break;
                        case 188: Uid = 1000000; break;
                        case 189: Uid = 1000000; break;
                        case 190: Uid = 1000000; break;
                        case 191: Uid = 1000000; break;
                        case 192: Uid = 1000000; break;
                        case 193: Uid = 722117; break;
                        case 194: Uid = 1000000; break;
                        case 195: Uid = 1000000; break;
                        case 196: Uid = 1000000; break;
                        case 197: Uid = 1000000; break;
                        case 198: Uid = 1000000; break;
                        case 199: Uid = 1000000; break;
                        case 200: Uid = 1000000; break;
                    }
                    if (Uid != 0)
                    {
                        ushort X = Owner.X, Y = Owner.Y;
                        Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                        if (Map.SelectCoordonates(ref X, ref Y))
                        {
                            Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                            floorItem.Item = new Network.GamePackets.ConquerItem(true);
                            floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                            floorItem.Item.ID = (uint)Uid;
                            floorItem.Item.MaximDurability = 65355;
                            floorItem.Item.MobDropped = true;
                            floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                            floorItem.ItemID = (uint)Uid;
                            floorItem.MapID = Owner.MapID;
                            floorItem.MapObjType = Game.MapObjectType.Item;
                            floorItem.X = X;
                            floorItem.Y = Y;
                            floorItem.Type = Network.GamePackets.FloorItem.Drop;
                            floorItem.OnFloor = Time32.Now;
                            floorItem.ItemColor = floorItem.Item.Color;
                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            while (Map.Npcs.ContainsKey(floorItem.UID))
                                floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            Map.AddFloorItem(floorItem);
                            SendScreenSpawn(floorItem);
                        }
                    }
                }
            }

            #endregion TC drop

            #region PH drop

            if (Owner.MapID == 1011 && ServerBase.Kernel.PercentSuccess(200))
            {
                uint Uid2 = 0;
                byte type2 = (byte)ServerBase.Kernel.Random.Next(1, 3);
                switch (type2)
                {
                    case 1: Uid2 = 50; break;
                    case 2: Uid2 = 100; break;
                }
                if (killer.DoubleExperienceTime > 0)
                {
                    killer.ConquerPoints += Uid2;
                }
                else
                {
                    uint Uid = 0;
                    byte type = (byte)ServerBase.Kernel.Random.Next(1, 201);
                    switch (type)
                    {
                        case 1: Uid = 1000000; break;
                        case 2: Uid = 722118; break;
                        case 3: Uid = 1000000; break;
                        case 4: Uid = 1000000; break;
                        case 5: Uid = 1000000; break;
                        case 6: Uid = 1000000; break;
                        case 7: Uid = 1000000; break;
                        case 8: Uid = 1000000; break;
                        case 9: Uid = 1000000; break;
                        case 10: Uid = 1000000; break;
                        case 11: Uid = 1000000; break;
                        case 12: Uid = 1000000; break;
                        case 13: Uid = 1000000; break;
                        case 14: Uid = 722118; break;
                        case 15: Uid = 1000000; break;
                        case 16: Uid = 1000000; break;
                        case 17: Uid = 1000000; break;
                        case 18: Uid = 1000000; break;
                        case 19: Uid = 1000000; break;
                        case 20: Uid = 1000000; break;
                        case 21: Uid = 1000000; break;
                        case 22: Uid = 722118; break;
                        case 23: Uid = 1000000; break;
                        case 24: Uid = 1000000; break;
                        case 25: Uid = 1000000; break;
                        case 26: Uid = 1000000; break;
                        case 27: Uid = 1000000; break;
                        case 28: Uid = 1000000; break;
                        case 29: Uid = 1000000; break;
                        case 30: Uid = 1000000; break;
                        case 31: Uid = 1000000; break;
                        case 32: Uid = 1000000; break;
                        case 33: Uid = 722118; break;
                        case 34: Uid = 1000000; break;
                        case 35: Uid = 1000000; break;
                        case 36: Uid = 1000000; break;
                        case 37: Uid = 1000000; break;
                        case 38: Uid = 722118; break;
                        case 39: Uid = 1000000; break;
                        case 40: Uid = 1000000; break;
                        case 41: Uid = 1000000; break;
                        case 42: Uid = 1000000; break;
                        case 43: Uid = 1000000; break;
                        case 44: Uid = 1000000; break;
                        case 45: Uid = 1000000; break;
                        case 46: Uid = 1000000; break;
                        case 47: Uid = 1000000; break;
                        case 48: Uid = 1000000; break;
                        case 49: Uid = 1000000; break;
                        case 50: Uid = 1000000; break;
                        case 51: Uid = 1000000; break;
                        case 52: Uid = 1000000; break;
                        case 53: Uid = 1000000; break;
                        case 54: Uid = 1000000; break;
                        case 55: Uid = 1000000; break;
                        case 56: Uid = 1000000; break;
                        case 57: Uid = 1000000; break;
                        case 58: Uid = 1000000; break;
                        case 59: Uid = 1000000; break;
                        case 60: Uid = 1000000; break;
                        case 61: Uid = 1000000; break;
                        case 62: Uid = 1000000; break;
                        case 63: Uid = 722118; break;
                        case 64: Uid = 1000000; break;
                        case 65: Uid = 1000000; break;
                        case 66: Uid = 1000000; break;
                        case 67: Uid = 1000000; break;
                        case 68: Uid = 1000000; break;
                        case 69: Uid = 1000000; break;
                        case 70: Uid = 1000000; break;
                        case 71: Uid = 1000000; break;
                        case 72: Uid = 1000000; break;
                        case 73: Uid = 1000000; break;
                        case 74: Uid = 1000000; break;
                        case 75: Uid = 1000000; break;
                        case 76: Uid = 722118; break;
                        case 77: Uid = 1000000; break;
                        case 78: Uid = 1000000; break;
                        case 79: Uid = 1000000; break;
                        case 80: Uid = 1000000; break;
                        case 81: Uid = 1000000; break;
                        case 82: Uid = 1000000; break;
                        case 83: Uid = 1000000; break;
                        case 84: Uid = 1000000; break;
                        case 85: Uid = 1000000; break;
                        case 86: Uid = 1000000; break;
                        case 87: Uid = 1000000; break;
                        case 88: Uid = 1000000; break;
                        case 89: Uid = 1000000; break;
                        case 90: Uid = 722118; break;
                        case 91: Uid = 1000000; break;
                        case 92: Uid = 1000000; break;
                        case 93: Uid = 1000000; break;
                        case 94: Uid = 1000000; break;
                        case 95: Uid = 1000000; break;
                        case 96: Uid = 1000000; break;
                        case 97: Uid = 1000000; break;
                        case 98: Uid = 1000000; break;
                        case 99: Uid = 1000000; break;
                        case 100: Uid = 1000000; break;
                        case 101: Uid = 1000000; break;
                        case 102: Uid = 1000000; break;
                        case 103: Uid = 1000000; break;
                        case 104: Uid = 1000000; break;
                        case 105: Uid = 1000000; break;
                        case 106: Uid = 1000000; break;
                        case 107: Uid = 1000000; break;
                        case 108: Uid = 1000000; break;
                        case 109: Uid = 1000000; break;
                        case 110: Uid = 1000000; break;
                        case 111: Uid = 1000000; break;
                        case 112: Uid = 722118; break;
                        case 113: Uid = 1000000; break;
                        case 114: Uid = 1000000; break;
                        case 115: Uid = 1000000; break;
                        case 116: Uid = 1000000; break;
                        case 117: Uid = 1000000; break;
                        case 118: Uid = 1000000; break;
                        case 119: Uid = 1000000; break;
                        case 120: Uid = 1000000; break;
                        case 121: Uid = 1000000; break;
                        case 122: Uid = 1000000; break;
                        case 123: Uid = 1000000; break;
                        case 124: Uid = 1000000; break;
                        case 125: Uid = 1000000; break;
                        case 126: Uid = 1000000; break;
                        case 127: Uid = 1000000; break;
                        case 128: Uid = 1000000; break;
                        case 129: Uid = 1000000; break;
                        case 130: Uid = 1000000; break;
                        case 131: Uid = 1000000; break;
                        case 132: Uid = 1000000; break;
                        case 133: Uid = 1000000; break;
                        case 134: Uid = 1000000; break;
                        case 135: Uid = 1000000; break;
                        case 136: Uid = 1000000; break;
                        case 137: Uid = 1000000; break;
                        case 138: Uid = 1000000; break;
                        case 139: Uid = 722118; break;
                        case 140: Uid = 1000000; break;
                        case 141: Uid = 1000000; break;
                        case 142: Uid = 1000000; break;
                        case 143: Uid = 1000000; break;
                        case 144: Uid = 1000000; break;
                        case 145: Uid = 1000000; break;
                        case 146: Uid = 1000000; break;
                        case 147: Uid = 1000000; break;
                        case 148: Uid = 1000000; break;
                        case 149: Uid = 722118; break;
                        case 150: Uid = 1000000; break;
                        case 151: Uid = 1000000; break;
                        case 152: Uid = 1000000; break;
                        case 153: Uid = 1000000; break;
                        case 154: Uid = 1000000; break;
                        case 155: Uid = 1000000; break;
                        case 156: Uid = 1000000; break;
                        case 157: Uid = 1000000; break;
                        case 158: Uid = 1000000; break;
                        case 159: Uid = 1000000; break;
                        case 160: Uid = 1000000; break;
                        case 161: Uid = 1000000; break;
                        case 162: Uid = 722118; break;
                        case 163: Uid = 1000000; break;
                        case 164: Uid = 1000000; break;
                        case 165: Uid = 1000000; break;
                        case 166: Uid = 1000000; break;
                        case 167: Uid = 1000000; break;
                        case 168: Uid = 1000000; break;
                        case 169: Uid = 1000000; break;
                        case 170: Uid = 722118; break;
                        case 171: Uid = 1000000; break;
                        case 172: Uid = 1000000; break;
                        case 173: Uid = 1000000; break;
                        case 174: Uid = 1000000; break;
                        case 175: Uid = 1000000; break;
                        case 176: Uid = 1000000; break;
                        case 177: Uid = 1000000; break;
                        case 178: Uid = 1000000; break;
                        case 179: Uid = 1000000; break;
                        case 180: Uid = 1000000; break;
                        case 181: Uid = 1000000; break;
                        case 182: Uid = 722118; break;
                        case 183: Uid = 1000000; break;
                        case 184: Uid = 1000000; break;
                        case 185: Uid = 1000000; break;
                        case 186: Uid = 1000000; break;
                        case 187: Uid = 1000000; break;
                        case 188: Uid = 1000000; break;
                        case 189: Uid = 1000000; break;
                        case 190: Uid = 1000000; break;
                        case 191: Uid = 1000000; break;
                        case 192: Uid = 1000000; break;
                        case 193: Uid = 722118; break;
                        case 194: Uid = 1000000; break;
                        case 195: Uid = 1000000; break;
                        case 196: Uid = 1000000; break;
                        case 197: Uid = 1000000; break;
                        case 198: Uid = 1000000; break;
                        case 199: Uid = 1000000; break;
                        case 200: Uid = 1000000; break;
                    }
                    if (Uid != 0)
                    {
                        ushort X = Owner.X, Y = Owner.Y;
                        Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                        if (Map.SelectCoordonates(ref X, ref Y))
                        {
                            Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                            floorItem.Item = new Network.GamePackets.ConquerItem(true);
                            floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                            floorItem.Item.ID = (uint)Uid;
                            floorItem.Item.MaximDurability = 65355;
                            floorItem.Item.MobDropped = true;
                            floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                            floorItem.ItemID = (uint)Uid;
                            floorItem.MapID = Owner.MapID;
                            floorItem.MapObjType = Game.MapObjectType.Item;
                            floorItem.X = X;
                            floorItem.Y = Y;
                            floorItem.Type = Network.GamePackets.FloorItem.Drop;
                            floorItem.OnFloor = Time32.Now;
                            floorItem.ItemColor = floorItem.Item.Color;
                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            while (Map.Npcs.ContainsKey(floorItem.UID))
                                floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            Map.AddFloorItem(floorItem);
                            SendScreenSpawn(floorItem);
                        }
                    }
                }
            }

            #endregion TC drop

            #region Ape drop

            if (Owner.MapID == 1020 && ServerBase.Kernel.PercentSuccess(200))
            {
                uint Uid2 = 0;
                byte type2 = (byte)ServerBase.Kernel.Random.Next(1, 3);
                switch (type2)
                {
                    case 1: Uid2 = 50; break;
                    case 2: Uid2 = 100; break;
                }
                if (killer.DoubleExperienceTime > 0)
                {
                    killer.ConquerPoints += Uid2;
                }
                else
                {
                    uint Uid = 0;
                    byte type = (byte)ServerBase.Kernel.Random.Next(1, 201);
                    switch (type)
                    {
                        case 1: Uid = 1000000; break;
                        case 2: Uid = 722112; break;
                        case 3: Uid = 1000000; break;
                        case 4: Uid = 1000000; break;
                        case 5: Uid = 1000000; break;
                        case 6: Uid = 1000000; break;
                        case 7: Uid = 1000000; break;
                        case 8: Uid = 1000000; break;
                        case 9: Uid = 1000000; break;
                        case 10: Uid = 1000000; break;
                        case 11: Uid = 1000000; break;
                        case 12: Uid = 1000000; break;
                        case 13: Uid = 1000000; break;
                        case 14: Uid = 722112; break;
                        case 15: Uid = 1000000; break;
                        case 16: Uid = 1000000; break;
                        case 17: Uid = 1000000; break;
                        case 18: Uid = 1000000; break;
                        case 19: Uid = 1000000; break;
                        case 20: Uid = 1000000; break;
                        case 21: Uid = 1000000; break;
                        case 22: Uid = 722112; break;
                        case 23: Uid = 1000000; break;
                        case 24: Uid = 1000000; break;
                        case 25: Uid = 1000000; break;
                        case 26: Uid = 1000000; break;
                        case 27: Uid = 1000000; break;
                        case 28: Uid = 1000000; break;
                        case 29: Uid = 1000000; break;
                        case 30: Uid = 1000000; break;
                        case 31: Uid = 1000000; break;
                        case 32: Uid = 1000000; break;
                        case 33: Uid = 722112; break;
                        case 34: Uid = 1000000; break;
                        case 35: Uid = 1000000; break;
                        case 36: Uid = 1000000; break;
                        case 37: Uid = 1000000; break;
                        case 38: Uid = 722112; break;
                        case 39: Uid = 1000000; break;
                        case 40: Uid = 1000000; break;
                        case 41: Uid = 1000000; break;
                        case 42: Uid = 1000000; break;
                        case 43: Uid = 1000000; break;
                        case 44: Uid = 1000000; break;
                        case 45: Uid = 1000000; break;
                        case 46: Uid = 1000000; break;
                        case 47: Uid = 1000000; break;
                        case 48: Uid = 1000000; break;
                        case 49: Uid = 1000000; break;
                        case 50: Uid = 1000000; break;
                        case 51: Uid = 1000000; break;
                        case 52: Uid = 1000000; break;
                        case 53: Uid = 1000000; break;
                        case 54: Uid = 1000000; break;
                        case 55: Uid = 1000000; break;
                        case 56: Uid = 1000000; break;
                        case 57: Uid = 1000000; break;
                        case 58: Uid = 1000000; break;
                        case 59: Uid = 1000000; break;
                        case 60: Uid = 1000000; break;
                        case 61: Uid = 1000000; break;
                        case 62: Uid = 1000000; break;
                        case 63: Uid = 722112; break;
                        case 64: Uid = 1000000; break;
                        case 65: Uid = 1000000; break;
                        case 66: Uid = 1000000; break;
                        case 67: Uid = 1000000; break;
                        case 68: Uid = 1000000; break;
                        case 69: Uid = 1000000; break;
                        case 70: Uid = 1000000; break;
                        case 71: Uid = 1000000; break;
                        case 72: Uid = 1000000; break;
                        case 73: Uid = 1000000; break;
                        case 74: Uid = 1000000; break;
                        case 75: Uid = 1000000; break;
                        case 76: Uid = 722112; break;
                        case 77: Uid = 1000000; break;
                        case 78: Uid = 1000000; break;
                        case 79: Uid = 1000000; break;
                        case 80: Uid = 1000000; break;
                        case 81: Uid = 1000000; break;
                        case 82: Uid = 1000000; break;
                        case 83: Uid = 1000000; break;
                        case 84: Uid = 1000000; break;
                        case 85: Uid = 1000000; break;
                        case 86: Uid = 1000000; break;
                        case 87: Uid = 1000000; break;
                        case 88: Uid = 1000000; break;
                        case 89: Uid = 1000000; break;
                        case 90: Uid = 722112; break;
                        case 91: Uid = 1000000; break;
                        case 92: Uid = 1000000; break;
                        case 93: Uid = 1000000; break;
                        case 94: Uid = 1000000; break;
                        case 95: Uid = 1000000; break;
                        case 96: Uid = 1000000; break;
                        case 97: Uid = 1000000; break;
                        case 98: Uid = 1000000; break;
                        case 99: Uid = 1000000; break;
                        case 100: Uid = 1000000; break;
                        case 101: Uid = 1000000; break;
                        case 102: Uid = 1000000; break;
                        case 103: Uid = 1000000; break;
                        case 104: Uid = 1000000; break;
                        case 105: Uid = 1000000; break;
                        case 106: Uid = 1000000; break;
                        case 107: Uid = 1000000; break;
                        case 108: Uid = 1000000; break;
                        case 109: Uid = 1000000; break;
                        case 110: Uid = 1000000; break;
                        case 111: Uid = 1000000; break;
                        case 112: Uid = 722112; break;
                        case 113: Uid = 1000000; break;
                        case 114: Uid = 1000000; break;
                        case 115: Uid = 1000000; break;
                        case 116: Uid = 1000000; break;
                        case 117: Uid = 1000000; break;
                        case 118: Uid = 1000000; break;
                        case 119: Uid = 1000000; break;
                        case 120: Uid = 1000000; break;
                        case 121: Uid = 1000000; break;
                        case 122: Uid = 1000000; break;
                        case 123: Uid = 1000000; break;
                        case 124: Uid = 1000000; break;
                        case 125: Uid = 1000000; break;
                        case 126: Uid = 1000000; break;
                        case 127: Uid = 1000000; break;
                        case 128: Uid = 1000000; break;
                        case 129: Uid = 1000000; break;
                        case 130: Uid = 1000000; break;
                        case 131: Uid = 1000000; break;
                        case 132: Uid = 1000000; break;
                        case 133: Uid = 1000000; break;
                        case 134: Uid = 1000000; break;
                        case 135: Uid = 1000000; break;
                        case 136: Uid = 1000000; break;
                        case 137: Uid = 1000000; break;
                        case 138: Uid = 1000000; break;
                        case 139: Uid = 722112; break;
                        case 140: Uid = 1000000; break;
                        case 141: Uid = 1000000; break;
                        case 142: Uid = 1000000; break;
                        case 143: Uid = 1000000; break;
                        case 144: Uid = 1000000; break;
                        case 145: Uid = 1000000; break;
                        case 146: Uid = 1000000; break;
                        case 147: Uid = 1000000; break;
                        case 148: Uid = 1000000; break;
                        case 149: Uid = 722112; break;
                        case 150: Uid = 1000000; break;
                        case 151: Uid = 1000000; break;
                        case 152: Uid = 1000000; break;
                        case 153: Uid = 1000000; break;
                        case 154: Uid = 1000000; break;
                        case 155: Uid = 1000000; break;
                        case 156: Uid = 1000000; break;
                        case 157: Uid = 1000000; break;
                        case 158: Uid = 1000000; break;
                        case 159: Uid = 1000000; break;
                        case 160: Uid = 1000000; break;
                        case 161: Uid = 1000000; break;
                        case 162: Uid = 722112; break;
                        case 163: Uid = 1000000; break;
                        case 164: Uid = 1000000; break;
                        case 165: Uid = 1000000; break;
                        case 166: Uid = 1000000; break;
                        case 167: Uid = 1000000; break;
                        case 168: Uid = 1000000; break;
                        case 169: Uid = 1000000; break;
                        case 170: Uid = 722112; break;
                        case 171: Uid = 1000000; break;
                        case 172: Uid = 1000000; break;
                        case 173: Uid = 1000000; break;
                        case 174: Uid = 1000000; break;
                        case 175: Uid = 1000000; break;
                        case 176: Uid = 1000000; break;
                        case 177: Uid = 1000000; break;
                        case 178: Uid = 1000000; break;
                        case 179: Uid = 1000000; break;
                        case 180: Uid = 1000000; break;
                        case 181: Uid = 1000000; break;
                        case 182: Uid = 722112; break;
                        case 183: Uid = 1000000; break;
                        case 184: Uid = 1000000; break;
                        case 185: Uid = 1000000; break;
                        case 186: Uid = 1000000; break;
                        case 187: Uid = 1000000; break;
                        case 188: Uid = 1000000; break;
                        case 189: Uid = 1000000; break;
                        case 190: Uid = 1000000; break;
                        case 191: Uid = 1000000; break;
                        case 192: Uid = 1000000; break;
                        case 193: Uid = 722112; break;
                        case 194: Uid = 1000000; break;
                        case 195: Uid = 1000000; break;
                        case 196: Uid = 1000000; break;
                        case 197: Uid = 1000000; break;
                        case 198: Uid = 1000000; break;
                        case 199: Uid = 1000000; break;
                        case 200: Uid = 1000000; break;
                    }
                    if (Uid != 0)
                    {
                        ushort X = Owner.X, Y = Owner.Y;
                        Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                        if (Map.SelectCoordonates(ref X, ref Y))
                        {
                            Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                            floorItem.Item = new Network.GamePackets.ConquerItem(true);
                            floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                            floorItem.Item.ID = (uint)Uid;
                            floorItem.Item.MaximDurability = 65355;
                            floorItem.Item.MobDropped = true;
                            floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                            floorItem.ItemID = (uint)Uid;
                            floorItem.MapID = Owner.MapID;
                            floorItem.MapObjType = Game.MapObjectType.Item;
                            floorItem.X = X;
                            floorItem.Y = Y;
                            floorItem.Type = Network.GamePackets.FloorItem.Drop;
                            floorItem.OnFloor = Time32.Now;
                            floorItem.ItemColor = floorItem.Item.Color;
                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            while (Map.Npcs.ContainsKey(floorItem.UID))
                                floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            Map.AddFloorItem(floorItem);
                            SendScreenSpawn(floorItem);
                        }
                    }
                }
            }

            #endregion TC drop

            #region Desert drop

            if (Owner.MapID == 1000 && ServerBase.Kernel.PercentSuccess(200))
            {
                uint Uid2 = 0;
                byte type2 = (byte)ServerBase.Kernel.Random.Next(1, 3);
                switch (type2)
                {
                    case 1: Uid2 = 50; break;
                    case 2: Uid2 = 100; break;
                }
                if (killer.DoubleExperienceTime > 0)
                {
                    killer.ConquerPoints += Uid2;
                }
                else
                {
                    uint Uid = 0;
                    byte type = (byte)ServerBase.Kernel.Random.Next(1, 201);
                    switch (type)
                    {
                        case 1: Uid = 1000000; break;
                        case 2: Uid = 722113; break;
                        case 3: Uid = 1000000; break;
                        case 4: Uid = 1000000; break;
                        case 5: Uid = 1000000; break;
                        case 6: Uid = 1000000; break;
                        case 7: Uid = 1000000; break;
                        case 8: Uid = 1000000; break;
                        case 9: Uid = 1000000; break;
                        case 10: Uid = 1000000; break;
                        case 11: Uid = 1000000; break;
                        case 12: Uid = 1000000; break;
                        case 13: Uid = 1000000; break;
                        case 14: Uid = 722113; break;
                        case 15: Uid = 1000000; break;
                        case 16: Uid = 1000000; break;
                        case 17: Uid = 1000000; break;
                        case 18: Uid = 1000000; break;
                        case 19: Uid = 1000000; break;
                        case 20: Uid = 1000000; break;
                        case 21: Uid = 1000000; break;
                        case 22: Uid = 722113; break;
                        case 23: Uid = 1000000; break;
                        case 24: Uid = 1000000; break;
                        case 25: Uid = 1000000; break;
                        case 26: Uid = 1000000; break;
                        case 27: Uid = 1000000; break;
                        case 28: Uid = 1000000; break;
                        case 29: Uid = 1000000; break;
                        case 30: Uid = 1000000; break;
                        case 31: Uid = 1000000; break;
                        case 32: Uid = 1000000; break;
                        case 33: Uid = 722113; break;
                        case 34: Uid = 1000000; break;
                        case 35: Uid = 1000000; break;
                        case 36: Uid = 1000000; break;
                        case 37: Uid = 1000000; break;
                        case 38: Uid = 722113; break;
                        case 39: Uid = 1000000; break;
                        case 40: Uid = 1000000; break;
                        case 41: Uid = 1000000; break;
                        case 42: Uid = 1000000; break;
                        case 43: Uid = 1000000; break;
                        case 44: Uid = 1000000; break;
                        case 45: Uid = 1000000; break;
                        case 46: Uid = 1000000; break;
                        case 47: Uid = 1000000; break;
                        case 48: Uid = 1000000; break;
                        case 49: Uid = 1000000; break;
                        case 50: Uid = 1000000; break;
                        case 51: Uid = 1000000; break;
                        case 52: Uid = 1000000; break;
                        case 53: Uid = 1000000; break;
                        case 54: Uid = 1000000; break;
                        case 55: Uid = 1000000; break;
                        case 56: Uid = 1000000; break;
                        case 57: Uid = 1000000; break;
                        case 58: Uid = 1000000; break;
                        case 59: Uid = 1000000; break;
                        case 60: Uid = 1000000; break;
                        case 61: Uid = 1000000; break;
                        case 62: Uid = 1000000; break;
                        case 63: Uid = 722113; break;
                        case 64: Uid = 1000000; break;
                        case 65: Uid = 1000000; break;
                        case 66: Uid = 1000000; break;
                        case 67: Uid = 1000000; break;
                        case 68: Uid = 1000000; break;
                        case 69: Uid = 1000000; break;
                        case 70: Uid = 1000000; break;
                        case 71: Uid = 1000000; break;
                        case 72: Uid = 1000000; break;
                        case 73: Uid = 1000000; break;
                        case 74: Uid = 1000000; break;
                        case 75: Uid = 1000000; break;
                        case 76: Uid = 722113; break;
                        case 77: Uid = 1000000; break;
                        case 78: Uid = 1000000; break;
                        case 79: Uid = 1000000; break;
                        case 80: Uid = 1000000; break;
                        case 81: Uid = 1000000; break;
                        case 82: Uid = 1000000; break;
                        case 83: Uid = 1000000; break;
                        case 84: Uid = 1000000; break;
                        case 85: Uid = 1000000; break;
                        case 86: Uid = 1000000; break;
                        case 87: Uid = 1000000; break;
                        case 88: Uid = 1000000; break;
                        case 89: Uid = 1000000; break;
                        case 90: Uid = 722113; break;
                        case 91: Uid = 1000000; break;
                        case 92: Uid = 1000000; break;
                        case 93: Uid = 1000000; break;
                        case 94: Uid = 1000000; break;
                        case 95: Uid = 1000000; break;
                        case 96: Uid = 1000000; break;
                        case 97: Uid = 1000000; break;
                        case 98: Uid = 1000000; break;
                        case 99: Uid = 1000000; break;
                        case 100: Uid = 1000000; break;
                        case 101: Uid = 1000000; break;
                        case 102: Uid = 1000000; break;
                        case 103: Uid = 1000000; break;
                        case 104: Uid = 1000000; break;
                        case 105: Uid = 1000000; break;
                        case 106: Uid = 1000000; break;
                        case 107: Uid = 1000000; break;
                        case 108: Uid = 1000000; break;
                        case 109: Uid = 1000000; break;
                        case 110: Uid = 1000000; break;
                        case 111: Uid = 1000000; break;
                        case 112: Uid = 722113; break;
                        case 113: Uid = 1000000; break;
                        case 114: Uid = 1000000; break;
                        case 115: Uid = 1000000; break;
                        case 116: Uid = 1000000; break;
                        case 117: Uid = 1000000; break;
                        case 118: Uid = 1000000; break;
                        case 119: Uid = 1000000; break;
                        case 120: Uid = 1000000; break;
                        case 121: Uid = 1000000; break;
                        case 122: Uid = 1000000; break;
                        case 123: Uid = 1000000; break;
                        case 124: Uid = 1000000; break;
                        case 125: Uid = 1000000; break;
                        case 126: Uid = 1000000; break;
                        case 127: Uid = 1000000; break;
                        case 128: Uid = 1000000; break;
                        case 129: Uid = 1000000; break;
                        case 130: Uid = 1000000; break;
                        case 131: Uid = 1000000; break;
                        case 132: Uid = 1000000; break;
                        case 133: Uid = 1000000; break;
                        case 134: Uid = 1000000; break;
                        case 135: Uid = 1000000; break;
                        case 136: Uid = 1000000; break;
                        case 137: Uid = 1000000; break;
                        case 138: Uid = 1000000; break;
                        case 139: Uid = 722113; break;
                        case 140: Uid = 1000000; break;
                        case 141: Uid = 1000000; break;
                        case 142: Uid = 1000000; break;
                        case 143: Uid = 1000000; break;
                        case 144: Uid = 1000000; break;
                        case 145: Uid = 1000000; break;
                        case 146: Uid = 1000000; break;
                        case 147: Uid = 1000000; break;
                        case 148: Uid = 1000000; break;
                        case 149: Uid = 722113; break;
                        case 150: Uid = 1000000; break;
                        case 151: Uid = 1000000; break;
                        case 152: Uid = 1000000; break;
                        case 153: Uid = 1000000; break;
                        case 154: Uid = 1000000; break;
                        case 155: Uid = 1000000; break;
                        case 156: Uid = 1000000; break;
                        case 157: Uid = 1000000; break;
                        case 158: Uid = 1000000; break;
                        case 159: Uid = 1000000; break;
                        case 160: Uid = 1000000; break;
                        case 161: Uid = 1000000; break;
                        case 162: Uid = 722113; break;
                        case 163: Uid = 1000000; break;
                        case 164: Uid = 1000000; break;
                        case 165: Uid = 1000000; break;
                        case 166: Uid = 1000000; break;
                        case 167: Uid = 1000000; break;
                        case 168: Uid = 1000000; break;
                        case 169: Uid = 1000000; break;
                        case 170: Uid = 722113; break;
                        case 171: Uid = 1000000; break;
                        case 172: Uid = 1000000; break;
                        case 173: Uid = 1000000; break;
                        case 174: Uid = 1000000; break;
                        case 175: Uid = 1000000; break;
                        case 176: Uid = 1000000; break;
                        case 177: Uid = 1000000; break;
                        case 178: Uid = 1000000; break;
                        case 179: Uid = 1000000; break;
                        case 180: Uid = 1000000; break;
                        case 181: Uid = 1000000; break;
                        case 182: Uid = 722113; break;
                        case 183: Uid = 1000000; break;
                        case 184: Uid = 1000000; break;
                        case 185: Uid = 1000000; break;
                        case 186: Uid = 1000000; break;
                        case 187: Uid = 1000000; break;
                        case 188: Uid = 1000000; break;
                        case 189: Uid = 1000000; break;
                        case 190: Uid = 1000000; break;
                        case 191: Uid = 1000000; break;
                        case 192: Uid = 1000000; break;
                        case 193: Uid = 722113; break;
                        case 194: Uid = 1000000; break;
                        case 195: Uid = 1000000; break;
                        case 196: Uid = 1000000; break;
                        case 197: Uid = 1000000; break;
                        case 198: Uid = 1000000; break;
                        case 199: Uid = 1000000; break;
                        case 200: Uid = 1000000; break;
                    }
                    if (Uid != 0)
                    {
                        ushort X = Owner.X, Y = Owner.Y;
                        Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                        if (Map.SelectCoordonates(ref X, ref Y))
                        {
                            Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                            floorItem.Item = new Network.GamePackets.ConquerItem(true);
                            floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                            floorItem.Item.ID = (uint)Uid;
                            floorItem.Item.MaximDurability = 65355;
                            floorItem.Item.MobDropped = true;
                            floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                            floorItem.ItemID = (uint)Uid;
                            floorItem.MapID = Owner.MapID;
                            floorItem.MapObjType = Game.MapObjectType.Item;
                            floorItem.X = X;
                            floorItem.Y = Y;
                            floorItem.Type = Network.GamePackets.FloorItem.Drop;
                            floorItem.OnFloor = Time32.Now;
                            floorItem.ItemColor = floorItem.Item.Color;
                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            while (Map.Npcs.ContainsKey(floorItem.UID))
                                floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            Map.AddFloorItem(floorItem);
                            SendScreenSpawn(floorItem);
                        }
                    }
                }
            }

            #endregion TC drop

            #region Bird drop

            if (Owner.MapID == 1015 && ServerBase.Kernel.PercentSuccess(200))
            {
                uint Uid2 = 0;
                byte type2 = (byte)ServerBase.Kernel.Random.Next(1, 3);
                switch (type2)
                {
                    case 1: Uid2 = 50; break;
                    case 2: Uid2 = 100; break;
                }
                if (killer.DoubleExperienceTime > 0)
                {
                    killer.ConquerPoints += Uid2;
                }
                else
                {
                    uint Uid = 0;
                    byte type = (byte)ServerBase.Kernel.Random.Next(1, 201);
                    switch (type)
                    {
                        case 1: Uid = 1000000; break;
                        case 2: Uid = 722114; break;
                        case 3: Uid = 1000000; break;
                        case 4: Uid = 1000000; break;
                        case 5: Uid = 1000000; break;
                        case 6: Uid = 1000000; break;
                        case 7: Uid = 1000000; break;
                        case 8: Uid = 1000000; break;
                        case 9: Uid = 1000000; break;
                        case 10: Uid = 1000000; break;
                        case 11: Uid = 1000000; break;
                        case 12: Uid = 1000000; break;
                        case 13: Uid = 1000000; break;
                        case 14: Uid = 722114; break;
                        case 15: Uid = 1000000; break;
                        case 16: Uid = 1000000; break;
                        case 17: Uid = 1000000; break;
                        case 18: Uid = 1000000; break;
                        case 19: Uid = 1000000; break;
                        case 20: Uid = 1000000; break;
                        case 21: Uid = 1000000; break;
                        case 22: Uid = 722114; break;
                        case 23: Uid = 1000000; break;
                        case 24: Uid = 1000000; break;
                        case 25: Uid = 1000000; break;
                        case 26: Uid = 1000000; break;
                        case 27: Uid = 1000000; break;
                        case 28: Uid = 1000000; break;
                        case 29: Uid = 1000000; break;
                        case 30: Uid = 1000000; break;
                        case 31: Uid = 1000000; break;
                        case 32: Uid = 1000000; break;
                        case 33: Uid = 722114; break;
                        case 34: Uid = 1000000; break;
                        case 35: Uid = 1000000; break;
                        case 36: Uid = 1000000; break;
                        case 37: Uid = 1000000; break;
                        case 38: Uid = 722114; break;
                        case 39: Uid = 1000000; break;
                        case 40: Uid = 1000000; break;
                        case 41: Uid = 1000000; break;
                        case 42: Uid = 1000000; break;
                        case 43: Uid = 1000000; break;
                        case 44: Uid = 1000000; break;
                        case 45: Uid = 1000000; break;
                        case 46: Uid = 1000000; break;
                        case 47: Uid = 1000000; break;
                        case 48: Uid = 1000000; break;
                        case 49: Uid = 1000000; break;
                        case 50: Uid = 1000000; break;
                        case 51: Uid = 1000000; break;
                        case 52: Uid = 1000000; break;
                        case 53: Uid = 1000000; break;
                        case 54: Uid = 1000000; break;
                        case 55: Uid = 1000000; break;
                        case 56: Uid = 1000000; break;
                        case 57: Uid = 1000000; break;
                        case 58: Uid = 1000000; break;
                        case 59: Uid = 1000000; break;
                        case 60: Uid = 1000000; break;
                        case 61: Uid = 1000000; break;
                        case 62: Uid = 1000000; break;
                        case 63: Uid = 722114; break;
                        case 64: Uid = 1000000; break;
                        case 65: Uid = 1000000; break;
                        case 66: Uid = 1000000; break;
                        case 67: Uid = 1000000; break;
                        case 68: Uid = 1000000; break;
                        case 69: Uid = 1000000; break;
                        case 70: Uid = 1000000; break;
                        case 71: Uid = 1000000; break;
                        case 72: Uid = 1000000; break;
                        case 73: Uid = 1000000; break;
                        case 74: Uid = 1000000; break;
                        case 75: Uid = 1000000; break;
                        case 76: Uid = 722114; break;
                        case 77: Uid = 1000000; break;
                        case 78: Uid = 1000000; break;
                        case 79: Uid = 1000000; break;
                        case 80: Uid = 1000000; break;
                        case 81: Uid = 1000000; break;
                        case 82: Uid = 1000000; break;
                        case 83: Uid = 1000000; break;
                        case 84: Uid = 1000000; break;
                        case 85: Uid = 1000000; break;
                        case 86: Uid = 1000000; break;
                        case 87: Uid = 1000000; break;
                        case 88: Uid = 1000000; break;
                        case 89: Uid = 1000000; break;
                        case 90: Uid = 722114; break;
                        case 91: Uid = 1000000; break;
                        case 92: Uid = 1000000; break;
                        case 93: Uid = 1000000; break;
                        case 94: Uid = 1000000; break;
                        case 95: Uid = 1000000; break;
                        case 96: Uid = 1000000; break;
                        case 97: Uid = 1000000; break;
                        case 98: Uid = 1000000; break;
                        case 99: Uid = 1000000; break;
                        case 100: Uid = 1000000; break;
                        case 101: Uid = 1000000; break;
                        case 102: Uid = 1000000; break;
                        case 103: Uid = 1000000; break;
                        case 104: Uid = 1000000; break;
                        case 105: Uid = 1000000; break;
                        case 106: Uid = 1000000; break;
                        case 107: Uid = 1000000; break;
                        case 108: Uid = 1000000; break;
                        case 109: Uid = 1000000; break;
                        case 110: Uid = 1000000; break;
                        case 111: Uid = 1000000; break;
                        case 112: Uid = 722114; break;
                        case 113: Uid = 1000000; break;
                        case 114: Uid = 1000000; break;
                        case 115: Uid = 1000000; break;
                        case 116: Uid = 1000000; break;
                        case 117: Uid = 1000000; break;
                        case 118: Uid = 1000000; break;
                        case 119: Uid = 1000000; break;
                        case 120: Uid = 1000000; break;
                        case 121: Uid = 1000000; break;
                        case 122: Uid = 1000000; break;
                        case 123: Uid = 1000000; break;
                        case 124: Uid = 1000000; break;
                        case 125: Uid = 1000000; break;
                        case 126: Uid = 1000000; break;
                        case 127: Uid = 1000000; break;
                        case 128: Uid = 1000000; break;
                        case 129: Uid = 1000000; break;
                        case 130: Uid = 1000000; break;
                        case 131: Uid = 1000000; break;
                        case 132: Uid = 1000000; break;
                        case 133: Uid = 1000000; break;
                        case 134: Uid = 1000000; break;
                        case 135: Uid = 1000000; break;
                        case 136: Uid = 1000000; break;
                        case 137: Uid = 1000000; break;
                        case 138: Uid = 1000000; break;
                        case 139: Uid = 722114; break;
                        case 140: Uid = 1000000; break;
                        case 141: Uid = 1000000; break;
                        case 142: Uid = 1000000; break;
                        case 143: Uid = 1000000; break;
                        case 144: Uid = 1000000; break;
                        case 145: Uid = 1000000; break;
                        case 146: Uid = 1000000; break;
                        case 147: Uid = 1000000; break;
                        case 148: Uid = 1000000; break;
                        case 149: Uid = 722114; break;
                        case 150: Uid = 1000000; break;
                        case 151: Uid = 1000000; break;
                        case 152: Uid = 1000000; break;
                        case 153: Uid = 1000000; break;
                        case 154: Uid = 1000000; break;
                        case 155: Uid = 1000000; break;
                        case 156: Uid = 1000000; break;
                        case 157: Uid = 1000000; break;
                        case 158: Uid = 1000000; break;
                        case 159: Uid = 1000000; break;
                        case 160: Uid = 1000000; break;
                        case 161: Uid = 1000000; break;
                        case 162: Uid = 722114; break;
                        case 163: Uid = 1000000; break;
                        case 164: Uid = 1000000; break;
                        case 165: Uid = 1000000; break;
                        case 166: Uid = 1000000; break;
                        case 167: Uid = 1000000; break;
                        case 168: Uid = 1000000; break;
                        case 169: Uid = 1000000; break;
                        case 170: Uid = 722114; break;
                        case 171: Uid = 1000000; break;
                        case 172: Uid = 1000000; break;
                        case 173: Uid = 1000000; break;
                        case 174: Uid = 1000000; break;
                        case 175: Uid = 1000000; break;
                        case 176: Uid = 1000000; break;
                        case 177: Uid = 1000000; break;
                        case 178: Uid = 1000000; break;
                        case 179: Uid = 1000000; break;
                        case 180: Uid = 1000000; break;
                        case 181: Uid = 1000000; break;
                        case 182: Uid = 722114; break;
                        case 183: Uid = 1000000; break;
                        case 184: Uid = 1000000; break;
                        case 185: Uid = 1000000; break;
                        case 186: Uid = 1000000; break;
                        case 187: Uid = 1000000; break;
                        case 188: Uid = 1000000; break;
                        case 189: Uid = 1000000; break;
                        case 190: Uid = 1000000; break;
                        case 191: Uid = 1000000; break;
                        case 192: Uid = 1000000; break;
                        case 193: Uid = 722114; break;
                        case 194: Uid = 1000000; break;
                        case 195: Uid = 1000000; break;
                        case 196: Uid = 1000000; break;
                        case 197: Uid = 1000000; break;
                        case 198: Uid = 1000000; break;
                        case 199: Uid = 1000000; break;
                        case 200: Uid = 1000000; break;
                    }
                    if (Uid != 0)
                    {
                        ushort X = Owner.X, Y = Owner.Y;
                        Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                        if (Map.SelectCoordonates(ref X, ref Y))
                        {
                            Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                            floorItem.Item = new Network.GamePackets.ConquerItem(true);
                            floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                            floorItem.Item.ID = (uint)Uid;
                            floorItem.Item.MaximDurability = 65355;
                            floorItem.Item.MobDropped = true;
                            floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                            floorItem.ItemID = (uint)Uid;
                            floorItem.MapID = Owner.MapID;
                            floorItem.MapObjType = Game.MapObjectType.Item;
                            floorItem.X = X;
                            floorItem.Y = Y;
                            floorItem.Type = Network.GamePackets.FloorItem.Drop;
                            floorItem.OnFloor = Time32.Now;
                            floorItem.ItemColor = floorItem.Item.Color;
                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            while (Map.Npcs.ContainsKey(floorItem.UID))
                                floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            Map.AddFloorItem(floorItem);
                            SendScreenSpawn(floorItem);
                        }
                    }
                }
            }

            #endregion TC drop

            #endregion



            if (killer.Name.Contains("Guard"))
            {
                goto kimoz;
            }
            if (Owner.Name.Contains("Guard"))
            {

                return;
            }
            
            if (Name == "Naga")
            {
                {
                    killer.DisKO += 1;
                    killer.Owner.Send(new Message("Congratulations! You have got 1 Kill you have Now " + killer.DisKO + " DisKo Points", System.Drawing.Color.Azure, Message.Monster));
                   // return;
                }
            }

            if (ID == killer.kilid)
            {
                if (killer.Owner.Inventory.Contains(750000, 1))
                {

                    killer.Status4 += 1;
                }
            }
            if (ID == 83)
            {
                if (killer.kilid == 58)
                {
                    if (killer.Owner.Inventory.Contains(750000, 1))
                    {

                        killer.Status4 += 1;
                    }
                }
            }
            if (ID == 8319)
            {
                if (killer.kilid == 58)
                {
                    if (killer.Owner.Inventory.Contains(750000, 1))
                    {

                        killer.Status4 += 1;
                    }
                }
            }
            #region CPs  20

            if (ServerBase.Kernel.Rate(100))
            {
                killer.ConquerPoints += 100;
                killer.Owner.Send(new Network.GamePackets.Message("You have found 100 Cps ! BLackLisT ", System.Drawing.Color.Yellow, 2005));
            }

            #endregion CPs  50

            #region Error

            if (this.Name == "FireDevil(Error)")
            {
                uint Uid = 0;
                byte type = (byte)ServerBase.Kernel.Random.Next(1, 21);
                switch (type)
                {
                    case 1: Uid = 722158; break;
                    case 2: Uid = 722134; break;
                    case 3: Uid = 722133; break;
                    case 4: Uid = 722132; break;
                    case 5: Uid = 722131; break;
                    case 6: Uid = 722131; break;
                    case 7: Uid = 722132; break;
                    case 8: Uid = 722133; break;
                    case 9: Uid = 722134; break;
                    case 10: Uid = 722158; break;
                    case 11: Uid = 721962; break;
                    case 12: Uid = 722131; break;
                    case 13: Uid = 722132; break;
                    case 14: Uid = 722133; break;
                    case 15: Uid = 722134; break;
                    case 16: Uid = 722158; break;
                    case 17: Uid = 722134; break;
                    case 18: Uid = 722133; break;
                    case 19: Uid = 722132; break;
                    case 20: Uid = 722131; break;
                    case 21: Uid = 721962; break;
                }

                killer.Owner.Inventory.Add(Uid, 0, 1);

                //killer.ConquerPoints += 100;
                //killer.SubClasses.StudyPoints += 100;
                ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("  " + killer.Name + "Has Killed " + Owner.Name + "", System.Drawing.Color.Yellow, 2011), ServerBase.Kernel.GamePool.Values);
            }

            #endregion LavaBeast  SwordMaster

            #region Error1

            if (this.Name == "SkeletonA(Error)")
            {
                uint Uid = 0;
                byte type = (byte)ServerBase.Kernel.Random.Next(1, 21);
                switch (type)
                {
                    case 1: Uid = 722316; break;
                    case 2: Uid = 722316; break;
                    case 3: Uid = 722316; break;
                    case 4: Uid = 722316; break;
                    case 5: Uid = 722316; break;
                    case 6: Uid = 722316; break;
                    case 7: Uid = 722316; break;
                    case 8: Uid = 722316; break;
                    case 9: Uid = 722316; break;
                    case 10: Uid = 722316; break;
                    case 11: Uid = 722316; break;
                    case 12: Uid = 722316; break;
                    case 13: Uid = 722316; break;
                    case 14: Uid = 722317; break;
                    case 15: Uid = 722317; break;
                    case 16: Uid = 722317; break;
                    case 17: Uid = 722318; break;
                    case 18: Uid = 722318; break;
                    case 19: Uid = 722319; break;
                    case 20: Uid = 722319; break;
                    case 21: Uid = 722320; break;
                }

                killer.Owner.Inventory.Add(Uid, 0, 1);

                //killer.ConquerPoints += 100;
                //killer.SubClasses.StudyPoints += 100;
                ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("  " + killer.Name + "Has Killed " + Owner.Name + "", System.Drawing.Color.Yellow, 2011), ServerBase.Kernel.GamePool.Values);
            }

            #endregion LavaBeast  SwordMaster

            if (Name == "ThrillingSpook")
            {
                {
                    killer.ConquerPoints += Database.rates.ThrillingSpook;
                    PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Congratulations! " + killer.Name + " Has Killed ThrillingSpook and got " + Database.rates.ThrillingSpook + " cps!", System.Drawing.Color.IndianRed, 2600), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                   // return;
                }
            }
            if (Name == "LavaBeast")
            {
                {
                    killer.SubClasses.StudyPoints += 20;
                    killer.Owner.Send(new Message("Congratulations You have got 20 StudyPoints!", System.Drawing.Color.Red, Network.GamePackets.Message.study));
                    PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Congratulations! " + killer.Name + " Has Killed LavaBeast and got 20 StudyPoints!", System.Drawing.Color.White, 2600), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                    Data data = new Data(true);
                    data.ID = Data.OpenCustom;
                    data.UID =  killer.Owner.Entity.UID;
                    data.TimeStamp = Time32.Now;
                    data.dwParam = 3179;
                    data.wParam1 =  killer.Owner.Entity.X;
                    data.wParam2 =  killer.Owner.Entity.Y;
                    killer.Owner.Send(data);
                   // return;
                }
            }
            if (Name == "Temptress")
            {
                {
                    killer.DisKO += 1;
                    killer.Owner.Send(new Message("Congratulations! You have got 1 Kill you have Now " + killer.DisKO + " DisKo Points", System.Drawing.Color.Azure, Message.Monster));
                   // return;
                }
            }
            if (Name == "Centicore")
            {
                {
                    killer.DisKO += 1;
                    killer.Owner.Send(new Message("Congratulations! You have got 1 Kill you have Now " + killer.DisKO + " DisKo Points", System.Drawing.Color.Azure, Message.Monster));
                  //  return;
                }
            }
            if (Name == "DemonCave[1]")
            {
                {
                    killer.DemonCave1 += 1;
                    killer.Owner.Send(new Message("Congratulations! You have got 1 Kill you have Now " + killer.DemonCave1 + " Stage[1] Points", System.Drawing.Color.Azure, Message.Monster));
                  //  return;
                }
            }
            if (Name == "DemonCave[2]")
            {
                {
                    killer.DemonCave2 += 1;
                    killer.Owner.Send(new Message("Congratulations! You have got 1 Kill you have Now " + killer.DemonCave2 + " Stage[2] Points", System.Drawing.Color.Azure, Message.Monster));
                   // return;
                }
            }
            if (Name == "DemonCave[3]")
            {
                {
                    killer.DemonCave3 += 1;
                    killer.Owner.Send(new Message("Congratulations! You have got 1 Kill you have Now " + killer.DemonCave3 + " Stage[3] Points", System.Drawing.Color.Azure, Message.Monster));
                   // return;
                }
            }
            if (Name == "HellTroll")
            {
                {
                    killer.DisKO += 3;
                    killer.Owner.Send(new Message("Congratulations! You have got 3 Kill you have Now " + killer.DisKO + " DisKo Points", System.Drawing.Color.Azure, Message.Monster));
                  //  return;
                }
            }
            if (Name == "SnowBanshee")
            {
                {
                    killer.Owner.Inventory.Add(723694, 0, 1);
                    killer.ConquerPoints += Database.rates.SnowBanshe;
                    killer.Owner.Send(new Message("Congratulations! " + killer.Name + " has defeated SnowBanshee in BirdIsland and got PermanentStone and " + Database.rates.SnowBanshe + " CPS!", System.Drawing.Color.Azure, Message.Monster));
                   // return;
                }
            }
            if (Name == "SwordMaster")
            {
                {
                    uint randsouls = (uint)PhoenixProject.ServerBase.Kernel.Random.Next(1, (int)Database.DROP_SOULS.Count_souls);
                    //Database.Monster.Souls[randsouls].item_id
                    uint ItemID = Database.DROP_SOULS.Souls[randsouls].item_id;
                    var infos = Database.ConquerItemInformation.BaseInformations[ItemID];
                    if (infos == null) return;
                    ushort X = Owner.X, Y = Owner.Y;
                    Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                    if (Map.SelectCoordonates(ref X, ref Y))
                    {
                        Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                        floorItem.Item = new Network.GamePackets.ConquerItem(true);
                        floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                        floorItem.Item.ID = ItemID;
                        floorItem.Item.MaximDurability = infos.Durability;

                        floorItem.Item.Durability = (ushort)(ServerBase.Kernel.Random.Next(infos.Durability / 10));

                        if (!Network.PacketHandler.IsEquipment(ItemID) && infos.ConquerPointsWorth == 0)
                        {
                            floorItem.Item.StackSize = 1;
                            floorItem.Item.MaxStackSize = infos.StackSize;
                        }
                        floorItem.Item.MobDropped = true;
                        floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                        floorItem.ItemID = ItemID;
                        floorItem.MapID = Owner.MapID;
                        floorItem.MapObjType = Game.MapObjectType.Item;
                        floorItem.X = X;
                        floorItem.Y = Y;
                        floorItem.Owner = killer.Owner;
                        floorItem.Type = Network.GamePackets.FloorItem.Drop;
                        floorItem.OnFloor = Time32.Now;
                        floorItem.ItemColor = floorItem.Item.Color;
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;

                        while (Map.Npcs.ContainsKey(floorItem.UID))
                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                        Map.AddFloorItem(floorItem);
                        SendScreenSpawn(floorItem);
                    }
                    killer.Owner.Send(new Message("Congratulations! " + killer.Name + " has defeated SwordMaster in His/Her House and Dropped " + Database.ConquerItemInformation.BaseInformations[ItemID].Name + " !", System.Drawing.Color.Azure, Message.Monster));
                   // return;
                }
            }
            if (Name == "GoldenOctopus")
            {
                {
                    killer.ConquerPoints += 20000;
                    killer.Owner.Send(new Message("Congratulations! " + killer.Name + " has defeated GoldenOctopus in PirateSea Map and got 20,000 CPS!", System.Drawing.Color.Azure, Message.Monster));
                  ///  return;
                }
            }
            if (Name == "TeratoDragon")
            {
                byte times = (byte)ServerBase.Kernel.Random.Next(1, 3);
                byte ref_times = (byte)ServerBase.Kernel.Random.Next(1, 6);
                for (byte i = 0; i < times; i++)
                {
                    uint Uid = 0;
                    byte type = (byte)ServerBase.Kernel.Random.Next(1, 50);
                    switch (type)
                    {
                        case 1:
                            Uid = 800320;
                            break;

                        case 2:
                            Uid = 822054;
                            break;

                        case 3:
                            Uid = 800110;
                            break;

                        case 4:
                            Uid = 820056;
                            break;

                        case 5:
                            Uid = 822056;
                            break;

                        case 6:
                            Uid = 822057;
                            break;

                        case 7:
                            Uid = 822053;
                            break;

                        case 8:
                            Uid = 800019;
                            break;

                        case 9:
                            Uid = 800050;
                            break;

                        case 10:
                            Uid = 800015;
                            break;

                        case 11:
                            Uid = 800090;
                            break;

                        case 12:
                            Uid = 800513;
                            break;

                        case 13:
                            Uid = 800017;
                            break;

                        case 14:
                            Uid = 800071;
                            break;

                        case 15:
                            Uid = 800016;
                            break;

                        case 16:
                            Uid = 823051;
                            break;

                        case 17:
                            Uid = 800130;
                            break;

                        case 18:
                            Uid = 800140;
                            break;

                        case 19:
                            Uid = 800141;
                            break;

                        case 20:
                            Uid = 800200;
                            break;

                        case 21:
                            Uid = 800310;
                            break;

                        case 22:
                            Uid = 800014;
                            break;

                        case 23:
                            Uid = 800214;
                            break;

                        case 24:
                            Uid = 800230;
                            break;

                        case 25:
                            Uid = 800414;
                            break;

                        case 26:
                            Uid = 822052;
                            break;

                        case 27:
                            Uid = 800420;
                            break;

                        case 28:
                            Uid = 800401;
                            break;

                        case 29:
                            Uid = 800512;
                            break;

                        case 30:
                            Uid = 823043;
                            break;

                        case 31:
                            Uid = 800514;
                            break;

                        case 32:
                            Uid = 800520;
                            break;

                        case 33:
                            Uid = 800521;
                            break;

                        case 34:
                            Uid = 800613;
                            break;

                        case 35:
                            Uid = 800614;
                            break;

                        case 36:
                            Uid = 800615;
                            break;

                        case 37:
                            Uid = 824001;
                            break;

                        case 38:
                            Uid = 800617;
                            break;

                        case 39:
                            Uid = 800720;
                            break;

                        case 40:
                            Uid = 800721;
                            break;

                        case 41:
                            Uid = 800070;
                            break;

                        case 42:
                            Uid = 800723;
                            break;

                        case 43:
                            Uid = 800724;
                            break;

                        case 44:
                            Uid = 800018;
                            break;

                        case 45:
                            Uid = 820001;
                            break;

                        case 46:
                            Uid = 820052;
                            break;

                        case 47:
                            Uid = 820053;
                            break;

                        case 48:
                            Uid = 820054;
                            break;

                        case 49:
                            Uid = 820055;
                            break;

                        case 50:
                            Uid = 800722;
                            break;
                    }

                    if (Uid != 0)
                    {
                        ushort X = Owner.X, Y = Owner.Y;
                        Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                        if (Map.SelectCoordonates(ref X, ref Y))
                        {
                            killer.Owner.Inventory.Add(Uid, 0, 1);
                            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Congratulations! " + killer.Name + " Has Defeated " + Name + " and dropped! " + Database.ConquerItemInformation.BaseInformations[Uid].Name + " and "+Database.rates.TeratoDragon+" CPS!", System.Drawing.Color.White, 2600), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            killer.ConquerPoints += Database.rates.TeratoDragon;
                           // return;
                        }
                    }
                }
            }

            
            kimoz:

            byte morepercent = 0;
            byte lessrate = 0;
            if (killer.VIPLevel > 0)
                morepercent = (byte)(killer.VIPLevel * 5);
            if (killer.Level <= 10 && killer.MapID == 1002)
                morepercent += 100;
            if (killer.VIPLevel != 6 && killer.Class >= 40 && killer.Class <= 45)
                lessrate = 3;
            if (killer.VIPLevel != 6 && killer.Level >= 132 && killer.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                lessrate = 3;

            if (ServerBase.Kernel.Rate(ServerBase.Constants.MoneyDropRate - lessrate + morepercent))
            {

                uint amount = (uint)ServerBase.Kernel.Random.Next(1000, 5000);
               // amount *= ServerBase.Constants.MoneyDropMultiple;

                if (amount > 1000000)
                    amount = 5000;

                if (amount == 0)
                    return;
               /* if (killer.VIPLevel > 0)
                {
                    int percent = 10;
                    percent += killer.VIPLevel * 5 - 5;
                    amount += (uint)(amount * percent / 100);
                }
                if (killer.VIPLevel > 4)
                {
                    killer.Money += amount;
                    goto next;
                }*/
                uint ItemID = Network.PacketHandler.MoneyItemID(amount);
                ushort X = Owner.X, Y = Owner.Y;
                Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Money;
                    floorItem.Value = amount;
                    floorItem.ItemID = ItemID;
                    floorItem.MapID = Owner.MapID;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    SendScreenSpawn(floorItem);
                }
            }
       // next:

            if (ServerBase.Kernel.Rate(ServerBase.Constants.ConquerPointsDropRate - lessrate))
            {
                // uint amount = (uint)ServerBase.Kernel.Random.Next((int)((Level / 4) * ServerBase.Constants.ConquerPointsDropMultiple), (int)((Level / 2) * ServerBase.Constants.ConquerPointsDropMultiple));
                // if (amount == 0)
                //     amount = 2;
                // if (amount > 300)
                //      amount = 10;
                //  amount /= 2;

                uint amount = (uint)Level /PhoenixProject.Database.rates.CpsMethodNum;
                if (amount < PhoenixProject.Database.rates.minicps)
                    amount = PhoenixProject.Database.rates.minicps;
                if (amount > PhoenixProject.Database.rates.maxcps)
                    amount = PhoenixProject.Database.rates.maxcps;
                // if (killer.VIPLevel > 4)
                // {
                if (killer != null && killer.Owner != null)
                {
                    if (killer.Owner.Map.BaseID == 1354)
                    {
                        amount = PhoenixProject.Database.rates.maxcps;
                    }
                }
                if (killer != null && killer.Owner != null)
                {
                    //killer.Owner.Send(ServerBase.Constants.PickupConquerPoints(amount));
                    killer.ConquerPoints += (uint)amount;
                    killer.Owner.Send(new Network.GamePackets.Message("You received " + amount + " ConquerPoints! for Kill " + Name + "", System.Drawing.Color.Red, Network.GamePackets.Message.TopLeft).ToArray());
                   // return;
                }
                //  }

                #region CPBag

                //uint ItemID = 729911;
                //ushort X = Owner.X, Y = Owner.Y;
                //Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                //if (Map.SelectCoordonates(ref X, ref Y))
                //{
                //    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                //    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                //    floorItem.Value = amount;
                //    floorItem.ItemID = ItemID;
                //    floorItem.MapID = Owner.MapID;
                //    floorItem.MapObjType = Game.MapObjectType.Item;
                //    floorItem.X = X;
                //    floorItem.Y = Y;
                //    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                //    floorItem.OnFloor = Time32.Now;
                //    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                //    while (Map.Npcs.ContainsKey(floorItem.UID))
                //        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                //    Map.AddFloorItem(floorItem);
                //    SendScreenSpawn(floorItem);
                //}
                #endregion
            }
            else if (ServerBase.Kernel.Rate(OwnItemRate + morepercent) && OwnItemID != 0)
            {
                if (killer.VIPLevel > 6)
                {
                    if (killer.Owner.Inventory.Count <= 39)
                    {
                        killer.Owner.Inventory.Add((uint)OwnItemID, 0, 1);
                        //return;
                    }
                }
                var infos = Database.ConquerItemInformation.BaseInformations[(uint)OwnItemID];
                ushort X = Owner.X, Y = Owner.Y;
                Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.Item = new Network.GamePackets.ConquerItem(true);
                    floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                    floorItem.Item.ID = (uint)OwnItemID;
                    floorItem.Item.MaximDurability = infos.Durability;
                    if (!Network.PacketHandler.IsEquipment(OwnItemID) && infos.ConquerPointsWorth == 0)
                    {
                        floorItem.Item.StackSize = 1;
                        floorItem.Item.MaxStackSize = infos.StackSize;
                    }
                    floorItem.Item.MobDropped = true;
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                    floorItem.ItemID = (uint)OwnItemID;
                    floorItem.MapID = Owner.MapID;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Owner = killer.Owner;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.ItemColor = floorItem.Item.Color;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    SendScreenSpawn(floorItem);
                }
            }
            else if (ServerBase.Kernel.Rate(ServerBase.Constants.ItemDropRate + morepercent))
            {
                int quality = 3;
                for (int count = 0; count < 5; count++)
                {
                    int rate = int.Parse(ServerBase.Constants.ItemDropQualityRates[count]);
                    if (ServerBase.Kernel.Rate(rate, 1000))
                    {
                        quality = count + 5;
                        break;
                    }
                }
                int times = 50;
                byte lvl = Owner.Level;
                if (LabirinthDrop)
                    lvl = 20;
                List<uint> itemdroplist = ItemDropCache[lvl];
                if (Boss)
                    itemdroplist = SoulItemCache[lvl];
            retry:
                times--;
                int generateItemId = ServerBase.Kernel.Random.Next(itemdroplist.Count);
                uint id = itemdroplist[generateItemId];
                if (!Boss)
                {
                    if (Database.ConquerItemInformation.BaseInformations[id].Level > 121 && times > 0)
                        goto retry;
                    id = (id / 10) * 10 + (uint)quality;
                }
                if (!Database.ConquerItemInformation.BaseInformations.ContainsKey(id))
                {
                    id = itemdroplist[generateItemId];
                }
                if (killer.VIPLevel > 6)
                {
                    if (killer.Owner.Inventory.Count <= 39)
                    {
                        if (id % 10 > 7)
                        {
                            killer.Owner.Inventory.Add(id, 0, 1);
                            return;
                        }
                    }
                }
                var infos = Database.ConquerItemInformation.BaseInformations[id];
                ushort X = Owner.X, Y = Owner.Y;
                Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.Item = new Network.GamePackets.ConquerItem(true);
                    floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                    floorItem.Item.ID = id;
                    floorItem.Item.MaximDurability = infos.Durability;
                    if (quality >= 6)
                        floorItem.Item.Durability = (ushort)(infos.Durability - ServerBase.Kernel.Random.Next(500));
                    else
                        floorItem.Item.Durability = (ushort)(ServerBase.Kernel.Random.Next(infos.Durability / 10));
                    if (!Network.PacketHandler.IsEquipment(id) && infos.ConquerPointsWorth == 0)
                    {
                        floorItem.Item.StackSize = 1;
                        floorItem.Item.MaxStackSize = infos.StackSize;
                    }
                    floorItem.Item.MobDropped = true;
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                    floorItem.ItemID = id;
                    floorItem.MapID = Owner.MapID;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.Owner = killer.Owner;
                    floorItem.ItemColor = floorItem.Item.Color;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    SendScreenSpawn(floorItem);
                }
            }
            else if (ServerBase.Kernel.Rate(20 + morepercent))
            {
                if (HPPotionID == 0)
                    return;
                var infos = Database.ConquerItemInformation.BaseInformations[(uint)HPPotionID];
                ushort X = Owner.X, Y = Owner.Y;
                Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.Item = new Network.GamePackets.ConquerItem(true);
                    floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                    floorItem.Item.ID = (uint)HPPotionID;
                    floorItem.Item.MobDropped = true;
                    floorItem.Item.MaximDurability = infos.Durability;
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                    if (!Network.PacketHandler.IsEquipment(HPPotionID))
                    {
                        floorItem.Item.StackSize = 1;
                        floorItem.Item.MaxStackSize = infos.StackSize;
                    }
                    floorItem.ItemID = (uint)HPPotionID;
                    floorItem.MapID = Owner.MapID;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.Owner = killer.Owner;
                    floorItem.ItemColor = floorItem.Item.Color;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    SendScreenSpawn(floorItem);
                }
            }
            else if (ServerBase.Kernel.Rate(20 + morepercent))
            {
                if (MPPotionID == 0)
                    return;
                var infos = Database.ConquerItemInformation.BaseInformations[(uint)MPPotionID];
                ushort X = Owner.X, Y = Owner.Y;
                Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.Item = new Network.GamePackets.ConquerItem(true);
                    floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                    floorItem.Item.ID = (uint)MPPotionID;
                    floorItem.Item.MaximDurability = infos.Durability;
                    floorItem.Item.MobDropped = true;
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                    if (!Network.PacketHandler.IsEquipment(MPPotionID))
                    {
                        floorItem.Item.StackSize = 1;
                        floorItem.Item.MaxStackSize = infos.StackSize;
                    }
                    floorItem.ItemID = (uint)MPPotionID;
                    floorItem.MapID = Owner.MapID;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.Owner = killer.Owner;
                    floorItem.ItemColor = floorItem.Item.Color;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    SendScreenSpawn(floorItem);
                }
            }
            else
            {
                foreach (SpecialItemDrop sitem in SpecialItemDropList)
                {
                    if (sitem.Map != 0 && Owner.MapID != sitem.Map)
                        continue;
                    if (ServerBase.Kernel.Rate(sitem.Rate + morepercent, sitem.Discriminant))
                    {
                        if (killer.VIPLevel > 6)
                        {
                            if (killer.Owner.Inventory.Count <= 39)
                            {
                                killer.Owner.Inventory.Add((uint)sitem.ItemID, 0, 1);
                                return;
                            }
                        }
                        var infos = Database.ConquerItemInformation.BaseInformations[(uint)sitem.ItemID];
                        ushort X = Owner.X, Y = Owner.Y;
                        Game.Map Map = ServerBase.Kernel.Maps[Owner.MapID];
                        if (Map.SelectCoordonates(ref X, ref Y))
                        {
                            Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                            floorItem.Item = new Network.GamePackets.ConquerItem(true);
                            floorItem.Item.Color = (PhoenixProject.Game.Enums.Color)ServerBase.Kernel.Random.Next(4, 8);
                            floorItem.Item.ID = (uint)sitem.ItemID;
                            floorItem.Item.MaximDurability = infos.Durability;
                            floorItem.Item.MobDropped = true;
                            if (!Network.PacketHandler.IsEquipment(sitem.ItemID) && infos.ConquerPointsWorth == 0)
                            {
                                floorItem.Item.StackSize = 1;
                                floorItem.Item.MaxStackSize = infos.StackSize;
                            }
                            floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.Item;
                            floorItem.ItemID = (uint)sitem.ItemID;
                            floorItem.MapID = Owner.MapID;
                            floorItem.MapObjType = Game.MapObjectType.Item;
                            floorItem.X = X;
                            floorItem.Y = Y;
                            floorItem.Type = Network.GamePackets.FloorItem.Drop;
                            floorItem.OnFloor = Time32.Now;
                            floorItem.ItemColor = floorItem.Item.Color;
                            floorItem.Owner = killer.Owner;
                            floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            while (Map.Npcs.ContainsKey(floorItem.UID))
                                floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                            Map.AddFloorItem(floorItem);
                            SendScreenSpawn(floorItem);
                            break;
                        }
                    }
                }
            }
        }

        public static SafeDictionary<uint, MonsterInformation> MonsterInfos = new SafeDictionary<uint, MonsterInformation>(8000);

        public static void Load()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("specialdrops");
            PhoenixProject.Database.MySqlReader rdr = new PhoenixProject.Database.MySqlReader(cmd);
            while (rdr.Read())
            {
                SpecialItemDrop sitem = new SpecialItemDrop();
                sitem.ItemID = rdr.ReadInt32("itemid");
                sitem.Rate = rdr.ReadInt32("rate");
                sitem.Discriminant = rdr.ReadInt32("discriminant");
                sitem.Map = rdr.ReadUInt64("map");
                SpecialItemDropList.Add(sitem);
            }
            rdr.Close();
            rdr.Dispose();
            MySqlCommand command = new MySqlCommand(MySqlCommandType.SELECT);
            command.Select("monsterinfos");
            PhoenixProject.Database.MySqlReader reader = new PhoenixProject.Database.MySqlReader(command);
            while (reader.Read())
            {
                MonsterInformation mf = new MonsterInformation();
                mf.ID = reader.ReadUInt32("id");
                mf.Name = reader.ReadString("name");
                mf.Mesh = reader.ReadUInt16("lookface");
                mf.Level = reader.ReadByte("level");
                mf.Hitpoints = reader.ReadUInt32("life");
                ServerBase.IniFile IniFile = new ServerBase.IniFile(ServerBase.Constants.MonstersPath);
                if (IniFile.ReadString(mf.Name, "MaxLife") != "")
                {
                    if (uint.Parse(IniFile.ReadString(mf.Name, "MaxLife")) != 0)
                    {
                        mf.Hitpoints = uint.Parse(IniFile.ReadString(mf.Name, "MaxLife"));
                        byte boss = byte.Parse(IniFile.ReadString(mf.Name, "Boss"));
                        if (boss == 0)
                            mf.Boss = false;
                        else mf.Boss = true;
                    }
                }
                mf.ViewRange = reader.ReadUInt16("view_range");
                mf.AttackRange = reader.ReadUInt16("attack_range");
                mf.AttackType = reader.ReadByte("attack_user");
                mf.MinAttack = reader.ReadUInt32("attack_min");
                mf.MaxAttack = reader.ReadUInt32("attack_max");
                mf.SpellID = reader.ReadUInt16("magic_type");
                mf.MoveSpeed = reader.ReadInt32("move_speed");
                mf.RunSpeed = reader.ReadInt32("run_speed");
                mf.OwnItemID = reader.ReadInt32("ownitem");
                mf.HPPotionID = reader.ReadInt32("drop_hp");
                mf.MPPotionID = reader.ReadInt32("drop_mp");
                mf.OwnItemRate = reader.ReadInt32("ownitemrate");
                mf.AttackSpeed = reader.ReadInt32("attack_speed");
                mf.ExtraExperience = reader.ReadUInt32("extra_exp");
                uint MoneyDropAmount = reader.ReadUInt16("level");
                if (MoneyDropAmount != 0)
                {
                    mf.MaxMoneyDropAmount = MoneyDropAmount * 25;
                    if (mf.MaxMoneyDropAmount != 0)
                        mf.MinMoneyDropAmount = 1;
                }
                if (mf.MoveSpeed <= 500)
                    mf.MoveSpeed += 500;
                if (mf.AttackSpeed <= 500)
                    mf.AttackSpeed += 500;
                MonsterInfos.Add(mf.ID, mf);
                byte lvl = mf.Level;
                if (mf.Name == "Slinger" ||
                    mf.Name == "GoldGhost" ||
                    mf.Name == "AgileRat" ||
                    mf.Name == "Bladeling" ||
                    mf.Name == "BlueBird" ||
                    mf.Name == "BlueFiend" ||
                    mf.Name == "MinotaurL120")
                {
                    mf.LabirinthDrop = true;
                    lvl = 20;
                }
                if (!ItemDropCache.ContainsKey(lvl))
                {
                    List<uint> itemdroplist = new List<uint>();
                    foreach (ConquerItemBaseInformation itemInfo in ConquerItemInformation.BaseInformations.Values)
                    {

                        if (itemInfo.ID >= 800000 && itemInfo.ID <= 824014)
                            continue;
                        ushort position = Network.PacketHandler.ItemPosition(itemInfo.ID);
                        if (Network.PacketHandler.IsArrow(itemInfo.ID) || itemInfo.Level == 0 || itemInfo.Level > 121)
                            continue;
                        if (position < 9 && position != 7)
                        {
                            if (itemInfo.Level == 100)
                                if (itemInfo.Name.Contains("Dress"))
                                    continue;
                            if (itemInfo.Level > 121)
                                continue;
                            int diff = (int)lvl - (int)itemInfo.Level;
                            if (!(diff > 10 || diff < -10))
                            {
                                itemdroplist.Add(itemInfo.ID);
                            }
                        }
                        if (position == 10 || position == 11 && lvl >= 70)
                            itemdroplist.Add(itemInfo.ID);
                    }
                    ItemDropCache.Add(lvl, itemdroplist);
                }
                if (mf.Boss)
                {
                    List<uint> itemdroplist = new List<uint>();
                    foreach (ConquerItemBaseInformation itemInfo in ConquerItemInformation.BaseInformations.Values)
                    {
                        if (itemInfo.ID >= 800000 && itemInfo.ID <= 824014)
                        {
                            if (itemInfo.PurificationLevel <= 3)
                            {
                                int diff = (int)mf.Level - (int)itemInfo.Level;
                                if (!(diff > 10 || diff < -10))
                                {
                                    itemdroplist.Add(itemInfo.ID);
                                }
                            }
                        }
                    }
                    SoulItemCache.Add(lvl, itemdroplist);
                }
            }

            reader.Close();
            reader.Dispose();
            Console.WriteLine("Monster information loaded.");
            Console.WriteLine("Monster drops generated.");
        }

        public MonsterInformation Copy()
        {
            MonsterInformation mf = new MonsterInformation();
            mf.ID = this.ID;
            mf.Name = this.Name;
            mf.Mesh = this.Mesh;
            mf.Level = this.Level;
            mf.Hitpoints = this.Hitpoints;
            mf.ViewRange = this.ViewRange;
            mf.AttackRange = this.AttackRange;
            mf.AttackType = this.AttackType;
            mf.MinAttack = this.MinAttack;
            mf.MaxAttack = this.MaxAttack;
            mf.SpellID = this.SpellID;
            mf.MoveSpeed = this.MoveSpeed;
            mf.RunSpeed = this.RunSpeed;
            mf.AttackSpeed = this.AttackSpeed;
            mf.BoundX = this.BoundX;
            mf.BoundY = this.BoundY;
            mf.BoundCX = this.BoundCX;
            mf.BoundCY = this.BoundCY;
            mf.RespawnTime = this.RespawnTime;
            mf.ExtraExperience = this.ExtraExperience;
            mf.MaxMoneyDropAmount = this.MaxMoneyDropAmount;
            mf.MinMoneyDropAmount = this.MinMoneyDropAmount;
            mf.OwnItemID = this.OwnItemID;
            mf.HPPotionID = this.HPPotionID;
            mf.MPPotionID = this.MPPotionID;
            mf.OwnItemRate = this.OwnItemRate;
            mf.LabirinthDrop = this.LabirinthDrop;
            mf.Boss = this.Boss;
            return mf;
        }
    }
}
