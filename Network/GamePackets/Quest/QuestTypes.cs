using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets.Quest
{
    public enum QuestTypes : byte
    {
        Daily = 3,
        EquipmentBonus = 4,
        Event = 5,
        NezhasFeud = 6,
        Recruit = 1,
        Region = 7,
        Tutorial = 2
    }
}
