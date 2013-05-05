﻿using System;

namespace PhoenixProject.Interfaces
{
    public interface IMapObject
    {
        ushort X { get; }
        ushort Y { get; }
        ulong MapID { get; }
        uint UID { get; }
        Client.GameState Owner { get; }
        Game.MapObjectType MapObjType { get; }
        void SendSpawn(Client.GameState client);
        void SendSpawn(Client.GameState client, bool checkScreen);
    }
}
