using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
namespace PhoenixProject.Game.ConquerStructures
{
    public class KillTheTerrorist
    {
        public static bool IsOn = false;
        public static bool Terrorist = false;
        public static ushort Map = 1801;
        public static void SendTimer()
        {
            Console.WriteLine("KTT Timer started!");
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
                send = false;
                end = false;
                teleend = false;
                signup = true;
                IsOn = true;
                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("KillTheTerrorist have started. Sign Up in TwinCity! You have one minute", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
            }
        }
        public static void Send()
        {
            if (DateTime.Now.Minute == 31 && send == false)
            {
                send = true;
                Terrorist = false;
                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Kill The Terrorist! <!His Flashy!> ", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                {
                    if (client.Entity.Tournament_Signed == true && client.Entity.KillTheTerrorist_IsTerrorist == false)
                    {
                        client.Entity.SpawnProtection = true;
                        client.Entity.Teleport(1801, 55, 55);
                    }
                    if (client.Entity.Tournament_Signed == true && client.Entity.KillTheTerrorist_IsTerrorist == true)
                    {
                        client.Entity.Teleport(1801, 55, 50);
                        client.Entity.AddFlag(Update.Flags.Flashy);
                    }
                }
            }
        }
        public static void End()
        {
            if (DateTime.Now.Minute == 40 && end == false)
            {
                signup = false;
                end = true;
                Terrorist = false;
                IsOn = false;
                foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                {
                    if (client.Entity.KillTheTerrorist_IsTerrorist == true)
                    {
                        client.Entity.ConquerPoints += 150;
                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message(":" + client.Entity.Name + " was the terrorist in the end and have won 150 cps ", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                    }
                    if (client.Entity.MapID == 1801)
                    {
                        client.Entity.Teleport(1002, 400, 400);
                    }
                    client.Entity.RemoveFlag(Update.Flags.Flashy);
                    client.Entity.KillTheTerrorist_IsTerrorist = false;
                    client.Entity.Tournament_Signed = false;
                }
            }
        }
        public static void TeleEnd()
        {
            if (DateTime.Now.Minute == 36)
            {
                Terrorist = false;
                IsOn = false;
                foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                {
                    if (client.Entity.MapID == 1801)
                    {
                        client.Entity.Teleport(1002, 400, 400);
                    }
                    if (client.Entity.KillTheTerrorist_IsTerrorist == true)
                    {
                        client.Entity.ConquerPoints += 100;
                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message(":" + client.Entity.Name + " was the terrorist in the end and have won 100 cps", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                    }
                    client.Entity.RemoveFlag(Update.Flags.Flashy);
                    client.Entity.KillTheTerrorist_IsTerrorist = false;
                    client.Entity.Tournament_Signed = false;
                }
            }
        }
    }
}
