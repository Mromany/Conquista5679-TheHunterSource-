using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using PhoenixProject.Interfaces;
using PhoenixProject.Client;
using PhoenixProject.ServerBase;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game
{
    public class Screen
    {

        public double interval = 400;
        public System.Timers.Timer MyTimer;

        public List<Interfaces.IMapObject> Objects;
        public Client.GameState Owner;
       

        private Queue<IMapObject> OnAdd;
        private Queue<IMapObject> OnRemove;

        public Screen(Client.GameState client)
        {
            Owner = client;
            Objects = new List<IMapObject>(10000);
            OnAdd = new Queue<IMapObject>(3000);
            OnRemove = new Queue<IMapObject>(1);
           


            MyTimer = new System.Timers.Timer(interval);
            MyTimer.AutoReset = true;

            MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack);
            MyTimer.Start();
        }

        public void _timerCallBack(object jjjhh, System.Timers.ElapsedEventArgs arg)
        {
            try
            {
                if (Owner.Socket ==null ||!Owner.Socket.Connected || Owner.Entity == null)
                {
                    if (Owner != null)
                    {
                        if (Owner.Screen != null)
                        {
                            if (Owner.Screen.MyTimer != null)
                            {

                                Owner.Screen.MyTimer.Close();
                                Owner.Screen.MyTimer.Dispose();
                            }
                        }
                    }
                    return;
                }
                try
                {
                    while (OnAdd.Count != 0)
                        Objects.Add(OnAdd.Dequeue());
                    while (OnRemove.Count != 0)
                        Objects.Remove(OnRemove.Dequeue());
                }
                catch
                {
                    // Console.WriteLine(e.ToString());
                }
                #region Monsters
               // List<IMapObject> toRemove = new List<IMapObject>();
                if (Owner.Map.FreezeMonsters)
                    return;
                try
                {
                    for (int c = 0; c < Objects.Count; c++)
                    {
                        if (c >= Objects.Count)
                            break;
                        //List<IMapObject> list = new List<IMapObject>();
                        IMapObject obj = Objects[c];
                        if (obj != null)
                        {
                            if (obj.MapObjType == MapObjectType.SobNpc)
                            {
                                /*short distance = Kernel.GetDistance(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);

                                if (distance <= monster.MonsterInfo.AttackRange)
                                {
                                    monster.MonsterInfo.LastMove = Time32.Now;
                                    new Game.Attacking.Handle(null, monster, Owner.Entity);
                                }*/
                                Network.GamePackets.SobNpcSpawn item = obj as Network.GamePackets.SobNpcSpawn;
                                if (item.MapID == 2060)
                                {

                                    if (item.UID == 10011)
                                    {
                                        if (Game.Team.RedCapture)
                                        {
                                            foreach (Interfaces.IMapObject _obj in Objects)
                                            {
                                                if (_obj != null)
                                                {
                                                    if (_obj.MapObjType == MapObjectType.Player)
                                                    {
                                                        (_obj as Entity).Owner.Send(item);
                                                        (_obj as Entity).Owner.Screen.Remove(item);
                                                    }
                                                }
                                            }
                                            Owner.Map.Floor[item.X, item.Y, MapObjectType.SobNpc, null] = true;
                                            Remove(obj);
                                            //toRemove.Add(item);
                                        }
                                    }
                                    if (item.UID == 10021)
                                    {
                                        if (Game.Team.RedCapture)
                                        {

                                            foreach (Interfaces.IMapObject _obj in Objects)
                                            {
                                                if (_obj != null)
                                                {
                                                    if (Owner != null)
                                                    {
                                                        if (Owner.CaptureR && Owner.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.CaryingFlag))
                                                        {
                                                            short distance = Kernel.GetDistance(item.X, item.Y, Owner.Entity.X, Owner.Entity.Y);
                                                            if (distance <= 5)
                                                            {
                                                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                                                                Data datas = new Data(true);
                                                                datas.UID = Owner.Entity.UID;
                                                                datas.ID = 116;
                                                                datas.dwParam = (uint)3145;
                                                                Owner.Send(datas);
                                                                Game.Team.RedCapture = false;
                                                                Game.Team.RScore += 1;
                                                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations! " + Owner.Entity.Name + " scored for the Red team. RedScore: " + Game.Team.RScore + "", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                                                if (Game.Team.RScore >= 10)
                                                                {
                                                                    Game.Team.ClaimRed();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (item.UID == 10022)
                                    {
                                        if (Game.Team.RedCapture)
                                        {

                                            foreach (Interfaces.IMapObject _obj in Objects)
                                            {
                                                if (_obj != null)
                                                {
                                                    if (Owner != null)
                                                    {
                                                        if (Owner.CaptureB && Owner.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.CaryingFlag))
                                                        {
                                                            short distance = Kernel.GetDistance(item.X, item.Y, Owner.Entity.X, Owner.Entity.Y);
                                                            if (distance <= 5)
                                                            {
                                                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                                                                Game.Team.RedCapture = false;
                                                                Game.Team.BScore += 1;
                                                                Data datas = new Data(true);
                                                                datas.UID = Owner.Entity.UID;
                                                                datas.ID = 116;
                                                                datas.dwParam = (uint)3145;
                                                                Owner.Send(datas);
                                                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations! " + Owner.Entity.Name + " scored for the Blue team. BlueScore: " + Game.Team.BScore + "", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                                                if (Game.Team.BScore >= 10)
                                                                {
                                                                    Game.Team.ClaimBLUE();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (item.UID == 10023)
                                    {
                                        if (Game.Team.RedCapture)
                                        {

                                            foreach (Interfaces.IMapObject _obj in Objects)
                                            {
                                                if (_obj != null)
                                                {
                                                    if (Owner != null)
                                                    {
                                                        if (Owner.CaptureW && Owner.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.CaryingFlag))
                                                        {
                                                            short distance = Kernel.GetDistance(item.X, item.Y, Owner.Entity.X, Owner.Entity.Y);
                                                            if (distance <= 5)
                                                            {
                                                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                                                                Game.Team.RedCapture = false;
                                                                Game.Team.WScore += 1;
                                                                Data datas = new Data(true);
                                                                datas.UID = Owner.Entity.UID;
                                                                datas.ID = 116;
                                                                datas.dwParam = (uint)3145;
                                                                Owner.Send(datas);
                                                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations! " + Owner.Entity.Name + " scored for the White team. WhiteScore: " + Game.Team.WScore + "", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                                                if (Game.Team.WScore >= 10)
                                                                {
                                                                    Game.Team.ClaimWhite();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (item.UID == 10024)
                                    {
                                        if (Game.Team.RedCapture)
                                        {

                                            foreach (Interfaces.IMapObject _obj in Objects)
                                            {
                                                if (_obj != null)
                                                {
                                                    if (Owner != null)
                                                    {
                                                        if (Owner.CaptureBL && Owner.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.CaryingFlag))
                                                        {
                                                            short distance = Kernel.GetDistance(item.X, item.Y, Owner.Entity.X, Owner.Entity.Y);
                                                            if (distance <= 5)
                                                            {
                                                                Owner.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                                                                Game.Team.RedCapture = false;
                                                                Game.Team.BLScore += 1;
                                                                Data datas = new Data(true);
                                                                datas.UID = Owner.Entity.UID;
                                                                datas.ID = 116;
                                                                datas.dwParam = (uint)3145;
                                                                Owner.Send(datas);
                                                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations! " + Owner.Entity.Name + " scored for the Black team. BlackScore: " + Game.Team.BLScore + "", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                                                if (Game.Team.BLScore >= 10)
                                                                {
                                                                    Game.Team.ClaimBlack();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (obj.MapObjType == MapObjectType.Monster)
                            {
                                if (Owner != null)
                                {
                                    if (Owner.Map != null)
                                    {
                                        if (Owner.Map != null)
                                        {
                                            Entity monster = null;
                                            if (Objects[c] == null) continue;
                                            monster = Objects[c] as Game.Entity;
                                            if (monster == null) continue;
                                            #region Buffers
                                            if (monster.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                                            {
                                                if (Time32.Now >= monster.StigmaStamp.AddSeconds(monster.StigmaTime))
                                                {
                                                    monster.StigmaTime = 0;
                                                    monster.StigmaIncrease = 0;
                                                    monster.RemoveFlag(Network.GamePackets.Update.Flags.Stigma);
                                                }
                                            }
                                            if (monster.BlackSpotTime2 > 0)
                                            {
                                                if (Time32.Now >= monster.BlackSpotTime.AddSeconds(monster.BlackSpotTime2))
                                                {
                                                    monster.BlackSpotTime2 = 0;
                                                    monster.BlackSpots = false;
                                                    BlackSpot spot = new BlackSpot
                                                    {
                                                        Remove = 1,
                                                        Identifier = monster.UID
                                                    };
                                                    Owner.Send(spot);
                                                }
                                                else
                                                {
                                                    BlackSpot spot = new BlackSpot
                                                    {
                                                        Remove = 0,
                                                        Identifier = monster.UID
                                                    };
                                                    Owner.Send(spot);
                                                }

                                            }
                                            else
                                            {
                                                BlackSpot spot = new BlackSpot
                                                {
                                                    Remove = 1,
                                                    Identifier = monster.UID
                                                };
                                                Owner.Send(spot);
                                            }

                                            if (monster.ContainsFlag(Network.GamePackets.Update.Flags.Dodge))
                                            {
                                                if (Time32.Now >= monster.DodgeStamp.AddSeconds(monster.DodgeTime))
                                                {
                                                    monster.DodgeTime = 0;
                                                    monster.DodgeIncrease = 0;
                                                    monster.RemoveFlag(Network.GamePackets.Update.Flags.Dodge);
                                                }
                                            }
                                            if (monster.ContainsFlag(Network.GamePackets.Update.Flags.Invisibility))
                                            {
                                                if (Time32.Now >= monster.InvisibilityStamp.AddSeconds(monster.InvisibilityTime))
                                                {
                                                    monster.RemoveFlag(Network.GamePackets.Update.Flags.Invisibility);
                                                }
                                            }
                                            if (monster.ContainsFlag(Network.GamePackets.Update.Flags.StarOfAccuracy))
                                            {
                                                if (monster.StarOfAccuracyTime != 0)
                                                {
                                                    if (Time32.Now >= monster.StarOfAccuracyStamp.AddSeconds(monster.StarOfAccuracyTime))
                                                    {
                                                        monster.RemoveFlag(Network.GamePackets.Update.Flags.StarOfAccuracy);
                                                    }
                                                }
                                                else
                                                {
                                                    if (Time32.Now >= monster.AccuracyStamp.AddSeconds(monster.AccuracyTime))
                                                    {
                                                        monster.RemoveFlag(Network.GamePackets.Update.Flags.StarOfAccuracy);
                                                    }
                                                }
                                            }
                                            if (monster.ContainsFlag(Network.GamePackets.Update.Flags.MagicShield))
                                            {
                                                if (monster.MagicShieldTime != 0)
                                                {
                                                    if (Time32.Now >= monster.MagicShieldStamp.AddSeconds(monster.MagicShieldTime))
                                                    {
                                                        monster.MagicShieldIncrease = 0;
                                                        monster.MagicShieldTime = 0;
                                                        monster.RemoveFlag(Network.GamePackets.Update.Flags.MagicShield);
                                                    }
                                                }
                                                else
                                                {
                                                    if (Time32.Now >= monster.ShieldStamp.AddSeconds(monster.ShieldTime))
                                                    {
                                                        monster.ShieldIncrease = 0;
                                                        monster.ShieldTime = 0;
                                                        monster.RemoveFlag(Network.GamePackets.Update.Flags.MagicShield);
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region Dead monster
                                            if (monster.Dead || monster.Killed)
                                            {
                                                if (!monster.ContainsFlag(Network.GamePackets.Update.Flags.Ghost) || monster.Killed)
                                                {
                                                    monster.Killed = false;
                                                    monster.MonsterInfo.InSight = 0;
                                                    monster.MonsterInfo.InStig = 0;
                                                    monster.MonsterInfo.InRev = 0;
                                                    monster.MonsterInfo.InBlack = 0;
                                                    monster.AddFlag(Network.GamePackets.Update.Flags.Ghost);
                                                    monster.AddFlag(Network.GamePackets.Update.Flags.Dead);
                                                    monster.AddFlag(Network.GamePackets.Update.Flags.FadeAway);
                                                    Network.GamePackets.Attack attack = new Network.GamePackets.Attack(true);
                                                    attack.Attacker = monster.Killer.UID;
                                                    attack.Attacked = monster.UID;
                                                    attack.AttackType = Network.GamePackets.Attack.Kill;
                                                    attack.X = monster.X;
                                                    attack.Y = monster.Y;
                                                    Owner.Map.Floor[monster.X, monster.Y, MapObjectType.Monster, monster] = true;
                                                    attack.KOCount = ++monster.Killer.KOCount;
                                                    if (monster.Killer.EntityFlag == EntityFlag.Player)
                                                    {
                                                        monster.MonsterInfo.ExcludeFromSend = monster.Killer.UID;
                                                        monster.Killer.Owner.Send(attack);
                                                        if (monster.Killer.Owner.Companion != null)
                                                        {
                                                            if (monster.Killer.Owner.Companion.MonsterInfo != null)
                                                            {
                                                                monster.Killer.Owner.Map.RemoveEntity(monster.Killer.Owner.Companion);
                                                                Data data = new Data(true);
                                                                data.UID = monster.Killer.Owner.Companion.UID;
                                                                data.ID = Data.RemoveEntity;
                                                                monster.Killer.Owner.Companion.MonsterInfo.SendScreen(data);
                                                                monster.Killer.Owner.Companion = null;
                                                            }
                                                        }
                                                    }
                                                    monster.MonsterInfo.SendScreen(attack);
                                                    monster.MonsterInfo.ExcludeFromSend = 0;
                                                }
                                                if (Time32.Now > monster.DeathStamp.AddSeconds(4))
                                                {
                                                    Network.GamePackets.Data data = new Network.GamePackets.Data(true);
                                                    data.UID = monster.UID;
                                                    data.ID = Network.GamePackets.Data.RemoveEntity;
                                                    monster.MonsterInfo.SendScreen(data);
                                                }
                                            }
                                            #endregion
                                            #region Alive monster
                                            else
                                            {
                                                if ((obj as Entity).Stunned)
                                                {
                                                    if (Time32.Now > (obj as Entity).StunStamp.AddMilliseconds(1500))
                                                        (obj as Entity).Stunned = false;
                                                    else
                                                        continue;
                                                }
                                                if ((obj as Entity).Companion)
                                                    continue;
                                                if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.MinimumSpeed))
                                                {
                                                    #region Guards
                                                    if (monster.Name.Contains("Guard"))
                                                    {
                                                        if (monster.Name == "Guard1" || monster.MonsterInfo.ID >= 9000 && monster.MonsterInfo.ID <= 9003)
                                                        {
                                                            if (monster.MonsterInfo.InSight == 0)
                                                            {
                                                                if (monster.X != monster.MonsterInfo.BoundX || monster.Y != monster.MonsterInfo.BoundY)
                                                                {
                                                                    monster.X = monster.MonsterInfo.BoundX;
                                                                    monster.Y = monster.MonsterInfo.BoundY;
                                                                    Network.GamePackets.TwoMovements jump = new PhoenixProject.Network.GamePackets.TwoMovements();
                                                                    jump.X = monster.MonsterInfo.BoundX;
                                                                    jump.Y = monster.MonsterInfo.BoundY;
                                                                    jump.EntityCount = 1;
                                                                    jump.FirstEntity = monster.UID;
                                                                    jump.MovementType = Network.GamePackets.TwoMovements.Jump;
                                                                    Owner.SendScreen(jump, true);
                                                                }
                                                                if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.FlashingName))
                                                                    monster.MonsterInfo.InSight = Owner.Entity.UID;
                                                            }
                                                            else
                                                            {
                                                                if (Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.FlashingName))
                                                                {
                                                                    if (monster.MonsterInfo.InSight == Owner.Entity.UID)
                                                                    {
                                                                        if (!Owner.Entity.Dead)
                                                                        {
                                                                            if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                            {
                                                                                short distance = Kernel.GetDistance(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);

                                                                                if (distance <= monster.MonsterInfo.AttackRange)
                                                                                {
                                                                                    monster.MonsterInfo.LastMove = Time32.Now;
                                                                                    new Game.Attacking.Handle(null, monster, Owner.Entity);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (distance <= monster.MonsterInfo.ViewRange)
                                                                                    {
                                                                                        Network.GamePackets.TwoMovements jump = new PhoenixProject.Network.GamePackets.TwoMovements();
                                                                                        jump.X = Owner.Entity.X;
                                                                                        jump.Y = Owner.Entity.Y;
                                                                                        monster.X = Owner.Entity.X;
                                                                                        monster.Y = Owner.Entity.Y;
                                                                                        jump.EntityCount = 1;
                                                                                        jump.FirstEntity = monster.UID;
                                                                                        jump.MovementType = Network.GamePackets.TwoMovements.Jump;
                                                                                        Owner.SendScreen(jump, true);
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            monster.MonsterInfo.InSight = 0;
                                                                        }
                                                                    }
                                                                    else
                                                                    {

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (monster.MonsterInfo.InSight == Owner.Entity.UID)
                                                                    {
                                                                        monster.MonsterInfo.InSight = 0;
                                                                    }
                                                                }
                                                            }

                                                            for (int i = 0; i < Objects.Count; i++)
                                                            {
                                                                if (i >= Objects.Count)
                                                                    break;
                                                                IMapObject obj2 = Objects[i];
                                                                if (obj2 == null)
                                                                    continue;
                                                                if (obj2.MapObjType == MapObjectType.Monster)
                                                                {
                                                                    Entity monster2 = obj2 as Entity;//null;// Owner.Map.Entities[obj2.UID];
                                                                    if (monster2 == null)
                                                                        continue;
                                                                    if (monster2.Dead)
                                                                        continue;
                                                                    if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                    {
                                                                        if (!monster2.Name.Contains("Guard") && !monster2.Companion)
                                                                        {
                                                                            short distance = Kernel.GetDistance(monster.X, monster.Y, monster2.X, monster2.Y);

                                                                            if (distance <= monster.MonsterInfo.AttackRange)
                                                                            {
                                                                                monster.MonsterInfo.LastMove = Time32.Now;
                                                                                new Game.Attacking.Handle(null, monster, monster2);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        #region BlackName Guard
                                                        if (monster.Name == "Guard2")
                                                        {
                                                            if (monster.MonsterInfo.InBlack == 0)
                                                            {
                                                                if (monster.X != monster.MonsterInfo.BoundX || monster.Y != monster.MonsterInfo.BoundY)
                                                                {
                                                                    monster.X = monster.MonsterInfo.BoundX;
                                                                    monster.Y = monster.MonsterInfo.BoundY;
                                                                    Network.GamePackets.TwoMovements jump = new PhoenixProject.Network.GamePackets.TwoMovements();
                                                                    jump.X = monster.MonsterInfo.BoundX;
                                                                    jump.Y = monster.MonsterInfo.BoundY;
                                                                    jump.EntityCount = 1;
                                                                    jump.FirstEntity = monster.UID;
                                                                    jump.MovementType = Network.GamePackets.TwoMovements.Jump;
                                                                    Owner.SendScreen(jump, true);
                                                                }
                                                                if (Owner.Entity.PKPoints > 99 || Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.FlashingName))
                                                                    monster.MonsterInfo.InBlack = Owner.Entity.UID;
                                                            }
                                                            else
                                                            {
                                                                if (Owner.Entity.PKPoints > 99 || Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.FlashingName))
                                                                {
                                                                    if (monster.MonsterInfo.InBlack == Owner.Entity.UID)
                                                                    {
                                                                        if (!Owner.Entity.Dead)
                                                                        {
                                                                            if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                            {
                                                                                short distance = Kernel.GetDistance(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);

                                                                                if (distance <= monster.MonsterInfo.AttackRange)
                                                                                {
                                                                                    monster.MonsterInfo.LastMove = Time32.Now;
                                                                                    new Game.Attacking.Handle(null, monster, Owner.Entity);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (distance <= monster.MonsterInfo.ViewRange)
                                                                                    {
                                                                                        Network.GamePackets.TwoMovements jump = new PhoenixProject.Network.GamePackets.TwoMovements();
                                                                                        jump.X = Owner.Entity.X;
                                                                                        jump.Y = Owner.Entity.Y;
                                                                                        monster.X = Owner.Entity.X;
                                                                                        monster.Y = Owner.Entity.Y;
                                                                                        jump.EntityCount = 1;
                                                                                        jump.FirstEntity = monster.UID;
                                                                                        jump.MovementType = Network.GamePackets.TwoMovements.Jump;
                                                                                        Owner.SendScreen(jump, true);
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            monster.MonsterInfo.InBlack = 0;
                                                                        }

                                                                    }
                                                                    else
                                                                    {

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (monster.MonsterInfo.InBlack == Owner.Entity.UID)
                                                                    {
                                                                        monster.MonsterInfo.InBlack = 0;
                                                                    }
                                                                }
                                                            }

                                                            for (int i = 0; i < Objects.Count; i++)
                                                            {
                                                                if (i >= Objects.Count)
                                                                    break;
                                                                IMapObject obj2 = Objects[i];
                                                                if (obj2 == null)
                                                                    continue;
                                                                if (obj2.MapObjType == MapObjectType.Monster)
                                                                {
                                                                    Entity monster2 = obj2 as Entity;//null;// Owner.Map.Entities[obj2.UID];
                                                                    if (monster2 == null)
                                                                        continue;
                                                                    if (monster2.Dead)
                                                                        continue;
                                                                    if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                    {
                                                                        if (!monster2.Name.Contains("Guard") && !monster2.Companion)
                                                                        {
                                                                            short distance = Kernel.GetDistance(monster.X, monster.Y, monster2.X, monster2.Y);

                                                                            if (distance <= monster.MonsterInfo.AttackRange)
                                                                            {
                                                                                monster.MonsterInfo.LastMove = Time32.Now;
                                                                                new Game.Attacking.Handle(null, monster, monster2);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        #endregion
                                                        #region Rev Guard
                                                        if (monster.Name == "RevGuard")
                                                        {
                                                            if (monster.MonsterInfo.InRev == 0)
                                                            {
                                                                if (monster.X != monster.MonsterInfo.BoundX || monster.Y != monster.MonsterInfo.BoundY)
                                                                {
                                                                    monster.X = monster.MonsterInfo.BoundX;
                                                                    monster.Y = monster.MonsterInfo.BoundY;
                                                                    Network.GamePackets.TwoMovements jump = new PhoenixProject.Network.GamePackets.TwoMovements();
                                                                    jump.X = monster.MonsterInfo.BoundX;
                                                                    jump.Y = monster.MonsterInfo.BoundY;
                                                                    jump.EntityCount = 1;
                                                                    jump.FirstEntity = monster.UID;
                                                                    jump.MovementType = Network.GamePackets.TwoMovements.Jump;
                                                                    Owner.SendScreen(jump, true);
                                                                }
                                                                if (Owner.Entity.Dead)
                                                                    monster.MonsterInfo.InRev = Owner.Entity.UID;
                                                            }
                                                            else
                                                            {
                                                                if (Owner.Entity.Dead)
                                                                {
                                                                    if (monster.MonsterInfo.InRev == Owner.Entity.UID)
                                                                    {
                                                                        if (Owner.Entity.Dead)
                                                                        {
                                                                            if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                            {
                                                                                short distance = Kernel.GetDistance(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);

                                                                                if (distance <= monster.MonsterInfo.AttackRange)
                                                                                {
                                                                                    monster.MonsterInfo.LastMove = Time32.Now;
                                                                                    new Game.Attacking.Handle(null, monster, Owner.Entity);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (distance <= monster.MonsterInfo.ViewRange)
                                                                                    {
                                                                                        /*Network.GamePackets.TwoMovements jump = new PhoenixProject.Network.GamePackets.TwoMovements();
                                                                                        jump.X = Owner.Entity.X;
                                                                                        jump.Y = Owner.Entity.Y;
                                                                                        monster.X = Owner.Entity.X;
                                                                                        monster.Y = Owner.Entity.Y;
                                                                                        jump.EntityCount = 1;
                                                                                        jump.FirstEntity = monster.UID;
                                                                                        jump.MovementType = Network.GamePackets.TwoMovements.Jump;
                                                                                        Owner.SendScreen(jump, true);*/
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            monster.MonsterInfo.InRev = 0;
                                                                        }

                                                                    }
                                                                    else
                                                                    {
                                                                        monster.MonsterInfo.InRev = 0;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (monster.MonsterInfo.InRev == Owner.Entity.UID)
                                                                    {
                                                                        monster.MonsterInfo.InRev = 0;
                                                                    }
                                                                }
                                                            }

                                                        }
                                                        #endregion
                                                        #region Stig Guard
                                                        if (monster.Name == "StigGuard")
                                                        {
                                                            if (monster.MonsterInfo.InStig == 0)
                                                            {
                                                                if (monster.X != monster.MonsterInfo.BoundX || monster.Y != monster.MonsterInfo.BoundY)
                                                                {
                                                                    monster.X = monster.MonsterInfo.BoundX;
                                                                    monster.Y = monster.MonsterInfo.BoundY;
                                                                    Network.GamePackets.TwoMovements jump = new PhoenixProject.Network.GamePackets.TwoMovements();
                                                                    jump.X = monster.MonsterInfo.BoundX;
                                                                    jump.Y = monster.MonsterInfo.BoundY;
                                                                    jump.EntityCount = 1;
                                                                    jump.FirstEntity = monster.UID;
                                                                    jump.MovementType = Network.GamePackets.TwoMovements.Jump;
                                                                    Owner.SendScreen(jump, true);
                                                                }
                                                                if (!Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                                                                    monster.MonsterInfo.InStig = Owner.Entity.UID;
                                                            }
                                                            else
                                                            {
                                                                if (!Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                                                                {
                                                                    if (monster.MonsterInfo.InStig == Owner.Entity.UID)
                                                                    {
                                                                        if (!Owner.Entity.Dead)
                                                                        {
                                                                            if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                            {
                                                                                short distance = Kernel.GetDistance(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);

                                                                                if (distance <= monster.MonsterInfo.AttackRange)
                                                                                {
                                                                                    monster.MonsterInfo.LastMove = Time32.Now;
                                                                                    new Game.Attacking.Handle(null, monster, Owner.Entity);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (distance <= monster.MonsterInfo.ViewRange)
                                                                                    {
                                                                                        /*Network.GamePackets.TwoMovements jump = new PhoenixProject.Network.GamePackets.TwoMovements();
                                                                                        jump.X = Owner.Entity.X;
                                                                                        jump.Y = Owner.Entity.Y;
                                                                                        monster.X = Owner.Entity.X;
                                                                                        monster.Y = Owner.Entity.Y;
                                                                                        jump.EntityCount = 1;
                                                                                        jump.FirstEntity = monster.UID;
                                                                                        jump.MovementType = Network.GamePackets.TwoMovements.Jump;
                                                                                        Owner.SendScreen(jump, true);*/
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            monster.MonsterInfo.InStig = 0;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        monster.MonsterInfo.InStig = 0;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (monster.MonsterInfo.InStig == Owner.Entity.UID)
                                                                    {
                                                                        monster.MonsterInfo.InStig = 0;
                                                                    }
                                                                }
                                                            }
                                                            for (int i = 0; i < Objects.Count; i++)
                                                            {
                                                                if (i >= Objects.Count)
                                                                    break;
                                                                IMapObject obj2 = Objects[i];
                                                                if (obj2 == null)
                                                                    continue;
                                                                if (obj2.MapObjType == MapObjectType.Monster)
                                                                {
                                                                    Entity monster2 = obj2 as Entity;//null;// Owner.Map.Entities[obj2.UID];
                                                                    if (monster2 == null)
                                                                        continue;
                                                                    if (monster2.Dead)
                                                                        continue;
                                                                    if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                    {
                                                                        if (!monster2.Name.Contains("Guard") && !monster2.Companion)
                                                                        {
                                                                            short distance = Kernel.GetDistance(monster.X, monster.Y, monster2.X, monster2.Y);

                                                                            if (distance <= monster.MonsterInfo.AttackRange)
                                                                            {
                                                                                monster.MonsterInfo.LastMove = Time32.Now;
                                                                                new Game.Attacking.Handle(null, monster, monster2);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                        }
                                                        #endregion
                                                    #endregion
                                                    }
                                                    else
                                                    {
                                                        #region Monsters

                                                        short distance = Kernel.GetDistance(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);
                                                        if (monster.MonsterInfo.InSight != 0 && monster.MonsterInfo.InSight != Owner.Entity.UID)
                                                        {
                                                            if (monster.MonsterInfo.InSight > 1000000)
                                                            {
                                                                var cl = ServerBase.Kernel.GamePool[monster.MonsterInfo.InSight];
                                                                if (cl == null)
                                                                    monster.MonsterInfo.InSight = 0;
                                                                else
                                                                {
                                                                    short dst = Kernel.GetDistance(monster.X, monster.Y, cl.Entity.X, cl.Entity.Y);
                                                                    if (dst > ServerBase.Constants.pScreenDistance)
                                                                        monster.MonsterInfo.InSight = 0;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Entity companion = null;
                                                                for (int x = 0; x < Owner.Map.Companions.Count; x++)
                                                                {
                                                                    if (x >= Owner.Map.Companions.Count)
                                                                        break;
                                                                    if (Owner.Map.Companions[x] != null)
                                                                    {
                                                                        if (Owner.Map.Companions[x].UID == monster.MonsterInfo.InSight)
                                                                        {
                                                                            companion = Owner.Map.Companions[x];
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                                if (companion == null)
                                                                    monster.MonsterInfo.InSight = 0;
                                                                else
                                                                {
                                                                    short dst = Kernel.GetDistance(monster.X, monster.Y, companion.X, companion.Y);
                                                                    if (dst > ServerBase.Constants.pScreenDistance)
                                                                        monster.MonsterInfo.InSight = 0;
                                                                }
                                                            }
                                                        }
                                                        /*if (monster.MonsterInfo.InRev != 0 && monster.MonsterInfo.InRev != Owner.Entity.UID)
                                                        {
                                                            if (monster.MonsterInfo.InRev > 1000000)
                                                            {
                                                                var cl = ServerBase.Kernel.GamePool[monster.MonsterInfo.InRev];
                                                                if (cl == null)
                                                                    monster.MonsterInfo.InRev = 0;
                                                                else
                                                                {
                                                                    short dst = Kernel.GetDistance(monster.X, monster.Y, cl.Entity.X, cl.Entity.Y);
                                                                    if (dst > ServerBase.Constants.pScreenDistance2 || !cl.Entity.Dead)
                                                                        monster.MonsterInfo.InRev = 0;
                                                                }
                                                            }
                                                    
                                                        }
                                                        if (monster.MonsterInfo.InStig != 0 && monster.MonsterInfo.InStig != Owner.Entity.UID)
                                                        {
                                                            if (monster.MonsterInfo.InStig > 1000000)
                                                            {
                                                                var cl = ServerBase.Kernel.GamePool[monster.MonsterInfo.InStig];
                                                                if (cl == null)
                                                                    monster.MonsterInfo.InStig = 0;
                                                                else
                                                                {
                                                                    short dst = Kernel.GetDistance(monster.X, monster.Y, cl.Entity.X, cl.Entity.Y);
                                                                    if (dst > ServerBase.Constants.pScreenDistance2 || cl.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                                                                        monster.MonsterInfo.InStig = 0;
                                                                }
                                                            }
                                                    
                                                        }
                                                        if (monster.MonsterInfo.InBlack != 0 && monster.MonsterInfo.InBlack != Owner.Entity.UID)
                                                        {
                                                            if (monster.MonsterInfo.InBlack > 1000000)
                                                            {
                                                                var cl = ServerBase.Kernel.GamePool[monster.MonsterInfo.InBlack];
                                                                if (cl == null)
                                                                    monster.MonsterInfo.InBlack = 0;
                                                                else
                                                                {
                                                                    short dst = Kernel.GetDistance(monster.X, monster.Y, cl.Entity.X, cl.Entity.Y);
                                                                    if (dst > ServerBase.Constants.pScreenDistance || cl.Entity.PKPoints < 100)
                                                                        monster.MonsterInfo.InBlack = 0;
                                                                }
                                                            }
                                                   
                                                        }*/
                                                        if (distance <= ServerBase.Constants.pScreenDistance)
                                                        {
                                                            #region Companions
                                                            if (Owner.Companion != null)
                                                            {
                                                                if (Owner.Companion.Companion && !Owner.Companion.Dead)
                                                                {
                                                                    short distance2 = Kernel.GetDistance(monster.X, monster.Y, Owner.Companion.X, Owner.Companion.Y);
                                                                    if (distance > distance2 || Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Invisibility) || Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                                    {
                                                                        if (monster.MonsterInfo.InSight == 0)
                                                                        {
                                                                            monster.MonsterInfo.InSight = Owner.Companion.UID;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (monster.MonsterInfo.InSight == Owner.Companion.UID)
                                                                            {
                                                                                if (distance2 > ServerBase.Constants.pScreenDistance)
                                                                                {
                                                                                    monster.MonsterInfo.InSight = 0;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (distance2 <= monster.MonsterInfo.AttackRange)
                                                                                    {
                                                                                        if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                                        {
                                                                                            monster.MonsterInfo.LastMove = Time32.Now;
                                                                                            new Game.Attacking.Handle(null, monster, Owner.Companion);
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (distance2 > monster.MonsterInfo.ViewRange / 2)
                                                                                        {
                                                                                            if (distance2 < ServerBase.Constants.pScreenDistance)
                                                                                            {
                                                                                                if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.RunSpeed))
                                                                                                {
                                                                                                    monster.MonsterInfo.LastMove = Time32.Now;

                                                                                                    Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Companion.X, Owner.Companion.Y);
                                                                                                    if (!monster.Move(facing))
                                                                                                    {
                                                                                                        facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                                        if (monster.Move(facing))
                                                                                                        {
                                                                                                            monster.Facing = facing;
                                                                                                            Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                            move.Direction = facing;
                                                                                                            move.UID = monster.UID;
                                                                                                            move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                            monster.MonsterInfo.SendScreen(move);
                                                                                                        }
                                                                                                    }
                                                                                                    else
                                                                                                    {
                                                                                                        monster.Facing = facing;
                                                                                                        Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                        move.Direction = facing;
                                                                                                        move.UID = monster.UID;
                                                                                                        move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                        monster.MonsterInfo.SendScreen(move);
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                monster.MonsterInfo.InSight = 0;
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.MoveSpeed))
                                                                                            {
                                                                                                monster.MonsterInfo.LastMove = Time32.Now;
                                                                                                Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Companion.X, Owner.Companion.Y);
                                                                                                if (!monster.Move(facing))
                                                                                                {
                                                                                                    facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                                    if (monster.Move(facing))
                                                                                                    {
                                                                                                        monster.Facing = facing;
                                                                                                        Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                        move.Direction = facing;
                                                                                                        move.UID = monster.UID;
                                                                                                        monster.MonsterInfo.SendScreen(move);
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    monster.Facing = facing;
                                                                                                    Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                    move.Direction = facing;
                                                                                                    move.UID = monster.UID;
                                                                                                    monster.MonsterInfo.SendScreen(move);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                                goto Over;
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                    goto Over;
                                                            }
                                                            else
                                                                goto Over;
                                                            #endregion
                                                        Over:
                                                            #region Player
                                                            if (monster.MonsterInfo.Name.Contains("Guard"))
                                                            {
                                                                if (monster.MonsterInfo.InBlack == 0)
                                                                {
                                                                    if (distance <= monster.MonsterInfo.ViewRange)
                                                                    {
                                                                        if (!Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Invisibility))
                                                                        {
                                                                            if (monster.MonsterInfo.SpellID != 0 || !Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                                            {
                                                                                monster.MonsterInfo.InBlack = Owner.Entity.UID;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (monster.MonsterInfo.InBlack == Owner.Entity.UID)
                                                                    {
                                                                        if (monster.MonsterInfo.SpellID == 0 && Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                                        {
                                                                            monster.MonsterInfo.InBlack = 0;
                                                                            return;
                                                                        }

                                                                        if (Owner.Entity.Dead)
                                                                        {
                                                                            monster.MonsterInfo.InBlack = 0;
                                                                            return;
                                                                        }
                                                                        if (distance > ServerBase.Constants.pScreenDistance)
                                                                        {
                                                                            monster.MonsterInfo.InBlack = 0;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (distance <= monster.MonsterInfo.AttackRange)
                                                                            {
                                                                                if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                                {
                                                                                    monster.MonsterInfo.LastMove = Time32.Now;
                                                                                    new Game.Attacking.Handle(null, monster, Owner.Entity);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (distance > monster.MonsterInfo.ViewRange / 2)
                                                                                {
                                                                                    if (distance < ServerBase.Constants.pScreenDistance)
                                                                                    {
                                                                                        if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.RunSpeed))
                                                                                        {
                                                                                            monster.MonsterInfo.LastMove = Time32.Now;

                                                                                            Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);
                                                                                            if (!monster.Move(facing))
                                                                                            {
                                                                                                facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                                if (monster.Move(facing))
                                                                                                {
                                                                                                    monster.Facing = facing;
                                                                                                    Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                    move.Direction = facing;
                                                                                                    move.UID = monster.UID;
                                                                                                    move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                    monster.MonsterInfo.SendScreen(move);
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                monster.Facing = facing;
                                                                                                Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                move.Direction = facing;
                                                                                                move.UID = monster.UID;
                                                                                                move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                monster.MonsterInfo.SendScreen(move);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        monster.MonsterInfo.InBlack = 0;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.MoveSpeed))
                                                                                    {
                                                                                        monster.MonsterInfo.LastMove = Time32.Now;
                                                                                        Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);
                                                                                        if (!monster.Move(facing))
                                                                                        {
                                                                                            facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                            if (monster.Move(facing))
                                                                                            {
                                                                                                monster.Facing = facing;
                                                                                                Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                move.Direction = facing;
                                                                                                move.UID = monster.UID;
                                                                                                monster.MonsterInfo.SendScreen(move);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            monster.Facing = facing;
                                                                                            Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                            move.Direction = facing;
                                                                                            move.UID = monster.UID;
                                                                                            monster.MonsterInfo.SendScreen(move);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (monster.MonsterInfo.InSight == 0)
                                                            {
                                                                if (!Owner.Entity.Dead)
                                                                {
                                                                    if (distance <= monster.MonsterInfo.ViewRange)
                                                                    {
                                                                        if (!Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Invisibility))
                                                                        {
                                                                            if (monster.MonsterInfo.SpellID != 0 || !Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                                            {
                                                                                monster.MonsterInfo.InSight = Owner.Entity.UID;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!Owner.Entity.Dead)
                                                                {
                                                                    if (monster.MonsterInfo.InSight == Owner.Entity.UID)
                                                                    {
                                                                        if (monster.MonsterInfo.SpellID == 0 && Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                                        {
                                                                            monster.MonsterInfo.InSight = 0;
                                                                            return;
                                                                        }

                                                                        if (Owner.Entity.Dead)
                                                                        {
                                                                            monster.MonsterInfo.InSight = 0;
                                                                            return;
                                                                        }
                                                                        if (distance > ServerBase.Constants.pScreenDistance)
                                                                        {
                                                                            monster.MonsterInfo.InSight = 0;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (!Owner.Entity.Dead)
                                                                            {
                                                                                if (distance <= monster.MonsterInfo.AttackRange)
                                                                                {
                                                                                    if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                                    {
                                                                                        monster.MonsterInfo.LastMove = Time32.Now;
                                                                                        new Game.Attacking.Handle(null, monster, Owner.Entity);
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (distance > monster.MonsterInfo.ViewRange / 2)
                                                                                    {
                                                                                        if (distance < ServerBase.Constants.pScreenDistance)
                                                                                        {
                                                                                            if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.RunSpeed))
                                                                                            {
                                                                                                monster.MonsterInfo.LastMove = Time32.Now;

                                                                                                Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);
                                                                                                if (!monster.Move(facing))
                                                                                                {
                                                                                                    facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                                    if (monster.Move(facing))
                                                                                                    {
                                                                                                        monster.Facing = facing;
                                                                                                        Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                        move.Direction = facing;
                                                                                                        move.UID = monster.UID;
                                                                                                        move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                        monster.MonsterInfo.SendScreen(move);
                                                                                                    }
                                                                                                }
                                                                                                else
                                                                                                {
                                                                                                    monster.Facing = facing;
                                                                                                    Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                    move.Direction = facing;
                                                                                                    move.UID = monster.UID;
                                                                                                    move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                    monster.MonsterInfo.SendScreen(move);
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            monster.MonsterInfo.InSight = 0;
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.MoveSpeed))
                                                                                        {
                                                                                            monster.MonsterInfo.LastMove = Time32.Now;
                                                                                            Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);
                                                                                            if (!monster.Move(facing))
                                                                                            {
                                                                                                facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                                if (monster.Move(facing))
                                                                                                {
                                                                                                    monster.Facing = facing;
                                                                                                    Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                    move.Direction = facing;
                                                                                                    move.UID = monster.UID;
                                                                                                    monster.MonsterInfo.SendScreen(move);
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                monster.Facing = facing;
                                                                                                Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                move.Direction = facing;
                                                                                                move.UID = monster.UID;
                                                                                                monster.MonsterInfo.SendScreen(move);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (Owner.Entity.Dead)
                                                                                {
                                                                                    monster.MonsterInfo.InSight = 0;
                                                                                    // Console.WriteLine("cheack1");
                                                                                    return;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (monster.MonsterInfo.Name.Contains("Guard"))
                                                            {
                                                                if (monster.MonsterInfo.InRev == 0)
                                                                {
                                                                    if (distance <= monster.MonsterInfo.ViewRange)
                                                                    {
                                                                        if (!Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Invisibility))
                                                                        {
                                                                            if (monster.MonsterInfo.SpellID != 0 || !Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                                            {
                                                                                monster.MonsterInfo.InRev = Owner.Entity.UID;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (monster.MonsterInfo.InRev == Owner.Entity.UID)
                                                                    {
                                                                        if (monster.MonsterInfo.SpellID == 0 && Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                                        {
                                                                            monster.MonsterInfo.InRev = 0;
                                                                            return;
                                                                        }

                                                                        if (!Owner.Entity.Dead)
                                                                        {
                                                                            monster.MonsterInfo.InRev = 0;
                                                                            return;
                                                                        }
                                                                        if (distance > ServerBase.Constants.pScreenDistance)
                                                                        {
                                                                            monster.MonsterInfo.InRev = 0;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (distance <= monster.MonsterInfo.AttackRange)
                                                                            {
                                                                                if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                                {
                                                                                    monster.MonsterInfo.LastMove = Time32.Now;
                                                                                    new Game.Attacking.Handle(null, monster, Owner.Entity);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (distance > monster.MonsterInfo.ViewRange / 2)
                                                                                {
                                                                                    if (distance < ServerBase.Constants.pScreenDistance)
                                                                                    {
                                                                                        if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.RunSpeed))
                                                                                        {
                                                                                            monster.MonsterInfo.LastMove = Time32.Now;

                                                                                            Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);
                                                                                            if (!monster.Move(facing))
                                                                                            {
                                                                                                facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                                if (monster.Move(facing))
                                                                                                {
                                                                                                    monster.Facing = facing;
                                                                                                    Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                    move.Direction = facing;
                                                                                                    move.UID = monster.UID;
                                                                                                    move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                    monster.MonsterInfo.SendScreen(move);
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                monster.Facing = facing;
                                                                                                Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                move.Direction = facing;
                                                                                                move.UID = monster.UID;
                                                                                                move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                monster.MonsterInfo.SendScreen(move);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        monster.MonsterInfo.InRev = 0;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.MoveSpeed))
                                                                                    {
                                                                                        monster.MonsterInfo.LastMove = Time32.Now;
                                                                                        Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);
                                                                                        if (!monster.Move(facing))
                                                                                        {
                                                                                            facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                            if (monster.Move(facing))
                                                                                            {
                                                                                                monster.Facing = facing;
                                                                                                Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                move.Direction = facing;
                                                                                                move.UID = monster.UID;
                                                                                                monster.MonsterInfo.SendScreen(move);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            monster.Facing = facing;
                                                                                            Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                            move.Direction = facing;
                                                                                            move.UID = monster.UID;
                                                                                            monster.MonsterInfo.SendScreen(move);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (monster.MonsterInfo.Name.Contains("Guard"))
                                                            {
                                                                if (monster.MonsterInfo.InStig == 0)
                                                                {
                                                                    if (distance <= monster.MonsterInfo.ViewRange)
                                                                    {
                                                                        if (!Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Invisibility))
                                                                        {
                                                                            if (monster.MonsterInfo.SpellID != 0 || !Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                                            {
                                                                                monster.MonsterInfo.InStig = Owner.Entity.UID;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (monster.MonsterInfo.InStig == Owner.Entity.UID)
                                                                    {
                                                                        if (monster.MonsterInfo.SpellID == 0 && Owner.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                                        {
                                                                            monster.MonsterInfo.InStig = 0;
                                                                            return;
                                                                        }

                                                                        if (Owner.Entity.Dead)
                                                                        {
                                                                            monster.MonsterInfo.InStig = 0;
                                                                            return;
                                                                        }
                                                                        if (distance > ServerBase.Constants.pScreenDistance)
                                                                        {
                                                                            monster.MonsterInfo.InStig = 0;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (distance <= monster.MonsterInfo.AttackRange)
                                                                            {
                                                                                if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                                                {
                                                                                    monster.MonsterInfo.LastMove = Time32.Now;
                                                                                    new Game.Attacking.Handle(null, monster, Owner.Entity);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                if (distance > monster.MonsterInfo.ViewRange / 2)
                                                                                {
                                                                                    if (distance < ServerBase.Constants.pScreenDistance)
                                                                                    {
                                                                                        if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.RunSpeed))
                                                                                        {
                                                                                            monster.MonsterInfo.LastMove = Time32.Now;

                                                                                            Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);
                                                                                            if (!monster.Move(facing))
                                                                                            {
                                                                                                facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                                if (monster.Move(facing))
                                                                                                {
                                                                                                    monster.Facing = facing;
                                                                                                    Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                    move.Direction = facing;
                                                                                                    move.UID = monster.UID;
                                                                                                    move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                    monster.MonsterInfo.SendScreen(move);
                                                                                                }
                                                                                            }
                                                                                            else
                                                                                            {
                                                                                                monster.Facing = facing;
                                                                                                Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                move.Direction = facing;
                                                                                                move.UID = monster.UID;
                                                                                                move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                                                                monster.MonsterInfo.SendScreen(move);
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        monster.MonsterInfo.InStig = 0;
                                                                                    }
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (Time32.Now >= monster.MonsterInfo.LastMove.AddMilliseconds(monster.MonsterInfo.MoveSpeed))
                                                                                    {
                                                                                        monster.MonsterInfo.LastMove = Time32.Now;
                                                                                        Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y);
                                                                                        if (!monster.Move(facing))
                                                                                        {
                                                                                            facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                                                                            if (monster.Move(facing))
                                                                                            {
                                                                                                monster.Facing = facing;
                                                                                                Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                                move.Direction = facing;
                                                                                                move.UID = monster.UID;
                                                                                                monster.MonsterInfo.SendScreen(move);
                                                                                            }
                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            monster.Facing = facing;
                                                                                            Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                                                            move.Direction = facing;
                                                                                            move.UID = monster.UID;
                                                                                            monster.MonsterInfo.SendScreen(move);
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                            #endregion
                                                        }
                                                        else
                                                        {
                                                            Remove(obj);
                                                        }
                                                    }
                                                        #endregion
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            if (obj.MapObjType == MapObjectType.Item)
                            {
                                Network.GamePackets.FloorItem item = obj as Network.GamePackets.FloorItem;
                                if (item.Type != Network.GamePackets.FloorItem.Effect)
                                {
                                    if (Time32.Now > item.OnFloor.AddSeconds(ServerBase.Constants.FloorItemSeconds))
                                    {
                                        item.Type = Network.GamePackets.FloorItem.Remove;
                                        foreach (Interfaces.IMapObject _obj in Objects)
                                        {
                                            if (_obj != null)
                                            {
                                                if (_obj.MapObjType == MapObjectType.Player)
                                                {
                                                    (_obj as Entity).Owner.Send(item);
                                                }
                                            }
                                        }
                                        Owner.Map.Floor[item.X, item.Y, MapObjectType.Item, null] = true;
                                        Remove(obj);
                                    }
                                }
                                else
                                {
                                    // Console.WriteLine("zaza");
                                    item.Type = Network.GamePackets.FloorItem.Effect;
                                    foreach (Interfaces.IMapObject _obj in Objects)
                                    {
                                        if (_obj != null)
                                        {
                                            if (_obj.MapObjType == MapObjectType.Player)
                                            {
                                                (_obj as Entity).Owner.Send(item);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e) { Program.SaveException(e); }

               /* foreach (IMapObject obj in toRemove)
                    Remove(obj);*/
              
                #endregion
            }
            catch { /*Console.WriteLine(e.ToString()); Console.WriteLine("erorrr screnn timer");*/ }
        }

        public bool Add(Interfaces.IMapObject _object)
        {
            // lock (Objects)
            {
                if (!ContainsKey(_object.UID))
                {
                    if (ServerBase.Kernel.GetDistance(_object.X, _object.Y, Owner.Entity.X, Owner.Entity.Y) <= ServerBase.Constants.pScreenDistance)
                    {
                        lock (OnAdd)
                            OnAdd.Enqueue(_object);
                        //lock (Objects)
                        //    Objects.Add(_object);
                        return true;
                    }
                }
                if (Objects.Count >= 900)
                    Objects.Clear();
                return false;
            }
        }

        private bool ContainsKey(uint uid)
        {
            try
            {
                for (int c = 0; c < Objects.Count; c++)
                {
                    //For a multi threaded application, while we go through the collection
                    //the collection might change. We will make sure that we wont go off  
                    //the limits with a check.
                    if (c >= Objects.Count)
                        break;
                    if (Objects[c] != null)
                        if (Objects[c].UID == uid)
                            return true;
                }
            }
            catch
            {

            }
            return false;
        }

        private bool RemoveNull()
        {
            //  lock (Objects)
            {
                for (int c = 0; c < Objects.Count; c++)
                {
                    //For a multi threaded application, while we go through the collection
                    //the collection might change. We will make sure that we wont go off  
                    //the limits with a check.
                    if (c >= Objects.Count)
                        break;
                    if (Objects[c] == null)
                    {
                        lock (OnRemove)
                            OnRemove.Enqueue(Objects[c]);
                        // lock (Objects)
                        //     Objects.Remove(Objects[c]);
                        //As we remove, we will delete the c position, so we have to make sure
                        //that we check all items, so we have to remain at the same position after
                        //we remove something
                        c--;
                    }
                }
                return false;
            }
        }

        public bool Remove(Interfaces.IMapObject _object)
        {
            //lock (Objects)
            {
                if (_object.MapObjType == MapObjectType.Item)
                {
                    Network.GamePackets.FloorItem item = _object as Network.GamePackets.FloorItem;
                    item.Type = Network.GamePackets.FloorItem.Remove;
                    item.SendSpawn(Owner, false);
                    item.Type = Network.GamePackets.FloorItem.Drop;
                }
                else if (_object.MapObjType == MapObjectType.Player)
                {
                    Owner.Send(new Network.GamePackets.Data(true)
                    {
                        UID = _object.UID,
                        ID = Network.GamePackets.Data.RemoveEntity
                    });
                }
                lock (OnRemove)
                    OnRemove.Enqueue(_object);
                // lock (Objects)
                //     Objects.Remove(_object);
                return true;
            }
        }

        private bool TryGetValue(uint uid, out Interfaces.IMapObject obj)
        {
            obj = null;
            try
            {
                for (int c = 0; c < Objects.Count; c++)
                {
                    //For a multi threaded application, while we go through the collection
                    //the collection might change. We will make sure that we wont go off  
                    //the limits with a check.
                    if (c >= Objects.Count)
                        break;
                    if (Objects[c] != null)
                    {
                        if (Objects[c].UID == uid)
                        {
                            obj = Objects[c];
                            return true;
                        }
                    }
                }
            }
            catch
            {

            }
            return false;
        }

        public bool TryGetValue(uint uid, out Entity entity)
        {
            entity = null;
            Interfaces.IMapObject imo = null;
            TryGetValue(uid, out imo);
            if (imo == null)
                return false;

            if (imo.MapObjType == MapObjectType.Player || imo.MapObjType == MapObjectType.Monster)
                entity = imo as Game.Entity;
            if (entity == null)
                return false;
            return true;
        }
        public bool TryGetSob(uint uid, out Network.GamePackets.SobNpcSpawn sob)
        {
            sob = null;
            Interfaces.IMapObject imo = null;
            TryGetValue(uid, out imo);
            if (imo == null)
                return false;

            if (imo.MapObjType == MapObjectType.SobNpc)
                sob = imo as Network.GamePackets.SobNpcSpawn;

            if (sob == null)
                return false;
            return true;
        }
        public bool TryGetFloorItem(uint uid, out Network.GamePackets.FloorItem item)
        {
            item = null;

            Interfaces.IMapObject imo = null;
            TryGetValue(uid, out imo);
            if (imo == null)
                return false;

            if (imo.MapObjType == MapObjectType.Item)
                item = imo as Network.GamePackets.FloorItem;

            if (item == null)
                return false;
            return true;
        }
        public bool Contains(Interfaces.IMapObject _object)
        {
            return ContainsKey(_object.UID);
        }
        public bool Contains(uint uid)
        {
            return ContainsKey(uid);
        }

        public void CleanUp(Interfaces.IPacket spawnWith)
        {
            bool remove;
            try
            {
                IEnumerator<IMapObject> obj_ = Objects.GetEnumerator();
                int count = Objects.Count;
                obj_.MoveNext();
                for (byte x = 0; x < count; x++)
                {
                    if (x >= count) break;

                    IMapObject Base = obj_.Current;

                    if (Base == null)
                        continue;
                    remove = false;
                    if (Base.MapObjType == MapObjectType.Monster)
                    {
                        if ((Base as Entity).Dead)
                        {
                            if (Time32.Now > (Base as Entity).DeathStamp.AddSeconds(8))
                                remove = true;
                            else
                                remove = false;
                        }
                        if (ServerBase.Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, Base.X, Base.Y) > Constants.remScreenDistance)
                            remove = true;
                        if (remove)
                        {
                            if ((Base as Entity).MonsterInfo.InSight == Owner.Entity.UID)
                                (Base as Entity).MonsterInfo.InSight = 0;
                        }
                    }
                    else if (Base.MapObjType == MapObjectType.Player)
                    {
                        if (remove = (ServerBase.Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, Base.X, Base.Y) > Constants.pScreenDistance))
                        {
                            GameState pPlayer = Base.Owner as GameState;
                            pPlayer.Screen.Remove(Owner.Entity);
                        }
                    }
                    else if (Base.MapObjType == MapObjectType.Item)
                    {
                        remove = (Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, Base.X, Base.Y) >= 22);

                    }
                    else
                    {
                        remove = (Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, Base.X, Base.Y) > Constants.remScreenDistance);
                    }
                    if (Base.MapID != Owner.Map.ID)
                        remove = true;
                    if (remove)
                    {
                        Remove(Base);
                    }
                    obj_.MoveNext();
                }
            }
            catch (Exception e) { Program.SaveException(e); }
        }
        public void Clear()
        {
            Objects.Clear();
        }
        public void FullWipe()
        {
            bool remove;
            try
            {
                IEnumerator<IMapObject> obj_ = Objects.GetEnumerator();
                int count = Objects.Count;
                obj_.MoveNext();
                for (byte x = 0; x < count; x++)
                {
                    if (x >= count) break;
                    IMapObject Base = obj_.Current;

                    if (Base == null)
                        continue;
                    remove = true;
                    if (Base.MapObjType == MapObjectType.Monster)
                    {
                        remove = (Base as IBaseEntity).Dead ||
                            (ServerBase.Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, Base.X, Base.Y) >= Constants.pScreenDistance);
                    }
                    else if (Base.MapObjType == MapObjectType.Player)
                    {
                        GameState pPlayer = Base.Owner as GameState;
                        pPlayer.Screen.Remove(Owner.Entity);
                        remove = true;
                    }
                    else if (Base.MapObjType == MapObjectType.Item)
                    {
                        remove = (Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, Base.X, Base.Y) >= 22);

                    }
                    else
                    {
                        remove = (Kernel.GetDistance(Owner.Entity.X, Owner.Entity.Y, Base.X, Base.Y) >= Constants.pScreenDistance);
                    }
                    if (Base.MapID != Owner.Map.ID)
                        remove = true;
                    if (remove)
                    {
                        Network.GamePackets.Data data = new Network.GamePackets.Data(true);
                        data.UID = Owner.Entity.UID;
                        data.ID = Network.GamePackets.Data.RemoveEntity;

                        if (Base.MapObjType == Game.MapObjectType.Player)
                        {
                            GameState pPlayer = Base.Owner as GameState;
                            pPlayer.Send(data);
                        }
                    }
                    obj_.MoveNext();
                }
            }
            catch (Exception e) { Program.SaveException(e); }
            Clear();
        }


        public void SendScreenSpawn(Interfaces.IMapObject obj, bool self)
        {
            for (int c = 0; c < Objects.Count; c++)
            {
                //For a multi threaded application, while we go through the collection
                //the collection might change. We will make sure that we wont go off  
                //the limits with a check.
                if (c >= Objects.Count)
                    break;
                Interfaces.IMapObject _obj = Objects[c];
                if (_obj == null)
                    continue;
                if (_obj.UID != Owner.Entity.UID)
                {
                    if (_obj.MapObjType == Game.MapObjectType.Player)
                    {
                        GameState client = _obj.Owner as GameState;
                        obj.SendSpawn(client, false);
                    }
                }
            }

            if (self)
                obj.SendSpawn(Owner, false);
        }

        public void RemoveScreenSpawn(Interfaces.IMapObject obj, bool self)
        {
            for (int c = 0; c < Objects.Count; c++)
            {
                //For a multi threaded application, while we go through the collection
                //the collection might change. We will make sure that we wont go off  
                //the limits with a check.
                if (c >= Objects.Count)
                    break;
                Interfaces.IMapObject _obj = Objects[c];
                if (_obj == null)
                    continue;
                if (_obj.UID != Owner.Entity.UID)
                {
                    if (_obj.MapObjType == Game.MapObjectType.Player)
                    {
                        GameState client = _obj.Owner as GameState;
                        client.Screen.Remove(obj);
                    }
                }
            }

            if (self)
                Remove(obj);
        }

        public void SendScreen(byte[] buffer, bool self)
        {
            for (int c = 0; c < Objects.Count; c++)
            {
                //For a multi threaded application, while we go through the collection
                //the collection might change. We will make sure that we wont go off  
                //the limits with a check.
                if (c >= Objects.Count)
                    break;
                Interfaces.IMapObject _obj = Objects[c];
                if (_obj == null)
                    continue;
                if (_obj.UID != Owner.Entity.UID)
                {
                    if (_obj.MapObjType == Game.MapObjectType.Player)
                    {
                        GameState client = _obj.Owner as GameState;
                        if (Owner.WatchingGroup != null && client.WatchingGroup == null)
                            continue;
                        client.Send(buffer);
                    }
                }
            }

            if (self)
                Owner.Send(buffer);
        }

        public void SendScreen(Interfaces.IPacket buffer, bool self)
        {
            for (int c = 0; c < Objects.Count; c++)
            {
                //For a multi threaded application, while we go through the collection
                //the collection might change. We will make sure that we wont go off  
                //the limits with a check.
                if (c >= Objects.Count)
                    break;
                Interfaces.IMapObject _obj = Objects[c];
                if (_obj == null)
                    continue;
                if (_obj.UID != Owner.Entity.UID)
                {
                    if (_obj.MapObjType == Game.MapObjectType.Player)
                    {
                        GameState client = _obj.Owner as GameState;
                        if (Owner.WatchingGroup != null && client.WatchingGroup == null)
                            continue;
                        client.Send(buffer);
                    }
                }
            }

            if (self)
                Owner.Send(buffer);
        }

        public void Reload(Interfaces.IPacket spawnWith)
        {
            CleanUp(spawnWith);
            Map Map = Owner.Map;

            try
            {

                var varr = Kernel.GamePool.Values.GetEnumerator();
                varr.MoveNext();
                int COunt = Kernel.GamePool.Count;
                for (uint x = 0; x < COunt; x++)
                {
                    if (x >= COunt) break;

                    GameState pClient = (varr.Current as GameState);
                    if (pClient.Entity.UID != Owner.Entity.UID)
                    {
                        if (pClient.Map.ID == Map.ID)
                        {
                            short dist = Kernel.GetDistance(pClient.Entity.X, pClient.Entity.Y, Owner.Entity.X, Owner.Entity.Y);
                            if (dist <= Constants.pScreenDistance && !Contains(pClient.Entity))
                            {
                                if (pClient.Guild != null)
                                    pClient.Guild.SendName(Owner);
                                if (Owner.Guild != null)
                                    Owner.Guild.SendName(pClient);
                                if (pClient.Entity.InteractionInProgress && pClient.Entity.InteractionWith != Owner.Entity.UID && pClient.Entity.InteractionSet)
                                {
                                    if (pClient.Entity.Body == 1003 || pClient.Entity.Body == 1004)
                                    {
                                        if (pClient.Entity.InteractionX == pClient.Entity.X && pClient.Entity.Y == pClient.Entity.InteractionY)
                                        {
                                            Network.GamePackets.Attack atak = new PhoenixProject.Network.GamePackets.Attack(true);
                                            atak.Attacker = pClient.Entity.UID;
                                            atak.Attacked = pClient.Entity.InteractionWith;
                                            atak.X = pClient.Entity.X;
                                            atak.Y = pClient.Entity.Y;
                                            atak.AttackType = 49;
                                            atak.Damage = pClient.Entity.InteractionType;
                                            Owner.Send(atak);
                                        }
                                    }
                                    else
                                    {
                                        if (PhoenixProject.ServerBase.Kernel.GamePool.ContainsKey(pClient.Entity.InteractionWith))
                                        {
                                            Client.GameState Cs = PhoenixProject.ServerBase.Kernel.GamePool[pClient.Entity.InteractionWith] as Client.GameState;
                                            if (Cs.Entity.X == pClient.Entity.InteractionX && pClient.Entity.Y == pClient.Entity.InteractionY)
                                            {
                                                Network.GamePackets.Attack atak = new PhoenixProject.Network.GamePackets.Attack(true);
                                                atak.Attacker = pClient.Entity.UID;
                                                atak.Attacked = pClient.Entity.InteractionWith;
                                                atak.X = pClient.Entity.X;
                                                atak.Y = pClient.Entity.Y;
                                                atak.AttackType = 49;
                                                atak.Damage = pClient.Entity.InteractionType;
                                                Owner.Send(atak);
                                            }
                                        }
                                    }
                                }
                                if (pClient.Map.BaseID == 700)
                                {
                                    if (Owner.QualifierGroup != null)
                                    {
                                        if (pClient.QualifierGroup != null)
                                        {
                                            Owner.Entity.SendSpawn(pClient);
                                            pClient.Entity.SendSpawn(Owner);
                                            if (spawnWith != null)
                                                pClient.Send(spawnWith);
                                        }
                                        else
                                        {
                                            Owner.Entity.SendSpawn(pClient);
                                            Add(pClient.Entity);
                                            if (spawnWith != null)
                                                pClient.Send(spawnWith);
                                        }
                                    }
                                    else
                                    {
                                        if (pClient.QualifierGroup != null)
                                        {
                                            pClient.Entity.SendSpawn(Owner);
                                            pClient.Screen.Add(Owner.Entity);
                                            if (spawnWith != null)
                                                Owner.Send(spawnWith);
                                        }
                                        else
                                        {
                                            Owner.Entity.SendSpawn(pClient);
                                            pClient.Entity.SendSpawn(Owner);
                                            if (spawnWith != null)
                                                pClient.Send(spawnWith);
                                        }
                                    }
                                }
                                else
                                {
                                    Owner.Entity.SendSpawn(pClient);
                                    pClient.Entity.SendSpawn(Owner);
                                    if (spawnWith != null)
                                        pClient.Send(spawnWith);
                                }
                            }
                        }
                    }

                    varr.MoveNext();
                }
                if (Map != null)
                {
                    int X = Owner.Entity.X, Y = Owner.Entity.Y;
                    for (int extrax = -16; extrax < 16; extrax++)
                    {
                        for (int extray = -16; extray < 16; extray++)
                        {
                            var tile = Map.Floor.GetLocation(X + extrax, Y + extray);
                            if (tile != null)
                            {
                                if (tile.Item != null)
                                {
                                    short dist = ServerBase.Kernel.GetDistance(Owner.Entity.PX, Owner.Entity.PY, (ushort)(X + extrax), (ushort)(Y + extray));

                                    if (dist >= 16)
                                    {
                                        var item = tile.Item;
                                        if (Time32.Now > item.OnFloor.AddSeconds(ServerBase.Constants.FloorItemSeconds) || item.PickedUpAlready)
                                        {
                                            item.Type = Network.GamePackets.FloorItem.Remove;
                                            for (int c = 0; c < Owner.Screen.Objects.Count; c++)
                                            {
                                                //For a multi threaded application, while we go through the collection
                                                //the collection might change. We will make sure that we wont go off  
                                                //the limits with a check.
                                                if (c >= Owner.Screen.Objects.Count)
                                                    break;
                                                Interfaces.IMapObject _obj = Owner.Screen.Objects[c];
                                                if (_obj == null)
                                                    continue;
                                                if (_obj.MapObjType == MapObjectType.Player)
                                                {
                                                    (_obj as Entity).Owner.Send(item.ToArray());
                                                }
                                            }
                                            Map.Floor[item.X, item.Y, MapObjectType.Item, null] = true;
                                        }
                                        else
                                            item.SendSpawn(Owner, false);
                                    }
                                }
                                if (tile.Npc != null)
                                {
                                    short dist = ServerBase.Kernel.GetDistance(Owner.Entity.PX, Owner.Entity.PY, (ushort)(X + extrax), (ushort)(Y + extray));

                                    if (dist >= 16)
                                    {
                                        tile.Npc.SendSpawn(Owner);
                                    }
                                }
                            }
                        }
                    }
                    try
                    {
                        for (int x = 0; x < Map.Entities.Count; x++)
                        {
                            if (x >= Map.Entities.Count)
                                break;
                            if (Map.Entities[x] != null)
                            {
                                Game.Entity monster = Map.Entities[x];
                                if (Kernel.GetDistance(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y) <= Constants.nScreenDistance && !Contains(monster.UID))
                                {
                                    if (!monster.Dead)
                                    {
                                        monster.SendSpawn(Owner);
                                        if (monster.MaxHitpoints > 65535)
                                        {
                                            Update upd = new Update(true) { UID = monster.UID };
                                            upd.Append(Update.Hitpoints, monster.Hitpoints);
                                            Owner.Send(upd);
                                        }
                                    }
                                }
                            }
                        }
                       
                    }
                    catch { }
                    try
                    {
                        for (int x = 0; x < Map.Companions.Count; x++)
                        {
                            if (x >= Map.Companions.Count)
                                break;
                            if (Map.Companions[x] != null)
                            {
                                Game.Entity monster = Map.Companions[x];
                                if (monster == null) continue;
                                if (Kernel.GetDistance(monster.X, monster.Y, Owner.Entity.X, Owner.Entity.Y) <= Constants.nScreenDistance && !Contains(monster.UID))
                                {
                                    if (!monster.Dead)
                                    {
                                        monster.SendSpawn(Owner);
                                    }
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            catch (Exception e) { Program.SaveException(e); }
        }
    }
}
