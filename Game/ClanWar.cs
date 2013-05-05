using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game
{
    class ClanWar//Coded By Kimo
    {
       
        public static SobNpcSpawn ClanFlag;
        public static SobNpcSpawn ClanFlag2;

        public static SafeDictionary<uint, Game.Clans> Scores = new SafeDictionary<uint, Game.Clans>(100);

        public static bool IsWar = false, Flame10th = false, FirstRound = false;

        public static Time32 ScoreSendStamp, LastWin;

        public static Game.Clans PoleKeeper, CurrentTopLeader;

        private static bool changed = false;

        public static int Claim = 0;

        private static string[] scoreMessages;

        public static DateTime StartTime;

        public static void Initiate()
        {
            var Map = ServerBase.Kernel.Maps[1509];
            ClanFlag = (SobNpcSpawn)Map.Npcs[812];
            
        }
        public static void Initiate2()
        {
            var Map = ServerBase.Kernel.Maps[1002];
            ClanFlag2 = (SobNpcSpawn)Map.Npcs[814];

        }

        public static void Start()
        {
            Scores = new SafeDictionary<uint, Game.Clans>(100);
            StartTime = DateTime.Now;
            //LeftGate.Mesh = (ushort)(240 + LeftGate.Mesh % 10);
            //RightGate.Mesh = (ushort)(270 + LeftGate.Mesh % 10);
            ServerBase.Kernel.SendWorldMessage(new Message("Clan war has began!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
            FirstRound = true;
            IsWar = true;
        }

        public static void Reset()
        {
            Scores = new SafeDictionary<uint, Game.Clans>(100);
            ClanFlag.Hitpoints = ClanFlag.MaxHitpoints;

            IsWar = true;
        }

        public static void FinishRound()
        {
            if (PoleKeeper != null && !FirstRound)
            {
               
            }
            LastWin = Time32.Now;

            FirstRound = false;
            SortScores(out PoleKeeper);
            if (PoleKeeper != null)
            {
                ServerBase.Kernel.SendWorldMessage(new Message("The Clan, " + PoleKeeper.ClanName + ", owned by " + PoleKeeper.ClanLider + " has won this Clan war round!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);

                ClanFlag.Name = PoleKeeper.ClanName;
            }
            ClanFlag.Hitpoints = ClanFlag.MaxHitpoints;
            ServerBase.Kernel.SendWorldMessage(ClanFlag, ServerBase.Kernel.GamePool.Values, (ushort)1509);
            Reset();
        }

        public static void End()
        {
            if (PoleKeeper != null)
            {
                ServerBase.Kernel.SendWorldMessage(new Message("The Clan, " + PoleKeeper.ClanName + ", owned by " + PoleKeeper.ClanLider + " has won this Clan war!---Clan war has ended!", System.Drawing.Color.White, Message.Center), ServerBase.Kernel.GamePool.Values);
                //Game.Flags.RemoveGuildDeaputy();
                //Game.Flags.RemoveGuildDeaputy();
            }
            else
            {
                ServerBase.Kernel.SendWorldMessage(new Message("Clan war has ended and there was no winner!", System.Drawing.Color.Red, Message.Center), ServerBase.Kernel.GamePool.Values);
                //Game.Flags.RemoveGuildDeaputy();
                //Game.Flags.RemoveGuildDeaputy();
            }
            IsWar = false;
            UpdatePole(ClanFlag);
           
        }

        public static void AddScore(uint addScore, Game.Clans guild)
        {
            if (guild != null)
            {
                guild.WarScore += addScore;
                if ((int)ClanFlag.Hitpoints <= 0)
                {
                    FinishRound();

                    return;
                }
                changed = true;
                if (!Scores.ContainsKey(guild.ClanId))
                    Scores.Add(guild.ClanId, guild);
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
                ServerBase.Kernel.SendWorldMessage(msg, ServerBase.Kernel.GamePool.Values, (ushort)1509);
                //ServerBase.Kernel.SendWorldMessage(msg, ServerBase.Kernel.GamePool.Values, (ushort)6001);
            }
        }

        private static void SortScores(out Game.Clans winner)
        {

            winner = null;
            List<string> ret = new List<string>();
            string strs = "" + Database.rates.servername + " ClanWar:";
            ret.Add(strs);
            SortedDictionary<uint, SortEntry<uint, Game.Clans>> sortdict = new SortedDictionary<uint, SortEntry<uint, Game.Clans>>();

            foreach (Game.Clans guild in Scores.Values)
            {
                if (!ServerBase.Kernel.ServerClans.ContainsKey(guild.ClanId))
                    continue;

                if (sortdict.ContainsKey(guild.WarScore))
                {
                    sortdict[guild.WarScore].Values.Add(guild.ClanId, guild);
                }
                else
                {
                    sortdict.Add(guild.WarScore, new SortEntry<uint, Game.Clans>());
                    sortdict[guild.WarScore].Values = new Dictionary<uint, Game.Clans>();
                    sortdict[guild.WarScore].Values.Add(guild.ClanId, guild);
                }
            }
            int Place = 0;
            foreach (KeyValuePair<uint, SortEntry<uint, Game.Clans>> entries in sortdict.Reverse())
            {
                
                foreach (Game.Clans guild in entries.Value.Values.Values)
                {
                    if (Place == 0)
                        winner = guild;
                    string str = "No  " + (Place + 1).ToString() + ": " + guild.ClanName + "(" + entries.Key + ")";
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

        private static void UpdatePole(SobNpcSpawn ClanFlag)
        {
            new Database.MySqlCommand(PhoenixProject.Database.MySqlCommandType.UPDATE)
            .Update("sobnpcs").Set("name", ClanFlag.Name).Set("life", ClanFlag.Hitpoints).Where("id", ClanFlag.UID).Execute();
        }
    }
}
