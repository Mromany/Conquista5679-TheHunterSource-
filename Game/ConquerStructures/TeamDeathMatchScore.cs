using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
namespace PhoenixProject.Game.ConquerStructures
{
    public class TeamDeathMatchScore
    {
        public static int RedTeamScore = 0;
        public static int BlueTeamScore = 0;
        public static int BlackTeamScore = 0;
        public static int WhiteTeamScore = 0;
        public static bool RedCaptain = false;
        public static bool BlueCaptain = false;
        public static bool WhiteCaptain = false;
        public static bool BlackCaptain = false;
        public static bool redwin = false;
        public static bool blackwin = false;
        public static bool bluewin = false;
        public static bool whitewin = false;

        public static void Reward()
        {
            if (RedTeamScore >= BlueTeamScore && RedTeamScore >= WhiteTeamScore && RedTeamScore >= BlackTeamScore)
            {
                redwin = true;
            }
            else if (BlueTeamScore >= RedTeamScore && BlueTeamScore >= WhiteTeamScore && BlueTeamScore >= BlackTeamScore)
            {
                bluewin = true;
            }
            else if (WhiteTeamScore >= RedTeamScore && WhiteTeamScore >= BlueTeamScore && WhiteTeamScore >= BlackTeamScore)
            {
                whitewin = true;
            }
            else if (BlackTeamScore >= RedTeamScore && BlackTeamScore >= BlueTeamScore && BlackTeamScore >= WhiteTeamScore)
            {
                blackwin = true;
            }

            foreach (Client.GameState C in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                #region Winner
                if (C.Entity.TeamDeathMatch_RedTeam == true)
                {
                    if (redwin == true)
                    {
                        C.Entity.ConquerPoints += 65;
                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("RedTeam have won TeamDeathMatch! The Winner Team Have Gained 200 ConquerPoints", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                    }
                }
                if (C.Entity.TeamDeathMatch_BlueTeam == true)
                {
                    if (bluewin == true)
                    {
                        C.Entity.ConquerPoints += 65;
                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("BlueTeam have won TeamDeathMatch! The Winner Team Have Gained 200 ConquerPoints", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                    }
                }
                if (C.Entity.TeamDeathMatch_BlackTeam == true)
                {
                    if (blackwin == true)
                    {
                        C.Entity.ConquerPoints += 65;
                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("BlackTeam have won TeamDeathMatch! The Winner Team Have Gained 200 ConquerPoints", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                    }
                }
                if (C.Entity.TeamDeathMatch_WhiteTeam == true)
                {
                    if (whitewin == true)
                    {
                        C.Entity.ConquerPoints += 65;
                        PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("WhiteTeam have won TeamDeathMatch! The Winner Team Have Gained 200 ConquerPoints", System.Drawing.Color.Red, PhoenixProject.Network.GamePackets.Message.Center), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                    }
                }
                #endregion

                if (C.Entity.TeamDeathMatch_Kills >= 25 && C.Entity.TeamDeathMatch_Kills <= 49)
                {
                    C.Entity.ConquerPoints += 40;
                }
                if (C.Entity.TeamDeathMatch_Kills >= 50 && C.Entity.TeamDeathMatch_Kills <= 99)
                {
                    C.Entity.ConquerPoints += 80;
                }
                if (C.Entity.TeamDeathMatch_Kills >= 100 && C.Entity.TeamDeathMatch_Kills <= 149)
                {
                    C.Entity.ConquerPoints += 150;
                }
                if (C.Entity.TeamDeathMatch_Kills >= 150)
                {
                    C.Entity.ConquerPoints += 350;
                }
                C.Entity.Tournament_Signed = false;
                C.Entity.TeamDeathMatch_Kills = 0;
                C.Entity.TeamDeathMatch_RedCaptain = false;
                C.Entity.TeamDeathMatch_RedTeam = false;
                C.Entity.TeamDeathMatch_BlueCaptain = false;
                C.Entity.TeamDeathMatch_BlueTeam = false;
                C.Entity.TeamDeathMatch_BlackCaptain = false;
                C.Entity.TeamDeathMatch_BlackTeam = false;
                C.Entity.TeamDeathMatch_WhiteCaptain = false;
                C.Entity.TeamDeathMatch_WhiteTeam = false;
                C.Entity.RemoveFlag(Update.Flags.Flashy);
            }
        }
        public static void Reward2()
        {
            if (RedTeamScore >= BlueTeamScore && RedTeamScore >= WhiteTeamScore && RedTeamScore >= BlackTeamScore)
            {
                redwin = true;
            }
            else if (BlueTeamScore >= RedTeamScore && BlueTeamScore >= WhiteTeamScore && BlueTeamScore >= BlackTeamScore)
            {
                bluewin = true;
            }
            else if (WhiteTeamScore >= RedTeamScore && WhiteTeamScore >= BlueTeamScore && WhiteTeamScore >= BlackTeamScore)
            {
                whitewin = true;
            }
            else if (BlackTeamScore >= RedTeamScore && BlackTeamScore >= BlueTeamScore && BlackTeamScore >= WhiteTeamScore)
            {
                blackwin = true;
            }

            foreach (Client.GameState C in PhoenixProject.ServerBase.Kernel.GamePool.Values)
            {
                #region Winner
                if (C.Entity.TeamDeathMatch_RedTeam == true)
                {
                    if (redwin == true)
                    {
                        C.Entity.ConquerPoints += 65;
                    }
                }
                if (C.Entity.TeamDeathMatch_BlueTeam == true)
                {
                    if (bluewin == true)
                    {
                        C.Entity.ConquerPoints += 65;
                    }
                }
                if (C.Entity.TeamDeathMatch_BlackTeam == true)
                {
                    if (blackwin == true)
                    {
                        C.Entity.ConquerPoints += 65;
                    }
                }
                if (C.Entity.TeamDeathMatch_WhiteTeam == true)
                {
                    if (whitewin == true)
                    {
                        C.Entity.ConquerPoints += 65;
                    }
                }
                #endregion

                if (C.Entity.TeamDeathMatch_Kills >= 25 && C.Entity.TeamDeathMatch_Kills <= 49)
                {
                    C.Entity.ConquerPoints += 40;
                }
                if (C.Entity.TeamDeathMatch_Kills >= 50 && C.Entity.TeamDeathMatch_Kills <= 99)
                {
                    C.Entity.ConquerPoints += 80;
                }
                if (C.Entity.TeamDeathMatch_Kills >= 100 && C.Entity.TeamDeathMatch_Kills <= 149)
                {
                    C.Entity.ConquerPoints += 150;
                }
                if (C.Entity.TeamDeathMatch_Kills >= 150)
                {
                    C.Entity.ConquerPoints += 350;
                }
                C.Entity.Tournament_Signed = false;
                C.Entity.TeamDeathMatch_Kills = 0;
                C.Entity.TeamDeathMatch_RedCaptain = false;
                C.Entity.TeamDeathMatch_RedTeam = false;
                C.Entity.TeamDeathMatch_BlueCaptain = false;
                C.Entity.TeamDeathMatch_BlueTeam = false;
                C.Entity.TeamDeathMatch_BlackCaptain = false;
                C.Entity.TeamDeathMatch_BlackTeam = false;
                C.Entity.TeamDeathMatch_WhiteCaptain = false;
                C.Entity.TeamDeathMatch_WhiteTeam = false;
                C.Entity.RemoveFlag(Update.Flags.Flashy);
            }
        }
    }
}
