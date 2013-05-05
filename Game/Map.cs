using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using PhoenixProject.Interfaces;
using System.Text;
using System.Linq;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game
{
    public class Map
    {
        public static ServerBase.Counter DynamicIDs = new PhoenixProject.ServerBase.Counter(11000) { Finish = 60000 };

        public static Enums.ConquerAngle[] Angles = new Enums.ConquerAngle[] {
            Enums.ConquerAngle.SouthWest,
            Enums.ConquerAngle.West,
            Enums.ConquerAngle.NorthWest,
            Enums.ConquerAngle.North,
            Enums.ConquerAngle.NorthEast,
            Enums.ConquerAngle.East,
            Enums.ConquerAngle.SouthEast,
            Enums.ConquerAngle.South };
        public static Floor ArenaBaseFloor = null;
        public ServerBase.Counter EntityUIDCounter = new PhoenixProject.ServerBase.Counter(400000);
        public List<Zoning.Zone> Zones = new List<Zoning.Zone>();
        public ulong ID;
        public ulong BaseID;
        public ushort Owner;
        public ushort Level;
        public Floor Floor;
        private string Path;
        public bool IsDynamic()
        {
            return BaseID != ID;
        }

        public List<Entity> Entities;
        public List<Entity> Companions;
        public Dictionary<uint, INpc> Npcs;
        public Dictionary<uint, table> Npcs2;
        public void AddNpc(INpc npc)
        {
            if (Npcs.ContainsKey(npc.UID) == false)
            {
                Npcs.Add(npc.UID, npc);
                #region Setting the near coords invalid to avoid unpickable items.
                Floor[npc.X, npc.Y, MapObjectType.InvalidCast, npc] = false;
                if (npc.Mesh / 10 != 108 && (byte)npc.Type < 10)
                {
                    ushort X = npc.X, Y = npc.Y;
                    foreach (Enums.ConquerAngle angle in Angles)
                    {
                        ushort xX = X, yY = Y;
                        UpdateCoordonatesForAngle(ref xX, ref yY, angle);
                        Floor[xX, yY, MapObjectType.InvalidCast, null] = false;
                    }
                }
                #endregion
            }
        }
        public void Removenpc(INpc npc)
        {
            if (Npcs.ContainsKey(npc.UID) == true)
            {
                
                Npcs.Remove(npc.UID);
                Floor[npc.X, npc.Y, MapObjectType.Npc, npc] = true;
               
               
            }
        }
        public void AddNpc2(table npc)
        {
            if (Npcs2.ContainsKey(npc.UID) == false)
            {
                Npcs2.Add(npc.UID, npc);
                #region Setting the near coords invalid to avoid unpickable items.
                Floor[npc.X, npc.Y, MapObjectType.InvalidCast, npc] = false;
                if (npc.Lookface / 10 != 108 && (byte)npc.Lookface < 10)
                {
                    ushort X = npc.X, Y = npc.Y;
                    foreach (Enums.ConquerAngle angle in Angles)
                    {
                        ushort xX = X, yY = Y;
                        UpdateCoordonatesForAngle(ref xX, ref yY, angle);
                        Floor[xX, yY, MapObjectType.InvalidCast, null] = false;
                    }
                }
                #endregion
            }
        }
        public void AddEntity(Entity entity)
        {
            if (entity.UID < 800000)
            {
                bool dont = false;
                for (int x = 0; x < Entities.Count; x++)
                {
                    if (x >= Entities.Count)
                        break;
                    if (Entities[x] != null)
                    {
                        if (Entities[x].UID == entity.UID)
                        {
                            dont = true;
                            break;
                        }
                    }
                }
                if (!dont)
                {
                    //   lock (Entities)
                    {
                        Entities.Add(entity);
                        Floor[entity.X, entity.Y, MapObjectType.Monster, entity] = false;
                    }
                }
            }
            else
            {
                bool dont = false;
                for (int x = 0; x < Companions.Count; x++)
                {
                    if (x >= Companions.Count)
                        break;
                    if (Companions[x] != null)
                    {
                        if (Companions[x].UID == entity.UID)
                        {
                            dont = true;
                            break;
                        }
                    }
                }
                if (!dont)
                {
                    // lock (Companions)
                    {
                        Companions.Add(entity);
                        Floor[entity.X, entity.Y, MapObjectType.Monster, entity] = false;
                    }
                }
            }
        }
        public void RemoveEntity(Entity entity)
        {
            Game.Entity mob = null;
            bool dont = false;
            for (int x = 0; x < Entities.Count; x++)
            {
                if (x >= Entities.Count)
                    break;
                if (Entities[x] != null)
                {
                    if (Entities[x].UID == entity.UID)
                    {
                        mob = Entities[x];
                        dont = true;
                        break;
                    }
                }
            }
            if (dont)
            {
                // lock (Entities)
                {
                    Entities.Remove(mob);
                    Floor[entity.X, entity.Y, MapObjectType.Monster, entity] = true;
                }
            }

            Game.Entity Comp = null;
            bool donta = false;
            for (int x = 0; x < Companions.Count; x++)
            {
                if (x >= Companions.Count)
                    break;
                if (Companions[x] != null)
                {
                    if (Companions[x].UID == entity.UID)
                    {
                        Comp = Companions[x];
                        donta = true;
                        break;
                    }
                }
            }
            if (donta)
            {
                //lock (Companions)
                {
                    Companions.Remove(Comp);
                    Floor[entity.X, entity.Y, MapObjectType.Monster, entity] = true;
                }
            }
        }
        public void AddFloorItem(Network.GamePackets.FloorItem floorItem)
        {
           
            Floor[floorItem.X, floorItem.Y, MapObjectType.Item, floorItem] = false;
            if (Network.GamePackets.FloorItem.FloorUID.Now >= 10000)
            {
                Network.GamePackets.FloorItem.FloorUID.Now = 0;
            }
        }
        public void RemoveFloorItem(Network.GamePackets.FloorItem floorItem)
        {
            Floor[floorItem.X, floorItem.Y, MapObjectType.Item, floorItem] = true;
        }

        public bool SelectCoordonates(ref ushort X, ref ushort Y)
        {
            if (Floor[X, Y, MapObjectType.Item, null])
            {
                bool can = true;
                if (Zones.Count != 0)
                {
                    foreach (Zoning.Zone z in Zones)
                    {
                        if (z.IsPartOfRectangle(new Point() { X = X, Y = Y }))
                        {
                            can = false;
                            break;
                        }
                    }
                }
                if (can)
                    return true;
            }

            foreach (Enums.ConquerAngle angle in Angles)
            {
                ushort xX = X, yY = Y;
                UpdateCoordonatesForAngle(ref xX, ref yY, angle);
                if (Floor[xX, yY, MapObjectType.Item, null])
                {
                    if (Zones.Count != 0)
                    {
                        bool can = true;
                        foreach (Zoning.Zone z in Zones)
                        {
                            if (z.IsPartOfRectangle(new Point() { X = xX, Y = yY }))
                            { can = false; break; }
                        }
                        if (!can)
                            continue;
                    }
                    X = xX;
                    Y = yY;
                    return true;
                }
            }
            return false;
        }
        public static void UpdateCoordonatesForAngle(ref ushort X, ref ushort Y, Enums.ConquerAngle angle)
        {
            sbyte xi = 0, yi = 0;
            switch (angle)
            {
                case Enums.ConquerAngle.North: xi = -1; yi = -1; break;
                case Enums.ConquerAngle.South: xi = 1; yi = 1; break;
                case Enums.ConquerAngle.East: xi = 1; yi = -1; break;
                case Enums.ConquerAngle.West: xi = -1; yi = 1; break;
                case Enums.ConquerAngle.NorthWest: xi = -1; break;
                case Enums.ConquerAngle.SouthWest: yi = 1; break;
                case Enums.ConquerAngle.NorthEast: yi = -1; break;
                case Enums.ConquerAngle.SouthEast: xi = 1; break;
            }
            X = (ushort)(X + xi);
            Y = (ushort)(Y + yi);
        }
        #region Scenes
        private SceneFile[] Scenes;
        private static string NTString(string value)
        {
            value = value.Remove(value.IndexOf("\0"));
            return value;
        }
        private SceneFile CreateSceneFile(BinaryReader Reader)
        {
            SceneFile file = new SceneFile();
            file.SceneFileName = NTString(Encoding.ASCII.GetString(Reader.ReadBytes(260)));
            file.Location = new Point(Reader.ReadInt32(), Reader.ReadInt32());
            using (BinaryReader reader = new BinaryReader(new FileStream(ServerBase.Constants.DataHolderPath + file.SceneFileName, FileMode.Open)))
            {
                ScenePart[] partArray = new ScenePart[reader.ReadInt32()];
                for (int i = 0; i < partArray.Length; i++)
                {
                    reader.BaseStream.Seek(0x14cL, SeekOrigin.Current);
                    partArray[i].Size = new Size(reader.ReadInt32(), reader.ReadInt32());
                    reader.BaseStream.Seek(4L, SeekOrigin.Current);
                    partArray[i].StartPosition = new Point(reader.ReadInt32(), reader.ReadInt32());
                    reader.BaseStream.Seek(4L, SeekOrigin.Current);
                    partArray[i].NoAccess = new bool[partArray[i].Size.Width, partArray[i].Size.Height];
                    for (int j = 0; j < partArray[i].Size.Height; j++)
                    {
                        for (int k = 0; k < partArray[i].Size.Width; k++)
                        {
                            partArray[i].NoAccess[k, j] = reader.ReadInt32() == 0;
                            reader.BaseStream.Seek(8L, SeekOrigin.Current);
                        }
                    }
                }
                file.Parts = partArray;
            }
            return file;
        }
        public struct SceneFile
        {
            public string SceneFileName
            {
                get;
                set;
            }
            public Point Location
            {
                get;
                set;
            }
            public ScenePart[] Parts
            {
                get;
                set;
            }
        }
        public struct ScenePart
        {
            public string Animation;
            public string PartFile;
            public Point Offset;
            public int aniInterval;
            public System.Drawing.Size Size;
            public int Thickness;
            public Point StartPosition;
            public bool[,] NoAccess;
        }
        #endregion

        public Map(ulong id, string path)
        {

            if (!ServerBase.Kernel.Maps.ContainsKey(id))
                ServerBase.Kernel.Maps.Add(id, this);
            Npcs = new Dictionary<uint, INpc>();
            Npcs2 = new Dictionary<uint, table>();
            Entities = new List<Entity>();
            Floor = new Floor(0, 0, id);
            Companions = new List<Entity>();
            ID = id;
            BaseID = id;
            if (path == "")
                path = Database.DMaps.MapPaths[(ushort)id];
            Path = path;
            #region Loading floor.
           
            if (File.Exists(ServerBase.Constants.DMapsPath + "\\maps\\" + id.ToString() + ".map"))
            {
               
                byte[] buff = File.ReadAllBytes(ServerBase.Constants.DMapsPath + "\\maps\\" + id.ToString() + ".map");
               
                MemoryStream FS = new MemoryStream(buff);
                BinaryReader BR = new BinaryReader(FS);
                int Width = BR.ReadInt32();
                int Height = BR.ReadInt32();
                Floor = new Game.Floor(Width, Height, ID);
                if (id == 700)
                    if (ArenaBaseFloor == null)
                        ArenaBaseFloor = new Game.Floor(Width, Height, ID);
                for (ushort y = 0; y < Height; y = (ushort)(y + 1))
                {
                    for (ushort x = 0; x < Width; x = (ushort)(x + 1))
                    {
                        Floor[x, y, MapObjectType.InvalidCast, null] = !(BR.ReadByte() == 1 ? true : false);
                        if (id == 700)
                            if (ArenaBaseFloor == null)
                                ArenaBaseFloor[x, y, MapObjectType.InvalidCast, null] = !(BR.ReadByte() == 1 ? true : false);
                    }
                }

                BR.Close();
                FS.Close();
            }
            else
            {
                Console.WriteLine(""+(ServerBase.Constants.DMapsPath + Path)+"");
                if (File.Exists(ServerBase.Constants.DMapsPath + Path))
                {
                    Console.WriteLine("Loll");
                    byte[] buff = File.ReadAllBytes(ServerBase.Constants.DMapsPath + Path);
                    MemoryStream FS = new MemoryStream(buff);
                    BinaryReader BR = new BinaryReader(FS);
                    BR.ReadBytes(268);
                    int Width = BR.ReadInt32();
                    int Height = BR.ReadInt32();
                    Floor = new Game.Floor(Width, Height, ID);
                    if (id == 700)
                        if (ArenaBaseFloor == null)
                            ArenaBaseFloor = new Game.Floor(Width, Height, ID);
                    for (ushort y = 0; y < Height; y = (ushort)(y + 1))
                    {
                        for (ushort x = 0; x < Width; x = (ushort)(x + 1))
                        {
                            Floor[x, y, MapObjectType.InvalidCast, null] = !Convert.ToBoolean(BR.ReadUInt16());
                            if (id == 700)
                                if (ArenaBaseFloor == null)
                                    ArenaBaseFloor[x, y, MapObjectType.InvalidCast, null] = !(BR.ReadByte() == 1 ? true : false);
                            BR.BaseStream.Seek(4L, SeekOrigin.Current);
                        }
                        BR.BaseStream.Seek(4L, SeekOrigin.Current);
                    }
                    uint amount = BR.ReadUInt32();
                    BR.BaseStream.Seek(amount * 12, SeekOrigin.Current);

                    int num = BR.ReadInt32();
                    List<SceneFile> list = new List<SceneFile>();
                    for (int i = 0; i < num; i++)
                    {
                        switch (BR.ReadInt32())
                        {
                            case 10:
                                BR.BaseStream.Seek(0x48L, SeekOrigin.Current);
                                break;

                            case 15:
                                BR.BaseStream.Seek(0x114L, SeekOrigin.Current);
                                break;

                            case 1:
                                list.Add(this.CreateSceneFile(BR));
                                break;

                            case 4:
                                BR.BaseStream.Seek(0x1a0L, SeekOrigin.Current);
                                break;
                        }
                    }
                    Scenes = list.ToArray();

                    for (int i = 0; i < Scenes.Length; i++)
                    {
                        foreach (ScenePart part in Scenes[i].Parts)
                        {
                            for (int j = 0; j < part.Size.Width; j++)
                            {
                                for (int k = 0; k < part.Size.Height; k++)
                                {
                                    Point point = new Point();
                                    point.X = ((Scenes[i].Location.X + part.StartPosition.X) + j) - part.Size.Width;
                                    point.Y = ((Scenes[i].Location.Y + part.StartPosition.Y) + k) - part.Size.Height;
                                    Floor[(ushort)point.X, (ushort)point.Y, MapObjectType.InvalidCast, null] = part.NoAccess[j, k];
                                }
                            }
                        }
                    }

                    BR.Close();
                    FS.Close();
                    ////BR.Dispose();
                    //FS.Dispose();
                    SaveMap();
                }
            }
            #endregion
            LoadNpcs();
           // LoadTables();
            LoadZones();
            LoadMonsters();
            //Loadtale();
            LoadPortals();
            /*if (Database.MapsTable.MapInformations[(ushort)id].Owner == id)
            {
                if (Database.MapsTable.MapInformations[(ushort)id].Box == 1)
                {
                    PhoenixProject.Interfaces.INpc npc = new Network.GamePackets.NpcSpawn();
                    npc.UID = 30657;
                    npc.Mesh = 8200;
                    //Console.WriteLine("ss");
                    npc.Type = (PhoenixProject.Game.Enums.NpcType)3;
                    npc.X = (ushort)Database.MapsTable.MapInformations[id].BoxX;
                    npc.Y = (ushort)Database.MapsTable.MapInformations[id].BoxY;
                    npc.MapID = (ushort)Database.MapsTable.MapInformations[id].Owner;
                    AddNpc(npc);
                }
            }*/
            //FloorItemTimerCallBack = new TimerCallback(_timerFloorItemCallBack);
            //_timerFloorItem = new Timer(FloorItemTimerCallBack, this, 10000, 2000);
            //Thread.Sleep(1000);
        }
        public Map(ulong id, ulong baseid, string path, bool dynamic)
        {
            if (!ServerBase.Kernel.Maps.ContainsKey(id))
                ServerBase.Kernel.Maps.Add(id, this);
            Npcs = new Dictionary<uint, INpc>();
            Npcs2 = new Dictionary<uint, table>();
            Entities = new List<Entity>();
            //Floor = new Floor(0, 0, id);
            Companions = new List<Entity>();
            ID = id;
            BaseID = baseid;
            Path = path;
            Floor = new Floor(0, 0, id);
            #region Loading floor.
            if (id != baseid && baseid == 700 && ArenaBaseFloor != null)
            {
                Floor = new Game.Floor(ArenaBaseFloor.Bounds.Width, ArenaBaseFloor.Bounds.Height, ID);
                for (ushort y = 0; y < ArenaBaseFloor.Bounds.Height; y = (ushort)(y + 1))
                {
                    for (ushort x = 0; x < ArenaBaseFloor.Bounds.Width; x = (ushort)(x + 1))
                    {
                        Floor[x, y, MapObjectType.Player, null] = ArenaBaseFloor[x, y, MapObjectType.Player, null];
                    }
                }
            }
            else
            {
                if (File.Exists(ServerBase.Constants.DMapsPath + "\\maps\\" + baseid.ToString() + ".map"))
                {
                    //Console.WriteLine("kim");
                    byte[] buff = File.ReadAllBytes(ServerBase.Constants.DMapsPath + "\\maps\\" + baseid.ToString() + ".map");
                    MemoryStream FS = new MemoryStream(buff);
                    BinaryReader BR = new BinaryReader(FS);
                    int Width = BR.ReadInt32();
                    int Height = BR.ReadInt32();

                    Floor = new Game.Floor(Width, Height, ID);

                    for (ushort y = 0; y < Height; y = (ushort)(y + 1))
                    {
                        for (ushort x = 0; x < Width; x = (ushort)(x + 1))
                        {
                            Floor[x, y, MapObjectType.InvalidCast, null] = !(BR.ReadByte() == 1 ? true : false);
                        }
                    }
                    BR.Close();
                    FS.Close();
                    BR.Dispose();
                    FS.Flush();
                    ////BR.Dispose();
                    //FS.Dispose();
                }
                else
                {
                    if (File.Exists(ServerBase.Constants.DMapsPath + Path))
                    {
                        FileStream FS = new FileStream(ServerBase.Constants.DMapsPath + Path, FileMode.Open);
                        BinaryReader BR = new BinaryReader(FS);
                        BR.ReadBytes(268);
                        int Width = BR.ReadInt32();
                        int Height = BR.ReadInt32();

                        Floor = new Game.Floor(Width, Height, ID);

                        for (ushort y = 0; y < Height; y = (ushort)(y + 1))
                        {
                            for (ushort x = 0; x < Width; x = (ushort)(x + 1))
                            {
                                Floor[x, y, MapObjectType.InvalidCast, null] = !Convert.ToBoolean(BR.ReadUInt16());

                                BR.BaseStream.Seek(4L, SeekOrigin.Current);
                            }
                            BR.BaseStream.Seek(4L, SeekOrigin.Current);
                        }
                        uint amount = BR.ReadUInt32();
                        BR.BaseStream.Seek(amount * 12, SeekOrigin.Current);

                        int num = BR.ReadInt32();
                        List<SceneFile> list = new List<SceneFile>();
                        for (int i = 0; i < num; i++)
                        {
                            switch (BR.ReadInt32())
                            {
                                case 10:
                                    BR.BaseStream.Seek(0x48L, SeekOrigin.Current);
                                    break;

                                case 15:
                                    BR.BaseStream.Seek(0x114L, SeekOrigin.Current);
                                    break;

                                case 1:
                                    list.Add(this.CreateSceneFile(BR));
                                    break;

                                case 4:
                                    BR.BaseStream.Seek(0x1a0L, SeekOrigin.Current);
                                    break;
                            }
                        }
                        Scenes = list.ToArray();

                        for (int i = 0; i < Scenes.Length; i++)
                        {
                            foreach (ScenePart part in Scenes[i].Parts)
                            {
                                for (int j = 0; j < part.Size.Width; j++)
                                {
                                    for (int k = 0; k < part.Size.Height; k++)
                                    {
                                        Point point = new Point();
                                        point.X = ((Scenes[i].Location.X + part.StartPosition.X) + j) - part.Size.Width;
                                        point.Y = ((Scenes[i].Location.Y + part.StartPosition.Y) + k) - part.Size.Height;
                                        Floor[(ushort)point.X, (ushort)point.Y, MapObjectType.InvalidCast, null] = part.NoAccess[j, k];
                                    }
                                }
                            }
                        }

                        BR.Close();
                        FS.Close();
                        ////BR.Dispose();
                        //FS.Dispose();
                        SaveMap();
                    }
                }
            }
            #endregion
            if (baseid == 3024)
            {//54 46
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1765;
                npc.Mesh = 8200;
                npc.Type = Enums.NpcType.WareHouse;
                npc.X = 90 ;
                npc.Y = 67;
                npc.MapID = id;
                AddNpc(npc);
            }
            LoadNpcs();
           // LoadTables();
            LoadZones();
            LoadMonsters();
            //Loadtale();
            LoadPortals();
            //FloorItemTimerCallBack = new TimerCallback(_timerFloorItemCallBack);
            //_timerFloorItem = new Timer(FloorItemTimerCallBack, this, 10000, 2000);
        }

        private void LoadPortals()
        {
            ServerBase.IniFile file = new PhoenixProject.ServerBase.IniFile(ServerBase.Constants.PortalsPath);
            ushort portalCount = file.ReadUInt16(BaseID.ToString(), "Count");

            for (int i = 0; i < portalCount; i++)
            {
                string _PortalEnter = file.ReadString(BaseID.ToString(), "PortalEnter" + i.ToString());
                string _PortalExit = file.ReadString(BaseID.ToString(), "PortalExit" + i.ToString());
                string[] PortalEnter = _PortalEnter.Split(' ');
                string[] PortalExit = _PortalExit.Split(' ');
                Game.Portal portal = new PhoenixProject.Game.Portal();
                portal.CurrentMapID = Convert.ToUInt16(PortalEnter[0]);
                portal.CurrentX = Convert.ToUInt16(PortalEnter[1]);
                portal.CurrentY = Convert.ToUInt16(PortalEnter[2]);
                portal.DestinationMapID = Convert.ToUInt16(PortalExit[0]);
                portal.DestinationX = Convert.ToUInt16(PortalExit[1]);
                portal.DestinationY = Convert.ToUInt16(PortalExit[2]);
                Portals.Add(portal);
            }
        }
        public List<Game.Portal> Portals = new List<Game.Portal>();
        private TimerCallback _timercallback;
        private Timer _timer;
        private void SaveMap()
        {
            if (!File.Exists(ServerBase.Constants.DMapsPath + "\\maps\\" + BaseID.ToString() + ".map"))
            {
                FileStream stream = new FileStream(ServerBase.Constants.DMapsPath + "\\maps\\" + BaseID.ToString() + ".map", FileMode.Create);
                BinaryWriter writer = new BinaryWriter(stream);
                writer.Write((uint)Floor.Bounds.Width);
                writer.Write((uint)Floor.Bounds.Height);
                for (int y = 0; y < Floor.Bounds.Height; y++)
                {
                    for (int x = 0; x < Floor.Bounds.Width; x++)
                    {
                        writer.Write((byte)(Floor[x, y, MapObjectType.InvalidCast, null] == true ? 1 : 0));
                    }
                }
                writer.Close();
                stream.Close();
                //writer.Dispose();
                //stream.Dispose();
            }
        }
        private void LoadZones()
        {
            Database.MySqlCommand command = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.SELECT);
            command.Select("notavailablepaths").Where("mapid", ID);

            PhoenixProject.Database.MySqlReader reader = new PhoenixProject.Database.MySqlReader(command);
            while (reader.Read())
            {
                Zoning.Zone zone = new Zoning.Zone(
                    new Point() { X = reader.ReadInt32("Point1_X"), Y = reader.ReadInt32("Point1_Y") },
                    new Point() { X = reader.ReadInt32("Point2_X"), Y = reader.ReadInt32("Point2_Y") },
                    new Point() { X = reader.ReadInt32("Point3_X"), Y = reader.ReadInt32("Point3_Y") },
                    new Point() { X = reader.ReadInt32("Point4_X"), Y = reader.ReadInt32("Point4_Y") }
                    );
                Zones.Add(zone);
            }
            reader.Close();
            reader.Dispose();
        }
 
        private void TreasureBox()
        {
            INpc npc = new Network.GamePackets.NpcSpawn();
            npc.UID = 5012;
            npc.Mesh = 9307;
            npc.Type = Game.Enums.NpcType.Talker;
            npc.X = 33 ;
            npc.Y = 5;
            npc.MapID = 1225;
            //Network.GamePackets.PokerTable npc2 = new Network.GamePackets.PokerTable();
            AddNpc(npc);
        }
        private void LoadFlag(uint UID, string Name, ushort x, ushort y)
        {
            
                Network.GamePackets.SobNpcSpawn npc = new Network.GamePackets.SobNpcSpawn();
                npc.UID = UID;
                npc.Mesh = 8910;
                npc.Type = Enums.NpcType.Stake;
                npc.X = x ;
                npc.Y = y;
                npc.MapID = 2060;
                npc.Sort = 17;
                npc.ShowName = true;
                npc.Name = Name;
                npc.Hitpoints = 50000;
                npc.MaxHitpoints = 50000;
                AddNpc(npc);
        }
        private void LoadNpcs()
        {
            Database.MySqlCommand command = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.SELECT);
            command.Select("npcs").Where("mapid", ID);
            PhoenixProject.Database.MySqlReader reader = new PhoenixProject.Database.MySqlReader(command);
            while (reader.Read())
            {
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = reader.ReadUInt32("id");
                npc.Mesh = reader.ReadUInt16("lookface");
                npc.Type = (Enums.NpcType)reader.ReadByte("type");
                npc.X = reader.ReadUInt16("cellx"); ;
                npc.Y = reader.ReadUInt16("celly");
                npc.MapID = ID;
                //Network.GamePackets.PokerTable npc2 = new Network.GamePackets.PokerTable();
                AddNpc(npc);
            }
            reader.Close();
            reader.Dispose();
            command = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.SELECT);
            command.Select("sobnpcs").Where("mapid", ID);
            reader = new PhoenixProject.Database.MySqlReader(command);
            while (reader.Read())
            {
                Network.GamePackets.SobNpcSpawn npc = new Network.GamePackets.SobNpcSpawn();
                npc.UID = reader.ReadUInt32("id");
                npc.Mesh = reader.ReadUInt16("lookface");
                if (ID == 1039)
                    npc.Mesh = (ushort)(npc.Mesh - npc.Mesh % 10 + 7);
                npc.Type = (Enums.NpcType)reader.ReadByte("type");
                npc.X = reader.ReadUInt16("cellx"); ;
                npc.Y = reader.ReadUInt16("celly");
                npc.MapID = reader.ReadUInt16("mapid");
                npc.Sort = reader.ReadUInt16("sort");
                npc.ShowName = true;
                npc.Name = reader.ReadString("name");
                npc.Hitpoints = reader.ReadUInt32("life");
                npc.MaxHitpoints = reader.ReadUInt32("maxlife");
                AddNpc(npc);
            }
            reader.Close();
            reader.Dispose();
        }
       
        public bool FreezeMonsters = false;
        public void LoadMonsters()
        {
           // Companions = new SafeDictionary<uint, Entity>(1000);
            Database.MySqlCommand command = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.SELECT);
            command.Select("monsterspawns").Where("mapid", ID);
            PhoenixProject.Database.MySqlReader reader = new PhoenixProject.Database.MySqlReader(command);
            int mycount = 0;
            try
            {
                while (reader.Read())
                {
                    uint monsterID = reader.ReadUInt32("npctype");
                    ushort CircleDiameter = reader.ReadUInt16("maxnpc");
                    ushort X = reader.ReadUInt16("bound_x");
                    ushort Y = reader.ReadUInt16("bound_y");
                    ushort XPlus = reader.ReadUInt16("bound_cx");
                    ushort YPlus = reader.ReadUInt16("bound_cy");
                    ushort Amount = reader.ReadUInt16("max_per_gen");
                    int respawn = reader.ReadInt32("rest_secs");
                    if (Database.MonsterInformation.MonsterInfos.ContainsKey(monsterID))
                    {
                        Database.MonsterInformation mt = Database.MonsterInformation.MonsterInfos[monsterID];
                        mt.RespawnTime = respawn + 5;
                        mt.BoundX = X;
                        mt.BoundY = Y;
                        mt.BoundCX = XPlus;
                        mt.BoundCY = YPlus;

                        bool more = true;
                        for (int count = 0; count < Amount; count++)
                        {
                            if (!more)
                                break;
                            Entity entity = new Entity(EntityFlag.Monster, false);
                            entity.MapObjType = MapObjectType.Monster;
                            entity.MonsterInfo = mt.Copy();
                            entity.MonsterInfo.Owner = entity;
                            entity.Name = mt.Name;
                            entity.MinAttack = mt.MinAttack;
                            entity.MaxAttack = entity.MagicAttack = mt.MaxAttack;
                            entity.Hitpoints = entity.MaxHitpoints = mt.Hitpoints;
                            entity.Body = mt.Mesh;
                            entity.Level = mt.Level;
                            entity.UID = EntityUIDCounter.Next;
                            entity.MapID = ID;
                            entity.SendUpdates = true;
                            entity.X = (ushort)(X + ServerBase.Kernel.Random.Next(0, XPlus));
                            entity.Y = (ushort)(Y + ServerBase.Kernel.Random.Next(0, YPlus));
                            for (int count2 = 0; count2 < 50; count2++)
                            {
                                if (!Floor[entity.X, entity.Y, MapObjectType.Monster, entity])
                                {
                                    entity.X = (ushort)(X + ServerBase.Kernel.Random.Next(0, XPlus));
                                    entity.Y = (ushort)(Y + ServerBase.Kernel.Random.Next(0, YPlus));
                                    if (count2 == 50)
                                        more = false;
                                }
                                else
                                    break;
                            }
                            if (more)
                            {
                                if (Floor[entity.X, entity.Y, MapObjectType.Monster, entity])
                                {
                                    mycount++;
                                    AddEntity(entity);
                                  
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Program.SaveException(e); }
            reader.Close();
            reader.Dispose();
            if (mycount != 0)
            {
                MyTimer = new System.Timers.Timer(interval);
                MyTimer.AutoReset = true;
                MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack);
                MyTimer.Start();
               // _timercallback = new TimerCallback(_timerCallBack);
               // _timer = new Timer(_timercallback, this, 10000, 1000);
            }
        }
        public double interval = 1000;
        public System.Timers.Timer MyTimer;
        public Time32 LastReload = Time32.Now;
        public DateTime Thread_time;
        
        private void _timerCallBack(object myObject, System.Timers.ElapsedEventArgs arg)
        {
           /* foreach (Entity monster in Companions.Values)
            {
                if (!monster.Owner.Socket.Connected)
                {
                    RemoveEntity(monster);
                    break;
                }
            }*/
            if (Program.ServerRrestart == false)
            {
                try
                {
                    Thread_time = DateTime.Now;
                    for (int x = 0; x < Companions.Count; x++)
                    {

                        if (x >= Companions.Count)
                            break;
                        if (Companions[x] != null)
                        {
                            Entity monster = Companions[x];
                            if (monster.Owner.Socket != null)
                            {
                                if (!monster.Owner.Socket.Connected)
                                {
                                    RemoveEntity(monster);
                                    break;
                                }
                            }
                        }
                    }
                    for (int x = 0; x < Entities.Count; x++)
                    {

                        if (x >= Entities.Count)
                            break;
                        Entity monster = null;
                        if (Entities[x] != null)
                        {
                            monster = Entities[x];
                            if (monster.Dead)
                            {
                                if (Time32.Now > monster.DeathStamp.AddSeconds(monster.MonsterInfo.RespawnTime))
                                {
                                    monster.X = (ushort)(monster.MonsterInfo.BoundX + ServerBase.Kernel.Random.Next(0, monster.MonsterInfo.BoundCX));
                                    monster.Y = (ushort)(monster.MonsterInfo.BoundY + ServerBase.Kernel.Random.Next(0, monster.MonsterInfo.BoundCY));
                                    for (int count = 0; count < monster.MonsterInfo.BoundCX * monster.MonsterInfo.BoundCY; count++)
                                    {
                                        if (!Floor[monster.X, monster.Y, MapObjectType.Monster, null])
                                        {
                                            monster.X = (ushort)(monster.MonsterInfo.BoundX + ServerBase.Kernel.Random.Next(0, monster.MonsterInfo.BoundCX));
                                            monster.Y = (ushort)(monster.MonsterInfo.BoundY + ServerBase.Kernel.Random.Next(0, monster.MonsterInfo.BoundCY));
                                        }
                                        else
                                            break;
                                    }
                                    if (Floor[monster.X, monster.Y, MapObjectType.Monster, null] || monster.X == monster.MonsterInfo.BoundX && monster.Y == monster.MonsterInfo.BoundY)
                                    {
                                        monster.Hitpoints = monster.MonsterInfo.Hitpoints;
                                        monster.RemoveFlag(monster.StatusFlag);
                                        Network.GamePackets._String stringPacket = new PhoenixProject.Network.GamePackets._String(true);
                                        stringPacket.UID = monster.UID;
                                        stringPacket.Type = Network.GamePackets._String.Effect;
                                        stringPacket.Texts.Add("MBStandard");
                                        monster.StatusFlag = 0;
                                        if (monster.Body == 950 && monster.MapID == 2056)
                                        {
                                            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("TeratoDragon has apeared in Grotto[Floor2], Who will Defeat it !", System.Drawing.Color.White, PhoenixProject.Network.GamePackets.Message.Monster), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                        }
                                        if (monster.Body == 950 && monster.MapID == 1015)
                                        {
                                            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("TeratoDragon has apeared in BirdIsnalnd,  Who will Defeat it !", System.Drawing.Color.White, PhoenixProject.Network.GamePackets.Message.Monster), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                        }
                                        if (monster.Body == 951)
                                        {
                                            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Warrning SnowBanshee has Apeared in BirdIsland,  Who will defeat it !.", System.Drawing.Color.White, PhoenixProject.Network.GamePackets.Message.Monster), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                        }
                                        foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
                                        {
                                            if (client.Map.ID == ID)
                                            {
                                                if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, monster.X, monster.Y) < ServerBase.Constants.nScreenDistance)
                                                {
                                                    monster.CauseOfDeathIsMagic = false;
                                                    monster.SendSpawn(client, false);
                                                    client.Send(stringPacket);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (monster.ToxicFogLeft > 0)//Here may BlackSpot kimo
                                {
                                    if (Time32.Now >= monster.ToxicFogStamp.AddSeconds(2))
                                    {
                                        monster.ToxicFogLeft--;
                                        monster.ToxicFogStamp = Time32.Now;
                                        if (monster.Hitpoints > 1)
                                        {
                                            uint damage = Game.Attacking.Calculate.Percent(monster, monster.ToxicFogPercent);
                                            monster.Hitpoints -= damage;
                                            Network.GamePackets.SpellUse suse = new PhoenixProject.Network.GamePackets.SpellUse(true);
                                            suse.Attacker = monster.UID;
                                            suse.SpellID = 10010;
                                            suse.Targets.Add(monster.UID, damage);
                                            monster.MonsterInfo.SendScreen(suse);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch { }
            }

        }

 
        public Map MakeDynamicMap()
        {
            ulong id = (ushort)DynamicIDs.Next;
            Map myDynamic = new Map(id, this.ID, this.Path, true);
            return myDynamic;
        }
        public Map MakeDynamicMap2(uint client)
        {
            //ushort id = (ushort)DynamicIDs.Next;
            Map myDynamic = new Map(client, this.ID, this.Path, true);
            return myDynamic;
        }
        bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
                ServerBase.Kernel.Maps.Remove(ID);

            disposed = true;
        }
    }
    public class Floor
    {
        public Size Bounds;
        public FillStruct[,] Locations;
        public ulong FloorMapID;
        public Floor(int width, int height, ulong mapID)
        {
            FloorMapID = mapID;
            Bounds = new Size(width, height);
            Locations = new FillStruct[width, height];
        }
        public class FillStruct
        {
            public object item;
            public Network.GamePackets.FloorItem Item
            {
                get
                {
                    if (item == null) return null;
                    return item as Network.GamePackets.FloorItem;
                }
                set
                {
                    item = value;
                }
            }
            public Interfaces.INpc Npc;
          
            public byte Monsters;
            public bool Full;
        }
        public FillStruct GetLocation(int x, int y)
        {
            if (Bounds.Height == Bounds.Width && Bounds.Width == 0)
                return new FillStruct();
            if (y >= Bounds.Height || x >= Bounds.Width || x < 0 || y < 0)
                return new FillStruct();
            FillStruct filltype = Locations[x, y];
            return filltype;
        }
        public bool this[int x, int y, MapObjectType type, object obj]
        {
            get
            {

                if (Bounds.Height == Bounds.Width && Bounds.Width == 0)
                {
                    Console.WriteLine("Floor " + FloorMapID + " not loaded!!");
                    return true;
                }
                if (y >= Bounds.Height || x >= Bounds.Width || x < 0 || y < 0)
                    return false;

                if (Locations[x, y] == null)
                    Locations[x, y] = new FillStruct() { };

                FillStruct filltype = Locations[x, y];
                if (type == MapObjectType.InvalidCast)
                    return filltype.Full;
                if (filltype.Full)
                    return false;
                if (type == MapObjectType.Player)
                {
                    return true;
                }
                else if (type == MapObjectType.Monster)
                {
                    return filltype.Monsters == 0;
                }
                else if (type == MapObjectType.Item)
                {
                    return filltype.Item == null;
                }
              
                return false;
            }
            set
            {
                if (value)
                {
                    if (Bounds.Height == Bounds.Width && Bounds.Width == 0)
                        return;
                    if (y >= Bounds.Height || x >= Bounds.Width || x < 0 || y < 0)
                        return;

                    if (Locations[x, y] == null)
                        Locations[x, y] = new FillStruct() { };
                    if (type == MapObjectType.InvalidCast)
                    {
                        Locations[x, y].Full = false;
                    }
                    if (type == MapObjectType.Item)
                        Locations[x, y].Item = null;
                    
                    if (type == MapObjectType.Monster)
                        Locations[x, y].Monsters = 0;
                }
                else
                {
                    if (y >= Bounds.Height || x >= Bounds.Width)
                        return;

                    if (Locations[x, y] == null)
                        Locations[x, y] = new FillStruct() { };
                    if (type == MapObjectType.InvalidCast)
                        Locations[x, y].Full = true;
                    if (obj != null)
                    {
                        if (obj is Interfaces.INpc)
                        {
                            Locations[x, y].Npc = obj as Interfaces.INpc;
                        }
                    }
                   
                    if (type == MapObjectType.Item)
                        Locations[x, y].Item = obj as Network.GamePackets.FloorItem;

                    if (type == MapObjectType.Monster)
                        Locations[x, y].Monsters = 1;
                }
            }
        }
    }
    public enum MapObjectType
    {
        SobNpc, Npc, Item, Monster, Player, Nothing, InvalidCast,Table
    }
    public class Portal
    {
        public Portal(ushort CurrentMapID, ushort CurrentX, ushort CurrentY, ushort DestinationMapID, ushort DestinationX, ushort DestinationY)
        {
            this.CurrentMapID = CurrentMapID;
            this.CurrentX = CurrentX;
            this.CurrentY = CurrentY;
            this.DestinationMapID = DestinationMapID;
            this.DestinationX = DestinationX;
            this.DestinationY = DestinationY;
        }
        public Portal()
        {

        }
        public ushort CurrentMapID
        {
            get;
            set;
        }
        public ushort CurrentX
        {
            get;
            set;
        }
        public ushort CurrentY
        {
            get;
            set;
        }
        public ushort DestinationMapID
        {
            get;
            set;
        }
        public ushort DestinationX
        {
            get;
            set;
        }
        public ushort DestinationY
        {
            get;
            set;
        }
    }
}
