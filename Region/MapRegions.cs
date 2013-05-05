using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.IO;

namespace PhoenixProject.Region
{
    public class MapRegions : ConcurrentDictionary<uint, List<Region>>
    {
        public void Load()
        {
            using (StreamReader reader = new StreamReader(File.Open(Program.Mapr, FileMode.Open)))
            {
                while (!reader.EndOfStream)
                {
                
                    string[] strArray = reader.ReadLine().Replace("  ", " ").Split(new char[] { ' ' });
                    if (uint.Parse(strArray[1]) == 8)
                    {
                        Region item = new Region
                        {
                            MapID = ushort.Parse(strArray[0]),
                            StartX = ushort.Parse(strArray[2]),
                            StartY = ushort.Parse(strArray[3]),
                            EndX = (ushort)(ushort.Parse(strArray[2]) + ushort.Parse(strArray[4])),
                            EndY = (ushort)(ushort.Parse(strArray[3]) + ushort.Parse(strArray[5])),
                            Name = (strArray[6] != "none") ? strArray[6] : strArray[7],
                            Lineage = uint.Parse(strArray[8])
                        };
                        if (!base.ContainsKey(item.MapID))
                        {
                            base.TryAdd(item.MapID, new List<Region>());
                        }
                        base[item.MapID].Add(item);
                    }
                }
                reader.Close();
            }
        }
    }
}
