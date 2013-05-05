using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
namespace PhoenixProject.Game.ConquerStructures
{
    public class DM
    {
        public static bool IsOn = false;
        public static bool CouplesWar = false;
        public static void SendTimer()
        {
            Console.WriteLine("TDM Timer started!");
            System.Timers.Timer TimerA = new System.Timers.Timer(1000.0);
            TimerA.Start();
            TimerA.Elapsed += delegate { SignUp(); };

            System.Timers.Timer TimerB = new System.Timers.Timer(1000.0);
            TimerB.Start();
            TimerB.Elapsed += delegate { Send(); };

            System.Timers.Timer TimerC = new System.Timers.Timer(1000.0);
            TimerC.Start();
            TimerC.Elapsed += delegate { End(); };

            System.Timers.Timer TimerD = new System.Timers.Timer(1000.0);
            TimerD.Start();
            TimerD.Elapsed += delegate { TeleEnd(); };
        }
        public static bool signup = false;
        public static bool send = false;
        public static bool end = false;
        public static bool teleend = false;

        public static void SignUp()
        {
            if (DateTime.Now.Minute == 30 && signup == false)
            {
                TeamDeathMatchScore.BlackTeamScore = 0;
                TeamDeathMatchScore.BlackCaptain = false;
                TeamDeathMatchScore.RedTeamScore = 0;
                TeamDeathMatchScore.RedCaptain = false;
                TeamDeathMatchScore.BlueTeamScore = 0;
                TeamDeathMatchScore.BlueCaptain = false;
                TeamDeathMatchScore.WhiteTeamScore = 0;
                TeamDeathMatchScore.WhiteCaptain = false;
                send = false;
                end = false;
                teleend = false;
                signup = true;
                IsOn = true;
                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("TeamDeathMatch have started. Sign Up in TwinCity! You have one minute", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
            }
        }
        public static void Send()
        {
            if (DateTime.Now.Minute == 31 && send == false)
            {
                signup = false;
                send = true;
                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Kill!!", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                SendTeam();
            }
        }
        public static void SendTeam()
        {
            foreach (Client.GameState C in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                if (C.Entity.Tournament_Signed == true)
                {
                    C.Entity.SpawnProtection = true;
                    C.Entity.TeamDeathMatch_Kills = 0;

                    if (C.Entity.TeamDeathMatch_BlackTeam == true)
                    {
                        C.Entity.Teleport(8883, 125, 124);
                    }
                    if (C.Entity.TeamDeathMatch_BlueTeam == true)
                    {
                        C.Entity.Teleport(8883, 101, 196);
                    }
                    if (C.Entity.TeamDeathMatch_WhiteTeam == true)
                    {
                        C.Entity.Teleport(8883, 68, 139);
                    }
                    if (C.Entity.TeamDeathMatch_RedTeam == true)
                    {
                        C.Entity.Teleport(8883, 149, 52);
                    }
                }
            }
        }
        public static void End()
        {
            if (DateTime.Now.Minute == 50 && end == false)
            {
                signup = false;
                end = true;
                IsOn = false;
                foreach (Client.GameState C in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                {
                    if (C.Entity.MapID == 8883)
                    {
                        C.Entity.Teleport(1002, 400, 400);
                    }
                    C.Entity.RemoveFlag(Update.Flags.Flashy);
                    C.Entity.Tournament_Signed = false;
                }
                TeamDeathMatchScore.Reward();
            }
        }
        public static void TeleEnd()
        {
            if (DateTime.Now.Minute == 51 && teleend == false)
            {
                signup = false;
                teleend = true;
                IsOn = false;
                foreach (Client.GameState C in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                {
                    if (C.Entity.MapID == 8883)
                    {
                        C.Entity.Teleport(1002, 400, 400);
                    }
                    C.Entity.RemoveFlag(Update.Flags.Flashy);
                    C.Entity.Tournament_Signed = false;
                }
                TeamDeathMatchScore.Reward();
            }
        }
    }
}
