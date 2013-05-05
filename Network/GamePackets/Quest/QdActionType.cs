using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets.Quest
{
    public enum QdActionType
    {
        GetScreen,
        FullScreenRefresh,
        SendScreen,
        PathFind,
        Die,
        GetDamage,
        AddScreen,
        RemoveScreen,
        AddRange,
        RemoveRange
    }
}
