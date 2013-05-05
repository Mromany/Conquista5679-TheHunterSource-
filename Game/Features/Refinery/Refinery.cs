using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PhoenixProject.ServerBase;

namespace PhoenixProject
{
    public class Refinery
    {
        public class RefineryBoxes
        {
            public UInt32 Identifier, Position;
            public Boolean Untradable;
            public RefineryItem.RefineryType Type;
        }
        public class RefineryItem
        {
            public UInt32 Identifier, Position, Percent;
            public Byte Level;
            public Boolean Untradable;
            public RefineryType Type;

            public enum RefineryType
            {
                MDefence = 1,
                Critical = 2,
                SCritical = 3,
                Immunity = 4,
                BreakThrough = 5,
                Counteraction = 6,
                Detoxication = 7,
                Block = 8,
                Penetration = 9,
                Intensification = 10
            }
        }
        public static void Load()
        {
            string[] AllLines = System.IO.File.ReadAllLines("database\\Refinery.txt");
            int refinery = AllLines.Length;
            for (int x = 0; x < refinery; x++)
            {
                string[] Info = AllLines[x].Split('#');
                string Refineries = Info[0];
                RefineryItem ri = new RefineryItem();
                ri.Identifier = uint.Parse(Info[0]);
                ri.Type = (RefineryItem.RefineryType)uint.Parse(Info[1]);
                ri.Position = uint.Parse(Info[2]);
                ri.Level = byte.Parse(Info[3]);
                ri.Percent = (uint.Parse(Info[4]));
                ri.Untradable = Convert.ToBoolean(uint.Parse(Info[5]));
                Kernel.DatabaseRefinery.Add(ri.Identifier, ri);
            }
            Console.WriteLine(String.Format("Loaded Database Refinery Items ({0}).", Kernel.DatabaseRefinery.Count));
            LoadBoxes();
        }
        public static void LoadBoxes()
        {
            string[] AllLines = System.IO.File.ReadAllLines("database\\Refinerybox.txt");
            int refbox = AllLines.Length;
            for (int x = 0; x < refbox; x++)
            {
                string[] Info = AllLines[x].Split('#');
                string Refineries = Info[0];
                RefineryBoxes ri = new RefineryBoxes();
                ri.Identifier = UInt32.Parse(Info[0]);
                ri.Type = (RefineryItem.RefineryType)UInt32.Parse(Info[1]);
                ri.Position = UInt32.Parse(Info[2]);
                ri.Untradable = Convert.ToBoolean(UInt32.Parse(Info[3]));

                Kernel.DatabaseRefineryBoxes.Add(ri.Identifier, ri);
            }
            Console.WriteLine(String.Format("Loaded Database Refinery Boxes ({0}).", Kernel.DatabaseRefinery.Count));
        }
    }
}