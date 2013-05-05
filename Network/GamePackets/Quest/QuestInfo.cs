using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Database;

namespace PhoenixProject.Network.GamePackets.Quest
{
    public class QuestInfo
    {
        public static void Handle(byte[] Data, Client.GameState Client)
        {

            QuestInfoPacket info = new QuestInfoPacket(8);
            ushort num = BitConverter.ToUInt16(Data, 4);
            if (num == 3)
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.SELECT);
                cmd.Select("heroquests").Where("hero", Client.Entity.UID).And("quest", 0xb71b2);
                PhoenixProject.Database.MySqlReader reader = new PhoenixProject.Database.MySqlReader(cmd);
                if (Client != null)
                {
                    info.Type = 3;
                    while (reader.Read())
                    {
                        info.AddQuest(reader.ReadUInt32("quest"), (QuestCompleteTypes)((ushort)reader.ReadUInt32("completeflag")));
                        HeroQuest quest2 = new HeroQuest
                        {
                            Identifier = reader.ReadUInt32("quest"),
                            DailyFinishes =  reader.ReadUInt32("dailyfinishes"),
                            CompleteFlag = (QuestCompleteTypes)((ushort) reader.ReadUInt32("completeflag"))
                        };
                        DateTime time3 = new DateTime(0x7b2, 1, 1);
                        quest2.CompleteTime = time3.ToLocalTime().AddSeconds(reader.ReadUInt32("completetime"));

                       // quest2.CompleteTime = new DateTime().FromUnix(Convert.ToUInt32(reader["completetime"]));
                        quest2.Step = reader.ReadUInt32("step");
                        HeroQuest quest = quest2;
                        Client.Quests.GetOrAdd(quest.Identifier, quest);
                    }
                    reader.Close();
                    reader.Dispose();
                }
                Client.Send((byte[])info);
                if (Client != null)
                {
                    cmd.Select("killtargets").Where("hero", Client.Entity.UID);
                    while (reader.Read())
                    {
                        QuestQuery query = new QuestQuery
                        {
                            Identifier = reader.ReadUInt32("quest"),
                            Unknown2 = reader.ReadUInt32("count")
                        };
                        Client.Send((byte[])query);
                    }
                    reader.Close();
                    reader.Dispose();
                }
            }
            else
            {
                Console.WriteLine("Unhandled QuestInfo (1134) Type "+num+"");
            }
        }

    }
}
