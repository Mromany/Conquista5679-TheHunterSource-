using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Collections;

namespace PhoenixProject.Database
{
    public class DMaps
    {
        public static Hashtable MapOwner = new Hashtable();
        public static SafeDictionary<ulong, string> MapPaths = new SafeDictionary<ulong, string>(5000);
        public static void Load()
        {
            if (File.Exists(ServerBase.Constants.DataHolderPath + "GameMap.dat"))
            {
                Time32 start = Time32.Now;
                FileStream FS = new FileStream(ServerBase.Constants.DataHolderPath + "GameMap.dat", FileMode.Open);
                BinaryReader BR = new BinaryReader(FS);
                uint MapCount = BR.ReadUInt32();
                for (uint i = 0; i < MapCount; i++)
                {
                    ulong MapID = BR.ReadUInt32();
                    string Path = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadInt32()));
                    if (MapID >= 1712 && MapID <= 1720)
                    {
                        BR.ReadUInt32();
                        continue;
                    }
                    if (Path.EndsWith(".7z"))
                    {
                        Path = Path.Remove(Path.Length - 3, 3);
                        Path += ".dmap";
                    }
                    if (!File.Exists(ServerBase.Constants.DMapsPath + "\\maps\\" + MapID + ".map"))
                    {
                        Game.Map map = new PhoenixProject.Game.Map(MapID, Path);
                        Console.WriteLine("add map " + MapID + "");
                    }
                    MapPaths.Add(MapID, Path);
                    BR.ReadInt32();
                }
                BR.Close();
                FS.Close();
               //// FS.Dispose();
                ////BR.Dispose();

                Console.WriteLine("Game map loaded successfully.");
            }
            else
                Console.WriteLine("The specified Conquer Online folder doesn't exist. Game map couldn't be loaded.");
        }
        public static void LoadHouse()
        {
            MySqlCommand command = new MySqlCommand(MySqlCommandType.SELECT);
            command.Select("house");
            PhoenixProject.Database.MySqlReader reader = new PhoenixProject.Database.MySqlReader(command);
            while (reader.Read())
            {
                uint id;
                uint BaseID;
                uint HouseLevel;
                //MapsTable.MapInformation info = new MapsTable.MapInformation();
                id = reader.ReadUInt16("id");
                BaseID = 3024;
                // info.Status = reader.ReadUInt32("type");
                // info.Weather = reader.ReadUInt32("weather");
                id = reader.ReadUInt32("owner");
                HouseLevel = reader.ReadUInt32("HouseLevel");
                // info.Box = reader.ReadUInt32("Box");
                // info.BoxX = reader.ReadUInt32("BoxX");
                // info.BoxY = reader.ReadUInt32("BoxY");
                bool Success = CreateDynamicMap2(id, (ushort)BaseID, id, HouseLevel);

            }
            reader.Close();
            reader.Dispose();
            Console.WriteLine("Houses Loaded informations loaded.");
        }
       
        public static PhoenixProject.Game.Map dynamicMap;
        public static bool CreateDynamicMap2(ulong mapadd, ushort mapneed, uint ownerid, uint level)
        {

            bool addedmap = false;
            if (!ServerBase.Kernel.Maps.ContainsKey(3024))
                new PhoenixProject.Game.Map(3024, Database.DMaps.MapPaths[3024]);
            PhoenixProject.Game.Map origMap = ServerBase.Kernel.Maps[3024];
            dynamicMap = origMap.MakeDynamicMap2(ownerid);


            return addedmap;
        }
        public static bool CreateDynamicMap(ushort mapadd, ushort mapneed, uint ownerid)
        {
            bool addedmap = false;
            if (DMaps.MapOwner.Contains(Convert.ToInt32(ownerid)))
                return false;
            while (ServerBase.Kernel.Maps.ContainsKey(mapadd))
            {
                mapadd++;
            }
            FileStream FS = new FileStream(ServerBase.Constants.DataHolderPath + "GameMap.dat", FileMode.Open);
            BinaryReader BR = new BinaryReader(FS);
            uint MapCount = BR.ReadUInt32();
            for (uint i = 0; i < MapCount; i++)
            {
                ushort MapID = (ushort)BR.ReadUInt32();
                string Path = Encoding.ASCII.GetString(BR.ReadBytes(BR.ReadInt32()));
                if (mapneed == MapID)
                {
                    ushort NewMapID = mapadd;
                    Game.Map D = new Game.Map(NewMapID, Path);
                    D.Owner = NewMapID;
                    ServerBase.Kernel.Maps.Add(NewMapID, D);
                    //ServerBase.Kernel.Maps.Add(NewMapID, mapneed);
                    MapOwner.Add(Convert.ToInt32(ownerid), NewMapID);
                    addedmap = true;
                    break;
                }
                BR.ReadInt32();
            }
            BR.Close();
            FS.Close();
           // FS.Dispose();
            //BR.Dispose();
            return addedmap;
        }
        public static bool DeleteDynamicMap(ushort mapadd, uint ownerid)
        {
            bool deletedmap = false;
            if (!DMaps.MapOwner.Contains(Convert.ToInt32(ownerid)))
                return false;

            ushort NewMapID = mapadd;
            ServerBase.Kernel.Maps.Remove(NewMapID);
            MapOwner.Remove(Convert.ToInt32(ownerid));
            deletedmap = true;

            return deletedmap;
        }
        public static ushort GetHouseID(uint owner)
        {
            int key = Convert.ToInt32(owner);
            if (MapOwner.Contains(key))
            {
                return Convert.ToUInt16(MapOwner[key]);
            }
            return 0;
        }
        public static bool HouseUpgrade(ushort Map)
        {
            ushort STHseID = Convert.ToUInt16(ServerBase.Kernel.Maps[Map]);
            if (STHseID == 1099)
                return true;
            return false;
        }
        public static void Save()
        {
            FileStream FS = new FileStream(ServerBase.Constants.DataHolderPath + "DMapOwner.dat", FileMode.OpenOrCreate);
            BinaryWriter BW = new BinaryWriter(FS);
            BW.Write(MapOwner.Count);
            foreach (DictionaryEntry Map in MapOwner)
            {
                BW.Write(Convert.ToUInt32(Map.Key));
                BW.Write(Convert.ToUInt16(Map.Value));
                //BW.Write(Convert.ToUInt16(ServerBase.Kernel.Maps.[Map.Value]));
            }
            BW.Close();
            FS.Close();
        }
    }
}
