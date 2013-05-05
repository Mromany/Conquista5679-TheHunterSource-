using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game
{
    class KimoSkillWar
    {
        public static uint GarmID = 191305;
        public static uint MapID = 7009;
        public static uint RTeamNum = 0;
        public static uint RKills = 0;
        public static uint YKills= 0;
        public static uint YTeamNum = 0;
        public static uint Round = 0;
        public static uint Winner = 0;
        public static bool SignUP = false;

        public static Time32 StartAt;
        public static Time32 EndAt;
        public static bool Started = false;
        public static bool Running = false;


        public static void Start()
        {
            Round++;
            Started = true;
            Running = true;
            SignUP = true;
        }
        public static void END()
        {
            Started = false;
            Running = false;
            SignUP = false;
        }
        public static void SkillTeamRes(Client.GameState client)
        {
            string[] scores = new string[3];
            scores[0] = "SkillTeam PkWar:";
            scores[1] = "Red Team: " + RKills + " Score";
            scores[2] = "Yellow Team: " + YKills + " Score";
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
