using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PhoenixProject.Database
{
    public class DMapsTable
    {
       /* public static void Load()
        {
            DmapCollection DMaps = new DmapCollection();
            DMaps.LoadAndCloseReader(Dmap.FetchAll());
            for (int x = 0; x < DMaps.Count; x++)
            {
                PhoenixProject.Database.MapsTable.MapInformation info = new PhoenixProject.Database.MapsTable.MapInformation();
                info.ID = (ulong)DMaps[x].Id;
                info.BaseID = (ulong)DMaps[x].Mapdoc;
                info.Status = (uint)DMaps[x].Type;
                info.Weather = (uint)DMaps[x].Owner;
                PhoenixProject.Database.MapsTable.MapInformations.Add(info.ID, info);

            }
            Console.WriteLine("DMaps informations loaded.");
        }*/
    }
}