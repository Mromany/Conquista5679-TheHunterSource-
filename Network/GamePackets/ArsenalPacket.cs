using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Game.Features;
using PhoenixProject.Client;

namespace PhoenixProject.Network.GamePackets
{
    public class ArsenalPacket
    {
        public static byte[] ArsenalInscribedPage(GameState Client, byte[] Data, Arsenal_ID ID)
        {
            byte[] Buffer = null;
            Arsenal_Client[] Inscribed = null;
            uint Page = (byte)(BitConverter.ToUInt32(Data, 12) / 8);

        Again:
            try
            {
                Inscribed = new Arsenal_Client[Client.Guild.Arsenal.Arsenals[ID].Inscribed.Count];
                Client.Guild.Arsenal.Arsenals[ID].Inscribed.Values.CopyTo(Inscribed, 0);
            }
            catch { goto Again; }
            int Position = 44;
            int count = Inscribed.Length;
            if (count > 8) count = 8;
            Buffer = new byte[84 + (count * 40) + 8];
            for (int i = 0; i < 20; i++)
                Buffer[i] = Data[i];

            Writer.WriteUInt16((ushort)(Buffer.Length - 8), 0, Buffer);
            Writer.WriteUInt32((uint)Inscribed.Length, 20, Buffer);
            Writer.WriteUInt32((uint)Client.Guild.Arsenal.Arsenals[ID].Donation, 36, Buffer);
            Writer.WriteUInt32((uint)Inscribed.Length, 40, Buffer);
            for (uint i = BitConverter.ToUInt32(Data, 8); i <= BitConverter.ToUInt32(Data, 12); i++)
            {
                if (i - 1 >= Inscribed.Length) break;
                Arsenal_Client ac = Inscribed[i - 1];
                Interfaces.IConquerItem Item = ac.Item;
                Writer.WriteUInt32(Item.UID, Position, Buffer); Position += 4;

                Writer.WriteUInt32((uint)(i), Position, Buffer); Position += 4;
                Writer.WriteString(ac.Name, Position, Buffer); Position += 16;
                Writer.WriteUInt32(Item.ID, Position, Buffer); Position += 4;
                Writer.WriteByte((byte)(Item.ID % 10), Position, Buffer); Position++;
                Writer.WriteByte(Item.Plus, Position, Buffer); Position++;
                Writer.WriteByte((byte)Item.SocketOne, Position, Buffer); Position++;
                Writer.WriteByte((byte)Item.SocketTwo, Position, Buffer); Position++;
                Writer.WriteByte((byte)Item.BattlePower, Position, Buffer); Position++;
                Writer.WriteByte(0, Position, Buffer); Position++;
                Writer.WriteByte(0, Position, Buffer); Position++;
                Writer.WriteByte(0, Position, Buffer); Position++;
                Writer.WriteUInt32(0, Position, Buffer); Position += 4;
            }
            Writer.WriteString(ServerBase.Constants.ServerKey, (Buffer.Length - 8), Buffer);
            return Buffer;
        }
        public static byte[] GuildArsenal(GameState Client)
        {
            byte[] Buffer = new byte[244 + 8];
            Writer.WriteUInt16(244, 0, Buffer);
            Writer.WriteUInt16(2201, 2, Buffer);

            if (Client.Guild == null)
            {
                Writer.WriteString(ServerBase.Constants.ServerKey, (Buffer.Length - 8), Buffer);
                return Buffer;
            }
            SharedBattlePower(Client);
            // ObtenerLevelClan(Client);
            #region Default
            if (Client.Guild != null)
            {
                if (Client.Guild.Arsenal != null)
                {

                    Writer.WriteByte(Client.Guild.Arsenal.BattlePower, 8, Buffer);//Total Battle Power
                    Writer.WriteUInt32((uint)Client.Arsenal_Donation, 12, Buffer);//My Donation
                    Writer.WriteUInt32(Client.Entity.GuildSharedBp, 16, Buffer);//Shared Battle Power
                    Writer.WriteByte(8, 20, Buffer);
                    if (Client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Headgear))
                    {
                        Writer.WriteByte(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Potency, 28, Buffer);//Potency
                        Writer.WriteUInt64(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Donation, 36, Buffer);//Donation
                        Writer.WriteByte(1, 44, Buffer);//Allowed
                    }
                    Writer.WriteByte(1, 48, Buffer);
                    if (Client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Armor))
                    {
                        Writer.WriteByte(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Potency, 52, Buffer);//Potency
                        Writer.WriteUInt64(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Donation, 60, Buffer);//Donation
                        Writer.WriteByte(1, 68, Buffer);//Allowed
                    }
                    Writer.WriteByte(2, 72, Buffer);
                    if (Client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Weapon))
                    {
                        Writer.WriteByte(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Potency, 76, Buffer);//Potency
                        Writer.WriteUInt64(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Donation, 84, Buffer);//Donation
                        Writer.WriteByte(1, 92, Buffer);//Allowed
                    }
                    Writer.WriteByte(3, 96, Buffer);
                    if (Client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Ring))
                    {
                        Writer.WriteByte(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Potency, 102, Buffer);//Potency
                        Writer.WriteUInt64(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Donation, 108, Buffer);//Donation
                        Writer.WriteByte(1, 116, Buffer);//Allowed
                    }
                    Writer.WriteByte(4, 120, Buffer);
                    if (Client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Boots))
                    {
                        Writer.WriteByte(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Potency, 124, Buffer);//Potency
                        Writer.WriteUInt64(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Donation, 132, Buffer);//Donation
                        Writer.WriteByte(1, 140, Buffer);//Allowed
                    }
                    Writer.WriteByte(5, 144, Buffer);
                    if (Client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Necklace))
                    {
                        Writer.WriteByte(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Potency, 148, Buffer);//Potency
                        Writer.WriteUInt64(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Donation, 156, Buffer);//Donation
                        Writer.WriteByte(1, 164, Buffer);//Allowed
                    }
                    Writer.WriteByte(6, 168, Buffer);
                    if (Client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Fan))
                    {
                        Writer.WriteByte(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Potency, 172, Buffer);//Potency
                        Writer.WriteUInt64(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Donation, 180, Buffer);//Donation
                        Writer.WriteByte(1, 188, Buffer);//Allowed
                    }
                    Writer.WriteByte(7, 192, Buffer);
                    if (Client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Tower))
                    {
                        Writer.WriteByte(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Potency, 196, Buffer);//Potency
                        Writer.WriteUInt64(Client.Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Donation, 204, Buffer);//Donation
                        Writer.WriteByte(1, 212, Buffer);//Allowed
                    }
                }
            #endregion
            }
            Writer.WriteString(ServerBase.Constants.ServerKey, (Buffer.Length - 8), Buffer);
            return Buffer;

        }
        #region Variables Arsenal Buffers
        public static byte TotalBP = 0;
        public static byte BPMembers = 0;
        public static byte LevelClan = 9;
        public static byte MaximumBP { get { return TotalBP; } }
        public static void SharedBattlePower(Client.GameState client)
        {
            if (client.Guild != null)
            {
                if (client.Guild.Arsenal != null)
                {
                    if (client.Guild.Arsenal.BattlePower >= 1)
                    {
                        #region Puntos 15
                        if (client.Entity.GuildRank == 990)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 2);
                            if (client.Entity.GuildSharedBp >= 13)
                                client.Entity.GuildSharedBp = 13;
                        }
                        if (client.Entity.GuildRank == 620)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 2);
                            if (client.Entity.GuildSharedBp >= 13)
                                client.Entity.GuildSharedBp = 13;
                        }
                        if (client.Entity.GuildRank == 1000)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower);
                            if (client.Entity.GuildSharedBp >= 15)
                                client.Entity.GuildSharedBp = 15;
                        }
                        if (client.Entity.GuildRank == 920)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower);
                            if (client.Entity.GuildSharedBp >= 15)
                                client.Entity.GuildSharedBp = 15;
                        }
                        if (client.Entity.GuildRank == 890)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 5);
                            if (client.Entity.GuildSharedBp >= 10)
                                client.Entity.GuildSharedBp = 10;
                        }
                        if (client.Entity.GuildRank == 858)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 8);
                            if (client.Entity.GuildSharedBp >= 7)
                                client.Entity.GuildSharedBp = 7;
                        }
                        if (client.Entity.GuildRank == 857)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 8);
                            if (client.Entity.GuildSharedBp >= 7)
                                client.Entity.GuildSharedBp = 7;
                        }
                        if (client.Entity.GuildRank == 853)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 8);
                            if (client.Entity.GuildSharedBp >= 7)
                                client.Entity.GuildSharedBp = 7;
                        }
                        if (client.Entity.GuildRank == 854)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 10);
                            if (client.Entity.GuildSharedBp >= 5)
                                client.Entity.GuildSharedBp = 5;
                        }
                        if (client.Entity.GuildRank == 499)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 10);
                            if (client.Entity.GuildSharedBp >= 5)
                                client.Entity.GuildSharedBp = 5;
                        }
                        if (client.Entity.GuildRank == 851)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 10);
                            if (client.Entity.GuildSharedBp >= 5)
                                client.Entity.GuildSharedBp = 5;
                        }
                        if (client.Entity.GuildRank == 498)
                        {
                            client.Entity.GuildSharedBp = (uint)(client.Guild.Arsenal.BattlePower - 10);
                            if (client.Entity.GuildSharedBp >= 5)
                                client.Entity.GuildSharedBp = 5;
                        }
                        #endregion
                        if (client.Guild.Arsenal.BattlePower == 0)
                            client.Guild.Arsenal.BattlePower = 1;
                    }
                }
            }
        }
        public static void ObtenerLevelClan(Client.GameState client)
        {
            #region Obtener level clan
            LevelClan = 0;
            if (client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Weapon))
            {
                if (client.Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Donation >= 5000000)
                {
                    LevelClan += 1;
                }
            }
            if (client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Headgear))
            {
                if (client.Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Donation >= 5000000)
                {
                    LevelClan += 1;
                }
            }
            if (client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Armor))
            {
                if (client.Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Donation >= 5000000)
                {
                    LevelClan += 1;
                }
            }
            if (client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Tower))
            {
                if (client.Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Donation >= 5000000)
                {
                    LevelClan += 1;
                }
            }
            if (client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Fan))
            {
                if (client.Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Donation >= 5000000)
                {
                    LevelClan += 1;
                }
            }
            if (client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Necklace))
            {
                if (client.Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Donation >= 5000000)
                {
                    LevelClan += 1;
                }
            }
            if (client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Ring))
            {
                if (client.Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Donation >= 5000000)
                {
                    LevelClan += 1;
                }
            }
            if (client.Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Boots))
            {
                if (client.Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Donation >= 5000000)
                {
                    LevelClan += 1;
                }
            }
            if (TotalBP >= 15)
            {
                LevelClan += 1;
            }
            #endregion
            client.Guild.Level = LevelClan;
        }
        #endregion
    }
}