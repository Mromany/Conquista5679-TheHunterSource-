using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game
{
    class KimoChangeGear
    {
        public static void Load(Client.GameState client)
        {
        }
        public static void Load2(Client.GameState client)
        {
           
            client.Equipment.UpdateEntityPacket2();
        }
    }
}
