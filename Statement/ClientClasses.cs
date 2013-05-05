using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Statement
{
    public enum ClassID : byte
    {
        MartialArtist = 1,
        Warlock = 2,
        ChiMaster = 3,
        Sage = 4,
        Apothecary = 5,
        Performer = 6,
        Wrangler = 9
    }
    public class SubClass
    {
        public byte ID;
        public byte Phase;
        public byte Level;
    }
    public class ClientClasses
    {
        public Dictionary<byte, SubClass> Classes;
        public uint StudyPoints;
        public byte Active;
        public ClientClasses()
        {
            Classes = new Dictionary<byte, SubClass>();
            StudyPoints = 0;
            Active = 0;
        }
    }
}
