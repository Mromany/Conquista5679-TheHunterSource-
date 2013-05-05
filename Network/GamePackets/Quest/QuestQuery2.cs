using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets.Quest
{
    class QuestQuery2
    {
        public static void Handle(byte[] Data, Client.GameState Client)
        {
            QuestQuery query = new QuestQuery(Data);
        }
    }
}
