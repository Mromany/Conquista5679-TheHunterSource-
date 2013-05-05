using System;
using System.Collections.Generic;

namespace PhoenixProject.Interfaces
{
    public interface INpc
    {
        Game.Enums.NpcType Type { get; set; }
        uint UID { get; set; }
        ushort X { get; set; }
        ushort Y { get; set; }
        ushort Mesh { get; set; }
        ulong MapID { get; set; }
        void SendSpawn(Client.GameState Client);
        void SendSpawn(Client.GameState Client, bool checkScreen);
    }
    public interface table
    {
        PhoenixProject.Network.GamePackets.PokerBetType BetType { get; set; }
        PhoenixProject.Network.GamePackets.PokerCurrency Currency { get; set; }
        ulong MapID { get; set; }

        uint UID { get; set; }
        uint TableIndex { get; set; }
        ushort X { get; set; }
        ushort Y { get; set; }
        uint Lookface { get; set; }
        uint MinimumBet { get; set; }
        byte PlayerCount { get; set; }
        ulong PotAmount { get; set; }
        byte SizeAdd { get; set; }
      
        //public List<Client.GameState> Users = new List<Client.GameState>();
        void SendSpawn(Client.GameState Client);
        void SendSpawn(Client.GameState Client, bool checkScreen);
    }
}
