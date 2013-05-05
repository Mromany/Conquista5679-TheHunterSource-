using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
namespace PhoenixProject.Game.ConquerStructures
{
    public class ClassPk
    {
        public static bool ClassPks = false;
        public static ushort Map = 7001;
        public static bool signup = false;
        public static int howmanyinmap = 0;
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
        public static void CheackAlive(ulong mapid)
        {
            howmanyinmap = 0;
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                if (client.Entity.MapID == mapid && client.Entity.Hitpoints >= 1)
                {
                    howmanyinmap += 1;
                    PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Players Alive in ClassPk Now: " + howmanyinmap + " ", System.Drawing.Color.Black, PhoenixProject.Network.GamePackets.Message.FirstRightCorner), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                }

            }
        }
        public static void SignUp()
        {
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                if (DateTime.Now.Minute == 00 && signup == false && client.Entity.Class >= 10 && client.Entity.Class <= 15)
                {
                    signup = true;
                    ClassPks = true;
                    //client.Entity.Status = 0;
                    client.Entity.RemoveFlag(PhoenixProject.Network.GamePackets.Update.Flags.TopTrojan);
                }
        }
        public static void SignUp1()
        {
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                if (DateTime.Now.Minute == 00 && signup == false && client.Entity.Class >= 20 && client.Entity.Class <= 25)
                {
                    signup = true;
                    ClassPks = true;
                    //client.Entity.Status = 0;
                    client.Entity.RemoveFlag(PhoenixProject.Network.GamePackets.Update.Flags.TopTrojan);
                }
        }
        public static void SignUp2()
        {
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                if (DateTime.Now.Minute == 00 && signup == false && client.Entity.Class >= 40 && client.Entity.Class <= 45)
                {
                    signup = true;
                    ClassPks = true;
                    //client.Entity.Status = 0;
                    client.Entity.RemoveFlag(PhoenixProject.Network.GamePackets.Update.Flags.TopTrojan);
                }
        }
        public static void SignUp3()
        {
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                if (DateTime.Now.Minute == 00 && signup == false && client.Entity.Class >= 50 && client.Entity.Class <= 55)
                {
                    signup = true;
                    ClassPks = true;
                   // client.Entity.Status = 0;
                    client.Entity.RemoveFlag(PhoenixProject.Network.GamePackets.Update.Flags.TopTrojan);
                }
        }
        public static void SignUp4()
        {
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                if (DateTime.Now.Minute == 00 && signup == false && client.Entity.Class >= 60 && client.Entity.Class <= 65)
                {
                    signup = true;
                    ClassPks = true;
                    //client.Entity.Status = 0;
                    client.Entity.RemoveFlag(PhoenixProject.Network.GamePackets.Update.Flags.TopTrojan);
                }
        }
        public static void SignUp5()
        {
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                if (DateTime.Now.Minute == 00 && signup == false && client.Entity.Class >= 130 && client.Entity.Class <= 135)
                {
                    signup = true;
                    ClassPks = true;
                    //client.Entity.Status = 0;
                    client.Entity.RemoveFlag(PhoenixProject.Network.GamePackets.Update.Flags.TopTrojan);
                }
        }
        public static void SignUp6()
        {
            foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                if (DateTime.Now.Minute == 00 && signup == false && client.Entity.Class >= 140 && client.Entity.Class <= 145)
                {
                    signup = true;
                    ClassPks = true;
                    //client.Entity.Status = 0;
                    client.Entity.RemoveFlag(PhoenixProject.Network.GamePackets.Update.Flags.TopTrojan);
                }
        }
        public static void End()
        {
            if (DateTime.Now.Minute == 59)
            {
                signup = false;
                ClassPks = false;
                PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("[" + Database.rates.servername + "]: ClassPk Has Ended Come Next Day ", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                 
                foreach (Client.GameState client in PhoenixProject.ServerBase.Kernel.GamePool.Values)
                {
                   
                    if (client.Entity.MapID == 7001)
                    {
                        client.Entity.Teleport(1002, 400, 400);
                        client.Entity.RemoveFlag(Update.Flags.Flashy);

                    }
                   
                }
            }
        }
    }
}
