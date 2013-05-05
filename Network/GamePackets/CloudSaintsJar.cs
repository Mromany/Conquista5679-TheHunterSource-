using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class CloudSaintsJar
    {
        
        public static void Execute(Client.GameState Hero, Attack Packet)
        {

            Packet.ResponseDamage = Hero.Entity.Status4;
            Hero.Send(Packet);
        }
    }
}
