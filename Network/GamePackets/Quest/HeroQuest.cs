using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Interfaces;
using PhoenixProject.Database;

namespace PhoenixProject.Network.GamePackets.Quest
{
    public class HeroQuest
    {
        public QuestCompleteTypes CompleteFlag;
        public DateTime CompleteTime;
        public uint DailyFinishes;
        public uint Identifier;
        public uint Step;

        public static bool AddQuest(Client.GameState h, HeroQuest q)
        {
            if (h.Quests.TryAdd(q.Identifier, q))
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("heroquests").Insert("hero", h.Entity.UID).Insert("quest", q.Identifier)
                    .Insert("npc", "0").Insert("flag", (uint)q.CompleteFlag).Insert("step", q.Step)
                    .Insert("time", "0")
                    .Insert("daily", "0").Execute();
            
                QuestInfoPacket info = new QuestInfoPacket(0x10)
                {
                    Type = 1,
                    QuestIdentifier = q.Identifier,
                    QuestType = QuestCompleteTypes.Accepted
                };
                h.Send((byte[])info);
            }
            return false;
        }

        public static bool AddQuest(Client.GameState h, uint questID)
        {
            HeroQuest q = new HeroQuest
            {
                Identifier = questID
            };
            return AddQuest(h, q);
        }

        public static bool CanAccept(Client.GameState h, uint questID)
        {
            Quest quest;
            if (ServerBase.Kernel.Quest.TryGetValue(questID, out quest))
            {
                HeroQuest quest3;
                if ((h.Entity.Level > quest.MaxLevel) || (h.Entity.Level < quest.MinLevel))
                {
                    return false;
                }
                foreach (uint num in quest.Prerequests)
                {
                    HeroQuest quest2;
                    if (h.Quests.TryGetValue(num, out quest2))
                    {
                        if (quest2.CompleteFlag != QuestCompleteTypes.Done)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                if (h.Quests.TryGetValue(questID, out quest3) && ((quest.Type != QuestTypes.Daily) && (quest3.CompleteFlag == QuestCompleteTypes.Done)))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        public static bool Complete(Client.GameState h, uint questID, bool effect = true)
        {
            HeroQuest quest;
            if (h.Quests.TryGetValue(questID, out quest))
            {
                Quest quest2;
                if (ServerBase.Kernel.Quest.TryGetValue(questID, out quest2))
                {
                    quest.CompleteFlag = QuestCompleteTypes.Done;
                    quest.CompleteTime = DateTime.Now;
                    foreach (IConquerItem view in quest2.ItemPrizes)
                    {
                        if (h.Inventory.Count > 39)
                        {
                            Database.ConquerItemBaseInformation item;
                            string name = "reward";
                            if (PhoenixProject.Database.ConquerItemInformation.BaseInformations.TryGetValue(view.ID, out item))
                            {
                                name = item.Name;
                            }
                            string msg = string.Format("You don't have room for the " + name + ", go to the PrizeOfficer in Market to claim it.!", h.Entity.Name, h.Entity.Name);
                            h.Send(new Message(msg, System.Drawing.Color.White, Message.Talk));
                           // h.Send((byte[])new MessagePacket("You don't have room for the " + name + ", go to the PrizeOfficer in Market to claim it.", MessagePacket.Channels.Talk));
                        }
                        else
                        {
                            h.Inventory.Add(view, Game.Enums.ItemUse.CreateAndAdd);
                        }
                    }
                    h.Entity.Money += quest2.Prizes[QuestPrize.Gold];
                    h.Entity.ConquerPoints += quest2.Prizes[QuestPrize.CP];
                    h.IncreaseExperience((ulong)quest2.Prizes[QuestPrize.Exp], false);
                    QuestInfoPacket info = new QuestInfoPacket(0x10)
                    {
                        Type = 4,
                        QuestIdentifier = questID,
                        QuestType = QuestCompleteTypes.Done
                    };
                    h.Send((byte[])info);
                    if (effect)
                    {
                        Data packet = new Data(true);
                        packet.UID = h.Entity.UID;
                        packet.ID = Data.OpenCustom;
                        packet.dwParam = 0xc4b;
                        h.Send(packet);
                       
                    }
                }
                uint times;
                DateTime time2 = new DateTime(0x7b2, 1, 1);
            TimeSpan span = (TimeSpan) (DateTime.Now - time2.ToLocalTime());
            times = (uint)span.TotalSeconds;

                MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
                cmd3.Update("heroquests").Set("q", questID).Set("time", times)
                    .Set("flag", 1).Where("hero", h.Entity.UID).Execute();

                return true;
            }
            return false;
        }

        public static bool OnQuest(Client.GameState h, uint questID)
        {
            HeroQuest quest;
            return (h.Quests.TryGetValue(questID, out quest) && (quest.CompleteFlag == QuestCompleteTypes.Accepted));
        }
    }
}
