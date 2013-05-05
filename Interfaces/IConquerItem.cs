using System;
using System.Drawing;
using PhoenixProject.Network.GamePackets;
using System.Collections.Generic;
namespace PhoenixProject.Interfaces
{
    public interface IConquerItem
    {
        uint UID { get; set; }
        uint ID { get; set; }
        ushort Durability { get; set; }
        ushort MaximDurability { get; set; }
        ushort Position { get; set; }
       
        ushort Warehouse { get; set; }
        uint SocketProgress { get; set; }
        uint PlusProgress { get; set; }
        uint Unknown40 { get; set; }

        uint kimo1 { get; set; }
        uint kimo2 { get; set; }
        uint kimo3 { get; set; }
       
       
        Game.Enums.Gem SocketOne { get; set; }
        Game.Enums.Gem SocketTwo { get; set; }
        Game.Enums.ItemEffect Effect { get; set; }
        bool Inscribed { get; set; }
        Game.Enums.ItemMode Mode { get; set; }
        byte Plus { get; set; }
        byte Bless { get; set; }
        bool Bound { get; set; }
        byte Enchant { get; set; }
        byte Lock { get; set; }
        DateTime UnlockEnd { get; set; }
        bool Suspicious { get; set; }
        DateTime SuspiciousStart { get; set; }
        Game.Enums.Color Color { get; set; }
        short BattlePower { get; }
        bool StatsLoaded { get; set; }
        ushort Vigor { get; set; }
        ushort StackSize { get; set; }
        ushort MaxStackSize { get; set; }
        string Agate { get; set; }
        ItemAdding.Purification_ Purification { get; set; }
        ItemAdding.Refinery_ ExtraEffect { get; set; }
        void SetID(uint ID);
        bool MobDropped { get; set; }
        Dictionary<uint, string> Agate_map { get; set; }
        void SendAgate(Client.GameState Client);
        void Send(Client.GameState Client);
        Boolean IsTwoHander();
        ConquerItem.ItemTypes GetItemType();
        UInt32 RefineItem { get; set; }
        Refinery.RefineryItem RefineStats { get; }
        DateTime RefineryTime { get; set; }
        void SendExtras(Client.GameState client);
    }
}
