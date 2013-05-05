using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Region
{
    public class Region
    {
        public ushort EndX = 0;
        public ushort EndY = 0;
        public uint Lineage = 0;
        public uint MapID;
        public string Name = string.Empty;
        public ushort StartX = 0;
        public ushort StartY = 0;

        public static Region FindRegion(uint map, ushort x, ushort y)
        {
            List<Region> list;
            if (Program.MapRegions.TryGetValue(map, out list))
            {
                foreach (Region region in list.ToArray())
                {
                    if (Program.IsBetweenTwoPoints(x, y, region.StartX, region.StartY, region.EndX, region.EndY))
                    {
                        return region;
                    }
                }
            }
            return GetDefault(map);
        }

        public static Region GetDefault(uint mapid)
        {
            return new Region { MapID = mapid, Lineage = 0 };
        }
    }
}
