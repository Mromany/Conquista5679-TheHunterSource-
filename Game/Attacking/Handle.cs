using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Game.ConquerStructures;
using NpcDialogs;
using PhoenixProject.ServerBase;
using PhoenixProject.Network;
using PhoenixProject.Interfaces;
using PhoenixProject.Network.GamePackets.Quest;

namespace PhoenixProject.Game.Attacking
{
    public class Handle//Coded By Kimo
    {
        private Attack attack;
        private Entity attacker, attacked;
        public Handle(Attack attack, Entity attacker, Entity attacked)
        {
            this.attack = attack;
            this.attacker = attacker;
            this.attacked = attacked;
            this.Execute();
        }
        #region Interations
        public class InteractionRequest
        {
            public InteractionRequest(Network.GamePackets.Attack attack, Game.Entity a_client)
            {
                Client.GameState client = a_client.Owner;

                client.Entity.InteractionInProgress = false;
                client.Entity.InteractionWith = attack.Attacked;
                client.Entity.InteractionType = 0;

                if (ServerBase.Kernel.GamePool.ContainsKey(attack.Attacked))
                {
                    Client.GameState clienttarget = ServerBase.Kernel.GamePool[attack.Attacked];
                    clienttarget.Entity.InteractionInProgress = false;
                    clienttarget.Entity.InteractionWith = client.Entity.UID;
                    clienttarget.Entity.InteractionType = 0;
                    attack.Attacker = client.Entity.UID;
                    attack.X = clienttarget.Entity.X;
                    attack.Y = clienttarget.Entity.Y;
                    attack.AttackType = 46;

                    clienttarget.Send(attack);
                }
            }
        }
        public class InteractionEffect
        {
            public InteractionEffect(Network.GamePackets.Attack attack, Game.Entity a_client)
            {
                Client.GameState client = a_client.Owner;

                if (ServerBase.Kernel.GamePool.ContainsKey(client.Entity.InteractionWith))
                {
                    Client.GameState clienttarget = ServerBase.Kernel.GamePool[client.Entity.InteractionWith];

                    if (clienttarget.Entity.X == client.Entity.X && clienttarget.Entity.Y == client.Entity.Y)
                    {
                        attack.Damage = client.Entity.InteractionType;
                        clienttarget.Entity.InteractionSet = true;
                        client.Entity.InteractionSet = true;
                        attack.Attacker = clienttarget.Entity.UID;
                        attack.Attacked = client.Entity.UID;
                        attack.AttackType = 47;
                        attack.X = clienttarget.Entity.X;
                        attack.Y = clienttarget.Entity.Y;

                        clienttarget.Send(attack);
                        attack.AttackType = 49;

                        attack.Attacker = client.Entity.UID;
                        attack.Attacked = clienttarget.Entity.UID;
                        client.SendScreen(attack, true);

                        attack.Attacker = clienttarget.Entity.UID;
                        attack.Attacked = client.Entity.UID;
                        client.SendScreen(attack, true);
                    }
                }
            }
        }
        public class InteractionAccept
        {
            public InteractionAccept(Network.GamePackets.Attack attack, Game.Entity a_client)
            {

                Client.GameState client = a_client.Owner;
                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Ride))
                    client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Ride);
                if (client.Entity.InteractionWith != attack.Attacked)
                    return;
                client.Entity.InteractionSet = false;
                if (ServerBase.Kernel.GamePool.ContainsKey(attack.Attacked))
                {
                    Client.GameState clienttarget = ServerBase.Kernel.GamePool[attack.Attacked];//PhoenixProject.ServerBase.Kernel.GamePool[attack.Attacked] as Client.GameState;
                    if (clienttarget.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Ride))
                        clienttarget.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Ride);
                    clienttarget.Entity.InteractionSet = false;
                    if (clienttarget.Entity.InteractionWith != client.Entity.UID)
                        return;
                    if (clienttarget.Entity.Body == 1003 || clienttarget.Entity.Body == 1004)
                    {
                        attack.Attacker = client.Entity.UID;
                        attack.X = client.Entity.X;
                        attack.Y = client.Entity.Y;
                        clienttarget.Send(attack);
                        clienttarget.Entity.InteractionInProgress = true;
                        client.Entity.InteractionInProgress = true;
                        clienttarget.Entity.InteractionType = attack.Damage;
                        clienttarget.Entity.InteractionX = client.Entity.X;
                        clienttarget.Entity.InteractionY = client.Entity.Y;
                        client.Entity.InteractionType = attack.Damage;
                        client.Entity.InteractionX = client.Entity.X;
                        client.Entity.InteractionY = client.Entity.Y;
                        if (clienttarget.Entity.X == client.Entity.X && clienttarget.Entity.Y == client.Entity.Y)
                        {
                            attack.Damage = client.Entity.InteractionType;
                            clienttarget.Entity.InteractionSet = true;
                            client.Entity.InteractionSet = true;
                            attack.Attacker = clienttarget.Entity.UID;
                            attack.Attacked = client.Entity.UID;
                            attack.AttackType = 47;
                            attack.X = clienttarget.Entity.X;
                            attack.Y = clienttarget.Entity.Y;
                            clienttarget.Send(attack);
                            attack.AttackType = 49;
                            attack.Attacker = client.Entity.UID;
                            attack.Attacked = clienttarget.Entity.UID;
                            client.SendScreen(attack, true);
                            attack.Attacker = clienttarget.Entity.UID;
                            attack.Attacked = client.Entity.UID;
                            client.SendScreen(attack, true);
                        }
                    }
                    else
                    {
                        attack.AttackType = 47;
                        attack.Attacker = client.Entity.UID;
                        attack.X = client.Entity.X;
                        attack.Y = client.Entity.Y;
                        clienttarget.Send(attack);
                        clienttarget.Entity.InteractionInProgress = true;
                        client.Entity.InteractionInProgress = true;
                        clienttarget.Entity.InteractionType = attack.Damage;
                        clienttarget.Entity.InteractionX = clienttarget.Entity.X;
                        clienttarget.Entity.InteractionY = clienttarget.Entity.Y;
                        client.Entity.InteractionType = attack.Damage;
                        client.Entity.InteractionX = clienttarget.Entity.X;
                        client.Entity.InteractionY = clienttarget.Entity.Y;
                        if (clienttarget.Entity.X == client.Entity.X && clienttarget.Entity.Y == client.Entity.Y)
                        {
                            clienttarget.Entity.InteractionSet = true;
                            client.Entity.InteractionSet = true;
                            attack.Attacker = clienttarget.Entity.UID;
                            attack.Attacked = client.Entity.UID;
                            attack.X = clienttarget.Entity.X;
                            attack.Y = clienttarget.Entity.Y;
                            clienttarget.Send(attack);
                            attack.AttackType = 49;
                            client.SendScreen(attack, true);
                            attack.Attacker = client.Entity.UID;
                            attack.Attacked = clienttarget.Entity.UID;
                            client.SendScreen(attack, true);
                        }
                    }
                }
            }
        }
        public class InteractionStopEffect
        {
            public InteractionStopEffect(Network.GamePackets.Attack attack, Game.Entity a_client)
            {
                Client.GameState client = a_client.Owner;

                if (ServerBase.Kernel.GamePool.ContainsKey(attack.Attacked))
                {
                    Client.GameState clienttarget = ServerBase.Kernel.GamePool[attack.Attacked];
                    attack.Attacker = client.Entity.UID;
                    attack.Attacked = clienttarget.Entity.UID;
                    attack.Damage = client.Entity.InteractionType;
                    attack.X = client.Entity.X;
                    attack.Y = client.Entity.Y;
                    attack.AttackType = 50;
                    client.SendScreen(attack, true);
                    attack.Attacker = clienttarget.Entity.UID; ;
                    attack.Attacked = client.Entity.UID;
                    clienttarget.SendScreen(attack, true);
                    client.Entity.Teleport(client.Entity.MapID, client.Entity.X, client.Entity.Y);
                    clienttarget.Entity.Teleport(clienttarget.Entity.MapID, clienttarget.Entity.X, clienttarget.Entity.Y);
                    client.Entity.InteractionType = 0;
                    client.Entity.InteractionWith = 0;
                    client.Entity.InteractionInProgress = false;
                    clienttarget.Entity.InteractionType = 0;
                    clienttarget.Entity.InteractionWith = 0;
                    clienttarget.Entity.InteractionInProgress = false;
                }
            }
        }
        public class InteractionRefuse
        {
            public InteractionRefuse(Network.GamePackets.Attack attack, Game.Entity a_client)
            {
                Client.GameState client = a_client.Owner;

                client.Entity.InteractionType = 0;
                client.Entity.InteractionWith = 0;
                client.Entity.InteractionInProgress = false;

                if (ServerBase.Kernel.GamePool.ContainsKey(attack.Attacked))
                {
                    Client.GameState clienttarget = ServerBase.Kernel.GamePool[attack.Attacked];
                    clienttarget.Entity.InteractionType = 0;
                    clienttarget.Entity.InteractionWith = 0;
                    clienttarget.Entity.InteractionInProgress = false;
                }
            }
        }
        #endregion
        private void Execute()
        {
            #region interactions
            if (attack != null)
            {
                switch (attack.AttackType)
                {
                    case (uint)Network.GamePackets.Attack.InteractionRequest:
                        new InteractionRequest(attack, attacker);
                        return;
                    case (uint)Network.GamePackets.Attack.InteractionEffect:
                        new InteractionEffect(attack, attacker);
                        return;

                    case (uint)Network.GamePackets.Attack.InteractionAccept:
                        new InteractionAccept(attack, attacker);
                        return;
                    case (uint)Network.GamePackets.Attack.InteractionRefuse:
                        new InteractionRefuse(attack, attacker);
                        return;
                    case (uint)Network.GamePackets.Attack.InteractionStopEffect:
                        new InteractionStopEffect(attack, attacker);
                        return;
                }
            }
            #endregion
            #region Monster -> Player \ Monster
            if (attack == null)
            {
                if (attacker.EntityFlag != EntityFlag.Monster)
                    return;
                if (attacker.Companion)
                {
                    if (ServerBase.Constants.PKForbiddenMaps.Contains(attacker.MapID))
                        return;
                }
                if (attacked.EntityFlag == EntityFlag.Player)
                {
                    if (!attacked.Owner.Attackable)
                        return;
                    if (attacked.Dead && attacker.MonsterInfo.SpellID != 1050)
                        return;

                    if (attacker.MonsterInfo.SpellID == 0)// from this bracket to the next die delay replce this with urs
                    {
                        attack = new Attack(true);
                        attack.Effect1 = Attack.AttackEffects1.None;
                        uint damage = Calculate.Melee(attacker, attacked, ref attack);
                        attack.Attacker = attacker.UID;
                        attack.Attacked = attacked.UID;
                        attack.AttackType = Attack.Melee;
                        attack.Damage = damage;
                        attack.X = attacked.X;
                        attack.Y = attacked.Y;
                        BlessEffect.Effect(attacked);
                        if (attacked.EntityFlag == EntityFlag.Player)
                        {
                            if (attacked.Action == Enums.ConquerAction.Sit)
                                if (attacked.Stamina > 20)
                                    attacked.Stamina -= 20;
                                else
                                    attacked.Stamina = 0;
                            attacked.Action = Enums.ConquerAction.None;
                            //Console.WriteLine("ssS");
                        }
                        if (attacked.Hitpoints <= damage)
                        {
                            attacked.Owner.SendScreen(attack, true);
                            attacked.Die(attacker);
                        }
                        else
                        {
                            attacked.Hitpoints -= damage;
                            attacked.Owner.SendScreen(attack, true);
                        }
                    }
                    else
                    {
                        SpellUse suse = new SpellUse(true);
                        attack = new Attack(true);
                        attack.Effect1 = Attack.AttackEffects1.None;
                        uint damage = Calculate.Magic(attacker, attacked, attacker.MonsterInfo.SpellID, 0, ref attack);
                        suse.Effect1 = attack.Effect1;
                        if (attacked.EntityFlag == EntityFlag.Player)
                        {
                            if (attacked.Action == Enums.ConquerAction.Sit)
                                if (attacked.Stamina > 20)
                                    attacked.Stamina -= 20;
                                else
                                    attacked.Stamina = 0;
                            attacked.Action = Enums.ConquerAction.None;
                            //Console.WriteLine("ssS");
                        }
                        if (attacked.Hitpoints <= damage)
                        {
                            attacked.Die(attacker);
                        }
                        else
                        {
                            attacked.Hitpoints -= damage;
                        }
                        if (attacker.Companion)
                            attacker.Owner.IncreaseExperience(Math.Min(damage, attacked.Hitpoints), true);

                        suse.Attacker = attacker.UID;
                        suse.SpellID = attacker.MonsterInfo.SpellID;
                        suse.X = attacked.X;
                        suse.Y = attacked.Y;
                        suse.Targets.Add(attacked.UID, damage);
                        attacked.Owner.SendScreen(suse, true);
                    }
                }
                else
                {
                    if (attacker.MonsterInfo.SpellID == 0)
                    {
                        attack = new Attack(true);
                        attack.Effect1 = Attack.AttackEffects1.None;
                        uint damage = Calculate.Melee(attacker, attacked, ref attack);
                        attack.Attacker = attacker.UID;
                        attack.Attacked = attacked.UID;
                        attack.AttackType = Attack.Melee;
                        attack.Damage = damage;
                        attack.X = attacked.X;
                        attack.Y = attacked.Y;
                        attacked.MonsterInfo.SendScreen(attack);
                        if (attacker.Companion)
                            if (damage > attacked.Hitpoints)
                                attacker.Owner.IncreaseExperience(Math.Min(damage, attacked.Hitpoints), true);
                            else
                                attacker.Owner.IncreaseExperience(damage, true);
                        if (attacked.Hitpoints <= damage)
                        {
                            attacked.Die(attacker);
                            attack = new Attack(true);
                            attack.Attacker = attacker.UID;
                            attack.Attacked = attacked.UID;
                            attack.AttackType = Network.GamePackets.Attack.Kill;
                            attack.X = attacked.X;
                            attack.Y = attacked.Y;
                            attacked.MonsterInfo.SendScreen(attack);
                        }
                        else
                        {
                            attacked.Hitpoints -= damage;
                        }
                    }
                    else
                    {
                        SpellUse suse = new SpellUse(true);
                        if (attack != null) attack.Effect1 = Attack.AttackEffects1.None;
                        uint damage = Calculate.Magic(attacker, attacked, attacker.MonsterInfo.SpellID, 0, ref attack);
                        if (attack != null) suse.Effect1 = attack.Effect1;

                        suse.Attacker = attacker.UID;
                        suse.SpellID = attacker.MonsterInfo.SpellID;
                        suse.X = attacked.X;
                        suse.Y = attacked.Y;
                        suse.Targets.Add(attacked.UID, damage);
                        attacked.MonsterInfo.SendScreen(suse);
                        if (attacker.Companion)
                            if (damage > attacked.Hitpoints)
                                attacker.Owner.IncreaseExperience(Math.Min(damage, attacked.Hitpoints), true);
                            else
                                attacker.Owner.IncreaseExperience(damage, true);
                        if (attacked.Hitpoints <= damage)
                        {
                            attacked.Die(attacker);
                            attack = new Attack(true);
                            attack.Attacker = attacker.UID;
                            attack.Attacked = attacked.UID;
                            attack.AttackType = Network.GamePackets.Attack.Kill;
                            attack.X = attacked.X;
                            attack.Y = attacked.Y;
                            attacked.MonsterInfo.SendScreen(attack);
                        }
                        else
                        {
                            attacked.Hitpoints -= damage;
                        }
                    }
                }
            }
            #endregion
            #region Player -> Player \ Monster \ Sob Npc
            else
            {
                #region Merchant
                if (attack.AttackType == Attack.MerchantAccept || attack.AttackType == Attack.MerchantRefuse)
                {

                    attacker.AttackPacket = null;
                    return;
                }
                #endregion
                #region Marriage
                if (attack.AttackType == Attack.MarriageAccept || attack.AttackType == Attack.MarriageRequest)
                {
                    if (attack.AttackType == Attack.MarriageRequest)
                    {
                        Client.GameState Spouse = null;
                        uint takeout = attack.Attacked;
                        if (takeout == attacker.UID)
                            takeout = attack.Attacker;
                        if (ServerBase.Kernel.GamePool.TryGetValue(takeout, out Spouse))
                        {
                            if (attacker.Spouse != "None" || Spouse.Entity.Spouse != "None")
                            {
                                attacker.Owner.Send(new Message("You cannot marry someone that is already married with someone else!", System.Drawing.Color.Black, Message.TopLeft));
                            }
                            else
                            {
                                uint id1 = attacker.Mesh % 10, id2 = Spouse.Entity.Mesh % 10;

                                if (id1 <= 2 && id2 >= 3 || id1 >= 2 && id2 <= 3)
                                {

                                    attack.X = Spouse.Entity.X;
                                    attack.Y = Spouse.Entity.Y;

                                    Spouse.Send(attack);
                                }
                                else
                                {
                                    attacker.Owner.Send(new Message("You cannot marry someone of your gender!", System.Drawing.Color.Black, Message.TopLeft));
                                }
                            }
                        }
                    }
                    else
                    {
                        Client.GameState Spouse = null;
                        if (ServerBase.Kernel.GamePool.TryGetValue(attack.Attacked, out Spouse))
                        {
                            if (attacker.Spouse != "None" || Spouse.Entity.Spouse != "None")
                            {
                                attacker.Owner.Send(new Message("You cannot marry someone that is already married with someone else!", System.Drawing.Color.Black, Message.TopLeft));
                            }
                            else
                            {
                                if (attacker.Mesh % 10 <= 2 && Spouse.Entity.Mesh % 10 >= 3 || attacker.Mesh % 10 >= 3 && Spouse.Entity.Mesh % 10 <= 2)
                                {
                                    Spouse.Entity.Spouse = attacker.Name;
                                    attacker.Spouse = Spouse.Entity.Name;
                                    Message message = null;
                                    if (Spouse.Entity.Mesh % 10 >= 3)
                                        message = new Message("Joy and happiness! " + Spouse.Entity.Name + " and " + attacker.Name + " have joined together in the holy marriage. We wish them a stone house.", System.Drawing.Color.BurlyWood, Message.Center);
                                    else
                                        message = new Message("Joy and happiness! " + attacker.Name + " and " + attacker.Spouse + " have joined together in the holy marriage. We wish them a stone house.", System.Drawing.Color.BurlyWood, Message.Center);

                                    foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
                                    {
                                        client.Send(message);
                                    }

                                    Spouse.Entity.Update(_String.Effect, "firework-2love", true);
                                    attacker.Update(_String.Effect, "firework-2love", true);
                                }
                                else
                                {
                                    attacker.Owner.Send(new Message("You cannot marry someone of your gender!", System.Drawing.Color.Black, Message.TopLeft));
                                }
                            }
                        }
                    }
                }
                #endregion
                #region Attacking
                else
                {

                    attacker.Owner.Attackable = true;
                    Entity attacked = null;

                    SobNpcSpawn attackedsob = null;

                    #region Checks
                    if (attack.Attacker != attacker.UID)
                        return;
                    if (attacker.EntityFlag != EntityFlag.Player)
                        return;
                    attacker.RemoveFlag(Update.Flags.Invisibility);

                    bool pass = false;
                    if (attack.AttackType == Attack.Melee)
                    {
                        if (attacker.OnFatalStrike())
                        {
                            if (attack.Attacked < 600000)
                            {
                                pass = true;
                            }
                        }
                    }
                    ushort decrease = 0;
                    if (attacker.OnCyclone())
                        decrease = 1;

                    if (attacker.OnSuperman())
                        decrease = 300;
                    if (!pass)
                    {
                        int milliSeconds = 1000 - attacker.Agility - decrease;
                        if (milliSeconds < 0 || milliSeconds > 5000)
                            milliSeconds = 0;
                        if (Time32.Now < attacker.AttackStamp.AddMilliseconds(milliSeconds))
                            return;
                    }
                    if (attacker.Dead)
                    {
                        if (attacker.AttackPacket != null)
                            attacker.AttackPacket = null;
                        return;
                    }
                    attacker.AttackStamp = Time32.Now;
                    if (attacker.Owner.QualifierGroup != null)
                    {
                        if (Time32.Now < attacker.Owner.QualifierGroup.CreateTime.AddSeconds(12))
                        {
                            return;
                        }
                    }

                restart:

                    #region Extract attack information
                    ushort SpellID = 0, X = 0, Y = 0;
                    uint Target = 0;
                    if (attack.AttackType == Attack.Magic)
                    {
                        if (!attack.Decoded)
                        {

                            #region GetSkillID
                            SpellID = Convert.ToUInt16(((long)attack.ToArray()[24] & 0xFF) | (((long)attack.ToArray()[25] & 0xFF) << 8));
                            SpellID ^= (ushort)0x915d;
                            SpellID ^= (ushort)attacker.UID;
                            SpellID = (ushort)(SpellID << 0x3 | SpellID >> 0xd);
                            SpellID -= 0xeb42;
                            #endregion
                            #region GetCoords
                            X = (ushort)((attack.ToArray()[16] & 0xFF) | ((attack.ToArray()[17] & 0xFF) << 8));
                            X = (ushort)(X ^ (uint)(attacker.UID & 0xffff) ^ 0x2ed6);
                            X = (ushort)(((X << 1) | ((X & 0x8000) >> 15)) & 0xffff);
                            X = (ushort)((X | 0xffff0000) - 0xffff22ee);

                            Y = (ushort)((attack.ToArray()[18] & 0xFF) | ((attack.ToArray()[19] & 0xFF) << 8));
                            Y = (ushort)(Y ^ (uint)(attacker.UID & 0xffff) ^ 0xb99b);
                            Y = (ushort)(((Y << 5) | ((Y & 0xF800) >> 11)) & 0xffff);
                            Y = (ushort)((Y | 0xffff0000) - 0xffff8922);
                            #endregion
                            #region GetTarget
                            Target = ((uint)attack.ToArray()[12] & 0xFF) | (((uint)attack.ToArray()[13] & 0xFF) << 8) | (((uint)attack.ToArray()[14] & 0xFF) << 16) | (((uint)attack.ToArray()[15] & 0xFF) << 24);
                            Target = ((((Target & 0xffffe000) >> 13) | ((Target & 0x1fff) << 19)) ^ 0x5F2D2463 ^ attacker.UID) - 0x746F4AE6;
                            #endregion

                            attack.X = X;
                            attack.Y = Y;
                            attack.Damage = SpellID;
                            attack.Attacked = Target;
                            attack.Decoded = true;

                        }
                        else
                        {
                            X = attack.X;
                            Y = attack.Y;
                            SpellID = (ushort)attack.Damage;
                            Target = attack.Attacked;
                        }
                    }
                    #endregion
                    #endregion

                    if (attacker.ContainsFlag(Update.Flags.Ride) && attacker.Owner.Equipment.TryGetItem(18) == null)
                    {
                        if (attack.AttackType != Attack.Magic)
                            attacker.RemoveFlag(Update.Flags.Ride);
                        else
                            if (!(SpellID == 7003 || SpellID == 7002))
                                attacker.RemoveFlag(Update.Flags.Ride);
                    }
                    if (attacker.ContainsFlag(Update.Flags.CastPray))
                        attacker.RemoveFlag(Update.Flags.CastPray);
                    if (attacker.ContainsFlag(Update.Flags.Praying))
                        attacker.RemoveFlag(Update.Flags.Praying);


                    #region Dash
                    if (SpellID == 1051)
                    {
                        if (ServerBase.Kernel.GetDistance(attack.X, attack.Y, attacker.X, attacker.Y) > 4)
                        {
                            attacker.Owner.Disconnect();
                            return;
                        }
                        attacker.X = attack.X; attacker.Y = attack.Y;
                        ushort x = attacker.X, y = attacker.Y;
                        Game.Map.UpdateCoordonatesForAngle(ref x, ref y, (Enums.ConquerAngle)Target);
                        for (int c = 0; c < attacker.Owner.Screen.Objects.Count; c++)
                        {
                            if (c >= attacker.Owner.Screen.Objects.Count)
                                break;
                            //List<IMapObject> list = new List<IMapObject>();
                            IMapObject obj = attacker.Owner.Screen.Objects[c];
                            if (obj == null)
                                continue;
                            if (obj.X == x && obj.Y == y && (obj.MapObjType == MapObjectType.Monster || obj.MapObjType == MapObjectType.Player))
                            {
                                Entity entity = obj as Entity;
                                if (!entity.Dead)
                                {
                                    Target = obj.UID;
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                    #region CounterKill
                    if (attack.AttackType == Attack.CounterKillSwitch)
                    {
                        if (attacked != null)
                            if (attacked.ContainsFlag(Update.Flags.Fly))
                            { attacker.AttackPacket = null; return; }
                        if (attacker != null)
                            if (attacker.ContainsFlag(Update.Flags.Fly))
                            { attacker.AttackPacket = null; return; }
                        if (attacker.Owner.Spells.ContainsKey(6003))
                        {
                            if (!attacker.CounterKillSwitch)
                            {
                                if (Time32.Now >= attacker.CounterKillStamp.AddSeconds(30))
                                {
                                    attacker.CounterKillStamp = Time32.Now;
                                    attacker.CounterKillSwitch = true;
                                    Attack m_attack = new Attack(true);
                                    m_attack.Attacked = attacker.UID;
                                    m_attack.Attacker = attacker.UID;
                                    m_attack.AttackType = Attack.CounterKillSwitch;
                                    m_attack.Damage = 1;
                                    m_attack.X = attacker.X;
                                    m_attack.Y = attacker.Y;
                                    m_attack.Send(attacker.Owner);
                                }
                            }
                            else
                            {
                                attacker.CounterKillSwitch = false;
                                Attack m_attack = new Attack(true);
                                m_attack.Attacked = attacker.UID;
                                m_attack.Attacker = attacker.UID;
                                m_attack.AttackType = Attack.CounterKillSwitch;
                                m_attack.Damage = 0;
                                m_attack.X = attacker.X;
                                m_attack.Y = attacker.Y;
                                m_attack.Send(attacker.Owner);
                            }

                            attacker.Owner.IncreaseSpellExperience(100, 6003);
                            attacker.AttackPacket = null;
                        }
                    }
                    #endregion
                    #region Melee
                    else if (attack.AttackType == Attack.Melee)
                    {
                        if (attacker.Owner.Screen.TryGetValue(attack.Attacked, out attacked))
                        {
                            if (!attacker.Owner.AlternateEquipment)
                            {
                                CheckForExtraWeaponPowers(attacker.Owner, attacked);
                            }
                            else
                            {
                                CheckForExtraWeaponPowers2(attacker.Owner, attacked);
                            }
                            if (!CanAttack(attacker, attacked, null, attack.AttackType == Attack.Melee))
                                return;
                            pass = false;
                            if (attacker.OnFatalStrike())
                            {
                                if (attacked.EntityFlag == EntityFlag.Monster)
                                {
                                    pass = true;
                                }
                            }
                            ushort range = attacker.AttackRange;
                            if (attacker.Transformed)
                                range = (ushort)attacker.TransformationAttackRange;
                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= range || pass)
                            {
                                attack.Effect1 = Attack.AttackEffects1.None;
                                uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, ref attack);
                                attack.Damage = damage;
                                if (attacker.OnFatalStrike())
                                {
                                    if (attacked.EntityFlag == EntityFlag.Monster)
                                    {
                                        if (!attacker.Owner.AlternateEquipment)
                                        {
                                            bool can = false;
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.RightWeapon))
                                                if (attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.RightWeapon).ID / 1000 == 601)
                                                    can = true;
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.LeftWeapon))
                                                if (attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.LeftWeapon).ID / 1000 == 601)
                                                    can = true;
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.RightWeapon))
                                                if (attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.RightWeapon).ID / 1000 == 511)
                                                    can = true;
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.LeftWeapon))
                                                if (attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.LeftWeapon).ID / 1000 == 511)
                                                    can = true;
                                            if (!can)
                                                return;
                                            ushort x = attacked.X;
                                            ushort y = attacked.Y;
                                            Map.UpdateCoordonatesForAngle(ref x, ref y, ServerBase.Kernel.GetAngle(attacked.X, attacked.Y, attacker.X, attacker.Y));
                                            attacker.Shift(x, y);
                                            attack.X = x;
                                            attack.Y = y;
                                            attack.AttackType = Attack.FatalStrike;
                                        }
                                        else
                                        {
                                            bool can = false;
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltRightHand))
                                                if (attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.AltRightHand).ID / 1000 == 601)
                                                    can = true;
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltLeftHand))
                                                if (attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.AltLeftHand).ID / 1000 == 601)
                                                    can = true;
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltRightHand))
                                                if (attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.AltRightHand).ID / 1000 == 511)
                                                    can = true;
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltLeftHand))
                                                if (attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.AltLeftHand).ID / 1000 == 511)
                                                    can = true;
                                            if (!can)
                                                return;
                                            ushort x = attacked.X;
                                            ushort y = attacked.Y;
                                            Map.UpdateCoordonatesForAngle(ref x, ref y, ServerBase.Kernel.GetAngle(attacked.X, attacked.Y, attacker.X, attacker.Y));
                                            attacker.Shift(x, y);
                                            attack.X = x;
                                            attack.Y = y;
                                            attack.AttackType = Attack.FatalStrike;
                                        }
                                    }
                                }
                                //over:
                                if (!attacker.Owner.AlternateEquipment)
                                {
                                    if (!attacker.Owner.Equipment.Free((byte)ConquerItem.RightWeapon))
                                    {
                                        Interfaces.IConquerItem rightweapon = attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.RightWeapon);
                                        ushort wep1subyte = (ushort)(rightweapon.ID / 1000), wep2subyte = 0;
                                        bool wep1bs = false, wep2bs = false;
                                        /*if (wep1subyte == 421)
                                        {
                                            wep1bs = true;
                                            wep1subyte = 420;
                                        }
                                        if (wep1subyte == 511)
                                        {
                                            wep1bs = true;
                                            wep1subyte = 601;
                                        }*/
                                        ushort wep1spellid = 0, wep2spellid = 0;
                                        if (Database.SpellTable.WeaponSpells.ContainsKey(wep1subyte))
                                        {
                                            wep1spellid = Database.SpellTable.WeaponSpells[wep1subyte];
                                        }
                                        /*if (wep1subyte == 601 || wep1subyte == 511)
                                        {
                                            if (Database.SpellTable.WeaponSpells.ContainsKey(11230))
                                            {
                                                wep1spellid = 11230;
                                            }
                                        }*/
                                        Database.SpellInformation wep1spell = null, wep2spell = null;
                                        bool doWep1Spell = false, doWep2Spell = false;
                                        if (attacker.Owner.Spells.ContainsKey(wep1spellid) && Database.SpellTable.SpellInformations.ContainsKey(wep1spellid))
                                        {
                                            wep1spell = Database.SpellTable.SpellInformations[wep1spellid][attacker.Owner.Spells[wep1spellid].Level];
                                            if (wep1spell != null)
                                            {
                                                doWep1Spell = ServerBase.Kernel.Rate(wep1spell.Percent);
                                            }

                                        }

                                        if (!doWep1Spell)
                                        {
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.LeftWeapon))
                                            {
                                                Interfaces.IConquerItem leftweapon = attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.LeftWeapon);
                                                wep2subyte = (ushort)(leftweapon.ID / 1000);
                                                /* if (wep2subyte == 421)
                                                 {
                                                     wep2bs = true;
                                                     wep2subyte = 420;
                                                 }
                                                 if (wep2subyte == 511)
                                                 {
                                                     wep2bs = true;
                                                     wep2subyte=601;
                                                 }*/
                                                if (wep2subyte == 900 || leftweapon.ID == 1050002)
                                                {
                                                    return;
                                                }

                                                if (Database.SpellTable.WeaponSpells.ContainsKey(wep2subyte))
                                                {
                                                    wep2spellid = Database.SpellTable.WeaponSpells[wep2subyte];
                                                }

                                                if (attacker.Owner.Spells.ContainsKey(wep2spellid) && Database.SpellTable.SpellInformations.ContainsKey(wep2spellid))
                                                {
                                                    wep2spell = Database.SpellTable.SpellInformations[wep2spellid][attacker.Owner.Spells[wep2spellid].Level];
                                                    if (wep2spell != null)
                                                    {
                                                        doWep2Spell = ServerBase.Kernel.Rate(wep2spell.Percent);
                                                    }

                                                }

                                            }
                                        }

                                        if (!attacker.Transformed)
                                        {
                                            if (doWep1Spell)
                                            {
                                                attack.AttackType = Attack.Magic;
                                                attack.Decoded = true;
                                                attack.X = attacked.X;
                                                attack.Y = attacked.Y;
                                                attack.Attacked = attacked.UID;
                                                attack.Damage = wep1spell.ID;
                                                goto restart;
                                            }
                                            if (doWep2Spell)
                                            {
                                                attack.AttackType = Attack.Magic;
                                                attack.Decoded = true;
                                                attack.X = attacked.X;
                                                attack.Y = attacked.Y;
                                                attack.Attacked = attacked.UID;
                                                attack.Damage = wep2spell.ID;
                                                goto restart;
                                            }
                                            if (wep1bs)
                                                wep1subyte++;
                                            if (attacker.EntityFlag == EntityFlag.Player && attacked.EntityFlag != EntityFlag.Player)
                                                if (damage > attacked.Hitpoints)
                                                {
                                                    attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attacked.Hitpoints), wep1subyte);
                                                    if (wep2subyte != 0)
                                                    {
                                                        if (wep2bs)
                                                            wep2subyte++;
                                                        attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attacked.Hitpoints), wep2subyte);
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.Owner.IncreaseProficiencyExperience(damage, wep1subyte);
                                                    if (wep2subyte != 0)
                                                    {
                                                        if (wep2bs)
                                                            wep2subyte++;
                                                        attacker.Owner.IncreaseProficiencyExperience(damage, wep2subyte);
                                                    }
                                                }
                                        }
                                    }
                                    else
                                    {
                                        if (!attacker.Transformed)
                                        {
                                            if (attacker.EntityFlag == EntityFlag.Player && attacked.EntityFlag != EntityFlag.Player)
                                                if (damage > attacked.Hitpoints)
                                                {
                                                    attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attacked.Hitpoints), 0);
                                                }
                                                else
                                                {
                                                    attacker.Owner.IncreaseProficiencyExperience(damage, 0);
                                                }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltRightHand))
                                    {
                                        Interfaces.IConquerItem rightweapon = attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.AltRightHand);
                                        ushort wep1subyte = (ushort)(rightweapon.ID / 1000), wep2subyte = 0;
                                        bool wep1bs = false, wep2bs = false;
                                        /* if (wep1subyte == 421)
                                         {
                                             wep1bs = true;
                                             wep1subyte = 420;
                                         }
                                         if (wep1subyte == 511)
                                         {
                                             wep1bs = true;
                                             wep1subyte = 601;
                                         }*/

                                        ushort wep1spellid = 0, wep2spellid = 0;
                                        if (Database.SpellTable.WeaponSpells.ContainsKey(wep1subyte))
                                        {
                                            wep1spellid = Database.SpellTable.WeaponSpells[wep1subyte];
                                        }

                                        Database.SpellInformation wep1spell = null, wep2spell = null;
                                        bool doWep1Spell = false, doWep2Spell = false;
                                        if (attacker.Owner.Spells.ContainsKey(wep1spellid) && Database.SpellTable.SpellInformations.ContainsKey(wep1spellid))
                                        {
                                            wep1spell = Database.SpellTable.SpellInformations[wep1spellid][attacker.Owner.Spells[wep1spellid].Level];
                                            doWep1Spell = ServerBase.Kernel.Rate(wep1spell.Percent);

                                        }

                                        if (!doWep1Spell)
                                        {
                                            if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltLeftHand))
                                            {
                                                Interfaces.IConquerItem leftweapon = attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.AltLeftHand);
                                                wep2subyte = (ushort)(leftweapon.ID / 1000);
                                                /* if (wep2subyte == 421)
                                                 {
                                                     wep2bs = true;
                                                     wep2subyte = 420;
                                                 }
                                                 if (wep2subyte == 511)
                                                 {
                                                     wep2bs = true;
                                                     wep2subyte = 601;
                                                 }*/
                                                if (wep2subyte == 900 || leftweapon.ID == 1050002)
                                                {
                                                    return;
                                                }
                                                if (Database.SpellTable.WeaponSpells.ContainsKey(wep2subyte))
                                                {
                                                    wep2spellid = Database.SpellTable.WeaponSpells[wep2subyte];
                                                }

                                                if (attacker.Owner.Spells.ContainsKey(wep2spellid) && Database.SpellTable.SpellInformations.ContainsKey(wep2spellid))
                                                {
                                                    wep2spell = Database.SpellTable.SpellInformations[wep2spellid][attacker.Owner.Spells[wep2spellid].Level];
                                                    doWep2Spell = ServerBase.Kernel.Rate(wep2spell.Percent);

                                                }

                                            }
                                        }

                                        if (!attacker.Transformed)
                                        {
                                            if (doWep1Spell)
                                            {
                                                attack.AttackType = Attack.Magic;
                                                attack.Decoded = true;
                                                attack.X = attacked.X;
                                                attack.Y = attacked.Y;
                                                attack.Attacked = attacked.UID;
                                                attack.Damage = wep1spell.ID;
                                                goto restart;
                                            }
                                            if (doWep2Spell)
                                            {
                                                attack.AttackType = Attack.Magic;
                                                attack.Decoded = true;
                                                attack.X = attacked.X;
                                                attack.Y = attacked.Y;
                                                attack.Attacked = attacked.UID;
                                                attack.Damage = wep2spell.ID;
                                                goto restart;
                                            }
                                            if (wep1bs)
                                                wep1subyte++;
                                            if (attacker.EntityFlag == EntityFlag.Player && attacked.EntityFlag != EntityFlag.Player)
                                                if (damage > attacked.Hitpoints)
                                                {
                                                    attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attacked.Hitpoints), wep1subyte);
                                                    if (wep2subyte != 0)
                                                    {
                                                        if (wep2bs)
                                                            wep2subyte++;
                                                        attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attacked.Hitpoints), wep2subyte);
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.Owner.IncreaseProficiencyExperience(damage, wep1subyte);
                                                    if (wep2subyte != 0)
                                                    {
                                                        if (wep2bs)
                                                            wep2subyte++;
                                                        attacker.Owner.IncreaseProficiencyExperience(damage, wep2subyte);
                                                    }
                                                }
                                        }
                                    }
                                    else
                                    {
                                        if (!attacker.Transformed)
                                        {
                                            if (attacker.EntityFlag == EntityFlag.Player && attacked.EntityFlag != EntityFlag.Player)
                                                if (damage > attacked.Hitpoints)
                                                {
                                                    attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attacked.Hitpoints), 0);
                                                }
                                                else
                                                {
                                                    attacker.Owner.IncreaseProficiencyExperience(damage, 0);
                                                }
                                        }
                                    }
                                }
                                ReceiveAttack(attacker, attacked, attack, damage, null);
                                attack.AttackType = Attack.Melee;
                            }
                            else
                            {
                                attacker.AttackPacket = null;
                            }
                        }
                        else if (attacker.Owner.Screen.TryGetSob(attack.Attacked, out attackedsob))
                        {
                            // Console.WriteLine("Ss");
                            if (CanAttack(attacker, attackedsob, null))
                            {
                                ushort range = attacker.AttackRange;
                                if (attacker.Transformed)
                                    range = (ushort)attacker.TransformationAttackRange;
                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= range)
                                {
                                    attack.Effect1 = Attack.AttackEffects1.None;
                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);

                                    if (!attacker.Owner.AlternateEquipment)
                                    {
                                        if (!attacker.Owner.Equipment.Free((byte)ConquerItem.RightWeapon))
                                        {
                                            Interfaces.IConquerItem rightweapon = attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.RightWeapon);
                                            ushort wep1subyte = (ushort)(rightweapon.ID / 1000), wep2subyte = 0;
                                            bool wep1bs = false, wep2bs = false;
                                            /* if (wep1subyte == 421)
                                             {
                                                 wep1bs = true;
                                                 wep1subyte=420;
                                             }
                                             if (wep1subyte == 511)
                                             {
                                                 wep1bs = true;
                                                 wep1subyte=601;
                                             }*/
                                            ushort wep1spellid = 0, wep2spellid = 0;
                                            if (Database.SpellTable.WeaponSpells.ContainsKey(wep1subyte))
                                                wep1spellid = Database.SpellTable.WeaponSpells[wep1subyte];
                                            Database.SpellInformation wep1spell = null, wep2spell = null;
                                            bool doWep1Spell = false, doWep2Spell = false;
                                            if (attacker.Owner.Spells.ContainsKey(wep1spellid) && Database.SpellTable.SpellInformations.ContainsKey(wep1spellid))
                                            {
                                                wep1spell = Database.SpellTable.SpellInformations[wep1spellid][attacker.Owner.Spells[wep1spellid].Level];
                                                doWep1Spell = ServerBase.Kernel.Rate(wep1spell.Percent);
                                            }
                                            if (!doWep1Spell)
                                            {
                                                if (!attacker.Owner.Equipment.Free((byte)ConquerItem.LeftWeapon))
                                                {
                                                    Interfaces.IConquerItem leftweapon = attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.LeftWeapon);
                                                    wep2subyte = (ushort)(leftweapon.ID / 1000);
                                                    /* if (wep2subyte == 421)
                                                     {
                                                         wep2bs = true;
                                                         wep2subyte = 420;
                                                     }
                                                     if (wep2subyte == 511)
                                                     {
                                                         wep2bs = true;
                                                         wep2subyte = 601;
                                                     }*/
                                                    if (Database.SpellTable.WeaponSpells.ContainsKey(wep2subyte))
                                                        wep2spellid = Database.SpellTable.WeaponSpells[wep2subyte];
                                                    if (attacker.Owner.Spells.ContainsKey(wep2spellid) && Database.SpellTable.SpellInformations.ContainsKey(wep2spellid))
                                                    {
                                                        wep2spell = Database.SpellTable.SpellInformations[wep2spellid][attacker.Owner.Spells[wep2spellid].Level];
                                                        doWep2Spell = ServerBase.Kernel.Rate(wep2spell.Percent);
                                                    }
                                                }
                                            }

                                            if (!attacker.Transformed)
                                            {

                                                if (doWep1Spell)
                                                {
                                                    attack.AttackType = Attack.Magic;
                                                    attack.Decoded = true;
                                                    attack.X = attackedsob.X;
                                                    attack.Y = attackedsob.Y;
                                                    attack.Attacked = attackedsob.UID;
                                                    attack.Damage = wep1spell.ID;
                                                    goto restart;
                                                }
                                                if (doWep2Spell)
                                                {
                                                    attack.AttackType = Attack.Magic;
                                                    attack.Decoded = true;
                                                    attack.X = attackedsob.X;
                                                    attack.Y = attackedsob.Y;
                                                    attack.Attacked = attackedsob.UID;
                                                    attack.Damage = wep2spell.ID;
                                                    goto restart;
                                                }
                                                if (attacker.MapID == 1039)
                                                {
                                                    /* if (wep1bs)
                                                         wep1subyte++;*/
                                                    if (attacker.EntityFlag == EntityFlag.Player)
                                                        if (damage > attackedsob.Hitpoints)
                                                        {
                                                            attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attackedsob.Hitpoints), wep1subyte);
                                                            if (wep2subyte != 0)
                                                            {
                                                                /* if (wep2bs)
                                                                     wep2subyte++;*/
                                                                attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attackedsob.Hitpoints), wep2subyte);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            attacker.Owner.IncreaseProficiencyExperience(damage, wep1subyte);
                                                            if (wep2subyte != 0)
                                                            {
                                                                /*if (wep2bs)
                                                                    wep2subyte++;*/
                                                                attacker.Owner.IncreaseProficiencyExperience(damage, wep2subyte);
                                                            }
                                                        }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltRightHand))
                                        {
                                            Interfaces.IConquerItem rightweapon = attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.AltRightHand);
                                            ushort wep1subyte = (ushort)(rightweapon.ID / 1000), wep2subyte = 0;
                                            bool wep1bs = false, wep2bs = false;
                                            /*if (wep1subyte == 421)
                                            {
                                                wep1bs = true;
                                                wep1subyte = 420;
                                            }
                                            if (wep1subyte == 511)
                                            {
                                                wep1bs = true;
                                                wep1subyte = 601;
                                            }*/
                                            ushort wep1spellid = 0, wep2spellid = 0;
                                            if (Database.SpellTable.WeaponSpells.ContainsKey(wep1subyte))
                                                wep1spellid = Database.SpellTable.WeaponSpells[wep1subyte];
                                            Database.SpellInformation wep1spell = null, wep2spell = null;
                                            bool doWep1Spell = false, doWep2Spell = false;
                                            if (attacker.Owner.Spells.ContainsKey(wep1spellid) && Database.SpellTable.SpellInformations.ContainsKey(wep1spellid))
                                            {
                                                wep1spell = Database.SpellTable.SpellInformations[wep1spellid][attacker.Owner.Spells[wep1spellid].Level];
                                                doWep1Spell = ServerBase.Kernel.Rate(wep1spell.Percent);
                                            }
                                            if (!doWep1Spell)
                                            {
                                                if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltLeftHand))
                                                {
                                                    Interfaces.IConquerItem leftweapon = attacker.Owner.Equipment.TryGetItem((byte)ConquerItem.AltLeftHand);
                                                    wep2subyte = (ushort)(leftweapon.ID / 1000);
                                                    /* if (wep2subyte == 421)
                                                     {
                                                         wep2bs = true;
                                                         wep2subyte = 420;
                                                     }
                                                     if (wep2subyte == 511)
                                                     {
                                                         wep2bs = true;
                                                         wep2subyte = 601;
                                                     }*/
                                                    if (Database.SpellTable.WeaponSpells.ContainsKey(wep2subyte))
                                                        wep2spellid = Database.SpellTable.WeaponSpells[wep2subyte];
                                                    if (attacker.Owner.Spells.ContainsKey(wep2spellid) && Database.SpellTable.SpellInformations.ContainsKey(wep2spellid))
                                                    {
                                                        wep2spell = Database.SpellTable.SpellInformations[wep2spellid][attacker.Owner.Spells[wep2spellid].Level];
                                                        doWep2Spell = ServerBase.Kernel.Rate(wep2spell.Percent);
                                                    }
                                                }
                                            }

                                            if (!attacker.Transformed)
                                            {
                                                if (doWep1Spell)
                                                {
                                                    attack.AttackType = Attack.Magic;
                                                    attack.Decoded = true;
                                                    attack.X = attackedsob.X;
                                                    attack.Y = attackedsob.Y;
                                                    attack.Attacked = attackedsob.UID;
                                                    attack.Damage = wep1spell.ID;
                                                    goto restart;
                                                }
                                                if (doWep2Spell)
                                                {
                                                    attack.AttackType = Attack.Magic;
                                                    attack.Decoded = true;
                                                    attack.X = attackedsob.X;
                                                    attack.Y = attackedsob.Y;
                                                    attack.Attacked = attackedsob.UID;
                                                    attack.Damage = wep2spell.ID;
                                                    goto restart;
                                                }
                                                if (attacker.MapID == 1039)
                                                {
                                                    if (wep1bs)
                                                        wep1subyte++;
                                                    if (attacker.EntityFlag == EntityFlag.Player)
                                                        if (damage > attackedsob.Hitpoints)
                                                        {
                                                            attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attackedsob.Hitpoints), wep1subyte);
                                                            if (wep2subyte != 0)
                                                            {
                                                                if (wep2bs)
                                                                    wep2subyte++;
                                                                attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attackedsob.Hitpoints), wep2subyte);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            attacker.Owner.IncreaseProficiencyExperience(damage, wep1subyte);
                                                            if (wep2subyte != 0)
                                                            {
                                                                if (wep2bs)
                                                                    wep2subyte++;
                                                                attacker.Owner.IncreaseProficiencyExperience(damage, wep2subyte);
                                                            }
                                                        }
                                                }
                                            }
                                        }
                                    }
                                    attack.Damage = damage;
                                    ReceiveAttack(attacker, attackedsob, attack, damage, null);
                                }
                                else
                                {
                                    attacker.AttackPacket = null;
                                }
                            }
                        }
                        else
                        {
                            attacker.AttackPacket = null;
                        }
                    }
                    #endregion
                    #region Ranged
                    else if (attack.AttackType == Attack.Ranged)
                    {

                        if (attacker.Owner.Screen.TryGetValue(attack.Attacked, out attacked))
                        {
                            if (!attacker.Owner.AlternateEquipment)
                            {
                                CheckForExtraWeaponPowers(attacker.Owner, attacked);
                            }
                            else
                            {
                                CheckForExtraWeaponPowers2(attacker.Owner, attacked);
                            }
                            if (attacker.Owner.Equipment.TryGetItem(ConquerItem.LeftWeapon) == null)
                                return;
                            if (!CanAttack(attacker, attacked, null, attack.AttackType == Attack.Melee))
                                return;
                            if (!attacker.Owner.AlternateEquipment)
                            {
                                if (!attacker.Owner.Equipment.Free((byte)ConquerItem.LeftWeapon))
                                {
                                    Interfaces.IConquerItem arrow = attacker.Owner.Equipment.TryGetItem(ConquerItem.LeftWeapon);
                                    arrow.Durability -= 1;
                                    ItemUsage usage = new ItemUsage(true) { UID = arrow.UID, dwParam = arrow.Durability, ID = ItemUsage.UpdateDurability };
                                    usage.Send(attacker.Owner);
                                    if (arrow.Durability <= 0 || arrow.Durability > 5000)
                                    {
                                        Network.PacketHandler.ReloadArrows(attacker.Owner.Equipment.TryGetItem(ConquerItem.LeftWeapon), attacker.Owner);
                                    }
                                }
                            }
                            else
                            {
                                if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltLeftHand))
                                {
                                    Interfaces.IConquerItem arrow = attacker.Owner.Equipment.TryGetItem(ConquerItem.AltLeftHand);
                                    arrow.Durability -= 1;
                                    ItemUsage usage = new ItemUsage(true) { UID = arrow.UID, dwParam = arrow.Durability, ID = ItemUsage.UpdateDurability };
                                    usage.Send(attacker.Owner);
                                    if (arrow.Durability <= 0 || arrow.Durability > 5000)
                                    {
                                        Network.PacketHandler.ReloadArrows(attacker.Owner.Equipment.TryGetItem(ConquerItem.AltLeftHand), attacker.Owner);
                                    }
                                }
                            }
                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= ServerBase.Constants.pScreenDistance)
                            {
                                attack.Effect1 = Attack.AttackEffects1.None;
                                uint damage = Game.Attacking.Calculate.Ranged(attacker, attacked, ref attack);

                                attack.Damage = damage;
                                if (attacker.EntityFlag == EntityFlag.Player && attacked.EntityFlag != EntityFlag.Player)
                                    if (damage > attacked.Hitpoints)
                                    {
                                        attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attacked.Hitpoints), 500);
                                    }
                                    else
                                    {
                                        attacker.Owner.IncreaseProficiencyExperience(damage, 500);
                                    }
                                ReceiveAttack(attacker, attacked, attack, damage, null);
                            }
                        }
                        else if (attacker.Owner.Screen.TryGetSob(attack.Attacked, out attackedsob))
                        {
                            if (CanAttack(attacker, attackedsob, null))
                            {
                                if (attacker.Owner.Equipment.TryGetItem(ConquerItem.LeftWeapon) == null)
                                    return;
                                if (attacker.MapID != 1039)
                                {
                                    if (!attacker.Owner.AlternateEquipment)
                                    {
                                        if (!attacker.Owner.Equipment.Free((byte)ConquerItem.LeftWeapon))
                                        {
                                            Interfaces.IConquerItem arrow = attacker.Owner.Equipment.TryGetItem(ConquerItem.LeftWeapon);
                                            arrow.Durability -= 1;
                                            ItemUsage usage = new ItemUsage(true) { UID = arrow.UID, dwParam = arrow.Durability, ID = ItemUsage.UpdateDurability };
                                            usage.Send(attacker.Owner);
                                            if (arrow.Durability <= 0 || arrow.Durability > 5000)
                                            {
                                                Network.PacketHandler.ReloadArrows(attacker.Owner.Equipment.TryGetItem(ConquerItem.LeftWeapon), attacker.Owner);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!attacker.Owner.Equipment.Free((byte)ConquerItem.AltLeftHand))
                                        {
                                            Interfaces.IConquerItem arrow = attacker.Owner.Equipment.TryGetItem(ConquerItem.AltLeftHand);
                                            arrow.Durability -= 1;
                                            ItemUsage usage = new ItemUsage(true) { UID = arrow.UID, dwParam = arrow.Durability, ID = ItemUsage.UpdateDurability };
                                            usage.Send(attacker.Owner);
                                            if (arrow.Durability <= 0 || arrow.Durability > 5000)
                                            {
                                                Network.PacketHandler.ReloadArrows(attacker.Owner.Equipment.TryGetItem(ConquerItem.AltLeftHand), attacker.Owner);
                                            }
                                        }
                                    }
                                }
                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= ServerBase.Constants.pScreenDistance)
                                {
                                    attack.Effect1 = Attack.AttackEffects1.None;
                                    uint damage = Game.Attacking.Calculate.Ranged(attacker, attackedsob, ref attack);
                                    attack.Damage = damage;
                                    ReceiveAttack(attacker, attackedsob, attack, damage, null);
                                    if (damage > attackedsob.Hitpoints)
                                    {
                                        attacker.Owner.IncreaseProficiencyExperience(Math.Min(damage, attackedsob.Hitpoints), 500);
                                    }
                                    else
                                    {
                                        attacker.Owner.IncreaseProficiencyExperience(damage, 500);
                                    }
                                }
                            }
                        }
                        else
                        {
                            attacker.AttackPacket = null;
                        }
                    }
                    #endregion
                    #region Magic
                    else if (attack.AttackType == Attack.Magic)
                    {
                        if (!attacker.Owner.AlternateEquipment)
                        {
                            CheckForExtraWeaponPowers(attacker.Owner, attacked);
                        }
                        else
                        {
                            CheckForExtraWeaponPowers2(attacker.Owner, attacked);
                        }
                        uint Experience = 100;
                        bool shuriken = false;
                        ushort spellID = SpellID;
                        if (SpellID >= 3090 && SpellID <= 3306)
                            spellID = 3090;
                        if (spellID == 6012)
                            shuriken = true;

                        if (attacker == null)
                            return;
                        if (attacker.Owner == null)
                        {
                            attacker.AttackPacket = null;
                            return;
                        }
                        if (attacker.Owner.Spells == null)
                        {
                            attacker.Owner.Spells = new SafeDictionary<ushort, PhoenixProject.Interfaces.ISkill>(10000);
                            attacker.AttackPacket = null;
                            return;
                        }
                        if (attacker.Owner.Spells[spellID] == null && spellID != 6012)
                        {
                            attacker.AttackPacket = null;
                            return;
                        }

                        Database.SpellInformation spell = null;
                        if (shuriken)
                            spell = Database.SpellTable.SpellInformations[6010][0];
                        else
                        {
                            byte choselevel = 0;
                            if (spellID == SpellID)
                                choselevel = attacker.Owner.Spells[spellID].Level;
                            if (Database.SpellTable.SpellInformations[SpellID] != null && !Database.SpellTable.SpellInformations[SpellID].ContainsKey(choselevel))
                                choselevel = (byte)(Database.SpellTable.SpellInformations[SpellID].Count - 1);

                            spell = Database.SpellTable.SpellInformations[SpellID][choselevel];
                        }
                        if (spell == null)
                        {
                            attacker.AttackPacket = null;
                            return;
                        }
                        attacked = null;
                        attackedsob = null;
                        if (attacker.Owner.Screen.TryGetValue(Target, out attacked) || attacker.Owner.Screen.TryGetSob(Target, out attackedsob) || Target == attacker.UID || spell.Sort != 1)
                        {
                            if (Target == attacker.UID)
                                attacked = attacker;
                            if (attacked != null)
                            {
                                if (attacked.Dead && spell.Sort != Database.SpellSort.Revive && spell.ID != 10405 && spell.ID != 10425)
                                {
                                    attacker.AttackPacket = null;
                                    return;
                                }
                            }
                            if (Target >= 400000 && Target <= 600000 || Target >= 800000)
                            {
                                if (attacked == null && attackedsob == null)
                                    return;
                            }
                            else if (Target != 0 && attackedsob == null)
                                return;
                            if (attacked != null)
                            {
                                if (attacked.EntityFlag == EntityFlag.Monster)
                                {
                                    if (spell.CanKill == 1)
                                    {
                                        if (attacked.MonsterInfo.InSight == 0)
                                        {
                                            attacked.MonsterInfo.InSight = attacker.UID;
                                        }
                                    }
                                }
                            }
                            if (!attacker.Owner.Spells.ContainsKey(spellID))
                            {
                                if (spellID != 6012)
                                    return;
                            }
                            if (spell != null)
                            {
                                if (!attacker.Owner.AlternateEquipment)
                                {
                                    if (spell.OnlyWithThisWeaponSubtype != 0)
                                    {
                                        uint firstwepsubtype, secondwepsubtype;

                                        if (!attacker.Owner.Equipment.Free(4))
                                        {
                                            firstwepsubtype = attacker.Owner.Equipment.Objects[3].ID / 1000;
                                            if (!attacker.Owner.Equipment.Free(5) && attacker.Owner.Equipment.Objects[4] != null)
                                            {
                                                secondwepsubtype = attacker.Owner.Equipment.Objects[4].ID / 1000;
                                                if (firstwepsubtype != spell.OnlyWithThisWeaponSubtype)
                                                {
                                                    if (secondwepsubtype != spell.OnlyWithThisWeaponSubtype)
                                                    {
                                                        attacker.AttackPacket = null;
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (firstwepsubtype != spell.OnlyWithThisWeaponSubtype)
                                                {
                                                    attacker.AttackPacket = null;
                                                    return;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            attacker.AttackPacket = null;
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        if (spell.OnlyWithThisWeaponSubtype != 0)
                                        {
                                            uint firstwepsubtype, secondwepsubtype;

                                            if (!attacker.Owner.Equipment.Free(24))
                                            {
                                                firstwepsubtype = attacker.Owner.Equipment.Objects[23].ID / 1000;
                                                if (!attacker.Owner.Equipment.Free(24) && attacker.Owner.Equipment.Objects[24] != null)
                                                {
                                                    secondwepsubtype = attacker.Owner.Equipment.Objects[24].ID / 1000;
                                                    if (firstwepsubtype != spell.OnlyWithThisWeaponSubtype)
                                                    {
                                                        if (secondwepsubtype != spell.OnlyWithThisWeaponSubtype)
                                                        {
                                                            attacker.AttackPacket = null;
                                                            return;
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (firstwepsubtype != spell.OnlyWithThisWeaponSubtype)
                                                    {
                                                        attacker.AttackPacket = null;
                                                        return;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                attacker.AttackPacket = null;
                                                return;
                                            }
                                        }

                                    }
                                }
                                if (attacker.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.Coder)
                                {
                                    attacker.Owner.Send(new Message("Spell ID: " + spellID, System.Drawing.Color.CadetBlue, Message.Talk));
                                }
                                switch (spellID)
                                {
                                    #region Single magic damage spells
                                    case 1000:
                                    case 1001:
                                    case 1002:
                                    case 1150:
                                    case 1160:
                                    case 1180:
                                    case 1320:
                                    case 10310:
                                    case 11180:
                                        //case 11040:
                                        //case 10381:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                if (attacked != null)
                                                {
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Distance)
                                                    {
                                                        SpellUse suse = new SpellUse(true);
                                                        suse.Attacker = attacker.UID;
                                                        suse.SpellID = spell.ID;
                                                        suse.SpellLevel = spell.Level;
                                                        suse.X = X;
                                                        suse.Y = Y;

                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);

                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                            uint damage = Game.Attacking.Calculate.Magic(attacker, attacked, spell, ref attack);
                                                            suse.Effect1 = attack.Effect1;

                                                            ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                            suse.Targets.Add(attacked.UID, damage);

                                                            if (attacked.EntityFlag == EntityFlag.Player)
                                                                attacked.Owner.SendScreen(suse, true);
                                                            else
                                                                attacked.MonsterInfo.SendScreen(suse);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        attacker.AttackPacket = null;
                                                    }
                                                }
                                                else
                                                {
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Distance)
                                                    {
                                                        SpellUse suse = new SpellUse(true);
                                                        suse.Attacker = attacker.UID;
                                                        suse.SpellID = spell.ID;
                                                        suse.SpellLevel = spell.Level;
                                                        suse.X = X;
                                                        suse.Y = Y;

                                                        if (CanAttack(attacker, attackedsob, spell))
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);
                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                            uint damage = Game.Attacking.Calculate.Magic(attacker, attackedsob, spell, ref attack);
                                                            suse.Effect1 = attack.Effect1;

                                                            ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                            suse.Targets.Add(attackedsob.UID, damage);

                                                            attacker.Owner.SendScreen(suse, true);
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                attacker.AttackPacket = null;
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Single heal/meditation spells
                                    case 1190:
                                    case 1195:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;

                                                uint damage = spell.Power;
                                                if (spell.ID == 1190)
                                                {
                                                    Experience = damage = Math.Min(damage, attacker.MaxHitpoints - attacker.Hitpoints);
                                                    attacker.Hitpoints += damage;
                                                }
                                                else
                                                {
                                                    Experience = damage = Math.Min(damage, (uint)(attacker.MaxMana - attacker.Mana));
                                                    attacker.Mana += (ushort)damage;
                                                }

                                                suse.Targets.Add(attacker.UID, spell.Power);

                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Multi heal spells
                                    case 1005:
                                    case 1055:
                                    case 1170:
                                    case 1175:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                if (attackedsob != null)
                                                {
                                                    if (attacker.MapID == 1038)
                                                        break;
                                                    if (attacker.MapID == 2071)
                                                        break;
                                                    if (attacker.MapID == 1509)
                                                        break;
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Distance)
                                                    {
                                                        PrepareSpell(spell, attacker.Owner);

                                                        uint damage = spell.Power;
                                                        damage = Math.Min(damage, attackedsob.MaxHitpoints - attackedsob.Hitpoints);
                                                        attackedsob.Hitpoints += damage;
                                                        Experience += damage;
                                                        suse.Targets.Add(attackedsob.UID, damage);

                                                        attacker.Owner.SendScreen(suse, true);
                                                    }
                                                }
                                                else
                                                {
                                                    if (spell.Multi == 1)
                                                    {
                                                        if (attacker.Owner.Team != null)
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);
                                                            foreach (Client.GameState teammate in attacker.Owner.Team.Teammates)
                                                            {
                                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, teammate.Entity.X, teammate.Entity.Y) <= spell.Distance)
                                                                {
                                                                    uint damage = spell.Power;
                                                                    damage = Math.Min(damage, teammate.Entity.MaxHitpoints - teammate.Entity.Hitpoints);
                                                                    teammate.Entity.Hitpoints += damage;
                                                                    Experience += damage;
                                                                    suse.Targets.Add(teammate.Entity.UID, damage);

                                                                    if (spell.NextSpellID != 0)
                                                                    {
                                                                        attack.Damage = spell.NextSpellID;
                                                                        attacker.AttackPacket = attack;
                                                                    }
                                                                    else
                                                                    {
                                                                        attacker.AttackPacket = null;
                                                                    }
                                                                }
                                                            }
                                                            if (attacked.EntityFlag == EntityFlag.Player)
                                                                attacked.Owner.SendScreen(suse, true);
                                                            else
                                                                attacked.MonsterInfo.SendScreen(suse);
                                                        }
                                                        else
                                                        {
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Distance)
                                                            {
                                                                PrepareSpell(spell, attacker.Owner);

                                                                uint damage = spell.Power;
                                                                damage = Math.Min(damage, attacked.MaxHitpoints - attacked.Hitpoints);
                                                                attacked.Hitpoints += damage;
                                                                Experience += damage;
                                                                suse.Targets.Add(attacked.UID, damage);

                                                                if (spell.NextSpellID != 0)
                                                                {
                                                                    attack.Damage = spell.NextSpellID;
                                                                    attacker.AttackPacket = attack;
                                                                }
                                                                else
                                                                {
                                                                    attacker.AttackPacket = null;
                                                                }
                                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                                    attacked.Owner.SendScreen(suse, true);
                                                                else
                                                                    attacked.MonsterInfo.SendScreen(suse);
                                                            }
                                                            else
                                                            {
                                                                attacker.AttackPacket = null;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Distance)
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);

                                                            uint damage = spell.Power;
                                                            damage = Math.Min(damage, attacked.MaxHitpoints - attacked.Hitpoints);
                                                            attacked.Hitpoints += damage;
                                                            Experience += damage;
                                                            suse.Targets.Add(attacked.UID, damage);

                                                            if (spell.NextSpellID != 0)
                                                            {
                                                                attack.Damage = spell.NextSpellID;
                                                                attacker.AttackPacket = attack;
                                                            }
                                                            else
                                                            {
                                                                attacker.AttackPacket = null;
                                                            }
                                                            if (attacked.EntityFlag == EntityFlag.Player)
                                                                attacked.Owner.SendScreen(suse, true);
                                                            else
                                                                attacked.MonsterInfo.SendScreen(suse);
                                                        }
                                                        else
                                                        {
                                                            attacker.AttackPacket = null;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                attacker.AttackPacket = null;
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Revive
                                    case 1050:
                                    case 1100:
                                        {
                                            if (attackedsob != null)
                                                return;
                                            if (ServerBase.Constants.revnomap.Contains(attacker.MapID))
                                                return;
                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Distance)
                                            {
                                                if (CanUseSpell(spell, attacker.Owner))
                                                {
                                                    PrepareSpell(spell, attacker.Owner);

                                                    SpellUse suse = new SpellUse(true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = spell.ID;
                                                    suse.SpellLevel = spell.Level;
                                                    suse.X = X;
                                                    suse.Y = Y;

                                                    //suse.Targets.Add(attacked.UID, 0);
                                                    //attacked.Action = PhoenixProject.Game.Enums.ConquerAction.None;
                                                    //attacked.Owner.ReviveStamp = Time32.Now;
                                                    //attacked.Owner.Attackable = false;

                                                    attacked.Owner.Entity.Action = PhoenixProject.Game.Enums.ConquerAction.None;
                                                    attacked.Owner.ReviveStamp = Time32.Now;
                                                    attacked.Owner.Attackable = false;

                                                    attacked.Owner.Entity.TransformationID = 0;
                                                    attacked.Owner.Entity.RemoveFlag(Update.Flags.Dead);
                                                    attacked.Owner.Entity.RemoveFlag(Update.Flags.Ghost);
                                                    attacked.Owner.Entity.Hitpoints = attacked.Owner.Entity.MaxHitpoints;

                                                    attacked.Ressurect();

                                                    attacked.Owner.SendScreen(suse, true);
                                                }
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Linear spells
                                    case 1045:
                                    case 1046:
                                    // case 11110:
                                    case 1260:
                                    case 11000:
                                    case 11005:
                                        {

                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);
                                                Game.Attacking.InLineAlgorithm ila = new PhoenixProject.Game.Attacking.InLineAlgorithm(attacker.X,
                                            X, attacker.Y, Y, (byte)spell.Range, InLineAlgorithm.Algorithm.DDA);
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = SpellID;
                                                suse.SpellLevel = attacker.Owner.Spells[SpellID].Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                int xx = 0;
                                                int yy = 0;
                                                foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                {
                                                    if (_obj == null)
                                                        continue;
                                                    if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                    {
                                                        attacked = _obj as Entity;
                                                        if (ila.InLine(attacked.X, attacked.Y))
                                                        {
                                                            if (attacked.X != xx && attacked.Y != yy)
                                                            {
                                                                if (!CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                    continue;

                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                                suse.Effect1 = attack.Effect1;

                                                                attack.Damage = damage;
                                                                xx = attacked.X;
                                                                yy = attacked.Y;
                                                                ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                suse.Targets.Add(attacked.UID, damage);
                                                            }
                                                        }
                                                    }
                                                    else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                    {
                                                        attackedsob = _obj as SobNpcSpawn;

                                                        if (ila.InLine(attackedsob.X, attackedsob.Y))
                                                        {
                                                            if (!CanAttack(attacker, attackedsob, spell))
                                                                continue;

                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                            damage = (uint)(damage * spell.PowerPercent);
                                                            attack.Damage = damage;

                                                            ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                            suse.Targets.Add(attackedsob.UID, damage);
                                                        }
                                                    }
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region BladeTempst

                                    case 11110:
                                        {

                                            bool yes = true;
                                            Game.Map Map = attacker.Owner.Map;
                                            // ushort rr = ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y);
                                            Game.Attacking.InLineAlgorithm ila = new PhoenixProject.Game.Attacking.InLineAlgorithm(attacker.X,
                                               X, attacker.Y, Y, (byte)ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y), InLineAlgorithm.Algorithm.DDA);
                                            List<InLineAlgorithm.coords>.Enumerator enumerator6;
                                            ushort x = 0;
                                            ushort y = 0;
                                            using (enumerator6 = ila.getCoords.GetEnumerator())
                                            {
                                                while (enumerator6.MoveNext())
                                                {
                                                    if (yes)
                                                    {
                                                        Func<IMapObject, bool> func4 = null;
                                                        InLineAlgorithm.coords coord = enumerator6.Current;

                                                        if (Map.Floor[coord.X, coord.Y, Game.MapObjectType.Player, null])
                                                        {
                                                            if (attacker.MapID == 1038)
                                                            {
                                                                if (attacker.X > 223 && attacker.Y < 185 && coord.X < 224)
                                                                {
                                                                    // Console.WriteLine(" r " + PhoenixProject.Game.ConquerStructures.Society.GuildWar.RightGate.Mesh + "");
                                                                    if (PhoenixProject.Game.ConquerStructures.Society.GuildWar.RightGate.Mesh == 271)
                                                                    {
                                                                        yes = false;
                                                                    }
                                                                    else
                                                                    {
                                                                        yes = true;
                                                                        x = (ushort)coord.X;
                                                                        y = (ushort)coord.Y;
                                                                        foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                                        {
                                                                            if (_obj == null)
                                                                                continue;
                                                                            if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                                            {
                                                                                attacked = _obj as Entity;
                                                                                if (ServerBase.Kernel.GetDistance((ushort)coord.X, (ushort)coord.Y, attacked.X, attacked.Y) <= 1)
                                                                                {
                                                                                    attacked.CanBlade = true;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //Console.WriteLine(" r " + PhoenixProject.Game.ConquerStructures.Society.GuildWar.LeftGate.Mesh + "");
                                                                    if (attacker.X < 170 && attacker.Y > 210 && coord.Y < 211)
                                                                    {
                                                                        if (PhoenixProject.Game.ConquerStructures.Society.GuildWar.LeftGate.Mesh == 241)
                                                                        {
                                                                            yes = false;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        yes = true;
                                                                        x = (ushort)coord.X;
                                                                        y = (ushort)coord.Y;
                                                                        foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                                        {
                                                                            if (_obj == null)
                                                                                continue;
                                                                            if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                                            {
                                                                                attacked = _obj as Entity;
                                                                                if (ServerBase.Kernel.GetDistance((ushort)coord.X, (ushort)coord.Y, attacked.X, attacked.Y) <= 1)
                                                                                {
                                                                                    attacked.CanBlade = true;
                                                                                }
                                                                            }
                                                                        }
                                                                    }

                                                                }



                                                            }
                                                            else
                                                            {
                                                                yes = true;
                                                                x = (ushort)coord.X;
                                                                y = (ushort)coord.Y;
                                                                foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                                {
                                                                    if (_obj == null)
                                                                        continue;
                                                                    if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                                    {
                                                                        attacked = _obj as Entity;
                                                                        if (ServerBase.Kernel.GetDistance((ushort)coord.X, (ushort)coord.Y, attacked.X, attacked.Y) <= 1)
                                                                        {
                                                                            attacked.CanBlade = true;
                                                                        }
                                                                    }
                                                                }
                                                            }

                                                        }
                                                        else
                                                        {
                                                            yes = false;
                                                        }
                                                    }
                                                }
                                            }
                                            yes = true;
                                            X = x;
                                            Y = y;

                                            if (Map.Floor[X, Y, Game.MapObjectType.Player, null] && yes)
                                            {
                                                if (CanUseSpell(spell, attacker.Owner))
                                                {
                                                    PrepareSpell(spell, attacker.Owner);

                                                    SpellUse suse = new SpellUse(true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = SpellID;
                                                    suse.SpellLevel = attacker.Owner.Spells[SpellID].Level;
                                                    suse.X = X;
                                                    suse.Y = Y;
                                                    int xx = 0;
                                                    int yy = 0;
                                                    foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                    {
                                                        if (_obj == null)
                                                            continue;
                                                        if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                        {
                                                            attacked = _obj as Entity;
                                                            if (attacked.CanBlade)
                                                            {

                                                                // if (attacked.X != xx && attacked.Y != yy)
                                                                // {
                                                                if (!CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                    continue;

                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                               // uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                                suse.Effect1 = attack.Effect1;

                                                                attack.Damage = damage;
                                                                // xx = attacked.X;
                                                                // yy = attacked.Y;
                                                                ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                suse.Targets.Add(attacked.UID, damage);
                                                                attacked.CanBlade = false;
                                                                if (!attacked.Dead)
                                                                {
                                                                    if (attacker.Owner.Spells.ContainsKey(11120))
                                                                    {
                                                                        BlackSpot spot = new BlackSpot
                                                                        {
                                                                            Remove = 0,
                                                                            Identifier = attacked.UID
                                                                        };
                                                                        attacker.Owner.Send((byte[])spot);

                                                                        attacked.BlackSpotTime = Time32.Now;
                                                                        attacked.BlackSpotTime2 = 8;
                                                                        attacked.BlackSpots = true;
                                                                        attacker.Owner.IncreaseSpellExperience(80, 11120);
                                                                    }
                                                                }
                                                                // }
                                                            }
                                                        }
                                                        else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                        {
                                                            attackedsob = _obj as SobNpcSpawn;

                                                            if (ila.InLine(attackedsob.X, attackedsob.Y))
                                                            {
                                                                if (!CanAttack(attacker, attackedsob, spell))
                                                                    continue;

                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                damage = (uint)(damage * spell.PowerPercent);
                                                                attack.Damage = damage;

                                                                ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                                suse.Targets.Add(attackedsob.UID, damage);
                                                            }
                                                        }
                                                    }
                                                    attacker.X = X;
                                                    attacker.Y = Y;
                                                    attacker.Owner.SendScreen(attack, true);
                                                    attacker.Owner.SendScreen(suse, true);
                                                    attacker.Owner.Screen.Reload(attack);
                                                }
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region XPSpells inofensive
                                    case 1015:
                                    case 1020:
                                    case 1025:
                                    case 1110:
                                    case 6011:
                                    case 10390:
                                        {

                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;

                                                suse.Targets.Add(attacked.UID, 0);

                                                if (spell.ID == 6011)
                                                {
                                                    attacker.FatalStrikeStamp = Time32.Now;
                                                    attacker.FatalStrikeTime = 60;
                                                    attacker.AddFlag(Update.Flags.FatalStrike);
                                                    attacker.RemoveFlag(Update.Flags.Ride);
                                                }
                                                else
                                                {
                                                    if (spell.ID == 1110 || spell.ID == 1025 || spell.ID == 10390)
                                                    {
                                                        if (!attacked.OnKOSpell())
                                                            attacked.KOCount = 0;

                                                        attacked.KOSpell = spell.ID;
                                                        if (spell.ID == 1110)
                                                        {
                                                            attacked.CycloneStamp = Time32.Now;
                                                            attacked.CycloneTime = 20;
                                                            attacked.AddFlag(Update.Flags.Cyclone);
                                                        }
                                                        else if (spell.ID == 10390)
                                                        {
                                                            attacked.OblivionStamp = Time32.Now;
                                                            attacked.OblivionTime = 20;
                                                            attacked.AddFlag2(Update.Flags2.Oblivion);
                                                        }

                                                        else
                                                        {
                                                            attacked.SupermanStamp = Time32.Now;
                                                            attacked.SupermanTime = 20;
                                                            attacked.AddFlag(Update.Flags.Superman);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (spell.ID == 1020)
                                                        {
                                                            attacked.ShieldTime = 0;
                                                            attacked.ShieldStamp = Time32.Now;
                                                            attacked.MagicShieldStamp = Time32.Now;
                                                            attacked.MagicShieldTime = 0;

                                                            attacked.AddFlag(Update.Flags.MagicShield);
                                                            attacked.ShieldStamp = Time32.Now;
                                                            attacked.ShieldIncrease = spell.PowerPercent;
                                                            attacked.ShieldTime = (byte)spell.Duration;
                                                        }
                                                        else
                                                        {

                                                            attacked.AccuracyStamp = Time32.Now;
                                                            attacked.StarOfAccuracyStamp = Time32.Now;
                                                            attacked.StarOfAccuracyTime = 0;
                                                            attacked.AccuracyTime = 0;

                                                            attacked.AddFlag(Update.Flags.StarOfAccuracy);
                                                            attacked.AccuracyStamp = Time32.Now;
                                                            attacked.AccuracyTime = (byte)spell.Duration;
                                                        }
                                                    }
                                                }
                                                attacked.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Circle spells
                                    case 1010:
                                    case 1115:
                                    case 1120:
                                    case 1125:
                                    case 3090:
                                    case 5001:
                                    case 8030:
                                    case 11170:
                                    case 10315:
                                    case 11190:
                                        //case 11050:
                                        {
                                            // Console.WriteLine("xx3");
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                //Console.WriteLine("x4");
                                                PrepareSpell(spell, attacker.Owner);
                                                //Console.WriteLine("xx5");
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                UInt16 ox, oy;
                                                ox = attacker.X;
                                                oy = attacker.Y;
                                                if (spellID == 10315 || spellID == 11190)
                                                {
                                                    /* if (attacker.MapID == 1950)
                                                     {
                                                         break;
                                                     }*/
                                                    bool yes = true;
                                                    Game.Map Map = attacker.Owner.Map;

                                                    Game.Attacking.InLineAlgorithm ila = new PhoenixProject.Game.Attacking.InLineAlgorithm(attacker.X,
                                                       X, attacker.Y, Y, (byte)spell.Range, InLineAlgorithm.Algorithm.DDA);
                                                    List<InLineAlgorithm.coords>.Enumerator enumerator6;
                                                    ushort x = 0;
                                                    ushort y = 0;
                                                    using (enumerator6 = ila.getCoords.GetEnumerator())
                                                    {
                                                        while (enumerator6.MoveNext())
                                                        {
                                                            if (yes)
                                                            {
                                                                Func<IMapObject, bool> func4 = null;
                                                                InLineAlgorithm.coords coord = enumerator6.Current;
                                                                if (Map.Floor[coord.X, coord.Y, Game.MapObjectType.Player, null])
                                                                {
                                                                    if (attacker.MapID == 1038)
                                                                    {
                                                                        if (attacker.X > 223 && attacker.Y < 185 && coord.X < 224)
                                                                        {
                                                                            // Console.WriteLine(" r " + PhoenixProject.Game.ConquerStructures.Society.GuildWar.RightGate.Mesh + "");
                                                                            if (PhoenixProject.Game.ConquerStructures.Society.GuildWar.RightGate.Mesh == 271)
                                                                            {
                                                                                yes = false;
                                                                            }
                                                                            else
                                                                            {
                                                                                yes = true;
                                                                                x = (ushort)coord.X;
                                                                                y = (ushort)coord.Y;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            //Console.WriteLine(" r " + PhoenixProject.Game.ConquerStructures.Society.GuildWar.LeftGate.Mesh + "");
                                                                            if (attacker.X < 170 && attacker.Y > 210 && coord.Y < 211)
                                                                            {
                                                                                if (PhoenixProject.Game.ConquerStructures.Society.GuildWar.LeftGate.Mesh == 241)
                                                                                {
                                                                                    yes = false;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                yes = true;
                                                                                x = (ushort)coord.X;
                                                                                y = (ushort)coord.Y;
                                                                            }

                                                                        }


                                                                    }
                                                                    else
                                                                    {
                                                                        yes = true;
                                                                        x = (ushort)coord.X;
                                                                        y = (ushort)coord.Y;
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    yes = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    yes = true;
                                                    X = x;
                                                    Y = y;

                                                    Attack npacket = new Attack(true);
                                                    npacket.Attacker = attacker.UID;
                                                    npacket.AttackType = 53;
                                                    npacket.X = X;
                                                    npacket.Y = Y;
                                                    Writer.WriteUInt16(spell.ID, 24, npacket.ToArray());
                                                    Writer.WriteByte(spell.Level, 26, npacket.ToArray());
                                                    attacker.Owner.SendScreen(npacket, true);
                                                    attacker.X = X;
                                                    attacker.Y = Y;
                                                    attacker.SendSpawn(attacker.Owner);
                                                    attacker.Owner.Screen.Reload(npacket);
                                                }

                                                List<IMapObject> objects = new List<IMapObject>();
                                                if (attacker.Owner.Screen.Objects.Count() > 0)
                                                    objects = GetObjects(ox, oy, attacker.Owner);
                                                if (objects != null)
                                                {
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= spell.Range)
                                                    {
                                                        if (spellID == 10315 || spellID == 11190)
                                                        {
                                                            foreach (IMapObject objs in objects.ToArray())
                                                            {
                                                                if (objs == null)
                                                                    continue;

                                                                if (objs.MapObjType == MapObjectType.Monster || objs.MapObjType == MapObjectType.Player)
                                                                {
                                                                    attacked = objs as Entity;
                                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Range)
                                                                    {
                                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                        {
                                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, ref attack);
                                                                            suse.Effect1 = attack.Effect1;
                                                                            if (spell.Power > 0)
                                                                            {
                                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                                damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                                                //damage = Game.Attacking.Calculate.Magic(attacker, attacked, spell, ref attack);
                                                                                suse.Effect1 = attack.Effect1;
                                                                            }
                                                                            if (spell.ID == 8030)
                                                                            {
                                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                                damage = Game.Attacking.Calculate.Ranged(attacker, attacked, ref attack);
                                                                            }

                                                                            ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                            suse.Targets.Add(attacked.UID, damage);
                                                                        }
                                                                    }
                                                                }
                                                                else if (objs.MapObjType == MapObjectType.SobNpc)
                                                                {
                                                                    attackedsob = objs as SobNpcSpawn;
                                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Range)
                                                                    {
                                                                        if (CanAttack(attacker, attackedsob, spell))
                                                                        {
                                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                            if (spell.Power > 0)
                                                                            {
                                                                                damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                                               // damage = Game.Attacking.Calculate.Magic(attacker, attackedsob, spell, ref attack);
                                                                            }
                                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                                            if (spell.ID == 8030)
                                                                                damage = Game.Attacking.Calculate.Ranged(attacker, attackedsob, ref attack);
                                                                            suse.Effect1 = attack.Effect1;
                                                                            ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                                            suse.Targets.Add(attackedsob.UID, damage);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                            {
                                                                if (_obj == null)
                                                                    continue;
                                                                if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                                {
                                                                    attacked = _obj as Entity;
                                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Range)
                                                                    {
                                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                        {
                                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, ref attack);
                                                                            suse.Effect1 = attack.Effect1;
                                                                            if (spell.Power > 0)
                                                                            {
                                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                                damage = Game.Attacking.Calculate.Magic(attacker, attacked, spell, ref attack);
                                                                                suse.Effect1 = attack.Effect1;
                                                                            }
                                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                                            if (spell.ID == 8030)
                                                                                damage = Game.Attacking.Calculate.Ranged(attacker, attacked, ref attack);
                                                                            if (spell.ID == 1115)
                                                                                damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);


                                                                            ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                            suse.Targets.Add(attacked.UID, damage);
                                                                        }
                                                                    }
                                                                }
                                                                else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                                {
                                                                    attackedsob = _obj as SobNpcSpawn;
                                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Range)
                                                                    {
                                                                        if (CanAttack(attacker, attackedsob, spell))
                                                                        {
                                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                            if (spell.Power > 0)
                                                                                damage = Game.Attacking.Calculate.Magic(attacker, attackedsob, spell, ref attack);
                                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                                            if (spell.ID == 8030)
                                                                                damage = Game.Attacking.Calculate.Ranged(attacker, attackedsob, ref attack);

                                                                            suse.Effect1 = attack.Effect1;
                                                                            ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                                            suse.Targets.Add(attackedsob.UID, damage);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        attacker.AttackPacket = null;
                                                    }
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Buffers
                                    case 1075:
                                    case 1085:
                                    case 1090:
                                    case 1095:
                                    case 3080:
                                    case 10405://this is not what I edited yesterday...
                                    case 30000:
                                    case 11160:
                                    case 11200:
                                        {
                                            if (attackedsob != null)
                                            {
                                                if (CanUseSpell(spell, attacker.Owner))
                                                {
                                                    PrepareSpell(spell, attacker.Owner);

                                                    SpellUse suse = new SpellUse(true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = spell.ID;
                                                    suse.SpellLevel = spell.Level;
                                                    suse.X = X;
                                                    suse.Y = Y;

                                                    suse.Targets.Add(attackedsob.UID, 0);

                                                    attacker.Owner.SendScreen(suse, true);
                                                }
                                            }
                                            else
                                            {
                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Distance)
                                                {
                                                    if (CanUseSpell(spell, attacker.Owner))
                                                    {
                                                        PrepareSpell(spell, attacker.Owner);

                                                        SpellUse suse = new SpellUse(true);
                                                        suse.Attacker = attacker.UID;
                                                        suse.SpellID = spell.ID;
                                                        suse.SpellLevel = spell.Level;
                                                        suse.X = X;
                                                        suse.Y = Y;

                                                        suse.Targets.Add(attacked.UID, 0);

                                                        if (spell.ID == 1075 || spell.ID == 1085)
                                                        {
                                                            if (spell.ID == 1075)
                                                            {
                                                                attacked.AddFlag(Update.Flags.Invisibility);
                                                                attacked.InvisibilityStamp = Time32.Now;
                                                                attacked.InvisibilityTime = (byte)spell.Duration;
                                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                                    attacked.Owner.Send(ServerBase.Constants.Invisibility((int)spell.Duration));
                                                            }
                                                            else
                                                            {
                                                                attacked.AccuracyStamp = Time32.Now;
                                                                attacked.StarOfAccuracyStamp = Time32.Now;
                                                                attacked.StarOfAccuracyTime = 0;
                                                                attacked.AccuracyTime = 0;

                                                                attacked.AddFlag(Update.Flags.StarOfAccuracy);
                                                                attacked.StarOfAccuracyStamp = Time32.Now;
                                                                attacked.StarOfAccuracyTime = (byte)spell.Duration;
                                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                                    attacked.Owner.Send(ServerBase.Constants.Accuracy((int)spell.Duration));

                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (spell.ID == 1090)
                                                            {
                                                                attacked.ShieldTime = 0;
                                                                attacked.ShieldStamp = Time32.Now;
                                                                attacked.MagicShieldStamp = Time32.Now;
                                                                attacked.MagicShieldTime = 0;

                                                                attacked.AddFlag(Update.Flags.MagicShield);
                                                                attacked.MagicShieldStamp = Time32.Now;
                                                                attacked.MagicShieldIncrease = spell.PowerPercent;
                                                                attacked.MagicShieldTime = (byte)spell.Duration;
                                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                                    attacked.Owner.Send(ServerBase.Constants.Shield(spell.PowerPercent, (int)spell.Duration));
                                                            }
                                                            else if (spell.ID == 1095)
                                                            {
                                                                attacked.AddFlag(Update.Flags.Stigma);
                                                                attacked.StigmaStamp = Time32.Now;
                                                                attacked.StigmaIncrease = spell.PowerPercent;
                                                                attacked.StigmaTime = (byte)spell.Duration;
                                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                                    attacked.Owner.Send(ServerBase.Constants.Stigma(spell.PowerPercent, (int)spell.Duration));
                                                            }
                                                            else if (spell.ID == 11200)
                                                            {

                                                                if (attacker.ContainsFlag(Update.Flags.Ride))
                                                                {
                                                                    attacker.RemoveFlag(Update.Flags.Ride);
                                                                }
                                                                attacker.AddFlag3(Update.Flags3.MagicDefender);
                                                                attacker.MagicDefenderStamp = Time32.Now;
                                                                attacker.MagicDefenderIncrease = spell.PowerPercent;
                                                                attacker.MagicDefenderTime = (byte)spell.Duration;

                                                                if (attacker.EntityFlag == EntityFlag.Player)
                                                                {

                                                                    attacker.Owner.SendScreen(suse, true);
                                                                    SyncPacket packet = new SyncPacket
                                                                    {
                                                                        Identifier = attacker.UID,
                                                                        Count = 2,
                                                                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                                                                        StatusFlag1 = (ulong)attacker.StatusFlag,
                                                                        StatusFlag2 = (ulong)attacker.StatusFlag2,
                                                                        Unknown1 = 0x31,
                                                                        StatusFlagOffset = 0x80,
                                                                        Time = (uint)spell.Duration,
                                                                        Value = 10,
                                                                        Level = spell.Level
                                                                    };
                                                                    attacker.Owner.Send((byte[])packet);
                                                                }
                                                                break;
                                                            }
                                                            else if (spell.ID == 11160)
                                                            {
                                                                if (attacker.DefensiveStanceTime != 0)
                                                                {
                                                                    attacker.DefensiveStanceTime = 0;
                                                                    attacker.DefensiveStanceIncrease = 0;
                                                                    attacker.RemoveFlag2(Network.GamePackets.Update.Flags2.WarriorWalk);
                                                                }
                                                                else
                                                                {
                                                                    if (attacker.ContainsFlag(Update.Flags.Ride))
                                                                    {
                                                                        attacker.RemoveFlag(Update.Flags.Ride);
                                                                    }
                                                                    attacker.AddFlag2(Update.Flags2.WarriorWalk);
                                                                    attacker.DefensiveStanceStamp = Time32.Now;
                                                                    attacker.DefensiveStanceIncrease = spell.PowerPercent;
                                                                    attacker.DefensiveStanceTime = (byte)spell.Duration;

                                                                    if (attacker.EntityFlag == EntityFlag.Player)
                                                                    {
                                                                        attacker.Owner.Send(ServerBase.Constants.DefensiveStance(spell.Percent, (int)spell.Duration));

                                                                        attacker.Owner.SendScreen(suse, true);
                                                                    }
                                                                }
                                                                break;
                                                            }
                                                            else if (spell.ID == 30000)
                                                            {

                                                                if (attacked.ContainsFlag2(Update.Flags2.AzureShield))
                                                                {
                                                                    return;
                                                                }
                                                                //attack.AttackType = Attack.kimo2;
                                                                attacked.ShieldTime = 0;
                                                                attacked.ShieldStamp = Time32.Now;
                                                                attacked.MagicShieldStamp = Time32.Now;
                                                                attacked.MagicShieldTime = 0;
                                                                //Console.WriteLine("The Dodge is :" + attacked.Dodge.ToString());
                                                                attacked.AddFlag2(Update.Flags2.AzureShield);
                                                                attacked.MagicShieldStamp = Time32.Now;
                                                                attacked.AzureDamage = 12000;
                                                                //Console.WriteLine("AzureShiled granted " + 12000 + "  The Dodge is :" + attacked.Dodge.ToString());
                                                                attacked.MagicShieldIncrease = spell.PowerPercent;
                                                                attacked.MagicShieldTime = 60;
                                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                                {
                                                                    attacked.Owner.Send(ServerBase.Constants.AzureShield(12000, 30));

                                                                    SyncPacket packet4 = new SyncPacket
                                                                    {
                                                                        Identifier = attacked.UID,
                                                                        Count = 2,
                                                                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                                                                        StatusFlag1 = (ulong)attacked.StatusFlag,
                                                                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                                                                        Unknown1 = 0x31,
                                                                        StatusFlagOffset = 0x5d,
                                                                        Time = (uint)60,
                                                                        Value = (uint)attacked.AzureDamage,
                                                                        Level = spell.Level
                                                                    };
                                                                    attacked.Owner.Send((byte[])packet4);
                                                                }
                                                            }
                                                            if (spell.ID == 10405 && attacked.Dead)
                                                            {
                                                                if ((attacked.BattlePower - attacker.BattlePower) >= 10)
                                                                    return;
                                                                attacked.AddFlag(Update.Flags.Dead);//Flag them as dead... should not be needed. This is no movement
                                                                attacked.ShackleStamp = Time32.Now;//Set stamp so source can remove the flag after X seconds
                                                                attacked.ShackleTime = (short)(30 + 15 * spell.Level);//double checking here. Could be db has this wrong.
                                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                                {
                                                                    attacked.Owner.Send(ServerBase.Constants.Shackled(attacked.ShackleTime));

                                                                    attacked.AddFlag2(Update.Flags2.SoulShackle);//Give them shackeld effect   

                                                                    SyncPacket packet3 = new SyncPacket
                                                                    {
                                                                        Identifier = attacked.UID,
                                                                        Count = 2,
                                                                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                                                                        StatusFlag1 = (ulong)attacked.StatusFlag,
                                                                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                                                                        Unknown1 = 0x36,
                                                                        StatusFlagOffset = 0x6f,
                                                                        Time = (uint)spell.Duration,
                                                                        Value = 10,
                                                                        Level = spell.Level
                                                                    };
                                                                    attacked.Owner.Send((byte[])packet3);

                                                                }
                                                            }
                                                        }
                                                        attacker.Owner.IncreaseSpellExperience(Experience, spellID);
                                                        if (attacked.EntityFlag == EntityFlag.Player)
                                                            attacked.Owner.SendScreen(suse, true);
                                                        else
                                                            attacked.MonsterInfo.SendScreen(suse);

                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Percent
                                    case 3050:
                                        // case 11230:
                                        {
                                            //Console.WriteLine("1");
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                if (attackedsob != null)
                                                {
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Distance)
                                                    {
                                                        SpellUse suse = new SpellUse(true);
                                                        suse.Attacker = attacker.UID;
                                                        suse.SpellID = spell.ID;
                                                        suse.SpellLevel = spell.Level;
                                                        suse.X = X;
                                                        suse.Y = Y;

                                                        if (CanAttack(attacker, attackedsob, spell))
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);
                                                            uint damage = Game.Attacking.Calculate.Percent(attackedsob, spell.PowerPercent);

                                                            attackedsob.Hitpoints -= damage;

                                                            suse.Targets.Add(attackedsob.UID, damage);

                                                            attacker.Owner.SendScreen(suse, true);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Distance)
                                                    {
                                                        SpellUse suse = new SpellUse(true);
                                                        suse.Attacker = attacker.UID;
                                                        suse.SpellID = spell.ID;
                                                        suse.SpellLevel = spell.Level;
                                                        suse.X = X;
                                                        suse.Y = Y;

                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);
                                                            uint damage = Game.Attacking.Calculate.Percent(attacked, spell.PowerPercent);

                                                            if (attacker.Owner.QualifierGroup != null)
                                                                attacker.Owner.QualifierGroup.UpdateDamage(attacker.Owner, damage);

                                                            attacked.Hitpoints -= damage;

                                                            suse.Targets.Add(attacked.UID, damage);

                                                            if (attacked.EntityFlag == EntityFlag.Player)
                                                                attacked.Owner.SendScreen(suse, true);
                                                            else
                                                                attacked.MonsterInfo.SendScreen(suse);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region ExtraXP
                                    case 1040:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                if (attacker.Owner.Team != null)
                                                {
                                                    PrepareSpell(spell, attacker.Owner);
                                                    foreach (Client.GameState teammate in attacker.Owner.Team.Teammates)
                                                    {
                                                        if (teammate.Entity.UID != attacker.UID)
                                                        {
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, teammate.Entity.X, teammate.Entity.Y) <= spell.Distance)
                                                            {
                                                                teammate.XPCount += 20;
                                                                Update update = new Update(true);
                                                                update.UID = teammate.Entity.UID;
                                                                update.Append(Update.XPCircle, teammate.XPCount);
                                                                update.Send(teammate);
                                                                suse.Targets.Add(teammate.Entity.UID, 20);

                                                                if (spell.NextSpellID != 0)
                                                                {
                                                                    attack.Damage = spell.NextSpellID;
                                                                    attacker.AttackPacket = attack;
                                                                }
                                                                else
                                                                {
                                                                    attacker.AttackPacket = null;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                    attacked.Owner.SendScreen(suse, true);
                                                else
                                                    attacked.MonsterInfo.SendScreen(suse);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region WeaponSpells
                                    #region Circle
                                    case 5010:
                                    case 7020:

                                        //case 11110:
                                        //case 10490:
                                        {

                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                if (suse.SpellID != 10415)
                                                {
                                                    suse.X = X;
                                                    suse.Y = Y;
                                                }
                                                else
                                                {
                                                    suse.X = 6;
                                                }

                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= attacker.AttackRange + 1)
                                                {
                                                    foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                    {
                                                        if (_obj == null)
                                                            continue;
                                                        if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                        {
                                                            attacked = _obj as Entity;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Range)
                                                            {
                                                                if (attacked.ContainsFlag(Update.Flags.Fly))
                                                                    return;
                                                                if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                {
                                                                    PrepareSpell(spell, attacker.Owner);

                                                                    //attack.Effect1 = Attack.AttackEffects1.None;
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                                    //suse.Effect1 = attack.Effect1;

                                                                    ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                    suse.Targets.Add(attacked.UID, damage);
                                                                }
                                                            }
                                                        }
                                                        else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                        {
                                                            attackedsob = _obj as SobNpcSpawn;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Range)
                                                            {
                                                                if (CanAttack(attacker, attackedsob, spell))
                                                                {
                                                                    PrepareSpell(spell, attacker.Owner);
                                                                    // attack.Effect1 = Attack.AttackEffects1.None;
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                    damage = (uint)(damage * spell.PowerPercent);
                                                                    ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                                    suse.Targets.Add(attackedsob.UID, damage);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }

                                            break;
                                        }
                                    #endregion
                                    #region Single target
                                    case 10490:
                                    case 11140:
                                        //case 11230:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;

                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= attacker.AttackRange + 1)
                                                {
                                                    if (attackedsob != null)
                                                    {
                                                        if (CanAttack(attacker, attackedsob, spell))
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);
                                                            suse.MakeConst();
                                                            for (uint c = 0; c < 3; c++)
                                                            {
                                                                uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                if (damage > attackedsob.Hitpoints)
                                                                    damage = attackedsob.Hitpoints;

                                                                ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                                suse.Targets.Add(attackedsob.UID + c, damage);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);
                                                            suse.MakeConst();
                                                            for (uint c = 0; c < 3; c++)
                                                            {
                                                                uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                                if (damage > attacked.Hitpoints)
                                                                    damage = attacked.Hitpoints;
                                                                ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                suse.Targets.Add(attacked.UID + c, damage);
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    case 1290:
                                    case 5030:
                                    case 5040:
                                    case 7000:
                                    case 7010:
                                    case 7030:
                                    case 7040:
                                    case 11230:
                                        //case 10381:

                                        //case 10490:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;

                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= attacker.AttackRange + 1)
                                                {
                                                    if (attackedsob != null)
                                                    {
                                                        if (CanAttack(attacker, attackedsob, spell))
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);
                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                            damage = (uint)(damage * spell.PowerPercent);
                                                            ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                            suse.Targets.Add(attackedsob.UID, damage);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);

                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                            suse.Effect1 = attack.Effect1;

                                                            ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                            suse.Targets.Add(attacked.UID, damage);
                                                        }
                                                    }
                                                    attacker.AttackPacket = null;
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            attacker.AttackPacket = null;
                                            break;
                                        }
                                    #endregion
                                    #region Sector
                                    case 1250:
                                    case 5050:
                                    case 5020:
                                    case 1300:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                Sector sector = new Sector(attacker.X, attacker.Y, X, Y);
                                                sector.Arrange(spell.Sector, spell.Range);
                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= spell.Distance + 1)
                                                {
                                                    foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                    {
                                                        if (_obj == null)
                                                            continue;
                                                        if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                        {
                                                            attacked = _obj as Entity;

                                                            if (sector.Inside(attacked.X, attacked.Y))
                                                            {
                                                                if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                {
                                                                    attack.Effect1 = Attack.AttackEffects1.None;
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                                    suse.Effect1 = attack.Effect1;

                                                                    ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                    suse.Targets.Add(attacked.UID, damage);
                                                                }
                                                            }
                                                        }
                                                        else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                        {
                                                            attackedsob = _obj as SobNpcSpawn;

                                                            if (sector.Inside(attackedsob.X, attackedsob.Y))
                                                            {
                                                                if (CanAttack(attacker, attackedsob, spell))
                                                                {
                                                                    attack.Effect1 = Attack.AttackEffects1.None;
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                    damage = (uint)(damage * spell.PowerPercent);
                                                                    ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                                    suse.Targets.Add(attackedsob.UID, damage);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #endregion
                                    #region Fly
                                    case 8002:
                                    case 8003:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                if (attacker.MapID == 1950)
                                                    return;
                                                PrepareSpell(spell, attacker.Owner);
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                attacked.FlyStamp = Time32.Now;
                                                attacked.FlyTime = (byte)spell.Duration;

                                                suse.Targets.Add(attacker.UID, attacker.FlyTime);

                                                attacker.AddFlag(Update.Flags.Fly);
                                                attacker.RemoveFlag(Update.Flags.Ride);
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Pirate Spells
                                    #region Xp Skills
                                    case 10309: //Cannon Barrage
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                if (!attacker.ContainsFlag2(Update.Flags2.ChainBoltActive))
                                                {
                                                    //attacker.RemoveFlag(Update.Flags.XPList);
                                                    attacker.ChainBoltStamp = Time32.Now;
                                                    attacker.ChainBoltTime = 30;
                                                    attacker.AddFlag2(Update.Flags2.ChainBoltActive);
                                                }

                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= spell.Range)
                                                {
                                                    for (int c = 0; c < attacker.Owner.Screen.Objects.Count(); c++)
                                                    {
                                                        //For a multi threaded application, while we go through the collection
                                                        //the collection might change. We will make sure that we wont go off  
                                                        //the limits with a check.
                                                        if (c >= attacker.Owner.Screen.Objects.Count())
                                                            break;
                                                        Interfaces.IMapObject _obj = attacker.Owner.Screen.Objects[c];
                                                        if (_obj == null)
                                                            continue;
                                                        if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                        {
                                                            attacked = _obj as Entity;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Range)
                                                            {
                                                                if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                {
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, ref attack);
                                                                    if (spell.Power > 0)
                                                                        damage = Game.Attacking.Calculate.Magic(attacker, attacked, spell, ref attack);
                                                                    if (spell.ID == 8030)
                                                                        damage = Game.Attacking.Calculate.Ranged(attacker, attacked, ref attack);
                                                                    ReceiveAttack(attacker, attacked, attack, damage, spell);
                                                                    suse.Targets.Add(attacked.UID, damage);
                                                                    //attacked.AddFlag(Update.Effects.CannonBarrage);
                                                                }
                                                            }
                                                        }
                                                        else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                        {
                                                            attackedsob = _obj as SobNpcSpawn;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Range)
                                                            {
                                                                if (CanAttack(attacker, attackedsob, spell))
                                                                {
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                    if (spell.Power > 0)
                                                                        damage = Game.Attacking.Calculate.Magic(attacker, attackedsob, spell, ref attack);
                                                                    if (spell.ID == 8030)
                                                                        damage = Game.Attacking.Calculate.Ranged(attacker, attackedsob, ref attack);
                                                                    ReceiveAttack(attacker, attackedsob, attack, damage, spell);
                                                                    suse.Targets.Add(attackedsob.UID, damage);
                                                                    //attacked.AddFlag(Update.Effects.CannonBarrage);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    case 11050: //Cannon Barrage
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                if (!attacker.ContainsFlag2(Update.Flags2.CannonBraga))
                                                {
                                                    // attacker.RemoveFlag(Update.Flags.XPList);
                                                    attacker.CannonBarageStamp = Time32.Now;
                                                    attacker.Cannonbarage = 30;
                                                    attacker.AddFlag2(Update.Flags2.CannonBraga);
                                                }

                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= 10)
                                                {
                                                    for (int c = 0; c < attacker.Owner.Screen.Objects.Count(); c++)
                                                    {
                                                        //For a multi threaded application, while we go through the collection
                                                        //the collection might change. We will make sure that we wont go off  
                                                        //the limits with a check.
                                                        if (c >= attacker.Owner.Screen.Objects.Count())
                                                            break;
                                                        Interfaces.IMapObject _obj = attacker.Owner.Screen.Objects[c];
                                                        if (_obj == null)
                                                            continue;
                                                        if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                        {
                                                            attacked = _obj as Entity;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= 10)
                                                            {
                                                                if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                {
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, ref attack);
                                                                    if (spell.Power > 0)
                                                                        damage = Game.Attacking.Calculate.Magic(attacker, attacked, spell, ref attack);
                                                                    if (spell.ID == 8030)
                                                                        damage = Game.Attacking.Calculate.Ranged(attacker, attacked, ref attack);
                                                                    ReceiveAttack(attacker, attacked, attack, damage, spell);
                                                                    suse.Targets.Add(attacked.UID, damage);
                                                                    //attacked.AddFlag(Update.Effects.CannonBarrage);
                                                                }
                                                            }
                                                        }
                                                        else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                        {
                                                            attackedsob = _obj as SobNpcSpawn;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= 10)
                                                            {
                                                                if (CanAttack(attacker, attackedsob, spell))
                                                                {
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                    if (spell.Power > 0)
                                                                        damage = Game.Attacking.Calculate.Magic(attacker, attackedsob, spell, ref attack);
                                                                    if (spell.ID == 8030)
                                                                        damage = Game.Attacking.Calculate.Ranged(attacker, attackedsob, ref attack);
                                                                    ReceiveAttack(attacker, attackedsob, attack, damage, spell);
                                                                    suse.Targets.Add(attackedsob.UID, damage);
                                                                    //attacked.AddFlag(Update.Effects.CannonBarrage);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    case 11060: // BlackPearl Rage
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                if (!attacker.ContainsFlag2(Update.Flags2.BlackBread))
                                                {
                                                    //attacker.RemoveFlag(Update.Flags.XPList);
                                                    attacker.BlackBeardStamp = Time32.Now;
                                                    attacker.Blackbeard = 30;
                                                    attacker.AddFlag2(Update.Flags2.BlackBread);
                                                }
                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= spell.Range)
                                                {
                                                    for (int c = 0; c < attacker.Owner.Screen.Objects.Count(); c++)
                                                    {
                                                        //For a multi threaded application, while we go through the collection
                                                        //the collection might change. We will make sure that we wont go off  
                                                        //the limits with a check.
                                                        if (c >= attacker.Owner.Screen.Objects.Count())
                                                            break;
                                                        Interfaces.IMapObject _obj = attacker.Owner.Screen.Objects[c];
                                                        if (_obj == null)
                                                            continue;
                                                        if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                        {
                                                            attacked = _obj as Entity;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Range)
                                                            {
                                                                if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                {
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, ref attack);
                                                                    if (spell.Power > 0)
                                                                        damage = Game.Attacking.Calculate.Magic(attacker, attacked, spell, ref attack);
                                                                    if (spell.ID == 8030)
                                                                        damage = Game.Attacking.Calculate.Ranged(attacker, attacked, ref attack);
                                                                    ReceiveAttack(attacker, attacked, attack, damage, spell);
                                                                    suse.Targets.Add(attacked.UID, damage);
                                                                }
                                                            }
                                                        }
                                                        else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                        {
                                                            attackedsob = _obj as SobNpcSpawn;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Range)
                                                            {
                                                                if (CanAttack(attacker, attackedsob, spell))
                                                                {
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                    if (spell.Power > 0)
                                                                        damage = Game.Attacking.Calculate.Magic(attacker, attackedsob, spell, ref attack);
                                                                    if (spell.ID == 8030)
                                                                        damage = Game.Attacking.Calculate.Ranged(attacker, attackedsob, ref attack);
                                                                    ReceiveAttack(attacker, attackedsob, attack, damage, spell);
                                                                    suse.Targets.Add(attackedsob.UID, damage);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    #endregion

                                    #region Auto On Skills
                                    case 11130: //Adrenaline Rush
                                        //case 11130: //BlackSpot
                                        {
                                            spell.RemoveCooldown(0x2b16);
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PhoenixProject.Network.GamePackets.Quest.Target target5 = new PhoenixProject.Network.GamePackets.Quest.Target
                                                {
                                                    Obj = attacker,
                                                    Damage = 0x2b16,
                                                    DamageType = HitType.NoDamage,
                                                    Hit = true
                                                };
                                                if (spell.CoolDown > 0)
                                                {
                                                    spell.AddCooldown(spell.ID, (int)spell.CoolDown);
                                                }


                                            }
                                            break;
                                        }
                                    #endregion // Doesnt Work // Doest Work



                                    #region Linear Skills
                                    /* case 11110: //Blade Tempest
                                        {
                                            //if (Time32.Now < attacker.WhilrwindKick.AddMilliseconds(1500))
                                            // return;
                                            //attacker.WhilrwindKick = Time32.Now;
                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= 5)
                                            {

                                                if (CanUseSpell(spell, attacker.Owner))
                                                {
                                                    PrepareSpell(spell, attacker.Owner);

                                                    SpellUse suse = new SpellUse(true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = spell.ID;
                                                    suse.SpellLevel = spell.Level;
                                                    suse.X = X;
                                                    suse.Y = Y;

                                                    Game.Map Map = ServerBase.Kernel.Maps[attacker.MapID];
                                                    if (Map.SelectCoordonates(ref X, ref Y))
                                                    {

                                                        //attacker.X = X;
                                                        // attacker.Y = Y;
                                                        if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= 5)
                                                        {
                                                            for (int c = 0; c < attacker.Owner.Screen.Objects.Count(); c++)
                                                            {
                                                                //For a multi threaded application, while we go through the collection
                                                                //the collection might change. We will make sure that we wont go off  
                                                                //the limits with a check.
                                                                if (c >= attacker.Owner.Screen.Objects.Count())
                                                                    break;
                                                                Interfaces.IMapObject _obj = attacker.Owner.Screen.Objects[c];
                                                                if (_obj == null)
                                                                    continue;
                                                                if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                                {
                                                                    attacked = _obj as Entity;
                                                                    if (ServerBase.Kernel.GetDistance(X, Y, attacked.X, attacked.Y) <= 1)
                                                                    {

                                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Ranged))
                                                                        {
                                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, ref attack);

                                                                            ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                            suse.Targets.Add(attacked.UID, damage);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        attacker.AttackPacket = null;
                                                    }
                                                    attacker.Owner.SendScreen(suse, true);
                                                }
                                            }
                                            break;
                                        }*/
                                    #endregion

                                    #region Single damage Skill
                                    case 11120:
                                        {
                                            if (attacked != null)
                                            {
                                                if (attacked.EntityFlag == EntityFlag.Player || attacked.EntityFlag == EntityFlag.Monster)
                                                {
                                                    if (!attacked.Dead)
                                                    {
                                                        BlackSpot spot = new BlackSpot
                                                        {
                                                            Remove = 0,
                                                            Identifier = attacked.UID
                                                        };
                                                        attacker.Owner.Send((byte[])spot);

                                                        attacked.BlackSpotTime = Time32.Now;
                                                        attacked.BlackSpotTime2 = 12;
                                                        attacked.BlackSpots = true;
                                                        attacker.Owner.IncreaseSpellExperience(80, 11120);
                                                    }
                                                }

                                            }
                                            break;
                                        }
                                    case 11030: // Eagle Eye

                                        if (CanUseSpell(spell, attacker.Owner))
                                        {
                                            PrepareSpell(spell, attacker.Owner);



                                            SpellUse suse = new SpellUse(true);
                                            suse.Attacker = attacker.UID;
                                            suse.SpellID = spell.ID;
                                            suse.SpellLevel = spell.Level;
                                            suse.X = X;
                                            suse.Y = Y;
                                            suse.Effect1 = 0;
                                            bool kimo = false;
                                            //suse.Targets.Add(attacker.UID, spell.Power);
                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= spell.Distance)
                                            {
                                                if (attackedsob != null)
                                                {
                                                    if (CanAttack(attacker, attackedsob, spell))
                                                    {

                                                        PrepareSpell(spell, attacker.Owner);
                                                        uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                        damage = (uint)(damage * spell.PowerPercent * 30);
                                                        ReceiveAttack(attacker, attackedsob, attack, damage, spell);
                                                        suse.Targets.Add(attackedsob.UID, damage);
                                                    }
                                                }
                                                else
                                                {
                                                    if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                    {
                                                        kimo = attacked.BlackSpots;
                                                        PrepareSpell(spell, attacker.Owner);
                                                        uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                        ReceiveAttack(attacker, attacked, attack, damage, spell);
                                                        suse.Targets.Add(attacked.UID, damage);

                                                    }
                                                }
                                            }

                                            if (attacker.Owner.Spells.ContainsKey(11130))
                                            {
                                                if (kimo)
                                                {
                                                    suse.SpellID = 11060;
                                                    attacker.Owner.SendScreen(suse, true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = 11130;
                                                    suse.SpellLevel = attacker.Owner.Spells[11130].Level;
                                                    suse.X = attacker.X;
                                                    suse.Y = attacker.X;

                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                    suse.Targets.Add(attacked.UID, damage);
                                                    attacker.Owner.SendScreen(suse, true);
                                                    attacker.Owner.IncreaseSpellExperience(80, 11130);
                                                    break;

                                                }
                                            }
                                            if (attacker.Owner.Spells.ContainsKey(11130))
                                            {
                                                if (ServerBase.Kernel.Rate(5))
                                                {
                                                    ////spell.Duration = 0;
                                                    attacker.Update(_String.Effect, "RapidReplace", true);
                                                    suse.SpellID = 11060;
                                                    attacker.Owner.SendScreen(suse, true);

                                                }
                                                else
                                                {
                                                    //// spell.Duration = 0;
                                                    //attacker.Update(_String.Effect, "RapidReplace", true); 
                                                    attacker.Owner.SendScreen(suse, true);
                                                }
                                            }
                                            else
                                            {
                                                // spell.Duration = 0;
                                                // attacker.Update(_String.Effect, "RapidReplace", true); 
                                                attacker.Owner.SendScreen(suse, true);
                                            }


                                        }
                                        break;
                                    #endregion

                                    #region Region Damage Skills
                                    case 11100: //kracken revenge
                                        {
                                            //Console.WriteLine("0");
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                //Console.WriteLine("1");
                                                int counts = 0;
                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= 5)
                                                {
                                                    for (int c = 0; c < attacker.Owner.Screen.Objects.Count(); c++)
                                                    {
                                                        // Console.WriteLine("2");
                                                        if (counts < 5)
                                                        {
                                                            //Console.WriteLine("3");
                                                            //For a multi threaded application, while we go through the collection
                                                            //the collection might change. We will make sure that we wont go off  
                                                            //the limits with a check.
                                                            if (c >= attacker.Owner.Screen.Objects.Count())
                                                                break;
                                                            Interfaces.IMapObject _obj = attacker.Owner.Screen.Objects[c];
                                                            if (_obj == null)
                                                                continue;
                                                            //Console.WriteLine("4");
                                                            if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                            {
                                                                attacked = _obj as Entity;
                                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= 5)
                                                                {
                                                                    // Console.WriteLine("5");
                                                                    if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                    {
                                                                        if (attacked.EntityFlag == EntityFlag.Monster)
                                                                        {

                                                                            BlackSpot spot = new BlackSpot
                                                                            {
                                                                                Remove = 0,
                                                                                Identifier = attacked.UID
                                                                            };
                                                                            attacker.Owner.Send((byte[])spot);

                                                                            attacked.BlackSpotTime = Time32.Now;
                                                                            attacked.BlackSpotTime2 = (byte)spell.Duration;
                                                                            attacked.BlackSpots = true;
                                                                            counts += 1;
                                                                        }
                                                                        else
                                                                        {
                                                                            if (!ServerBase.Constants.BlackSpotNo.Contains(attacker.MapID))
                                                                            {

                                                                                BlackSpot spot = new BlackSpot
                                                                                {
                                                                                    Remove = 0,
                                                                                    Identifier = attacked.UID
                                                                                };
                                                                                attacker.Owner.Send((byte[])spot);

                                                                                attacked.BlackSpotTime = Time32.Now;
                                                                                attacked.BlackSpotTime2 = (byte)spell.Duration;
                                                                                attacked.BlackSpots = true;
                                                                                counts += 1;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                                attacker.Owner.IncreaseSpellExperience(80, spellID);
                                            }
                                            break;
                                        }
                                    case 11040: // Scurvey Bomb
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;

                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= spell.Range)
                                                {
                                                    for (int c = 0; c < attacker.Owner.Screen.Objects.Count(); c++)
                                                    {
                                                        //For a multi threaded application, while we go through the collection
                                                        //the collection might change. We will make sure that we wont go off  
                                                        //the limits with a check.
                                                        if (c >= attacker.Owner.Screen.Objects.Count())
                                                            break;
                                                        Interfaces.IMapObject _obj = attacker.Owner.Screen.Objects[c];
                                                        if (_obj == null)
                                                            continue;
                                                        if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                        {
                                                            attacked = _obj as Entity;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Range)
                                                            {
                                                                if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                {
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, ref attack);
                                                                    if (spell.Power > 0)
                                                                        damage = Game.Attacking.Calculate.Magic(attacker, attacked, spell, ref attack);

                                                                    ReceiveAttack(attacker, attacked, attack, damage, spell);
                                                                    suse.Targets.Add(attacked.UID, damage);
                                                                    attacker.Owner.SendScreen(suse, true);
                                                                }
                                                            }
                                                        }
                                                        else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                        {
                                                            attackedsob = _obj as SobNpcSpawn;
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Range)
                                                            {
                                                                if (CanAttack(attacker, attackedsob, spell))
                                                                {
                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                    if (spell.Power > 0)
                                                                        damage = Game.Attacking.Calculate.Magic(attacker, attackedsob, spell, ref attack);

                                                                    ReceiveAttack(attacker, attackedsob, attack, damage, spell);
                                                                    suse.Targets.Add(attackedsob.UID, damage);
                                                                    attacker.Owner.SendScreen(suse, true);
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }

                                            }
                                            break;
                                        }
                                    case 11070: // Gale Bomb
                                        {
                                            if (Time32.Now >= attacker.MagikAttackTimeAtaque.AddMilliseconds(200))
                                            {
                                                attacker.MagikAttackTimeAtaque = Time32.Now;
                                                if (CanUseSpell(spell, attacker.Owner))
                                                {
                                                    PrepareSpell(spell, attacker.Owner);

                                                    SpellUse suse = new SpellUse(true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = spell.ID;
                                                    suse.SpellLevel = spell.Level;
                                                    suse.X = attack.X;
                                                    suse.Y = attack.Y;

                                                    attack.AttackType = 0x1b;
                                                    byte MaxDistance = (byte)spell.UnknownPush;
                                                    int counts = 0;
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= attacker.AttackRange + 3)
                                                    {
                                                        foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                        {
                                                            if (counts < 3)
                                                            {
                                                                if (_obj == null)
                                                                    continue;

                                                                if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                                {
                                                                    attacked = _obj as Entity;
                                                                    if (ServerBase.Kernel.GetDistance(X, Y, attacked.X, attacked.Y) <= 5)
                                                                    {
                                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                        {

                                                                            if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                            {
                                                                                if (Misc.Distance(_obj.X, _obj.Y, (ushort)attack.X, (ushort)attack.Y) < (int)MaxDistance)
                                                                                {

                                                                                    var direction = ServerBase.Kernel.GetAngle(attacker.X, attacker.Y, attacked.X, attacked.Y);
                                                                                    attack = new Attack(true);
                                                                                    byte angle = (byte)(attack.Attacked % 8);
                                                                                    attack.Effect1 = Attack.AttackEffects1.None;
                                                                                    uint damage = Calculate.Melee(attacker, attacked, ref attack);
                                                                                    attack.AttackType = 0x1b;
                                                                                    attack.X = attacker.X;
                                                                                    attack.Y = attacker.Y;
                                                                                    attack.PushBack = angle;
                                                                                    attack.Attacker = attacker.UID;
                                                                                    attack.Attacked = attacked.UID;
                                                                                    attack.Damage = damage;
                                                                                    // attack.ToArray()[27] = (byte)direction;
                                                                                    // attacked.Move(direction);
                                                                                    // attacker.Move(direction);

                                                                                    ReceiveAttack(attacker, attacked, attack, damage, spell);
                                                                                    counts++;
                                                                                    // attacker.Owner.SendScreen(attack, true);
                                                                                    attacker.Owner.SendScreen(suse, true);
                                                                                    //attacker.X = attacker.X;

                                                                                }
                                                                                else
                                                                                    break;



                                                                            }
                                                                            else
                                                                            {
                                                                                suse.Targets.Add(attacked.UID, 0);
                                                                                suse.Targets[attacked.UID].Hit = false;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                                {
                                                                    attackedsob = _obj as SobNpcSpawn;
                                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= 5)
                                                                    {
                                                                        if (CanAttack(attacker, attackedsob, spell))
                                                                        {
                                                                            if (ServerBase.Kernel.Rate(100))
                                                                            {
                                                                                if (CanAttack(attacker, attackedsob, spell))
                                                                                {
                                                                                    uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                                                    ReceiveAttack(attacker, attackedsob, attack, damage, spell);
                                                                                    counts++;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                suse.Targets.Add(attacked.UID, 0);
                                                                                suse.Targets[attacked.UID].Hit = false;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        attacker.AttackPacket = null;
                                                    }
                                                    attacker.Owner.SendScreen(suse, true);
                                                }
                                            }
                                            break;
                                        }
                                    #endregion

                                    #endregion
                                    #region Ninja Spells
                                    case 6010://Vortex
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;

                                                attacker.AddFlag(Update.Flags.ShurikenVortex);
                                                attacker.RemoveFlag(Update.Flags.Ride);
                                                attacker.ShurikenVortexStamp = Time32.Now;
                                                attacker.ShurikenVortexTime = 20;

                                                attacker.Owner.SendScreen(suse, true);

                                                attacker.VortexPacket = new Attack(true);
                                                attacker.VortexPacket.Decoded = true;
                                                attacker.VortexPacket.Damage = 6012;
                                                attacker.VortexPacket.AttackType = Attack.Magic;
                                                attacker.VortexPacket.Attacker = attacker.UID;
                                            }
                                            break;
                                        }
                                    case 6012://VortexRespone
                                        {
                                            if (!attacker.ContainsFlag(Update.Flags.ShurikenVortex))
                                            {
                                                attacker.AttackPacket = null;
                                                break;
                                            }
                                            SpellUse suse = new SpellUse(true);
                                            suse.Attacker = attacker.UID;
                                            suse.SpellID = spell.ID;
                                            suse.SpellLevel = spell.Level;
                                            suse.X = attacker.X;
                                            suse.Y = attacker.Y;
                                            foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                            {
                                                if (_obj == null)
                                                    continue;
                                                if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                {
                                                    attacked = _obj as Entity;
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Range)
                                                    {
                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                        {
                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);

                                                            suse.Effect1 = attack.Effect1;

                                                            ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                            suse.Targets.Add(attacked.UID, damage);
                                                        }
                                                    }
                                                }
                                                else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                {
                                                    attackedsob = _obj as SobNpcSpawn;
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attackedsob.X, attackedsob.Y) <= spell.Range)
                                                    {
                                                        if (CanAttack(attacker, attackedsob, spell))
                                                        {
                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                            suse.Effect1 = attack.Effect1;
                                                            ReceiveAttack(attacker, attackedsob, attack, damage, spell);
                                                            suse.Targets.Add(attackedsob.UID, damage);
                                                        }
                                                    }
                                                }
                                            }
                                            attacker.Owner.SendScreen(suse, true);
                                            break;
                                        }
                                    case 6001:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= spell.Distance)
                                                {
                                                    foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                    {
                                                        if (_obj.MapObjType == MapObjectType.Player)
                                                        {
                                                            attacked = _obj as Entity;
                                                            if (ServerBase.Kernel.GetDistance(X, Y, attacked.X, attacked.Y) <= spell.Range)
                                                            {
                                                                if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                {
                                                                    int potDifference = attacker.BattlePower - attacked.BattlePower;

                                                                    int rate = spell.Percent + potDifference - 20;

                                                                    if (ServerBase.Kernel.Rate(rate))
                                                                    {
                                                                        attacked.ToxicFogStamp = Time32.Now;
                                                                        attacked.ToxicFogLeft = 20;
                                                                        attacked.ToxicFogPercent = spell.PowerPercent;
                                                                        suse.Targets.Add(attacked.UID, 1);
                                                                    }
                                                                    if (attacker.BattlePower - attacked.BattlePower > 0)
                                                                    {
                                                                        attacked.ToxicFogStamp = Time32.Now;
                                                                        attacked.ToxicFogLeft = 20;
                                                                        attacked.ToxicFogPercent = spell.PowerPercent;
                                                                        suse.Targets.Add(attacked.UID, 1);
                                                                    }
                                                                    else
                                                                    {
                                                                        suse.Targets.Add(attacked.UID, 0);
                                                                        suse.Targets[attacked.UID].Hit = false;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (_obj.MapObjType == MapObjectType.Monster)
                                                            {
                                                                attacked = _obj as Entity;
                                                                if (!ServerBase.Constants.NoFog.Contains(attacked.Name))
                                                                {

                                                                    if (ServerBase.Kernel.GetDistance(X, Y, attacked.X, attacked.Y) <= spell.Range)
                                                                    {
                                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                        {
                                                                            int potDifference = attacker.BattlePower - attacked.BattlePower;

                                                                            int rate = spell.Percent + potDifference - 20;

                                                                            if (ServerBase.Kernel.Rate(rate))
                                                                            {
                                                                                attacked.ToxicFogStamp = Time32.Now;
                                                                                attacked.ToxicFogLeft = 20;
                                                                                attacked.ToxicFogPercent = spell.PowerPercent;
                                                                                suse.Targets.Add(attacked.UID, 1);
                                                                            }
                                                                            if (attacker.BattlePower - attacked.BattlePower > 0)
                                                                            {
                                                                                attacked.ToxicFogStamp = Time32.Now;
                                                                                attacked.ToxicFogLeft = 20;
                                                                                attacked.ToxicFogPercent = spell.PowerPercent;
                                                                                suse.Targets.Add(attacked.UID, 1);
                                                                            }
                                                                            else
                                                                            {
                                                                                suse.Targets.Add(attacked.UID, 0);
                                                                                suse.Targets[attacked.UID].Hit = false;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                                attacker.Owner.IncreaseSpellExperience(80, spellID);
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    case 6000:
                                    case 10381:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                ushort Xx, Yx;
                                                if (attacked != null)
                                                {
                                                    Xx = attacked.X;
                                                    Yx = attacked.Y;
                                                }
                                                else
                                                {
                                                    Xx = attackedsob.X;
                                                    Yx = attackedsob.Y;
                                                }
                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, Xx, Yx) <= spell.Range)
                                                {
                                                    if (attackedsob == null)
                                                        if (attacked.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                            return;
                                                    //if (attacked.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                    //  return;
                                                    if (attacker.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                                        return;
                                                    PrepareSpell(spell, attacker.Owner);

                                                    SpellUse suse = new SpellUse(true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = spell.ID;
                                                    suse.SpellLevel = spell.Level;
                                                    suse.X = X;
                                                    suse.Y = Y;

                                                    bool send = false;

                                                    if (attackedsob == null)
                                                    {
                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                        {
                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);
                                                            suse.Effect1 = attack.Effect1;

                                                            ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                            suse.Targets.Add(attacked.UID, damage);
                                                            send = true;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (CanAttack(attacker, attackedsob, spell))
                                                        {
                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                            uint damage = Game.Attacking.Calculate.Melee(attacker, attackedsob, ref attack);
                                                            damage = (uint)(damage * spell.PowerPercent);
                                                            ReceiveAttack(attacker, attackedsob, attack, damage, spell);
                                                            suse.Effect1 = attack.Effect1;

                                                            suse.Targets.Add(attackedsob.UID, damage);
                                                            send = true;
                                                        }
                                                    }
                                                    if (send)
                                                        attacker.Owner.SendScreen(suse, true);
                                                }
                                                else
                                                {
                                                    attacker.AttackPacket = null;
                                                }
                                            }
                                            break;
                                        }
                                    case 6002:
                                        {
                                            if (attackedsob != null)
                                                return;
                                            if (attacked.EntityFlag == EntityFlag.Monster)
                                                return;
                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Distance)
                                            {
                                                if (CanUseSpell(spell, attacker.Owner))
                                                {
                                                    PrepareSpell(spell, attacker.Owner);

                                                    int potDifference = attacker.BattlePower - attacked.BattlePower;

                                                    int rate = spell.Percent + potDifference;

                                                    SpellUse suse = new SpellUse(true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = spell.ID;
                                                    suse.SpellLevel = spell.Level;
                                                    suse.X = X;
                                                    suse.Y = Y;
                                                    if (CanAttack(attacker, attacked, spell, false))
                                                    {
                                                        suse.Targets.Add(attacked.UID, 0);
                                                        if (ServerBase.Kernel.Rate(rate))
                                                        {
                                                            attacked.NoDrugsStamp = Time32.Now;
                                                            attacked.NoDrugsTime = (short)spell.Duration;
                                                            if (attacked.EntityFlag == EntityFlag.Player)
                                                            {
                                                                attacker.Owner.IncreaseSpellExperience(80, spellID);
                                                                attacked.Owner.Send(ServerBase.Constants.NoDrugs((int)spell.Duration));
                                                            }
                                                        }
                                                        else
                                                        {
                                                            suse.Targets[attacked.UID].Hit = false;
                                                        }

                                                        attacked.Owner.SendScreen(suse, true);
                                                    }
                                                }
                                            }

                                            break;
                                        }
                                    case 6004:
                                        {
                                            if (attackedsob != null)
                                                return;
                                            if (attacked.EntityFlag == EntityFlag.Monster)
                                                return;
                                            if (!attacked.ContainsFlag(Update.Flags.Fly))
                                                return;
                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Distance)
                                            {
                                                if (CanUseSpell(spell, attacker.Owner))
                                                {
                                                    PrepareSpell(spell, attacker.Owner);

                                                    int potDifference = attacker.BattlePower - attacked.BattlePower;

                                                    int rate = spell.Percent + potDifference;

                                                    SpellUse suse = new SpellUse(true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = spell.ID;
                                                    suse.SpellLevel = spell.Level;
                                                    suse.X = X;
                                                    suse.Y = Y;
                                                    if (CanAttack(attacker, attacked, spell, false))
                                                    {
                                                        uint dmg = Calculate.Percent(attacked, 0.1F);
                                                        suse.Targets.Add(attacked.UID, dmg);

                                                        if (ServerBase.Kernel.Rate(rate))
                                                        {
                                                            attacked.Hitpoints -= dmg;
                                                            attacked.RemoveFlag(Update.Flags.Fly);
                                                        }
                                                        else
                                                        {
                                                            suse.Targets[attacked.UID].Hit = false;
                                                        }

                                                        attacked.Owner.SendScreen(suse, true);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Riding

                                    case 7001:
                                        {
                                            if (attacker.ContainsFlag2(Update.Flags2.WarriorWalk))
                                                return;
                                            if (attacker.ContainsFlag(Update.Flags.ShurikenVortex))
                                                return;
                                            if (ServerBase.Constants.steedguard.Contains(attacker.MapID))
                                                return;
                                            if (!attacker.Owner.Equipment.Free(12))
                                            {
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;

                                                if (attacker.ContainsFlag(Update.Flags.Ride))
                                                {
                                                    attacker.RemoveFlag(Update.Flags.Ride);
                                                }
                                                else
                                                {
                                                    if (attacker.Owner.Equipment.TryGetItem((byte)12).Plus < attacker.MapRegion.Lineage)
                                                        break;
                                                    if (attacker.Stamina >= 100 && (attacker.Owner.QualifierGroup == null || attacker.Owner.QualifierGroup != null && !attacker.Owner.QualifierGroup.Inside))
                                                    {
                                                        attacker.AddFlag(Update.Flags.Ride);
                                                        attacker.Stamina -= 100;
                                                        attacker.Vigor = (ushort)attacker.MaxVigor;
                                                        Network.GamePackets.Vigor vigor = new Network.GamePackets.Vigor(true);
                                                        vigor.VigorValue = attacker.Owner.Entity.MaxVigor;
                                                        vigor.Send(attacker.Owner);
                                                    }
                                                }
                                                suse.Targets.Add(attacker.UID, 0);
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    case 7002:
                                        {//Spook
                                            if (attacked.ContainsFlag(Update.Flags.Ride) && attacker.ContainsFlag(Update.Flags.Ride))
                                            {
                                                Interfaces.IConquerItem attackedSteed = null, attackerSteed = null;
                                                if ((attackedSteed = attacked.Owner.Equipment.TryGetItem(ConquerItem.Steed)) != null)
                                                {
                                                    if ((attackerSteed = attacker.Owner.Equipment.TryGetItem(ConquerItem.Steed)) != null)
                                                    {
                                                        SpellUse suse = new SpellUse(true);
                                                        suse.Attacker = attacker.UID;
                                                        suse.SpellID = spell.ID;
                                                        suse.SpellLevel = spell.Level;
                                                        suse.X = X;
                                                        suse.Y = Y;
                                                        suse.Targets.Add(attacked.UID, 0);

                                                        if (attackedSteed.Plus < attackerSteed.Plus)
                                                            attacked.RemoveFlag(Update.Flags.Ride);
                                                        else if (attackedSteed.Plus == attackerSteed.Plus && attackedSteed.PlusProgress <= attackerSteed.PlusProgress)
                                                            attacked.RemoveFlag(Update.Flags.Ride);
                                                        else
                                                            suse.Targets[attacked.UID].Hit = false;
                                                        attacker.Owner.SendScreen(suse, true);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    case 7003:
                                        {//WarCry
                                            SpellUse suse = new SpellUse(true);
                                            suse.Attacker = attacker.UID;
                                            suse.SpellID = spell.ID;
                                            suse.SpellLevel = spell.Level;
                                            suse.X = X;
                                            suse.Y = Y;
                                            Interfaces.IConquerItem attackedSteed = null, attackerSteed = null;
                                            foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                            {
                                                if (_obj == null)
                                                    continue;
                                                if (_obj.MapObjType == MapObjectType.Player && _obj.UID != attacker.UID)
                                                {
                                                    attacked = _obj as Entity;
                                                    if ((attackedSteed = attacked.Owner.Equipment.TryGetItem(ConquerItem.Steed)) != null)
                                                    {
                                                        if ((attackerSteed = attacker.Owner.Equipment.TryGetItem(ConquerItem.Steed)) != null)
                                                        {
                                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= attackedSteed.Plus)
                                                            {
                                                                if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                                {
                                                                    suse.Targets.Add(attacked.UID, 0);
                                                                    if (attackedSteed.Plus < attackerSteed.Plus)
                                                                        attacked.RemoveFlag(Update.Flags.Ride);
                                                                    else if (attackedSteed.Plus == attackerSteed.Plus && attackedSteed.PlusProgress <= attackerSteed.PlusProgress)
                                                                        attacked.RemoveFlag(Update.Flags.Ride);
                                                                    else
                                                                        suse.Targets[attacked.UID].Hit = false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            attacker.Owner.SendScreen(suse, true);
                                            break;
                                        }
                                    #endregion
                                    #region Dash
                                    case 1051:
                                        {
                                            if (attacked != null)
                                            {
                                                if (!attacked.Dead)
                                                {
                                                    var direction = ServerBase.Kernel.GetAngle(attacker.X, attacker.Y, attacked.X, attacked.Y);
                                                    if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                    {
                                                        attack = new Attack(true);
                                                        attack.Effect1 = Attack.AttackEffects1.None;
                                                        uint damage = Calculate.Melee(attacker, attacked, ref attack);
                                                        attack.AttackType = Attack.Dash;
                                                        attack.X = attacked.X;
                                                        attack.Y = attacked.Y;
                                                        attack.Attacker = attacker.UID;
                                                        attack.Attacked = attacked.UID;
                                                        attack.Damage = damage;
                                                        attack.ToArray()[27] = (byte)direction;
                                                        attacked.Move(direction);
                                                        attacker.Move(direction);

                                                        ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                        attacker.Owner.SendScreen(attack, true);
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    #endregion

                                    #region RapidFire
                                    case 8000:
                                        {
                                            if (attackedsob != null)
                                            {
                                                if (CanUseSpell(spell, attacker.Owner))
                                                {
                                                    if (CanAttack(attacker, attackedsob, spell))
                                                    {
                                                        PrepareSpell(spell, attacker.Owner);
                                                        SpellUse suse = new SpellUse(true);
                                                        suse.Attacker = attacker.UID;
                                                        suse.SpellID = spell.ID;
                                                        suse.SpellLevel = spell.Level;
                                                        suse.X = attackedsob.X;
                                                        suse.Y = attackedsob.Y;
                                                        attack.Effect1 = Attack.AttackEffects1.None;
                                                        uint damage = Calculate.Ranged(attacker, attackedsob, ref attack);
                                                        suse.Effect1 = attack.Effect1;
                                                        damage = (uint)(damage * spell.PowerPercent);
                                                        suse.Targets.Add(attackedsob.UID, damage);

                                                        ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                        attacker.Owner.SendScreen(suse, true);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (!attacked.Dead)
                                                {
                                                    if (CanUseSpell(spell, attacker.Owner))
                                                    {
                                                        if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                        {
                                                            PrepareSpell(spell, attacker.Owner);
                                                            SpellUse suse = new SpellUse(true);
                                                            suse.Attacker = attacker.UID;
                                                            suse.SpellID = spell.ID;
                                                            suse.SpellLevel = spell.Level;
                                                            suse.X = attacked.X;
                                                            suse.Y = attacked.Y;
                                                            attack.Effect1 = Attack.AttackEffects1.None;
                                                            uint damage = Calculate.Ranged(attacker, attacked, ref attack);
                                                            damage = (uint)(damage * spell.PowerPercent);
                                                            suse.Targets.Add(attacked.UID, damage);

                                                            ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                            attacker.Owner.SendScreen(suse, true);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region FireOfHell
                                    case 1165:
                                    case 7014:
                                    case 7012:
                                    case 9971:
                                    case 7017:
                                    case 7015:
                                    case 7013:
                                    case 10360:
                                    case 9966:
                                    case 30010:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                Sector sector = new Sector(attacker.X, attacker.Y, X, Y);
                                                sector.Arrange(spell.Sector, spell.Distance);
                                                foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                {
                                                    if (_obj == null)
                                                        continue;
                                                    if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                    {
                                                        attacked = _obj as Entity;

                                                        if (sector.Inside(attacked.X, attacked.Y))
                                                        {
                                                            if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                            {
                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                uint damage = Game.Attacking.Calculate.Magic(attacker, attacked, spell, ref attack);
                                                                suse.Effect1 = attack.Effect1;

                                                                ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                suse.Targets.Add(attacked.UID, damage);
                                                            }
                                                        }
                                                    }
                                                    else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                    {
                                                        attackedsob = _obj as SobNpcSpawn;

                                                        if (sector.Inside(attackedsob.X, attackedsob.Y))
                                                        {
                                                            if (CanAttack(attacker, attackedsob, spell))
                                                            {
                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                uint damage = Game.Attacking.Calculate.Magic(attacker, attackedsob, spell, ref attack);
                                                                suse.Effect1 = attack.Effect1;
                                                                ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                                suse.Targets.Add(attackedsob.UID, damage);
                                                            }
                                                        }
                                                    }
                                                }
                                                attacker.Owner.SendScreen(suse, true);

                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Scatter
                                    case 8001:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                if (attacker.xx == attacker.X && attacker.yy == attacker.Y)
                                                {
                                                    attacker.scatter += 1;
                                                    if (attacker.scatter > 30)
                                                    {
                                                        attacker.Owner.Disconnect();
                                                    }
                                                }
                                                else
                                                {
                                                    attacker.xx = attacker.X;
                                                    attacker.yy = attacker.Y;
                                                    attacker.scatter = 0;
                                                }

                                                Sector sector = new Sector(attacker.X, attacker.Y, X, Y);
                                                sector.Arrange(spell.Sector, spell.Distance);
                                                foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                {
                                                    if (_obj == null)
                                                        continue;
                                                    if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                    {
                                                        attacked = _obj as Entity;

                                                        if (sector.Inside(attacked.X, attacked.Y))
                                                        {
                                                            if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                            {
                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                uint damage = Game.Attacking.Calculate.Ranged(attacker, attacked, spell, ref attack);

                                                                ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                suse.Targets.Add(attacked.UID, damage);
                                                            }
                                                        }
                                                    }
                                                    else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                    {
                                                        attackedsob = _obj as SobNpcSpawn;

                                                        if (sector.Inside(attackedsob.X, attackedsob.Y))
                                                        {
                                                            if (CanAttack(attacker, attackedsob, spell))
                                                            {
                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                uint damage = Game.Attacking.Calculate.Ranged(attacker, attackedsob, ref attack);
                                                                suse.Effect1 = attack.Effect1;
                                                                if (damage == 0)
                                                                    damage = 1;
                                                                damage = Game.Attacking.Calculate.Percent((int)damage, spell.PowerPercent);

                                                                ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                                suse.Targets.Add(attackedsob.UID, damage);
                                                            }
                                                        }
                                                    }
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Intensify
                                    case 9000:
                                        {
                                            attacker.IntensifyStamp = Time32.Now;
                                            attacker.OnIntensify = true;
                                            SpellUse suse = new SpellUse(true);
                                            suse.Attacker = attacker.UID;
                                            suse.SpellID = spell.ID;
                                            suse.SpellLevel = spell.Level;
                                            suse.X = X;
                                            suse.Y = Y;
                                            suse.Targets.Add(attacker.UID, 0);
                                            suse.Send(attacker.Owner);
                                            break;
                                        }
                                    #endregion
                                    #region StarArrow
                                    case 10313:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                Sector sector = new Sector(attacker.X, attacker.Y, X, Y);
                                                sector.Arrange(spell.Sector, spell.Distance);
                                                foreach (Interfaces.IMapObject _obj in attacker.Owner.Screen.Objects)
                                                {
                                                    if (_obj == null)
                                                        continue;
                                                    if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                    {
                                                        attacked = _obj as Entity;

                                                        if (sector.Inside(attacked.X, attacked.Y))
                                                        {
                                                            if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Melee))
                                                            {
                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                uint damage = Game.Attacking.Calculate.Ranged(attacker, attacked, spell, ref attack);

                                                                ReceiveAttack(attacker, attacked, attack, damage, spell);

                                                                suse.Targets.Add(attacked.UID, damage);
                                                            }
                                                        }
                                                    }
                                                    else if (_obj.MapObjType == MapObjectType.SobNpc)
                                                    {
                                                        attackedsob = _obj as SobNpcSpawn;

                                                        if (sector.Inside(attackedsob.X, attackedsob.Y))
                                                        {
                                                            if (CanAttack(attacker, attackedsob, spell))
                                                            {
                                                                attack.Effect1 = Attack.AttackEffects1.None;
                                                                uint damage = Game.Attacking.Calculate.Ranged(attacker, attackedsob, ref attack);
                                                                suse.Effect1 = attack.Effect1;
                                                                if (damage == 0)
                                                                    damage = 1;
                                                                damage = Game.Attacking.Calculate.Percent((int)damage, spell.PowerPercent);

                                                                ReceiveAttack(attacker, attackedsob, attack, damage, spell);

                                                                suse.Targets.Add(attackedsob.UID, damage);
                                                            }
                                                        }
                                                    }
                                                }
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Trasnformations
                                    case 1270:
                                    case 1280:
                                    case 1350:
                                    case 1360:
                                    case 3321:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                if (attacker.MapID == 1036)
                                                    return;
                                                bool wasTransformated = attacker.Transformed;
                                                PrepareSpell(spell, attacker.Owner);

                                                #region Atributes
                                                switch (spell.ID)
                                                {
                                                    case 1350:
                                                        switch (spell.Level)
                                                        {
                                                            case 0:
                                                                {
                                                                    attacker.TransformationMaxAttack = 182;
                                                                    attacker.TransformationMinAttack = 122;
                                                                    attacker.TransformationDefence = 1300;
                                                                    attacker.TransformationMagicDefence = 94;
                                                                    attacker.TransformationDodge = 35;
                                                                    attacker.TransformationTime = 39;
                                                                    attacker.TransformationID = 207;
                                                                    break;
                                                                }
                                                            case 1:
                                                                {
                                                                    attacker.TransformationMaxAttack = 200;
                                                                    attacker.TransformationMinAttack = 134;
                                                                    attacker.TransformationDefence = 1400;
                                                                    attacker.TransformationMagicDefence = 96;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 49;
                                                                    attacker.TransformationID = 207;
                                                                    break;
                                                                }
                                                            case 2:
                                                                {
                                                                    attacker.TransformationMaxAttack = 240;
                                                                    attacker.TransformationMinAttack = 160;
                                                                    attacker.TransformationDefence = 1500;
                                                                    attacker.TransformationMagicDefence = 97;
                                                                    attacker.TransformationDodge = 45;
                                                                    attacker.TransformationTime = 59;
                                                                    attacker.TransformationID = 207;
                                                                    break;
                                                                }
                                                            case 3:
                                                                {
                                                                    attacker.TransformationMaxAttack = 258;
                                                                    attacker.TransformationMinAttack = 172;
                                                                    attacker.TransformationDefence = 1600;
                                                                    attacker.TransformationMagicDefence = 98;
                                                                    attacker.TransformationDodge = 50;
                                                                    attacker.TransformationTime = 69;
                                                                    attacker.TransformationID = 267;
                                                                    break;
                                                                }
                                                            case 4:
                                                                {
                                                                    attacker.TransformationMaxAttack = 300;
                                                                    attacker.TransformationMinAttack = 200;
                                                                    attacker.TransformationDefence = 1900;
                                                                    attacker.TransformationMagicDefence = 99;
                                                                    attacker.TransformationDodge = 55;
                                                                    attacker.TransformationTime = 79;
                                                                    attacker.TransformationID = 267;
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    case 1270:
                                                        switch (spell.Level)
                                                        {
                                                            case 0:
                                                                {
                                                                    attacker.TransformationMaxAttack = 282;
                                                                    attacker.TransformationMinAttack = 179;
                                                                    attacker.TransformationDefence = 73;
                                                                    attacker.TransformationMagicDefence = 34;
                                                                    attacker.TransformationDodge = 9;
                                                                    attacker.TransformationTime = 34;
                                                                    attacker.TransformationID = 214;
                                                                    break;
                                                                }
                                                            case 1:
                                                                {
                                                                    attacker.TransformationMaxAttack = 395;
                                                                    attacker.TransformationMinAttack = 245;
                                                                    attacker.TransformationDefence = 126;
                                                                    attacker.TransformationMagicDefence = 45;
                                                                    attacker.TransformationDodge = 12;
                                                                    attacker.TransformationTime = 39;
                                                                    attacker.TransformationID = 214;
                                                                    break;
                                                                }
                                                            case 2:
                                                                {
                                                                    attacker.TransformationMaxAttack = 616;
                                                                    attacker.TransformationMinAttack = 367;
                                                                    attacker.TransformationDefence = 180;
                                                                    attacker.TransformationMagicDefence = 53;
                                                                    attacker.TransformationDodge = 15;
                                                                    attacker.TransformationTime = 44;
                                                                    attacker.TransformationID = 214;
                                                                    break;
                                                                }
                                                            case 3:
                                                                {
                                                                    attacker.TransformationMaxAttack = 724;
                                                                    attacker.TransformationMinAttack = 429;
                                                                    attacker.TransformationDefence = 247;
                                                                    attacker.TransformationMagicDefence = 53;
                                                                    attacker.TransformationDodge = 15;
                                                                    attacker.TransformationTime = 49;
                                                                    attacker.TransformationID = 214;
                                                                    break;
                                                                }
                                                            case 4:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1231;
                                                                    attacker.TransformationMinAttack = 704;
                                                                    attacker.TransformationDefence = 499;
                                                                    attacker.TransformationMagicDefence = 50;
                                                                    attacker.TransformationDodge = 20;
                                                                    attacker.TransformationTime = 54;
                                                                    attacker.TransformationID = 274;
                                                                    break;
                                                                }
                                                            case 5:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1573;
                                                                    attacker.TransformationMinAttack = 941;
                                                                    attacker.TransformationDefence = 601;
                                                                    attacker.TransformationMagicDefence = 53;
                                                                    attacker.TransformationDodge = 25;
                                                                    attacker.TransformationTime = 59;
                                                                    attacker.TransformationID = 274;
                                                                    break;
                                                                }
                                                            case 6:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1991;
                                                                    attacker.TransformationMinAttack = 1107;
                                                                    attacker.TransformationDefence = 1029;
                                                                    attacker.TransformationMagicDefence = 55;
                                                                    attacker.TransformationDodge = 30;
                                                                    attacker.TransformationTime = 64;
                                                                    attacker.TransformationID = 274;
                                                                    break;
                                                                }
                                                            case 7:
                                                                {
                                                                    attacker.TransformationMaxAttack = 2226;
                                                                    attacker.TransformationMinAttack = 1235;
                                                                    attacker.TransformationDefence = 1029;
                                                                    attacker.TransformationMagicDefence = 55;
                                                                    attacker.TransformationDodge = 35;
                                                                    attacker.TransformationTime = 69;
                                                                    attacker.TransformationID = 274;
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    case 1360:
                                                        switch (spell.Level)
                                                        {
                                                            case 0:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1215;
                                                                    attacker.TransformationMinAttack = 610;
                                                                    attacker.TransformationDefence = 100;
                                                                    attacker.TransformationMagicDefence = 96;
                                                                    attacker.TransformationDodge = 30;
                                                                    attacker.TransformationTime = 59;
                                                                    attacker.TransformationID = 217;
                                                                    break;
                                                                }
                                                            case 1:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1310;
                                                                    attacker.TransformationMinAttack = 650;
                                                                    attacker.TransformationDefence = 400;
                                                                    attacker.TransformationMagicDefence = 97;
                                                                    attacker.TransformationDodge = 30;
                                                                    attacker.TransformationTime = 79;
                                                                    attacker.TransformationID = 217;
                                                                    break;
                                                                }
                                                            case 2:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1420;
                                                                    attacker.TransformationMinAttack = 710;
                                                                    attacker.TransformationDefence = 650;
                                                                    attacker.TransformationMagicDefence = 98;
                                                                    attacker.TransformationDodge = 30;
                                                                    attacker.TransformationTime = 89;
                                                                    attacker.TransformationID = 217;
                                                                    break;
                                                                }
                                                            case 3:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1555;
                                                                    attacker.TransformationMinAttack = 780;
                                                                    attacker.TransformationDefence = 720;
                                                                    attacker.TransformationMagicDefence = 98;
                                                                    attacker.TransformationDodge = 30;
                                                                    attacker.TransformationTime = 99;
                                                                    attacker.TransformationID = 277;
                                                                    break;
                                                                }
                                                            case 4:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1660;
                                                                    attacker.TransformationMinAttack = 840;
                                                                    attacker.TransformationDefence = 1200;
                                                                    attacker.TransformationMagicDefence = 99;
                                                                    attacker.TransformationDodge = 30;
                                                                    attacker.TransformationTime = 109;
                                                                    attacker.TransformationID = 277;
                                                                    break;
                                                                }
                                                        }
                                                        break;
                                                    case 1280:
                                                        switch (spell.Level)
                                                        {
                                                            case 0:
                                                                {
                                                                    attacker.TransformationMaxAttack = 930;
                                                                    attacker.TransformationMinAttack = 656;
                                                                    attacker.TransformationDefence = 290;
                                                                    attacker.TransformationMagicDefence = 45;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 29;
                                                                    attacker.TransformationID = 213;
                                                                    break;
                                                                }
                                                            case 1:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1062;
                                                                    attacker.TransformationMinAttack = 750;
                                                                    attacker.TransformationDefence = 320;
                                                                    attacker.TransformationMagicDefence = 46;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 34;
                                                                    attacker.TransformationID = 213;
                                                                    break;
                                                                }
                                                            case 2:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1292;
                                                                    attacker.TransformationMinAttack = 910;
                                                                    attacker.TransformationDefence = 510;
                                                                    attacker.TransformationMagicDefence = 50;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 39;
                                                                    attacker.TransformationID = 213;
                                                                    break;
                                                                }
                                                            case 3:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1428;
                                                                    attacker.TransformationMinAttack = 1000;
                                                                    attacker.TransformationDefence = 600;
                                                                    attacker.TransformationMagicDefence = 53;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 44;
                                                                    attacker.TransformationID = 213;
                                                                    break;
                                                                }
                                                            case 4:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1570;
                                                                    attacker.TransformationMinAttack = 1100;
                                                                    attacker.TransformationDefence = 700;
                                                                    attacker.TransformationMagicDefence = 55;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 49;
                                                                    attacker.TransformationID = 213;
                                                                    break;
                                                                }
                                                            case 5:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1700;
                                                                    attacker.TransformationMinAttack = 1200;
                                                                    attacker.TransformationDefence = 880;
                                                                    attacker.TransformationMagicDefence = 57;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 54;
                                                                    attacker.TransformationID = 273;
                                                                    break;
                                                                }
                                                            case 6:
                                                                {
                                                                    attacker.TransformationMaxAttack = 1900;
                                                                    attacker.TransformationMinAttack = 1300;
                                                                    attacker.TransformationDefence = 1540;
                                                                    attacker.TransformationMagicDefence = 59;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 59;
                                                                    attacker.TransformationID = 273;
                                                                    break;
                                                                }
                                                            case 7:
                                                                {
                                                                    attacker.TransformationMaxAttack = 2100;
                                                                    attacker.TransformationMinAttack = 1500;
                                                                    attacker.TransformationDefence = 1880;
                                                                    attacker.TransformationMagicDefence = 61;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 59;
                                                                    attacker.TransformationID = 273;
                                                                    break;
                                                                }
                                                            case 8:
                                                                {
                                                                    attacker.TransformationMaxAttack = 2300;
                                                                    attacker.TransformationMinAttack = 1600;
                                                                    attacker.TransformationDefence = 1970;
                                                                    attacker.TransformationMagicDefence = 63;
                                                                    attacker.TransformationDodge = 40;
                                                                    attacker.TransformationTime = 59;
                                                                    attacker.TransformationID = 273;
                                                                    break;
                                                                }
                                                        }
                                                        break;

                                                    case 3321:
                                                        {
                                                            attacker.TransformationMaxAttack = 2000000;
                                                            attacker.TransformationMinAttack = 2000000;
                                                            attacker.TransformationDefence = 65355;
                                                            attacker.TransformationMagicDefence = 65355;
                                                            attacker.TransformationDodge = 35;
                                                            attacker.TransformationTime = 65355;
                                                            attacker.TransformationID = 223;
                                                            break;
                                                        }


                                                }
                                                #endregion

                                                SpellUse spellUse = new SpellUse(true);
                                                spellUse.Attacker = attacker.UID;
                                                spellUse.SpellID = spell.ID;
                                                spellUse.SpellLevel = spell.Level;
                                                spellUse.X = X;
                                                spellUse.Y = Y;
                                                spellUse.Targets.Add(attacker.UID, (uint)0);
                                                attacker.Owner.SendScreen(spellUse, true);
                                                attacker.TransformationStamp = Time32.Now;
                                                attacker.TransformationMaxHP = 3000;
                                                if (spell.ID == 1270)
                                                    attacker.TransformationMaxHP = 50000;
                                                attacker.TransformationAttackRange = 3;
                                                if (spell.ID == 1360)
                                                    attacker.TransformationAttackRange = 10;
                                                if (!wasTransformated)
                                                {
                                                    double maxHP = attacker.MaxHitpoints;
                                                    double HP = attacker.Hitpoints;
                                                    double point = HP / maxHP;

                                                    attacker.Hitpoints = (uint)(attacker.TransformationMaxHP * point);
                                                }
                                                attacker.Update(Update.MaxHitpoints, attacker.TransformationMaxHP, false);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Bless
                                    case 9876:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);
                                                attacker.AddFlag(Update.Flags.CastPray);
                                                SpellUse spellUse = new SpellUse(true);
                                                spellUse.Attacker = attacker.UID;
                                                spellUse.SpellID = spell.ID;
                                                spellUse.SpellLevel = spell.Level;
                                                spellUse.X = X;
                                                spellUse.Y = Y;
                                                spellUse.Targets.Add(attacker.UID, 0);
                                                attacker.Owner.SendScreen(spellUse, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region Companions
                                    case 4000:
                                    case 4010:
                                    case 4020:
                                    case 4050:
                                    case 4060:
                                    case 4070:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                if (ServerBase.Constants.steedguard.Contains(attacker.MapID))
                                                    return;
                                                if (attacker.Owner.Map.BaseID == 700)
                                                    return;
                                                if (attacker.Owner.Companion != null)
                                                {
                                                    if (attacker.Owner.Companion.MonsterInfo != null)
                                                    {
                                                        attacker.Owner.Map.RemoveEntity(attacker.Owner.Companion);
                                                        Data data = new Data(true);
                                                        data.UID = attacker.Owner.Companion.UID;
                                                        data.ID = Data.RemoveEntity;
                                                        attacker.Owner.Companion.MonsterInfo.SendScreen(data);
                                                        attacker.Owner.Companion = null;
                                                    }
                                                }
                                                PrepareSpell(spell, attacker.Owner);
                                                SpellUse spellUse = new SpellUse(true);
                                                spellUse.Attacker = attacker.UID;
                                                spellUse.SpellID = spell.ID;
                                                spellUse.SpellLevel = spell.Level;
                                                spellUse.X = X;
                                                spellUse.Y = Y;
                                                spellUse.Targets.Add(attacker.UID, 0);
                                                attacker.Owner.SendScreen(spellUse, true);
                                                attacker.Owner.Companion = new Entity(EntityFlag.Monster, true);
                                                attacker.Owner.Companion.MonsterInfo = new PhoenixProject.Database.MonsterInformation();
                                                Database.MonsterInformation mt = Database.MonsterInformation.MonsterInfos[spell.Power];
                                                attacker.Owner.Companion.Owner = attacker.Owner;
                                                attacker.Owner.Companion.MapObjType = MapObjectType.Monster;
                                                attacker.Owner.Companion.MonsterInfo = mt.Copy();
                                                attacker.Owner.Companion.MonsterInfo.Owner = attacker.Owner.Companion;
                                                attacker.Owner.Companion.Name = mt.Name;
                                                attacker.Owner.Companion.MinAttack = mt.MinAttack;
                                                attacker.Owner.Companion.MaxAttack = attacker.Owner.Companion.MagicAttack = mt.MaxAttack;
                                                attacker.Owner.Companion.Hitpoints = attacker.Owner.Companion.MaxHitpoints = mt.Hitpoints;
                                                attacker.Owner.Companion.Body = mt.Mesh;
                                                attacker.Owner.Companion.Level = mt.Level; //10000181 - 1000006

                                                attacker.Owner.Companion.UID = (uint)ServerBase.Kernel.Random.Next(900300, 970350);

                                                attacker.Owner.Companion.MapID = attacker.Owner.Map.ID;
                                                attacker.Owner.Companion.SendUpdates = true;
                                                attacker.Owner.Companion.X = attacker.X;
                                                attacker.Owner.Companion.Y = attacker.Y;
                                                attacker.Owner.Map.AddEntity(attacker.Owner.Companion);
                                                attacker.Owner.SendScreenSpawn(attacker.Owner.Companion, true);
                                            }
                                            break;
                                        }
                                    #endregion
                                    #region MonkSpells
                                    //Compassion
                                    case 10395:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);
                                                if (attacker.Owner.Team != null)
                                                {
                                                    foreach (Client.GameState c in attacker.Owner.Team.Teammates)
                                                    {
                                                        if (c.Entity.MapID == attacker.MapID)
                                                        {
                                                            short distance = Kernel.GetDistance(c.Entity.X, c.Entity.Y, attacker.X, attacker.Y);
                                                            if (distance < ServerBase.Constants.pScreenDistance)
                                                            {
                                                                if (c.Entity.UID != attacker.UID)
                                                                {
                                                                    if (!c.AlternateEquipment)
                                                                    {
                                                                        c.LoadItemStats(c.Entity);
                                                                    }
                                                                    else
                                                                    {
                                                                        c.LoadItemStats2(c.Entity);
                                                                    }

                                                                    c.Entity.AuraStamp = Time32.Now;
                                                                    c.Entity.AuraTime = 20;
                                                                    // attacked.MaxAttack += 1000;
                                                                    c.Entity.CriticalStrike += 10;
                                                                    //Console.WriteLine("sk" + attacked.CriticalStrike + "");
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FendAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.MetalAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WoodAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WaterAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.EarthAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FireAura);
                                                                    c.Entity.AddFlag2(Update.Flags2.TyrantAura);
                                                                    //Update ud = new Update(true);
                                                                    //ud.Aura(attacked, attacked.CriticalStrike, spell.Level, (byte)spell.Duration);
                                                                    PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                    switch (spell.ID)
                                                                    {
                                                                        case 0x28b4:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                                            break;

                                                                        case 0x28b5:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                                            break;

                                                                        case 0x28b6:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                                            break;

                                                                        case 0x28b7:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                                            break;

                                                                        case 0x28b8:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                                            break;

                                                                        case 0x2bc0:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                                            break;

                                                                        case 0x289b:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                            break;

                                                                        case 0x28aa:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                                            break;
                                                                    }
                                                                    StatusIconData data2 = new StatusIconData
                                                                    {
                                                                        AuraLevel = spell.Level,
                                                                        AuraPower = spell.Power,
                                                                        AuraType2 = tyrantAura,
                                                                        Identifier = c.Entity.UID,
                                                                        Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                                    };
                                                                    c.Entity.Owner.Send((byte[])data2);
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                if (!attacked.Owner.AlternateEquipment)
                                                {
                                                    attacked.Owner.LoadItemStats(attacked.Owner.Entity);
                                                }
                                                else
                                                {
                                                    attacked.Owner.LoadItemStats2(attacked.Owner.Entity);
                                                }
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                attacker.Owner.SendScreen(suse, true);
                                                attacked.AuraStamp = Time32.Now;
                                                attacked.AuraTime = 20;
                                                // attacked.MaxAttack += 1000;
                                                attacked.CriticalStrike += 10;
                                                //Console.WriteLine("sk" + attacked.CriticalStrike + "");
                                                attacked.RemoveFlag2(Update.Flags2.FendAura);
                                                attacked.RemoveFlag2(Update.Flags2.MetalAura);
                                                attacked.RemoveFlag2(Update.Flags2.WoodAura);
                                                attacked.RemoveFlag2(Update.Flags2.WaterAura);
                                                attacked.RemoveFlag2(Update.Flags2.EarthAura);
                                                attacked.RemoveFlag2(Update.Flags2.FireAura);
                                                attacked.AddFlag2(Update.Flags2.TyrantAura);
                                                //Update ud = new Update(true);
                                                //ud.Aura(attacked, attacked.CriticalStrike, spell.Level, (byte)spell.Duration);
                                                PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                switch (spell.ID)
                                                {
                                                    case 0x28b4:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                        break;

                                                    case 0x28b5:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                        break;

                                                    case 0x28b6:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                        break;

                                                    case 0x28b7:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                        break;

                                                    case 0x28b8:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                        break;

                                                    case 0x2bc0:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                        break;

                                                    case 0x289b:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                        break;

                                                    case 0x28aa:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                        break;
                                                }
                                                StatusIconData data = new StatusIconData
                                                {
                                                    AuraLevel = spell.Level,
                                                    AuraPower = spell.Power,
                                                    AuraType2 = tyrantAura2,
                                                    Identifier = attacker.UID,
                                                    Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                };
                                                attacker.Owner.Send((byte[])data);
                                            }
                                            break;
                                        }
                                    case 10410:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);
                                                if (attacker.Owner.Team != null)
                                                {
                                                    foreach (Client.GameState c in attacker.Owner.Team.Teammates)
                                                    {
                                                        if (c.Entity.MapID == attacker.MapID)
                                                        {
                                                            short distance = Kernel.GetDistance(c.Entity.X, c.Entity.Y, attacker.X, attacker.Y);
                                                            if (distance < ServerBase.Constants.pScreenDistance)
                                                            {
                                                                if (c.Entity.UID != attacker.UID)
                                                                {
                                                                    if (!c.AlternateEquipment)
                                                                    {
                                                                        c.LoadItemStats(c.Entity);
                                                                    }
                                                                    else
                                                                    {
                                                                        c.LoadItemStats2(c.Entity);
                                                                    }

                                                                    c.Entity.AuraStamp = Time32.Now;
                                                                    c.Entity.AuraTime = 20;
                                                                    c.Entity.SkillCStrike += 10;
                                                                    c.Entity.CriticalStrike += 10;
                                                                    // attacked.MaxAttack += 100;
                                                                    c.Entity.RemoveFlag2(Update.Flags2.TyrantAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.MetalAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WoodAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WaterAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.EarthAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FireAura);
                                                                    c.Entity.AddFlag2(Update.Flags2.FendAura);
                                                                    PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                    switch (spell.ID)
                                                                    {
                                                                        case 0x28b4:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                                            break;

                                                                        case 0x28b5:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                                            break;

                                                                        case 0x28b6:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                                            break;

                                                                        case 0x28b7:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                                            break;

                                                                        case 0x28b8:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                                            break;

                                                                        case 0x2bc0:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                                            break;

                                                                        case 0x289b:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                            break;

                                                                        case 0x28aa:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                                            break;
                                                                    }
                                                                    StatusIconData data2 = new StatusIconData
                                                                    {
                                                                        AuraLevel = spell.Level,
                                                                        AuraPower = spell.Power,
                                                                        AuraType2 = tyrantAura2,
                                                                        Identifier = c.Entity.UID,
                                                                        Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                                    };
                                                                    c.Entity.Owner.Send((byte[])data2);
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                if (!attacked.Owner.AlternateEquipment)
                                                {
                                                    attacked.Owner.LoadItemStats(attacked.Owner.Entity);
                                                }
                                                else
                                                {
                                                    attacked.Owner.LoadItemStats2(attacked.Owner.Entity);
                                                }
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                attacker.Owner.SendScreen(suse, true);
                                                attacked.AuraStamp = Time32.Now;
                                                attacked.AuraTime = 20;
                                                attacked.SkillCStrike += 10;
                                                attacked.CriticalStrike += 10;
                                                // attacked.MaxAttack += 100;
                                                attacked.RemoveFlag2(Update.Flags2.TyrantAura);
                                                attacked.RemoveFlag2(Update.Flags2.MetalAura);
                                                attacked.RemoveFlag2(Update.Flags2.WoodAura);
                                                attacked.RemoveFlag2(Update.Flags2.WaterAura);
                                                attacked.RemoveFlag2(Update.Flags2.EarthAura);
                                                attacked.RemoveFlag2(Update.Flags2.FireAura);
                                                attacked.AddFlag2(Update.Flags2.FendAura);
                                                PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                switch (spell.ID)
                                                {
                                                    case 0x28b4:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                        break;

                                                    case 0x28b5:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                        break;

                                                    case 0x28b6:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                        break;

                                                    case 0x28b7:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                        break;

                                                    case 0x28b8:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                        break;

                                                    case 0x2bc0:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                        break;

                                                    case 0x289b:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                        break;

                                                    case 0x28aa:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                        break;
                                                }
                                                StatusIconData data = new StatusIconData
                                                {
                                                    AuraLevel = spell.Level,
                                                    AuraPower = spell.Power,
                                                    AuraType2 = tyrantAura,
                                                    Identifier = attacker.UID,
                                                    Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                };
                                                attacker.Owner.Send((byte[])data);
                                                //Update ud = new Update(true);
                                                //ud.Aura(attacked, attacked.SkillCStrike, spell.Level, (byte)spell.Duration);
                                            }
                                            break;
                                        }
                                    case 10420:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);
                                                if (attacker.Owner.Team != null)
                                                {
                                                    foreach (Client.GameState c in attacker.Owner.Team.Teammates)
                                                    {
                                                        if (c.Entity.MapID == attacker.MapID)
                                                        {
                                                            short distance = Kernel.GetDistance(c.Entity.X, c.Entity.Y, attacker.X, attacker.Y);
                                                            if (distance < ServerBase.Constants.pScreenDistance)
                                                            {
                                                                if (c.Entity.UID != attacker.UID)
                                                                {
                                                                    if (!c.AlternateEquipment)
                                                                    {
                                                                        c.LoadItemStats(c.Entity);
                                                                    }
                                                                    else
                                                                    {
                                                                        c.LoadItemStats2(c.Entity);
                                                                    }

                                                                    c.Entity.AuraStamp = Time32.Now;
                                                                    c.Entity.AuraTime = 20;
                                                                    c.Entity.MetalResistance += 30;
                                                                    c.Entity.RemoveFlag2(Update.Flags2.TyrantAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FendAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WoodAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WaterAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.EarthAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FireAura);
                                                                    c.Entity.AddFlag2(Update.Flags2.MetalAura);
                                                                    PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                    switch (spell.ID)
                                                                    {
                                                                        case 0x28b4:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                                            break;

                                                                        case 0x28b5:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                                            break;

                                                                        case 0x28b6:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                                            break;

                                                                        case 0x28b7:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                                            break;

                                                                        case 0x28b8:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                                            break;

                                                                        case 0x2bc0:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                                            break;

                                                                        case 0x289b:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                            break;

                                                                        case 0x28aa:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                                            break;
                                                                    }
                                                                    StatusIconData data2 = new StatusIconData
                                                                    {
                                                                        AuraLevel = spell.Level,
                                                                        AuraPower = spell.Power,
                                                                        AuraType2 = tyrantAura2,
                                                                        Identifier = c.Entity.UID,
                                                                        Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                                    };
                                                                    c.Entity.Owner.Send((byte[])data2);
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                if (!attacked.Owner.AlternateEquipment)
                                                {
                                                    attacked.Owner.LoadItemStats(attacked.Owner.Entity);
                                                }
                                                else
                                                {
                                                    attacked.Owner.LoadItemStats2(attacked.Owner.Entity);
                                                }
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                attacker.Owner.SendScreen(suse, true);
                                                attacked.AuraStamp = Time32.Now;
                                                attacked.AuraTime = 20;
                                                attacked.MetalResistance += 30;
                                                attacked.RemoveFlag2(Update.Flags2.TyrantAura);
                                                attacked.RemoveFlag2(Update.Flags2.FendAura);
                                                attacked.RemoveFlag2(Update.Flags2.WoodAura);
                                                attacked.RemoveFlag2(Update.Flags2.WaterAura);
                                                attacked.RemoveFlag2(Update.Flags2.EarthAura);
                                                attacked.RemoveFlag2(Update.Flags2.FireAura);
                                                attacked.AddFlag2(Update.Flags2.MetalAura);
                                                PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                switch (spell.ID)
                                                {
                                                    case 0x28b4:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                        break;

                                                    case 0x28b5:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                        break;

                                                    case 0x28b6:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                        break;

                                                    case 0x28b7:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                        break;

                                                    case 0x28b8:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                        break;

                                                    case 0x2bc0:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                        break;

                                                    case 0x289b:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                        break;

                                                    case 0x28aa:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                        break;
                                                }
                                                StatusIconData data = new StatusIconData
                                                {
                                                    AuraLevel = spell.Level,
                                                    AuraPower = spell.Power,
                                                    AuraType2 = tyrantAura,
                                                    Identifier = attacker.UID,
                                                    Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                };
                                                attacker.Owner.Send((byte[])data);
                                            }
                                            //Update ud = new Update(true);
                                            //ud.Aura(attacked, attacked.MetalResistance, spell.Level, (byte)spell.Duration);
                                            break;
                                        }
                                    case 10421:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);
                                                if (attacker.Owner.Team != null)
                                                {
                                                    foreach (Client.GameState c in attacker.Owner.Team.Teammates)
                                                    {
                                                        if (c.Entity.MapID == attacker.MapID)
                                                        {
                                                            short distance = Kernel.GetDistance(c.Entity.X, c.Entity.Y, attacker.X, attacker.Y);
                                                            if (distance < ServerBase.Constants.pScreenDistance)
                                                            {
                                                                if (c.Entity.UID != attacker.UID)
                                                                {
                                                                    if (!c.AlternateEquipment)
                                                                    {
                                                                        c.LoadItemStats(c.Entity);
                                                                    }
                                                                    else
                                                                    {
                                                                        c.LoadItemStats2(c.Entity);
                                                                    }

                                                                    c.Entity.AuraStamp = Time32.Now;
                                                                    c.Entity.AuraTime = 20;
                                                                    c.Entity.WoodResistance += 30;
                                                                    c.Entity.RemoveFlag2(Update.Flags2.TyrantAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FendAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.MetalAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WaterAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.EarthAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FireAura);
                                                                    c.Entity.AddFlag2(Update.Flags2.WoodAura);
                                                                    PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                    switch (spell.ID)
                                                                    {
                                                                        case 0x28b4:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                                            break;

                                                                        case 0x28b5:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                                            break;

                                                                        case 0x28b6:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                                            break;

                                                                        case 0x28b7:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                                            break;

                                                                        case 0x28b8:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                                            break;

                                                                        case 0x2bc0:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                                            break;

                                                                        case 0x289b:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                            break;

                                                                        case 0x28aa:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                                            break;
                                                                    }
                                                                    StatusIconData data2 = new StatusIconData
                                                                    {
                                                                        AuraLevel = spell.Level,
                                                                        AuraPower = spell.Power,
                                                                        AuraType2 = tyrantAura2,
                                                                        Identifier = c.Entity.UID,
                                                                        Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                                    };
                                                                    c.Entity.Owner.Send((byte[])data2);
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                if (!attacked.Owner.AlternateEquipment)
                                                {
                                                    attacked.Owner.LoadItemStats(attacked.Owner.Entity);
                                                }
                                                else
                                                {
                                                    attacked.Owner.LoadItemStats2(attacked.Owner.Entity);
                                                }
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                attacker.Owner.SendScreen(suse, true);
                                                attacked.AuraStamp = Time32.Now;
                                                attacked.AuraTime = 20;
                                                attacked.WoodResistance += 30;
                                                attacked.RemoveFlag2(Update.Flags2.TyrantAura);
                                                attacked.RemoveFlag2(Update.Flags2.FendAura);
                                                attacked.RemoveFlag2(Update.Flags2.MetalAura);
                                                attacked.RemoveFlag2(Update.Flags2.WaterAura);
                                                attacked.RemoveFlag2(Update.Flags2.EarthAura);
                                                attacked.RemoveFlag2(Update.Flags2.FireAura);
                                                attacked.AddFlag2(Update.Flags2.WoodAura);
                                                PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                switch (spell.ID)
                                                {
                                                    case 0x28b4:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                        break;

                                                    case 0x28b5:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                        break;

                                                    case 0x28b6:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                        break;

                                                    case 0x28b7:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                        break;

                                                    case 0x28b8:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                        break;

                                                    case 0x2bc0:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                        break;

                                                    case 0x289b:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                        break;

                                                    case 0x28aa:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                        break;
                                                }
                                                StatusIconData data = new StatusIconData
                                                {
                                                    AuraLevel = spell.Level,
                                                    AuraPower = spell.Power,
                                                    AuraType2 = tyrantAura,
                                                    Identifier = attacker.UID,
                                                    Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                };
                                                attacker.Owner.Send((byte[])data);
                                                //Update ud = new Update(true);
                                            }
                                            //ud.Aura(attacked, attacked.WoodResistance, spell.Level, (byte)spell.Duration);
                                            break;
                                        }
                                    case 10422:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);
                                                if (attacker.Owner.Team != null)
                                                {
                                                    foreach (Client.GameState c in attacker.Owner.Team.Teammates)
                                                    {
                                                        if (c.Entity.MapID == attacker.MapID)
                                                        {
                                                            short distance = Kernel.GetDistance(c.Entity.X, c.Entity.Y, attacker.X, attacker.Y);
                                                            if (distance < ServerBase.Constants.pScreenDistance)
                                                            {
                                                                if (c.Entity.UID != attacker.UID)
                                                                {
                                                                    if (!c.AlternateEquipment)
                                                                    {
                                                                        c.LoadItemStats(c.Entity);
                                                                    }
                                                                    else
                                                                    {
                                                                        c.LoadItemStats2(c.Entity);
                                                                    }

                                                                    c.Entity.AuraStamp = Time32.Now;
                                                                    c.Entity.AuraTime = 20;
                                                                    c.Entity.WaterResistance += 30;
                                                                    c.Entity.RemoveFlag2(Update.Flags2.TyrantAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FendAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.MetalAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WoodAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.EarthAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FireAura);
                                                                    c.Entity.AddFlag2(Update.Flags2.WaterAura);
                                                                    PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                    switch (spell.ID)
                                                                    {
                                                                        case 0x28b4:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                                            break;

                                                                        case 0x28b5:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                                            break;

                                                                        case 0x28b6:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                                            break;

                                                                        case 0x28b7:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                                            break;

                                                                        case 0x28b8:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                                            break;

                                                                        case 0x2bc0:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                                            break;

                                                                        case 0x289b:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                            break;

                                                                        case 0x28aa:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                                            break;
                                                                    }
                                                                    StatusIconData data2 = new StatusIconData
                                                                    {
                                                                        AuraLevel = spell.Level,
                                                                        AuraPower = spell.Power,
                                                                        AuraType2 = tyrantAura2,
                                                                        Identifier = c.Entity.UID,
                                                                        Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                                    };
                                                                    c.Entity.Owner.Send((byte[])data2);
                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                if (!attacked.Owner.AlternateEquipment)
                                                {
                                                    attacked.Owner.LoadItemStats(attacked.Owner.Entity);
                                                }
                                                else
                                                {
                                                    attacked.Owner.LoadItemStats2(attacked.Owner.Entity);
                                                }
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                attacker.Owner.SendScreen(suse, true);
                                                attacked.AuraStamp = Time32.Now;
                                                attacked.AuraTime = 20;
                                                attacked.WaterResistance += 30;
                                                attacked.RemoveFlag2(Update.Flags2.TyrantAura);
                                                attacked.RemoveFlag2(Update.Flags2.FendAura);
                                                attacked.RemoveFlag2(Update.Flags2.MetalAura);
                                                attacked.RemoveFlag2(Update.Flags2.WoodAura);
                                                attacked.RemoveFlag2(Update.Flags2.EarthAura);
                                                attacked.RemoveFlag2(Update.Flags2.FireAura);
                                                attacked.AddFlag2(Update.Flags2.WaterAura);
                                                PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                switch (spell.ID)
                                                {
                                                    case 0x28b4:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                        break;

                                                    case 0x28b5:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                        break;

                                                    case 0x28b6:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                        break;

                                                    case 0x28b7:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                        break;

                                                    case 0x28b8:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                        break;

                                                    case 0x2bc0:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                        break;

                                                    case 0x289b:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                        break;

                                                    case 0x28aa:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                        break;
                                                }
                                                StatusIconData data = new StatusIconData
                                                {
                                                    AuraLevel = spell.Level,
                                                    AuraPower = spell.Power,
                                                    AuraType2 = tyrantAura,
                                                    Identifier = attacker.UID,
                                                    Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                };
                                                attacker.Owner.Send((byte[])data);
                                                //Update ud = new Update(true);
                                            }
                                            //ud.Aura(attacked, attacked.WaterResistance, spell.Level, (byte)spell.Duration);
                                            break;
                                        }

                                    case 10424:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);
                                                if (attacker.Owner.Team != null)
                                                {
                                                    foreach (Client.GameState c in attacker.Owner.Team.Teammates)
                                                    {
                                                        if (c.Entity.MapID == attacker.MapID)
                                                        {
                                                            short distance = Kernel.GetDistance(c.Entity.X, c.Entity.Y, attacker.X, attacker.Y);
                                                            if (distance < ServerBase.Constants.pScreenDistance)
                                                            {
                                                                if (c.Entity.UID != attacker.UID)
                                                                {
                                                                    if (!c.AlternateEquipment)
                                                                    {
                                                                        c.LoadItemStats(c.Entity);
                                                                    }
                                                                    else
                                                                    {
                                                                        c.LoadItemStats2(c.Entity);
                                                                    }

                                                                    c.Entity.AuraStamp = Time32.Now;
                                                                    c.Entity.AuraTime = 20;
                                                                    c.Entity.EarthResistance += 30;
                                                                    c.Entity.RemoveFlag2(Update.Flags2.TyrantAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FendAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.MetalAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WoodAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WaterAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FireAura);
                                                                    c.Entity.AddFlag2(Update.Flags2.EarthAura);
                                                                    PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                    switch (spell.ID)
                                                                    {
                                                                        case 0x28b4:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                                            break;

                                                                        case 0x28b5:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                                            break;

                                                                        case 0x28b6:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                                            break;

                                                                        case 0x28b7:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                                            break;

                                                                        case 0x28b8:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                                            break;

                                                                        case 0x2bc0:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                                            break;

                                                                        case 0x289b:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                            break;

                                                                        case 0x28aa:
                                                                            tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                                            break;
                                                                    }
                                                                    StatusIconData data2 = new StatusIconData
                                                                    {
                                                                        AuraLevel = spell.Level,
                                                                        AuraPower = spell.Power,
                                                                        AuraType2 = tyrantAura,
                                                                        Identifier = c.Entity.UID,
                                                                        Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                                    };
                                                                    c.Entity.Owner.Send((byte[])data2);

                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                if (!attacked.Owner.AlternateEquipment)
                                                {
                                                    attacked.Owner.LoadItemStats(attacked.Owner.Entity);
                                                }
                                                else
                                                {
                                                    attacked.Owner.LoadItemStats2(attacked.Owner.Entity);
                                                }
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                attacker.Owner.SendScreen(suse, true);
                                                attacked.AuraStamp = Time32.Now;
                                                attacked.AuraTime = 20;
                                                attacked.EarthResistance += 30;
                                                attacked.RemoveFlag2(Update.Flags2.TyrantAura);
                                                attacked.RemoveFlag2(Update.Flags2.FendAura);
                                                attacked.RemoveFlag2(Update.Flags2.MetalAura);
                                                attacked.RemoveFlag2(Update.Flags2.WoodAura);
                                                attacked.RemoveFlag2(Update.Flags2.WaterAura);
                                                attacked.RemoveFlag2(Update.Flags2.FireAura);
                                                attacked.AddFlag2(Update.Flags2.EarthAura);
                                                PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                switch (spell.ID)
                                                {
                                                    case 0x28b4:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                        break;

                                                    case 0x28b5:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                        break;

                                                    case 0x28b6:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                        break;

                                                    case 0x28b7:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                        break;

                                                    case 0x28b8:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                        break;

                                                    case 0x2bc0:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                        break;

                                                    case 0x289b:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                        break;

                                                    case 0x28aa:
                                                        tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                        break;
                                                }
                                                StatusIconData data = new StatusIconData
                                                {
                                                    AuraLevel = spell.Level,
                                                    AuraPower = spell.Power,
                                                    AuraType2 = tyrantAura2,
                                                    Identifier = attacker.UID,
                                                    Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                };
                                                attacker.Owner.Send((byte[])data);
                                                //Update ud = new Update(true);
                                            }
                                            ///ud.Aura(attacked, attacked.EarthResistance, spell.Level, (byte)spell.Duration);
                                            break;
                                        }
                                    case 10423:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);
                                                if (attacker.Owner.Team != null)
                                                {
                                                    foreach (Client.GameState c in attacker.Owner.Team.Teammates)
                                                    {
                                                        if (c.Entity.MapID == attacker.MapID)
                                                        {
                                                            short distance = Kernel.GetDistance(c.Entity.X, c.Entity.Y, attacker.X, attacker.Y);
                                                            if (distance < ServerBase.Constants.pScreenDistance)
                                                            {
                                                                if (c.Entity.UID != attacker.UID)
                                                                {
                                                                    if (!c.AlternateEquipment)
                                                                    {
                                                                        c.LoadItemStats(c.Entity);
                                                                    }
                                                                    else
                                                                    {
                                                                        c.LoadItemStats2(c.Entity);
                                                                    }

                                                                    c.Entity.AuraStamp = Time32.Now;
                                                                    c.Entity.AuraTime = 20;
                                                                    c.Entity.FireResistance += 30;
                                                                    c.Entity.RemoveFlag2(Update.Flags2.TyrantAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.FendAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.MetalAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WoodAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.WaterAura);
                                                                    c.Entity.RemoveFlag2(Update.Flags2.EarthAura);
                                                                    c.Entity.AddFlag2(Update.Flags2.FireAura);
                                                                    PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                    switch (spell.ID)
                                                                    {
                                                                        case 0x28b4:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                                            break;

                                                                        case 0x28b5:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                                            break;

                                                                        case 0x28b6:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                                            break;

                                                                        case 0x28b7:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                                            break;

                                                                        case 0x28b8:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                                            break;

                                                                        case 0x2bc0:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                                            break;

                                                                        case 0x289b:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                                            break;

                                                                        case 0x28aa:
                                                                            tyrantAura2 = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                                            break;
                                                                    }
                                                                    StatusIconData data2 = new StatusIconData
                                                                    {
                                                                        AuraLevel = spell.Level,
                                                                        AuraPower = spell.Power,
                                                                        AuraType2 = tyrantAura2,
                                                                        Identifier = c.Entity.UID,
                                                                        Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                                    };
                                                                    c.Entity.Owner.Send((byte[])data2);

                                                                }
                                                            }
                                                        }

                                                    }
                                                }
                                                if (!attacked.Owner.AlternateEquipment)
                                                {
                                                    attacked.Owner.LoadItemStats(attacked.Owner.Entity);
                                                }
                                                else
                                                {
                                                    attacked.Owner.LoadItemStats2(attacked.Owner.Entity);
                                                }
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;
                                                attacker.Owner.SendScreen(suse, true);
                                                attacked.AuraStamp = Time32.Now;
                                                attacked.AuraTime = 20;
                                                attacked.FireResistance += 30;
                                                attacked.RemoveFlag2(Update.Flags2.TyrantAura);
                                                attacked.RemoveFlag2(Update.Flags2.FendAura);
                                                attacked.RemoveFlag2(Update.Flags2.MetalAura);
                                                attacked.RemoveFlag2(Update.Flags2.WoodAura);
                                                attacked.RemoveFlag2(Update.Flags2.WaterAura);
                                                attacked.RemoveFlag2(Update.Flags2.EarthAura);
                                                attacked.AddFlag2(Update.Flags2.FireAura);
                                                PhoenixProject.Network.GamePackets.StatusIconData.AuraType tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                switch (spell.ID)
                                                {
                                                    case 0x28b4:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MetalAura;
                                                        break;

                                                    case 0x28b5:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WoodAura;
                                                        break;

                                                    case 0x28b6:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.WaterAura;
                                                        break;

                                                    case 0x28b7:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FireAura;
                                                        break;

                                                    case 0x28b8:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.EarthAura;
                                                        break;

                                                    case 0x2bc0:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.MagicDefender;
                                                        break;

                                                    case 0x289b:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.TyrantAura;
                                                        break;

                                                    case 0x28aa:
                                                        tyrantAura = PhoenixProject.Network.GamePackets.StatusIconData.AuraType.FendAura;
                                                        break;
                                                }
                                                StatusIconData data = new StatusIconData
                                                {
                                                    AuraLevel = spell.Level,
                                                    AuraPower = spell.Power,
                                                    AuraType2 = tyrantAura,
                                                    Identifier = attacker.UID,
                                                    Type = PhoenixProject.Network.GamePackets.StatusIconData.AuraDataTypes.Add
                                                };
                                                attacker.Owner.Send((byte[])data);
                                            }
                                            // Update ud = new Update(true);
                                            // ud.Aura(attacked, attacked.FireResistance, spell.Level, (byte)spell.Duration);
                                            break;
                                        }
                                    case 10430:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = spell.ID;
                                                suse.SpellLevel = spell.Level;
                                                suse.X = X;
                                                suse.Y = Y;

                                                if (attacker.Owner.Team != null)
                                                {
                                                    PrepareSpell(spell, attacker.Owner);
                                                    foreach (Client.GameState teammate in attacker.Owner.Team.Teammates)
                                                    {
                                                        if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, teammate.Entity.X, teammate.Entity.Y) <= spell.Distance)
                                                        {
                                                            attacker.RemoveFlag(Update.Flags.Poisoned);

                                                            suse.Targets.Add(teammate.Entity.UID, 1);
                                                        }
                                                    }
                                                    if (attacked.EntityFlag == EntityFlag.Player)
                                                        attacked.Owner.SendScreen(suse, true);
                                                    else
                                                        attacked.MonsterInfo.SendScreen(suse);
                                                }
                                                else
                                                {
                                                    if (attacked == null)
                                                        return;
                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Distance)
                                                    {
                                                        PrepareSpell(spell, attacker.Owner);

                                                        attacker.RemoveFlag(Update.Flags.Poisoned);

                                                        suse.Targets.Add(attacked.UID, 1);

                                                        if (attacked.EntityFlag == EntityFlag.Player)
                                                            attacked.Owner.SendScreen(suse, true);
                                                        else
                                                            attacked.MonsterInfo.SendScreen(suse);
                                                    }
                                                    else
                                                    {
                                                        attacker.AttackPacket = null;
                                                    }
                                                }
                                            }
                                            attacker.AttackPacket = null;
                                            break;
                                        }
                                    //Serenity
                                    case 10400:
                                        {
                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                if (attacker == null) return;

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = SpellID;
                                                suse.SpellLevel = attacker.Owner.Spells[SpellID].Level;
                                                suse.X = X;
                                                suse.Y = Y;

                                                suse.Targets.Add(attacker.UID, 1);

                                                attacker.ToxicFogLeft = 0;
                                                attacker.NoDrugsTime = 0;
                                                attacker.RemoveFlag2(Update.Flags2.SoulShackle);
                                                SyncPacket packet3 = new SyncPacket
                                                {
                                                    Identifier = attacked.UID,
                                                    Count = 2,
                                                    Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                                                    StatusFlag1 = (ulong)attacked.StatusFlag,
                                                    StatusFlag2 = (ulong)attacked.StatusFlag2,
                                                    Unknown1 = 0x36,
                                                    StatusFlagOffset = 0x6f,
                                                    Time = 0,
                                                    Value = 0,
                                                    Level = spell.Level
                                                };
                                                attacked.Owner.Send((byte[])packet3);
                                                attacker.Owner.SendScreen(suse, true);
                                            }
                                            attacker.AttackPacket = null;
                                            break;
                                        }
                                    //Tranquility
                                    case 10425:
                                        {
                                            if (attacked == null) return;

                                            if (CanUseSpell(spell, attacker.Owner))
                                            {
                                                PrepareSpell(spell, attacker.Owner);

                                                if (attacked == null) return;

                                                SpellUse suse = new SpellUse(true);
                                                suse.Attacker = attacker.UID;
                                                suse.SpellID = SpellID;
                                                suse.SpellLevel = attacker.Owner.Spells[SpellID].Level;
                                                suse.X = X;
                                                suse.Y = Y;


                                                suse.Targets.Add(attacked.UID, 1);

                                                attacked.ToxicFogLeft = 0;
                                                attacked.ShackleTime = 0;
                                                attacked.NoDrugsTime = 0;
                                                attacked.RemoveFlag2(Update.Flags2.SoulShackle);
                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                {
                                                    SyncPacket packet3 = new SyncPacket
                                                    {
                                                        Identifier = attacked.UID,
                                                        Count = 2,
                                                        Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                                                        StatusFlag1 = (ulong)attacked.StatusFlag,
                                                        StatusFlag2 = (ulong)attacked.StatusFlag2,
                                                        Unknown1 = 0x36,
                                                        StatusFlagOffset = 0x6f,
                                                        Time = 0,
                                                        Value = 0,
                                                        Level = spell.Level
                                                    };
                                                    attacked.Owner.Send((byte[])packet3);
                                                }
                                                if (attacked.EntityFlag == EntityFlag.Player)
                                                    attacked.Owner.SendScreen(suse, true);
                                                else
                                                    attacked.MonsterInfo.SendScreen(suse);
                                            }
                                            attacker.AttackPacket = null;
                                            break;
                                        }

                                    //WhirlwindKick
                                    case 10415:
                                        {
                                            if (Time32.Now < attacker.WhilrwindKick.AddMilliseconds(1200))
                                            { attacker.AttackPacket = null; return; }
                                            attacker.WhilrwindKick = Time32.Now;
                                            if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= 3)
                                            {
                                                if (CanUseSpell(spell, attacker.Owner))
                                                {
                                                    PrepareSpell(spell, attacker.Owner);

                                                    SpellUse suse = new SpellUse(true);
                                                    suse.Attacker = attacker.UID;
                                                    suse.SpellID = spell.ID;
                                                    suse.SpellLevel = 0;
                                                    suse.X = (ushort)ServerBase.Kernel.Random.Next(3, 10);
                                                    suse.Y = 0;

                                                    if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, X, Y) <= 3)
                                                    {
                                                        for (int c = 0; c < attacker.Owner.Screen.Objects.Count; c++)
                                                        {
                                                            //For a multi threaded application, while we go through the collection
                                                            //the collection might change. We will make sure that we wont go off  
                                                            //the limits with a check.
                                                            if (c >= attacker.Owner.Screen.Objects.Count)
                                                                break;
                                                            Interfaces.IMapObject _obj = attacker.Owner.Screen.Objects[c];
                                                            if (_obj == null)
                                                                continue;
                                                            if (_obj.MapObjType == MapObjectType.Monster || _obj.MapObjType == MapObjectType.Player)
                                                            {
                                                                attacked = _obj as Entity;
                                                                if (ServerBase.Kernel.GetDistance(attacker.X, attacker.Y, attacked.X, attacked.Y) <= spell.Range)
                                                                {
                                                                    if (CanAttack(attacker, attacked, spell, attack.AttackType == Attack.Ranged))
                                                                    {
                                                                        uint damage = Game.Attacking.Calculate.Melee(attacker, attacked, spell, ref attack);

                                                                        suse.Effect1 = attack.Effect1;
                                                                        ReceiveAttack(attacker, attacked, attack, damage, spell);
                                                                        attacked.Stunned = true;
                                                                        attacked.StunStamp = Time32.Now;
                                                                        suse.Targets.Add(attacked.UID, damage);

                                                                    }
                                                                }
                                                            }
                                                        }
                                                        attacker.AttackPacket = null;
                                                    }
                                                    else
                                                    {
                                                        attacker.AttackPacket = null; return;
                                                    }
                                                    attacker.Owner.SendScreen(suse, true);
                                                    suse.Targets = new SafeDictionary<uint, SpellUse.DamageClass>();
                                                    attacker.AttackPacket = null; return;
                                                }
                                                attacker.AttackPacket = null;
                                            }
                                            attacker.AttackPacket = null; return;
                                        }
                                    #endregion
                                    default:
                                        if (attacker.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.ProjectManager)
                                            attacker.Owner.Send(new Message("Unknown spell id: " + spell.ID, System.Drawing.Color.CadetBlue, Message.Talk));
                                        break;

                                }

                                if (spell.CanKill == 0)
                                {
                                    attacker.Owner.IncreaseSpellExperience(80, spellID);
                                }
                                //  attacker.Owner.IncreaseSpellExperience(Experience, spellID);//kimo
                                if (attacker.MapID == 1039)
                                {
                                    if (spell.ID == 7001 || spell.ID == 9876)
                                    {
                                        attacker.AttackPacket = null;
                                        return;
                                    }
                                    if (attacker.AttackPacket != null)
                                    {
                                        attack.Damage = spell.ID;
                                        attacker.AttackPacket = attack;
                                        if (Database.SpellTable.WeaponSpells.ContainsValue(spell.ID))
                                        {
                                            if (attacker.AttackPacket == null)
                                            {
                                                attack.AttackType = Attack.Melee;
                                                attacker.AttackPacket = attack;
                                            }
                                            else
                                            {
                                                attacker.AttackPacket.AttackType = Attack.Melee;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (spell.NextSpellID != 0)
                                    {
                                        if (spell.NextSpellID >= 1000 && spell.NextSpellID <= 1002)
                                            if (Target >= 1000000)
                                            {
                                                attacker.AttackPacket = null;
                                                return;
                                            }
                                        attack.Damage = spell.NextSpellID;
                                        attacker.AttackPacket = attack;
                                    }
                                    else
                                    {
                                        if (!Database.SpellTable.WeaponSpells.ContainsValue(spell.ID) || spell.ID == 9876)
                                            attacker.AttackPacket = null;
                                        else
                                        {
                                            if (attacker.AttackPacket == null)
                                            {
                                                attack.AttackType = Attack.Melee;
                                                attacker.AttackPacket = attack;
                                            }
                                            else
                                            {
                                                attacker.AttackPacket.AttackType = Attack.Melee;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                attacker.AttackPacket = null;
                            }
                        }
                    #endregion


                    }

                #endregion

                }
            #endregion
            }
        }
        public static List<IMapObject> GetObjects(UInt16 ox, UInt16 oy, Client.GameState c)
        {
            UInt16 x, y;
            x = c.Entity.X;
            y = c.Entity.Y;

            var list = new List<IMapObject>();
            c.Entity.X = ox;
            c.Entity.Y = oy;
            foreach (IMapObject objects in c.Screen.Objects)
            {
                if (objects != null)
                    if (objects.UID != c.Entity.UID)
                        if (!list.Contains(objects))
                            list.Add(objects);
            }
            c.Entity.X = x;
            c.Entity.Y = y;
            foreach (IMapObject objects in c.Screen.Objects)
            {
                if (objects != null)
                    if (objects.UID != c.Entity.UID)
                        if (!list.Contains(objects))
                            list.Add(objects);
            }
            if (list.Count > 0)
                return list;
            return null;
        }
        public static void ReceiveAttack(Game.Entity attacker, Game.Entity attacked, Attack attack, uint damage, Database.SpellInformation spell)
        {
            //Console.WriteLine("ssS");
            if (attacked.EntityFlag == EntityFlag.Player)
            {
                if (attacked.Action == Enums.ConquerAction.Sit)
                    if (attacked.Stamina > 20)
                        attacked.Stamina -= 20;
                    else
                        attacked.Stamina = 0;
                attacked.Action = Enums.ConquerAction.None;
                //Console.WriteLine("ssS");
            }
            #region TeamSkill
            if (attacker.MapID == KimoSkillWar.MapID && attacker.Owner.YellowOn == true)
            {
                if (attacker.Owner.YellowOn == true && attacked.Owner.YellowOn == true)
                    return;
                damage = 1;
                #region Disqualification & Spell check
                try
                {
                    if (spell == null)
                    {
                        attacker.Owner.Send(new Message("Only Skills is allowed!", System.Drawing.Color.Red, Message.TopLeft));
                        return;
                    }
                    else
                    {
                        if (!ServerBase.Constants.SkillTeamSkills.Contains(spell.ID))
                        {
                            attacker.Owner.Send(new Message("Only Skills is allowed!", System.Drawing.Color.Red, Message.TopLeft));
                            return;
                        }

                    }
                }
                catch { }
                #endregion

                /*if (attacked.Owner.Equipment.Objects[8].ID == attacker.Owner.Equipment.Objects[8].ID)
                    return;*/

                KimoSkillWar.YKills += 1;
                return;
            }
            if (attacker.MapID == KimoSkillWar.MapID && attacker.Owner.RedOn == true)
            {
                if (attacker.Owner.RedOn == true && attacked.Owner.RedOn == true)
                    return;
                damage = 1;
                #region Disqualification & Spell check
                try
                {
                    if (spell == null)
                    {
                        attacker.Owner.Send(new Message("Only Skills is allowed!", System.Drawing.Color.Red, Message.TopLeft));
                        return;
                    }
                    else
                    {
                        if (!ServerBase.Constants.SkillTeamSkills.Contains(spell.ID))
                        {
                            attacker.Owner.Send(new Message("Only Skills is allowed!", System.Drawing.Color.Red, Message.TopLeft));
                            return;
                        }

                    }
                }
                catch { }
                #endregion

                if (attacked.Owner.Equipment.Objects[8].ID == attacker.Owner.Equipment.Objects[8].ID)
                    return;

                KimoSkillWar.RKills += 1;
                return;
            }
            #endregion

           // if (!(attacked.Name.Contains("Guard") && attacked.EntityFlag == EntityFlag.Monster))
            if (attacker.EntityFlag == EntityFlag.Player && !attacked.Name.Contains("Guard"))
            {
                if (attacked.EntityFlag == EntityFlag.Monster)
                {
                    if (damage > attacked.Hitpoints)
                    {
                        attacker.Owner.IncreaseExperience(Calculate.CalculateExpBonus(attacker.Level, attacked.Level, Math.Min(damage, attacked.Hitpoints)), true);
                        if (spell != null)
                            attacker.Owner.IncreaseSpellExperience((uint)Calculate.CalculateExpBonus(attacker.Level, attacked.Level, Math.Min(damage, attacked.Hitpoints)), spell.ID);
                    }
                    else
                    {
                        attacker.Owner.IncreaseExperience(Calculate.CalculateExpBonus(attacker.Level, attacked.Level, damage), true);
                        if (spell != null)
                            attacker.Owner.IncreaseSpellExperience((uint)Calculate.CalculateExpBonus(attacker.Level, attacked.Level, damage), spell.ID);
                    }
                }
                else
                {
                    if (attacked.EntityFlag == EntityFlag.Player)
                    {
                        if (spell != null)
                            attacker.Owner.IncreaseSpellExperience((uint)Calculate.CalculateExpBonus(attacker.Level, attacked.Level, damage), spell.ID);
                    }
                }

            }
           

            if (attack.AttackType == Attack.Magic)
            {
                if (attacked.Hitpoints <= damage)
                {
                    if (attacker.Owner.QualifierGroup != null)
                        attacker.Owner.QualifierGroup.UpdateDamage(attacker.Owner, attacked.Hitpoints);
                    attacked.CauseOfDeathIsMagic = true;
                    if (attacker.MapID == 2060)
                    {
                        if (attacked.ContainsFlag2(Network.GamePackets.Update.Flags2.CaryingFlag))
                        {
                            attacked.RemoveFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                            attacker.AddFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                        }
                    }
                    attacked.Die(attacker);
                }
                else
                {
                    if (attacker.Owner.QualifierGroup != null)
                        attacker.Owner.QualifierGroup.UpdateDamage(attacker.Owner, damage);
                    attacked.Hitpoints -= damage;
                }
            }
            else
            {
                if (attacked.Hitpoints <= damage)
                {
                    if (attacked.EntityFlag == EntityFlag.Player)
                    {
                        if (attacker.Owner.QualifierGroup != null)
                            attacker.Owner.QualifierGroup.UpdateDamage(attacker.Owner, attacked.Hitpoints);
                        attacked.Owner.SendScreen(attack, true);
                        attacker.AttackPacket = null;
                        if (attacker.MapID == 2060)
                        {
                            if (attacked.ContainsFlag2(Network.GamePackets.Update.Flags2.CaryingFlag))
                            {
                                attacked.RemoveFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                                attacker.AddFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                            }
                        }
                    }
                    else
                    {
                        attacked.MonsterInfo.SendScreen(attack);
                    }
                    attacked.Die(attacker);
                }
                else
                {
                    attacked.Hitpoints -= damage;
                    if (attacked.EntityFlag == EntityFlag.Player)
                    {
                        if (attacker.Owner.QualifierGroup != null)
                            attacker.Owner.QualifierGroup.UpdateDamage(attacker.Owner, damage);
                        attacked.Owner.SendScreen(attack, true);
                    }
                    else
                        attacked.MonsterInfo.SendScreen(attack);
                    attacker.AttackPacket = attack;
                    attacker.AttackStamp = Time32.Now;
                }
            }
        }
        public static void ReceiveAttack(Game.Entity attacker, SobNpcSpawn attacked, Attack attack, uint damage, Database.SpellInformation spell)
        {

            if (attacker.EntityFlag == EntityFlag.Player)
            {
                
                if (damage > attacked.Hitpoints)
                {
                   // if (attacker.MapID == 1039)
                      //  attacker.Owner.IncreaseExperience(Math.Min(damage, attacked.Hitpoints), true);
                    if (spell != null)
                        attacker.Owner.IncreaseSpellExperience(Math.Min(damage, attacked.Hitpoints), spell.ID);
                }
                else
                {
                   // if (attacker.MapID == 1039)
                     //   attacker.Owner.IncreaseExperience(damage, true);
                    if (spell != null)
                        attacker.Owner.IncreaseSpellExperience(damage, spell.ID);
                }
                if (attacker.MapID == 1038)
                {
                    if (attacked.UID == 810)
                    {
                        if (Game.ConquerStructures.Society.GuildWar.PoleKeeper == attacker.Owner.Guild)
                            return;
                        if (attacked.Hitpoints <= damage)
                            attacked.Hitpoints = 0;
                        Game.ConquerStructures.Society.GuildWar.AddScore(damage, attacker.Owner.Guild);
                    }
                }
                if (attacker.MapID == 2060)
                {
                    if (attacked.UID == 10001)
                    {
                        if (attacked.Hitpoints <= damage)
                        {
                            attacked.Hitpoints = 0;
                            if (Game.Team.RScore > 0)
                            {
                                Game.Team.RScore -= 1;
                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("[CaptureTheFlag] RedFlag has Destroyed and RedTeam losed 1 Points and  Score Now for RedTeam:" + Game.Team.RScore + "", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            }
                        }
                    }
                }
                if (attacker.MapID == 2060)
                {
                    if (attacked.UID == 10002)
                    {
                        if (attacked.Hitpoints <= damage)
                        {
                            attacked.Hitpoints = 0;
                            if (Game.Team.BScore > 0)
                            {
                                Game.Team.BScore -= 1;
                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("[CaptureTheFlag] BlueFlag has Destroyed and BlueTeam losed 1 Points and  Score Now for BlueTeam:" + Game.Team.BScore + "", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            }
                        }
                    }
                }
                if (attacker.MapID == 2060)
                {
                    if (attacked.UID == 10003)
                    {
                        if (attacked.Hitpoints <= damage)
                        {
                            attacked.Hitpoints = 0;
                            if (Game.Team.WScore > 0)
                            {
                                Game.Team.WScore -= 1;
                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("[CaptureTheFlag] WhiteFlag has Destroyed and WhiteTeam losed 1 Points and  Score Now for WhiteTeam:" + Game.Team.WScore + "", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            }
                        }
                    }
                }
                if (attacker.MapID == 2060)
                {
                    if (attacked.UID == 10004)
                    {
                        if (attacked.Hitpoints <= damage)
                        {
                            attacked.Hitpoints = 0;
                            if (Game.Team.BLScore > 0)
                            {
                                Game.Team.BLScore -= 1;
                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("[CaptureTheFlag] BlackFlag has Destroyed and BlackTeam losed 1 Points and  Score Now for BlackTeam:" + Game.Team.BLScore + "", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            }
                        }
                    }
                }
                if (attacker.MapID == 2060)
                {
                    if (attacked.UID == 10011)
                    {
                        if (attacked.Hitpoints <= damage)
                            attacked.Hitpoints = 0;
                        attacker.AddFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                        Game.Team.RedCapture = true;
                    }
                }
                if (attacker.MapID == 2060)
                {
                    if (attacked.UID == 10012)
                    {
                        if (attacked.Hitpoints <= damage)
                            attacked.Hitpoints = 0;
                        attacker.AddFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                        Game.Team.BlueCapture = true;
                    }
                }
                if (attacker.MapID == 2060)
                {
                    if (attacked.UID == 10013)
                    {
                        if (attacked.Hitpoints <= damage)
                            attacked.Hitpoints = 0;
                        attacker.AddFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                        Game.Team.WhiteCapture = true;
                    }
                }
                if (attacker.MapID == 2060)
                {
                    if (attacked.UID == 10014)
                    {
                        if (attacked.Hitpoints <= damage)
                            attacked.Hitpoints = 0;
                        attacker.AddFlag2(Network.GamePackets.Update.Flags2.CaryingFlag);
                        Game.Team.BlackCapture = true;
                    }
                }
                if (attacker.MapID == 2071)
                {
                    if (attacked.UID == 811)
                    {
                        if (Game.ConquerStructures.Society.EliteGuildWar.PoleKeeper == attacker.Owner.Guild)
                            return;
                        if (attacked.Hitpoints <= damage)
                            attacked.Hitpoints = 0;
                        Game.ConquerStructures.Society.EliteGuildWar.AddScore(damage, attacker.Owner.Guild);
                    }
                }
                if (attacker.MapID == 1509)
                {
                    if (attacked.UID == 812)
                    {
                        if (Game.ClanWar.PoleKeeper == attacker.Myclan)
                            return;
                        if (attacked.Hitpoints <= damage)
                            attacked.Hitpoints = 0;
                        Game.ClanWar.AddScore(damage, attacker.Myclan);
                    }
                }
                if (attack.AttackType == Attack.Magic)
                {
                    if (attacked.Hitpoints <= damage)
                    {
                        attacked.Die(attacker);
                    }
                    else
                    {
                        attacked.Hitpoints -= damage;
                    }
                }
                else
                {
                    attacker.Owner.SendScreen(attack, true);
                    if (attacked.Hitpoints <= damage)
                    {
                        attacked.Die(attacker);
                    }
                    else
                    {
                        attacked.Hitpoints -= damage;
                        attacker.AttackPacket = attack;
                        attacker.AttackStamp = Time32.Now;
                    }
                }
            }
        }
        public static bool isArcherSkill(uint ID)
        {
            if (ID >= 8000 && ID <= 9875)
                return true;
            return false;
        }
        public static bool CanUseSpell(Database.SpellInformation spell, Client.GameState client)
        {
            if (client.WatchingGroup != null)
                return false;
            if (spell == null)
                return false;
            if (client.Entity.Mana < spell.UseMana)
                return false;
            if (client.Entity.Stamina < spell.UseStamina)
                return false;
            if (!client.AlternateEquipment)
            {
                if (spell.UseArrows > 0 && isArcherSkill(spell.ID))
                {
                    if (!client.Equipment.Free((byte)ConquerItem.LeftWeapon))
                    {
                        Interfaces.IConquerItem arrow = client.Equipment.TryGetItem(ConquerItem.LeftWeapon);
                        if (arrow.Durability <= spell.UseArrows)
                        {
                            return false;
                        }
                        return arrow.Durability >= spell.UseArrows;
                    }
                    return false;
                }
            }
            else
            {
                if (spell.UseArrows > 0 && isArcherSkill(spell.ID))
                {
                    if (!client.Equipment.Free((byte)ConquerItem.AltLeftHand))
                    {
                        Interfaces.IConquerItem arrow = client.Equipment.TryGetItem(ConquerItem.AltLeftHand);
                        if (arrow.Durability <= spell.UseArrows)
                        {
                            return false;
                        }
                        return arrow.Durability >= spell.UseArrows;
                    }
                    return false;
                }
            }
            if (spell.NeedXP == 1 && !client.Entity.ContainsFlag(Update.Flags.XPList) && !ServerBase.Constants.AllowSkillsonXp.Contains(spell.ID))
                return false;
            return true;
        }
        public static void PrepareSpell(Database.SpellInformation spell, Client.GameState client)
        {
            if (spell.NeedXP == 1)
                client.Entity.RemoveFlag(Update.Flags.XPList);
            if (client.Map.ID != 1039)
            {
                if (spell.UseMana > 0)
                    if (client.Entity.Mana >= spell.UseMana)
                        client.Entity.Mana -= spell.UseMana;
                if (spell.UseStamina > 0)
                    if (client.Entity.Stamina >= spell.UseStamina)
                        client.Entity.Stamina -= spell.UseStamina;
                if (spell.UseArrows > 0 && isArcherSkill(spell.ID))
                {
                    if (!client.AlternateEquipment)
                    {
                        if (!client.Equipment.Free((byte)ConquerItem.LeftWeapon))
                        {
                            Interfaces.IConquerItem arrow = client.Equipment.TryGetItem(ConquerItem.LeftWeapon);
                            arrow.Durability -= spell.UseArrows;
                            ItemUsage usage = new ItemUsage(true) { UID = arrow.UID, dwParam = arrow.Durability, ID = ItemUsage.UpdateDurability };
                            usage.Send(client);
                            if (arrow.Durability <= spell.UseArrows || arrow.Durability > 5000)
                            {
                                Network.PacketHandler.ReloadArrows(client.Equipment.TryGetItem(ConquerItem.LeftWeapon), client);
                            }
                        }
                    }
                    else
                    {
                        if (!client.Equipment.Free((byte)ConquerItem.AltLeftHand))
                        {
                            Interfaces.IConquerItem arrow = client.Equipment.TryGetItem(ConquerItem.AltLeftHand);
                            arrow.Durability -= spell.UseArrows;
                            ItemUsage usage = new ItemUsage(true) { UID = arrow.UID, dwParam = arrow.Durability, ID = ItemUsage.UpdateDurability };
                            usage.Send(client);
                            if (arrow.Durability <= spell.UseArrows || arrow.Durability > 5000)
                            {
                                Network.PacketHandler.ReloadArrows(client.Equipment.TryGetItem(ConquerItem.AltLeftHand), client);
                            }
                        }
                    }
                }
            }
        }
        public static bool CanAttack(Game.Entity attacker, SobNpcSpawn attacked, Database.SpellInformation spell)
        {
            if (attacker.MapID == 2060)
            {
                if (attacked.UID >= 10021 && attacked.UID <= 10024)
                {
                    return false;
                }
            }
            if (attacker.MapID == 2060)
            {
                if (attacked.UID == 10001 && attacker.Owner.CaptureR)
                {
                    return false;
                }
            }
            if (attacker.MapID == 2060)
            {
                if (attacked.UID == 10002 && attacker.Owner.CaptureB)
                {
                    return false;
                }
            }
            if (attacker.MapID == 2060)
            {
                if (attacked.UID == 10003 && attacker.Owner.CaptureW)
                {
                    return false;
                }
            }
            if (attacker.MapID == 2060)
            {
                if (attacked.UID == 10004 && attacker.Owner.CaptureBL)
                {
                    return false;
                }
            }
            if (attacker.MapID == 1038)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour >= Game.KimoEvents.GWEEndHour)
                {
                    if (attacked.UID == 810)
                    {
                        return false;
                    }
                }
                if (attacker.GuildID == 0 || !Game.ConquerStructures.Society.GuildWar.IsWar)
                {
                    if (attacked.UID == 810)
                    {
                        return false;
                    }
                }
                if (Game.ConquerStructures.Society.GuildWar.PoleKeeper != null)
                {
                    if (Game.ConquerStructures.Society.GuildWar.PoleKeeper == attacker.Owner.Guild)
                    {
                        if (attacked.UID == 810)
                        {
                            return false;
                        }
                    }
                    else if (attacked.UID == 516075 || attacked.UID == 516074)
                    {
                        if (Game.ConquerStructures.Society.GuildWar.PoleKeeper == attacker.Owner.Guild)
                        {
                            if (attacker.PKMode == Enums.PKMode.Team)
                                return false;
                        }
                    }
                }
            }
            if (attacker.MapID == 2071)
            {
                if (DateTime.Now.Hour >= 17 && DateTime.Now.Hour < 18)
                {
                    if (attacked.UID == 811)
                    {
                       // return false;
                    }
                }
                else
                {
                    if (attacked.UID == 811)
                    {
                        return false;
                    }
                }
                if (attacker.GuildID == 0 || !Game.ConquerStructures.Society.EliteGuildWar.IsWar)
                {
                    if (attacked.UID == 811)
                    {
                        return false;
                    }
                }
                if (Game.ConquerStructures.Society.EliteGuildWar.PoleKeeper != null)
                {
                    if (Game.ConquerStructures.Society.EliteGuildWar.PoleKeeper == attacker.Owner.Guild)
                    {
                        if (attacked.UID == 811)
                        {
                            return false;
                        }
                    }
                }
            }
            if (attacked.UID == 813)
            {
                return false;
            }
            if (attacked.UID == 814)
            {
                return false;
            }
            if (attacker.MapID == 1509)
            {
                if (DateTime.Now.Hour >= Game.KimoEvents.ClanEndHour)
                {
                    if (attacked.UID == 812)
                    {
                        return false;
                    }
                }
                if (attacker.ClanId == 0 || !Game.ClanWar.IsWar)
                {
                    if (attacked.UID == 812)
                    {
                        return false;
                    }
                }
                if (Game.ClanWar.PoleKeeper != null)
                {
                    if (Game.ClanWar.PoleKeeper == attacker.Myclan)
                    {
                        if (attacked.UID == 812)
                        {
                            return false;
                        }
                    }
                    
                }
            }
            if (attacker.MapID == 1039)
            {
                bool stake = true;
                if (attacked.Name.ToLower().Contains("crow"))
                    stake = false;

                ushort levelbase = (ushort)(attacked.Mesh / 10);
                if (stake)
                    levelbase -= 42;
                else
                    levelbase -= 43;

                byte level = (byte)(20 + (levelbase / 3) * 5);
                if (levelbase == 108 || levelbase == 109)
                    level = 125;
                if (attacker.Level >= level)
                    return true;
                else
                {
                    attacker.AttackPacket = null;
                    attacker.Owner.Send(ServerBase.Constants.DummyLevelTooHigh());
                    return false;
                }
            }
            return true;
        }
        public static bool CanAttack(Game.Entity attacker, Game.Entity attacked, Database.SpellInformation spell, bool melee)
        {
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (attacked.EntityFlag == EntityFlag.Player)
                {
                    if (attacker.MapID == 2060)
                    {
                        if (attacked.Owner.CaptureB && attacker.Owner.CaptureB)
                        {
                            return false;
                        }
                        if (attacked.Owner.CaptureW && attacker.Owner.CaptureW)
                        {
                            return false;
                        }
                        if (attacked.Owner.CaptureBL && attacker.Owner.CaptureBL)
                        {
                            return false;
                        }
                        if (attacked.Owner.CaptureR && attacker.Owner.CaptureR)
                        {
                            return false;
                        }
                    }
                }
            }
           
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (attacked.EntityFlag == EntityFlag.Monster)
                {
                    if (attacked.MonsterInfo.Name == "StigGuard")
                    {
                        return false;
                    }
                }
            }
            if (attacker.EntityFlag == EntityFlag.Monster)
            {
                if (attacked.EntityFlag == EntityFlag.Monster)
                {
                    if (attacked.MonsterInfo.Name == "StigGuard")
                    {
                        return false;
                    }
                }
            }
            if (attacker.EntityFlag == EntityFlag.Player)
            {
                if (attacked.EntityFlag == EntityFlag.Monster)
                {
                    if (attacked.MonsterInfo.Name == "RevGuard")
                    {
                        return false;
                    }
                }
            }
            if (attacker.EntityFlag == EntityFlag.Monster)
            {
                if (attacked.EntityFlag == EntityFlag.Monster)
                {
                    if (attacked.MonsterInfo.Name == "RevGuard")
                    {
                        return false;
                    }
                }
            }
            if (spell != null)
                if (spell.CanKill == 1 && attacker.EntityFlag == EntityFlag.Player && ServerBase.Constants.PKForbiddenMaps.Contains(attacker.Owner.Map.ID) && attacked.EntityFlag == EntityFlag.Player)
                    return false;
            if (attacker.EntityFlag == EntityFlag.Player)
                if (attacker.Owner.WatchingGroup != null)
                    return false;
            if (attacked == null)
                return false;
            if (attacked.Dead)
            {
                attacker.AttackPacket = null;
                return false;
            }
            if (attacked.EntityFlag == EntityFlag.Monster)
            {
                if (attacked.Companion)
                {
                    if (ServerBase.Constants.PKForbiddenMaps.Contains(attacker.Owner.Map.ID))
                    {
                        if (attacked.Owner == attacker.Owner)
                            return false;
                        if (attacker.PKMode != PhoenixProject.Game.Enums.PKMode.PK &&
                         attacker.PKMode != PhoenixProject.Game.Enums.PKMode.Team)
                            return false;
                        else
                        {
                            attacker.AddFlag(Network.GamePackets.Update.Flags.FlashingName);
                            attacker.FlashingNameStamp = Time32.Now;
                            attacker.FlashingNameTime = 10;

                            return true;
                        }
                    }
                }
                if (attacked.Name.Contains("Guard"))
                {
                    if (attacker.PKMode != PhoenixProject.Game.Enums.PKMode.PK &&
                    attacker.PKMode != PhoenixProject.Game.Enums.PKMode.Team)
                        return false;
                    else
                    {
                        attacker.AddFlag(Network.GamePackets.Update.Flags.FlashingName);
                        attacker.FlashingNameStamp = Time32.Now;
                        attacker.FlashingNameTime = 10;

                        return true;
                    }
                }
                else
                    return true;
            }
            else
            {
                if (attacked.EntityFlag == EntityFlag.Player)
                    if (!attacked.Owner.Attackable)
                        return false;
                if (attacker.EntityFlag == EntityFlag.Player)
                    if (attacker.Owner.WatchingGroup == null)
                        if (attacked.EntityFlag == EntityFlag.Player)
                            if (attacked.Owner.WatchingGroup != null)
                                return false;

                if (spell != null)
                {
                    if (spell.ID != 8001)
                    {
                        if (spell.OnlyGround == 1)
                            if (attacked.ContainsFlag(Update.Flags.Fly))
                                return false;
                        if (melee && attacked.ContainsFlag(Update.Flags.Fly))
                            return false;
                    }
                }
                if (spell != null)
                {
                    if (spell.ID == 6010)
                    {
                            if (attacked.ContainsFlag(Update.Flags.Fly))
                                return false;
                    }
                }
                if (spell != null)
                {
                    if (spell.ID == 10381)
                    {
                        if (attacked.ContainsFlag(Update.Flags.Fly))
                            return false;
                    }
                }
                if (spell != null)
                {
                    if (spell.ID == 6000)
                    {
                        if (attacked.ContainsFlag(Update.Flags.Fly))
                            return false;
                    }
                }
                if (spell != null)
                {
                    if (spell.ID == 5030)
                    {
                        if (attacked.ContainsFlag(Update.Flags.Fly))
                            return false;
                    }
                }
                if (spell == null)
                {
                        if (attacked.ContainsFlag(Update.Flags.Fly))
                            return false;
                }

                if (attacked.ContainsFlag(Network.GamePackets.Update.Flags2.IceBlock))
                {
                    return false;
                }
                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameMaster)
                {
                    return false;
                }
                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.ProjectManager)
                {
                    return false;
                }
                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameHelper)
                {
                    return false;
                }
                if (ServerBase.Constants.PKForbiddenMaps.Contains(attacker.Owner.Map.ID))
                {
                    if (attacker.PKMode == PhoenixProject.Game.Enums.PKMode.PK ||
                        attacker.PKMode == PhoenixProject.Game.Enums.PKMode.Team || (spell != null && spell.CanKill == 1))
                    {
                        attacker.Owner.Send(ServerBase.Constants.PKForbidden);
                        attacker.AttackPacket = null;
                    }
                    return false;
                }
                if (attacker.MapID >= 1000000)
                {
                    if (attacker.PKMode == PhoenixProject.Game.Enums.PKMode.PK ||
                        attacker.PKMode == PhoenixProject.Game.Enums.PKMode.Team || (spell != null && spell.CanKill == 1))
                    {
                        attacker.Owner.Send(ServerBase.Constants.PKForbidden);
                        attacker.AttackPacket = null;
                    }
                    return false;
                }
                if (attacker.PKMode == PhoenixProject.Game.Enums.PKMode.Capture)
                {
                    if (attacked.ContainsFlag(Update.Flags.FlashingName) || attacked.PKPoints > 99)
                    {
                        return true;
                    }
                }
                if (attacker.PKMode == PhoenixProject.Game.Enums.PKMode.Revenge)
                {
                    if (attacker.Owner.Enemy.ContainsKey(attacked.UID))
                    {
                        return true;
                    }
                }
                if (attacker.PKMode == PhoenixProject.Game.Enums.PKMode.Peace)
                {
                    if (attacker.PKMode == PhoenixProject.Game.Enums.PKMode.Peace)
                    {
                        return false;
                    }
                }
                if (attacker.Name == attacked.Name)
                {
                    if (attacker.Name == attacked.Name)
                    {
                        return false;
                    }
                }
                if (attacker.PKMode == PhoenixProject.Game.Enums.PKMode.Team)
                {
                    if (attacker.Owner.Team != null)
                        if (attacker.Owner.Team.IsTeammate(attacked.UID))
                        {
                            attacker.AttackPacket = null;
                            return false;
                        }
                    if (attacker.Owner.Guild != null)
                    {
                        if (attacker.GuildID != 0)
                        {
                            if (attacked.GuildID != 0)
                            {
                                if (attacker.GuildID == attacked.GuildID)
                                {
                                    attacker.AttackPacket = null;
                                    return false;
                                }
                            }
                        }
                    }
                    if (attacker.ClanId != 0)
                    {
                        if (attacker.ClanId != 0)
                        {
                            if (attacked.ClanId != 0)
                            {
                                if (attacker.ClanId == attacked.ClanId)
                                {
                                    attacker.AttackPacket = null;
                                    return false;
                                }
                            }
                        }
                    }
                    if (attacker.Owner.Friends.ContainsKey(attacked.UID))
                    {
                        attacker.AttackPacket = null;
                        return false;
                    }
                    if (attacker.Owner.Guild != null)
                    {
                        if (attacker.GuildID != 0)
                        {
                            if (attacked.GuildID != 0)
                            {
                                if (attacker.Owner.Guild.Ally.ContainsKey(attacked.GuildID))
                                {
                                    attacker.AttackPacket = null;
                                    return false;
                                }
                            }
                        }
                    }
                    if (attacked.UID == attacker.UID)
                        return false;
                    if (attacker.Myclan != null)
                    {
                        if (attacker.ClanId != 0)
                        {
                            if (attacked.ClanId != 0)
                            {
                                if (attacker.Myclan.Allies.ContainsKey(attacked.ClanId))
                                {
                                    attacker.AttackPacket = null;
                                    return false;
                                }
                            }
                        }
                    }
                }

                if (spell != null)
                    if (spell.OnlyGround == 1)
                        if (attacked.ContainsFlag(Update.Flags.Fly))
                            return false;

                if (spell != null)
                    if (spell.CanKill == 0)
                        return true;

                if (attacker.PKMode != PhoenixProject.Game.Enums.PKMode.PK &&
                    attacker.PKMode != PhoenixProject.Game.Enums.PKMode.Team && attacked.PKPoints < 99)
                {
                    attacker.AttackPacket = null;
                    return false;
                }
                else
                {
                    if (!attacked.ContainsFlag(Update.Flags.FlashingName))
                    {
                        if (!attacked.ContainsFlag(Update.Flags.BlackName))
                        {
                            if (ServerBase.Constants.PKFreeMaps.Contains(attacker.MapID))
                                return true;
                            if (ServerBase.Constants.Damage1Map.Contains(attacker.MapID))
                                return true;
                            if (attacker.Owner.Map.BaseID == 700)
                                return true;
                            attacker.AddFlag(Network.GamePackets.Update.Flags.FlashingName);
                            attacker.FlashingNameStamp = Time32.Now;
                            attacker.FlashingNameTime = 10;
                        }
                    }
                }
                return true;
            }
        }
        public static void CheckForExtraWeaponPowers(Client.GameState client, Entity attacked)
        {
            #region Right Hand
            if (client.Equipment.TryGetItem(ConquerItem.RightWeapon) != null)
            {
                if (client.Equipment.TryGetItem(ConquerItem.RightWeapon).ID != 0)
                {
                    var Item = client.Equipment.TryGetItem(ConquerItem.RightWeapon);
                    if (Item.Effect != Enums.ItemEffect.None)
                    {
                        if (ServerBase.Kernel.Rate(30))
                        {
                            switch (Item.Effect)
                            {
                                case Enums.ItemEffect.HP:
                                    {
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1175;
                                        spellUse.SpellLevel = 4;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 300);
                                        uint damage = Math.Min(300, client.Entity.MaxHitpoints - client.Entity.Hitpoints);
                                        client.Entity.Hitpoints += damage;
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.MP:
                                    {
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1175;
                                        spellUse.SpellLevel = 2;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 300);
                                        ushort damage = (ushort)Math.Min(300, client.Entity.MaxMana - client.Entity.Mana);
                                        client.Entity.Mana += damage;
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.Shield:
                                    {
                                        if (client.Entity.ContainsFlag(Update.Flags.MagicShield))
                                            return;
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1020;
                                        spellUse.SpellLevel = 0;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 120);
                                        client.Entity.ShieldTime = 0;
                                        client.Entity.ShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldTime = 0;

                                        client.Entity.AddFlag(Update.Flags.MagicShield);
                                        client.Entity.MagicShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldIncrease = 2;
                                        client.Entity.MagicShieldTime = 120;
                                        if (client.Entity.EntityFlag == EntityFlag.Player)
                                            client.Send(ServerBase.Constants.Shield(2, 120));
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.Poison:
                                    {
                                        if (attacked != null)
                                        {
                                            if (attacked.EntityFlag == EntityFlag.Player)
                                            {
                                                if (attacked.ContainsFlag(Network.GamePackets.Update.Flags2.IceBlock))
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameMaster)
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.ProjectManager)
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameHelper)
                                                {
                                                    return;
                                                }
                                                if (ServerBase.Constants.PKForbiddenMaps.Contains(client.Entity.MapID))
                                                return;
                                            }
                                            
                                            if (client.Map.BaseID == 700)
                                                return;
                                            if (attacked.UID == client.Entity.UID)
                                                return;
                                            if (attacked.ToxicFogLeft > 0)
                                                return;
                                            Attack attack = new Attack(true);
                                            attack.Effect1 = Attack.AttackEffects1.None;
                                            uint damages = Calculate.Melee(client.Entity, attacked, ref attack);
                                            damages = damages / 2;
                                            //uint damage = Math.Min(1, client.Entity.MinAttack);
                                            SpellUse spellUse = new SpellUse(true);
                                            spellUse.SpellID = 5040;
                                            spellUse.Attacker = attacked.UID;
                                            spellUse.SpellLevel = 9;
                                            spellUse.X = attacked.X;
                                            spellUse.Y = attacked.Y;
                                            spellUse.Targets.Add(attacked.UID, damages);
                                            spellUse.Targets[attacked.UID].Hit = true;
                                            attacked.ToxicFogStamp = Time32.Now;
                                            attacked.ToxicFogLeft = 10;
                                            attacked.ToxicFogPercent = 0.05F;
                                            client.SendScreen(spellUse, true);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Left Hand
            if (client.Equipment.TryGetItem(ConquerItem.LeftWeapon) != null)
            {
                if (client.Equipment.TryGetItem(ConquerItem.LeftWeapon).ID != 0)
                {
                    var Item = client.Equipment.TryGetItem(ConquerItem.LeftWeapon);
                    if (Item.Effect != Enums.ItemEffect.None)
                    {
                        if (ServerBase.Kernel.Rate(30))
                        {
                            switch (Item.Effect)
                            {
                                case Enums.ItemEffect.HP:
                                    {
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1175;
                                        spellUse.SpellLevel = 4;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 300);
                                        uint damage = Math.Min(300, client.Entity.MaxHitpoints - client.Entity.Hitpoints);
                                        client.Entity.Hitpoints += damage;
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.MP:
                                    {
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1175;
                                        spellUse.SpellLevel = 2;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 300);
                                        ushort damage = (ushort)Math.Min(300, client.Entity.MaxMana - client.Entity.Mana);
                                        client.Entity.Mana += damage;
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.Shield:
                                    {
                                        if (client.Entity.ContainsFlag(Update.Flags.MagicShield))
                                            return;
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1020;
                                        spellUse.SpellLevel = 0;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 120);
                                        client.Entity.ShieldTime = 0;
                                        client.Entity.ShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldTime = 0;

                                        client.Entity.AddFlag(Update.Flags.MagicShield);
                                        client.Entity.MagicShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldIncrease = 2;
                                        client.Entity.MagicShieldTime = 120;
                                        if (client.Entity.EntityFlag == EntityFlag.Player)
                                            client.Send(ServerBase.Constants.Shield(2, 120));
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.Poison:
                                    {
                                        if (attacked != null)
                                        {
                                            if (attacked.EntityFlag == EntityFlag.Player)
                                            {
                                                if (attacked.ContainsFlag(Network.GamePackets.Update.Flags2.IceBlock))
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameMaster)
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.ProjectManager)
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameHelper)
                                                {
                                                    return;
                                                }
                                                if (ServerBase.Constants.PKForbiddenMaps.Contains(client.Entity.MapID))
                                                {
                                                    return;
                                                }
                                            }
                                            if (attacked.UID == client.Entity.UID)
                                                return;
                                            
                                            if (client.Map.BaseID == 700)
                                                return;
                                            if (attacked.ToxicFogLeft > 0)
                                                return;
                                            Attack attack = new Attack(true);
                                            attack.Effect1 = Attack.AttackEffects1.None;
                                            uint damages = Calculate.Melee(client.Entity, attacked, ref attack);
                                            damages = damages / 2;
                                            //uint damage = Math.Min(1, client.Entity.MinAttack);
                                            SpellUse spellUse = new SpellUse(true);
                                            spellUse.SpellID = 5040;
                                            spellUse.Attacker = attacked.UID;
                                            spellUse.SpellLevel = 9;
                                            spellUse.X = attacked.X;
                                            spellUse.Y = attacked.Y;
                                            spellUse.Targets.Add(attacked.UID, damages);
                                            spellUse.Targets[attacked.UID].Hit = true;
                                            attacked.ToxicFogStamp = Time32.Now;
                                            attacked.ToxicFogLeft = 10;
                                            attacked.ToxicFogPercent = 0.05F;
                                            client.SendScreen(spellUse, true);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            #endregion
        }
        public static void CheckForExtraWeaponPowers2(Client.GameState client, Entity attacked)
        {
            #region Right Hand
            if (client.Equipment.TryGetItem(ConquerItem.AltRightHand) != null)
            {
                if (client.Equipment.TryGetItem(ConquerItem.AltRightHand).ID != 0)
                {
                    var Item = client.Equipment.TryGetItem(ConquerItem.AltRightHand);
                    if (Item.Effect != Enums.ItemEffect.None)
                    {
                        if (ServerBase.Kernel.Rate(30))
                        {
                            switch (Item.Effect)
                            {
                                case Enums.ItemEffect.HP:
                                    {
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1175;
                                        spellUse.SpellLevel = 4;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 300);
                                        uint damage = Math.Min(300, client.Entity.MaxHitpoints - client.Entity.Hitpoints);
                                        client.Entity.Hitpoints += damage;
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.MP:
                                    {
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1175;
                                        spellUse.SpellLevel = 2;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 300);
                                        ushort damage = (ushort)Math.Min(300, client.Entity.MaxMana - client.Entity.Mana);
                                        client.Entity.Mana += damage;
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.Shield:
                                    {
                                        if (client.Entity.ContainsFlag(Update.Flags.MagicShield))
                                            return;
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1020;
                                        spellUse.SpellLevel = 0;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 120);
                                        client.Entity.ShieldTime = 0;
                                        client.Entity.ShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldTime = 0;

                                        client.Entity.AddFlag(Update.Flags.MagicShield);
                                        client.Entity.MagicShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldIncrease = 2;
                                        client.Entity.MagicShieldTime = 120;
                                        if (client.Entity.EntityFlag == EntityFlag.Player)
                                            client.Send(ServerBase.Constants.Shield(2, 120));
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.Poison:
                                    {
                                        if (attacked != null)
                                        {
                                            if (attacked.EntityFlag == EntityFlag.Player)
                                            {
                                                if (attacked.ContainsFlag(Network.GamePackets.Update.Flags2.IceBlock))
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameMaster)
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.ProjectManager)
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameHelper)
                                                {
                                                    return;
                                                }
                                                if (ServerBase.Constants.PKForbiddenMaps.Contains(client.Entity.MapID))
                                                    return;
                                            }

                                            if (client.Map.BaseID == 700)
                                                return;
                                            if (attacked.UID == client.Entity.UID)
                                                return;
                                            if (attacked.ToxicFogLeft > 0)
                                                return;
                                            Attack attack = new Attack(true);
                                            attack.Effect1 = Attack.AttackEffects1.None;
                                            uint damages = Calculate.Melee(client.Entity, attacked, ref attack);
                                            damages = damages / 2;
                                            //uint damage = Math.Min(1, client.Entity.MinAttack);
                                            SpellUse spellUse = new SpellUse(true);
                                            spellUse.SpellID = 5040;
                                            spellUse.Attacker = attacked.UID;
                                            spellUse.SpellLevel = 9;
                                            spellUse.X = attacked.X;
                                            spellUse.Y = attacked.Y;
                                            spellUse.Targets.Add(attacked.UID, damages);
                                            spellUse.Targets[attacked.UID].Hit = true;
                                            attacked.ToxicFogStamp = Time32.Now;
                                            attacked.ToxicFogLeft = 10;
                                            attacked.ToxicFogPercent = 0.05F;
                                            client.SendScreen(spellUse, true);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            #endregion
            #region Left Hand
            if (client.Equipment.TryGetItem(ConquerItem.AltLeftHand) != null)
            {
                if (client.Equipment.TryGetItem(ConquerItem.AltLeftHand).ID != 0)
                {
                    var Item = client.Equipment.TryGetItem(ConquerItem.AltLeftHand);
                    if (Item.Effect != Enums.ItemEffect.None)
                    {
                        if (ServerBase.Kernel.Rate(30))
                        {
                            switch (Item.Effect)
                            {
                                case Enums.ItemEffect.HP:
                                    {
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1175;
                                        spellUse.SpellLevel = 4;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 300);
                                        uint damage = Math.Min(300, client.Entity.MaxHitpoints - client.Entity.Hitpoints);
                                        client.Entity.Hitpoints += damage;
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.MP:
                                    {
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1175;
                                        spellUse.SpellLevel = 2;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 300);
                                        ushort damage = (ushort)Math.Min(300, client.Entity.MaxMana - client.Entity.Mana);
                                        client.Entity.Mana += damage;
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.Shield:
                                    {
                                        if (client.Entity.ContainsFlag(Update.Flags.MagicShield))
                                            return;
                                        SpellUse spellUse = new SpellUse(true);
                                        spellUse.SpellID = 1020;
                                        spellUse.SpellLevel = 0;
                                        spellUse.X = client.Entity.X;
                                        spellUse.Y = client.Entity.Y;
                                        spellUse.Targets.Add(client.Entity.UID, 120);
                                        client.Entity.ShieldTime = 0;
                                        client.Entity.ShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldTime = 0;

                                        client.Entity.AddFlag(Update.Flags.MagicShield);
                                        client.Entity.MagicShieldStamp = Time32.Now;
                                        client.Entity.MagicShieldIncrease = 2;
                                        client.Entity.MagicShieldTime = 120;
                                        if (client.Entity.EntityFlag == EntityFlag.Player)
                                            client.Send(ServerBase.Constants.Shield(2, 120));
                                        client.SendScreen(spellUse, true);
                                        break;
                                    }
                                case Enums.ItemEffect.Poison:
                                    {
                                        if (attacked != null)
                                        {
                                            if (attacked.EntityFlag == EntityFlag.Player)
                                            {
                                                if (attacked.ContainsFlag(Network.GamePackets.Update.Flags2.IceBlock))
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameMaster)
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.ProjectManager)
                                                {
                                                    return;
                                                }
                                                if (attacked.Owner.Account.State == PhoenixProject.Database.AccountTable.AccountState.GameHelper)
                                                {
                                                    return;
                                                }
                                                if (ServerBase.Constants.PKForbiddenMaps.Contains(client.Entity.MapID))
                                                {
                                                    return;
                                                }
                                            }
                                            if (attacked.UID == client.Entity.UID)
                                                return;

                                            if (client.Map.BaseID == 700)
                                                return;
                                            if (attacked.ToxicFogLeft > 0)
                                                return;
                                            Attack attack = new Attack(true);
                                            attack.Effect1 = Attack.AttackEffects1.None;
                                            uint damages = Calculate.Melee(client.Entity, attacked, ref attack);
                                            damages = damages / 2;
                                            //uint damage = Math.Min(1, client.Entity.MinAttack);
                                            SpellUse spellUse = new SpellUse(true);
                                            spellUse.SpellID = 5040;
                                            spellUse.Attacker = attacked.UID;
                                            spellUse.SpellLevel = 9;
                                            spellUse.X = attacked.X;
                                            spellUse.Y = attacked.Y;
                                            spellUse.Targets.Add(attacked.UID, damages);
                                            spellUse.Targets[attacked.UID].Hit = true;
                                            attacked.ToxicFogStamp = Time32.Now;
                                            attacked.ToxicFogLeft = 10;
                                            attacked.ToxicFogPercent = 0.05F;
                                            client.SendScreen(spellUse, true);
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            #endregion
        }
        public void CheckForSuperGems(Client.GameState client)
        {
            for (uint i = 1; i < 12; i++)
            {
                if (i != 7)
                {
                    Interfaces.IConquerItem item = client.Equipment.TryGetItem(i);
                    if (item != null && item.ID != 0)
                    {
                        if (item.SocketOne != 0)
                        {
                            if (item.SocketOne == Enums.Gem.SuperPhoenixGem)
                            {
                                if (ServerBase.Kernel.Rate(3)) //this is where your chances when to display the phoenix gem effect
                                {
                                    _String str = new _String(true);
                                    str.UID = attacker.UID;
                                    str.TextsCount = 1;
                                    str.Type = _String.Effect;
                                    str.Texts.Add("phoenix");
                                    attacker.Owner.SendScreen(str, true);
                                }
                            }
                            if (item.SocketOne == Enums.Gem.SuperDragonGem)
                            {
                                if (ServerBase.Kernel.Rate(3)) //this is where your chances when to display the phoenix gem effect
                                {
                                    _String str = new _String(true);
                                    str.UID = attacker.UID;
                                    str.TextsCount = 1;
                                    str.Type = _String.Effect;
                                    str.Texts.Add("dragon");
                                    attacker.Owner.SendScreen(str, true);
                                }
                            }
                        }
                    }
                }
            }
        } 
    }
}
