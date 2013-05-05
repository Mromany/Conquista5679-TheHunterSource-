using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    class NpcInitial2
    {
        public static void Handle(byte[] Data, Client.GameState Client)
        {
           
            NpcInitial initial = new NpcInitial(Data);
            if (Client.Entity.EntityFlag == Game.EntityFlag.Player)
            {
                PhoenixProject.Network.GamePackets.NpcInitial.NpcModes mode = initial.Mode;
                if (mode == PhoenixProject.Network.GamePackets.NpcInitial.NpcModes.Talk)
                {
                    //KinConquer.Packets.Handles.NpcTalk.Handle(Data, Client);
                }
                else if (mode == PhoenixProject.Network.GamePackets.NpcInitial.NpcModes.Delete)
                {
                    DeleteNPC(initial.Identifier, Client);
                }
                else
                {
                    Program.WriteLine("Unhandled NpcInitial Type: " + initial.Mode);
                }
            }
        }
        public static void DeleteNPC(uint identifier, Client.GameState Hero)
        {
            PhoenixProject.Interfaces.INpc npc;
            if (ServerBase.Kernel.Maps[1002].Npcs.TryGetValue(identifier, out npc))
            {
                ServerBase.Kernel.Maps[1002].Npcs.Remove(npc.UID);
                ServerBase.Kernel.Maps[1002].Removenpc(npc);
                foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                {
                    if (C.Entity.MapID == 1002)
                    {
                        Data data = new Data(true);
                        data.UID = npc.UID;
                        data.ID = Data.RemoveEntity;
                        Hero.Send(data);
                    }
                }
            }
        }
        public static void DeleteNPC2(uint identifier)
        {
            PhoenixProject.Interfaces.INpc npc;
            if (ServerBase.Kernel.Maps[1002].Npcs.TryGetValue(identifier, out npc))
            {
                ServerBase.Kernel.Maps[1002].Npcs.Remove(npc.UID);
                ServerBase.Kernel.Maps[1002].Removenpc(npc);
                foreach (Client.GameState C in ServerBase.Kernel.GamePool.Values)
                {
                    if (C.Entity.MapID == 1002)
                    {
                        Data data = new Data(true);
                        data.UID = npc.UID;
                        data.ID = Data.RemoveEntity;
                        C.Send(data);
                    }
                }
            }
        }
    }
}
