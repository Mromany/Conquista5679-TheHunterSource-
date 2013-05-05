using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Game.Tournaments
{
    public class SteedRace
    {
        public static DateTime TimerStart = DateTime.Now;
        public static ushort No = 0;
        public static ushort Reward = 0;
        public static bool IsRace = false;


        public static void SteedRacee()
        {
            IsRace = true;
            TimerStart = DateTime.Now;
            //TimerStart.Minute = DateTime.Now.Minute;
            //TimerStart.Millisecond = DateTime.Now.Millisecond;
            //TimerStart.Second = DateTime.Now.Second;
            No = 0;
            Reward = (ushort)Database.rates.SteedRace;
        }
        public static void FinishRace()
        {
            if (DateTime.Now.Minute == 59)
            {
                IsRace = false;
                Client.GameState[] players = null;
                players = PhoenixProject.ServerBase.Kernel.GamePool.Values;
                foreach (Client.GameState clients in players)
                {
                    if (clients.Entity != null)
                    {
                        if (clients.Entity != null)
                        {
                            if (clients.Entity.MapID == 1950)
                            {
                                clients.Entity.InSteedRace = false;
                                clients.Entity.Teleport(1002, 403, 396);
                            }
                        }
                        //clients.Send(new Network.GamePackets.Message("Steed Race has Finish", System.Drawing.Color.Red, Network.GamePackets.Message.BroadcastMessage));
                        clients.Send(new Network.GamePackets.Message("Steed Race has Finished", System.Drawing.Color.White, Network.GamePackets.Message.Center));
                    }
                }
            }
        }
        public static void GiveReward(Client.GameState client)
        {
            if (IsRace)
            {
                if (No < 10)
                {
                   
                    client.Entity.ConquerPoints += Reward;
                    Reward -= 400;
                    No++;
                    if (No == 1)
                    {
                        PhoenixProject.Network.GamePackets._String Packet = new PhoenixProject.Network.GamePackets._String(true);
                        Packet.UID = client.Entity.UID;
                        Packet.Type = PhoenixProject.Network.GamePackets._String.Effect;
                        Packet.TextsCount = 1;
                        Packet.Texts.Add("ridmatch_first");
                        client.SendScreen(Packet, true);
                        client.Entity.RacePoints += 20;
                    }
                    if (No == 2)
                    {
                        PhoenixProject.Network.GamePackets._String Packet = new PhoenixProject.Network.GamePackets._String(true);
                        Packet.UID = client.Entity.UID;
                        Packet.Type = PhoenixProject.Network.GamePackets._String.Effect;
                        Packet.TextsCount = 1;
                        Packet.Texts.Add("ridmatch_second");
                        client.SendScreen(Packet, true);
                        client.Entity.RacePoints += 15;
                    }
                    if (No == 3)
                    {
                        PhoenixProject.Network.GamePackets._String Packet = new PhoenixProject.Network.GamePackets._String(true);
                        Packet.UID = client.Entity.UID;
                        Packet.Type = PhoenixProject.Network.GamePackets._String.Effect;
                        Packet.TextsCount = 1;
                        Packet.Texts.Add("ridmatch_third");
                        client.SendScreen(Packet, true);
                        client.Entity.RacePoints += 10;
                    }
                }
                else
                {
                    client.Entity.ConquerPoints += 5000;
                    client.Entity.RacePoints += 5;
                    No++;
                }
                SendTimerStatus(client.Entity.Name);
                //FinishRace();
            }

        }
        public static void SendTimerStatus(string name)
        {
            Client.GameState[] players = null;
            players = PhoenixProject.ServerBase.Kernel.GamePool.Values;
            foreach (Client.GameState clients in players)
            {
                if (clients.Entity != null)
                {
                    if (clients.Entity != null)
                    {
                        if (clients.Entity.MapID == 1950)
                        {
                            string Minutes = "0";
                            int Minuts = DateTime.Now.Minute - 44;
                            if (Minuts.ToString().Contains('-'))
                                Minutes = Minuts.ToString().Replace('-', ' ');

                            string Seconds = "0";
                            int sec = DateTime.Now.Second - 0;
                            if (sec.ToString().Contains('-'))
                                Seconds = sec.ToString().Replace('-', ' ');

                            string milSeconds = "0";
                            int milsec = DateTime.Now.Millisecond - 0;
                            if (sec.ToString().Contains('-'))
                                milSeconds = milsec.ToString().Replace('-', ' ');

                            string mess = "No." + No + "           " + name + "    Time: " + Minuts + " : " + sec + " : " + milsec + " ";
                            clients.Send(new Network.GamePackets.Message(mess, System.Drawing.Color.White, Network.GamePackets.Message.ContinueRightCorner));
                        }
                    }
                }
            }
        }
        public static void Add_Competition(Client.GameState client)
        {

        }
    }
}
