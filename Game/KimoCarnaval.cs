using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Interfaces;

namespace PhoenixProject.Game
{
    class KimoCarnaval
    {
       public static ushort X = 439, Y = 391;
       public static ushort X1 = 429, Y1 = 387;
       public static ushort X2 = 428, Y2 = 378;
       public static ushort X3 = 428, Y3 = 368;
       public static ushort X4 = 439, Y4 = 367;
       public static ushort X5 = 448, Y5 = 372;
       public static ushort X6 = 448, Y6 = 378;
       public static ushort X7 = 448, Y7 = 387;
        public static void Load()
        {
            if (ServerBase.Kernel.Maps.ContainsKey(1002))
            {
                uint ItemID = 720159;
                #region CPBag

                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1305;
                npc.Mesh = 13050;
                npc.Type = Enums.NpcType.Talker;
                npc.X = (ushort)(X-1);
                npc.Y = (ushort)(Y-1);
                npc.MapID = 1002;
                //ServerBase.Kernel.Maps[1002].AddNpc(npc);
               // Program.KimoTime16 = Time32.Now;
           


                
                Game.Map Map = ServerBase.Kernel.Maps[1002];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                    floorItem.Value = Database.rates.PartyDrop;
                    floorItem.ItemID = ItemID;
                    floorItem.MapID = 1002;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                    {
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    }
                    Map.AddFloorItem(floorItem);
                    foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                    {
                        if (C.Entity.MapID == 1002)
                        {
                            C.SendScreenSpawn(floorItem, true);
                            npc.SendSpawn(C);

                            //C.Entity.Update(PhoenixProject.Network.GamePackets._String.Effect, "wsmhcxq_att", true);
                        }
                    }
                }
          
                 // Network.GamePackets.NpcInitial2.DeleteNPC2(1305);
                #endregion
                //Load2();
            }
        }
        public static void Load2()
        {
            if (ServerBase.Kernel.Maps.ContainsKey(1002))
            {
                #region CPBag

                uint ItemID = 720159;
                ushort X = X1, Y = Y1;
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1305;
                npc.Mesh = 13050;
                npc.Type = Enums.NpcType.Talker;
                npc.X = (ushort)(X - 1);
                npc.Y = (ushort)(Y - 1);
                npc.MapID = 1002;
                // ServerBase.Kernel.Maps[1002].AddNpc(npc);

                //  Program.KimoTime16 = Time32.Now;


                Game.Map Map = ServerBase.Kernel.Maps[1002];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                    floorItem.Value = Database.rates.PartyDrop;
                    floorItem.ItemID = ItemID;
                    floorItem.MapID = 1002;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                    {
                        if (C.Entity.MapID == 1002)
                        {
                            C.SendScreenSpawn(floorItem, true);
                            npc.SendSpawn(C);

                            // C.Entity.Update(PhoenixProject.Network.GamePackets._String.Effect, "wsmhcxq_att", true);
                        }
                    }

                    // Network.GamePackets.NpcInitial2.DeleteNPC2(1305);
                #endregion

                }
            }
        }
        public static void Load3()
        {
            if (ServerBase.Kernel.Maps.ContainsKey(1002))
            {
                #region CPBag

                uint ItemID = 720159;
                ushort X = X2, Y = Y2;
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1305;
                npc.Mesh = 13050;
                npc.Type = Enums.NpcType.Talker;
                npc.X = (ushort)(X - 1);
                npc.Y = (ushort)(Y - 1);
                npc.MapID = 1002;
                // ServerBase.Kernel.Maps[1002].AddNpc(npc);

                Game.Map Map = ServerBase.Kernel.Maps[1002];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                    floorItem.Value = Database.rates.PartyDrop;
                    floorItem.ItemID = ItemID;
                    floorItem.MapID = 1002;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                    {
                        if (C.Entity.MapID == 1002)
                        {
                            C.SendScreenSpawn(floorItem, true);
                            npc.SendSpawn(C);
                            // C.Entity.Update(PhoenixProject.Network.GamePackets._String.Effect, "wsmhcxq_att", true);
                        }
                    }
                    // Network.GamePackets.NpcInitial2.DeleteNPC2(1305);
                #endregion
                    //Load4();
                }
            }
        }
        public static void Load4()
        {
            if (ServerBase.Kernel.Maps.ContainsKey(1002))
            {
                #region CPBag

                uint ItemID = 720159;
                ushort X = X3, Y = Y3;
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1305;
                npc.Mesh = 13050;
                npc.Type = Enums.NpcType.Talker;
                npc.X = (ushort)(X - 1);
                npc.Y = (ushort)(Y - 1);
                npc.MapID = 1002;
                //ServerBase.Kernel.Maps[1002].AddNpc(npc);

                Game.Map Map = ServerBase.Kernel.Maps[1002];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                    floorItem.Value = Database.rates.PartyDrop;
                    floorItem.ItemID = ItemID;
                    floorItem.MapID = 1002;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                    {
                        if (C.Entity.MapID == 1002)
                        {
                            C.SendScreenSpawn(floorItem, true);
                            npc.SendSpawn(C);
                            //  C.Entity.Update(PhoenixProject.Network.GamePackets._String.Effect, "wsmhcxq_att", true);
                        }
                    }
                    // Network.GamePackets.NpcInitial2.DeleteNPC2(1305);
                #endregion
                    Load5();
                }
            }
        }
        public static void Load5()
        {
            if (ServerBase.Kernel.Maps.ContainsKey(1002))
            {
                #region CPBag

                uint ItemID = 720159;
                ushort X = X4, Y = Y4;
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1305;
                npc.Mesh = 13050;
                npc.Type = Enums.NpcType.Talker;
                npc.X = (ushort)(X - 1);
                npc.Y = (ushort)(Y - 1);
                npc.MapID = 1002;
                //ServerBase.Kernel.Maps[1002].AddNpc(npc);*

                Game.Map Map = ServerBase.Kernel.Maps[1002];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                    floorItem.Value = Database.rates.PartyDrop;
                    floorItem.ItemID = ItemID;
                    floorItem.MapID = 1002;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                    {
                        if (C.Entity.MapID == 1002)
                        {
                            C.SendScreenSpawn(floorItem, true);
                            npc.SendSpawn(C);
                            // C.Entity.Update(PhoenixProject.Network.GamePackets._String.Effect, "wsmhcxq_att", true);
                        }
                    }
                    // Network.GamePackets.NpcInitial2.DeleteNPC2(1305);
                #endregion
                    // Load6();
                }
            }
        }
        public static void Load6()
        {
            if (ServerBase.Kernel.Maps.ContainsKey(1002))
            {
                #region CPBag

                uint ItemID = 720159;
                ushort X = X5, Y = Y5;
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1305;
                npc.Mesh = 13050;
                npc.Type = Enums.NpcType.Talker;
                npc.X = (ushort)(X - 1);
                npc.Y = (ushort)(Y - 1);
                npc.MapID = 1002;
                //ServerBase.Kernel.Maps[1002].AddNpc(npc);

                Game.Map Map = ServerBase.Kernel.Maps[1002];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                    floorItem.Value = Database.rates.PartyDrop;
                    floorItem.ItemID = ItemID;
                    floorItem.MapID = 1002;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                    {
                        if (C.Entity.MapID == 1002)
                        {
                            C.SendScreenSpawn(floorItem, true);
                            npc.SendSpawn(C);
                            //  C.Entity.Update(PhoenixProject.Network.GamePackets._String.Effect, "wsmhcxq_att", true);
                        }
                    }
                    //Network.GamePackets.NpcInitial2.DeleteNPC2(1305);
                #endregion
                    //Load7();
                }
            }
        }
        public static void Load7()
        {
            if (ServerBase.Kernel.Maps.ContainsKey(1002))
            {
                #region CPBag

                uint ItemID = 720159;
                ushort X = X6, Y = Y6;
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1305;
                npc.Mesh = 13050;
                npc.Type = Enums.NpcType.Talker;
                npc.X = (ushort)(X - 1);
                npc.Y = (ushort)(Y - 1);
                npc.MapID = 1002;
                //  ServerBase.Kernel.Maps[1002].AddNpc(npc);

                Game.Map Map = ServerBase.Kernel.Maps[1002];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                    floorItem.Value = Database.rates.PartyDrop;
                    floorItem.ItemID = ItemID;
                    floorItem.MapID = 1002;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                    {
                        if (C.Entity.MapID == 1002)
                        {
                            C.SendScreenSpawn(floorItem, true);
                            npc.SendSpawn(C);
                            // C.Entity.Update(PhoenixProject.Network.GamePackets._String.Effect, "wsmhcxq_att", true);
                        }
                    }

                    // Network.GamePackets.NpcInitial2.DeleteNPC2(1305);

                }
                #endregion
            }
           // Load8();
        }//443 377
        public static void Load8()
        {
            if (ServerBase.Kernel.Maps.ContainsKey(1002))
            {
                #region CPBag

                uint ItemID = 720159;
                ushort X = X7, Y = Y7;
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1305;
                npc.Mesh = 13050;
                npc.Type = Enums.NpcType.Talker;
                npc.X = (ushort)(X - 1);
                npc.Y = (ushort)(Y - 1);
                npc.MapID = 1002;
                //ServerBase.Kernel.Maps[1002].AddNpc(npc);

                Game.Map Map = ServerBase.Kernel.Maps[1002];
                if (Map.SelectCoordonates(ref X, ref Y))
                {
                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                    floorItem.ValueType = Network.GamePackets.FloorItem.FloorValueType.ConquerPoints;
                    floorItem.Value = Database.rates.PartyDrop;
                    floorItem.ItemID = ItemID;
                    floorItem.MapID = 1002;
                    floorItem.MapObjType = Game.MapObjectType.Item;
                    floorItem.X = X;
                    floorItem.Y = Y;
                    floorItem.Type = Network.GamePackets.FloorItem.Drop;
                    floorItem.OnFloor = Time32.Now;
                    floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    while (Map.Npcs.ContainsKey(floorItem.UID))
                        floorItem.UID = Network.GamePackets.FloorItem.FloorUID.Next;
                    Map.AddFloorItem(floorItem);
                    foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                    {
                        if (C.Entity.MapID == 1002)
                        {
                            C.SendScreenSpawn(floorItem, true);
                            npc.SendSpawn(C);
                            // C.Entity.Update(PhoenixProject.Network.GamePackets._String.Effect, "wsmhcxq_att", true);
                        }
                    }
                    // Network.GamePackets.NpcInitial2.DeleteNPC2(1305);

                #endregion
                    // Load9();
                }
            }
        }
        public static void Load9()
        {
            if (ServerBase.Kernel.Maps.ContainsKey(1002))
            {
                ushort X = 900, Y = 900;
                INpc npc = new Network.GamePackets.NpcSpawn();
                npc.UID = 1305;
                npc.Mesh = 13050;
                npc.Type = Enums.NpcType.Talker;
                npc.X = (ushort)(X - 1);
                npc.Y = (ushort)(Y - 1);
                npc.MapID = 1002;
                foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                {
                    if (C.Entity.MapID == 1002)
                    {

                        npc.SendSpawn(C);
                        // C.Entity.Update(PhoenixProject.Network.GamePackets._String.Effect, "wsmhcxq_att", true);
                    }
                }
                // ServerBase.Kernel.Maps[1002].AddNpc(npc);

            }

            
        }

    
    }

}

