using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Game;
using PhoenixProject.ServerBase;

namespace PhoenixProject.Network.GamePackets
{
    public static class MovementHandler
    {
        /*public static class MovementHandler
        {
            private static sbyte[] _walkXCoords = new sbyte[8] { 0, -1, -1, -1, 0, 1, 1, 1 };
            private static sbyte[] _walkYCoords = new sbyte[8] { 1, 1, 0, -1, -1, -1, 0, 1 };

            private static sbyte[] _rideXCoords = new sbyte[24] { 0, -2, -2, -2, 0, 2, 2, 2, -1, -2, -2, -1, 1, 2, 2, 1, -1, -2, -2, -1, 1, 2, 2, 1 };
            private static sbyte[] _rideYCoords = new sbyte[24] { 2, 2, 0, -2, -2, -2, 0, 2, 2, 1, -1, -2, -2, -1, 1, 2, 2, 1, -1, -2, -2, -1, 1, 2 };

            public static void Walk(Movement movement, Client.GameState client)
            {
                client.Entity.Direction = (byte)(movement.Direction % 8);
                client.Entity.Action = 0;

                ushort _x = client.Entity.X;
                ushort _y = client.Entity.Y;
                _x += (ushort)(_walkXCoords[movement.Direction % 8]);
                _y += (ushort)(_walkYCoords[movement.Direction % 8]);

                if (client.Map.Floor[_x, _y] < Enums.Tile.Available)
                {
                    client.Entity.Teleport(client.Entity.MapId, client.Entity.X, client.Entity.Y);
                    return;
                }

                client.Entity.X = _x;
                client.Entity.Y = _y;
                if (client.Interaction != null)
                {
                    client.Interaction.Target.Character.Direction = (byte)(movement.Direction % 8);
                    client.Interaction.Target.Character.Action = 0;
                    client.Interaction.Target.Character.X = _x;
                    client.Interaction.Target.Character.Y = _y;
                    client.Interaction.Packet.Parameter = movement.Direction % 8;
                    client.Interaction.Packet.X = _x;
                    client.Interaction.Packet.Y = _y;
                    client.Interaction.Target.Interaction.Packet.Parameter = movement.Direction % 8;
                    client.Interaction.Target.Interaction.Packet.X = _x;
                    client.Interaction.Target.Interaction.Packet.Y = _y;
                }

                #region Screen Checks
                Packets.HoldingHands hands = new Packets.HoldingHands((byte)movement.Direction, (byte)movement.Speed);
                if (client.Interaction != null)
                {
                    hands.Identity = client.Identity;
                    hands.Target = client.Interaction.Target.Identity;
                    client.Send(hands.Buffer);
                }
                else
                    client.Send(movement.ToArray());

                foreach (Client.GameState other in client.Map.Entities.Values)
                    if (client.Identity != other.Identity)
                        if (Kernel.InScreen(_x, _y, other.Character.X, other.Character.Y))
                        {
                            if (!other.Screen.InScreen.ContainsKey(client.Identity))
                            {
                                client.Entity.SendSpawn(other);
                                other.Character.SendSpawn(client);
                                if (client.Interaction != null)
                                {
                                    other.Character.SendSpawn(client.Interaction.Target);
                                    client.Interaction.Target.Character.SendSpawn(other);
                                }
                            }
                            else
                            {
                                if (client.Interaction != null)
                                    other.Send(hands.Buffer);
                                else
                                    other.Send(movement.ToArray());
                            }
                        }
                        else if (client.Screen.InScreen.ContainsKey(other.Identity))
                        {
                            if (!Kernel.InScreen(_x, _y, other.Character.X, other.Character.Y))
                            {
                                if (client.Interaction != null)
                                    other.Send(hands.Buffer);
                                else
                                    other.Send(movement.ToArray());
                                other.Screen.Remove(client.Entity, false);
                                client.Screen.Remove(other.Character, false);
                                if (client.Interaction != null)
                                {
                                    client.Interaction.Target.Screen.Remove(other.Character, false);
                                    other.Screen.Remove(client.Interaction.Target.Character, false);
                                }
                            }
                        }
                foreach (Packets.NpcSpawn npc in client.Map.Npcs.Values)
                    if (Kernel.InScreen(npc.X, npc.Y, _x, _y))
                    {
                        if (client.Screen.Add(npc))
                            client.Send(npc.ToArray());
                    }
                    else
                        client.Screen.Remove(npc, false);
                foreach (Interfaces.iMapObject obj in client.Map.FloorItems.Values)
                    if (Kernel.InScreen(obj.X, obj.Y, _x, _y))
                    {
                        if (client.Screen.Add(obj))
                            client.Send(obj.ToArray());
                    }
                    else if (Kernel.ObjOutScreen(obj.X, obj.Y, _x, _y))
                        client.Screen.Remove(obj, false);
                #endregion
            }
            public static void Ride(Packets.Movement movement, Client.GameState client)
            {
                client.Entity.Direction = (byte)(movement.Direction % 8);
                client.Entity.Action = 0;

                ushort _x = client.Entity.X;
                ushort _y = client.Entity.Y;
                _x += (ushort)(_walkXCoords[movement.Direction % 24]);
                _y += (ushort)(_walkYCoords[movement.Direction % 24]);

                if (client.Map.Floor[_x, _y] < Enums.Tile.Available)
                {
                    client.Entity.Teleport(client.Entity.MapID, client.Entity.X, client.Entity.Y);
                    return;
                }
                client.Send(movement.ToArray());

                #region Screen Checks
                foreach (Client.GameState other in client.Map.Players.Values)
                    if (client.Identity != other.Identity)
                        if (Kernel.InScreen(_x, _y, other.Character.X, other.Character.Y))
                        {
                            if (!other.Screen.InScreen.ContainsKey(client.Identity))
                            {
                                client.Entity.SendSpawn(other, _x, _y);
                                other.Character.SendSpawn(client);
                            }
                            else
                                other.Send(movement.ToArray());
                        }
                        else if (client.Screen.InScreen.ContainsKey(other.Identity))
                        {
                            if (!Kernel.InScreen(_x, _y, other.Character.X, other.Character.Y))
                            {
                                other.Send(movement.ToArray());
                                other.Screen.Remove(client.Entity, false);
                                client.Screen.Remove(other.Character, false);
                            }
                        }
                foreach (Packets.NpcSpawn npc in client.Map.Npcs.Values)
                    if (Kernel.InScreen(npc.X, npc.Y, _x, _y))
                    {
                        if (client.Screen.Add(npc))
                            client.Send(npc.ToArray());
                    }
                    else
                        client.Screen.Remove(npc, false);
                foreach (Interfaces.iMapObject obj in client.Map.FloorItems.Values)
                    if (Kernel.InScreen(obj.X, obj.Y, _x, _y))
                    {
                        if (client.Screen.Add(obj))
                            client.Send(obj.ToArray());
                    }
                    else if (Kernel.ObjOutScreen(obj.X, obj.Y, _x, _y))
                        client.Screen.Remove(obj, false);
                #endregion

                client.Entity.X = _x;
                client.Entity.Y = _y;
            }
            public static void Jump(Packets.GeneralAction action, Client.GameState client)
            {
                try
                {
                    if (Kernel.GetDistance(action.Value1, action.Value2, action.X, action.Y) <= 18)
                    {
                        client.Entity.Action = 0;

                        if (client.Map.Floor[action.Value1, action.Value2] < Enums.Tile.Available)
                        {
                            client.Entity.Teleport(client.Entity.MapID, action.X, action.Y);
                            return;
                        }
                        client.Send(action.ToArray());

                        #region Direction
                        double Θ = Kernel.GetAngle(action.Value1, action.Value2, action.X, action.Y);
                        byte direction = 0;
                        if (Θ >= -25 && Θ < 25)
                            direction = 6;
                        else if (Θ >= 25 && Θ < 70)
                            direction = 7;
                        else if (Θ >= 70 && Θ < 110)
                            direction = 0;
                        else if (Θ >= 110 && Θ < 160)
                            direction = 1;
                        else if (Θ >= 160 && Θ < 200)
                            direction = 2;
                        else if (Θ >= 200 && Θ < 245)
                            direction = 3;
                        else if (Θ >= 245 || Θ < -60)
                            direction = 4;
                        else
                            direction = 5;
                        client.Entity.Direction = direction;
                        if (client.Interaction != null)
                        {
                            client.Interaction.Target.Character.Direction = direction;
                            client.Interaction.Packet.Parameter = direction;
                            client.Interaction.Target.Interaction.Packet.Parameter = direction;
                        }
                        #endregion

                        #region Screen Checks
                        HoldingHands hands = new HoldingHands(action.Value1, action.Value2);
                        if (client.Interaction != null)
                        {
                            hands.Identity = client.Identity;
                            hands.Target = client.Interaction.Target.Identity;
                            client.Send(hands.Buffer);
                        }

                        foreach (Client.GameState other in client.Map.Entities.Values)
                            if (client.Identity != other.Identity)
                                if (Kernel.InScreen(action.Value1, action.Value2, other.Character.X, other.Character.Y))
                                {
                                    if (!other.Screen.Contains(client.Identity))
                                    {
                                        Kernel.Vector v = Kernel.GetBorderCoords(client.Entity.X, client.Entity.Y, other.Character.X, other.Character.Y);
                                        client.Entity.SendSpawn(other, v.X, v.Y);
                                        if (v.X != action.Value1 || v.Y != action.Value2)
                                            if (client.Interaction == null)
                                                other.Send(action.ToArray());
                                        other.Character.SendSpawn(client);
                                        if (client.Interaction != null)
                                        {
                                            other.Character.SendSpawn(client.Interaction.Target);
                                            client.Interaction.Target.Character.SendSpawn(other, v.X, v.Y);
                                        }
                                    }
                                    else
                                    {
                                        if (client.Interaction == null)
                                            other.Send(action.ToArray());
                                        else
                                            other.Send(hands.Buffer);
                                    }
                                }
                                else if (client.Screen.InScreen.ContainsKey(other.Identity))
                                {
                                    if (!Kernel.InScreen(action.Value1, action.Value2, other.Character.X, other.Character.Y))
                                    {
                                        if (client.Interaction != null)
                                            other.Send(hands.Buffer);
                                        else
                                            other.Send(action.ToArray());
                                        client.Screen.Remove(other.Entity);
                                        other.Screen.Remove(client.Entity);
                                        if (client.Interaction != null)
                                        {
                                            client.Interaction.Target.Screen.Remove(other.Character, false);
                                            other.Screen.Remove(client.Interaction.Target.Character, false);
                                        }
                                    }
                                }
                        foreach (Packets.NpcSpawn npc in client.Map.Npcs.Values)
                            if (Kernel.InScreen(npc.X, npc.Y, action.Value1, action.Value2))
                            {
                                if (client.Screen.Add(npc))
                                    client.Send(npc.ToArray());
                            }
                            else
                                client.Screen.Remove(npc, false);
                        foreach (Interfaces.iMapObject obj in client.Map.Floor.Values)
                            if (Kernel.InScreen(obj.X, obj.Y, action.Value1, action.Value2))
                            {
                                if (client.Screen.Add(obj))
                                    client.Send(obj.ToArray());
                            }
                            else if (Kernel.ObjOutScreen(obj.X, obj.Y, action.Value1, action.Value2))
                                client.Screen.Remove(obj, false);
                        #endregion

                        client.Entity.X = action.Value1;
                        client.Entity.Y = action.Value2;
                        if (client.Interaction != null)
                        {
                            client.Interaction.X = action.Value1;
                            client.Interaction.Y = action.Value2;
                            client.Interaction.X = action.Value1;
                            client.Interaction.Y = action.Value2;
                            client.Interaction.X = action.Value1;
                            client.Interaction.Y = action.Value2;
                            client.Interaction.Action = 0;
                        }
                    }
                    else
                        client.Disconnect();
                }
                catch (Exception e) { Console.WriteLine(e); client.Entity.Teleport(client.Entity.MapId, action.X, action.Y); }
            }
        }*/
    }
}
