using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.GamePackets.Quest;
using KinSocket.Database;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Game;
using System.Collections.Concurrent;

namespace PhoenixProject.Database
{
    public class Quests 
    {
        public static void Load()
        {
            Ini ini = new Ini(Program.QuestInfo2);
            int num = ini.ReadInt32("TotalMission", "TotalMission");
            for (int i = 0; i <= num; i++)
            {
                
                Quest quest = new Quest
                {
                    Type = (QuestTypes)((byte)ini.ReadInt32(i.ToString(), "TypeId")),
                    CompleteFlag = ini.ReadInt32(i.ToString(), "CompleteFlag"),
                    ActivityType = ini.ReadInt32(i.ToString(), "ActivityType"),
                    Identifier = ini.ReadUInt32(i.ToString(), "MissionId"),
                    Name = ini.Read(i.ToString(), "Name"),
                    MinLevel = ini.ReadUInt16(i.ToString(), "Lv_min"),
                    MaxLevel = ini.ReadUInt16(i.ToString(), "Lv_max"),
                    Auto = Convert.ToBoolean(ini.ReadByte(i.ToString(), "Auto")),
                    First = Convert.ToBoolean(ini.ReadByte(i.ToString(), "First"))
                };
                string source = ini.Read(i.ToString(), "Prequest");
                if (source.Contains<char>('|'))
                {
                    foreach (string str2 in source.Split(new char[] { '|' }))
                    {
                        quest.Prerequests.Add(Convert.ToUInt32(str2));
                    }
                }
                else if ((source != "0") && (source != string.Empty))
                {
                    quest.Prerequests.Add(Convert.ToUInt32(source));
                }
                quest.Map = ini.ReadUInt32(i.ToString(), "Map");
                string str3 = ini.Read(i.ToString(), "Profession");
                if (str3.Contains<char>(','))
                {
                    foreach (string str2 in str3.Split(new char[] { ',' }))
                    {
                        quest.Professions.Add((PhoenixProject.Network.GamePackets.Quest.Quest.ClassNames)Convert.ToByte(str2));
                    }
                }
                else if ((source != "0") && (source != string.Empty))
                {
                    quest.Professions.Add((PhoenixProject.Network.GamePackets.Quest.Quest.ClassNames)Convert.ToByte(str3));
                }
                quest.Sex = ini.ReadUInt16(i.ToString(), "Sex");
                quest.FinishTime = ini.Read(i.ToString(), "FinishTime");
                quest.ActivityBeginTime = ini.Read(i.ToString(), "ActivityBeginTime");
                quest.ActivityEndTime = ini.Read(i.ToString(), "ActivityEndTime");
                string str4 = ini.Read(i.ToString(), "Prize");
                string str5 = str4;
                string str6 = string.Empty;
                if (str4.Contains<char>('['))
                {
                    str5 = str4.Remove(str4.IndexOf('['));
                    str6 = str4.Remove(0, str4.IndexOf('[')).Replace(' ', ':').Replace(']', ' ').Replace("[", "");
                }
                IEnumerable<string> first = str5.Contains<char>(' ') ? ((IEnumerable<string>)str5.Split(new char[] { ' ' })) : ((IEnumerable<string>)new string[] { str5 });
                if (str6 != string.Empty)
                {
                    first = first.Union<string>(str6.Split(new char[] { ' ' }));
                }
                foreach (string str7 in first)
                {
                    string[] strArray;
                    Dictionary<QuestPrize, uint> dictionary;
                    if (str7.Contains<char>(':'))
                    {
                        strArray = str7.ToLower().Split(new char[] { ':' });
                        string str8 = strArray[0];
                        if (str8 != null)
                        {
                            if (!(str8 == "exp"))
                            {
                                if (str8 == "gold")
                                {
                                    goto Label_048B;
                                }
                                if (str8 == "cp")
                                {
                                    goto Label_04B2;
                                }
                                if (str8 == "item")
                                {
                                    goto Label_04D9;
                                }
                            }
                            else
                            {
                                (dictionary = quest.Prizes)[QuestPrize.Exp] = dictionary[QuestPrize.Exp] + Convert.ToUInt32(strArray[1]);
                            }
                        }
                    }
                    continue;
                Label_048B:
                    (dictionary = quest.Prizes)[QuestPrize.Gold] = dictionary[QuestPrize.Gold] + Convert.ToUInt32(strArray[1]);
                    continue;
                Label_04B2:
                    (dictionary = quest.Prizes)[QuestPrize.CP] = dictionary[QuestPrize.CP] + Convert.ToUInt32(strArray[1]);
                    continue;
                Label_04D9: ;
                    string[] strArray2 = strArray[1].Split(new char[] { ',' });
                    BoothItem2 item = new BoothItem2
                    {
                        Identifier = 0,
                        ItemID = Convert.ToUInt32(strArray2[0]),
                        Plus = Convert.ToByte(strArray2[1]),
                        SocketOne = Convert.ToByte(strArray2[2]),
                        SocketTwo = Convert.ToByte(strArray2[3]),
                        Color = (uint)(Enums.Color)Convert.ToByte(strArray2[4]),
                        Enchant = Convert.ToByte(strArray2[5]),
                        Bless = Convert.ToByte(strArray2[6])
                    };
                   
                    quest.ItemPrizes.Add(item);
                }
                ServerBase.Kernel.Quest.TryAdd(quest.Identifier, quest);
            }
        }

        public static uint StartingQuest(ushort job)
        {
            return new uint[] { 0, 500, 0x1f5, 0, 0x1f6, 0x1f7, 0x1f8, 0x1f9, 0, 0, 0x1fa }[job / 10];
        }
    }
}
