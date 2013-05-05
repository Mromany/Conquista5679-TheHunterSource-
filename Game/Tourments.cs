using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Database;

namespace PhoenixProject.Game.Tournaments
{
    public enum top_typ//Coded By Kimo
    {// 11 GoldenRacer
        GoldenRacer = 11,
        Elite_PK_Champion__Low_ = 12,
        Elite_PK_2nd_Place_Low_ = 13,
        Elite_PK_3rd_Place_Low_ = 14,
        Elite_PK_Top_8__Low_ = 15,

        Elite_PK_Champion_High_ = 16,
        Elite_PK_2nd_Place_High_ = 17,
        Elite_PK_3rd_Place__High_ = 18,
        Elite_PK_Top_8_High_ = 19
    }
    public class Elite_client
    {
        public uint Points = 0;
        public uint UID = 0;
        public ushort Avatar = 0;
        public ushort Mesh = 0;
        public string Name = "";
        public ushort Postion = 0;
        public byte MyTitle = 0;

        public Elite_client(Client.GameState client)
        {
            this.UID = client.Entity.UID;
            this.Avatar = client.Entity.Face;
            this.Mesh = client.Entity.Body;
            this.Name = client.Entity.Name;

        }
        public Elite_client(uint _uid, ushort _avatar, ushort _mesh, string _name, uint _points, ushort Position, byte Tytle)
        {
            this.MyTitle = Tytle;
            this.Postion = Position;
            this.Points = _points;
            this.UID = _uid;
            this.Avatar = _avatar;
            this.Mesh = _mesh;
            this.Name = _name;
        }
    }
    public class EliteTournament
    {
        public static Dictionary<uint, Elite_client> Elite_PK_Tournament = new Dictionary<uint, Elite_client>(500);
        public static Dictionary<uint, Elite_client> Top8 = new Dictionary<uint, Elite_client>(10);

        public static void LoginClient(Client.GameState client)
        {
            if (Top8.ContainsKey(client.Entity.UID))
            {
                if (Top8.ContainsKey(client.Entity.UID))
                {
                    client.Entity.Elite = Top8[client.Entity.UID];
                    CreatePacket(client);
                    //Console.WriteLine("ss");
                }
            }
        }
        public static void CreatePacket(Client.GameState client)
        {
            client.Entity.TitlePacket = new Network.GamePackets.TitlePacket(true);
            client.Entity.TitlePacket.UID = client.Entity.UID;
            client.Entity.TitlePacket.Type = 4;
            client.Entity.TitlePacket.dwParam = 1;
            client.Entity.TitlePacket.dwParam2 = client.Entity.Elite.MyTitle;
            //Console.WriteLine("ss2");
        }
        public static void CreatePacket2(Client.GameState client)
        {
            client.Entity.TitlePacket = new Network.GamePackets.TitlePacket(true);
            client.Entity.TitlePacket.UID = client.Entity.UID;
            client.Entity.TitlePacket.Type = 4;
            client.Entity.TitlePacket.dwParam = 1;
            client.Entity.TitlePacket.dwParam2 = Top8[client.Entity.UID].MyTitle;
            //Console.WriteLine("ss2");
        }
        public static void DeleteTabelInstances()
        {
            foreach (Elite_client client in Top8.Values)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.DELETE);
               cmd.Delete("elitepk", "UID", client.UID).Execute();
           } 
        }
        public static void LoadTop8()
        {
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
            cmd.Select("elitepk");
            MySqlReader r = new MySqlReader(cmd);
            while (r.Read())
            {
                Elite_client client = new Elite_client(r.ReadUInt32("UID"), r.ReadUInt16("Avatar"), r.ReadUInt16("Mesh"), r.ReadString("Name"), r.ReadUInt32("Points"), r.ReadUInt16("Postion"), r.ReadByte("MyTitle"));
                if (!Top8.ContainsKey(client.UID))
                    Top8.Add(client.UID, client);
            }
            r.Close();
            r.Dispose();
        }
        public static void SaveTop8(Client.GameState client)
        {
            //Conquer.Database.Elitepk.Insert((int)client.Entity.UID, (int)client.Entity.Face, (string)client.Entity.Name, (int)client.Entity.Mesh, (int)client.Entity.Points, (int)client.Entity.Postion, (long)client.Entity.MyTitle);
            Elite_client clients = new Elite_client(
                (uint)client.Entity.UID
                , (ushort)client.Entity.Face
                , (ushort)client.Entity.Body
                , (string)client.Entity.Name
                , (uint)client.Entity.Points
                , (ushort)Program.EliteRank
                , (byte)client.Entity.MyTitle
                    );
            if (!Top8.ContainsKey(clients.UID))
                Top8.Add(clients.UID, clients);
            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
            cmd.Insert("elitepk")
                .Insert("UID", clients.UID).Insert("Avatar", clients.Avatar)
                .Insert("Mesh", clients.Mesh).Insert("Name", clients.Name)
                .Insert("Points", clients.Points).Insert("Postion", Program.EliteRank)
          .Insert("MyTitle", clients.MyTitle);
            cmd.Execute();
           // Conquer.Database.Elitepk.Insert((int)clients.UID, (int)clients.Avatar, (string)clients.Name, (int)clients.Mesh, (int)clients.Points, (int)Program.EliteRank, (long)clients.MyTitle);
        }
        //public EliteTournament() { LoadTop8(); }
        public static void Open()
        {
            if (!Start)
            {
                DeleteTabelInstances();
                Start = true;
                //CalculateTime = DateTime.Now;
                //StartTimer = DateTime.Now;
                //SendInvitation();
                Elite_PK_Tournament.Clear();
                Top8.Clear();
            }
        }
        public static void ElitePkKimo()
        {
            if (DateTime.Now.Minute == DateTime.Now.Minute)
            {
                if (DateTime.Now.Minute == DateTime.Now.Minute)
                {
                    DeleteTabelInstances();
                    Elite_PK_Tournament.Clear();
                    Top8.Clear();
                }
            }
        }
        public void Open(int hour, int minute)
        {
            if (DateTime.Now.Minute == minute && DateTime.Now.Hour == hour)
            {
                if (!Start)
                {
                    DeleteTabelInstances();
                    Start = true;
                    //CalculateTime = DateTime.Now;
                    //StartTimer = DateTime.Now;
                    //SendInvitation();
                    Elite_PK_Tournament.Clear();
                    Top8.Clear();
                }
            }
        }
        /*public static void SendInvitation()
        {
            Client.GameState[] client = PhoenixProject.ServerBase.Kernel.GamePool.Values.ToArray();
            foreach (Client.GameState clientss in client)
            {
                Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "The Elite Tournament has Started! You Wana Join?");
                npc.OptionID = 249;
                clientss.Send(npc.ToArray());
            }
        }*/
        //private static DateTime CalculateTime;
 
        public static void ObtinedReward(Client.GameState client)
        {
            if(ServerBase.Kernel.GamePool.ContainsKey(client.Entity.UID))
            {
                switch (Program.EliteRank)
                {
                    case 1:
                        {
                            client.Entity.MyTitle = (byte)top_typ.Elite_PK_Champion_High_;
                            client.Entity.ConquerPoints += Database.rates.elitepk;
                            SaveTop8(client);
                            CreatePacket2(client);
                            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations, " + client.Entity.Name + " has Won ElitePk Tourment and Get [1st] rank and " + Database.rates.elitepk + " cps!", System.Drawing.Color.White, Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            break;
                        }
                    case 2:
                        {
                            client.Entity.MyTitle = (byte)top_typ.Elite_PK_2nd_Place_High_;
                            client.Entity.ConquerPoints += Database.rates.elitepk - 3000;
                            SaveTop8(client);
                            CreatePacket2(client);
                            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations, " + client.Entity.Name + " has got [2nd] rank in ElitePk Tourment and " + (Database.rates.elitepk - 3000) + " cps!", System.Drawing.Color.White, Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            break;
                        }
                    case 3:
                        {
                            client.Entity.MyTitle = (byte)top_typ.Elite_PK_3rd_Place__High_;
                            client.Entity.ConquerPoints += Database.rates.elitepk - 5000;
                            SaveTop8(client);
                            CreatePacket2(client);
                            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations, " + client.Entity.Name + " has got [3rd] rank in ElitePk Tourment and " + (Database.rates.elitepk - 5000) + " cps!", System.Drawing.Color.White, Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            break;
                        }
                    default:
                        {
                            client.Entity.MyTitle = (byte)top_typ.Elite_PK_Top_8_High_;
                            client.Entity.ConquerPoints += Database.rates.elitepk - 7000;
                            SaveTop8(client);
                            CreatePacket2(client);
                            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations, " + client.Entity.Name + " has got ["+Program.EliteRank+"Th] rank in ElitePk Tourment and "+(Database.rates.elitepk - 7000)+" cps!", System.Drawing.Color.White, Network.GamePackets.Message.World), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                            break;
                        }
                        
                }
                //Console.WriteLine("a7a7");
            }
        }
        public static  DateTime StartTimer;
        public static  bool Start = false;
       // private static ushort Mapid = 7777;

        public static void AddMap(Client.GameState client)
        {

                //client.elitepoints = 0;
                client.Entity.Teleport(7777, 150, 162);
        }
 
    }
}
