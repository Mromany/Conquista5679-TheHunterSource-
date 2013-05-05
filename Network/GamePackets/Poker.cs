using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public enum TableState : ushort
    {
        Unopened = 0,
        Pocket,
        Flop,
        River,
        Showdown
    }
    public enum PlayerPokerStatus : ushort
    {
        Offline = 0,
        Normal = 1,
    }
    public enum PokerCurrency : ushort
    {
        Gold = 0,
        CP = 1,
    }
    public enum PokerBetType : ushort
    {
        FixedBet = 0,
        NoLimit = 1,
    }
}
