using System;
using OpenSSL;
using System.Linq;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using PhoenixProject.Network.Cryptography;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Network.Sockets;
using PhoenixProject.Network;
using PhoenixProject.Interfaces;

namespace PhoenixProject.Client
{
    public class GameState
    {
        public bool LoggedIn = false, Kimoz = false, ChangeGear = true, Disconnected = false, Disconnected2 = false;
        private WinSocket _socket;
        public Database.AccountTable Account;
        public bool Effect = false;
        public bool Effect2 = false;
        public bool Effect3 = false;
        public bool Effect4 = false;
        public bool Effect5 = false;
        public bool Effect6 = false;
        public bool Effect7 = false;
        public System.Collections.Concurrent.ConcurrentDictionary<uint, PhoenixProject.Network.GamePackets.Quest.HeroQuest> Quests;
        public bool SkillTeamJoin = false;
        public bool CaptureTeamJoin = false;
        public MonsterHunterStats MonsterHunterStats;
        public GameCryptography Cryptography;
        public DHKeyExchange.ServerKeyExchange DHKeyExchance;
        public bool Exchange = true;
        public bool InArenaMatch = false;
        public TimerCallback CallBack;
        public PhoenixProject.Game.ConquerStructures.QuizShow.Info QuizInfo;
        public Network.GamePackets.Interaction Interaction;
        public int quarantineKill = 0;
        public int quarantineDeath = 0;
        public int TopDlClaim = 0;
        public int TopGlClaim = 0;
        public int apprtnum = 0;
        public int popups = 0;
        public int Edite = 0;
        public int Edita = 1;
        public Game.Team CtfTeam = null;
        public Game.TeamType CaptureFlagOwner = Game.TeamType.NONE;
        public Game.Enums.Color staticArmorColor;
        public bool AlternateEquipment;
        public bool JustCreated = false;
        public bool ItemGive = false;

        public bool SpiltStack = false;

        public Timer Timer;
        #region Network

        public Logger Logger = null;
        public GameState(WinSocket socket)
        {
            Attackable = false;
            Action = 0;
            _socket = socket;

            Cryptography = new GameCryptography(System.Text.ASCIIEncoding.ASCII.GetBytes(ServerBase.Constants.GameCryptographyKey));
            DHKeyExchance = new Network.GamePackets.DHKeyExchange.ServerKeyExchange();
        }
        public void TryLoginTimeCallBack()
        {


            Disconnect();

        }
        public void ReadyToPlay()
        {
            try
            {
                Screen = new Game.Screen(this);
                Inventory = new Game.ConquerStructures.Inventory(this);
                Equipment = new Game.ConquerStructures.Equipment(this);
                WarehouseOpen = false;
                WarehouseOpenTries = 0;
                TempPassword = "";
                Warehouses = new SafeDictionary<PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID, PhoenixProject.Game.ConquerStructures.Warehouse>(20);
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.TwinCity, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.TwinCity));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.PhoenixCity, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.PhoenixCity));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.ApeCity, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.ApeCity));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.DesertCity, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.DesertCity));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.BirdCity, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.BirdCity));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.StoneCity, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.StoneCity));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.Market, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.Market));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.House, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.House));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.poker1, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.poker1));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.poker2, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.poker2));
                Warehouses.Add(PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.House2, new PhoenixProject.Game.ConquerStructures.Warehouse(this, PhoenixProject.Game.ConquerStructures.Warehouse.WarehouseID.House2));
                Trade = new Game.ConquerStructures.Trade();
                ArenaStatistic = new ArenaStatistic(true);
                Prayers = new List<GameState>();
                MagicDef = new List<GameState>();
                map = null;
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
        }
        public bool SocketDisposed = false;
        public void Send(byte[] buffer)
        {
            if (SocketDisposed)
                return;
            byte[] _buffer = new byte[buffer.Length];
            Buffer.BlockCopy(buffer, 0, _buffer, 0, buffer.Length);
            Network.Writer.WriteString(ServerBase.Constants.ServerKey, _buffer.Length - 8, _buffer);
            try
            {
                if (_socket != null)
                {
                    if (_socket.Connected)
                    {
                        if (Monitor.TryEnter(Cryptography, 10))
                        {
                            if (_socket != null)
                            {
                                if (Monitor.TryEnter(_socket, 10))
                                {
                                    if (_socket != null)
                                    {
                                        Cryptography.Encrypt(_buffer);
                                        _socket.Send(_buffer);

                                        Monitor.Exit(_socket);
                                    }
                                }
                            }
                            Monitor.Exit(Cryptography);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //SocketDisposed = true;
                Disconnect();
                if (ServerBase.Kernel.GamePool.ContainsKey(Account.EntityID))
                {
                    ServerBase.Kernel.GamePool.Remove(Account.EntityID);
                }
                if (ServerBase.Kernel.WasInGamePool.ContainsKey(Account.EntityID))
                {
                    ServerBase.Kernel.WasInGamePool.Remove(Account.EntityID);
                }
                if (ServerBase.Kernel.AwaitingPool.ContainsKey(Account.EntityID))
                {
                    ServerBase.Kernel.AwaitingPool.Remove(Account.EntityID);
                }

            }
        }
        public void Send(Interfaces.IPacket buffer)
        {
            Send(buffer.ToArray());
        }
        public void SendScreenSpawn(Interfaces.IMapObject obj, bool self)
        {
            try
            {
                foreach (Interfaces.IMapObject _obj in Screen.Objects)
                {
                    if (_obj == null)
                        continue;
                    if (_obj.UID != Entity.UID)
                    {
                        if (_obj.MapObjType == Game.MapObjectType.Player)
                        {
                            byte spawnType = 0;
                            GameState client = _obj.Owner as GameState;
                            obj.SendSpawn(client, false);
                            byte[] array = new byte[Entity.SpawnPacket.Length];
                            Entity.SpawnPacket.CopyTo(array, 0);
                            // array[0x6d] = spawnType;
                            _obj.Owner.Send(array);

                        }
                    }
                }
                if (self)
                    obj.SendSpawn(this);
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
        }
        public void RemoveScreenSpawn(Interfaces.IMapObject obj, bool self)
        {
            try
            {
                foreach (Interfaces.IMapObject _obj in Screen.Objects)
                {
                    if (_obj == null) continue;
                    if (obj == null) continue;
                    if (_obj.UID != Entity.UID)
                    {
                        if (_obj.MapObjType == Game.MapObjectType.Player)
                        {
                            GameState client = _obj.Owner as GameState;
                            client.Screen.Remove(obj);
                        }
                    }
                }
                if (self)
                    Screen.Remove(obj);
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
        }
        public void SendScreen(byte[] buffer, bool self)
        {
            try
            {
                for (int c = 0; c < Screen.Objects.Count; c++)
                {
                    if (c >= Screen.Objects.Count)
                        break;
                    //List<IMapObject> list = new List<IMapObject>();
                    IMapObject obj = Screen.Objects[c];
                    if (obj == null) continue;
                    if (obj.UID != Entity.UID)
                    {
                        if (obj.MapObjType == Game.MapObjectType.Player)
                        {
                            GameState client = obj.Owner as GameState;
                            if (WatchingGroup != null && client.WatchingGroup == null)
                                continue;
                            client.Send(buffer);
                        }
                    }
                }
                if (self)
                    Send(buffer);
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
        }
        public void SendScreen(Interfaces.IPacket buffer, bool self)
        {
            for (int c = 0; c < Screen.Objects.Count; c++)
            {
                if (c >= Screen.Objects.Count)
                    break;
                //List<IMapObject> list = new List<IMapObject>();
                IMapObject obj = Screen.Objects[c];
                if (obj == null) continue;

                if (obj.MapObjType == Game.MapObjectType.Player)
                {
                    GameState client = obj.Owner as GameState;
                    //if (WatchingGroup != null && client.WatchingGroup == null)
                    //   continue;
                    client.Send(buffer);
                }
            }
            if (self)
                Send(buffer);
        }
        public void SendScreen22(Interfaces.IPacket buffer, bool self)
        {
            for (int c = 0; c < Screen.Objects.Count; c++)
            {
                if (c >= Screen.Objects.Count)
                    break;
                //List<IMapObject> list = new List<IMapObject>();
                IMapObject obj = Screen.Objects[c];
                if (obj == null) continue;

                if (obj.MapObjType == Game.MapObjectType.Player)
                {
                    GameState client = obj.Owner as GameState;
                    //if (WatchingGroup != null && client.WatchingGroup == null)
                    //   continue;
                    client.Send(buffer);
                }
            }
            if (self)
                Send(buffer);
        }
        public void Disconnect6()
        {

            if (_socket.Connected)
            {
                SocketDisposed = true;
                _socket.Disconnect(false);
                //_socket.Shutdown(SocketShutdown.Both);
                //_socket.Close();
            }
            //ShutDown();kimo
        }
        public void Disconnect()
        {
            if (Disconnected)
                return;




            try
            {
                if (_socket != null)
                {

                    if (_socket.Connected)
                    {

                        if (!SocketDisposed)
                        {
                            SocketDisposed = true;
                            _socket.Disconnect(false);
                            _socket.Shutdown(SocketShutdown.Both);
                            _socket.Close();
                            _socket = null;
                        }
                    }

                }
                else if (Socket != null)
                {
                    if (Socket.Connected)
                    {

                        if (SocketDisposed)
                        {
                            SocketDisposed = true;
                            Socket.Disconnect(false);
                            Socket.Shutdown(SocketShutdown.Both);
                            Socket.Close();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
            if (this.Screen != null)
            {
                if (this.Screen.MyTimer != null)
                {

                    this.Screen.MyTimer.Close();
                    this.Screen.MyTimer.Dispose();
                }
            }
            if (this.Entity != null)
            {
                if (this.Entity.MyTimer != null)
                {
                    this.Entity.MyTimer.Close();
                    this.Entity.MyTimer.Dispose();
                }

            }
            ShutDown();
        }

        private void ShutDown()
        {


            try
            {
                if (Disconnected)
                    return;
                if (Logger != null)
                {
                    Logger.Close();
                    Logger = null;
                }

                if (this != null && this.Entity != null)
                {
                    if (this.JustCreated)
                        return;
                    if (ServerBase.Kernel.GamePool.ContainsKey(Account.EntityID))
                    {
                        ServerBase.Kernel.GamePool.Remove(Account.EntityID);
                    }
                    if (ServerBase.Kernel.WasInGamePool.ContainsKey(Account.EntityID))
                    {
                        ServerBase.Kernel.WasInGamePool.Remove(Account.EntityID);
                    }
                    //if (ServerBase.Kernel.AwaitingPool.ContainsKey(Account.EntityID))
                    //{
                    //    ServerBase.Kernel.AwaitingPool.Remove(Account.EntityID);
                    //}
                    Time32 now = Time32.Now;
                    RemoveScreenSpawn(this.Entity, false);
                    /* for (byte i = 1; i < 12; i++)
                     {
                         Interfaces.IConquerItem item = this.Equipment.TryGetItem(i);
                         if (item != null && item.ID != 0)
                         {
                             Database.ConquerItemTable.PonerDurabilidad(item);
                         }
                     }*/
                    if (this.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.CaryingFlag))
                    {

                        Game.Team.RedCapture = false;
                        Game.Team.BlueCapture = false;
                        Game.Team.BlackCapture = false;
                        Game.Team.WhiteCapture = false;
                    }
                    if (this.Entity.ContainsFlag(Update.Flags.Dead))
                    {
                        if (this.Entity.MapID == 1038 && DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                        {
                            if (DateTime.Now.Hour >= 19 && DateTime.Now.Hour < 20 && DateTime.Now.Minute > 29)
                            {
                                this.Entity.Teleport(6001, 31, 74);
                            }
                        }
                    }

                    if (Booth != null)
                    {
                        Booth.Remove();
                    }


                    //Database.SubClassTable.save(this.Entity);
                    //Database.FlowerSystemTable.SaveFlowerTable(this);

                    if (Companion != null)
                    {
                        Map.RemoveEntity(Companion);
                        Data data = new Data(true);
                        data.UID = Companion.UID;
                        data.ID = Data.RemoveEntity;
                        Companion.MonsterInfo.SendScreen(data);
                    }
                    if (QualifierGroup != null)
                        QualifierGroup.End(this);


                    if (ArenaStatistic.Status != Network.GamePackets.ArenaStatistic.NotSignedUp)
                        Game.ConquerStructures.Arena.QualifyEngine.DoQuit(this);

                    Database.FlowerSystemTable.SaveFlowerTable(this);
                    Database.EntityTable.SaveEntity(this);

                    Database.SkillTable.SaveProficiencies(this);
                    Database.SkillTable.SaveSpells(this);

                    Database.ArenaTable.SaveArenaStatistics(this.ArenaStatistic);



                    RemoveScreenSpawn(this.Entity, false);
                    Database.EntityTable.UpdateOnlineStatus(this, false);

                    string name200 = Entity.Name;
                    string name300 = Entity.NewName;
                    if (Entity.NewName != "")
                    {
                        // Console.WriteLine("Change Name In Progress");
                        if (Entity.NewName != "")
                        {
                            PhoenixProject.Database.MySqlCommand cmdupdate = null;
                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("apprentice").Set("MentorName", Entity.NewName).Where("MentorID", Entity.UID).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("apprentice").Set("ApprenticeName", Entity.NewName).Where("ApprenticeID", Entity.UID).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("elitepk").Set("Name", Entity.NewName).Where("UID", Entity.UID).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopArcher", Entity.NewName).Where("TopArcher", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopPirate", Entity.NewName).Where("TopPirate", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopTrojan", Entity.NewName).Where("TopTrojan", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopGuildLeader", Entity.NewName).Where("TopGuildLeader", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopNinja", Entity.NewName).Where("TopNinja", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopMonk", Entity.NewName).Where("TopMonk", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopWarrior", Entity.NewName).Where("TopWarrior", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopSpouse", Entity.NewName).Where("TopSpouse", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopWaterTaoist", Entity.NewName).Where("TopWaterTaoist", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopFireTaoist", Entity.NewName).Where("TopFireTaoist", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("MonthlyPkChampion", Entity.NewName).Where("MonthlyPkChampion", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("WeeklyPkChampion", Entity.NewName).Where("WeeklyPkChampion", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopDeputyLeader", Entity.NewName).Where("TopDeputyLeader", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopDeputyLeader2", Entity.NewName).Where("TopDeputyLeader2", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopDeputyLeader3", Entity.NewName).Where("TopDeputyLeader3", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopDeputyLeader4", Entity.NewName).Where("TopDeputyLeader4", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("flags").Set("TopDeputyLeader5", Entity.NewName).Where("TopDeputyLeader5", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("arena").Set("EntityName", Entity.NewName).Where("EntityID", Entity.UID).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("claimitems").Set("OwnerName", Entity.NewName).Where("OwnerName", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("claimitems").Set("GainerName", Entity.NewName).Where("GainerName", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("detaineditems").Set("OwnerName", Entity.NewName).Where("OwnerName", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("detaineditems").Set("GainerName", Entity.NewName).Where("GainerName", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("enemy").Set("EnemyName", Entity.NewName).Where("EnemyID", Entity.UID).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("friends").Set("FriendName", Entity.NewName).Where("FriendID", Entity.UID).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("guilds").Set("Name", Entity.NewName).Where("Name", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("guilds").Set("LeaderName", Entity.NewName).Where("LeaderName", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("clans").Set("Leader", Entity.NewName).Where("Leader", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("nobility").Set("EntityName", Entity.NewName).Where("EntityUID", Entity.UID).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("partners").Set("PartnerName", Entity.NewName).Where("PartnerID", Entity.UID).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("entities").Set("Spouse", Entity.NewName).Where("Spouse", Entity.Name).Execute();

                            cmdupdate = new PhoenixProject.Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE);
                            cmdupdate.Update("entities").Set("Name", Entity.NewName).Where("Name", Entity.Name).Execute();

                            if (Game.ConquerStructures.Nobility.Board.ContainsKey(Entity.UID))
                            {
                                Game.ConquerStructures.Nobility.Board[Entity.UID].Name = Entity.NewName;
                            }
                            if (Game.ConquerStructures.Arena.ArenaStatistics.ContainsKey(Entity.UID))
                            {
                                Game.ConquerStructures.Arena.ArenaStatistics[Entity.UID].Name = Entity.NewName;
                            }

                            if (Guild != null)
                            {
                                if (Guild.LeaderName == name200)
                                {
                                    ServerBase.Kernel.Guilds[Guild.ID].LeaderName = Entity.NewName;
                                    ServerBase.Kernel.Guilds[Guild.ID].Members[Entity.UID].Name = Entity.NewName;
                                }
                            }
                            if (Entity.ClanId != 0 && Entity.Myclan != null)
                            {
                                if (Entity.Myclan.ClanLider == name200)
                                {
                                    PhoenixProject.ServerBase.Kernel.ServerClans[Entity.ClanId].ClanLider = Entity.NewName;
                                    PhoenixProject.ServerBase.Kernel.ServerClans[Entity.ClanId].Members[Entity.UID].Name = Entity.NewName;
                                }
                            }
                            //foreach (Client.GameState c in ServerBase.Kernel.GamePool.Values)
                            //{
                            //   if(c.Enemy.ContainsKey(Entity.UID))
                            //    {

                            //        var packet = new KnownPersons(true)
                            //        {
                            //            UID = Entity.UID,
                            //            Type = KnownPersons.RemovePerson,
                            //            Name = name200,
                            //            Online = false
                            //        };
                            //        c.Send(packet);
                            //        c.Enemy[Entity.UID].Name = Entity.NewName;
                            //        packet.Type = KnownPersons.AddEnemy;
                            //        c.Send(packet);
                            //    }
                            //}
                        }
                    }


                    #region Friend/TradePartner/Apprentice
                    Message msg = new Message("Your friend, " + name200 + ", has logged off.", System.Drawing.Color.Red, Message.TopLeft);
                    if (Friends == null)
                        Friends = new SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.Friend>(100);
                    foreach (Game.ConquerStructures.Society.Friend friend in Friends.Values)
                    {
                        if (friend.IsOnline)
                        {
                            var packet = new KnownPersons(true)
                            {
                                UID = Entity.UID,
                                Type = KnownPersons.RemovePerson,
                                Name = name200,
                                Online = false
                            };
                            friend.Client.Send(packet);
                            if (Entity.NewName != "")
                            {
                                if (friend.Client.Friends.ContainsKey(Entity.UID))
                                {
                                    friend.Client.Friends[Entity.UID].Name = Entity.NewName;
                                }
                            }
                            packet.Type = KnownPersons.AddFriend;
                            if (friend != null)
                            {
                                if (friend.Client != null)
                                {
                                    friend.Client.Send(packet);
                                    friend.Client.Send(msg);
                                }
                            }
                        }
                    }
                    Message msg2 = new Message("Your partner, " + name200 + ", has logged off.", System.Drawing.Color.Red, Message.TopLeft);

                    foreach (Game.ConquerStructures.Society.TradePartner partner in Partners.Values)
                    {
                        if (partner.IsOnline)
                        {
                            var packet = new TradePartner(true)
                            {
                                UID = Entity.UID,
                                Type = TradePartner.BreakPartnership,
                                Name = name200,
                                HoursLeft = (int)(new TimeSpan(partner.ProbationStartedOn.AddDays(3).Ticks).TotalHours - new TimeSpan(DateTime.Now.Ticks).TotalHours),
                                Online = false
                            };
                            partner.Client.Send(packet);
                            if (Entity.NewName != "")
                            {
                                if (partner.Client.Partners.ContainsKey(Entity.UID))
                                {
                                    partner.Client.Partners[Entity.UID].Name = Entity.NewName;
                                }
                            }

                            packet.Type = TradePartner.AddPartner;
                            if (partner != null)
                            {
                                if (partner.Client != null)
                                {
                                    partner.Client.Send(packet);
                                    partner.Client.Send(msg2);
                                }
                            }
                        }
                    }
                    MentorInformation Information = new MentorInformation(true);
                    Information.Mentor_Type = 1;
                    Information.Mentor_ID = Entity.UID;
                    Information.Mentor_Level = Entity.Level;
                    Information.Mentor_Class = Entity.Class;
                    Information.Mentor_PkPoints = Entity.PKPoints;
                    Information.Mentor_Mesh = Entity.Mesh;
                    Information.Mentor_Online = false;
                    Information.String_Count = 3;
                    Information.Mentor_Name = name200;
                    Information.Mentor_Spouse_Name = Entity.Spouse;
                    foreach (var appr in Apprentices.Values)
                    {
                        if (appr.IsOnline)
                        {
                            Information.Apprentice_ID = appr.ID;
                            Information.Enrole_Date = appr.EnroleDate;
                            Information.Apprentice_Name = appr.Name;
                            appr.Client.Send(Information);
                            appr.Client.ReviewMentor();
                            if (Entity.NewName != "")
                            {
                                if (appr.Client.Apprentices.ContainsKey(Entity.UID))
                                {
                                    appr.Client.Apprentices[Entity.UID].Name = Entity.NewName;
                                }
                            }
                        }
                    }
                    if (Mentor != null)
                    {
                        if (Mentor.IsOnline)
                        {
                            ApprenticeInformation AppInfo = new ApprenticeInformation();
                            AppInfo.Apprentice_ID = Entity.UID;
                            AppInfo.Apprentice_Level = Entity.Level;
                            AppInfo.Apprentice_Name = name200;
                            AppInfo.Apprentice_Online = false;
                            AppInfo.Apprentice_Spouse_Name = Entity.Spouse;
                            AppInfo.Enrole_date = Mentor.EnroleDate;
                            AppInfo.Mentor_ID = Mentor.Client.Entity.UID;
                            AppInfo.Mentor_Mesh = Mentor.Client.Entity.Mesh;
                            AppInfo.Mentor_Name = Mentor.Client.Entity.Name;
                            AppInfo.Type = 2;
                            Mentor.Client.Send(AppInfo);
                        }
                    }

                    #endregion
                    if (Team != null)
                    {
                        if (Team.TeamLeader)
                        {
                            Network.GamePackets.Team team = new Team();
                            team.UID = Account.EntityID;
                            team.Type = Network.GamePackets.Team.Dismiss;
                            foreach (Client.GameState Teammate in Team.Teammates)
                            {
                                if (Teammate != null)
                                {
                                    if (Teammate.Entity.UID != Account.EntityID)
                                    {
                                        Teammate.Send(team);
                                        Teammate.Team = null;
                                    }
                                }
                            }
                        }
                        else
                        {
                            Network.GamePackets.Team team = new Team();
                            team.UID = Account.EntityID;
                            team.Type = Network.GamePackets.Team.ExitTeam;
                            foreach (Client.GameState Teammate in Team.Teammates)
                            {
                                if (Teammate != null)
                                {
                                    if (Teammate.Entity.UID != Account.EntityID)
                                    {
                                        Teammate.Send(team);
                                        Teammate.Team.Remove(Account.EntityID);
                                    }
                                }
                            }
                        }
                    }
                    if (Account.TempID == 400)
                    {
                        Account.EntityID = 0;
                        Account.Save();
                    }
                    if (!Disconnected)
                    {
                        Console.WriteLine(this.Entity.Name + " has logged off! Ip:[" + this.Account.IP + "]");
                        Console.Title = "[" + Database.rates.servername + "]Kimo Proj. Start time: " + Program.StartDate.ToString("dd MM yyyy hh:mm") + ". Players online: " + ServerBase.Kernel.GamePool.Count + "/" + Program.PlayerCap + " Max Online: " + Program.MaxOn + "";
                    }
                    Disconnected = true;
                    try
                    {
                        if (_socket != null)
                        {
                            // Console.WriteLine(" Close1 ");
                            if (_socket.Connected)
                            {
                                // Console.WriteLine(" Close2 ");
                                if (!SocketDisposed)
                                {
                                    //Monitor.Exit(_socket);
                                    // Monitor.Exit(Cryptography);
                                    // Console.WriteLine(" Close3 ");
                                    SocketDisposed = true;
                                    _socket.Disconnect(false);
                                    _socket.Shutdown(SocketShutdown.Both);
                                    _socket.Close();
                                    _socket = null;
                                }
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Program.SaveException(e);
                        //Disabled = true;
                    }

                }
            }
            catch (Exception e)
            {
                Program.SaveException(e);
                if (ServerBase.Kernel.GamePool.ContainsKey(Account.EntityID))
                {
                    ServerBase.Kernel.GamePool.Remove(Account.EntityID);
                }
                if (ServerBase.Kernel.WasInGamePool.ContainsKey(Account.EntityID))
                {
                    ServerBase.Kernel.WasInGamePool.Remove(Account.EntityID);
                }
                //if (ServerBase.Kernel.AwaitingPool.ContainsKey(Account.EntityID))
                //{
                //    ServerBase.Kernel.AwaitingPool.Remove(Account.EntityID);
                //}
                RemoveScreenSpawn(this.Entity, false);
                Database.EntityTable.UpdateOnlineStatus(this, false);
                if (Account.TempID == 400)
                {
                    Account.EntityID = 0;
                    Account.Save();
                }
                if (!Disconnected)
                {
                    Console.WriteLine(this.Entity.Name + " has logged off! Ip:[" + this.Account.IP + "] Temp");
                    Console.Title = "[" + Database.rates.servername + "]Kimo Proj. Start time: " + Program.StartDate.ToString("dd MM yyyy hh:mm") + ". Players online: " + ServerBase.Kernel.GamePool.Count + "/" + Program.PlayerCap + " Max Online: " + Program.MaxOn + "";
                }
                Disconnected = true;
                try
                {
                    if (_socket != null)
                    {
                        // Console.WriteLine(" Close1 ");
                        if (_socket.Connected)
                        {
                            // Console.WriteLine(" Close2 ");
                            if (!SocketDisposed)
                            {

                                SocketDisposed = true;
                                _socket.Disconnect(false);
                                _socket.Shutdown(SocketShutdown.Both);
                                _socket.Close();
                                _socket = null;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Program.SaveException(ex);
                    //Disabled = true;
                }

            }
        }

        public WinSocket Socket
        { get { return _socket; } }
        public string IP
        {
            get
            {
                return Socket.RemoteEndPoint.ToString().Split(':')[0];
            }
        }
        #endregion

        #region Game

        public SafeDictionary<uint, DetainedItem> ClaimableItem = new SafeDictionary<uint, DetainedItem>(1000),
                                                  DeatinedItem = new SafeDictionary<uint, DetainedItem>(1000);

        public bool DoSetOffline = true;

        public ushort OnlineTrainingPoints = 0;
        public Time32 LastTrainingPointsUp;

        public List<string> GuildNamesSpawned = new List<string>();

        public byte KylinUpgradeCount = 0;

        public ulong OblivionExperience = 0;
        public byte OblivionKills = 0;

        public int PremShopType = 0;
        public DateTime VIPDate;
        public DateTime LastVote;
        public uint VIPDays;
        public uint DonationPoints;
        public uint VotePoints;

        public Time32 ScreenReloadTime;
        public int MillisecondsScreenReload;
        public bool Reloaded = false;
        public Interfaces.IPacket ReloadWith;

        public ushort VendingDisguise;
        public uint BlessTime;
        public int speedHackSuspiction = 0;
        public int HackTime = 0;
        public Time32 speedHackTime;
        public Time32 speedsleep;
        public Time32 LastPingT;
        public Game.Entity Companion;

        public List<GameState> Prayers;
        public List<GameState> MagicDef;
        public GameState PrayLead;
        public GameState MagicLead;

        public DateTime ChatBanTime;
        public uint ChatBanLasts;
        public bool ChatBanned;

        public byte JewelarLauKind, JewelarLauGems;
        public uint VirtuePoints;
        public DateTime LastLotteryEntry;
        public byte LotteryEntries;
        public bool InLottery;
        public DateTime OfflineTGEnterTime;
        public bool Mining = false;
        public Time32 MiningStamp;
        public ushort Vigor
        {
            get
            {
                if (!Equipment.Free(12))
                    return Equipment.TryGetItem((byte)12).Vigor;
                return 65535;
            }
            set
            {
                if (!Equipment.Free(12))
                    Equipment.TryGetItem((byte)12).Vigor = value;
            }
        }
        public ushort MaxVigor
        {
            get { return (ushort)(30 + Entity.ExtraVigor); }
        }

        public bool HeadgearClaim, NecklaceClaim, ArmorClaim, WeaponClaim, RingClaim, BootsClaim, TowerClaim, FanClaim;
        public string PromoteItemNameNeed
        {
            get
            {
                if (Entity.Class % 10 == 0)
                    return " nothing but";
                if (Entity.Class % 10 == 1)
                    if (Entity.Class / 10 == 4)
                        return " five Euxenite Ores and";
                    else
                        return " nothing but";
                if (Entity.Class % 10 == 2)
                    return " one Emerald and";
                if (Entity.Class % 10 == 3)
                    return " one Meteor and";
                if (Entity.Class % 10 == 4)
                    return " one MoonBox and";
                return " nothing but";
            }
        }
        public byte PromoteItemCountNeed
        {
            get
            {
                if (Entity.Class % 10 == 0)
                    return 0;
                if (Entity.Class % 10 == 1)
                    if (Entity.Class / 10 == 4)
                        return 5;
                    else
                        return 0;
                if (Entity.Class % 10 == 2)
                    return 1;
                if (Entity.Class % 10 == 3)
                    return 1;
                if (Entity.Class % 10 == 4)
                    return 1;
                return 0;
            }
        }
        public uint PromoteItemNeed
        {
            get
            {
                if (Entity.Class % 10 == 0)
                    return 0;
                if (Entity.Class % 10 == 1)
                    if (Entity.Class / 10 == 4)
                        return 1072031;
                    else
                        return 0;
                if (Entity.Class % 10 == 2)
                    return 1080001;
                if (Entity.Class % 10 == 3)
                    return 1088001;
                if (Entity.Class % 10 == 4)
                    return 721020;
                return 0;
            }
        }
        public uint PromoteItemGain
        {
            get
            {
                if (Entity.Class % 10 == 0)
                    return 0;
                if (Entity.Class % 10 == 1)
                    if (Entity.Class / 10 == 4)
                        return 500067;
                    else
                        return 0;
                if (Entity.Class % 10 == 2)
                    return 0;
                if (Entity.Class % 10 == 3)
                    return 700031;
                if (Entity.Class % 10 == 4)
                    return 1088000;
                return 0;
            }
        }
        public uint PromoteLevelNeed
        {
            get
            {
                if (Entity.Class % 10 == 0)
                    return 15;
                if (Entity.Class % 10 == 1)
                    return 40;
                if (Entity.Class % 10 == 2)
                    return 70;
                if (Entity.Class % 10 == 3)
                    return 100;
                if (Entity.Class % 10 == 4)
                    return 110;
                return 0;
            }
        }
        public byte SelectedItem, UpdateType;
        public ushort UplevelProficiency;
        public uint OnHoldGuildJoin = 0;
        public uint OnHoldClanJoin = 0;
        public uint elitepoints = 0;
        public uint eliterank = 0;
        public bool SentRequest = false;
        public bool YellowOn = false;
        public bool RedOn = false;
        public bool CaptureR = false;
        public bool CaptureW = false;
        public bool CaptureB = false;
        public bool CaptureBL = false;
        public bool DemonCave = false;
        public Game.ConquerStructures.Society.Guild Guild;
        public Game.ConquerStructures.Society.Guild.Member AsMember;
        public uint Arsenal_Donation = 0;
        public Game.ConquerStructures.Booth Booth;

        public void ReviewMentor()
        {
            #region NotMentor
            uint nowBP = 0;
            if (Mentor != null)
            {
                if (Mentor.IsOnline)
                {
                    nowBP = (uint)(((Mentor.Client.Entity.BattlePower - Mentor.Client.Entity.ExtraBattlePower) - (Entity.BattlePower - Entity.ExtraBattlePower)) / 3.3F);
                }
            }
            if (nowBP > 200)
                nowBP = 0;
            if (nowBP < 0)
                nowBP = 0;
            if (Entity.ExtraBattlePower != nowBP)
            {
                Entity.ExtraBattlePower = nowBP;
                if (Mentor != null)
                {
                    if (Mentor.IsOnline)
                    {
                        MentorInformation Information = new MentorInformation(true);
                        Information.Mentor_Type = 1;
                        Information.Mentor_ID = Mentor.Client.Entity.UID;
                        Information.Apprentice_ID = Entity.UID;
                        Information.Enrole_Date = Mentor.EnroleDate;
                        Information.Mentor_Level = Mentor.Client.Entity.Level;
                        Information.Mentor_Class = Mentor.Client.Entity.Class;
                        Information.Mentor_PkPoints = Mentor.Client.Entity.PKPoints;
                        Information.Mentor_Mesh = Mentor.Client.Entity.Mesh;
                        Information.Mentor_Online = true;
                        Information.Shared_Battle_Power = (uint)(((Mentor.Client.Entity.BattlePower - Mentor.Client.Entity.ExtraBattlePower) - (Entity.BattlePower - Entity.ExtraBattlePower)) / 3.3F);
                        Information.String_Count = 3;
                        Information.Mentor_Name = Mentor.Client.Entity.Name;
                        Information.Apprentice_Name = Entity.Name;
                        Information.Mentor_Spouse_Name = Mentor.Client.Entity.Spouse;
                        Send(Information);
                    }
                }
            }
            #endregion
            #region Mentor
            foreach (var appr in Apprentices.Values)
            {
                if (appr.IsOnline)
                {
                    uint nowBPs = 0;
                    nowBPs = (uint)(((Entity.BattlePower - Entity.ExtraBattlePower) - (appr.Client.Entity.BattlePower - appr.Client.Entity.ExtraBattlePower)) / 3.3F);
                    if (appr.Client.Entity.ExtraBattlePower != nowBPs)
                    {
                        appr.Client.Entity.ExtraBattlePower = nowBPs;
                        MentorInformation Information = new MentorInformation(true);
                        Information.Mentor_Type = 1;
                        Information.Mentor_ID = Entity.UID;
                        Information.Apprentice_ID = appr.Client.Entity.UID;
                        Information.Enrole_Date = appr.EnroleDate;
                        Information.Mentor_Level = Entity.Level;
                        Information.Mentor_Class = Entity.Class;
                        Information.Mentor_PkPoints = Entity.PKPoints;
                        Information.Mentor_Mesh = Entity.Mesh;
                        Information.Mentor_Online = true;
                        Information.Shared_Battle_Power = nowBPs;
                        Information.String_Count = 3;
                        Information.Mentor_Name = Entity.Name;
                        Information.Apprentice_Name = appr.Client.Entity.Name;
                        Information.Mentor_Spouse_Name = Entity.Spouse;
                        appr.Client.Send(Information);
                    }
                }
            }
            #endregion
        }
        public void AddQuarantineKill()
        {
            quarantineKill++;
            UpdateQuarantineScore();
        }
        public void AddGl()
        {
            TopGlClaim++;
            return;
        }
        public void AddDl()
        {
            TopDlClaim++;
            return;
        }
        public void AddQuarantineDeath()
        {
            quarantineDeath++;
            UpdateQuarantineScore();
        }
        public void UpdateQuarantineScore()
        {
            string[] scores = new string[3];
            scores[0] = "Black team: " + PhoenixProject.Game.ConquerStructures.Quarantine.BlackScore.ToString() + " wins";
            scores[1] = "White team: " + PhoenixProject.Game.ConquerStructures.Quarantine.WhiteScore.ToString() + " wins";
            scores[2] = "Your score: " + quarantineKill + " kills, " + quarantineDeath + " death";
            for (int i = 0; i < scores.Length; i++)
            {
                Message msg = new Message(scores[i], System.Drawing.Color.Red, i == 0 ? Message.FirstRightCorner : Message.ContinueRightCorner);
                Send(msg);
            }
        }
        public void DeamDeathS()
        {
            string[] scores = new string[5];
            scores[0] = "Black Team: " + Game.ConquerStructures.TeamDeathMatchScore.BlackTeamScore + " Score";
            scores[1] = "White Team: " + Game.ConquerStructures.TeamDeathMatchScore.WhiteTeamScore + " Score";
            scores[2] = "Blue  Team: " + Game.ConquerStructures.TeamDeathMatchScore.BlueTeamScore + " Score";
            scores[3] = "Red   Team: " + Game.ConquerStructures.TeamDeathMatchScore.RedTeamScore + " Score";
            scores[4] = "Your Score: " + Entity.TeamDeathMatch_Kills + " kills";
            for (int i = 0; i < scores.Length; i++)
            {
                Message msg = new Message(scores[i], System.Drawing.Color.Red, i == 0 ? Message.FirstRightCorner : Message.ContinueRightCorner);
                Send(msg);
            }
        }
        public void KillTerrorist()
        {
            foreach (Client.GameState Terrorist in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                if (Terrorist.Entity.KillTheTerrorist_IsTerrorist == true && Terrorist.Entity.MapID == 1801)
                    PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Terrorist: " + Terrorist.Entity.Name + " ", System.Drawing.Color.Black, PhoenixProject.Network.GamePackets.Message.FirstRightCorner), PhoenixProject.ServerBase.Kernel.GamePool.Values);
            }
        }
        public void AddBless(uint value)
        {
            Entity.HeavenBlessing += value;
            Entity.Update(Network.GamePackets._String.Effect, "bless", true);
            if (Mentor != null)
            {
                if (Mentor.IsOnline)
                {
                    Mentor.Client.PrizeHeavenBlessing += (ushort)(value / 10 / 60 / 60);
                    AsApprentice = Mentor.Client.Apprentices[Entity.UID];
                }
                if (AsApprentice != null)
                {
                    AsApprentice.Actual_HeavenBlessing += (ushort)(value / 10 / 60 / 60);
                    AsApprentice.Total_HeavenBlessing += (ushort)(value / 10 / 60 / 60);
                    if (Time32.Now > LastMentorSave.AddSeconds(5))
                    {
                        Database.KnownPersons.SaveApprenticeInfo(AsApprentice);
                        LastMentorSave = Time32.Now;
                    }
                }
            }
        }
        public ulong PrizeExperience;
        public ushort PrizeHeavenBlessing;
        public ushort PrizePlusStone;

        public uint MentorApprenticeRequest;
        public uint TradePartnerRequest;
        public uint lastkilled;

        public object[] OnMessageBoxEventParams;
        public Action OnMessageBoxOK;
        public Action OnMessageBoxCANCEL;

        public bool JustLoggedOn = true;

        public Time32 ReviveStamp = Time32.Now;
        public bool Attackable
        {
            get;
            set;
        }

        public Game.ConquerStructures.NobilityInformation NobilityInformation;
        public Game.Entity Entity;
        public Game.Screen Screen;
        public Time32 LastPing = Time32.Now;
        public int PingCount = 0;
        public static ushort NpcTestType = 0;
        public byte TinterItemSelect = 0;
        public DateTime LastDragonBallUse, LastResetTime;
        public byte Action = 0;

        public bool CheerSent = false;
        public Game.ConquerStructures.Arena.QualifierList.QualifierGroup WatchingGroup;
        public byte XPCount = 0;
        public Time32 XPCountStamp = Time32.Now;
        public Time32 XPListStamp = Time32.Now;
        public Game.ConquerStructures.Arena.QualifierList.QualifierGroup QualifierGroup;
        public Network.GamePackets.ArenaStatistic ArenaStatistic;

        public PhoenixProject.Game.ConquerStructures.Trade Trade;
        public byte ExpBalls = 0;
        public uint MoneySave = 0, ActiveNpc = 0;
        public string WarehousePW, TempPassword;
        public bool WarehouseOpen;
        public Time32 CoolStamp;
        public sbyte WarehouseOpenTries;
        public ushort InputLength;
        public PhoenixProject.Game.ConquerStructures.Society.Mentor Mentor;
        public PhoenixProject.Game.ConquerStructures.Society.Apprentice AsApprentice;
        public SafeDictionary<ushort, Interfaces.IProf> Proficiencies;
        public SafeDictionary<ushort, Interfaces.ISkill> Spells;
        public SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.Friend> Friends;
        public SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.Enemy> Enemy;
        public SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.TradePartner> Partners;
        public SafeDictionary<uint, PhoenixProject.Game.ConquerStructures.Society.Apprentice> Apprentices;
        public Game.ConquerStructures.Inventory Inventory;
        public Game.ConquerStructures.Equipment Equipment;
        public SafeDictionary<Game.ConquerStructures.Warehouse.WarehouseID, Game.ConquerStructures.Warehouse> Warehouses;
        public Game.ConquerStructures.Team Team;
        public Time32 lastClientJumpTime = Time32.Now;
        public Time32 lastJumpTime = Time32.Now;
        public int LastJumpTime = 0;
        public short lastJumpDistance = 0;
        public bool DoubleExpToday = false;

        private Game.Map map;
        public Game.Map Map
        {
            get
            {
                if (map == null)
                {
                    ServerBase.Kernel.Maps.TryGetValue(Entity.MapID, out map);
                    /*if (map == null)
                        Entity.MapID = 1005;*/
                    if (map == null)
                        return (map = new PhoenixProject.Game.Map(Entity.MapID, Database.MapsTable.MapInformations[Entity.MapID].BaseID, Database.DMaps.MapPaths[Database.MapsTable.MapInformations[Entity.MapID].BaseID], false));
                }
                else
                {
                    if (map.ID != Entity.MapID)
                    {
                        ServerBase.Kernel.Maps.TryGetValue(Entity.MapID, out map);
                        /*if (map == null)
                            Entity.MapID = 1005;*/
                        if (map == null)
                            return (map = new PhoenixProject.Game.Map(Entity.MapID, Database.MapsTable.MapInformations[Entity.MapID].BaseID, Database.DMaps.MapPaths[Database.MapsTable.MapInformations[Entity.MapID].BaseID], false));
                    }
                    if (Entity.MapID == 1005 || Entity.MapID == 5580 || Entity.MapID == 5570 || Entity.MapID == 1452 || Entity.MapID == 8684 || Entity.MapID == 2057 || Entity.MapID == 5560 || Entity.MapID == 5530 || Entity.MapID == 6630 || Entity.MapID == 6634 || Entity.MapID == 6633 || Entity.MapID == 6632 || Entity.MapID == 6631 || Entity.MapID == 1038 || Entity.MapID == 2636 || Entity.MapID == 2637 || Entity.MapID == 8595 || Entity.MapID == 6060 || Entity.MapID == 6061 || Entity.MapID == 6062 || Entity.MapID == 6063 || Entity.MapID == 6064 || Entity.MapID == 1038 || Entity.MapID == 6066)
                        Entity.RemoveFlag(Network.GamePackets.Update.Flags.Ride);

                }
                return map;
            }
        }

        public uint ExpBall
        {
            get
            {
                ulong exp = Database.DataHolder.LevelExperience(Entity.Level);
                return (uint)(exp * 13000 / (ulong)((Entity.Level * Entity.Level * Entity.Level / 12) + 1));
            }
        }

        public bool AddProficiency(Interfaces.IProf proficiency)
        {
            if (Proficiencies.ContainsKey(proficiency.ID))
            {
                Proficiencies[proficiency.ID].Level = proficiency.Level;
                Proficiencies[proficiency.ID].Experience = proficiency.Experience;
                proficiency.Send(this);
                return false;
            }
            else
            {
                Proficiencies.Add(proficiency.ID, proficiency);
                proficiency.Send(this);
                return true;
            }
        }

        public bool AddSpell(Interfaces.ISkill spell)
        {
            if (Spells.ContainsKey(spell.ID))
            {
                if (Spells[spell.ID].Level < spell.Level)
                {
                    Spells[spell.ID].Level = spell.Level;
                    Spells[spell.ID].Experience = spell.Experience;
                    if (spell.ID != 3060)
                        spell.Send(this);
                }
                return false;
            }
            else
            {
                if (spell.ID == 1045 || spell.ID == 1046)
                {
                    if (Proficiencies.ContainsKey((ushort)Database.SpellTable.SpellInformations[spell.ID][spell.Level].WeaponSubtype))
                    {
                        if (Proficiencies[(ushort)Database.SpellTable.SpellInformations[spell.ID][spell.Level].WeaponSubtype].Level < 5)
                            return false;
                    }
                    else
                        return false;
                }
                Spells.Add(spell.ID, spell);
                if (spell.ID != 3060)
                    spell.Send(this);
                return true;
            }
        }
        public bool RemoveSpell(Interfaces.ISkill spell)
        {
            if (Spells.ContainsKey(spell.ID))
            {
                Spells.Remove(spell.ID);
                Network.GamePackets.Data data = new Data(true);
                data.UID = Entity.UID;
                data.dwParam = spell.ID;
                data.ID = 109;
                Send(data);
                Database.SkillTable.DeleteSpell(this, spell.ID);
                return true;
            }
            return false;
        }
        public bool WentToComplete = false;
        public byte SelectedGem = 0;
        public Time32 LastMentorSave = Time32.Now;
        public void IncreaseExperience(ulong experience, bool addMultiple)
        {
            if (Entity.Dead)
                return;
            byte level = Entity.Level;
            ulong _experience = Entity.Experience;
            ulong prExperienece = experience;
            if (addMultiple)
            {
                if (Entity.VIPLevel > 0)
                    experience *= 3;
                if (Program.Today == DayOfWeek.Saturday || Program.Today == DayOfWeek.Sunday || Program.Today == DayOfWeek.Monday || Program.Today == DayOfWeek.Thursday || Program.Today == DayOfWeek.Tuesday || Program.Today == DayOfWeek.Wednesday || Program.Today == DayOfWeek.Friday)
                    experience *= 8;
                experience *= ServerBase.Constants.ExtraExperienceRate;
                experience += experience * Entity.Gems[3] / 100;
                if (Entity.DoubleExperienceTime > 0)
                    experience *= 2;
                if (Entity.HeavenBlessing > 0)
                    experience += (uint)(experience * 20 / 100);
                if (Entity.Reborn >= 2)
                    experience /= 3;
                if (Map.BaseID == 1039)
                    experience /= 50;
                if (Guild != null)
                {
                    if (Guild.Level > 0)
                    {
                        experience += (ushort)(experience * Guild.Level / 100);
                    }
                }
                prExperienece = experience + (ulong)(experience * ((float)Entity.BattlePower / 100));

                _experience += prExperienece;
            }
            else
                _experience += experience;
            if (Entity.Level < 140)
            {
                while (_experience >= Database.DataHolder.LevelExperience(level) && level < 140)
                {
                    _experience -= Database.DataHolder.LevelExperience(level);
                    level++;
                    if (Entity.Reborn == 1)
                    {
                        if (level >= 130 && Entity.FirstRebornLevel > 130 && level < Entity.FirstRebornLevel)
                            level = Entity.FirstRebornLevel;
                    }
                    else if (Entity.Reborn == 2)
                    {
                        if (level >= 130 && Entity.SecondRebornLevel > 130 && level < Entity.SecondRebornLevel)
                            level = Entity.SecondRebornLevel;
                    }
                    if (Entity.Class >= 10 && Entity.Class <= 15)
                        if (!Spells.ContainsKey(0x456))
                            AddSpell(new Network.GamePackets.Spell(true) { ID = 0x456 });
                    if (Entity.Class >= 50 && Entity.Class <= 55)
                        if (!Spells.ContainsKey(11230))
                            AddSpell(new Network.GamePackets.Spell(true) { ID = 11230 });
                    if (Entity.Class >= 20 && Entity.Class <= 25)
                        if (!Spells.ContainsKey(0x401))
                            AddSpell(new Network.GamePackets.Spell(true) { ID = 0x401 });
                    if (Entity.Class >= 40 && Entity.Class <= 45)
                        if (!Spells.ContainsKey(0x1f42))
                            AddSpell(new Network.GamePackets.Spell(true) { ID = 0x1f42 });
                    if (Entity.Class >= 50 && Entity.Class <= 55)
                        if (!Spells.ContainsKey(0x177b))
                            AddSpell(new Network.GamePackets.Spell(true) { ID = 0x177b });
                    if (Entity.Class >= 60 && Entity.Class <= 65)
                        if (!Spells.ContainsKey(0x2896))
                            AddSpell(new Network.GamePackets.Spell(true) { ID = 0x2896 });
                    if (Entity.Class >= 70 && Entity.Class <= 75)
                        if (!Spells.ContainsKey(0x2b2a))
                            AddSpell(new Network.GamePackets.Spell(true) { ID = 0x2b2a });
                    if (Entity.Class > 100)
                        if (!Spells.ContainsKey(0x3f2))
                            AddSpell(new Network.GamePackets.Spell(true) { ID = 0x3f2 });

                    if (Mentor != null)
                    {
                        if (Mentor.IsOnline)
                        {
                            Mentor.Client.PrizeExperience += (ulong)level;
                            AsApprentice = Mentor.Client.Apprentices[Entity.UID];
                            try
                            {
                                AsApprentice.Actual_Experience += (ulong)level;
                                AsApprentice.Total_Experience += (ulong)level;
                            }
                            catch { }
                            if (Mentor.Client.PrizeExperience > 50 * 606)
                                Mentor.Client.PrizeExperience = 50 * 606;
                        }
                    }
                    if (level == 70)
                    {
                        if (ArenaStatistic == null || ArenaStatistic.EntityID == 0)
                        {
                            ArenaStatistic = new PhoenixProject.Network.GamePackets.ArenaStatistic(true);
                            ArenaStatistic.EntityID = Entity.UID;
                            ArenaStatistic.Name = Entity.Name;
                            ArenaStatistic.Level = Entity.Level;
                            ArenaStatistic.Class = Entity.Class;
                            ArenaStatistic.Model = Entity.Mesh;
                            ArenaStatistic.ArenaPoints = Database.ArenaTable.ArenaPointFill(Entity.Level);
                            ArenaStatistic.LastArenaPointFill = DateTime.Now;
                            Database.ArenaTable.InsertArenaStatistic(this);
                            ArenaStatistic.Status = Network.GamePackets.ArenaStatistic.NotSignedUp;
                            Game.ConquerStructures.Arena.ArenaStatistics.Add(Entity.UID, ArenaStatistic);
                        }
                    }
                    if (Entity.Reborn == 0)
                    {
                        if (level <= 120)
                        {
                            Database.DataHolder.GetStats(Entity.Class, level, this);
                            CalculateStatBonus();
                            CalculateHPBonus();
                            GemAlgorithm();
                        }
                        else
                            Entity.Atributes += 3;
                    }
                    else
                    {
                        Entity.Atributes += 3;
                    }
                }
                if (Entity.Level != level)
                {
                    if (Team != null)
                    {
                        if (Team.LowestLevelsUID == Entity.UID)
                        {
                            Team.LowestLevel = 0;
                            Team.LowestLevelsUID = 0;
                            Team.SearchForLowest();
                        }
                    }
                    Entity.Level = level;
                    Entity.Hitpoints = Entity.MaxHitpoints;
                    Entity.Mana = Entity.MaxMana;
                    if (Entity.Level > 130)
                    {
                        Database.EntityTable.UpdateLevel(Entity.Owner);
                    }
                    _String str = new _String(true);
                    str.UID = Entity.UID;
                    str.TextsCount = 1;
                    str.Type = _String.Effect;
                    str.Texts.Add("xp");
                    SendScreen(str, true);
                }
                if (Entity.Experience != _experience)
                    Entity.Experience = _experience;
                if (Entity.Reborn == 2 && Entity.Level >= 110)
                {
                    if (ServerBase.Kernel.ReincarnatedCharacters.ContainsKey(Entity.UID))
                        Network.PacketHandler.ReincarnationHash(Entity.Owner);
                }
            }
        }

        public void IncreaseSpellExperience(uint experience, ushort id)
        {
            if (Spells.ContainsKey(id))
            {
                switch (id)
                {
                    case 1290:
                    case 5030:
                    case 7030:
                        experience = 100; break;
                }
                experience *= ServerBase.Constants.ExtraSpellRate;
                experience += experience * Entity.Gems[6] / 100;
                if (Map.BaseID == 1039)
                    experience /= 40;
                Interfaces.ISkill spell = Spells[id];
                if (spell == null)
                    return;
                if (Entity.VIPLevel > 0)
                {
                    experience *= 5;
                }
                Database.SpellInformation spellInfo = Database.SpellTable.SpellInformations[spell.ID][spell.Level];
                if (spellInfo != null)
                {
                    if (spellInfo.NeedExperience != 0 && Entity.Level >= spellInfo.NeedLevel)
                    {
                        spell.Experience += experience;
                        bool leveled = false;
                        if (spell.Experience >= spellInfo.NeedExperience)
                        {
                            spell.Experience = 0;
                            spell.Level++;
                            leveled = true;
                            Send(ServerBase.Constants.SpellLeveled);
                        }
                        if (leveled)
                        {
                            spell.Send(this);
                        }
                        else
                        {
                            Network.GamePackets.SkillExperience update = new SkillExperience(true);
                            update.AppendSpell(spell.ID, spell.Experience);
                            update.Send(this);
                        }
                    }
                }
            }
        }

        public void IncreaseProficiencyExperience(uint experience, ushort id)
        {
            if (Proficiencies.ContainsKey(id))
            {
                Interfaces.IProf proficiency = Proficiencies[id];
                experience *= ServerBase.Constants.ExtraProficiencyRate;
                experience += experience * Entity.Gems[5] / 100;
                if (Map.BaseID == 1039)
                    experience /= 40;
                if (Entity.VIPLevel > 0)
                {
                    experience *= 5;
                }
                proficiency.Experience += experience;
                if (proficiency.Level < 20)
                {
                    bool leveled = false;
                    while (proficiency.Experience >= Database.DataHolder.ProficiencyLevelExperience(proficiency.Level))
                    {
                        proficiency.Experience -= Database.DataHolder.ProficiencyLevelExperience(proficiency.Level);
                        proficiency.Level++;
                        if (proficiency.Level == 20)
                        {
                            proficiency.Experience = 0;
                            proficiency.Send(this);
                            Send(ServerBase.Constants.ProficiencyLeveled);
                            return;
                        }
                        leveled = true;
                        Send(ServerBase.Constants.ProficiencyLeveled);
                    }
                    if (leveled)
                    {
                        proficiency.Send(this);
                    }
                    else
                    {
                        Network.GamePackets.SkillExperience update = new SkillExperience(true);
                        update.AppendProficiency(proficiency.ID, proficiency.Experience, Database.DataHolder.ProficiencyLevelExperience(proficiency.Level));
                        update.Send(this);
                    }
                }
            }
            else
            {
                AddProficiency(new Network.GamePackets.Proficiency(true) { ID = id });
            }
        }

        public byte ExtraAtributePoints(byte level, byte mClass)
        {
            if (mClass == 135)
            {
                if (level <= 110)
                    return 0;
                switch (level)
                {
                    case 112: return 1;
                    case 114: return 3;
                    case 116: return 6;
                    case 118: return 10;
                    case 120: return 15;
                    case 121: return 15;
                    case 122: return 21;
                    case 123: return 21;
                    case 124: return 28;
                    case 125: return 28;
                    case 126: return 36;
                    case 127: return 36;
                    case 128: return 45;
                    case 129: return 45;
                    default:
                        return 55;
                }
            }
            else
            {
                if (level <= 120)
                    return 0;
                switch (level)
                {
                    case 121: return 1;
                    case 122: return 3;
                    case 123: return 6;
                    case 124: return 10;
                    case 125: return 15;
                    case 126: return 21;
                    case 127: return 28;
                    case 128: return 36;
                    case 129: return 45;
                    default:
                        return 55;
                }
            }
        }
        public static ISkill LearnableSpell(ushort spellid)
        {
            ISkill spell = new Spell(true);
            spell.ID = spellid;
            return spell;
        }
        public bool Reborn(byte toClass)
        {
            #region Items
            if (Inventory.Count > 38)
                return false;
            switch (toClass)
            {
                case 11:
                case 21:
                case 51:
                case 61:
                case 71:
                    {
                        Inventory.Add(410077, Game.Enums.ItemEffect.Poison);
                        break;
                    }
                case 41:
                    {
                        Inventory.Add(500057, Game.Enums.ItemEffect.Shield);
                        break;
                    }
                case 132:
                case 142:
                    {
                        if (toClass == 132)
                            Inventory.Add(421077, Game.Enums.ItemEffect.MP);
                        else
                            Inventory.Add(421077, Game.Enums.ItemEffect.HP);
                        break;
                    }
            }
            #region Low level items
            for (byte i = 1; i < 9; i++)
            {
                if (i != 7)
                {
                    Interfaces.IConquerItem item = Equipment.TryGetItem(i);
                    if (item != null && item.ID != 0)
                    {
                        try
                        {
                            //UnloadItemStats(item, false);
                            Database.ConquerItemInformation cii = new PhoenixProject.Database.ConquerItemInformation(item.ID, item.Plus);
                            item.ID = cii.LowestID(Network.PacketHandler.ItemMinLevel(Network.PacketHandler.ItemPosition(item.ID)));
                            item.Mode = PhoenixProject.Game.Enums.ItemMode.Update;
                            item.Send(this);
                            LoadItemStats(this.Entity);
                            Database.ConquerItemTable.UpdateItemID(item, this);
                        }
                        catch
                        {
                            Console.WriteLine("Reborn item problem: " + item.ID);
                        }
                    }
                }
            }
            #region Alt Fix By Amjad
            Interfaces.IConquerItem AltRing = Equipment.TryGetItem(26);
            if (AltRing != null)
            {
                Equipment.Remove(26);
                CalculateStatBonus();
                CalculateHPBonus();
            }
            else
                SendScreen(Entity.SpawnPacket, false);
            Interfaces.IConquerItem AltRightHand = Equipment.TryGetItem(24);
            if (AltRightHand != null)
            {
                Equipment.Remove(24);
                CalculateStatBonus();
                CalculateHPBonus();
            }
            else
                SendScreen(Entity.SpawnPacket, false);
            Interfaces.IConquerItem AltNecklace = Equipment.TryGetItem(22);
            if (AltNecklace != null)
            {
                Equipment.Remove(22);
                CalculateStatBonus();
                CalculateHPBonus();
            }
            else
                SendScreen(Entity.SpawnPacket, false);
            Interfaces.IConquerItem AltHead = Equipment.TryGetItem(21);
            if (AltHead != null)
            {
                Equipment.Remove(21);
                CalculateStatBonus();
                CalculateHPBonus();
            }
            else
                SendScreen(Entity.SpawnPacket, false);
            Interfaces.IConquerItem AltLeftHand = Equipment.TryGetItem(25);
            if (AltLeftHand != null)
            {
                Equipment.Remove(25);
                CalculateStatBonus();
                CalculateHPBonus();
            }
            else
                SendScreen(Entity.SpawnPacket, false);
            Interfaces.IConquerItem AltBoots = Equipment.TryGetItem(28);
            if (AltBoots != null)
            {
                Equipment.Remove(28);
                CalculateStatBonus();
                CalculateHPBonus();
            }
            else
                SendScreen(Entity.SpawnPacket, false);
            Interfaces.IConquerItem AltArmor = Equipment.TryGetItem(23);
            if (AltArmor != null)
            {
                Equipment.Remove(23);
                CalculateStatBonus();
                CalculateHPBonus();
            }
            else
                SendScreen(Entity.SpawnPacket, false);

            #endregion
            Interfaces.IConquerItem hand = Equipment.TryGetItem(5);
            if (hand != null)
            {
                Equipment.Remove(5);
                CalculateStatBonus();
                CalculateHPBonus();
            }
            else
                SendScreen(Entity.SpawnPacket, false);
            #endregion
            #endregion
            if (Entity.Reborn == 0)
            {
                Entity.FirstRebornClass = Entity.Class;
                Entity.FirstRebornLevel = Entity.Level;
                Entity.Atributes =
                    (ushort)(ExtraAtributePoints(Entity.FirstRebornClass, Entity.FirstRebornLevel) + 52);
            }
            else
            {
                Entity.SecondRebornClass = Entity.Class;
                Entity.SecondRebornLevel = Entity.Level;
                Entity.Atributes =
                    (ushort)(ExtraAtributePoints(Entity.FirstRebornClass, Entity.FirstRebornLevel) +
                    ExtraAtributePoints(Entity.SecondRebornClass, Entity.SecondRebornLevel) + 62);
            }
            byte PreviousClass = Entity.Class;
            Entity.Reborn++;
            Entity.Class = toClass;
            Entity.Level = 15;
            Entity.Experience = 0;
            #region Spells
            Interfaces.ISkill[] spells = Spells.Values.ToArray();
            foreach (Interfaces.ISkill spell in spells)
            {
                spell.PreviousLevel = spell.Level;
                spell.Level = 0;
                spell.Experience = 0;
                #region Pirate
                if (PreviousClass == 75)
                {
                    if (Entity.Class != 71)
                    {
                        switch (spell.ID)
                        {
                            //BladeTempest =
                            case 11110:
                            //ScurvyBomb =
                            case 11040:
                            //CannonBarrage =
                            case 11050:
                            //BlackbeardRage =
                            case 11060:
                            //GaleBomb = 
                            //case 11070:
                            //KrakensRevenge =
                            case 11100:
                            //BlackSpot =
                            case 11120:
                            //AdrenalineRush =
                            case 11130:
                            //PiEagleEye 
                            case 11030:
                            case 11140:
                                RemoveSpell(spell);
                                break;
                        }
                    }
                }
                #endregion
                #region Monk
                if (PreviousClass == 65)
                {
                    if (Entity.Class != 61)
                    {
                        switch (spell.ID)
                        {
                            case 10490:
                            case 10415:
                            case 10381:
                                RemoveSpell(spell);
                                break;
                        }
                    }
                }
                #endregion
                #region Warrior
                if (PreviousClass == 25)
                {
                    if (Entity.Class != 21)
                    {
                        switch (spell.ID)
                        {
                            case 1025:
                            case 11160:
                            case 11200:
                                if (spell.ID == 1025)
                                {
                                    if (Entity.Class != 132)
                                        RemoveSpell(spell);
                                }
                                else
                                {
                                    RemoveSpell(spell);
                                }

                                break;
                        }
                    }
                }
                #endregion
                #region Ninja
                if (toClass != 51)
                {
                    switch (spell.ID)
                    {
                        case 6010:
                        case 6000:
                        case 6011:
                        case 11170:
                        case 11180:
                        case 11230:
                            RemoveSpell(spell);
                            break;
                    }
                }
                #endregion
                #region Trojan
                if (toClass != 11)
                {
                    switch (spell.ID)
                    {
                        case 1115:
                            RemoveSpell(spell);
                            break;
                    }
                }
                #endregion
                #region Archer
                if (toClass != 41)
                {
                    switch (spell.ID)
                    {
                        case 8001:
                        case 8000:
                        case 8003:
                        case 9000:
                        case 8002:
                        case 8030:
                            RemoveSpell(spell);
                            break;
                    }
                }
                #endregion
                #region WaterTaoist
                if (PreviousClass == 135)
                {
                    if (toClass != 132)
                    {
                        switch (spell.ID)
                        {
                            case 1000:
                            case 1001:
                            case 1010:
                            case 1125:
                            case 1100:
                            case 8030:
                                RemoveSpell(spell);
                                break;
                            case 1050:
                            case 1175:
                            case 1170:
                                if (toClass != 142)
                                    RemoveSpell(spell);
                                break;
                        }
                    }
                }
                #endregion
                #region FireTaoist
                if (PreviousClass == 145)
                {
                    if (toClass != 142)
                    {
                        switch (spell.ID)
                        {
                            case 1000:
                            case 1001:
                            case 1150:
                            case 1180:
                            case 1120:
                            case 1002:
                            case 1160:
                            case 1165:
                                RemoveSpell(spell);
                                break;
                        }
                    }
                }
                #endregion
                if (Spells.ContainsKey(spell.ID))
                    if (spell.ID != (ushort)Game.Enums.SkillIDs.Reflect)
                        spell.Send(this);
            }
            #endregion
            #region Proficiencies
            foreach (Interfaces.IProf proficiency in Proficiencies.Values)
            {
                proficiency.PreviousLevel = proficiency.Level;
                proficiency.Level = 0;
                proficiency.Experience = 0;
                proficiency.Send(this);
            }
            #endregion
            #region Adding earned skills
            if (Entity.Reborn == 2)
                AddSpell(new Spell(true) { ID = 9876 });
            if (toClass == 51 && PreviousClass == 55 && Entity.Reborn == 1)
                AddSpell(new Spell(true) { ID = 6002 });
            if (Entity.FirstRebornClass == 15 && Entity.SecondRebornClass == 15 && Entity.Class == 11 && Entity.Reborn == 2)
                AddSpell(new Spell(true) { ID = 10315 });
            if (Entity.FirstRebornClass == 25 && Entity.SecondRebornClass == 25 && Entity.Class == 21 && Entity.Reborn == 2)
                AddSpell(new Spell(true) { ID = 10311 });
            if (Entity.FirstRebornClass == 45 && Entity.SecondRebornClass == 45 && Entity.Class == 41 && Entity.Reborn == 2)
                AddSpell(new Spell(true) { ID = 10313 });
            if (Entity.FirstRebornClass == 55 && Entity.SecondRebornClass == 55 && Entity.Class == 51 && Entity.Reborn == 2)
                AddSpell(new Spell(true) { ID = 6003 });
            if (Entity.FirstRebornClass == 65 && Entity.SecondRebornClass == 65 && Entity.Class == 61 && Entity.Reborn == 2)
                AddSpell(new Spell(true) { ID = 10405 });
            if (Entity.FirstRebornClass == 135 && Entity.SecondRebornClass == 135 && Entity.Class == 132 && Entity.Reborn == 2)
                AddSpell(new Spell(true) { ID = 30000 });
            if (Entity.FirstRebornClass == 145 && Entity.SecondRebornClass == 145 && Entity.Class == 142 && Entity.Reborn == 2)
                AddSpell(new Spell(true) { ID = 10310 });
            if (Entity.FirstRebornClass == 75 && Entity.SecondRebornClass == 75 && Entity.Class == 71 && Entity.Reborn == 2)
                AddSpell(new Spell(true) { ID = 11040 });
            if (Entity.Reborn == 1)
            {

                if (Entity.FirstRebornClass == 15 && Entity.Class == 11)
                {
                    AddSpell(new Spell(true) { ID = 3050 });
                }
                if (Entity.FirstRebornClass == 75 && Entity.Class == 71)
                {
                    AddSpell(new Spell(true) { ID = 11100 });
                }
                else if (Entity.FirstRebornClass == 25 && Entity.Class == 21)
                {
                    AddSpell(new Spell(true) { ID = 3060 });
                }
                else if (Entity.FirstRebornClass == 145 && Entity.Class == 142)
                {
                    AddSpell(new Spell(true) { ID = 3080 });
                }
                else if (Entity.FirstRebornClass == 135 && Entity.Class == 132)
                {
                    AddSpell(new Spell(true) { ID = 3090 });
                }
            }
            if (Entity.Reborn == 2)
            {

                if (Entity.SecondRebornClass == 15 && Entity.Class == 11)
                {
                    AddSpell(new Spell(true) { ID = 3050 });
                }
                if (Entity.SecondRebornClass == 75 && Entity.Class == 71)
                {
                    AddSpell(new Spell(true) { ID = 11100 });
                }
                else if (Entity.SecondRebornClass == 25)
                {
                    AddSpell(new Spell(true) { ID = 3060 });
                }
                else if (Entity.SecondRebornClass == 145 && Entity.Class == 142)
                {
                    AddSpell(new Spell(true) { ID = 3080 });
                }
                else if (Entity.SecondRebornClass == 135 && Entity.Class == 132)
                {
                    AddSpell(new Spell(true) { ID = 3090 });
                }
            }
            #endregion

            Database.DataHolder.GetStats(Entity.Class, Entity.Level, this);
            CalculateStatBonus();
            CalculateHPBonus();
            GemAlgorithm();

            Database.EntityTable.SaveEntity(this);

            Database.SkillTable.SaveProficiencies(this);
            Database.SkillTable.SaveSpells(this);

            Equipment.UpdateEntityPacket();
            ServerBase.Kernel.SendWorldMessage(new Message("" + Entity.Name + " has got " + Entity.Reborn + " reborns. Congratulations!", System.Drawing.Color.White, Message.Center), ServerBase.Kernel.GamePool.Values);
            return true;
        }
        #region Items
        private int StatHP;
        public void CalculateHPBonus()
        {
            switch (Entity.Class)
            {
                case 11: Entity.MaxHitpoints = (uint)(StatHP * 1.05F); break;
                case 12: Entity.MaxHitpoints = (uint)(StatHP * 1.08F); break;
                case 13: Entity.MaxHitpoints = (uint)(StatHP * 1.10F); break;
                case 14: Entity.MaxHitpoints = (uint)(StatHP * 1.12F); break;
                case 15: Entity.MaxHitpoints = (uint)(StatHP * 1.15F); break;
                default: Entity.MaxHitpoints = (uint)StatHP; break;
            }
            Entity.MaxHitpoints += Entity.ItemHP;
            Entity.Hitpoints = Math.Min(Entity.Hitpoints, Entity.MaxHitpoints);
        }
        public void CalculateStatBonus()
        {
            byte ManaBoost = 5;
            const byte HitpointBoost = 24;
            sbyte Class = (sbyte)(Entity.Class / 10);
            if (Class == 13 || Class == 14)
                ManaBoost += (byte)(5 * (Entity.Class - (Class * 10)));
            StatHP = (ushort)((Entity.Strength * 3) +
                                     (Entity.Agility * 3) +
                                     (Entity.Spirit * 3) +
                                     (Entity.Vitality * HitpointBoost));
            Entity.MaxMana = (ushort)((Entity.Spirit * ManaBoost) + Entity.ItemMP);
            Entity.Mana = Math.Min(Entity.Mana, Entity.MaxMana);
        }
        public void SendStatMessage()
        {
            this.ReviewMentor();
            Network.GamePackets.Message Msg = new PhoenixProject.Network.GamePackets.Message(" Your status has been changed", System.Drawing.Color.DarkGoldenrod
                , Network.GamePackets.Message.TopLeft);
            Msg.__Message = string.Format(Msg.__Message,
                new object[] { Entity.MinAttack, Entity.MaxAttack, Entity.MagicAttack, Entity.Defence, (Entity.MagicDefence + Entity.MagicDefence), Entity.Dodge, Entity.PhysicalDamageDecrease, Entity.MagicDamageDecrease, Entity.PhysicalDamageIncrease, Entity.MagicDamageIncrease, Entity.Hitpoints, Entity.MaxHitpoints, Entity.Mana, Entity.MaxMana, Entity.BattlePower });
            this.Send(Msg);
        }
        public void LoadItemStats2(Game.Entity e)
        {
            #region Set Every Variable to Zero
            Entity.Defence = 0;
            Entity.MagicDefence = 0;
            Entity.Dodge = 0;
            Entity.BaseMagicAttack = 0;
            Entity.WoodResistance = 0;
            Entity.MetalResistance = 0;
            Entity.FireResistance = 0;
            Entity.WaterResistance = 0;
            Entity.EarthResistance = 0;
            Entity.Breaktrough = 0;
            Entity.CriticalStrike = 0;
            Entity.Immunity = 0;
            Entity.Penetration = 0;
            Entity.Counteraction = 0;
            Entity.Block = 0;
            Entity.Detoxication = 0;
            Entity.Intensification = 0;
            Entity.Penetration = 0;
            Entity.SkillCStrike = 0;
            Entity.MaxAttack = 0;
            Entity.MinAttack = 0;
            Entity.PhysicalDamageDecrease = 0;
            Entity.MagicDamageDecrease = 0;
            Entity.MagicDamageIncrease = 0;
            Entity.PhysicalDamageIncrease = 0;
            Entity.MagicDefencePercent = 0;
            Entity.ItemHP = 0;
            Entity.ItemMP = 0;
            Entity.ItemBless = 0;
            Entity.AttackRange = 0;
            Entity.BaseMinAttack = 0;
            Entity.BaseMaxAttack = 0;
            Entity.BaseMagicDefence = 0;
            Entity.BaseDefence = 0;
            Entity.MagicAttack = 0;
            Entity.MagicDefence = 0;
            Entity.DragonGem = 0;
            Entity.PhoenixGem = 0;
            Entity.TortisGem = 0;
            Entity.MagicDefencePercent = 0;
            Entity.MagicDamageIncrease = 0;
            Entity.Gems = new UInt16[10];
            #endregion

            foreach (IConquerItem i in Equipment.Objects)
            {
                if (i != null)
                {

                    if (i.Durability != 0 && i.Position > 9)
                    {
                        if (i.Position != ConquerItem.AltGarment && i.Position != ConquerItem.LeftWeaponAccessory && i.Position != ConquerItem.RightWeaponAccessory)
                        {
                            if (i.Position != ConquerItem.SteedArmor)
                            {
                                Database.ConquerItemInformation dbi = new Database.ConquerItemInformation(i.ID, i.Plus);
                                if (dbi != null)
                                {
                                    /*if (i.ID == 0x493e0)
                                    {
                                        byte r = 0;
                                        byte g = 0;
                                        byte b = 0;
                                        r = (byte)i.kimo1;
                                        g = (byte)i.kimo2;
                                        b = (byte)i.kimo3;
                                        i.Color = (PhoenixProject.Game.Enums.Color)b;
                                        i.Bless = g;
                                        i.Unknown40 = r;

                                    }*/
                                    #region Give Stats.
                                    Refinery.RefineryItem refine = null;
                                    Database.ConquerItemInformation soulDB = new PhoenixProject.Database.ConquerItemInformation(i.Purification.PurificationItemID, 0);
                                    if (i.RefineItem != 0)
                                        refine = i.RefineStats;
                                    if (soulDB != null)
                                    {
                                        Entity.Defence += soulDB.BaseInformation.PhysicalDefence;

                                        //Entity.Defence += soulDB.BaseInformation.PhysicalDefence;
                                        //Entity.Defence += soulDB.BaseInformation.PhysicalDefence;
                                        Entity.MagicDefence += soulDB.BaseInformation.MagicDefence;
                                        Entity.Dodge += (byte)soulDB.BaseInformation.Dodge;
                                        //Entity.Hitpoints += Infos.ItemHP;
                                        Entity.BaseMagicAttack += soulDB.BaseInformation.MagicAttack;
                                        Entity.BaseMinAttack += soulDB.BaseInformation.MinAttack;
                                        Entity.BaseMaxAttack += soulDB.BaseInformation.MaxAttack;


                                        Entity.WoodResistance += soulDB.BaseInformation.WoodResist;
                                        Entity.FireResistance += soulDB.BaseInformation.FireResist;
                                        Entity.WaterResistance += soulDB.BaseInformation.WaterResist;
                                        Entity.EarthResistance += soulDB.BaseInformation.EarthResist;
                                        Entity.Breaktrough += soulDB.BaseInformation.BreakThrough;
                                        Entity.CriticalStrike += soulDB.BaseInformation.CriticalStrike;
                                        Entity.Immunity += soulDB.BaseInformation.Immunity;
                                        Entity.ItemHP += soulDB.BaseInformation.ItemHP;
                                        Entity.ItemMP += soulDB.BaseInformation.ItemMP;
                                        Entity.Penetration += soulDB.BaseInformation.Penetration;
                                        Entity.Counteraction += soulDB.BaseInformation.CounterAction;
                                        Entity.Block += soulDB.BaseInformation.Block;
                                    }
                                    if (refine != null)
                                    {
                                        switch (refine.Type)
                                        {
                                            case Refinery.RefineryItem.RefineryType.Block:
                                                Entity.Block += (UInt16)(refine.Percent * 100);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.BreakThrough:
                                                Entity.Breaktrough += (UInt16)((refine.Percent * 10) + 100);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Counteraction:
                                                Entity.Counteraction += (UInt16)(refine.Percent * 10);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Critical:
                                                Entity.CriticalStrike += (UInt16)((refine.Percent * 100) + 1000);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Detoxication:
                                                Entity.Detoxication += (UInt16)(refine.Percent);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Immunity:
                                                Entity.Immunity += (UInt16)(refine.Percent * 100);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Intensification:
                                                Entity.Intensification += (UInt16)(refine.Percent);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Penetration:
                                                Entity.Penetration += (UInt16)(refine.Percent * 100);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.SCritical:
                                                Entity.SkillCStrike += (UInt16)(refine.Percent * 100);
                                                break;
                                        }
                                    }
                                    if (i.Position == ConquerItem.Tower)
                                    {
                                        Entity.PhysicalDamageDecrease += dbi.BaseInformation.PhysicalDefence;
                                        Entity.MagicDamageDecrease += dbi.BaseInformation.MagicDefence;
                                    }
                                    else
                                    {
                                        Entity.Defence += dbi.BaseInformation.PhysicalDefence;
                                        Entity.MagicDefencePercent += dbi.BaseInformation.MagicDefence;
                                        Entity.Dodge += (byte)dbi.BaseInformation.Dodge;
                                        if (i.Position != ConquerItem.Fan)
                                            Entity.BaseMagicAttack += dbi.BaseInformation.MagicAttack;
                                    }
                                    Entity.ItemHP += dbi.BaseInformation.ItemHP;
                                    Entity.ItemMP += dbi.BaseInformation.ItemMP;
                                    if (i.ID != 0x493e0)
                                    {
                                        Entity.ItemBless += i.Bless;
                                    }
                                    if (i.Position == ConquerItem.AltRightHand)
                                    {
                                        Entity.AttackRange += dbi.BaseInformation.AttackRange;
                                        if (Network.PacketHandler.IsTwoHand(dbi.BaseInformation.ID))
                                            Entity.AttackRange += 3;
                                        else
                                        {
                                            Entity.AttackRange += 2;
                                        }
                                    }
                                    if (i.Position == ConquerItem.AltLeftHand)
                                    {
                                        Entity.BaseMinAttack += (uint)(dbi.BaseInformation.MinAttack * 0.5F);
                                        Entity.BaseMaxAttack += (uint)(dbi.BaseInformation.MaxAttack * 0.5F);
                                    }
                                    else if (i.Position == ConquerItem.Fan)
                                    {
                                        Entity.PhysicalDamageIncrease += dbi.BaseInformation.MinAttack;
                                        Entity.MagicDamageIncrease += dbi.BaseInformation.MagicAttack;
                                    }
                                    else
                                    {
                                        Entity.BaseMinAttack += dbi.BaseInformation.MinAttack;
                                        Entity.BaseMaxAttack += dbi.BaseInformation.MaxAttack;
                                    }
                                    if (i.Plus != 0)
                                    {
                                        if (i.Position == ConquerItem.Tower)
                                        {
                                            Entity.PhysicalDamageDecrease += dbi.PlusInformation.PhysicalDefence;
                                            Entity.MagicDamageDecrease += (ushort)dbi.PlusInformation.MagicDefence;
                                        }
                                        else if (i.Position == ConquerItem.Fan)
                                        {
                                            Entity.PhysicalDamageIncrease += (ushort)dbi.PlusInformation.MinAttack;
                                            Entity.MagicDamageIncrease += (ushort)dbi.PlusInformation.MagicAttack;
                                        }
                                        else
                                        {

                                            Entity.BaseMinAttack += dbi.PlusInformation.MinAttack;
                                            Entity.BaseMaxAttack += dbi.PlusInformation.MaxAttack;
                                            Entity.BaseMagicAttack += dbi.PlusInformation.MagicAttack;
                                            Entity.Defence += dbi.PlusInformation.PhysicalDefence;
                                            Entity.MagicDefence += dbi.PlusInformation.MagicDefence;
                                            Entity.ItemHP += dbi.PlusInformation.ItemHP;

                                            if (i.Position == ConquerItem.AltBoots)
                                            {
                                                Entity.Dodge += (byte)dbi.PlusInformation.Dodge;
                                            }
                                            if (i.Position == ConquerItem.Steed)
                                            {
                                                Entity.ExtraVigor += dbi.PlusInformation.Agility;
                                            }
                                        }
                                    }
                                    byte socketone = (byte)i.SocketOne;
                                    byte sockettwo = (byte)i.SocketTwo;
                                    ushort madd = 0, dadd = 0, aatk = 0, matk = 0;
                                    switch (socketone)
                                    {
                                        case 1: Entity.PhoenixGem += 5; break;
                                        case 2: Entity.PhoenixGem += 10; break;
                                        case 3: Entity.PhoenixGem += 15; break;

                                        case 11: Entity.DragonGem += 5; break;
                                        case 12: Entity.DragonGem += 10; break;
                                        case 13: Entity.DragonGem += 15; break;

                                        case 71: Entity.TortisGem += 15; break;
                                        case 72: Entity.TortisGem += 30; break;
                                        case 73: Entity.TortisGem += 50; break;

                                        case 101: aatk = matk += 100; break;
                                        case 102: aatk = matk += 300; break;
                                        case 103: aatk = matk += 500; break;

                                        case 121: madd = dadd += 100; break;
                                        case 122: madd = dadd += 300; break;
                                        case 123: madd = dadd += 500; break;
                                    }
                                    switch (sockettwo)
                                    {
                                        case 1: Entity.PhoenixGem += 5; break;
                                        case 2: Entity.PhoenixGem += 10; break;
                                        case 3: Entity.PhoenixGem += 15; break;

                                        case 11: Entity.DragonGem += 5; break;
                                        case 12: Entity.DragonGem += 10; break;
                                        case 13: Entity.DragonGem += 15; break;

                                        case 71: Entity.TortisGem += 15; break;
                                        case 72: Entity.TortisGem += 30; break;
                                        case 73: Entity.TortisGem += 50; break;

                                        case 101: aatk = matk += 100; break;
                                        case 102: aatk = matk += 300; break;
                                        case 103: aatk = matk += 500; break;

                                        case 121: madd = dadd += 100; break;
                                        case 122: madd = dadd += 300; break;
                                        case 123: madd = dadd += 500; break;
                                    }
                                    Entity.PhysicalDamageDecrease += dadd;
                                    Entity.MagicDamageDecrease += madd;
                                    Entity.PhysicalDamageIncrease += aatk;
                                    Entity.MagicDamageIncrease += matk;

                                    Entity.ItemHP += i.Enchant;
                                    GemAlgorithm();

                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }
        public void LoadItemStats(Game.Entity e)
        {
            #region Set Every Variable to Zero
            Entity.Defence = 0;
            Entity.MagicDefence = 0;
            Entity.Dodge = 0;
            Entity.BaseMagicAttack = 0;
            Entity.WoodResistance = 0;
            Entity.FireResistance = 0;
            Entity.WaterResistance = 0;
            Entity.EarthResistance = 0;
            Entity.MetalResistance = 0;
            Entity.Breaktrough = 0;
            Entity.CriticalStrike = 0;
            Entity.Immunity = 0;
            Entity.Penetration = 0;
            Entity.Counteraction = 0;
            Entity.Block = 0;
            Entity.Detoxication = 0;
            Entity.Intensification = 0;
            Entity.Penetration = 0;
            Entity.SkillCStrike = 0;
            Entity.MaxAttack = 0;
            Entity.MinAttack = 0;
            Entity.PhysicalDamageDecrease = 0;
            Entity.MagicDamageDecrease = 0;
            Entity.MagicDamageIncrease = 0;
            Entity.PhysicalDamageIncrease = 0;
            Entity.MagicDefencePercent = 0;
            Entity.ItemHP = 0;
            Entity.ItemMP = 0;
            Entity.ItemBless = 0;
            Entity.AttackRange = 0;
            Entity.BaseMinAttack = 0;
            Entity.BaseMaxAttack = 0;
            Entity.BaseMagicDefence = 0;
            Entity.BaseDefence = 0;
            Entity.MagicAttack = 0;
            Entity.MagicDefence = 0;
            Entity.DragonGem = 0;
            Entity.PhoenixGem = 0;
            Entity.TortisGem = 0;
            Entity.MagicDefencePercent = 0;
            Entity.MagicDamageIncrease = 0;
            Entity.Gems = new UInt16[10];
            #endregion

            foreach (IConquerItem i in Equipment.Objects)
            {
                if (i != null)
                {

                    if (i.Durability != 0 && i.Position < 20)
                    {
                        if (i.Position != ConquerItem.Garment && i.Position != ConquerItem.LeftWeaponAccessory && i.Position != ConquerItem.RightWeaponAccessory)
                        {
                            if (i.Position != ConquerItem.SteedArmor)
                            {

                                Database.ConquerItemInformation dbi = new Database.ConquerItemInformation(i.ID, i.Plus);
                                if (dbi != null)
                                {
                                    if (i.ID == 0x493e0)
                                    {
                                        byte r = 0;
                                        byte g = 0;
                                        byte b = 0;
                                        r = (byte)i.kimo1;
                                        g = (byte)i.kimo2;
                                        b = (byte)i.kimo3;
                                        i.Color = (PhoenixProject.Game.Enums.Color)b;
                                        i.Bless = g;
                                        i.Unknown40 = r;
                                        //i.Send(e.Owner);
                                    }

                                    #region Give Stats.
                                    Refinery.RefineryItem refine = null;
                                    Database.ConquerItemInformation soulDB = new PhoenixProject.Database.ConquerItemInformation(i.Purification.PurificationItemID, 0);
                                    if (i.RefineItem != 0)
                                        refine = i.RefineStats;
                                    if (soulDB != null)
                                    {
                                        Entity.Defence += soulDB.BaseInformation.PhysicalDefence;

                                        //Entity.Defence += soulDB.BaseInformation.PhysicalDefence;
                                        //Entity.Defence += soulDB.BaseInformation.PhysicalDefence;
                                        Entity.MagicDefence += soulDB.BaseInformation.MagicDefence;
                                        Entity.Dodge += (byte)soulDB.BaseInformation.Dodge;
                                        //Entity.Hitpoints += Infos.ItemHP;
                                        Entity.BaseMagicAttack += soulDB.BaseInformation.MagicAttack;
                                        Entity.BaseMinAttack += soulDB.BaseInformation.MinAttack;
                                        Entity.BaseMaxAttack += soulDB.BaseInformation.MaxAttack;

                                        // Entity.MetalResistance += soulDB.BaseInformation.me;
                                        Entity.WoodResistance += soulDB.BaseInformation.WoodResist;
                                        Entity.FireResistance += soulDB.BaseInformation.FireResist;
                                        Entity.WaterResistance += soulDB.BaseInformation.WaterResist;
                                        Entity.EarthResistance += soulDB.BaseInformation.EarthResist;
                                        Entity.Breaktrough += soulDB.BaseInformation.BreakThrough;
                                        Entity.CriticalStrike += soulDB.BaseInformation.CriticalStrike;
                                        Entity.Immunity += soulDB.BaseInformation.Immunity;
                                        Entity.ItemHP += soulDB.BaseInformation.ItemHP;
                                        Entity.ItemMP += soulDB.BaseInformation.ItemMP;
                                        Entity.Penetration += soulDB.BaseInformation.Penetration;
                                        Entity.Counteraction += soulDB.BaseInformation.CounterAction;
                                        Entity.Block += soulDB.BaseInformation.Block;
                                    }
                                    if (refine != null)
                                    {
                                        switch (refine.Type)
                                        {
                                            case Refinery.RefineryItem.RefineryType.Block:
                                                Entity.Block += (UInt16)(refine.Percent * 100);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.BreakThrough:
                                                Entity.Breaktrough += (UInt16)((refine.Percent * 10) + 100);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Counteraction:
                                                Entity.Counteraction += (UInt16)(refine.Percent * 10);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Critical:
                                                Entity.CriticalStrike += (UInt16)((refine.Percent * 100) + 1000);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Detoxication:
                                                Entity.Detoxication += (UInt16)(refine.Percent);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Immunity:
                                                Entity.Immunity += (UInt16)(refine.Percent * 100);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Intensification:
                                                Entity.Intensification += (UInt16)(refine.Percent);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.Penetration:
                                                Entity.Penetration += (UInt16)(refine.Percent * 100);
                                                break;
                                            case Refinery.RefineryItem.RefineryType.SCritical:
                                                Entity.SkillCStrike += (UInt16)(refine.Percent * 100);
                                                break;
                                        }
                                    }
                                    if (i.Position == ConquerItem.Tower)
                                    {
                                        Entity.PhysicalDamageDecrease += dbi.BaseInformation.PhysicalDefence;
                                        Entity.MagicDamageDecrease += dbi.BaseInformation.MagicDefence;
                                    }
                                    else
                                    {
                                        Entity.Defence += dbi.BaseInformation.PhysicalDefence;
                                        Entity.MagicDefencePercent += dbi.BaseInformation.MagicDefence;
                                        Entity.Dodge += (byte)dbi.BaseInformation.Dodge;
                                        if (i.Position != ConquerItem.Fan)
                                            Entity.BaseMagicAttack += dbi.BaseInformation.MagicAttack;
                                    }
                                    Entity.ItemHP += dbi.BaseInformation.ItemHP;
                                    Entity.ItemMP += dbi.BaseInformation.ItemMP;
                                    if (i.ID != 0x493e0)
                                    {
                                        Entity.ItemBless += i.Bless;
                                    }
                                    if (i.Position == ConquerItem.RightWeapon)
                                    {
                                        Entity.AttackRange += dbi.BaseInformation.AttackRange;
                                        if (Network.PacketHandler.IsTwoHand(dbi.BaseInformation.ID))
                                            Entity.AttackRange += 3;
                                        else
                                        {
                                            Entity.AttackRange += 2;
                                        }
                                    }
                                    if (i.Position == ConquerItem.LeftWeapon)
                                    {
                                        Entity.BaseMinAttack += (uint)(dbi.BaseInformation.MinAttack * 0.5F);
                                        Entity.BaseMaxAttack += (uint)(dbi.BaseInformation.MaxAttack * 0.5F);
                                    }
                                    else if (i.Position == ConquerItem.Fan)
                                    {
                                        Entity.PhysicalDamageIncrease += dbi.BaseInformation.MinAttack;
                                        Entity.MagicDamageIncrease += dbi.BaseInformation.MagicAttack;
                                    }
                                    else
                                    {
                                        Entity.BaseMinAttack += dbi.BaseInformation.MinAttack;
                                        Entity.BaseMaxAttack += dbi.BaseInformation.MaxAttack;
                                    }
                                    if (i.Plus != 0)
                                    {
                                        if (i.Position == ConquerItem.Tower)
                                        {
                                            Entity.PhysicalDamageDecrease += dbi.PlusInformation.PhysicalDefence;
                                            Entity.MagicDamageDecrease += (ushort)dbi.PlusInformation.MagicDefence;
                                        }
                                        else if (i.Position == ConquerItem.Fan)
                                        {
                                            Entity.PhysicalDamageIncrease += (ushort)dbi.PlusInformation.MinAttack;
                                            Entity.MagicDamageIncrease += (ushort)dbi.PlusInformation.MagicAttack;
                                        }
                                        else
                                        {

                                            Entity.BaseMinAttack += dbi.PlusInformation.MinAttack;
                                            Entity.BaseMaxAttack += dbi.PlusInformation.MaxAttack;
                                            Entity.BaseMagicAttack += dbi.PlusInformation.MagicAttack;
                                            Entity.Defence += dbi.PlusInformation.PhysicalDefence;
                                            Entity.MagicDefence += dbi.PlusInformation.MagicDefence;
                                            Entity.ItemHP += dbi.PlusInformation.ItemHP;
                                            if (i.Position == ConquerItem.Boots)
                                            {
                                                Entity.Dodge += (byte)dbi.PlusInformation.Dodge;
                                            }
                                            if (i.Position == ConquerItem.Steed)
                                            {
                                                Entity.ExtraVigor += dbi.PlusInformation.Agility;
                                            }
                                        }
                                    }
                                    byte socketone = (byte)i.SocketOne;
                                    byte sockettwo = (byte)i.SocketTwo;
                                    ushort madd = 0, dadd = 0, aatk = 0, matk = 0;
                                    switch (socketone)
                                    {
                                        case 1: Entity.PhoenixGem += 5; break;
                                        case 2: Entity.PhoenixGem += 10; break;
                                        case 3: Entity.PhoenixGem += 15; break;

                                        case 11: Entity.DragonGem += 5; break;
                                        case 12: Entity.DragonGem += 10; break;
                                        case 13: Entity.DragonGem += 15; break;

                                        case 71: Entity.TortisGem += 15; break;
                                        case 72: Entity.TortisGem += 30; break;
                                        case 73: Entity.TortisGem += 50; break;

                                        case 101: aatk = matk += 100; break;
                                        case 102: aatk = matk += 300; break;
                                        case 103: aatk = matk += 500; break;

                                        case 121: madd = dadd += 100; break;
                                        case 122: madd = dadd += 300; break;
                                        case 123: madd = dadd += 500; break;
                                    }
                                    switch (sockettwo)
                                    {
                                        case 1: Entity.PhoenixGem += 5; break;
                                        case 2: Entity.PhoenixGem += 10; break;
                                        case 3: Entity.PhoenixGem += 15; break;

                                        case 11: Entity.DragonGem += 5; break;
                                        case 12: Entity.DragonGem += 10; break;
                                        case 13: Entity.DragonGem += 15; break;

                                        case 71: Entity.TortisGem += 15; break;
                                        case 72: Entity.TortisGem += 30; break;
                                        case 73: Entity.TortisGem += 50; break;

                                        case 101: aatk = matk += 100; break;
                                        case 102: aatk = matk += 300; break;
                                        case 103: aatk = matk += 500; break;

                                        case 121: madd = dadd += 100; break;
                                        case 122: madd = dadd += 300; break;
                                        case 123: madd = dadd += 500; break;
                                    }
                                    Entity.PhysicalDamageDecrease += dadd;
                                    Entity.MagicDamageDecrease += madd;
                                    Entity.PhysicalDamageIncrease += aatk;
                                    Entity.MagicDamageIncrease += matk;

                                    Entity.ItemHP += i.Enchant;
                                    GemAlgorithm();

                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }

        #region OLD LOADITEMSTATS
        /*
        public void LoadItemStats(Interfaces.IConquerItem item)//Use to load every item 
        {
            if (item == null)
                return;
            if (item.Durability == 0)
                return;
            if (item.Position == ConquerItem.Garment)
                return;
            Database.ConquerItemInformation Infos = new PhoenixProject.Database.ConquerItemInformation(item.ID, item.Plus);

            if (Infos.BaseInformation == null)
                return;
            Refinery.RefineryItem refine = null;
            Database.ConquerItemInformation soulDB = new PhoenixProject.Database.ConquerItemInformation(item.Purification.PurificationItemID, 0);
            if (item.RefineItem != 0)
                refine = item.RefineStats;
            if (soulDB != null)
            {
                Entity.Defence += soulDB.BaseInformation.PhysicalDefence;
                Entity.MagicDefence += soulDB.BaseInformation.MagicDefence;
                Entity.Dodge += soulDB.BaseInformation.Dodge;
                Entity.BaseMagicAttack += soulDB.BaseInformation.MagicAttack;
                Entity.WoodResistance += soulDB.BaseInformation.WoodResist;
                Entity.FireResistance += soulDB.BaseInformation.FireResist;
                Entity.WaterResistance += soulDB.BaseInformation.WaterResist;
                Entity.EarthResistance += soulDB.BaseInformation.EarthResist;
                Entity.Breaktrough += soulDB.BaseInformation.BreakThrough;
                Entity.CriticalStrike += soulDB.BaseInformation.CriticalStrike;
                Entity.Immunity += soulDB.BaseInformation.Immunity;
                Entity.Penetration += soulDB.BaseInformation.Penetration;
                Entity.Counteraction += soulDB.BaseInformation.CounterAction;
                Entity.Block += soulDB.BaseInformation.Block;
            }
            if (refine != null)
            {
                switch (refine.Type)
                {
                    case Refinery.RefineryItem.RefineryType.Block:
                        Entity.Block += (UInt16)(refine.Percent * 100);
                        break;
                    case Refinery.RefineryItem.RefineryType.BreakThrough:
                        Entity.Breaktrough += (UInt16)((refine.Percent * 10) + 100);
                        break;
                    case Refinery.RefineryItem.RefineryType.Counteraction:
                        Entity.Counteraction += (UInt16)(refine.Percent * 10);
                        break;
                    case Refinery.RefineryItem.RefineryType.Critical:
                        Entity.CriticalStrike += (UInt16)((refine.Percent * 100) + 1000);
                        break;
                    case Refinery.RefineryItem.RefineryType.Detoxication:
                        Entity.Detoxication += (UInt16)(refine.Percent);
                        break;
                    case Refinery.RefineryItem.RefineryType.Immunity:
                        Entity.Immunity += (UInt16)(refine.Percent * 100);
                        break;
                    case Refinery.RefineryItem.RefineryType.Intensification:
                        Entity.Intensification += (UInt16)(refine.Percent);
                        break;
                    case Refinery.RefineryItem.RefineryType.Penetration:
                        Entity.Penetration += (UInt16)(refine.Percent * 100);
                        break;
                    case Refinery.RefineryItem.RefineryType.SCritical:
                        Entity.SkillCStrike += (UInt16)(refine.Percent * 100);
                        break;
                }
            }
            if (item.Position == ConquerItem.Tower)
            {
                Entity.PhysicalDamageDecrease += Infos.BaseInformation.PhysicalDefence;
                Entity.MagicDamageDecrease += Infos.BaseInformation.MagicDefence;
            }
            else
            {
                Entity.Defence += Infos.BaseInformation.PhysicalDefence;
                Entity.MagicDefencePercent += Infos.BaseInformation.MagicDefence;
                Entity.Dodge += (byte)Infos.BaseInformation.Dodge;
                if (item.Position != ConquerItem.Fan)
                    Entity.BaseMagicAttack += Infos.BaseInformation.MagicAttack;
            }
            Entity.ItemHP += Infos.BaseInformation.ItemHP;
            Entity.ItemMP += Infos.BaseInformation.ItemMP;
            Entity.ItemBless += item.Bless;
            if (item.Position == ConquerItem.RightWeapon)
            {
                Entity.AttackRange += Infos.BaseInformation.AttackRange;
                if (Network.PacketHandler.IsTwoHand(Infos.BaseInformation.ID))
                    Entity.AttackRange += 3;
                else
                {
                    Entity.AttackRange += 2;
                }
            }
            if (item.Position == ConquerItem.LeftWeapon)
            {
                Entity.BaseMinAttack += (uint)(Infos.BaseInformation.MinAttack * 0.5F);
                Entity.BaseMaxAttack += (uint)(Infos.BaseInformation.MaxAttack * 0.5F);
            }
            else if (item.Position == ConquerItem.Fan)
            {
                Entity.PhysicalDamageIncrease += Infos.BaseInformation.MinAttack;
                Entity.MagicDamageIncrease += Infos.BaseInformation.MagicAttack;
            }
            else
            {
                Entity.BaseMinAttack += Infos.BaseInformation.MinAttack;
                Entity.BaseMaxAttack += Infos.BaseInformation.MaxAttack;
            }
            if (item.Plus != 0)
            {
                if (item.Position == ConquerItem.Tower)
                {
                    Entity.PhysicalDamageDecrease += Infos.PlusInformation.PhysicalDefence;
                    Entity.MagicDamageDecrease += (ushort)Infos.PlusInformation.MagicDefence;
                }
                else if (item.Position == ConquerItem.Fan)
                {
                    Entity.PhysicalDamageIncrease += (ushort)Infos.PlusInformation.MinAttack;
                    Entity.MagicDamageIncrease += (ushort)Infos.PlusInformation.MagicAttack;
                }
                else
                {
                    if (item.Position == ConquerItem.Steed)
                        Entity.ExtraVigor += Infos.PlusInformation.Agility;
                    Entity.BaseMinAttack += Infos.PlusInformation.MinAttack;
                    Entity.BaseMaxAttack += Infos.PlusInformation.MaxAttack;
                    Entity.BaseMagicAttack += Infos.PlusInformation.MagicAttack;
                    Entity.Defence += Infos.PlusInformation.PhysicalDefence;
                    Entity.MagicDefence += Infos.PlusInformation.MagicDefence;
                    Entity.ItemHP += Infos.PlusInformation.ItemHP;
                    if (item.Position == ConquerItem.Boots)
                        Entity.Dodge += (byte)Infos.PlusInformation.Dodge;
                }
            }
            byte socketone = (byte)item.SocketOne;
            byte sockettwo = (byte)item.SocketTwo;
            ushort madd = 0, dadd = 0, aatk = 0, matk = 0;
            if (item.Position != ConquerItem.Garment &&
                item.Position != ConquerItem.Bottle &&
                item.Position != ConquerItem.Steed)
                switch (socketone)
                {
                    case 1: Entity.PhoenixGem += 5; break;
                        case 2: Entity.PhoenixGem += 10; break;
                        case 3: Entity.PhoenixGem += 15; break;

                        case 11: Entity.DragonGem += 5; break;
                        case 12: Entity.DragonGem += 10; break;
                        case 13: Entity.DragonGem += 15; break;

                        case 71: Entity.TortisGem += 15; break;
                        case 72: Entity.TortisGem += 30; break;
                        case 73: Entity.TortisGem += 50; break;

                    case 101: aatk = matk += 100; break;
                    case 102: aatk = matk += 300; break;
                    case 103: aatk = matk += 500; break;

                    case 121: madd = dadd += 100; break;
                    case 122: madd = dadd += 300; break;
                    case 123: madd = dadd += 500; break;
                }
            if (item.Position != ConquerItem.Garment &&
                 item.Position != ConquerItem.Bottle &&
                 item.Position != ConquerItem.Steed)
                switch (sockettwo)
                {
                   case 1: Entity.PhoenixGem += 5; break;
                        case 2: Entity.PhoenixGem += 10; break;
                        case 3: Entity.PhoenixGem += 15; break;

                        case 11: Entity.DragonGem += 5; break;
                        case 12: Entity.DragonGem += 10; break;
                        case 13: Entity.DragonGem += 15; break;

                        case 71: Entity.TortisGem += 15; break;
                        case 72: Entity.TortisGem += 30; break;
                        case 73: Entity.TortisGem += 50; break;

                    case 101: aatk = matk += 100; break;
                    case 102: aatk = matk += 300; break;
                    case 103: aatk = matk += 500; break;

                    case 121: madd = dadd += 100; break;
                    case 122: madd = dadd += 300; break;
                    case 123: madd = dadd += 500; break;
                }
            Entity.PhysicalDamageDecrease += dadd;
            Entity.MagicDamageDecrease += madd;
            Entity.PhysicalDamageIncrease += aatk;
            Entity.MagicDamageIncrease += matk;
            if (item.Position != ConquerItem.Garment &&
               item.Position != ConquerItem.Bottle)
            {
                Entity.ItemHP += item.Enchant;
                GemAlgorithm();
            }
        }
        public void UnloadItemStats(Interfaces.IConquerItem item, bool onPurpose)
        {
            if (item == null) return;

            if (item.Durability == 0 && !onPurpose)
                return;
            if (item.Position == ConquerItem.Garment)
                return;
            Database.ConquerItemInformation Infos = new PhoenixProject.Database.ConquerItemInformation(item.ID, item.Plus);
            if (Infos.BaseInformation == null)
                return;
            Refinery.RefineryItem refine = null;
            Database.ConquerItemInformation soulDB = new PhoenixProject.Database.ConquerItemInformation(item.Purification.PurificationItemID, 0);
            if (item.RefineItem != 0)
                refine = item.RefineStats;
            if (soulDB != null)
            {
                Entity.Defence -= soulDB.BaseInformation.PhysicalDefence;
                Entity.MagicDefence -= soulDB.BaseInformation.MagicDefence;
                Entity.Dodge -= soulDB.BaseInformation.Dodge;
                Entity.BaseMagicAttack -= soulDB.BaseInformation.MagicAttack;
                Entity.WoodResistance -= soulDB.BaseInformation.WoodResist;
                Entity.FireResistance -= soulDB.BaseInformation.FireResist;
                Entity.WaterResistance -= soulDB.BaseInformation.WaterResist;
                Entity.EarthResistance -= soulDB.BaseInformation.EarthResist;
                Entity.Breaktrough -= soulDB.BaseInformation.BreakThrough;
                Entity.CriticalStrike -= soulDB.BaseInformation.CriticalStrike;
                Entity.Immunity -= soulDB.BaseInformation.Immunity;
                Entity.Penetration -= soulDB.BaseInformation.Penetration;
                Entity.Counteraction -= soulDB.BaseInformation.CounterAction;
                Entity.Block -= soulDB.BaseInformation.Block;
            }
            if (refine != null)
            {
                switch (refine.Type)
                {
                    case Refinery.RefineryItem.RefineryType.Block:
                        Entity.Block -= (UInt16)(refine.Percent * 100);
                        break;
                    case Refinery.RefineryItem.RefineryType.BreakThrough:
                        Entity.Breaktrough -= (UInt16)(refine.Percent * 100);
                        break;
                    case Refinery.RefineryItem.RefineryType.Counteraction:
                        Entity.Counteraction -= (UInt16)(refine.Percent * 10);
                        break;
                    case Refinery.RefineryItem.RefineryType.Critical:
                        Entity.CriticalStrike -= (UInt16)(refine.Percent * 100);
                        break;
                    case Refinery.RefineryItem.RefineryType.Detoxication:
                        Entity.Detoxication -= (UInt16)(refine.Percent);
                        break;
                    case Refinery.RefineryItem.RefineryType.Immunity:
                        Entity.Immunity -= (UInt16)(refine.Percent * 100);
                        break;
                    case Refinery.RefineryItem.RefineryType.Intensification:
                        Entity.Intensification -= (UInt16)(refine.Percent);
                        break;
                    case Refinery.RefineryItem.RefineryType.Penetration:
                        Entity.Penetration -= (UInt16)(refine.Percent * 100);
                        break;
                    case Refinery.RefineryItem.RefineryType.SCritical:
                        Entity.SkillCStrike -= (UInt16)(refine.Percent * 100);
                        break;
                }
            }
            if (item.Position == ConquerItem.Tower)
            {
                Entity.PhysicalDamageDecrease -= Infos.BaseInformation.PhysicalDefence;
                Entity.MagicDamageDecrease -= Infos.BaseInformation.MagicDefence;
            }
            else
            {
                Entity.Defence -= Infos.BaseInformation.PhysicalDefence;
                Entity.MagicDefencePercent -= Infos.BaseInformation.MagicDefence;
                Entity.Dodge -= (byte)Infos.BaseInformation.Dodge;
                if (item.Position != ConquerItem.Fan)
                    Entity.BaseMagicAttack -= Infos.BaseInformation.MagicAttack;
            }

            Entity.ItemHP -= Infos.BaseInformation.ItemHP;
            Entity.ItemMP -= Infos.BaseInformation.ItemMP;
            Entity.ItemBless -= item.Bless;
            if (item.Position == ConquerItem.RightWeapon)
            {
                Entity.AttackRange -= Infos.BaseInformation.AttackRange;
                if (Network.PacketHandler.IsTwoHand(Infos.BaseInformation.ID))
                    Entity.AttackRange -= 2;
            }
            if (item.Position == ConquerItem.LeftWeapon)
            {
                Entity.BaseMinAttack -= (uint)(Infos.BaseInformation.MinAttack * 0.5F);
                Entity.BaseMaxAttack -= (uint)(Infos.BaseInformation.MaxAttack * 0.5F);
            }
            else if (item.Position == ConquerItem.Fan)
            {
                Entity.PhysicalDamageIncrease -= Infos.BaseInformation.MinAttack;
                Entity.MagicDamageIncrease -= Infos.BaseInformation.MagicAttack;
            }
            else
            {
                Entity.BaseMinAttack -= Infos.BaseInformation.MinAttack;
                Entity.BaseMaxAttack -= Infos.BaseInformation.MaxAttack;
            }
            if (item.Plus != 0)
            {
                if (item.Position == ConquerItem.Tower)
                {
                    Entity.PhysicalDamageDecrease -= Infos.PlusInformation.PhysicalDefence;
                    Entity.MagicDamageDecrease -= (ushort)Infos.PlusInformation.MagicDefence;
                }
                else if (item.Position == ConquerItem.Fan)
                {
                    Entity.PhysicalDamageIncrease -= (ushort)Infos.PlusInformation.MinAttack;
                    Entity.MagicDamageIncrease -= (ushort)Infos.PlusInformation.MagicAttack;
                }
                else
                {
                    if (item.Position == ConquerItem.Steed)
                        Entity.ExtraVigor -= Infos.PlusInformation.Agility;
                    Entity.BaseMinAttack -= Infos.PlusInformation.MinAttack;
                    Entity.BaseMaxAttack -= Infos.PlusInformation.MaxAttack;
                    Entity.BaseMagicAttack -= Infos.PlusInformation.MagicAttack;
                    Entity.Defence -= Infos.PlusInformation.PhysicalDefence;
                    Entity.MagicDefence -= Infos.PlusInformation.MagicDefence;
                    Entity.ItemHP -= Infos.PlusInformation.ItemHP;
                    if (item.Position == ConquerItem.Boots)
                        Entity.Dodge -= (byte)Infos.PlusInformation.Dodge;
                }
            }
            byte socketone = (byte)item.SocketOne;
            byte sockettwo = (byte)item.SocketTwo;
            ushort madd = 0, dadd = 0, aatk = 0, matk = 0;
            if (item.Position != ConquerItem.Garment &&
                item.Position != ConquerItem.Bottle &&
                item.Position != ConquerItem.Steed)
                switch (socketone)
                {
                    case 1: Entity.PhoenixGem += 5; break;
                        case 2: Entity.PhoenixGem += 10; break;
                        case 3: Entity.PhoenixGem += 15; break;

                        case 11: Entity.DragonGem += 5; break;
                        case 12: Entity.DragonGem += 10; break;
                        case 13: Entity.DragonGem += 15; break;

                        case 71: Entity.TortisGem += 15; break;
                        case 72: Entity.TortisGem += 30; break;
                        case 73: Entity.TortisGem += 50; break;

                    case 101: aatk = matk += 100; break;
                    case 102: aatk = matk += 300; break;
                    case 103: aatk = matk += 500; break;

                    case 121: madd = dadd += 100; break;
                    case 122: madd = dadd += 300; break;
                    case 123: madd = dadd += 500; break;
                }
            if (item.Position != ConquerItem.Garment &&
                 item.Position != ConquerItem.Bottle &&
                 item.Position != ConquerItem.Steed)
                switch (sockettwo)
                {
                    case 1: Entity.PhoenixGem += 5; break;
                        case 2: Entity.PhoenixGem += 10; break;
                        case 3: Entity.PhoenixGem += 15; break;

                        case 11: Entity.DragonGem += 5; break;
                        case 12: Entity.DragonGem += 10; break;
                        case 13: Entity.DragonGem += 15; break;

                        case 71: Entity.TortisGem += 15; break;
                        case 72: Entity.TortisGem += 30; break;
                        case 73: Entity.TortisGem += 50; break;

                    case 101: aatk = matk += 100; break;
                    case 102: aatk = matk += 300; break;
                    case 103: aatk = matk += 500; break;

                    case 121: madd = dadd += 100; break;
                    case 122: madd = dadd += 300; break;
                    case 123: madd = dadd += 500; break;
                }
            Entity.PhysicalDamageDecrease -= dadd;
            Entity.MagicDamageDecrease -= madd;
            Entity.PhysicalDamageIncrease -= aatk;
            Entity.MagicDamageIncrease -= matk;
            if (item.Position != ConquerItem.Garment &&
                item.Position != ConquerItem.Bottle)
            {
                Entity.ItemHP -= item.Enchant;
                GemAlgorithm();
            }
        }
        */
        #endregion
        public void LoadSoulStats(uint ID)
        {
            var Infos = PhoenixProject.Database.ConquerItemInformation.BaseInformations[ID];

            Entity.Defence += Infos.PhysicalDefence;
            Entity.MagicDefence += Infos.MagicDefence;
            Entity.Dodge += (byte)Infos.Dodge;
            //Entity.max += Infos.ItemHP;
            Entity.BaseMagicAttack += Infos.MagicAttack;
            Entity.BaseMinAttack += Infos.MinAttack;
            Entity.BaseMaxAttack += Infos.MaxAttack;
        }
        public void UnloadSoulStats(uint ID)
        {
            var Infos = PhoenixProject.Database.ConquerItemInformation.BaseInformations[ID];

            Entity.Defence -= Infos.PhysicalDefence;
            Entity.MagicDefence -= Infos.MagicDefence;
            Entity.Dodge -= (byte)Infos.Dodge;
            //Entity.Hitpoints -= Infos.ItemHP;
            Entity.BaseMagicAttack -= Infos.MagicAttack;
            Entity.BaseMinAttack -= Infos.MinAttack;
            Entity.BaseMaxAttack -= Infos.MaxAttack;
        }
        public void GemAlgorithm()
        {
            Entity.MaxAttack = Entity.Strength + Entity.BaseMaxAttack;
            Entity.MinAttack = Entity.Strength + Entity.BaseMinAttack;
            Entity.MagicAttack = Entity.BaseMagicAttack;
            if (Entity.PhoenixGem != 0)
            {
                Entity.MagicAttack += (uint)Math.Floor(Entity.MagicAttack * (double)(Entity.PhoenixGem * 0.01));
            }
            if (Entity.DragonGem != 0)
            {
                Entity.MaxAttack += (uint)Math.Floor(Entity.MaxAttack * (double)(Entity.DragonGem * 0.01));
                Entity.MinAttack += (uint)Math.Floor(Entity.MinAttack * (double)(Entity.DragonGem * 0.01));
            }
            /*if (Entity.TortisGem != 0)
            {
                Entity.Defence += (ushort)Math.Floor(Entity.Defence * (double)(Entity.TortisGem * 0.01));
                Entity.MagicDefence += (ushort)Math.Floor(Entity.MagicDefence * (double)(Entity.TortisGem * 0.01));
            }*/
        }
        #endregion
        #endregion
        public bool IsFairy = false;
        public uint FairyType = 0;
        public uint SType = 0;
    }
}
