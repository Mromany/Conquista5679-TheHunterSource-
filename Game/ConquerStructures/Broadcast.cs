using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Game.ConquerStructures
{
    public class Broadcast
    {
        public static ServerBase.Counter BroadcastCounter = new PhoenixProject.ServerBase.Counter(1);

        public struct BroadcastStr
        {
            public uint ID;
            public uint EntityID;
            public string EntityName;
            public uint SpentCPs;
            public string Message;
        }

        public static DateTime LastBroadcast = DateTime.Now;

        public static BroadcastStr CurrentBroadcast = new BroadcastStr() { EntityID = 1 };

        public static List<BroadcastStr> Broadcasts = new List<BroadcastStr>();
    }
}
