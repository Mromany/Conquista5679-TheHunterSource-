using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PhoenixProject.Database;

using System.Collections.Concurrent;

namespace PhoenixProject.ServerBase
{
    public class Kernel
    {
        public static Dictionary<UInt32, Refinery.RefineryBoxes> DatabaseRefineryBoxes =
  new Dictionary<UInt32, Refinery.RefineryBoxes>();
        public static Dictionary<uint, PhoenixProject.Database.DROP_SOULS.Items_drop2> JarItem = new Dictionary<uint, PhoenixProject.Database.DROP_SOULS.Items_drop2>();
        public static Dictionary<UInt32, Refinery.RefineryItem> DatabaseRefinery = new Dictionary<UInt32, Refinery.RefineryItem>();
        public static List<string> VotsAdress = new List<string>();
        public static Dictionary<uint, PhoenixProject.Network.GamePackets.Clan> Clans = new Dictionary<uint, PhoenixProject.Network.GamePackets.Clan>(100000);
        public static Dictionary<uint, Game.Clans> ServerClans = new Dictionary<uint, PhoenixProject.Game.Clans>();
        public static Dictionary<string, PhoenixProject.Game.ConquerStructures.PlayersVot> VotePool = new Dictionary<string, PhoenixProject.Game.ConquerStructures.PlayersVot>();
        public static Dictionary<uint, PhoenixProject.Game.ConquerStructures.PlayersVot> VotePoolUid = new Dictionary<uint, PhoenixProject.Game.ConquerStructures.PlayersVot>();
        public static ConcurrentDictionary<uint, PhoenixProject.Network.GamePackets.Quest.Quest> Quest = new ConcurrentDictionary<uint, PhoenixProject.Network.GamePackets.Quest.Quest>();
         public static PhoenixProject.Generated.Interfaces.Table Table;
        public static uint MaxRoses = 5000;
        public static uint MaxLilies = 999;
        public static uint MaxOrchads = 500;
        public static uint MaxTulips = 50;
        public static Game.Tournaments.EliteTournament Elite_PK_Tournament;
        public static SafeDictionary<uint, Game.Features.Flowers.Flowers> AllFlower = new SafeDictionary<uint, Game.Features.Flowers.Flowers>(1000000);
        
        public static Dictionary<uint, Game.Features.Reincarnation.ReincarnateInfo> ReincarnatedCharacters = new Dictionary<uint, Game.Features.Reincarnation.ReincarnateInfo>();
        public static SafeDictionary<uint, Database.AccountTable> AwaitingPool = new SafeDictionary<uint, Database.AccountTable>(1000000);
        public static ThreadSafeDictionary<uint, Client.GameState> GamePool = new ThreadSafeDictionary<uint, Client.GameState>(1000000);
        public static PhoenixProject.Game.ConquerStructures.QuizShow.MainInfo MainQuiz = new PhoenixProject.Game.ConquerStructures.QuizShow.MainInfo();
        public static SafeDictionary<uint, Client.GameState> WasInGamePool = new SafeDictionary<uint, Client.GameState>(1000000);
        public static SafeDictionary<ulong, Game.Map> Maps = new SafeDictionary<ulong, Game.Map>(1000000);
        public static SafeDictionary<uint, Game.ConquerStructures.Society.Guild> Guilds = new SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.Guild>(100000);
        public static List<char> InvalidCharacters = new List<char>() { ' ', '[', '{', '}', '(', ')', ']', '#', '*', '\\', '/', '<', '>', ':', '?', '"', '|', '=', '' };
        public static Random Random = new Random();
        public static int boundID = 45;
        public static int boundIDEnd = 46;
        public static short GetDistance(ushort X, ushort Y, ushort X2, ushort Y2)
        {
            return (short)Math.Sqrt((X - X2) * (X - X2) + (Y - Y2) * (Y - Y2));
        }
        public static bool PercentSuccess(double percent)
        {
            int percentgen = Random.Next(0, 99);
            int maingen = Random.Next(0, 100);
            double thepercent = double.Parse(maingen.ToString() + "." + percentgen.ToString());
            return (thepercent <= percent);
        }
        public static int GetDegree(int X, int X2, int Y, int Y2)
        {
            int direction = 0;

            double AddX = X2 - X;
            double AddY = Y2 - Y;
            double r = (double)Math.Atan2(AddY, AddX);
            if (r < 0) r += (double)Math.PI * 2;

            direction = (int)(360 - (r * 180 / Math.PI));

            return direction;
        }
        public static UInt64 ToDateTimeInt(DateTime dt)
        {
            return UInt64.Parse(dt.ToString("yyyyMMddHHmmss"));
        }
        public static DateTime FromDateTimeInt(UInt64 val)
        {
            return new DateTime(
                (Int32)(val / 10000000000),
                (Int32)((val % 10000000000) / 100000000),
                (Int32)((val % 100000000) / 1000000),
                (Int32)((val % 1000000) / 10000),
                (Int32)((val % 10000) / 100),
                (Int32)(val % 100));
        }
        public static Game.Enums.ConquerAngle GetAngle(ushort X, ushort Y, ushort X2, ushort Y2)
        {
            double direction = 0;

            double AddX = X2 - X;
            double AddY = Y2 - Y;
            double r = (double)Math.Atan2(AddY, AddX);

            if (r < 0) r += (double)Math.PI * 2;

            direction = 360 - (r * 180 / (double)Math.PI);

            byte Dir = (byte)((7 - (Math.Floor(direction) / 45 % 8)) - 1 % 8);
            return (Game.Enums.ConquerAngle)(byte)((int)Dir % 8);
        }
        /*public static Boolean ValidClanName(String name)
        {
            lock (Clans)
            {
                foreach (Clan clans in Clans.Values)
                {
                    if (clans.Name == name)
                        return false;
                }
            }
            return true;
        }*/
        public static void SendWorldMessage(Interfaces.IPacket packet)
        {
            foreach (Client.GameState client in Kernel.GamePool.Values)
            {
                if (client != null)
                {
                    client.Send(packet);
                }
            }
        }
        public static void SendWorldMessage(Interfaces.IPacket message, Client.GameState[] to)
        {
            foreach (Client.GameState client in to)
            {
                if (client != null)
                {
                    client.Send(message);
                }
            }
        }

        public static void SendWorldMessage(Interfaces.IPacket message, Client.GameState[] to, uint exceptuid)
        {
            foreach (Client.GameState client in to)
            {
                if (client != null)
                {
                    if (client.Entity.UID != exceptuid)
                    {
                        client.Send(message);
                    }
                }
            }
        }

        public static void SendWorldMessage(Interfaces.IPacket message, Client.GameState[] to, ushort mapid)
        {
            foreach (Client.GameState client in to)
            {
                if (client != null)
                {
                    if (client.Map.ID == mapid)
                    {
                        client.Send(message);
                    }
                }
            }
        }

        public static void SendWorldMessage(Interfaces.IPacket message, Client.GameState[] to, ushort mapid, uint exceptuid)
        {
            foreach (Client.GameState client in to)
            {
                if (client != null)
                {
                    if (client.Map.ID == mapid)
                    {
                        if (client.Entity.UID != exceptuid)
                        {
                            client.Send(message);
                        }
                    }
                }
            }
        }

        public static uint maxJumpTime(short distance)
        {
            uint x = 0;
            x = 400 * (uint)distance / 10;
            return x;
        }
        public static bool Rate(int value)
        {
            return value > Random.Next() % 100;
        }
        public static bool Rate(int value, int discriminant)
        {
            return value > Random.Next() % discriminant;
        }
        public static bool Rate(ulong value)
        {
            return Rate((int)value);
        }
    }
}
