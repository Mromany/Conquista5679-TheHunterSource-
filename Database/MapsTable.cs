using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PhoenixProject.Database
{
    public class MapsTable
    {
        public struct MapInformation
        {
            public ulong ID;
            public ulong BaseID;
            public uint Status;
            public uint Weather;
            public uint Owner;
            public uint HouseLevel;
            public uint Box;
            public uint BoxX;
            public uint BoxY;
        }
        public static SafeDictionary<ulong, MapInformation> MapInformations = new SafeDictionary<ulong, MapInformation>(50000);
        public static void Load()
        {
            MySqlCommand command = new MySqlCommand(MySqlCommandType.SELECT);
            command.Select("maps");
            MySqlReader reader = new MySqlReader(command);
            while (reader.Read())
            {
                MapInformation info = new MapInformation();
                info.ID = reader.ReadUInt64("id");
                info.BaseID = reader.ReadUInt64("mapdoc");
                //Console.WriteLine("id " + info.ID + "  base : " + info.BaseID + "");
                info.Status = reader.ReadUInt32("type");
                info.Weather = reader.ReadUInt32("weather");
                MapInformations.Add(info.ID, info);
            }
            reader.Close();
            reader.Dispose();
            Console.WriteLine("Map informations loaded.");
        }
    }
}
