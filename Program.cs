using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using PhoenixProject.Database;
using PhoenixProject.Game;
using PhoenixProject.Game.ConquerStructures.Society;
using PhoenixProject.Network.AuthPackets;
using PhoenixProject.Network.Sockets;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Network.GamePackets.EventAlert;

namespace PhoenixProject
{

    class Program
    {
        public static void WriteError(Exception ex)
        {
            Console.WriteLine(ex.ToString() + " (Report this Shit to Kimo!)");
        }
        public static void WriteLine(Exception Exc)
        {
            try
            {
                Console.WriteLine(Exc.ToString());

            }
            catch { }
        }
        public static void WriteLine(string Line)
        {
            try
            {
                Console.WriteLine(Line);
            }
            catch { }
        }
        public static void WriteLine()
        {
            try
            {
                Console.WriteLine("");
            }
            catch { }
        }
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static bool connec = false;
        public static bool restarted = false;
        public static uint mess = 0;
        public static Time32 messtime;
        public static long PoolSize = 0;
        public static bool IsBetweenTwoPoints(ushort x, ushort y, ushort startx, ushort starty, ushort endx, ushort endy)
        {
            return (((x >= startx) && (y >= starty)) && ((x <= endx) && (y <= endy)));
        }
        public static long Carnaval = 0;
        public static long Carnaval2 = 0;
        public static long Carnaval3 = 0;
        public static long MaxOn = 0;
        public static KimoDatabase.Database KimoDatabase;
        //public static Quests QuestDatabase;
        public static long Carnaval4 = 0;
        public static Time32 QuizStamp;
        public static Time32 KimoTime1;
        public static PhoenixProject.Region.MapRegions MapRegions;
        public static Time32 KimoTime2;
        public static Time32 KimoTime3;
        public static Time32 KimoTime4;
        public static Time32 KimoTime5;
        public static Time32 KimoTime6;
        public static Time32 KimoTime7;
        public static Time32 KimoTime8;
        public static Time32 KimoTime9;
        public static Time32 KimoTime10;
        public static Time32 KimoTime11;
        public static Time32 KimoTime12;
        public static Time32 KimoTime13;
        public static Time32 KimoTime14;
        public static Time32 KimoTime15;
        public static Time32 KimoTime16;
        //public static SpellTable SpellDatabase;
        public static long Carnaval5 = 0;
        public static string ConquerDirectory = @"C:\";
        public static string QuestInfo = Path.Combine(ConquerDirectory, "Questinfo.ini");
        public static string QuestInfo2 = "database\\Questinfo.ini";
        public static string Mapr = "database\\region.ini";
        public static long Carnaval6 = 0;
        public static KinSocket.TqRandom Random;
        public static long Carnaval7 = 0;
        public static long Carnaval8 = 0;
        public static long EliteRank = 0;
        public static long WeatherType = 0;
        public static int PlayerCap = 400;
        public static int BoxX = 43;
        public static int DemonCave1 = 0;
        public static int DemonCave2 = 0;
        public static int DemonCave3 = 0;
        public static int Quiz1 = 0;
        public static ulong kimo = 2105;
        public static ulong kimo2 = 2;
        public static ulong kimo3 = 33;
        public static string DBName = "";
        public static string DBUser = "";
        public static string DBPass = "";
        public const string PacketSniffingPath = "database\\sniff\\";
        public static int BoxY = 50;
        public static DateTime m_msgDate;
        public static byte m_Counter;
        //public static Time32 InviteSendStamp;
        //public static Time32 KillConnectionTime;
        public static AsyncSocket AuthServer;
        public static uint ScreenColor = 0;
        public static uint steed = 147;
        public static uint lotterytype = 300;
        public static uint lotteryprize = 7;
        public static AsyncSocket GameServer;
        public static ServerBase.Counter EntityUID;
        public static string GameIP;
        public static DayOfWeek Today;
        public static ushort GamePort;
        public static ushort AuthPort;
        public static uint nextID = 0;
        public static uint nextClanid = 0;
        public static uint nextEntityID = 0;
        public static uint nextGuildID = 0;
        public static DateTime StartDate;
        public static DateTime Refresh;
        public static System.Timers.Timer MyTimer;
        public static DateTime RestartDate = StartDate.AddHours(12);

        

        public static Thread[] Threads = new Thread[4];
        static byte counter = 0;
        static byte Equation = 0;
        public static ServerBase.Threads kimoz6 = new ServerBase.Threads(200);
        public static ServerBase.Threads kimoz5 = new ServerBase.Threads(1000);
        public static ServerBase.Threads kimoz4 = new ServerBase.Threads(1000);
        public static ServerBase.Threads kimoz3 = new ServerBase.Threads(1000);
        public static ServerBase.Threads kimoz2 = new ServerBase.Threads(1000);
        public static ServerBase.Threads kimoz1 = new ServerBase.Threads(100);
        public static ServerBase.Threads kimo_ = new PhoenixProject.ServerBase.Threads(1000);
        public static ServerBase.Threads SystemMessages = new ServerBase.Threads(1000);
        public static ServerBase.Threads ServerStuff = new ServerBase.Threads(1000);

        public static ServerBase.Threads ArenaSystem = new PhoenixProject.ServerBase.Threads(1000);
        public static void StartThreads()
        {
           /*MyTimer = new System.Timers.Timer(1000);
            MyTimer.AutoReset = true;
            MyTimer.Elapsed += new System.Timers.ElapsedEventHandler(_timerCallBack5);
            MyTimer.Start();*/
           /* Program.Factory = new KinSocket.ProcessFactory(20);

            Program.Factory.AddProcess(new Action(ServerStuff_Execute));
            Program.Factory.AddProcess(new Action(kimo_Execute));
            Program.Factory.AddProcess(new Action(SystemMessages_Execute));
            Program.Factory.AddProcess(new Action(Game.ConquerStructures.Arena.ArenaSystem_Execute));
            Program.Factory.AddProcess(new Action(FactoryHandles.OnMobTimer));
            Program.Factory.AddProcess(new Action(FactoryHandles.OnPlayerAction));
            Program.Factory.AddProcess(new Action(FactoryHandles.OnPlayerAttack));
            Program.Factory.AddProcess(new Action(FactoryHandles.OnPlayerBuff));
            Program.Factory.AddProcess(new Action(FactoryHandles.OnPlayerMisc));
            Program.Factory.AddProcess(new Action(FactoryHandles.OnPlayerReceive));
            Program.Factory.AddProcess(new Action(FactoryHandles.OnPlayerSave));
            Program.Factory.AddProcess(new Action(FactoryHandles.OnTimerPet));
            Program.Factory.Start();*/
            Threads[0] = new Thread(new ThreadStart(Game.ConquerStructures.Arena.ArenaSystem_Execute));
            Threads[1] = new Thread(new ThreadStart(kimo_Execute));
            Threads[2] = new Thread(new ThreadStart(SystemMessages_Execute));
            Threads[3] = new Thread(new ThreadStart(ServerStuff_Execute));
            ////Threads[4] = new Thread(new ThreadStart(Kimoz1_Execute));
            //Threads[5] = new Thread(new ThreadStart(Kimoz2_Execute));
           // Threads[6] = new Thread(new ThreadStart(Kimoz3_Execute));
           // Threads[7] = new Thread(new ThreadStart(Kimoz4_Execute));
            //Threads[8] = new Thread(new ThreadStart(Kimoz5_Execute));
            //Threads[9] = new Thread(new ThreadStart(Kimoz6_Execute));
             
            foreach (Thread _t in Threads)
            {
                counter++;
                _t.Start();
                Equation = counter; Equation -= 1;
            }
            Console.WriteLine("Threads Started Compelete!!");
        }
        public static Client.GameState[] Values = null;

        public static Server GUI;
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException +=
        new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.Run(new NotForPublicNotAtAll.NoCrash());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            SaveException(e.Exception);
        }
        static void CurrentDomain_UnhandledException
        (object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                OutOfMemoryException ex = (OutOfMemoryException)e.ExceptionObject;
                SaveException2(ex);
              
            }
            finally
            {
               // Application.Exit();
            }
        }

        public static void SaveException(Exception e)
        {
            if (e.TargetSite.Name == "ThrowInvalidOperationException")
                return;
            if (e.Message.Contains("String reference not set"))
                return;

            Console.WriteLine(e);

            var dt = DateTime.Now;
            string date = dt.Month + "-" + dt.Day + "//";

            if (!Directory.Exists(Application.StartupPath + ServerBase.Constants.UnhandledExceptionsPath))
                Directory.CreateDirectory(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath);
            if (!Directory.Exists(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date))
                Directory.CreateDirectory(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date);
            if (!Directory.Exists(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date + e.TargetSite.Name))
                Directory.CreateDirectory(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date + e.TargetSite.Name);

            string fullPath = Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date + e.TargetSite.Name + "\\";

            string date2 = dt.Hour + "-" + dt.Minute;
            List<string> Lines = new List<string>();

            Lines.Add("----Exception message----");
            Lines.Add(e.Message);
            Lines.Add("----End of exception message----\r\n");

            Lines.Add("----Stack trace----");
            Lines.Add(e.StackTrace);
            Lines.Add("----End of stack trace----\r\n");

            //Lines.Add("----Data from exception----");
            //foreach (KeyValuePair<object, object> data in e.Data)
            //    Lines.Add(data.Key.ToString() + "->" + data.Value.ToString());
            //Lines.Add("----End of data from exception----\r\n");
            
            File.WriteAllLines(fullPath + date2 + ".txt", Lines.ToArray());
        }
        public static void SaveException2(OutOfMemoryException e)
        {
            if (e.TargetSite.Name == "ThrowInvalidOperationException")
                return;
            if (e.Message.Contains("String reference not set"))
                return;

            Console.WriteLine(e);

            var dt = DateTime.Now;
            string date = dt.Month + "-" + dt.Day + "//";

            if (!Directory.Exists(Application.StartupPath + ServerBase.Constants.UnhandledExceptionsPath))
                Directory.CreateDirectory(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath);
            if (!Directory.Exists(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date))
                Directory.CreateDirectory(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date);
            if (!Directory.Exists(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date + e.TargetSite.Name))
                Directory.CreateDirectory(Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date + e.TargetSite.Name);

            string fullPath = Application.StartupPath + "\\" + ServerBase.Constants.UnhandledExceptionsPath + date + e.TargetSite.Name + "\\";

            string date2 = dt.Hour + "-" + dt.Minute;
            List<string> Lines = new List<string>();

            Lines.Add("----Exception message----");
            Lines.Add(e.Message);
            Lines.Add("----End of exception message----\r\n");

            Lines.Add("----Stack trace----");
            Lines.Add(e.StackTrace);
            Lines.Add("----End of stack trace----\r\n");

            //Lines.Add("----Data from exception----");
            //foreach (KeyValuePair<object, object> data in e.Data)
            //    Lines.Add(data.Key.ToString() + "->" + data.Value.ToString());
            //Lines.Add("----End of data from exception----\r\n");

            File.WriteAllLines(fullPath + date2 + ".txt", Lines.ToArray());
        }
        public static void StartEngine()
        {
            EngineThread_Execute();
        }
        public static int RandomSeed = 0;
        static DateTime LastRandomReset = DateTime.Now;
        public static KinSocket.ProcessFactory Factory;
        #region Thread Processes
        static void EngineThread_Execute()
        {
           
           
            /*var proc = System.Diagnostics.Process.GetCurrentProcess();
            proc.PriorityBoostEnabled = true;
            proc.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;*/
            var proc = System.Diagnostics.Process.GetCurrentProcess();
            proc.PriorityBoostEnabled = true;
            proc.PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            if (DateTime.Now.DayOfYear > 365)
            {
                return;
            }
            Time32 Start = Time32.Now;
            RandomSeed = Convert.ToInt32(DateTime.Now.Ticks.ToString().Remove(DateTime.Now.Ticks.ToString().Length / 2));
            ServerBase.Kernel.Random = new Random(RandomSeed);
            StartDate = DateTime.Now;
            // Application.Run(new Server().ShowDialog());
            Console.Title = "TQ Conquer Server Loading....."; Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            IntPtr hWnd = FindWindow(null, Console.Title);


            Console.WriteLine("|------( TheHunter#3 5692-2013 )------|");
            Console.WriteLine("|------( ----All Rights bk to Kimo Source---- )------|");
            Console.WriteLine(">>>>>>>>Loading.....................");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Load server configuration! !");
            string ConfigFileName = "configuration.ini";

            ServerBase.IniFile IniFile = new ServerBase.IniFile(ConfigFileName);

            {
                GameIP = IniFile.ReadString("configuration", "IP");
                GamePort = IniFile.ReadUInt16("configuration", "GamePort");
                AuthPort = IniFile.ReadUInt16("configuration", "AuthPort");
                ServerBase.Constants.ServerName = IniFile.ReadString("configuration", "ServerName");
                Database.DataHolder.CreateConnection(IniFile.ReadString("MySql", "Username"), IniFile.ReadString("MySql", "Password"), IniFile.ReadString("MySql", "Database"), IniFile.ReadString("MySql", "Host"));
            }

            MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT).Select("configuration").Where("Server", ServerBase.Constants.ServerName);
            PhoenixProject.Database.MySqlReader r = new PhoenixProject.Database.MySqlReader(cmd);
            if (r.Read())
            {
                //EntityUID = new ServerBase.Counter(r.ReadUInt32("EntityID"));
                Game.ConquerStructures.Society.Guild.GuildCounter = new PhoenixProject.ServerBase.Counter(r.ReadUInt32("GuildID"));
                Network.GamePackets.ConquerItem.ItemUID = new PhoenixProject.ServerBase.Counter(r.ReadUInt32("ItemUID"));
                ServerBase.Constants.ExtraExperienceRate = r.ReadUInt32("ExperienceRate");
                ServerBase.Constants.ExtraSpellRate = r.ReadUInt32("ProficiencyExperienceRate");
                ServerBase.Constants.ExtraProficiencyRate = r.ReadUInt32("SpellExperienceRate");
                ServerBase.Constants.MoneyDropRate = r.ReadUInt32("MoneyDropRate");
                ServerBase.Constants.MoneyDropMultiple = r.ReadUInt32("MoneyDropMultiple");
                ServerBase.Constants.ConquerPointsDropRate = r.ReadUInt32("ConquerPointsDropRate");
                ServerBase.Constants.ConquerPointsDropMultiple = r.ReadUInt32("ConquerPointsDropMultiple");
                ServerBase.Constants.ItemDropRate = r.ReadUInt32("ItemDropRate");
                ServerBase.Constants.ItemDropQualityRates = r.ReadString("ItemDropQualityString").Split('~');
                ServerBase.Constants.WebAccExt = r.ReadString("AccountWebExt");
                ServerBase.Constants.WebVoteExt = r.ReadString("VoteWebExt");
                ServerBase.Constants.WebDonateExt = r.ReadString("DonateWebExt");
                ServerBase.Constants.ServerWebsite = r.ReadString("ServerWebsite");
                PlayerCap = r.ReadInt32("PlayerCap");
            }
            r.Close();
            Console.WriteLine("Initializing database.");

            // Database.Quests.Load();

            Console.WriteLine("Initializing database Succes.");
            Program.ServerRrestart = true;
           





           
         
            Database.NameChange.UpdateNames();
           
           
            Game.KimoEvents.LordsWarTime();
            Game.KimoEvents.CaptureTeamTime();
            Game.KimoEvents.GWstartTime();
            Game.KimoEvents.GWEndTime();
            Game.KimoEvents.DisCityTime();
            Game.KimoEvents.DemonCaveTime();
            Game.KimoEvents.ElitePKTime();
            Game.KimoEvents.SkillTeamTime();
            Game.KimoEvents.SpouseTime();
            Game.KimoEvents.ClassTime();
            Game.KimoEvents.EliteGWTime();
            Game.KimoEvents.ClanWarTime();
            
            //TreasureBox Game.KimoEvents.TreasureTime();
            Game.KimoEvents.WeeklyTime();
            Game.KimoEvents.DonationWarTime();
            Database.GameUpdatess.LoadRates();
            Database.Messagess.LoadRates();
            Database.HelpDesk.LoadRates();
            Database.ConquerItemInformation.Load();
           // Database.ItemLog.CleanUp();
            Game.Flags.LoadFlags();
            Database.DataHolder.ReadStats();
            Database.MonsterInformation.Load();
            Database.SpellTable.Load();
            Console.WriteLine("New Spells loaded.");
            Database.ShopFile.Load();
            Database.MapsTable.Load();
            Game.PrizeNPC.Load();
            Database.NobilityTable.Load();
            Database.ArenaTable.Load();
            Database.GuildTable.Load();
            Database.LotteryTable.Load();
            Database.DROP_SOULS.LoadDrops();
            Database.DROP_SOULS.LoadJar();
            Refinery.Load();
            Database.DMaps.Load();
            Database.QuizData.Load();
            Database.EntityTable.LoadPlayersVots();
            Values = new Client.GameState[0];
           
          /*  foreach (Database.MapsTable.MapInformation map in Database.MapsTable.MapInformations.Values)
            {
                if (map.ID == map.BaseID)
                {
                    new Game.Map(map.ID, Database.DMaps.MapPaths[map.BaseID]);
                    Console.WriteLine(" " + map.ID + "");
                    Console.WriteLine("Maps Installed " + ServerBase.Kernel.Maps.Count + "");
                }
            }*/
           
          
            new Game.Map(1038, Database.DMaps.MapPaths[1038]);
            new Game.Map(2071, Database.DMaps.MapPaths[2071]);
            new Game.Map(1509, Database.DMaps.MapPaths[1509]);
            Console.WriteLine("Maps Installed " + ServerBase.Kernel.Maps.Count + "");
            Game.ConquerStructures.Society.GuildWar.Initiate();
            Console.WriteLine("Guild war initializated.");
            Game.ClanWar.Initiate();
            Console.WriteLine("Clan war initializated.");
            Game.ConquerStructures.Society.EliteGuildWar.EliteGwint();
            Console.WriteLine("Elite Guild war initializated.");
            Database.rates.LoadRates();
            Database.EntityTable.NextEntity();
            EntityUID = new ServerBase.Counter(Program.nextEntityID);
            if (EntityUID.Now == 0) // i fixed the bug, now it shows what it's supposed to, you have database problems
            {
                Console.Clear();
                Console.WriteLine("Database error. Please check your MySQL. Server will now close.");

                return;
            }
            Console.WriteLine("Loading Game Clans.");
            Database.Clans.LoadAllClans();
            Database.EntityTable.NextUit();
            Database.EntityTable.NextEntity();
            Database.Clans.NextClan();
            Database.EntityTable.NextGuild();
            ServerBase.FrameworkTimer.SetPole(100000, 100000);
           // ServerBase.FrameworkTimer.SetPole(100, 50);
            //System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(ServerBase.FrameworkTimer.DoNothing));
            Values = new Client.GameState[0];
            Database.DMaps.LoadHouse();
            Console.WriteLine("House Table Loaded.");
            Program.MapRegions = new PhoenixProject.Region.MapRegions();
            Program.MapRegions.Load();
            Console.WriteLine("MapRegions Loaded.");
            Game.Tournaments.EliteTournament.LoadTop8();
            Console.WriteLine("ElitePk Winner Loaded.");
            Console.WriteLine("Flower Table Loaded.");
           

            SystemMessages.Execute += new Action(SystemMessages_Execute);
            SystemMessages.Start();
            kimo_.Execute += new Action(kimo_Execute);
            kimo_.Start();
            ServerStuff.Execute += new Action(ServerStuff_Execute);
            ServerStuff.Start();
            ArenaSystem.Execute += new Action(Game.ConquerStructures.Arena.ArenaSystem_Execute);
            ArenaSystem.Start();
            kimoz6.Execute += new Action(Kimoz6_Execute);
            kimoz6.Start();
            kimoz5.Execute += new Action(Kimoz5_Execute);
            kimoz5.Start();
            kimoz4.Execute += new Action(Kimoz4_Execute);
            kimoz4.Start();
            kimoz3.Execute += new Action(Kimoz3_Execute);
            kimoz3.Start();
            kimoz2.Execute += new Action(Kimoz2_Execute);
            kimoz2.Start();
            kimoz1.Execute += new Action(Kimoz1_Execute);
            kimoz1.Start();
            //StartThreads();
           // Console.Title = "[" + Database.rates.servername + "]Phoenix Conquer Project. Start time: " + StartDate.ToString("dd MM yyyy hh:mm") + "";
            new MySqlCommand(MySqlCommandType.UPDATE).Update("entities").Set("Online", 0).Where("Online", 1).Execute();
            Console.Title = "[" + Database.rates.servername + "] TheHunter Source. Start time: " + Program.StartDate.ToString("dd MM yyyy hh:mm") + ". Players online: " + ServerBase.Kernel.GamePool.Count + "/" + Program.PlayerCap + " Max Online: " + Program.MaxOn + "";
            
            Network.AuthPackets.Forward.Incrementer = new ServerBase.Counter();
            Network.Cryptography.AuthCryptography.PrepareAuthCryptography();
            Console.WriteLine("Initializing sockets.");

            AuthServer = new AsyncSocket(AuthPort);
            AuthServer.OnClientConnect += new Action<Interfaces.ISocketWrapper>(AuthServer_AnnounceNewConnection);
            AuthServer.OnClientReceive += new Action<byte[], Interfaces.ISocketWrapper>(AuthServer_AnnounceReceive);
            AuthServer.OnClientDisconnect += new Action<Interfaces.ISocketWrapper>(AuthServer_AnnounceDisconnection);
            GameServer = new AsyncSocket(GamePort);
            GameServer.OnClientConnect += new Action<Interfaces.ISocketWrapper>(GameServer_AnnounceNewConnection);
            GameServer.OnClientReceive += new Action<byte[], Interfaces.ISocketWrapper>(GameServer_AnnounceReceive);
            GameServer.OnClientDisconnect += new Action<Interfaces.ISocketWrapper>(GameServer_AnnounceDisconnection);



            Console.WriteLine("|------[ >>>>>Server Loaded<<<<< ]------|");
            Console.WriteLine("|------[ Coded and Edited by theHunter ]------|");
            Console.WriteLine("|--------Call_mee_aly@hotmail.com-----|");
            Console.WriteLine("|------------01116315131---------|");
            Console.WriteLine("|----All Rights bk to TheHunter#3 Source----|");
            Console.WriteLine("Server loaded in " + (Time32.Now - Start) + " milliseconds.");

            Program.ServerRrestart = false;
            GC.Collect();
            while (true)
            {
                CommandsAI(Console.ReadLine());
            }
            
        }
        static void Kimoz6_Execute()
        {
           
                lock (Values)
                    Values = ServerBase.Kernel.GamePool.Values.ToArray();
                Time32 Now = Time32.Now;
               // KimoTime1 = Time32.Now;
                foreach (Client.GameState client in Values)
                {
                    if (client.Socket != null)
                    {
                        if (client.Socket.Connected)
                        {
                            if (client.Entity.HandleTiming)
                            {
                                #region Training points
                                if (client.Entity.HeavenBlessing > 0 && !client.Entity.Dead)
                                {
                                    if (Now > client.LastTrainingPointsUp.AddMinutes(10))
                                    {
                                        client.OnlineTrainingPoints += 10;
                                        if (client.OnlineTrainingPoints >= 30)
                                        {
                                            client.OnlineTrainingPoints -= 30;
                                            client.IncreaseExperience(client.ExpBall / 100, false);
                                        }
                                        client.LastTrainingPointsUp = Now;
                                        client.Entity.Update(Network.GamePackets.Update.OnlineTraining, client.OnlineTrainingPoints, false);
                                    }
                                }
                                #endregion
                                #region Minning
                                if (client.Mining && !client.Entity.Dead)
                                {
                                    if (Now >= client.MiningStamp.AddSeconds(2))
                                    {
                                        client.MiningStamp = Now;
                                        Game.ConquerStructures.Mining.Mine(client);
                                    }
                                }
                                #endregion
                                #region MentorPrizeSave
                                if (Now > client.LastMentorSave.AddSeconds(5))
                                {
                                    Database.KnownPersons.SaveApprenticeInfo(client.AsApprentice);
                                    client.LastMentorSave = Now;
                                }
                                #endregion
                                #region Attackable
                                if (client.JustLoggedOn)
                                {
                                    client.JustLoggedOn = false;
                                    client.ReviveStamp = Now;
                                }
                                if (!client.Attackable)
                                {
                                    if (Now > client.ReviveStamp.AddSeconds(5))
                                    {
                                        client.Attackable = true;
                                    }
                                }
                                #endregion
                                #region DoubleExperience
                                if (client.Entity.DoubleExperienceTime > 0)
                                {
                                    if (Now > client.Entity.DoubleExpStamp.AddMilliseconds(1000))
                                    {
                                        client.Entity.DoubleExpStamp = Now;
                                        client.Entity.DoubleExperienceTime--;
                                    }
                                }
                                #endregion
                                #region HeavenBlessing
                                if (client.Entity.HeavenBlessing > 0)
                                {
                                    if (Now > client.Entity.HeavenBlessingStamp.AddMilliseconds(1000))
                                    {
                                        client.Entity.HeavenBlessingStamp = Now;
                                        client.Entity.HeavenBlessing--;
                                    }
                                }
                                #endregion
                                #region Enlightment
                                if (client.Entity.EnlightmentTime > 0)
                                {
                                    if (Now >= client.Entity.EnlightmentStamp.AddMinutes(1))
                                    {
                                        client.Entity.EnlightmentStamp = Now;
                                        client.Entity.EnlightmentTime--;
                                        if (client.Entity.EnlightmentTime % 10 == 0 && client.Entity.EnlightmentTime > 0)
                                            client.IncreaseExperience(Game.Attacking.Calculate.Percent((int)client.ExpBall, .10F), false);
                                    }
                                }
                                #endregion
                                #region PKPoints
                                if (Now >= client.Entity.PKPointDecreaseStamp.AddMinutes(5))
                                {
                                    client.Entity.PKPointDecreaseStamp = Now;
                                    if (client.Entity.PKPoints > 0)
                                    {
                                        client.Entity.PKPoints--;
                                    }
                                    else
                                        client.Entity.PKPoints = 0;
                                }
                                #endregion
                                #region OverHP
                                if (client.Entity.FullyLoaded)
                                {
                                    if (client.Entity.Hitpoints > client.Entity.MaxHitpoints && client.Entity.MaxHitpoints > 1 && !client.Entity.Transformed)
                                    {
                                        client.Entity.Hitpoints = client.Entity.MaxHitpoints;
                                    }
                                }
                                #endregion
                                #region Stamina
                                if (Now > client.Entity.StaminaStamp.AddMilliseconds(500))
                                {
                                    if (client.Entity.Vigor < client.Entity.MaxVigor)
                                    {
                                        if (client.Entity.Vigor + 3 < client.Entity.MaxVigor)
                                        {
                                            client.Entity.Vigor += (ushort)(3 + (client.Entity.Action == Game.Enums.ConquerAction.Sit ? 2 : 0));

                                            {
                                                Network.GamePackets.Vigor vigor = new Network.GamePackets.Vigor(true);
                                                vigor.VigorValue = client.Entity.Vigor;
                                                vigor.Send(client);
                                            }
                                        }
                                        else
                                        {
                                            client.Entity.Vigor = (ushort)client.Entity.MaxVigor;

                                            {
                                                Network.GamePackets.Vigor vigor = new Network.GamePackets.Vigor(true);
                                                vigor.VigorValue = client.Entity.Vigor;
                                                vigor.Send(client);
                                            }
                                        }
                                    }
                                    if (!client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                    {
                                        int limit = 0;
                                        if (client.Entity.HeavenBlessing > 0)
                                            limit = 50;
                                        if (client.Entity.Stamina != 100 + limit)
                                        {
                                            if (client.Entity.Action == Game.Enums.ConquerAction.Sit || !client.Equipment.Free(18))
                                            {
                                                if (client.Entity.Stamina <= 93 + limit)
                                                {
                                                    client.Entity.Stamina += 7;
                                                }
                                                else
                                                {
                                                    if (client.Entity.Stamina != 100 + limit)
                                                        client.Entity.Stamina = (byte)(100 + limit);
                                                }
                                            }
                                            else
                                            {
                                                if (client.Entity.Stamina <= 97 + limit)
                                                {
                                                    client.Entity.Stamina += 3;
                                                }
                                                else
                                                {
                                                    if (client.Entity.Stamina != 100 + limit)
                                                        client.Entity.Stamina = (byte)(100 + limit);
                                                }
                                            }
                                        }
                                        client.Entity.StaminaStamp = Now;
                                    }
                                }
                                #endregion
                                #region SoulShackle
                                if (client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.SoulShackle))
                                {
                                    if (Now > client.Entity.ShackleStamp.AddSeconds(client.Entity.ShackleTime))
                                    {
                                        client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.SoulShackle);
                                    }
                                }
                                #endregion
                                #region Freeze
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags2.IceBlock))
                                {
                                    if (Now > client.Entity.FreezeStamp.AddSeconds(client.Entity.FreezeTime))
                                    {
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags2.IceBlock);
                                    }
                                }
                                #endregion
                                #region AzureShield
                                if (client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.AzureShield))
                                {
                                    if (Now > client.Entity.MagicShieldStamp.AddSeconds(client.Entity.MagicShieldTime))
                                    {
                                        client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.AzureShield);
                                    }
                                }
                                #endregion
                                #region Die Delay
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Dead) && !client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Ghost))
                                {
                                    if (Now > client.Entity.DeathStamp.AddSeconds(2))
                                    {
                                        client.Entity.AddFlag(Network.GamePackets.Update.Flags.Ghost);
                                        if (client.Entity.Body % 10 < 3)
                                            client.Entity.TransformationID = 99;
                                        else
                                            client.Entity.TransformationID = 98;

                                        client.SendScreenSpawn(client.Entity, true);
                                    }
                                }
                                #endregion
                                #region SkillTeam
                                if (client.Entity.MapID == 7009)
                                {
                                    if (client.Entity.MapID == 7009)
                                    {
                                        Game.KimoSkillWar.SkillTeamRes(client);
                                    }
                                }
                                #endregion
                                #region CaptureFlag
                                if (client.Entity.MapID == 2060)
                                {
                                    if (client.Entity.MapID == 2060)
                                    {
                                        Game.Team.CaptureRes(client);
                                    }
                                }
                                #endregion

                            }
                        }
                    }
                    
                }
               // Thread.Sleep(200);
            
        }
        static void Kimoz5_Execute()
        {
           
                lock (Values)
                    Values = ServerBase.Kernel.GamePool.Values.ToArray();
                Time32 Now = Time32.Now;
                KimoTime2 = Time32.Now;
                foreach (Client.GameState client in Values)
                {
                    if (client.Socket != null)
                    {
                        if (client.Socket.Connected)
                        {
                            if (client.Entity.HandleTiming)
                            {
                                #region CharacterThread_Execute
                                if (client.Entity.BlackSpots)
                                {
                                    if (Now >= client.Entity.BlackSpotTime.AddSeconds(client.Entity.BlackSpotTime2))
                                    {

                                        BlackSpot spot = new BlackSpot
                                        {
                                            Remove = 1,
                                            Identifier = client.Entity.UID
                                        };
                                        client.Send((byte[])spot);
                                        client.Entity.BlackSpots = false;
                                        client.Entity.BlackSpotTime2 = 0;
                                        client.Entity.BlackSpotCheck = 0;
                                    }
                                    else
                                    {
                                        if (client.Entity.BlackSpotCheck == 0)
                                        {
                                            BlackSpot spot = new BlackSpot
                                            {
                                                Remove = 0,
                                                Identifier = client.Entity.UID
                                            };
                                            client.Send((byte[])spot);
                                            client.Entity.BlackSpotCheck = 1;
                                        }

                                    }

                                }
                                /* if (ActivePOPUP == 99995)
                                 {
                                     if (Now > LastPopUPCheck.AddSeconds(20))
                                     {
                                         Owner.Disconnect();
                                     }
                                 }*/
                                /* if (Owner.popups == 0)
                                 {//kimo
                                     Owner.popups = 1;
                                     Owner.Send(new Network.GamePackets.Message("" + PhoenixProject.Database.rates.PopUpURL + "", System.Drawing.Color.Red, Network.GamePackets.Message.Website));
            
                                 }*/
                                if (Database.rates.Night == 1)
                                {
                                    if (client.Entity.MapID == 701)
                                    {
                                        Random disco = new Random();
                                        uint discocolor = (uint)disco.Next(50000, 999999999);
                                        //Program.ScreenColor = discocolor;
                                        //ScreenColor = Program.ScreenColor;
                                        PhoenixProject.Network.GamePackets.Data datas = new PhoenixProject.Network.GamePackets.Data(true);
                                        datas.UID = client.Entity.UID;
                                        datas.ID = 104;
                                        datas.dwParam = discocolor;
                                        client.Send(datas);
                                    }
                                    else
                                    {
                                        if (DateTime.Now.Minute >= 40 && DateTime.Now.Minute <= 45)// Program.ScreenColor = 5855577
                                        {
                                            PhoenixProject.Network.GamePackets.Data datas = new PhoenixProject.Network.GamePackets.Data(true);
                                            datas.UID = client.Entity.UID;
                                            datas.ID = 104;
                                            datas.dwParam = 5855577;
                                            client.Send(datas);
                                        }
                                        else
                                        {
                                            PhoenixProject.Network.GamePackets.Data datas = new PhoenixProject.Network.GamePackets.Data(true);
                                            datas.UID = client.Entity.UID;
                                            datas.ID = 104;
                                            datas.dwParam = 0;
                                            client.Send(datas);
                                        }
                                    }
                                }
                                if (DateTime.Now.DayOfYear > 365)
                                {
                                    client.Disconnect();
                                    return;
                                }
                                if (DateTime.Now.Hour == 16 && DateTime.Now.Minute >= 20 && DateTime.Now.Second == 00)
                                {
                                    if (client.Entity.MapID == 7777)
                                    {
                                        client.Entity.Teleport(1002, 391, 371);
                                    }
                                }

                                if (Now > client.Entity.InviteSendStamp.AddSeconds(5) && client.Entity.invite)
                                {

                                    Game.ClanWar.ScoreSendStamp = Time32.Now;
                                    client.Entity.invite = false;

                                    //Console.WriteLine("a7a");

                                }
                                /*if (Now > LastPopUPCheck.AddMinutes(30))
                                 {
                                     if (!ServerBase.Constants.PKForbiddenMaps.Contains(Owner.Map.BaseID))
                                     {
                                         if (!ServerBase.Constants.PKFreeMaps.Contains(MapID))
                                         {
                                             if (MapID < 1000000)
                                             {
                                                 ActivePOPUP = 99995;
                                                 Owner.Send(new Network.GamePackets.NpcReply(6, "Are You Here? Please Press OK or Cancel To verrify You are Not Using any sort of Bots."));
                                                 LastPopUPCheck = Time32.Now;
                                             }
                                         }
                                     }
                                 }*/
                                if (DateTime.Now.Hour == Game.KimoEvents.EBHour && DateTime.Now.Minute == 05 && DateTime.Now.Second == 15)
                                {
                                    if (DateTime.Now.Hour == Game.KimoEvents.EBHour && DateTime.Now.Minute == 05 && DateTime.Now.Second == 15)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10533,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10533;
                                            client.Send((byte[])alert);
                                            //return;
                                        }
                                    }
                                }
                                if (DateTime.Now.Hour == Game.KimoEvents.DWHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                                {
                                    if (DateTime.Now.Hour == Game.KimoEvents.DWHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                                    {
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "DonationCps War has Started! You Wana Join?");
                                        npc.OptionID = 237;
                                        client.Send(npc.ToArray());
                                        //return;
                                    }
                                    Program.DemonCave3 = 0;
                                }
                                if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    if (DateTime.Now.Hour == Game.KimoEvents.ClanHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                                    {
                                        Program.kimo = 1313;
                                        Program.kimo2 = 7;
                                        testpacket str = new testpacket(true);
                                        client.Send(str);
                                        //ClanWar;
                                    }
                                }
                                if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == (Game.KimoEvents.GWEEndHour - 1) && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                                {
                                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == (Game.KimoEvents.GWEEndHour - 1) && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10515,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10515;
                                            client.Send((byte[])alert);
                                            //return;
                                        }

                                    }
                                }
                                if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                                {
                                    if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10541,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10541;
                                            client.Send((byte[])alert);
                                        }
                                        //return;

                                    }
                                }
                                if (DateTime.Now.Hour == Game.KimoEvents.CFHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                                {
                                    if (DateTime.Now.Hour == Game.KimoEvents.CFHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10535,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10535;
                                            client.Send((byte[])alert);
                                            //return;
                                        }

                                    }
                                }
                                if (DateTime.Now.Hour == Game.KimoEvents.DemonHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                                {
                                    if (DateTime.Now.Hour == Game.KimoEvents.DemonHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                                    {
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "DemonCave Event  has Started! You Wana Join?");
                                        npc.OptionID = 235;
                                        client.Send(npc.ToArray());
                                        //return;

                                    }
                                }
                                if (DateTime.Now.Hour == Game.KimoEvents.LordsWarHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                                {
                                    if (DateTime.Now.Hour == Game.KimoEvents.LordsWarHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                                    {
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "LordsWar Tourment  has Started! You Wana Join?");
                                        npc.OptionID = 233;
                                        client.Send(npc.ToArray());
                                        //return;

                                    }
                                }
                                /*if (DateTime.Now.Hour == Game.KimoEvents.THour && DateTime.Now.Minute == 30 && DateTime.Now.Second == 30)
                                {
                                    if (DateTime.Now.Hour == Game.KimoEvents.THour && DateTime.Now.Minute == 30 && DateTime.Now.Second == 30)
                                    {
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "TreasureBox Event  has Started! You Wana Join?");
                                        npc.OptionID = 238;
                                        client.Send(npc.ToArray());
                                        //return;

                                    }
                                 * //TreasureBox
                                }*/

                                if (DateTime.Now.Hour == Game.KimoEvents.THour && DateTime.Now.Minute == 45 && DateTime.Now.Second == 00)
                                {
                                    if (client.Entity.MapID == 1225)
                                    {
                                        client.Entity.Teleport(1002, 428, 243);
                                        //return;

                                    }
                                }

                                if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 59 && DateTime.Now.Second == 30)
                                {
                                    if (client.Entity.MapID == 4023 || client.Entity.MapID == 4024 || client.Entity.MapID == 4025)
                                    {
                                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("DisCity has finished come Next Day it Start at 21:00 EveryDay!", System.Drawing.Color.White, Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                        client.Entity.Teleport(1002, 430, 378);
                                        //return;
                                    }
                                }
                                if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 45 && DateTime.Now.Second == 00)
                                {
                                    if (client.Entity.MapID == 4023 || client.Entity.MapID == 4024)
                                    {
                                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("All Players in DisCity Stage3 has been Teleported to FinalStage Goodluck!", System.Drawing.Color.White, Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                        client.Entity.Teleport(4025, 150, 286);
                                        client.Inventory.Add(723087, 0, 1);
                                        //return;
                                    }
                                }


                                if (DateTime.Now.Second == 00 && DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == Game.KimoEvents.WHour)
                                {
                                    if (DateTime.Now.Second == 00 && DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == Game.KimoEvents.WHour && DateTime.Now.Minute == 00)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10529,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10529;
                                            client.Send((byte[])alert);
                                            //return;
                                        }
                                    }
                                }
                                if (DateTime.Now.Hour == Game.KimoEvents.ClassHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 30)
                                {
                                    if (client.Entity.Class >= 41 && client.Entity.Class <= 45)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10519,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10519;
                                            client.Send((byte[])alert);
                                        }
                                        //return;
                                    }
                                    if (client.Entity.Class >= 61 && client.Entity.Class <= 65)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10519,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10519;
                                            client.Send((byte[])alert);
                                        }
                                    }
                                    if (client.Entity.Class >= 11 && client.Entity.Class <= 15)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10519,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10519;
                                            client.Send((byte[])alert);
                                        }
                                    }
                                    if (client.Entity.Class >= 21 && client.Entity.Class <= 25)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10519,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10519;
                                            client.Send((byte[])alert);
                                        }
                                    }
                                    if (client.Entity.Class >= 142 && client.Entity.Class <= 145)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10519,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10519;
                                            client.Send((byte[])alert);
                                        }
                                    }
                                    if (client.Entity.Class >= 51 && client.Entity.Class <= 55)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10519,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10519;
                                            client.Send((byte[])alert);
                                        }
                                    }
                                    if (client.Entity.Class >= 132 && client.Entity.Class <= 135)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10519,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10519;
                                            client.Send((byte[])alert);
                                        }
                                    }
                                    if (client.Entity.Class >= 70 && client.Entity.Class <= 75)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10519,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10519;
                                            client.Send((byte[])alert);
                                        }
                                    }

                                }

                                if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    if (DateTime.Now.Minute == 00 && DateTime.Now.Hour == Game.KimoEvents.EGHour && DateTime.Now.Second == 15)
                                    {
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "Elite GuildWar has Started! You Wana Join?");
                                        npc.OptionID = 239;
                                        client.Send(npc.ToArray());
                                        //return;
                                    }
                                }
                                if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00 && DateTime.Now.Hour == Game.KimoEvents.SpouseHour)
                                {
                                    if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00 && DateTime.Now.Hour == Game.KimoEvents.SpouseHour)
                                    {
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "Couples Pk War has Started! You Wana Join?");
                                        npc.OptionID = 241;
                                        //Owner.HeadgearClaim = false;
                                        client.Send(npc.ToArray());
                                        //return;
                                    }
                                }
                                if (DateTime.Now.DayOfYear != client.LastResetTime.DayOfYear)
                                {
                                    if (client.Entity.Level >= 90)
                                    {
                                        client.Entity.EnlightenPoints = 100;
                                        if (client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Knight ||
                                            client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Baron)
                                            client.Entity.EnlightenPoints += 100;
                                        else if (client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Earl ||
                                            client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Duke)
                                            client.Entity.EnlightenPoints += 200;
                                        else if (client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.Prince)
                                            client.Entity.EnlightenPoints += 300;
                                        else if (client.Entity.NobilityRank == PhoenixProject.Game.ConquerStructures.NobilityRank.King)
                                            client.Entity.EnlightenPoints += 400;
                                        if (client.Entity.VIPLevel != 0)
                                        {
                                            if (client.Entity.VIPLevel <= 3)
                                                client.Entity.EnlightenPoints += 100;
                                            else if (client.Entity.VIPLevel <= 5)
                                                client.Entity.EnlightenPoints += 200;
                                            else if (client.Entity.VIPLevel == 6)
                                                client.Entity.EnlightenPoints += 300;
                                        }
                                    }
                                    client.Entity.ReceivedEnlightenPoints = 0;
                                    client.DoubleExpToday = false;
                                    client.ExpBalls = 0;
                                    client.LotteryEntries = 0;
                                    client.Entity.Quest = 0;
                                    client.Entity.SubClassLevel = 0;
                                    client.LastResetTime = DateTime.Now;
                                    client.Send(new FlowerPacket(client.Entity.Flowers));
                                }
                                if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 01)
                                {
                                    if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 01)
                                    {
                                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("DisCity has been started Go to Ape City to signup at SolarSaint!", System.Drawing.Color.White, Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                        PhoenixProject.Game.Features.DisCity.dis = true;
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "DisCity has Started! You Wana Join?");
                                        npc.OptionID = 245;
                                        client.Send(npc.ToArray());
                                        //return;
                                    }
                                }
                                if (DateTime.Now.Minute == 44 && DateTime.Now.Second == 00)
                                {
                                    if (DateTime.Now.Minute == 44 && DateTime.Now.Second == 00)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10525,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10525;
                                            client.Send((byte[])alert);
                                        }
                                    }
                                }



                                if (DateTime.Now.Minute == 30 && DateTime.Now.Second == 00 && !Game.Tournaments.EliteTournament.Start)
                                {
                                    if (DateTime.Now.Minute == 30 && DateTime.Now.Second == 00 && !Game.Tournaments.EliteTournament.Start)
                                    {
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "LastManStanding has Started! You Wana Join?");
                                        npc.OptionID = 240;
                                        client.Send(npc.ToArray());
                                        //return;
                                    }
                                }
                                if (DateTime.Now.Minute == 10 && DateTime.Now.Second == 00)
                                {
                                    if (DateTime.Now.Minute == 10 && DateTime.Now.Second == 00)
                                    {
                                    }
                                }
                                if (DateTime.Now.Minute == 13 && DateTime.Now.Second == 00)
                                {
                                    if (DateTime.Now.Minute == 13 && DateTime.Now.Second == 00)
                                    {
                                    }
                                }
                                if (DateTime.Now.Minute == 40 && DateTime.Now.Second == 00)
                                {
                                    if (DateTime.Now.Minute == 40 && DateTime.Now.Second == 00)
                                    {
                                    }
                                }
                                if (DateTime.Now.Minute == 43 && DateTime.Now.Second == 00)
                                {
                                    if (DateTime.Now.Minute == 43 && DateTime.Now.Second == 00)
                                    {
                                    }
                                }
                                if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00 && !Game.Tournaments.EliteTournament.Start)
                                {
                                    if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00 && !Game.Tournaments.EliteTournament.Start)
                                    {
                                        if (client.Map.BaseID != 6001 && client.Map.BaseID != 6000 && !client.Entity.Dead)
                                        {

                                            EventAlert alert = new EventAlert
                                            {
                                                StrResID = 10531,
                                                Countdown = 30,
                                                UK12 = 1
                                            };
                                            client.Entity.StrResID = 10531;
                                            client.Send((byte[])alert);
                                        }
                                    }
                                }
                                #endregion

                            }
                        }
                    }
                    //else
                    //    client.Disconnect();
                }
                //Thread.Sleep(1000);
            
        }
        static void Kimoz4_Execute()
        {
            
                lock (Values)
                    Values = ServerBase.Kernel.GamePool.Values.ToArray();
                Time32 Now = Time32.Now;
                KimoTime3 = Time32.Now;
                foreach (Client.GameState client in Values)
                {
                    if (client.Socket != null)
                    {
                        if (client.Socket.Connected)
                        {
                            if (client.Entity.HandleTiming)
                            {
                                #region CompanionThread_Execute
                                if (client.Companion != null)
                                {
                                    short distance = ServerBase.Kernel.GetDistance(client.Companion.X, client.Companion.Y, client.Entity.X, client.Entity.Y);
                                    if (distance >= 8)
                                    {
                                        ushort X = (ushort)(client.Entity.X + ServerBase.Kernel.Random.Next(2));
                                        ushort Y = (ushort)(client.Entity.Y + ServerBase.Kernel.Random.Next(2));
                                        if (!client.Map.SelectCoordonates(ref X, ref Y))
                                        {
                                            X = client.Entity.X;
                                            Y = client.Entity.Y;
                                        }
                                        client.Companion.X = X;
                                        client.Companion.Y = Y;
                                        Network.GamePackets.Data data = new PhoenixProject.Network.GamePackets.Data(true);
                                        data.ID = Network.GamePackets.Data.Jump;
                                        data.dwParam = (uint)((Y << 16) | X);
                                        data.wParam1 = X;
                                        data.wParam2 = Y;
                                        data.UID = client.Companion.UID;
                                        client.Companion.MonsterInfo.SendScreen(data);
                                    }
                                    else if (distance > 4)
                                    {
                                        Enums.ConquerAngle facing = ServerBase.Kernel.GetAngle(client.Companion.X, client.Companion.Y, client.Companion.Owner.Entity.X, client.Companion.Owner.Entity.Y);
                                        if (!client.Companion.Move(facing))
                                        {
                                            facing = (Enums.ConquerAngle)ServerBase.Kernel.Random.Next(7);
                                            if (client.Companion.Move(facing))
                                            {
                                                client.Companion.Facing = facing;
                                                Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                                move.Direction = facing;
                                                move.UID = client.Companion.UID;
                                                move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                                client.Companion.MonsterInfo.SendScreen(move);
                                            }
                                        }
                                        else
                                        {
                                            client.Companion.Facing = facing;
                                            Network.GamePackets.GroundMovement move = new PhoenixProject.Network.GamePackets.GroundMovement(true);
                                            move.Direction = facing;
                                            move.UID = client.Companion.UID;
                                            move.GroundMovementType = Network.GamePackets.GroundMovement.Run;
                                            client.Companion.MonsterInfo.SendScreen(move);
                                        }
                                    }
                                    else
                                    {
                                        var monster = client.Companion;

                                        if (monster.MonsterInfo.InSight == 0)
                                        {
                                            if (client.Entity.AttackPacket != null)
                                            {
                                                if (client.Entity.AttackPacket.AttackType == Network.GamePackets.Attack.Magic)
                                                {
                                                    if (client.Entity.AttackPacket.Decoded)
                                                    {
                                                        if (SpellTable.SpellInformations.ContainsKey((ushort)client.Entity.AttackPacket.Damage))
                                                        {
                                                            var info = Database.SpellTable.SpellInformations[(ushort)client.Entity.AttackPacket.Damage][client.Spells[(ushort)client.Entity.AttackPacket.Damage].Level];
                                                            if (info.CanKill == 1)
                                                            {
                                                                monster.MonsterInfo.InSight = client.Entity.AttackPacket.Attacked;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    monster.MonsterInfo.InSight = client.Entity.AttackPacket.Attacked;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (monster.MonsterInfo.InSight > 400000 && monster.MonsterInfo.InSight < 600000 || monster.MonsterInfo.InSight > 800000 && monster.MonsterInfo.InSight != monster.UID)
                                            {
                                                Entity attacked = null;

                                                if (client.Screen.TryGetValue(monster.MonsterInfo.InSight, out attacked))
                                                {
                                                    if (Now > monster.AttackStamp.AddMilliseconds(monster.MonsterInfo.AttackSpeed))
                                                    {
                                                        monster.AttackStamp = Now;
                                                        if (attacked.Dead)
                                                        {
                                                            monster.MonsterInfo.InSight = 0;
                                                        }
                                                        else
                                                            new Game.Attacking.Handle(null, monster, attacked);
                                                    }
                                                }
                                                else
                                                    monster.MonsterInfo.InSight = 0;
                                            }
                                        }

                                    }
                                }
                                #endregion

                            }
                        }
                    }
                    //else
                    //    client.Disconnect();
                }
                //Thread.Sleep(1000);
            
        }
        static void Kimoz3_Execute()
        {
           
                lock (Values)
                    Values = ServerBase.Kernel.GamePool.Values.ToArray();
                Time32 Now = Time32.Now;
                //KimoTime5 = Time32.Now;
                foreach (Client.GameState client in Values)
                {
                    if (client.Socket != null)
                    {
                        if (client.Socket.Connected)
                        {
                            if (client.Entity.HandleTiming)
                            {
                                #region StatusFlagChange_Execute

                                #region Bless
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.CastPray))
                                {
                                    if (client.BlessTime <= 358500)
                                        client.BlessTime += 1500;
                                    else
                                        client.BlessTime = 360000;
                                }
                                else if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Praying))
                                {
                                    if (client.PrayLead != null)
                                    {
                                        if (client.PrayLead.Socket != null)
                                        {
                                            if (client.PrayLead.Socket.Connected)
                                            {
                                                if (client.BlessTime <= 359500)
                                                    client.BlessTime += 500;
                                                else
                                                    client.BlessTime = 360000;
                                            }
                                            else
                                                client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                                        }
                                    }
                                    else
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                                }
                                else
                                {
                                    if (client.BlessTime > 0)
                                    {
                                        if (client.BlessTime >= 500)
                                        {
                                            client.BlessTime -= 500;
                                            client.Entity.Update(Network.GamePackets.Update.LuckyTimeTimer, client.BlessTime, false);
                                        }
                                        else
                                        {
                                            client.BlessTime = 0;
                                            client.Entity.Update(Network.GamePackets.Update.LuckyTimeTimer, client.BlessTime, false);
                                        }
                                    }
                                }

                                #endregion
                                #region Flashing name
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.FlashingName))
                                {
                                    if (Now > client.Entity.FlashingNameStamp.AddSeconds(client.Entity.FlashingNameTime))
                                    {
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.FlashingName);
                                    }
                                }
                                #endregion
                                #region XPList
                                if (!client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.XPList))
                                {
                                    if (Now > client.XPCountStamp.AddSeconds(3))
                                    {
                                        #region Arrows
                                        if (client.Equipment != null)
                                        {
                                            if (!client.Equipment.Free(5))
                                            {
                                                if (Network.PacketHandler.IsArrow(client.Equipment.TryGetItem(5).ID))
                                                {
                                                    Database.ConquerItemTable.UpdateDurabilityItem(client.Equipment.TryGetItem(5));
                                                }
                                            }
                                        }
                                        #endregion
                                        client.XPCountStamp = Now;
                                        client.XPCount++;
                                        if (client.XPCount >= 100)
                                        {
                                            client.Entity.AddFlag(Network.GamePackets.Update.Flags.XPList);
                                            client.XPCount = 0;
                                            client.XPListStamp = Now;
                                        }
                                    }
                                }
                                else
                                {
                                    if (Now > client.XPListStamp.AddSeconds(20))
                                    {
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.XPList);
                                    }
                                }
                                #endregion
                                #region KOSpell
                                if (client.Entity.OnKOSpell())
                                {
                                    if (client.Entity.OnCyclone())
                                    {
                                        int Seconds = Now.AllSeconds() - client.Entity.CycloneStamp.AddSeconds(client.Entity.CycloneTime).AllSeconds();
                                        if (Seconds >= 1)
                                        {
                                            client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Cyclone);
                                            if (client.Entity.KOCount > rates.KoCount)
                                            {
                                                rates.KoCount = client.Entity.KOCount;
                                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations," + client.Entity.Name + " has Killed " + client.Entity.KOCount + " monsters with an Xp Skill and he/she is now ranked #1 on KoRank!", System.Drawing.Color.White, Network.GamePackets.Message.Talk), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                            }
                                            if (client.Entity.KOCount > client.Entity.KoKills)
                                            {
                                                client.Entity.KoKills = client.Entity.KOCount;
                                            }
                                        }
                                    }
                                    if (client.Entity.OnOblivion())
                                    {
                                        if (Now > client.Entity.OblivionStamp.AddSeconds(client.Entity.OblivionTime))
                                        {
                                            client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.Oblivion);
                                            if (client.Entity.KOCount > rates.KoCount)
                                            {
                                                rates.KoCount = client.Entity.KOCount;
                                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations," + client.Entity.Name + " has Killed " + client.Entity.KOCount + " monsters with an Xp Skill and he/she is now ranked #1 on KoRank!", System.Drawing.Color.White, Network.GamePackets.Message.Talk), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                            }
                                            if (client.Entity.KOCount > client.Entity.KoKills)
                                            {
                                                client.Entity.KoKills = client.Entity.KOCount;
                                            }
                                        }
                                    }
                                    if (client.Entity.OnSuperman())
                                    {
                                        int Seconds = Now.AllSeconds() - client.Entity.SupermanStamp.AddSeconds(client.Entity.SupermanTime).AllSeconds();
                                        if (Seconds >= 1)
                                        {
                                            client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Superman);
                                            if (client.Entity.KOCount > rates.KoCount)
                                            {
                                                rates.KoCount = client.Entity.KOCount;
                                                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("Congratulations," + client.Entity.Name + " has Killed " + client.Entity.KOCount + " monsters with an Xp Skill and he/she is now ranked #1 on KoRank!", System.Drawing.Color.White, Network.GamePackets.Message.Talk), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                            }
                                            if (client.Entity.KOCount > client.Entity.KoKills)
                                            {
                                                client.Entity.KoKills = client.Entity.KOCount;
                                            }
                                        }
                                    }
                                    if (!client.Entity.OnKOSpell())
                                    {
                                        //Record KO
                                        client.Entity.KOCount = 0;
                                    }
                                }
                                #endregion
                                #region Buffers

                                if (client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.WarriorWalk))
                                {
                                    if (Now >= client.Entity.DefensiveStanceStamp.AddSeconds(client.Entity.DefensiveStanceTime))
                                    {
                                        client.Entity.DefensiveStanceTime = 0;
                                        client.Entity.DefensiveStanceIncrease = 0;
                                        client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.WarriorWalk);
                                    }
                                }
                                if (client.Entity.ContainsFlag3(Network.GamePackets.Update.Flags3.MagicDefender))
                                {
                                    if (Now >= client.Entity.MagicDefenderStamp.AddSeconds(client.Entity.MagicDefenderTime))
                                    {
                                        client.Entity.MagicDefenderTime = 0;
                                        client.Entity.MagicDefenderIncrease = 0;
                                        client.Entity.RemoveFlag3(Network.GamePackets.Update.Flags3.MagicDefender);
                                        SyncPacket packet = new SyncPacket
                                        {
                                            Identifier = client.Entity.UID,
                                            Count = 2,
                                            Type = PhoenixProject.Network.GamePackets.SyncPacket.SyncType.StatusFlag,
                                            StatusFlag1 = (ulong)client.Entity.StatusFlag,
                                            StatusFlag2 = (ulong)client.Entity.StatusFlag2,
                                            Unknown1 = 0x31,
                                            StatusFlagOffset = 0x80,
                                            Time = 0,
                                            Value = 0,
                                            Level = 0
                                        };
                                        client.Send((byte[])packet);

                                        foreach (var Client in client.MagicDef)
                                        {
                                            if (Client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.kimo4))
                                            {
                                                Client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.kimo4);
                                            }
                                        }
                                        client.MagicDef.Clear();
                                    }
                                }


                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Stigma))
                                {
                                    if (Now >= client.Entity.StigmaStamp.AddSeconds(client.Entity.StigmaTime))
                                    {
                                        client.Entity.StigmaTime = 0;
                                        client.Entity.StigmaIncrease = 0;
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Stigma);
                                    }
                                }
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Dodge))
                                {
                                    if (Now >= client.Entity.DodgeStamp.AddSeconds(client.Entity.DodgeTime))
                                    {
                                        client.Entity.DodgeTime = 0;
                                        client.Entity.DodgeIncrease = 0;
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Dodge);
                                    }
                                }
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Invisibility))
                                {
                                    if (Now >= client.Entity.InvisibilityStamp.AddSeconds(client.Entity.InvisibilityTime))
                                    {
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Invisibility);
                                    }
                                }
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.StarOfAccuracy))
                                {
                                    if (client.Entity.StarOfAccuracyTime != 0)
                                    {
                                        if (Now >= client.Entity.StarOfAccuracyStamp.AddSeconds(client.Entity.StarOfAccuracyTime))
                                        {
                                            client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.StarOfAccuracy);
                                        }
                                    }
                                    else
                                    {
                                        if (Now >= client.Entity.AccuracyStamp.AddSeconds(client.Entity.AccuracyTime))
                                        {
                                            client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.StarOfAccuracy);
                                        }
                                    }
                                }
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.MagicShield))
                                {
                                    if (client.Entity.MagicShieldTime != 0)
                                    {
                                        if (Now >= client.Entity.MagicShieldStamp.AddSeconds(client.Entity.MagicShieldTime))
                                        {
                                            client.Entity.MagicShieldIncrease = 0;
                                            client.Entity.MagicShieldTime = 0;
                                            client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.MagicShield);
                                        }
                                    }
                                    else
                                    {
                                        if (Now >= client.Entity.ShieldStamp.AddSeconds(client.Entity.ShieldTime))
                                        {
                                            client.Entity.ShieldIncrease = 0;
                                            client.Entity.ShieldTime = 0;
                                            client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.MagicShield);
                                        }
                                    }
                                }
                                #endregion
                                #region Fly
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Fly))
                                {
                                    if (Now >= client.Entity.FlyStamp.AddSeconds(client.Entity.FlyTime))
                                    {
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Fly);
                                        client.Entity.FlyTime = 0;
                                    }
                                }
                                #endregion
                                #region PoisonStar
                                if (client.Entity.NoDrugsTime > 0)
                                {
                                    if (Now > client.Entity.NoDrugsStamp.AddSeconds(client.Entity.NoDrugsTime))
                                    {
                                        client.Entity.NoDrugsTime = 0;
                                    }
                                }
                                #endregion
                                #region ToxicFog
                                if (client.Entity.ToxicFogLeft > 0)
                                {
                                    if (Now >= client.Entity.ToxicFogStamp.AddSeconds(2))
                                    {
                                        float Percent = client.Entity.ToxicFogPercent;
                                        //Remove this line if you want it normal
                                        Percent = Math.Min(0.1F, client.Entity.ToxicFogPercent);
                                        client.Entity.ToxicFogLeft--;
                                        client.Entity.ToxicFogStamp = Now;
                                        if (client.Entity.Hitpoints > 1)
                                        {
                                            uint damage = Game.Attacking.Calculate.Percent(client.Entity, Percent);
                                            client.Entity.Hitpoints -= damage;
                                            Network.GamePackets.SpellUse suse = new PhoenixProject.Network.GamePackets.SpellUse(true);
                                            suse.Attacker = client.Entity.UID;
                                            suse.SpellID = 10010;
                                            suse.Targets.Add(client.Entity.UID, damage);
                                            client.SendScreen(suse, true);
                                            if (client.QualifierGroup != null)
                                                client.QualifierGroup.UpdateDamage(ServerBase.Kernel.GamePool[client.ArenaStatistic.PlayWith], damage);
                                        }
                                    }
                                }
                                #endregion
                                if (client.Entity.OnBlackBread())
                                {
                                    //int Seconds = Now.AllSeconds() - client.Entity.BlackBeardStamp.AddSeconds(client.Entity.Blackbeard).AllSeconds();
                                    if (Now > client.Entity.BlackBeardStamp.AddSeconds(client.Entity.Blackbeard))
                                    {
                                        client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.BlackBread);
                                    }
                                }
                                if (client.Entity.OnChainBolt())
                                {
                                    //int Seconds = Now.AllSeconds() - client.Entity.BlackBeardStamp.AddSeconds(client.Entity.Blackbeard).AllSeconds();
                                    if (Now > client.Entity.ChainBoltStamp.AddSeconds(client.Entity.ChainBoltTime))
                                    {
                                        client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.ChainBoltActive);
                                    }
                                }
                                if (client.Entity.OnCannonBrag())
                                {
                                    // int Seconds = Now.AllSeconds() - client.Entity.CannonBarageStamp.AddSeconds(client.Entity.Cannonbarage).AllSeconds();
                                    if (Now > client.Entity.CannonBarageStamp.AddSeconds(client.Entity.Cannonbarage))
                                    {
                                        client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.CannonBraga);
                                    }
                                }
                                #region FatalStrike
                                if (client.Entity.OnFatalStrike())
                                {
                                    if (Now > client.Entity.FatalStrikeStamp.AddSeconds(client.Entity.FatalStrikeTime))
                                    {
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.FatalStrike);
                                    }
                                }
                                #endregion

                                #region ShurikenVortex
                                if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                                {
                                    if (Now > client.Entity.ShurikenVortexStamp.AddSeconds(client.Entity.ShurikenVortexTime))
                                    {
                                        client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.ShurikenVortex);
                                    }
                                }
                                #endregion
                                #region Transformations
                                if (client.Entity.Transformed)
                                {
                                    if (Now > client.Entity.TransformationStamp.AddSeconds(client.Entity.TransformationTime))
                                    {
                                        client.Entity.Untransform();
                                    }
                                }
                                #endregion
                                #endregion

                            }
                        }
                    }
                    //else
                    //    client.Disconnect();
                }
                //Thread.Sleep(1000);
            
        }
        static void Kimoz2_Execute()
        {
            
                lock (Values)
                    Values = ServerBase.Kernel.GamePool.Values.ToArray();
                Time32 Now = Time32.Now;
               // KimoTime6 = Time32.Now;
                foreach (Client.GameState client in Values)
                {
                    if (client.Socket != null)
                    {
                        if (client.Socket.Connected)
                        {
                            if (client.Entity.HandleTiming)
                            {
                                #region BlessThread_Execute
                                if (client.Screen == null || client.Entity == null)
                                {
                                    client.Disconnect();
                                    continue;
                                }
                                if (client.Socket.Connected)
                                {
                                    for (int c = 0; c < client.Screen.Objects.Count; c++)
                                    {
                                        if (c >= client.Screen.Objects.Count)
                                            break;
                                        Interfaces.IMapObject ClientObj = client.Screen.Objects[c];
                                        if (ClientObj != null)
                                        {
                                            if (ClientObj.MapObjType == PhoenixProject.Game.MapObjectType.Player)
                                            {
                                                if (ClientObj is Game.Entity)
                                                {
                                                    if (ClientObj.Owner.Entity.BlackSpotTime2 > 0)
                                                    {
                                                        BlackSpot spot = new BlackSpot
                                                        {
                                                            Remove = 0,
                                                            Identifier = ClientObj.UID
                                                        };
                                                        client.Send(spot);
                                                    }
                                                    else
                                                    {
                                                        BlackSpot spot = new BlackSpot
                                                        {
                                                            Remove = 1,
                                                            Identifier = ClientObj.UID
                                                        };
                                                        client.Send(spot);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (!client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.Praying) && client.Entity.Reborn < 2)
                                    {
                                        for (int c = 0; c < client.Screen.Objects.Count; c++)
                                        {
                                            if (c >= client.Screen.Objects.Count)
                                                break;
                                            Interfaces.IMapObject ClientObj = client.Screen.Objects[c];
                                            if (ClientObj != null)
                                            {
                                                if (ClientObj is Game.Entity)
                                                {
                                                    if (ClientObj.MapObjType == PhoenixProject.Game.MapObjectType.Player)
                                                    {
                                                        var Client = ClientObj.Owner;
                                                        if (Client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.CastPray))
                                                        {
                                                            if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, ClientObj.X, ClientObj.Y) <= 3)
                                                            {
                                                                client.Entity.AddFlag(Network.GamePackets.Update.Flags.Praying);
                                                                client.PrayLead = Client;
                                                                client.Entity.Action = Client.Entity.Action;
                                                                Client.Prayers.Add(client);
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (client.PrayLead != null)
                                        {
                                            if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, client.PrayLead.Entity.X, client.PrayLead.Entity.Y) > 4)
                                            {
                                                client.Entity.RemoveFlag(Network.GamePackets.Update.Flags.Praying);
                                                client.PrayLead.Prayers.Remove(client);
                                                client.PrayLead = null;
                                            }
                                        }
                                    }
                                    if (!client.Entity.ContainsFlag2(Network.GamePackets.Update.Flags2.kimo4))
                                    {
                                        for (int c = 0; c < client.Screen.Objects.Count; c++)
                                        {
                                            if (c >= client.Screen.Objects.Count)
                                                break;
                                            Interfaces.IMapObject ClientObj = client.Screen.Objects[c];
                                            if (ClientObj != null)
                                            {
                                                if (ClientObj is Game.Entity)
                                                {
                                                    if (ClientObj.MapObjType == PhoenixProject.Game.MapObjectType.Player)
                                                    {
                                                        var Client = ClientObj.Owner;

                                                        if (Client.Entity.ContainsFlag3(Network.GamePackets.Update.Flags3.MagicDefender))
                                                        {
                                                            if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, ClientObj.X, ClientObj.Y) <= 3)
                                                            {
                                                                client.Entity.AddFlag2(Network.GamePackets.Update.Flags.kimo4);
                                                                client.MagicLead = Client;
                                                                client.Entity.Action = PhoenixProject.Game.Enums.ConquerAction.Sit;
                                                                Client.MagicDef.Add(client);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (client.MagicLead != null)
                                        {
                                            if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, client.MagicLead.Entity.X, client.MagicLead.Entity.Y) > 4)
                                            {
                                                client.Entity.RemoveFlag2(Network.GamePackets.Update.Flags2.kimo4);
                                                client.MagicLead.MagicDef.Remove(client);
                                                client.MagicLead = null;
                                            }
                                        }
                                    }
                                }
                                #endregion

                            }
                        }
                    }
                    //else
                    //    client.Disconnect();
                }
               // Thread.Sleep(1000);
            
        }
        static void Kimoz1_Execute()
        {
           

                   
                lock (Values)
                    Values = ServerBase.Kernel.GamePool.Values.ToArray();
                Time32 Now = Time32.Now;
                KimoTime7 = Time32.Now;
                foreach (Client.GameState client in Values)
                {
                    if (client.Socket != null)
                    {
                        if (client.Socket.Connected)
                        {
                            if (client.Entity.HandleTiming)
                            {
                                if (DateTime.Now.Minute == 42 && DateTime.Now.Second == 58)
                                {
                                    if (Database.rates.Weather == 1)
                                    {

                                        Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                                        weather.WeatherType = (uint)Program.WeatherType;
                                        weather.Intensity = 100;
                                        weather.Appearence = 2;
                                        weather.Direction = 4;
                                        client.Send(weather);

                                    }
                                }
                                if (DateTime.Now.Minute == 57 && DateTime.Now.Second == 58)
                                {
                                    if (Database.rates.Weather == 1)
                                    {

                                        Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                                        weather.WeatherType = (uint)Program.WeatherType;
                                        weather.Intensity = 100;
                                        weather.Appearence = 2;
                                        weather.Direction = 4;
                                        client.Send(weather);

                                    }
                                }
                                if (DateTime.Now.Minute == 12 && DateTime.Now.Second == 58)
                                {
                                    if (Database.rates.Weather == 1)
                                    {

                                        Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                                        weather.WeatherType = (uint)Program.WeatherType;
                                        weather.Intensity = 100;
                                        weather.Appearence = 2;
                                        weather.Direction = 4;
                                        client.Send(weather);

                                    }
                                }
                                if (DateTime.Now.Minute == 27 && DateTime.Now.Second == 58)
                                {
                                    if (Database.rates.Weather == 1)
                                    {


                                        Network.GamePackets.Weather weather = new Network.GamePackets.Weather(true);
                                        weather.WeatherType = (uint)Program.WeatherType;
                                        weather.Intensity = 100;
                                        weather.Appearence = 2;
                                        weather.Direction = 4;
                                        client.Send(weather);

                                    }
                                }
                                if (client.Entity.MapID == 1036)
                                {
                                    if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, 184, 205) < 17 && !client.Effect)
                                    {
                                        client.Effect = true;
                                        if (client.Entity.MapID == 1036)
                                        {
                                            Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                            // floorItem.MapObjType = Game.MapObjectType.Item;
                                            floorItem.ItemID = 812;//794
                                            floorItem.MapID = 1036;
                                            floorItem.X = 184;
                                            floorItem.Y = 205;
                                            floorItem.Type = Network.GamePackets.FloorItem.Effect;
                                            client.Send(floorItem);
                                        }
                                    }
                                    else
                                    {
                                        if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, 184, 205) > 17)
                                        {
                                            client.Effect = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (client.Entity.MapID == 1002)
                                    {
                                        if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, 438, 377) < 17 && !client.Effect)
                                        {
                                            client.Effect = true;
                                            if (client.Entity.MapID == 1002)
                                            {
                                                Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                                floorItem.ItemID = 23;//746
                                                floorItem.MapID = 1002;
                                                floorItem.X = 438;
                                                floorItem.Y = 377;
                                                floorItem.Type = 10;
                                                client.Send(floorItem);
                                                floorItem.ItemID = 31;//794
                                                floorItem.MapID = 1002;
                                                floorItem.X = 438;
                                                floorItem.Y = 377;
                                                floorItem.Type = 10;
                                                client.Send(floorItem);
                                            }
                                        }
                                        else
                                        {
                                            if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, 438, 377) > 17)
                                            {
                                                client.Effect = false;
                                            }
                                        }
                                        if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, 436, 444) < 17 && !client.Effect3)
                                        {
                                            client.Effect3 = true;
                                            if (client.Entity.MapID == 1002)
                                            {
                                                Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                                // floorItem.MapObjType = Game.MapObjectType.Item;
                                                floorItem.ItemID = 765;//794
                                                floorItem.MapID = 1002;
                                                floorItem.X = 436;
                                                floorItem.Y = 444;
                                                floorItem.Type = Network.GamePackets.FloorItem.Effect;

                                                client.Send(floorItem);
                                            }
                                        }
                                        else
                                        {
                                            if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, 436, 444) > 17)
                                            {
                                                client.Effect3 = false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (client.Entity.MapID == 1005)
                                        {
                                            if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, 42, 48) < 17 && !client.Effect2)
                                            {
                                                client.Effect2 = true;
                                                if (client.Entity.MapID == 1005)
                                                {//1005 42 50 790
                                                    Network.GamePackets.FloorItem floorItem = new Network.GamePackets.FloorItem(true);
                                                    // floorItem.MapObjType = Game.MapObjectType.Item;
                                                    floorItem.ItemID = 797;//794
                                                    floorItem.MapID = 1005;
                                                    floorItem.X = 42;
                                                    floorItem.Y = 48;
                                                    floorItem.Type = Network.GamePackets.FloorItem.Effect;
                                                    client.Send(floorItem);
                                                    // floorItem.MapObjType = Game.MapObjectType.Item;
                                                    floorItem.ItemID = 731;//794
                                                    floorItem.MapID = 1005;
                                                    floorItem.X = 42;
                                                    floorItem.Y = 51;
                                                    floorItem.Type = Network.GamePackets.FloorItem.Effect;
                                                    client.Send(floorItem);
                                                }
                                            }
                                            else
                                            {
                                                if (ServerBase.Kernel.GetDistance(client.Entity.X, client.Entity.Y, 42, 48) > 17)
                                                {
                                                    client.Effect2 = false;
                                                }
                                            }

                                        }
                                    }
                                }
                                // Console.WriteLine(" " + UID + " ");
                                #region Auto attack
                                if (client.Entity.AttackPacket != null || client.Entity.VortexAttackStamp != null)
                                {
                                    try
                                    {
                                        if (client.Entity.ContainsFlag(Network.GamePackets.Update.Flags.ShurikenVortex))
                                        {
                                            if (client.Entity.VortexPacket != null && client.Entity.VortexPacket.ToArray() != null)
                                            {
                                                if (Time32.Now > client.Entity.VortexAttackStamp.AddMilliseconds(1400))
                                                {
                                                    client.Entity.VortexAttackStamp = Time32.Now;
                                                    new Game.Attacking.Handle(client.Entity.VortexPacket, client.Entity, null);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            client.Entity.VortexPacket = null;
                                            var AttackPacket = client.Entity.AttackPacket;
                                            if (AttackPacket != null && AttackPacket.ToArray() != null)
                                            {
                                                uint AttackType = AttackPacket.AttackType;
                                                if (AttackType == Network.GamePackets.Attack.Magic || AttackType == Network.GamePackets.Attack.Melee || AttackType == Network.GamePackets.Attack.Ranged)
                                                {
                                                    if (AttackType == Network.GamePackets.Attack.Magic)
                                                    {
                                                        if (Time32.Now > client.Entity.AttackStamp.AddSeconds(1))
                                                        {

                                                            new Game.Attacking.Handle(AttackPacket, client.Entity, null);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        int decrease = -300;
                                                        if (client.Entity.OnCyclone())
                                                            decrease = 700;
                                                        if (client.Entity.OnSuperman())
                                                            decrease = 200;
                                                        if (Time32.Now > client.Entity.AttackStamp.AddMilliseconds((1000 - client.Entity.Agility - decrease) * (int)(AttackType == Network.GamePackets.Attack.Ranged ? 1 : 1)))
                                                        {
                                                            new Game.Attacking.Handle(AttackPacket, client.Entity, null);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        Program.SaveException(e);
                                        client.Entity.AttackPacket = null;
                                        client.Entity.VortexPacket = null;
                                    }
                                }
                                #endregion

                            }
                        }
                    }
                    //else
                    //    client.Disconnect();
                }
                //Thread.Sleep(100);
           
        }
        
        static void SystemMessages_Execute()
        {
            //Console.WriteLine("kimz");
            if (ServerRrestart == false)
            {

                #region AUTOMATED SYSTEM MESSAGES FIXED
                if (DateTime.Now > m_msgDate.AddMinutes(10))
                {
                    m_msgDate = DateTime.Now;
                    string Msg = "";
                    switch (m_Counter)
                    {
                        case 0: Msg = "" + Database.Messagess.Sys + ""; m_Counter++; break;
                        case 1: Msg = "" + Database.Messagess.Sys2 + ""; m_Counter++; break;
                        case 2: Msg = "" + Database.Messagess.Sys3 + ""; m_Counter++; break;
                        case 3: Msg = "" + Database.Messagess.Sys4 + ""; m_Counter++; break;
                        case 4: Msg = "" + Database.Messagess.Sys5 + ""; m_Counter++; break;
                        case 5: Msg = "" + Database.Messagess.Sys6 + ""; m_Counter++; break;
                        case 6: Msg = "" + Database.Messagess.Sys7 + ""; m_Counter++; break;
                        case 7: Msg = "" + Database.Messagess.Sys8 + ""; m_Counter++; break;
                        case 8: Msg = "" + Database.Messagess.Sys9 + ""; m_Counter = 0; break;
                        default: return;
                    }
                    Network.PacketHandler.WorldMessage(Msg);

                }
                #endregion
            }
        }
        static void ServerStuff_Execute()
        {
            if (ServerRrestart == false)
            {
                //Console.Title = "["+Database.rates.servername+"]Phoenix Conquer Project. Start time: " + StartDate.ToString("dd MM yyyy hh:mm") + ". Players online: " + ServerBase.Kernel.GamePool.Count + "/" + PlayerCap;
                // Console.WriteLine("kimozzzzzz");
                // new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("configuration").Set("GuildID", Game.ConquerStructures.Society.Guild.GuildCounter.Now).Set("ItemUID", Network.GamePackets.ConquerItem.ItemUID.Now).Where("Server", ServerBase.Constants.ServerName).Execute();
                //var Values = ServerBase.Kernel.WasInGamePool.Base.ToArray();

                if (DateTime.Now > Game.ConquerStructures.Broadcast.LastBroadcast.AddMinutes(1))
                {
                    if (Game.ConquerStructures.Broadcast.Broadcasts.Count > 0)
                    {
                        Game.ConquerStructures.Broadcast.CurrentBroadcast = Game.ConquerStructures.Broadcast.Broadcasts[0];
                        Game.ConquerStructures.Broadcast.Broadcasts.Remove(Game.ConquerStructures.Broadcast.CurrentBroadcast);
                        Game.ConquerStructures.Broadcast.LastBroadcast = DateTime.Now;
                        ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message(Game.ConquerStructures.Broadcast.CurrentBroadcast.Message, "ALLUSERS", Game.ConquerStructures.Broadcast.CurrentBroadcast.EntityName, System.Drawing.Color.Red, Network.GamePackets.Message.BroadcastMessage), ServerBase.Kernel.GamePool.Values);
                    }
                    else
                        Game.ConquerStructures.Broadcast.CurrentBroadcast.EntityID = 1;
                }

                DateTime Now = DateTime.Now;

                if (Now > LastRandomReset.AddMinutes(30))
                {
                    LastRandomReset = Now;
                    ServerBase.Kernel.Random = new Random(RandomSeed);
                }
                Today = Now.DayOfWeek;
                if (Now >= StartDate.AddHours(12))
                 {
                     if (mess == 0)
                     {
                         foreach (Client.GameState Server in ServerBase.Kernel.GamePool.Values)
                         {
                             //if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Wednesday || DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                             //{

                             PhoenixProject.Network.GamePackets.Message FiveMinute = new Network.GamePackets.Message("The server will be brought down for maintenance in 5 Minutes. Please exit the game now.", System.Drawing.Color.Red, Network.GamePackets.Message.Center);

                             Server.Send(FiveMinute);

                             // }
                         }
                         mess++;
                         messtime = Time32.Now;
                     }
                     if (mess == 1 && Time32.Now >= messtime.AddMinutes(1))
                     {
                         foreach (Client.GameState Server in ServerBase.Kernel.GamePool.Values)
                         {

                             PhoenixProject.Network.GamePackets.Message FiveMinute = new Network.GamePackets.Message("The server will be brought down for maintenance in 4 Minutes. Please exit the game now.", System.Drawing.Color.Red, Network.GamePackets.Message.Center);
                              Server.Send(FiveMinute);

                         }
                         mess++;
                         messtime = Time32.Now;
                     }
                     if (mess == 2 && Time32.Now >= messtime.AddMinutes(1))
                     {
                         foreach (Client.GameState Server in ServerBase.Kernel.GamePool.Values)
                         {

                             PhoenixProject.Network.GamePackets.Message FiveMinute = new Network.GamePackets.Message("The server will be brought down for maintenance in 3 Minutes. Please exit the game now.", System.Drawing.Color.Red, Network.GamePackets.Message.Center);
                             Server.Send(FiveMinute);

                         }
                         mess++;
                         messtime = Time32.Now;
                     }
                     if (mess == 3 && Time32.Now >= messtime.AddMinutes(1))
                     {
                         foreach (Client.GameState Server in ServerBase.Kernel.GamePool.Values)
                         {

                             PhoenixProject.Network.GamePackets.Message FiveMinute = new Network.GamePackets.Message("The server will be brought down for maintenance in 2 Minutes. Please exit the game now.", System.Drawing.Color.Red, Network.GamePackets.Message.Center);
                             Server.Send(FiveMinute);

                         }
                         mess++;
                         messtime = Time32.Now;
                     }
                     if (mess == 4 && Time32.Now >= messtime.AddMinutes(1))
                     {
                         foreach (Client.GameState Server in ServerBase.Kernel.GamePool.Values)
                         {

                             PhoenixProject.Network.GamePackets.Message FiveMinute = new Network.GamePackets.Message("The server will be brought down for maintenance in 1 Minute. Please exit the game now.", System.Drawing.Color.Red, Network.GamePackets.Message.Center);
                             Server.Send(FiveMinute);

                         }
                         mess++;
                         messtime = Time32.Now;
                     }
                }
                if (Now >= StartDate.AddHours(12) && !restarted && mess == 5)
                {
                    CommandsAI("@restart");
                    restarted = true;
                    ServerRrestart = true;
                    return;
                }
                  var Values = ServerBase.Kernel.WasInGamePool.Base.ToArray();

                  foreach (KeyValuePair<uint, Client.GameState> vals in Values)
                  {
                      Client.GameState client = vals.Value;

                      if (client == null || client.Entity == null || client.Account == null)
                      {
                          ServerBase.Kernel.WasInGamePool.Remove(vals.Key);
                          //Console.WriteLine("kimo4");
                          return;
                      }
                      if (client.Disconnected2 == true)
                          return;
                      if (client.Socket != null)
                      {
                       
                          if (!client.Socket.Connected)
                          {
                              Database.EntityTable.SaveEntity(client);
                              Database.SkillTable.SaveProficiencies(client);
                              Database.SkillTable.SaveSpells(client);
                              Database.ArenaTable.SaveArenaStatistics(client.ArenaStatistic);
                              ServerBase.Kernel.WasInGamePool.Remove(vals.Key);
                              Database.EntityTable.UpdateOnlineStatus(client, false);
                              if (ServerBase.Kernel.GamePool.ContainsKey(vals.Key))
                              {
                                  ServerBase.Kernel.GamePool.Remove(vals.Key);
                              }
                              if (ServerBase.Kernel.WasInGamePool.ContainsKey(vals.Key))
                              {
                                  ServerBase.Kernel.WasInGamePool.Remove(vals.Key);
                              }
                             
                              /*if (ServerBase.Kernel.AwaitingPool.ContainsKey(vals.Key))
                              {
                                  ServerBase.Kernel.AwaitingPool.Remove(vals.Key);
                              }*/
                              // Console.WriteLine("kimo5");
                              //Database.FlowerSystemTable.SaveFlowerTable(client);
                              if (client.Socket != null)
                              {
                                  if (!client.SocketDisposed)
                                  {

                                      // Monitor.Exit(_socket);
                                      // Monitor.Exit(Cryptography);
                                      // Console.WriteLine(" Close4 ");
                                      client.SocketDisposed = true;
                                      client.Socket.Disconnect(false);
                                      client.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                                      client.Socket.Close();
                                  }
                              }
                          }
                      }
                      else
                      {
                          
                          Database.EntityTable.SaveEntity(client);
                          Database.SkillTable.SaveProficiencies(client);
                          Database.SkillTable.SaveSpells(client);
                          Database.ArenaTable.SaveArenaStatistics(client.ArenaStatistic);
                          if (ServerBase.Kernel.WasInGamePool.ContainsKey(vals.Key))
                          {
                              ServerBase.Kernel.WasInGamePool.Remove(vals.Key);
                          }
                          if (ServerBase.Kernel.GamePool.ContainsKey(vals.Key))
                          {
                              ServerBase.Kernel.GamePool.Remove(vals.Key);
                          }
                          Database.EntityTable.UpdateOnlineStatus(client, false);
                         
                      }
                  }
            

                // Thread.Sleep(1000);
            }
        }
        static void kimo_Execute()
        {

            if(ServerRrestart == false)
            {
            lock (Values)
                Values = ServerBase.Kernel.GamePool.Values.ToArray();

            /* if (DateTime.Now >= Refresh.AddMinutes(5))
             {
                // KillConnections.Kill();
                 Refresh = DateTime.Now;
                 GC.Collect();
                 Console.WriteLine("MaxGeneration " + GC.MaxGeneration + " ");
                 AuthServer.Enable();
                 GameServer.Enable();
                   
             }*/

            if (DateTime.Now.DayOfYear > 365)
            {
                CommandsAI("@exit");
                return;
            }
            /* if (DateTime.Now.Minute == 58 && DateTime.Now.Second == 00 && !Game.ConquerStructures.QuizShow.QuizON)
             {
                 Game.ConquerStructures.QuizShow.Start2();
                 QuizStamp = Time32.Now;
             }
             if (Time32.Now > Program.QuizStamp.AddSeconds(30) && Game.ConquerStructures.QuizShow.QuizON)
             {
                 Game.ConquerStructures.QuizShow.KimoQuiz();
                 QuizStamp = Time32.Now;
             }*/
            /* if (Program.PoolSize >= 100000)
             {
                 System.Data.SqlClient.SqlConnection.ClearAllPools();
                 Program.PoolSize = 0;
             }*/
            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
            {
                if (Program.Carnaval == 0)
                {
                    Game.KimoCarnaval.Load();
                    Program.Carnaval = 1;

                }
            }
            if (DateTime.Now.Minute == 59 && DateTime.Now.Second == 00)
            {
                if (Program.Carnaval == 1)
                {
                    // Game.KimoCarnaval.Load();
                    Program.Carnaval = 0;
                }
                ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("TQEnvoy will apear in TwinCity after 1 Minute and DropParty will Start Hurry go to TC to Get some Gifts", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
            }
            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 03)
            {

                Game.KimoCarnaval.Load2();

            }
            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 06)
            {

                Game.KimoCarnaval.Load3();

            }
            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 09)
            {

                Game.KimoCarnaval.Load4();

            }
            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 12)
            {

                Game.KimoCarnaval.Load5();

            }
            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 15)
            {

                Game.KimoCarnaval.Load6();

            }
            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 18)
            {

                Game.KimoCarnaval.Load7();

            }
            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 21)
            {

                Game.KimoCarnaval.Load8();

            }
            if (DateTime.Now.Minute == 00 && DateTime.Now.Second == 24)
            {

                Game.KimoCarnaval.Load9();
                ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("TQEnvoy Drop Event ended come back next hour , it apear every hour at xx:00 goodluck", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Talk), ServerBase.Kernel.GamePool.Values);

            }
            if (DateTime.Now.Hour == 12 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 40)
            {
                if (Program.Carnaval2 == 1)
                {
                    //
                    Program.Carnaval2 = 0;
                }
            }
            if (DateTime.Now.Hour == 12 && DateTime.Now.Minute == 00 && DateTime.Now.Second == 45)
            {
                if (Program.Carnaval2 == 0)
                {
                    PhoenixProject.ServerBase.Kernel.VotePoolUid.Clear();
                    PhoenixProject.ServerBase.Kernel.VotePool.Clear();
                    Database.EntityTable.DeletVotes();
                    Program.Carnaval2 = 1;
                }
            }
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59 && DateTime.Now.Second == 40)
            {
                if (Program.Carnaval3 == 1)
                {
                    //
                    Program.Carnaval3 = 0;
                }
            }
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59 && DateTime.Now.Second == 45)
            {
                if (Program.Carnaval3 == 0)
                {
                    PhoenixProject.ServerBase.Kernel.VotePoolUid.Clear();
                    PhoenixProject.ServerBase.Kernel.VotePool.Clear();
                    Database.EntityTable.DeletVotes();
                    Program.Carnaval3 = 1;
                }
            }
            if (DateTime.Now.Hour == Game.KimoEvents.CFHour && DateTime.Now.Minute >= 00 && DateTime.Now.Minute < 30)
            {
                if (!Game.Team.IsWar)
                {
                    Game.Team.IsWar = true;
                    Game.Team.RScore = 0;
                    Game.Team.WScore = 0;
                    Game.Team.BScore = 0;
                    Game.Team.BLScore = 0;
                    Game.Team.RedCapture = false;
                    ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("CaptureTheFlag , has started go to TwinCity to Signup b4 " + Game.KimoEvents.CFHour + ":30", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                }
            }
            if (DateTime.Now.Hour == Game.KimoEvents.CFHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.CFHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                {
                    Game.Team.IsWar = true;
                    Game.Team.RScore = 0;
                    Game.Team.WScore = 0;
                    Game.Team.BScore = 0;
                    Game.Team.BLScore = 0;
                    Game.Team.RedCapture = false;
                    ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("CaptureTheFlag , has started go to TwinCity to Signup b4 " + Game.KimoEvents.CFHour + ":30", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                }
            }
            if (DateTime.Now.Hour == Game.KimoEvents.CFHour && DateTime.Now.Minute == 30 && DateTime.Now.Second == 00)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.CFHour && DateTime.Now.Minute == 30 && DateTime.Now.Second == 00)
                {
                    Game.Team.IsWar = false;
                    ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("CaptureTheFlag  Ended! Come again Tomorrow", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                }
            }
            if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 32)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 32)
                {
                    Game.KimoSkillWar.Start();
                }
            }
            /*if (GameIP != "25.150.192.102")
            {
                CommandsAI("@exit");
            }*/
            if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 29 && DateTime.Now.Second == 00)
            {
                if (Program.Carnaval4 == 1)
                {
                    //
                    Program.Carnaval4 = 0;
                }
            }
            if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 30 && DateTime.Now.Second == 00)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 30 && DateTime.Now.Second == 00)
                {
                    if (Program.Carnaval4 == 0)
                    {
                        Program.Carnaval4 = 1;
                        Game.KimoSkillWar.END();
                        if (Game.KimoSkillWar.RKills > Game.KimoSkillWar.YKills)
                        {
                            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                            {
                                if (client.Entity.MapID == Game.KimoSkillWar.MapID)
                                {
                                    if (client.YellowOn == true)
                                    {
                                        client.Entity.Teleport(1002, 429, 378);
                                        Game.KimoSkillWar.YTeamNum = 0;
                                    }
                                    else
                                    {
                                        client.Entity.ConquerPoints += Database.rates.SkillTeam;
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "your team is the winner of this round you obtained " + Database.rates.SkillTeam + " as a gift come again tomorrow!");
                                        npc.OptionID = 255;
                                        client.Send(npc.ToArray());
                                    }
                                }
                            }
                            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("SkillTeam PkWar Last Round Ended and RedTeam Wins! Come again Tomorrow", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                            Game.KimoSkillWar.Round = 0;
                            Game.KimoSkillWar.RKills = 0;
                            Game.KimoSkillWar.YKills = 0;
                            //Game.KimoSkillWar.SignUP = true;
                        }
                        else
                        {
                            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                            {
                                if (client.Entity.MapID == Game.KimoSkillWar.MapID)
                                {
                                    if (client.RedOn == true)
                                    {
                                        client.Entity.Teleport(1002, 429, 378);
                                        Game.KimoSkillWar.RTeamNum = 0;
                                    }
                                    else
                                    {
                                        client.Entity.ConquerPoints += Database.rates.SkillTeam;
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "your team is the winner of this round you obtained " + Database.rates.SkillTeam + " as a gift come back tomorrow!");
                                        npc.OptionID = 255;
                                        client.Send(npc.ToArray());
                                    }
                                }
                            }
                            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("SkillTeam PkWar Last Round Ended and YellowTeam Wins! Come again Tomorrow", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                            Game.KimoSkillWar.Round = 0;
                            //Game.KimoSkillWar.SignUP = true;
                            Game.KimoSkillWar.RKills = 0;
                            Game.KimoSkillWar.YKills = 0;
                        }
                    }
                }
            }
            if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 09 && DateTime.Now.Second == 00)
            {
                if (Program.Carnaval5 == 1)
                {
                    //
                    Program.Carnaval5 = 0;
                }
            }
            if (Game.KimoSkillWar.Started == true)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 10 && DateTime.Now.Second == 00)
                {
                    if (Program.Carnaval5 == 0)
                    {
                        Program.Carnaval5 = 1;
                        if (Game.KimoSkillWar.RKills > Game.KimoSkillWar.YKills)
                        {

                            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                            {
                                if (client.Entity.MapID == Game.KimoSkillWar.MapID)
                                {
                                    if (client.YellowOn == true)
                                    {
                                        client.Entity.Teleport(1002, 429, 378);
                                        Game.KimoSkillWar.YTeamNum = 0;
                                    }
                                    else
                                    {
                                        client.Entity.ConquerPoints += Database.rates.SkillTeam;
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "your team is the winner of this round you obtained " + Database.rates.SkillTeam + " as a gift Next Round began!");
                                        npc.OptionID = 255;
                                        client.Send(npc.ToArray());
                                    }

                                }
                            }
                            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("SkillTeam PkWar Round [" + Game.KimoSkillWar.Round + "] Ended and RedTeam Wins! next round Started", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                            Game.KimoSkillWar.Round += 1;
                            Game.KimoSkillWar.RKills = 0;
                            Game.KimoSkillWar.YKills = 0;
                            Game.KimoSkillWar.SignUP = true;
                        }
                        else
                        {
                            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                            {
                                if (client.Entity.MapID == Game.KimoSkillWar.MapID)
                                {
                                    if (client.RedOn == true)
                                    {
                                        client.Entity.Teleport(1002, 429, 378);
                                        Game.KimoSkillWar.RTeamNum = 0;
                                    }
                                    else
                                    {
                                        client.Entity.ConquerPoints += Database.rates.SkillTeam;
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "your team is the winner of this round you obtained " + Database.rates.SkillTeam + " as a gift Next Round began!");
                                        npc.OptionID = 255;
                                        client.Send(npc.ToArray());
                                    }
                                }
                            }
                            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("SkillTeam PkWar Round [" + Game.KimoSkillWar.Round + "] Ended and YellowTeam Wins! next round Started", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                            Game.KimoSkillWar.Round += 1;
                            Game.KimoSkillWar.SignUP = true;
                            Game.KimoSkillWar.RKills = 0;
                            Game.KimoSkillWar.YKills = 0;
                        }
                    }
                }
            }
            /* if (GameIP != "25.8.155.79")
             {
                 CommandsAI("@exit");
             }*/
            if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 19 && DateTime.Now.Second == 00)
            {
                if (Program.Carnaval6 == 1)
                {
                    //
                    Program.Carnaval6 = 0;
                }
            }
            if (Game.KimoSkillWar.Started == true)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.SKHour && DateTime.Now.Minute == 20 && DateTime.Now.Second == 00)
                {
                    if (Program.Carnaval6 == 0)
                    {
                        Program.Carnaval6 = 1;
                        if (Game.KimoSkillWar.RKills > Game.KimoSkillWar.YKills)
                        {
                            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                            {
                                if (client.Entity.MapID == Game.KimoSkillWar.MapID)
                                {
                                    if (client.YellowOn == true)
                                    {
                                        client.Entity.Teleport(1002, 429, 378);
                                        Game.KimoSkillWar.YTeamNum = 0;
                                    }
                                    else
                                    {
                                        client.Entity.ConquerPoints += Database.rates.SkillTeam;
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "your team is the winner of this round you obtained " + Database.rates.SkillTeam + " as a gift Next Round began!");
                                        npc.OptionID = 255;
                                        client.Send(npc.ToArray());
                                    }
                                }
                            }
                            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("SkillTeam PkWar Round [" + Game.KimoSkillWar.Round + "] Ended and RedTeam Wins! next round Started", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                            Game.KimoSkillWar.Round += 1;
                            Game.KimoSkillWar.RKills = 0;
                            Game.KimoSkillWar.YKills = 0;
                            Game.KimoSkillWar.SignUP = true;
                        }
                        else
                        {
                            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                            {
                                if (client.Entity.MapID == Game.KimoSkillWar.MapID)
                                {
                                    if (client.RedOn == true)
                                    {
                                        client.Entity.Teleport(1002, 429, 378);
                                        Game.KimoSkillWar.RTeamNum = 0;
                                    }
                                    else
                                    {
                                        client.Entity.ConquerPoints += Database.rates.SkillTeam;
                                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "your team is the winner of this round you obtained " + Database.rates.SkillTeam + " as a gift Next Round began!");
                                        npc.OptionID = 255;
                                        client.Send(npc.ToArray());
                                    }
                                }
                            }
                            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("SkillTeam PkWar Round [" + Game.KimoSkillWar.Round + "] Ended and YellowTeam Wins! next round Started", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                            Game.KimoSkillWar.Round += 1;
                            Game.KimoSkillWar.SignUP = true;
                            Game.KimoSkillWar.RKills = 0;
                            Game.KimoSkillWar.YKills = 0;
                        }
                    }
                }
            }

            if (DateTime.Now.Hour == Game.KimoEvents.DWHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
            {
                Game.CpsWar.Start();
            }
            if (DateTime.Now.Hour == Game.KimoEvents.DWHour && DateTime.Now.Minute == 05 && DateTime.Now.Second == 00)
            {
                Game.CpsWar.CloseSignUp();
            }
            if (DateTime.Now.Hour == Game.KimoEvents.DWHour && DateTime.Now.Minute == 30 && DateTime.Now.Second == 00)
            {
                Game.CpsWar.End();
            }
            /* if (GameIP != "25.8.155.79")
             {
                 CommandsAI("@exit");
             }*/
            if (DateTime.Now.Minute == 12 && DateTime.Now.Second == 58)
            {
                if (Database.rates.Weather == 1)
                {
                    Program.WeatherType = 4;

                }
            }
            if (DateTime.Now.Minute == 27 && DateTime.Now.Second == 58)
            {
                if (Database.rates.Weather == 1)
                {
                    Program.WeatherType = 10;
                }
            }
            /*if (GameIP != "25.8.155.79")
            {
                CommandsAI("@exit");
            }*/
            if (DateTime.Now.Minute == 42 && DateTime.Now.Second == 58)
            {
                if (Database.rates.Weather == 1)
                {
                    Program.WeatherType = 5;

                }
            }
            if (DateTime.Now.Minute == 57 && DateTime.Now.Second == 58)
            {
                if (Database.rates.Weather == 1)
                {
                    Program.WeatherType = 0;

                }
            }
            if (DateTime.Now.Hour == Game.KimoEvents.EBHour && DateTime.Now.Minute == 04 && DateTime.Now.Second == 58)
            {
                PhoenixProject.Game.Tournaments.EliteTournament.ElitePkKimo();
            }

            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59 && DateTime.Now.Second == 59)
            {
                PhoenixProject.Game.ConquerStructures.Society.EliteGuildWar.Claim = 0;
            }
            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.EGEndHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                {
                    //Game.ConquerStructures.Society.GuildWar.Flame10th = false;
                    Game.ConquerStructures.Society.EliteGuildWar.End();
                }
            }
            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.EGHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                {
                    Game.ConquerStructures.Society.EliteGuildWar.Start();
                }
            }
            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                if (DateTime.Now.Hour >= Game.KimoEvents.EGHour && DateTime.Now.Hour < Game.KimoEvents.EGEndHour)
                {
                    if (Game.ConquerStructures.Society.EliteGuildWar.IsWar == false)
                    {
                        Game.ConquerStructures.Society.EliteGuildWar.Start();
                    }
                }
            }
            if (Game.ConquerStructures.Society.EliteGuildWar.IsWar)
            {
                if (Time32.Now > Game.ConquerStructures.Society.EliteGuildWar.ScoreSendStamp.AddSeconds(3))
                {
                    Game.ConquerStructures.Society.EliteGuildWar.ScoreSendStamp = Time32.Now;
                    Game.ConquerStructures.Society.EliteGuildWar.SendScores();
                }
                if (DateTime.Now.Hour == Game.KimoEvents.EGHour && DateTime.Now.Minute == 50 && DateTime.Now.Second == 00)
                {
                    if (DateTime.Now.Hour == Game.KimoEvents.EGHour && DateTime.Now.Minute == 50 && DateTime.Now.Second == 00)
                    {
                        //Game.ConquerStructures.Society.GuildWar.Flame10th = true;
                        ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("" + Database.Messagess.EliteGW2 + "", System.Drawing.Color.White, Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                    }
                }
            }
            if (Game.ClanWar.IsWar)
            {
                if (Time32.Now > Game.ClanWar.ScoreSendStamp.AddSeconds(3))
                {
                    Game.ClanWar.ScoreSendStamp = Time32.Now;
                    Game.ClanWar.SendScores();
                }
                if (DateTime.Now.Hour == Game.KimoEvents.ClanHour && DateTime.Now.Minute == 50 && DateTime.Now.Second == 00)
                {
                    if (DateTime.Now.Hour == Game.KimoEvents.ClanHour && DateTime.Now.Minute == 50 && DateTime.Now.Second == 00)
                    {
                        //Game.ConquerStructures.Society.GuildWar.Flame10th = true;
                        ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("10 Minutes left till ClanWar Ended Hurrry!", System.Drawing.Color.White, Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                    }
                }
            }
            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                if (DateTime.Now.Hour >= Game.KimoEvents.ClanHour && DateTime.Now.Hour < Game.KimoEvents.ClanEndHour)
                {
                    if (Game.ClanWar.IsWar == false)
                    {
                        Game.ClanWar.Start();
                    }
                }
            }
            if (DateTime.Now.Hour == Game.KimoEvents.ClanHour && DateTime.Now.Minute == 05 && DateTime.Now.Second == 00)
            {
                PhoenixProject.Game.ClanWar.Claim = 0;
            }
            /*if (GameIP != "25.8.155.79")
            {
                CommandsAI("@exit");
            }*/
            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.ClanEndHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                {
                    //Game.ConquerStructures.Society.GuildWar.Flame10th = false;
                    Game.ClanWar.End();
                }
            }
            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.ClanHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                {
                    Game.ClanWar.Start();
                    //ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("ClanWa!", System.Drawing.Color.White, Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                }
            }

            if (DateTime.Now.Minute == 44 && DateTime.Now.Second == 00)
            {
                if (DateTime.Now.Minute == 44 && DateTime.Now.Second == 00)
                {
                    Game.Tournaments.SteedRace.SteedRacee();
                    PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("" + Database.Messagess.SteedRace + "", System.Drawing.Color.White, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                }
            }
            if (DateTime.Now.Hour == Game.KimoEvents.SpouseHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.SpouseHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                {
                    //Game.Flags.RemoveSpouse();
                    PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("" + Database.Messagess.CouplesPk + "", System.Drawing.Color.White, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                }
            }
            if (DateTime.Now.Second == 00 && DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == Game.KimoEvents.WHour && DateTime.Now.Minute == 00)
            {
                //Game.Flags.RemoveWeekly();
                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Weekly Pk Tourment has started go to TwinCity to Signup fast before it come " + Game.KimoEvents.WHour + ":05", System.Drawing.Color.White, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
            }
            if (DateTime.Now.Minute == 59 && DateTime.Now.Second == 00)
            {
                if (DateTime.Now.Minute == 59 && DateTime.Now.Second == 00)
                {
                    Game.Tournaments.SteedRace.FinishRace();
                    //Kernel.SendWorldMessage(new Message("SteedRace has started would you like to join it now ? go to TC.", System.Drawing.Color.White, Message.Center), Kernel.GamePool.Values);
                }
            }



            if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 05 && DateTime.Now.Second == 00)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.DisHour && DateTime.Now.Minute == 05 && DateTime.Now.Second == 00)
                {
                    PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("" + Database.Messagess.DisEnd + "", System.Drawing.Color.White, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                    PhoenixProject.Game.Features.DisCity.dis = false;
                }
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.GWSHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                {
                    Game.ConquerStructures.Society.GuildWar.Start();
                }
            }
            if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                if (DateTime.Now.Hour == Game.KimoEvents.GWEEndHour && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                {
                    Game.ConquerStructures.Society.GuildWar.Flame10th = false;
                    Game.ConquerStructures.Society.GuildWar.End();
                }
            }
            if (Game.ConquerStructures.Society.GuildWar.IsWar)
            {
                if (Time32.Now > Game.ConquerStructures.Society.GuildWar.ScoreSendStamp.AddSeconds(3))
                {
                    Game.ConquerStructures.Society.GuildWar.ScoreSendStamp = Time32.Now;
                    Game.ConquerStructures.Society.GuildWar.SendScores();
                }
                if (!Game.ConquerStructures.Society.GuildWar.Flame10th)
                {
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday && DateTime.Now.Hour == (Game.KimoEvents.GWEEndHour - 1) && DateTime.Now.Minute == 00 && DateTime.Now.Second == 00)
                    {
                        Game.ConquerStructures.Society.GuildWar.Flame10th = true;
                        ServerBase.Kernel.SendWorldMessage(new Network.GamePackets.Message("" + Database.Messagess.GuildFlame + "", System.Drawing.Color.White, Network.GamePackets.Message.Center), ServerBase.Kernel.GamePool.Values);
                    }
                }
            }
            if (DateTime.Now.Hour >= Game.KimoEvents.GWSHour && DateTime.Now.Hour < Game.KimoEvents.GWSEndHour && DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.Hour >= Game.KimoEvents.GWEHour && DateTime.Now.Hour < Game.KimoEvents.GWEEndHour && DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                if (Game.ConquerStructures.Society.GuildWar.IsWar == false)
                {
                    Game.ConquerStructures.Society.GuildWar.Start();
                }
            }

        }
        }
        #endregion

        public static void CommandsAI(string command)
        {
            if (command == null)
                return;
            string[] data = command.Split(' ');
            switch (data[0])
            {
                case "@alivetime":
                    {
                        DateTime now = DateTime.Now;
                        TimeSpan t2 = new TimeSpan(StartDate.ToBinary());
                        TimeSpan t1 = new TimeSpan(now.ToBinary());
                        Console.WriteLine("The server has been online " + (int)(t1.TotalHours - t2.TotalHours) + " hours, " + (int)((t1.TotalMinutes - t2.TotalMinutes) % 60) + " minutes.");
                        break;
                    }
                case "@kimo":
                    {
                        AuthServer.Disable();
                        AuthServer = null;
                        AuthServer = new AsyncSocket(AuthPort);
                        AuthServer.OnClientConnect += new Action<Interfaces.ISocketWrapper>(AuthServer_AnnounceNewConnection);
                        AuthServer.OnClientReceive += new Action<byte[], Interfaces.ISocketWrapper>(AuthServer_AnnounceReceive);
                        AuthServer.OnClientDisconnect += new Action<Interfaces.ISocketWrapper>(AuthServer_AnnounceDisconnection);
                        GameServer.Disable();
                        GameServer = null;
                        GameServer = new AsyncSocket(GamePort);
                        GameServer.OnClientConnect += new Action<Interfaces.ISocketWrapper>(GameServer_AnnounceNewConnection);
                        GameServer.OnClientReceive += new Action<byte[], Interfaces.ISocketWrapper>(GameServer_AnnounceReceive);
                        GameServer.OnClientDisconnect += new Action<Interfaces.ISocketWrapper>(GameServer_AnnounceDisconnection);
                        Console.WriteLine("Server Manually rebooted");
                        break;
                    }
                case "@d":
                    {
                        AuthServer.Disable();
                        GameServer.Disable();
                        Console.WriteLine("Server Offline Now");
                        break;
                    }
                case "@e":
                    {
                        AuthServer.Disable();
                        AuthServer = null;
                        AuthServer = new AsyncSocket(AuthPort);
                        AuthServer.OnClientConnect += new Action<Interfaces.ISocketWrapper>(AuthServer_AnnounceNewConnection);
                        AuthServer.OnClientReceive += new Action<byte[], Interfaces.ISocketWrapper>(AuthServer_AnnounceReceive);
                        AuthServer.OnClientDisconnect += new Action<Interfaces.ISocketWrapper>(AuthServer_AnnounceDisconnection);
                        GameServer.Disable();
                        GameServer = null;
                        GameServer = new AsyncSocket(GamePort);
                        GameServer.OnClientConnect += new Action<Interfaces.ISocketWrapper>(GameServer_AnnounceNewConnection);
                        GameServer.OnClientReceive += new Action<byte[], Interfaces.ISocketWrapper>(GameServer_AnnounceReceive);
                        GameServer.OnClientDisconnect += new Action<Interfaces.ISocketWrapper>(GameServer_AnnounceDisconnection);
                        Console.WriteLine("Server Online Now");
                        break;
                    }
                case "@online":
                    {
                        Console.WriteLine("Online players count: " + ServerBase.Kernel.GamePool.Count);
                        string line = "";
                        foreach (Client.GameState pClient in ServerBase.Kernel.GamePool.Values)
                            line += pClient.Entity.Name + ",";
                        if (line != "")
                        {
                            line = line.Remove(line.Length - 1);
                            Console.WriteLine("Players: " + line);
                        }
                        break;
                    }
                case "@memoryusage":
                    {
                        var proc = System.Diagnostics.Process.GetCurrentProcess();
                        Console.WriteLine("Thread count: " + proc.Threads.Count);
                        Console.WriteLine("Memory set(MB): " + ((double)((double)proc.WorkingSet64 / 1024)) / 1024);
                        proc.Close();
                        break;
                    }
                case "@save":
                    {
                        using (var conn = Database.DataHolder.MySqlConnection)
                        {
                            conn.Open();
                            foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
                            {
                                if (client != null)
                                {
                                    Database.EntityTable.SaveEntity(client);
                                    Database.SkillTable.SaveProficiencies(client);
                                    Database.SkillTable.SaveSpells(client);
                                    Database.ArenaTable.SaveArenaStatistics(client.ArenaStatistic);
                                }
                            }
                        }
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("configuration").Set("ItemUID", Network.GamePackets.ConquerItem.ItemUID.Now).Where("Server", ServerBase.Constants.ServerName).Execute();
                    }
                    break;
                case "@playercap":
                    {
                        try
                        {
                            PlayerCap = int.Parse(data[1]);
                        }
                        catch
                        {

                        }
                        break;
                    }
                case "@exit":
                    {
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastEntity", Program.nextEntityID).Where("Coder", "kimo").Execute();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("KoCount", rates.KoCount).Execute();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastItem", PhoenixProject.Client.AuthState.nextID).Execute();
                        //new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("configuration").Set("ItemUID", Network.GamePackets.ConquerItem.ItemUID.Now).Where("Server", ServerBase.Constants.ServerName).Execute();
                        ServerRrestart = true;
                       // GameServer.Disable();
                       // AuthServer.Disable();

                        var WC = ServerBase.Kernel.GamePool.Values.ToArray();
                        foreach (Client.GameState client in WC)
                            client.Disconnect();
                        
                        if (GuildWar.IsWar)
                            GuildWar.End();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("KoCount", rates.KoCount).Execute();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastItem", PhoenixProject.Client.AuthState.nextID).Execute();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastEntity", Program.nextEntityID).Where("Coder", "kimo").Execute();
                       // new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastIUID", PhoenixProject.Client.AuthState.nextID).Execute();

                        Environment.Exit(0);
                    }
                    break;
                case "@restart":
                    {
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastEntity", Program.nextEntityID).Where("Coder", "kimo").Execute();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("KoCount", rates.KoCount).Execute();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastItem", PhoenixProject.Client.AuthState.nextID).Execute();
                        //new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("configuration").Set("ItemUID", Network.GamePackets.ConquerItem.ItemUID.Now).Where("Server", ServerBase.Constants.ServerName).Execute();
                        ServerRrestart = true;
                       // GameServer.Disable();
                        //AuthServer.Disable();

                        var WC = ServerBase.Kernel.GamePool.Values.ToArray();
                        foreach (Client.GameState client in WC)
                            client.Disconnect();

                        if (GuildWar.IsWar)
                            GuildWar.End();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("KoCount", rates.KoCount).Where("Coder", "kimo").Execute();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastItem", PhoenixProject.Client.AuthState.nextID).Where("Coder", "kimo").Execute();
                        new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("rates").Set("LastEntity", Program.nextEntityID).Where("Coder", "kimo").Execute();
                        //new Database.MySqlCommand(Database.MySqlCommandType.UPDATE).Update("configuration").Set("ItemUID", Network.GamePackets.ConquerItem.ItemUID.Now).Where("Server", ServerBase.Constants.ServerName).Execute();

                        Application.Restart();
                        Environment.Exit(0);
                    }
                    break;
                case "@account":
                    {
                        Database.AccountTable account = new AccountTable(data[1]);
                        account.Password = data[2];
                        account.State = AccountTable.AccountState.Player;
                        account.Save();
                    }
                    break;
            }
        }

        static void GameServer_AnnounceNewConnection(Interfaces.ISocketWrapper obj)
        {
            obj.Connector = new Client.GameState(obj.Socket);
            Client.GameState Client = obj.Connector as Client.GameState;
            Client.Send(Client.DHKeyExchance.CreateServerKeyPacket());
        }
        public unsafe static void GameServer_AnnounceReceive(byte[] arg1, Interfaces.ISocketWrapper arg2)
        {
            Client.GameState Client = arg2.Connector as Client.GameState;
            Client.Cryptography.Decrypt(arg1);


            if (Client != null)
            {
                if (Client.Exchange == true)
                {
                    Client.Exchange = false;

                    Client.Action = 1;
                    ushort position = 7;
                    uint PacketLen = BitConverter.ToUInt32(arg1, position); position += 4;
                    int JunkLen = BitConverter.ToInt32(arg1, position); position += 4; position += (ushort)JunkLen;
                    int Len = BitConverter.ToInt32(arg1, position); position += 4;
                    byte[] pubKey = new byte[Len];
                    for (int x = 0; x < Len; x++)
                        pubKey[x] = arg1[x + position];
                    string PubKey = System.Text.ASCIIEncoding.Default.GetString(pubKey);
                    Client.Cryptography = Client.DHKeyExchance.HandleClientKeyPacket(PubKey, Client.Cryptography);
                }
                else
                {
                    if (!Client.Exchange && Client.Action != 0)
                    {
                        Network.PacketHandler.HandleBuffer(arg1, Client);
                    }
                    else
                    {
                        Console.WriteLine("Client Null");
                    }
                }
            }
            else
            {
                Console.WriteLine("Client Null");
            }
        }

        static void GameServer_AnnounceDisconnection(Interfaces.ISocketWrapper obj)
        {
            if (obj.Connector != null)
            {
                Client.GameState Client = obj.Connector as Client.GameState;
                if (Client.Account != null)
                    if (ServerBase.Kernel.GamePool.ContainsKey(Client.Account.EntityID))
                        Client.Disconnect();
            }
        }
        static void AuthServer_AnnounceNewConnection(Interfaces.ISocketWrapper obj)
        {
            Client.AuthState authState = new Client.AuthState(obj.Socket);
            authState.Cryptographer = new Network.Cryptography.AuthCryptography();
            Network.AuthPackets.PasswordCryptographySeed pcs = new PasswordCryptographySeed();
            pcs.Seed = ServerBase.Kernel.Random.Next();
            authState.PasswordSeed = pcs.Seed;
            authState.Send(pcs);
            obj.Connector = authState;
        }
        static void AuthServer_AnnounceReceive(byte[] arg1, Interfaces.ISocketWrapper arg2)
        {
            //Console.WriteLine(" this");
            bool InvalidQuestion = false;
            if (arg1.Length == 240)
            {
                Client.AuthState player = arg2.Connector as Client.AuthState;
                player.Cryptographer.Decrypt(arg1);
                player.Info = new Authentication();
                player.Info.Deserialize(arg1);
                player.Account = new AccountTable(player.Info.Username);
                string password = player.Info.Password;
                Forward Fw = new Forward();
                
                if (password == player.Account.Password)
                {
                   
                    if (player.Account.State == AccountTable.AccountState.Banned)
                    {
                        Fw.Type = (Forward.ForwardType)12;
                        goto kimoz;
                    }
                    else if (player.Account.State == AccountTable.AccountState.Cheat)
                    {
                        Fw.Type = (Forward.ForwardType)22;
                        goto kimoz;
                    }
                    else if (player.Account.State == AccountTable.AccountState.Aimbot)
                    {
                        Fw.Type = (Forward.ForwardType)12;
                        goto kimoz;
                    }
                    else if (player.Account.State == AccountTable.AccountState.Spam)
                    {
                        Fw.Type = (Forward.ForwardType)22;
                        goto kimoz;
                    }

                    else if (player.Account.State == AccountTable.AccountState.NotActivated)
                    {
                        Fw.Type = (Forward.ForwardType)30;
                        goto kimoz;
                    }
                    else if (player.Account.State == AccountTable.AccountState.BadWords)
                    {
                        Fw.Type = (Forward.ForwardType)22;
                        goto kimoz;
                    }

                    else if (player.Account.exists == false)
                    {
                        InvalidQuestion = true;
                        goto kimoz;
                    }
                    else if (ServerRrestart == true)
                    {
                        Fw.Type = (Forward.ForwardType)71;
                        goto kimoz;
                    }
                    else if (ServerBase.Kernel.GamePool.Count >= 400)
                    {
                        Fw.Type = (Forward.ForwardType)21;
                        goto kimoz;
                    }
                    else
                        Fw.Type = Forward.ForwardType.Ready;
                }
                else
                {
                    Fw.Type = Forward.ForwardType.InvalidInfo;
                }
               
            kimoz:
                if (InvalidQuestion == true)
                {
                    Fw.Type = Forward.ForwardType.InvalidInfo;
                }
                else if (Fw.Type == Forward.ForwardType.Ready)
                {
                    //Console.WriteLine(" here");
                    Fw.Identifier = Network.AuthPackets.Forward.Incrementer.Next;
                    ServerBase.Kernel.AwaitingPool.Add(Fw.Identifier, player.Account);
                }
                Fw.IP = GameIP;
                Fw.Port = GamePort;
                player.Send(Fw);
                //if (Fw.Type != Forward.ForwardType.Ready)
                //{
                //    arg2.Socket.Disconnect(false);
                //}
               // Console.WriteLine(" here5");
            }
            else
            {
                //Console.WriteLine(" here");
                arg2.Socket.Disconnect(false);
            }
        }
        static void AuthServer_AnnounceDisconnection(Interfaces.ISocketWrapper obj)
        {

        }
        public static bool CodificarServer { get; set; }

        public static bool Mantenimiento { get; set; }

        public static bool ServerRrestart = false;
    }
        
}
