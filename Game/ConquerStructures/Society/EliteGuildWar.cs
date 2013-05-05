using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;
namespace PhoenixProject.Game.ConquerStructures.Society
{
    public class EliteGuildWar//Coded By Kimo
    {
        public static SobNpcSpawn Poles;

        public static int Claim = 0;
        public static SafeDictionary<uint, Guild> Scores = new SafeDictionary<uint, Guild>(100);

        public static bool IsWar = false, Flame10th = false, FirstRound = false;

        public static Time32 ScoreSendStamp, LastWin;

        public static Guild PoleKeeper, CurrentTopLeader;

        private static bool changed = false;

        private static string[] scoreMessages;

        public static DateTime StartTime;


        public static void EliteGwint()
        {
            var Map = ServerBase.Kernel.Maps[2071];
            Poles = (SobNpcSpawn)Map.Npcs[811];
        }

        public static void Start()
        {
            Scores = new SafeDictionary<uint, Guild>(100);
            StartTime = DateTime.Now;
            ServerBase.Kernel.SendWorldMessage(new Message("Elite Guild war has began!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
            FirstRound = true;
            foreach (Guild guild in ServerBase.Kernel.Guilds.Values)
            {
                guild.WarScore = 0;
            }
            IsWar = true;
        }

        public static void Reset()
        {
            Scores = new SafeDictionary<uint, Guild>(100);

            Poles.Hitpoints = Poles.MaxHitpoints;


            foreach (Guild guild in ServerBase.Kernel.Guilds.Values)
            {
                guild.WarScore = 0;
            }

            IsWar = true;
        }

        public static void FinishRound()
        {
            if (PoleKeeper != null && !FirstRound)
            {
                if (PoleKeeper.Wins == 0)
                    PoleKeeper.Losts++;
                else
                    PoleKeeper.Wins--;
                Database.GuildTable.UpdateGuildWarStats(PoleKeeper);
            }
            LastWin = Time32.Now;

            FirstRound = false;
            SortScores(out PoleKeeper);
            if (PoleKeeper != null)
            {
                ServerBase.Kernel.SendWorldMessage(new Message("The guild, " + PoleKeeper.Name + ", owned by " + PoleKeeper.LeaderName + " has won this guild war round!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
                //ServerBase.Kernel.SendWorldMessage(new Message("It is generald pardon time. You have 5 minutes to leave, run for your life!", System.Drawing.Color.White, Message.TopLeft), ServerBase.Kernel.GamePool.Values, (ushort)6001);
                if (PoleKeeper.Losts == 0)
                    PoleKeeper.Wins++;
                else
                    PoleKeeper.Losts--;
                Database.GuildTable.UpdateGuildWarStats(PoleKeeper);
                Poles.Name = PoleKeeper.Name;
            }
            Poles.Hitpoints = Poles.MaxHitpoints;
            ServerBase.Kernel.SendWorldMessage(Poles, ServerBase.Kernel.GamePool.Values, (ushort)2071);
            Reset();
        }

        public static void End()
        {
            if (PoleKeeper != null)
            {
                ServerBase.Kernel.SendWorldMessage(new Message("The Elite guild, " + PoleKeeper.Name + ", owned by " + PoleKeeper.LeaderName + " has won this guild war!---Guild war has ended!", System.Drawing.Color.White, Message.Center), ServerBase.Kernel.GamePool.Values);
                //PhoenixProject.Database.EntityTable.Status2();
            }
            else
            {
                ServerBase.Kernel.SendWorldMessage(new Message("Elite Guild war has ended and there was no winner!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
                //PhoenixProject.Database.EntityTable.Status2();
            }
            IsWar = false;
            UpdatePole(Poles);
        }

        public static void AddScore(uint addScore, Guild guild)
        {
            if (guild != null)
            {
                guild.WarScore += addScore;
                if ((int)Poles.Hitpoints <= 0)
                {
                    FinishRound();

                    return;
                }
                changed = true;
                if (!Scores.ContainsKey(guild.ID))
                    Scores.Add(guild.ID, guild);
            }
        }

        public static void SendScores()
        {
            if (scoreMessages == null)
                scoreMessages = new string[0];
            if (Scores.Count == 0)
                return;
            if (changed)
                SortScores(out CurrentTopLeader);

            for (int c = 0; c < scoreMessages.Length; c++)
            {
                Message msg = new Message(scoreMessages[c], System.Drawing.Color.Red, c == 0 ? Message.FirstRightCorner : Message.ContinueRightCorner);
                ServerBase.Kernel.SendWorldMessage(msg, ServerBase.Kernel.GamePool.Values, (ushort)2071);
                ServerBase.Kernel.SendWorldMessage(msg, ServerBase.Kernel.GamePool.Values, (ushort)6001);
            }
        }

        private static void SortScores(out Guild winner)
        {
            winner = null;
            List<string> ret = new List<string>();

            SortedDictionary<uint, SortEntry<uint, Guild>> sortdict = new SortedDictionary<uint, SortEntry<uint, Guild>>();

            foreach (Guild guild in Scores.Values)
            {
                if (!ServerBase.Kernel.Guilds.ContainsKey(guild.ID))
                    continue;

                if (sortdict.ContainsKey(guild.WarScore))
                {
                    sortdict[guild.WarScore].Values.Add(guild.ID, guild);
                }
                else
                {
                    sortdict.Add(guild.WarScore, new SortEntry<uint, Guild>());
                    sortdict[guild.WarScore].Values = new Dictionary<uint, Guild>();
                    sortdict[guild.WarScore].Values.Add(guild.ID, guild);
                }
            }
            int Place = 0;
            foreach (KeyValuePair<uint, SortEntry<uint, Guild>> entries in sortdict.Reverse())
            {
                foreach (Guild guild in entries.Value.Values.Values)
                {
                    if (Place == 0)
                        winner = guild;
                    string str = "No  " + (Place + 1).ToString() + ": " + guild.Name + "(" + entries.Key + ")";
                    ret.Add(str);
                    Place++;
                    if (Place == 4)
                        break;
                }
                if (Place == 4)
                    break;
            }

            changed = false;
            scoreMessages = ret.ToArray();
        }

        private static void UpdatePole(SobNpcSpawn pole)
        {
            new Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE)
            .Update("sobnpcs").Set("name", pole.Name).Set("life", Poles.Hitpoints).Where("id", pole.UID).Execute();
        }
    }
}
