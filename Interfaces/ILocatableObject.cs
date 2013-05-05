using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Interfaces;

namespace PhoenixProject.Generated.Interfaces
{
    public interface ILocatableObject
    {

        Game.Enums.NpcType Type { get; set; }
        uint UID { get; set; }
        ushort X { get; set; }
        ushort Y { get; set; }
        uint Mesh { get; set; }
        uint TableUID { get; set; }
        ushort BE { get; set; }
        uint Other { get; set; }
        ushort MapID { get; set; }
        void SendSpawn(Client.GameState Client);
        void SendSpawn(Client.GameState Client, bool checkScreen);
    }
}
