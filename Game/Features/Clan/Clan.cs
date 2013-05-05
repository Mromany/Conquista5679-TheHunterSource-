using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Conquer_Online_Server.Game.Features.Clan
{
    public class Clan
    {
        public Clan()
        {
            Members = new Dictionary<uint, Members>();
        }
        public string Name = "";
        public string Leader = "";
        public uint ID = 0;
        public uint Level = 0;
        public uint Donation = 0;
        public string Bulletin;
        public Dictionary<uint, Members> Members;
    }
}
