using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game
{
    class CpsWar//Coded By Kimo
    {
        public static int TopCps = 0;
        public static int MinCps = 0;
        public static int LowCps = 0;
        public static bool IsWar = false;
        public static bool IsWar2 = false;
        public static bool IsWar3 = false;
        public static ushort TopMap = 7005;
        public static ushort MinMap = 7006;
        public static ushort LowMap = 7008;
        public static int howmanyinmap = 0;
        public static int howmanyinmap2 = 0;
        public static int howmanyinmap3 = 0;
        public static DateTime StartTime;
        public static void CheackLowAlive()
        {
            howmanyinmap3 = 0;
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                if (client.Entity.MapID == LowMap && client.Entity.Hitpoints >= 1)
                {
                    howmanyinmap3 += 1;
                    //PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Players Alive in ClassPk Now: " + howmanyinmap + " ", System.Drawing.Color.Black, PhoenixProject.Network.GamePackets.Message.FirstRightCorner), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                }

            }
        }
        public static void CheackMinAlive()
        {
            howmanyinmap2 = 0;
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                if (client.Entity.MapID == MinMap && client.Entity.Hitpoints >= 1)
                {
                    howmanyinmap2 += 1;
                    //PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Players Alive in ClassPk Now: " + howmanyinmap + " ", System.Drawing.Color.Black, PhoenixProject.Network.GamePackets.Message.FirstRightCorner), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                }

            }
        }
        public static void CheackTopAlive()
        {
            howmanyinmap = 0;
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                if (client.Entity.MapID == TopMap && client.Entity.Hitpoints >= 1)
                {
                    howmanyinmap += 1;
                    //PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Players Alive in ClassPk Now: " + howmanyinmap + " ", System.Drawing.Color.Black, PhoenixProject.Network.GamePackets.Message.FirstRightCorner), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                }

            }
        }
        public static void Start()
        {
            StartTime = DateTime.Now;
            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Donation Pk War has Started you need to SignUp now and must Pay Some cps find PumpkinPrince To SignUp!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
            IsWar = true;
        }
        public static void End()
        {
            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Donation Pk War has ended have fun in the next Time!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
            //IsWar = false;
            //IsWar2 = false;
            //IsWar3 = false;
        }
        public static void CloseSignUp()
        {
            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("You cant signup to Donation Pk War  Come again next Time!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
            IsWar = false;
            IsWar2 = false;
            IsWar3 = false;
        }
        public static void ClaimTopCps(Client.GameState client)
        {
            client.Entity.ConquerPoints += (uint)TopCps;
            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Congratulations, "+client.Entity.Name+" has won in TopCps Arena and claimed "+TopCps+" ConquerPoints!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
            TopCps = 0;
            IsWar = false;
        }
        public static void ClaimMinCps(Client.GameState client)
        {
            client.Entity.ConquerPoints += (uint)MinCps;
            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Congratulations, " + client.Entity.Name + " has won in MinCps Arena and claimed " + MinCps + " ConquerPoints!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
            MinCps = 0;
            IsWar2 = false;
        }
        public static void ClaimLowCps(Client.GameState client)
        {
            client.Entity.ConquerPoints += (uint)LowCps;
            ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Congratulations, " + client.Entity.Name + " has won in LowCps Arena and claimed " + LowCps + " ConquerPoints!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
            LowCps = 0;
            IsWar3 = false;
        }
        public static void SignUpTop(Client.GameState client)
        {
            client.Entity.ConquerPoints -= 3000;
            TopCps += 3000;
        }
        public static void SignUpMin(Client.GameState client)
        {
            client.Entity.ConquerPoints -= 2000;
            MinCps += 2000;
        }
        public static void SignUpLow(Client.GameState client)
        {
            client.Entity.ConquerPoints -= 1000;
            LowCps += 1000;
        }
    }
}
