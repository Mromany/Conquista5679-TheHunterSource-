﻿using System;
using System.Collections.Generic;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Network;
using System.IO;
using System.Text;
using System.Linq;
using PhoenixProject.Game.Features;
using PhoenixProject.ServerBase;
namespace PhoenixProject.Game.ConquerStructures.Society
{
    
    public enum GuildRank
    {
        Agent = 590,
        Aide = 0x25a,
        ArsenalAgent = 0x254,
        ArsFollower = 0x1f0,
        ASupervisor = 0x358,
        CPAgent = 0x255,
        CPFollower = 0x1f1,
        CPSupervisor = 0x359,
        DeputyLeader = 990,
        DeputySteward = 650,
        DLeaderAide = 0x263,
        DLeaderSpouse = 620,
        Follower = 490,
        GSupervisor = 0x356,
        GuideAgent = 0x252,
        GuideFollower = 0x1ee,
        GuildLeader = 0x3e8,
        HDeputyLeader = 980,
        HonoraryManager = 880,
        HonorarySteward = 680,
        HonorarySuperv = 840,
        LeaderSpouse = 920,
        LilyAgent = 0x24f,
        LilyFollower = 0x1eb,
        LilySupervisor = 0x353,
        LSpouseAide = 610,
        Manager = 890,
        ManagerAide = 510,
        ManagerSpouse = 520,
        Member = 200,
        None = 0,
        OrchidAgent = 0x256,
        OrchidFollower = 0x1f2,
        OSupervisor = 0x35a,
        PKAgent = 0x251,
        PKFollower = 0x1ed,
        PKSupervisor = 0x355,
        RoseAgent = 0x250,
        RoseFollower = 0x1ec,
        RoseSupervisor = 0x354,
        SeniorMember = 210,
        SilverAgent = 0x253,
        SilverFollower = 0x1ef,
        SSupervisor = 0x357,
        Steward = 690,
        StewardSpouse = 420,
        Supervisor = 850,
        SupervisorAide = 0x1ff,
        SupervSpouse = 0x209,
        TSupervisor = 0x35b,
        TulipAgent = 0x257,
        TulipFollower = 0x1f3
    }
    public class Guild
    {
        public static ServerBase.Counter GuildCounter;
        public ArsenalPacket A_Packet;
        public PhoenixProject.Game.Features.Arsenal Arsenal;
 
        public static void GuildProfile(byte[] Packet, Client.GameState client)
        {
            GuildProfilePacket p = new GuildProfilePacket();
            p.Deserialize(Packet);
            p.Silver = 0;
            p.Pk = client.Entity.PKPoints;
            p.Cps = 0;
            p.Guide = 0;
            p.Arsenal = 0;
            p.Rose = 0;
            p.Lily = 0;
            p.Orchid = 0;
            p.Tulip = 0;
            p.HistorySilvers = 0;
            p.HistoryCps = 0;
            p.HistoryGuide = 0;
            p.HistoryPk = 0;
            client.Send(Packet);
        }

        public class Member : Interfaces.IKnownPerson
        {
            public Member(uint GuildID)
            {
                this.GuildID = GuildID;
            }

            public uint ID
            {
                get;
                set;
            }
            public string Name
            {
                get;
                set;
            }
            public bool IsOnline
            {
                get
                {
                    return ServerBase.Kernel.GamePool.ContainsKey(ID);
                }
            }
            public Client.GameState Client
            {
                get
                {
                    return ServerBase.Kernel.GamePool[ID];
                }
            }
            public ulong SilverDonation
            {
                get;
                set;
            }
            public ulong ConquerPointDonation
            {
                get;
                set;
            }
            public uint GuildID
            {
                get;
                set;
            }
            public Guild Guild
            {
                get
                {
                    return ServerBase.Kernel.Guilds[GuildID];
                }
            }
            public Enums.GuildMemberRank Rank
            {
                get;
                set;
            }
            public byte Level
            {
                get;
                set;
            }
            public NobilityRank NobilityRank
            {
                get;
                set;
            }
            public byte Gender
            {
                get;
                set;
            }
        }

        private byte[] Buffer;

        public uint WarScore;

        public bool PoleKeeper
        {
            get
            {
                return GuildWar.Pole.Name == Name;
            }
        }
        public bool PoleKeeper2
        {
            get
            {
                return EliteGuildWar.Poles.Name == Name;
            }
        }
        public bool PoleKeeper3
        {
            get
            {
                return Game.ClanWar.ClanFlag.Name == Name;
            }
        }

        public Guild(string leadername)
        {
            Buffer = new byte[92 + 8];

            LeaderName = leadername;
            Writer.WriteUInt16(92, 0, Buffer);
            Writer.WriteUInt16(1106, 2, Buffer);
            Buffer[48] = 0x2;
            Buffer[49] = 0x1;
            Buffer[60] = Level;
            Buffer[75] = 0x1;
            Buffer[87] = 0x20;
            Members = new SafeDictionary<uint, Member>();
            Ally = new SafeDictionary<uint, Guild>();
            Enemy = new SafeDictionary<uint, Guild>();
        }

        public uint ID
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { Writer.WriteUInt32(value, 4, Buffer); }
        }

        public ulong SilverFund
        {
            get { return BitConverter.ToUInt64(Buffer, 12); }
            set { Writer.WriteUInt64(value, 12, Buffer); }
        }

        public uint ConquerPointFund
        {
            get { return BitConverter.ToUInt32(Buffer, 20); }
            set { Writer.WriteUInt32(value, 20, Buffer); }
        }

        public uint MemberCount
        {
            get { return BitConverter.ToUInt32(Buffer, 24); }
            set { Writer.WriteUInt32(value, 24, Buffer); }
        }

        public uint DeputyLeaderCount;

        public byte Level
        {
            get
            {
                if (Losts == 0)
                    return Buffer[60];
                else
                    return 0;
            }
            set
            {
                Buffer[60] = 0;
                if (Losts == 0)
                    Buffer[60] = (byte)(Math.Min(Wins, 100));
            }
        }

        public string Name;

        public SafeDictionary<uint, Member> Members;
        public SafeDictionary<uint, Guild> Ally, Enemy;
       // private GuildRank _HeroRank;

       // public Arsenals Arsenal;
        public uint Wins;
        public uint Losts;
        public uint cp_donaion = 0;
        public uint money_donation = 0;
        public uint honor_donation = 0;
        public uint pkp_donation = 0;
        public uint rose_donation = 0;
        public uint tuil_donation = 0;
        public uint orchid_donation = 0;
        public uint lilies_donation = 0;

        public string Bulletin;

        public Member Leader;
        private string leaderName;
        public string LeaderName
        {
            get
            {
                return leaderName;
            }
            set
            {
                leaderName = value;
                Writer.WriteString(value, 32, Buffer);
            }
        }
        public static Boolean CheckNameExist(String Name)
        {
            foreach (Guild guilds in Kernel.Guilds.Values)
            {
                if (guilds.Name == Name)
                    return true;
            }
            return false;
        }
        public bool Create(string name)
        {
            if (name.Length < 16)
            {
                Name = name;
                SilverFund = 500000;
                Members.Add(Leader.ID, Leader);
                try
                {
                    Database.GuildTable.Create(this);
                }
                catch { return false; }
                ServerBase.Kernel.Guilds.Add(ID, this);
                Message message = null;
                message = new Message("Congratulations, " + leaderName + " has created guild " + name + " Succesfully!", System.Drawing.Color.White, Message.World);
                foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
                {
                    client.Send(message);
                }
                return true;
            }
            return false;
        }

        public void AddMember(Client.GameState client)
        {
            if (client.AsMember == null && client.Guild == null)
            {
                client.AsMember = new Member(ID)
                {
                    ID = client.Entity.UID,
                    Level = client.Entity.Level,
                    Name = client.Entity.Name,
                    Rank = PhoenixProject.Game.Enums.GuildMemberRank.Member
                };
                if (Nobility.Board.ContainsKey(client.Entity.UID))
                {
                    client.AsMember.Gender = Nobility.Board[client.Entity.UID].Gender;
                    client.AsMember.NobilityRank = Nobility.Board[client.Entity.UID].Rank;
                }
                MemberCount++;
                client.Guild = this;
                client.Entity.GuildID = (ushort)client.Guild.ID;
                client.Entity.GuildRank = (ushort)client.AsMember.Rank;
                Database.EntityTable.UpdateGuildID(client);
                Database.EntityTable.UpdateGuildRank(client);
                Members.Add(client.Entity.UID, client.AsMember);
                SendGuild(client);
                client.Screen.FullWipe();
                client.Screen.Reload(null);
                SendGuildMessage(new Message(client.AsMember.Name + " has joined our guild.", System.Drawing.Color.Black, Message.Guild));
            }
        }
        public void SendMembers(Client.GameState client, ushort page)
        {
            MemoryStream strm = new MemoryStream();
            BinaryWriter wtr = new BinaryWriter(strm);
            wtr.Write((ushort)0);
            wtr.Write((ushort)2102);
            wtr.Write((uint)0);
            wtr.Write((uint)page);
            int left = (int)MemberCount - page;
            if (left > 12)
                left = 12;
            if (left < 0)
                left = 0;
            wtr.Write((uint)left);
            int count = 0;
            int maxmem = page + 12;
            int minmem = page;
            List<Member> online = new List<Member>(250);
            List<Member> offline = new List<Member>(250);
            foreach (Member member in Members.Values)
            {
                if (member.IsOnline)
                    online.Add(member);
                else
                    offline.Add(member);
            }
            var unite = online.Union<Member>(offline);
            foreach (Member member in unite)
            {
                if (count >= minmem && count < maxmem)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (i < member.Name.Length)
                        {
                            wtr.Write((byte)member.Name[i]);
                        }
                        else
                            wtr.Write((byte)0);
                    }
                    wtr.Write((uint)member.NobilityRank);
                    wtr.Write((uint)(member.Gender + 1));
                    wtr.Write((uint)member.Level);
                    wtr.Write((uint)member.Rank);
                    wtr.Write((uint)0);
                    wtr.Write((uint)member.SilverDonation);
                    wtr.Write((uint)(member.IsOnline ? 1 : 0));
                    wtr.Write((uint)0);
                }
                count++;
            }
            foreach (Member member in Members.Values)
            {
                if (count >= minmem && count < maxmem)
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (i < member.Name.Length)
                        {
                            wtr.Write((byte)member.Name[i]);
                        }
                        else
                            wtr.Write((byte)0);
                    }
                    wtr.Write((uint)member.NobilityRank);
                    wtr.Write((uint)(member.Gender + 1));
                    wtr.Write((uint)member.Level);
                    wtr.Write((uint)member.Rank);
                    wtr.Write((uint)0);
                    wtr.Write((uint)member.SilverDonation);
                    wtr.Write((uint)(member.IsOnline ? 1 : 0));
                    wtr.Write((uint)0);
                }
                count++;
            }
            int packetlength = (int)strm.Length;
            strm.Position = 0;
            wtr.Write((ushort)packetlength);
            strm.Position = strm.Length;
            wtr.Write(ASCIIEncoding.ASCII.GetBytes("TQServer"));
            strm.Position = 0;
            byte[] buf = new byte[strm.Length];
            strm.Read(buf, 0, buf.Length);
            wtr.Close();
            strm.Close();
            ////wtr.Dispose();
            ////strm.Dispose();
            client.Send(buf);
        }
        public void SendGuildMessage(Interfaces.IPacket message)
        {
            foreach (Member member in Members.Values)
            {
                if (member.IsOnline)
                {
                    member.Client.Send(message);
                }
            }
        }
        public Member GetMemberByName(string membername)
        {
            foreach (Member member in Members.Values)
            {
                if (member.Name == membername)
                {
                    return member;
                }
            }
            return null;
        }
        public void ExpelMember(string membername, bool ownquit)
        {
            Member member = GetMemberByName(membername);
            if (member != null)
            {
                if (ownquit)
                    SendGuildMessage(new Message(member.Name + " has quit our guild.", System.Drawing.Color.Black, Message.Guild));

                else
                    SendGuildMessage(new Message(member.Name + " have been expelled from our guild.", System.Drawing.Color.Black, Message.Guild));
                uint uid = member.ID;
                if (member.Rank == Enums.GuildMemberRank.DeputyLeader)
                    DeputyLeaderCount--;
                if (member.IsOnline)
                {
                    GuildCommand command = new GuildCommand(true);
                    command.Type = GuildCommand.Disband;
                    command.dwParam = ID;
                    member.Client.Send(command);
                    member.Client.AsMember = null;
                    member.Client.Guild = null;
                    member.Client.Entity.GuildID = (ushort)0;
                    member.Client.Entity.GuildRank = (ushort)0;
                    member.Client.Screen.FullWipe();
                    member.Client.Screen.Reload(null);
                }
                else
                {
                    member.GuildID = 0;
                }
                if (member.IsOnline == true)
                {
                    foreach (Interfaces.IConquerItem Equi in member.Client.Inventory.Objects)
                    {
                        if (Equi != null)
                        {
                            if (Equi.Inscribed)
                            {
                                Equi.Inscribed = false;
                                Equi.Send(member.Client);
                            }
                        }
                    }
                    foreach (Interfaces.IConquerItem Equi in member.Client.Equipment.Objects)
                    {
                        if (Equi != null)
                        {
                            if (Equi.Inscribed)
                            {
                                Equi.Inscribed = false;
                                Equi.Send(member.Client);
                            }
                        }
                    }
                }
                MemberCount--;
                Database.ConquerItemTable.UpdateInscre3(uid);
                Members.Remove(uid);
            }
        }

        public void Disband()
        {
            var members = Members.Values.ToArray();
            foreach (Member member in members)
            {
                uint uid = member.ID;
                if (member.IsOnline)
                {
                    GuildCommand command = new GuildCommand(true);
                    command.Type = GuildCommand.Disband;
                    command.dwParam = ID;
                    member.Client.Entity.GuildID = 0;
                    member.Client.Entity.GuildRank = 0;
                    member.Client.Send(command);
                    member.Client.Screen.FullWipe();
                    member.Client.Screen.Reload(null);
                    member.Client.AsMember = null;
                    member.Client.Guild = null;
                    Message message = null;
                    message = new Message("guild " + Name + " has been Disbanded!", System.Drawing.Color.White, Message.World);
                    foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
                    {
                        client.Send(message);
                    }
                }
                else
                {
                    member.GuildID = 0;
                }
                if (member.IsOnline == true)
                {
                    foreach (Interfaces.IConquerItem Equi in member.Client.Inventory.Objects)
                    {
                        if (Equi != null)
                        {
                            if (Equi.Inscribed)
                            {
                                Equi.Inscribed = false;
                                Equi.Send(member.Client);
                            }
                        }
                    }
                    foreach (Interfaces.IConquerItem Equi in member.Client.Equipment.Objects)
                    {
                        if (Equi != null)
                        {
                            if (Equi.Inscribed)
                            {
                                Equi.Inscribed = false;
                                Equi.Send(member.Client);
                            }
                        }
                    }
                }
                Database.ConquerItemTable.UpdateInscre3(uid);
                MemberCount--;
                Members.Remove(uid);
            }
            var ally_ = Ally.Values.ToArray();
            foreach (Guild ally in ally_)
            {
                RemoveAlly(ally.Name);
                ally.RemoveAlly(Name);
            }
            Database.GuildTable.Disband(this);
            //shit? ServerBase.Kernel.GamePool.Remove(ID);
        }

        public void AddAlly(string name)
        {
            foreach (Guild guild in ServerBase.Kernel.Guilds.Values)
            {
                if (guild.Name == name)
                {
                    if (Enemy.ContainsKey(guild.ID))
                        RemoveEnemy(guild.Name);
                    Ally.Add(guild.ID, guild);
                    _String stringPacket = new _String(true);
                    stringPacket.UID = guild.ID;
                    stringPacket.Type = _String.GuildAllies;
                    stringPacket.Texts.Add(guild.Name + " " + guild.LeaderName + " 0 " + guild.MemberCount);
                    SendGuildMessage(stringPacket);
                    SendGuildMessage(stringPacket);
                    Database.GuildTable.AddAlly(this, guild.ID);
                    return;
                }
            }
        }

        public void RemoveAlly(string name)
        {
            foreach (Guild guild in Ally.Values)
            {
                if (guild.Name == name)
                {
                    GuildCommand cmd = new GuildCommand(true);
                    cmd.Type = GuildCommand.Neutral1;
                    cmd.dwParam = guild.ID;
                    SendGuildMessage(cmd);
                    SendGuildMessage(cmd);
                    Database.GuildTable.RemoveAlly(this, guild.ID);
                    Ally.Remove(guild.ID);
                    return;
                }
            }
        }

        public void AddEnemy(string name)
        {
            foreach (Guild guild in ServerBase.Kernel.Guilds.Values)
            {
                if (guild.Name == name)
                {
                    if (Ally.ContainsKey(guild.ID))
                    {
                        RemoveAlly(guild.Name);
                        guild.RemoveAlly(Name);
                    }
                    Enemy.Add(guild.ID, guild);
                    _String stringPacket = new _String(true);
                    stringPacket.UID = guild.ID;
                    stringPacket.Type = _String.GuildEnemies;
                    stringPacket.Texts.Add(guild.Name + " " + guild.LeaderName + " 0 " + guild.MemberCount);
                    SendGuildMessage(stringPacket);
                    SendGuildMessage(stringPacket);
                    Database.GuildTable.AddEnemy(this, guild.ID);
                    return;
                }
            }
        }
       

        public void RemoveEnemy(string name)
        {
            foreach (Guild guild in Enemy.Values)
            {
                if (guild.Name == name)
                {
                    GuildCommand cmd = new GuildCommand(true);
                    cmd.Type = GuildCommand.Neutral2;
                    cmd.dwParam = guild.ID;
                    SendGuildMessage(cmd);
                    SendGuildMessage(cmd);
                    Database.GuildTable.RemoveEnemy(this, guild.ID);
                    Enemy.Remove(guild.ID);
                    return;
                }
            }
        }


        public void SendName(Client.GameState client)
        {
            _String stringPacket = new _String(true);
            stringPacket.UID = ID;
            stringPacket.Type = _String.GuildName;
            stringPacket.Texts.Add(Name + " " + LeaderName + " 0 " + MemberCount);
            client.Send(stringPacket);
        }

        public void SendGuild(Client.GameState client)
        {
            int Position = 48;
            if (Members.ContainsKey(client.Entity.UID))
            {
               if (Bulletin == null)
                    Bulletin = "Este es un nuevo Clan!";
                client.Send(new Message(Bulletin, System.Drawing.Color.White, Message.GuildAnnouncement));
                Writer.WriteUInt32(uint.Parse(DateTime.Now.ToString("yyyymmdd")), 67, Buffer);
                Writer.WriteUInt32((uint)client.Arsenal_Donation, 8, Buffer);
                Writer.WriteUInt32((uint)(uint.MaxValue -100000), 88, Buffer);
                Writer.WriteUInt32((ushort)client.AsMember.Rank, 28, Buffer);
                Writer.WriteByte((byte)Level, (Position + 1), Buffer); Position += 8;
                client.Send(Buffer);
                //Guild_Info Buffer2 = new Guild_Info(this);
                //client.Send(Buffer2.ToArray);
            }
        }

        public void SendAllyAndEnemy(Client.GameState client)
        {
            foreach (Guild guild in Enemy.Values)
            {
                _String stringPacket = new _String(true);
                stringPacket.UID = guild.ID;
                stringPacket.Type = _String.GuildEnemies;
                stringPacket.Texts.Add(guild.Name + " " + guild.LeaderName + " 0 " + guild.MemberCount);
                client.Send(stringPacket);
                client.Send(stringPacket);
                //return;
            }
            foreach (Guild guild in Ally.Values)
            {
                _String stringPacket = new _String(true);
                stringPacket.UID = guild.ID;
                stringPacket.Type = _String.GuildAllies;
                stringPacket.Texts.Add(guild.Name + " " + guild.LeaderName + " 0 " + guild.MemberCount);
                client.Send(stringPacket);
                client.Send(stringPacket);
                //return;
            }
            //Console.WriteLine("ss");
        }
        public static bool ValidName(string Name)
        {
            if (Name.Length < 4 && Name.Length > 15) return false;
            else if (Name.IndexOfAny(new char[20] { ' ', '#', '%', '^', '&', '*', '(', ')', ';', ':', '\'', '\"', '/', '\\', ',', '.', '{', '}', '[', ']' }) > 0) return false;
            else return true;
        }
        byte _BattlePower;
        public byte BattlePower
        {
            get
            {
                return _BattlePower;
            }
            set
            {
                _BattlePower = value;
            }
        }

        public ulong Points = 0;

        public ushort Postion = 0;

        public ushort Postion2 = 0;

        public ulong Points2 = 0;
    }
}
