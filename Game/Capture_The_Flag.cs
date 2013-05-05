using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game
{
    public enum TeamType
    {
        Red,
        Blue,
        White,
        Black,
        NONE
    }
    public class Team
    {
        public TeamType TeamName;
        public SobNpcSpawn GroundFlag;
        public SobNpcSpawn Flag;
        public static bool RedCapture = false;
        public static bool WhiteCapture = false;
        public static bool BlackCapture = false;
        public static bool BlueCapture = false;
        public static uint RScore = 0;
        public static uint WScore = 0;
        public static uint BScore = 0;
        public static uint BLScore = 0;
        public static bool IsWar = false;
        public static uint GarmID = 181525;//black
        public Map Map;
        public Team Red;
        public Team Blue;
        public Team White;
        public Team Black;
        //private Handler CTF;
        //public GenericActionList<DelayedActionType> DelayedActions;
        public Team(TeamType us, ushort X, ushort Y, uint FlagUID)
        {
            //DelayedActions = new GenericActionList<DelayedActionType>();
            TeamName = us;
            Flag = new SobNpcSpawn
            {
                Name = TeamName.ToString() + "Flag",
                UID = FlagUID,
                MapID = 2060,
                X = X,
                Y = Y,
                MaxHitpoints = 5000,
                Hitpoints = 5000,
                Mesh = 8684,
                //Sort = 21,
                Type = Enums.NpcType.Stake,
            };
            

            GroundFlag = new SobNpcSpawn
            {
                Name = TeamName.ToString() + "Flag",
                UID = FlagUID + 10,
                MapID = 2060,
                X = X,
                Y = Y,
                MaxHitpoints = 20000,
                Hitpoints = 20000,
                Mesh = 8910,
                //Sort = 21,
                Type = Enums.NpcType.Stake,
            };
            Map.AddNpc(Flag);
        }

        public static void ClaimBLUE()
        {
            foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
            {
                if (client != null)
                {
                    if (client.Entity.MapID == 2060)
                    {
                        if (client.CaptureB)
                        {
                            client.Entity.Teleport(1002, 429, 379);
                            client.Entity.ConquerPoints += Database.rates.CaptureFlag;
                        }
                        else
                        {
                            client.Entity.Teleport(1002, 429, 379);
                        }
                    }
                }
            }
            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations! BlueTeam has won CaptureTheFlag with Score:" + Game.Team.BScore + " and Each member claimed " + Database.rates.CaptureFlag + " cps", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
        }
        public static void ClaimRed()
        {
            foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
            {
                if (client != null)
                {
                    if (client.Entity.MapID == 2060)
                    {
                        if (client.CaptureR)
                        {
                            client.Entity.Teleport(1002, 429, 379);
                            client.Entity.ConquerPoints += Database.rates.CaptureFlag;
                        }
                        else
                        {
                            client.Entity.Teleport(1002, 429, 379);
                        }
                    }
                }
            }
            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations! RedTeam has won CaptureTheFlag with Score:" + Game.Team.RScore + " and Each member claimed " + Database.rates.CaptureFlag + " cps", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
        }
        public static void ClaimWhite()
        {
            foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
            {
                if (client != null)
                {
                    if (client.Entity.MapID == 2060)
                    {
                        if (client.CaptureW)
                        {
                            client.Entity.Teleport(1002, 429, 379);
                            client.Entity.ConquerPoints += Database.rates.CaptureFlag;
                        }
                        else
                        {
                            client.Entity.Teleport(1002, 429, 379);
                        }
                    }
                }
            }
            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations! WhiteTeam has won CaptureTheFlag with Score:" + Game.Team.WScore + " and Each member claimed " + Database.rates.CaptureFlag + " cps", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
        }
        public static void ClaimBlack()
        {
            foreach (Client.GameState client in ServerBase.Kernel.GamePool.Values)
            {
                if (client != null)
                {
                    if (client.Entity.MapID == 2060)
                    {
                        if (client.CaptureBL)
                        {
                            client.Entity.Teleport(1002, 429, 379);
                            client.Entity.ConquerPoints += Database.rates.CaptureFlag;
                        }
                        else
                        {
                            client.Entity.Teleport(1002, 429, 379);
                        }
                    }
                }
            }
            PhoenixProject.ServerBase.Kernel.SendWorldMessage(new Message("Congratulations! BlackTeam has won CaptureTheFlag with Score:" + Game.Team.BLScore + " and Each member claimed "+Database.rates.CaptureFlag+" cps", System.Drawing.Color.Black, Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
        }
        public static void CaptureRes(Client.GameState client)
        {
            string[] scores = new string[5];
            scores[0] = "CaptureTheFlag PkWar:";
            scores[1] = "Red   Team Score: "+ RScore +"";
            scores[2] = "Blue  Team Score: "+ BScore +"";
            scores[3] = "Black Team Score: "+ BLScore +"";
            scores[4] = "White Team Score: "+ WScore +"";
            // scores[3] = "Red   Team: " + Game.ConquerStructures.TeamDeathMatchScore.RedTeamScore + " Score";
            //scores[4] = "Your Score: " + Entity.TeamDeathMatch_Kills + " kills";
            for (int i = 0; i < scores.Length; i++)
            {
                Message msg = new Message(scores[i], System.Drawing.Color.Red, i == 0 ? Message.FirstRightCorner : Message.ContinueRightCorner);
                client.Send(msg);
            }
        }
    }
}
