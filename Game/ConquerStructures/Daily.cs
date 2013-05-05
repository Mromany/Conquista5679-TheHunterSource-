using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
namespace PhoenixProject.Game.ConquerStructures
{
    public class Daily
    {
        public static bool DailyPks = false;
        public static ushort Map = 8877;
        public static bool signup = false;
        public static int howmanyinmap = 0;
        public static int howmanyinmap2 = 0;
        public static int howmanyinmap3 = 0;
        public static int TopDlClaim = 0;
        public static int TopGlClaim = 0;
        public static void AddDl()
        {
            TopDlClaim++;
            //return;
        }
        public static void AddGl()
        {
            TopGlClaim++;
            //return;
        }
        public static void CheackAlive()
        {
            howmanyinmap = 0;
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                if (client.Entity.MapID == 8877 && client.Entity.Hitpoints >= 1)
                {
                    howmanyinmap += 1;
                    PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Players Alive in DailyPk Now: " + howmanyinmap + " ", System.Drawing.Color.Black, PhoenixProject.Network.GamePackets.Message.FirstRightCorner), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                }

            }
        }
        public static void CheackAlive2()
        {
            howmanyinmap2 = 0;
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                if (client.Entity.MapID == 3333 && client.Entity.Hitpoints >= 1)
                {
                    howmanyinmap2 += 1;
                    PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Players Alive in LastManStanding: " + howmanyinmap2 + " ", System.Drawing.Color.Black, PhoenixProject.Network.GamePackets.Message.FirstRightCorner), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                }

            }
        }
        public static void CheackSpouse()
        {
            howmanyinmap3 = 0;
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                if (client.Entity.MapID == 1090 && client.Entity.Hitpoints >= 1)
                {
                    if (client.Entity.MapID == 1090 && client.Entity.Hitpoints >= 1)
                    {
                        howmanyinmap3 += 1;
                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Teams Alive in CouplesPk: " + howmanyinmap3 + " ", System.Drawing.Color.Black, PhoenixProject.Network.GamePackets.Message.FirstRightCorner), PhoenixProject.ServerBase.Kernel.GamePool.Values);

                    }
                }
            }
        }
        public static void SignUp()
        {
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                if (DateTime.Now.Minute == 00 && signup == false && client.Entity.Class >= 10 && client.Entity.Class <= 15)
                {
                    signup = true;
                    DailyPks = true;
                    client.Entity.Status = 0;
                    client.Entity.RemoveFlag(PhoenixProject.Network.GamePackets.Update.Flags.TopTrojan);
                }
        }


        public static void End()
        {
            if (DateTime.Now.Minute == 30)
            {
                //signup = false;
                //DailyPks = false;
                foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                {
                    if (DateTime.Now.Minute == 30)
                    {
                        client.Entity.ConquerPoints += 150;
                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("[PhoenixCo]: Daily Has Ended Come Next Hour ", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.TopLeft), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                    }
                    if (client.Entity.MapID == 8877)
                    {
                        client.Entity.Teleport(1002, 400, 400);
                    }
                    client.Entity.RemoveFlag(Update.Flags.Flashy);
                }
            }
        }
    }
}
