using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets.Quest
{
    public enum QuestCompleteTypes : ushort
    {
        Accepted = 0,
        Avaliable = 2,
        Done = 1,
        Event = 3,
        Failed = 5
    }
}
