using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets.EventAlert
{
    public class EventAlert2
    {
        public const uint 
       
            CaptureFlag = 10535,
            SkillTeam = 10541,
            PowerArena = 10531,
            ElitePk = 10533,
            HorseRace = 10525,
            MonthlyPk = 10523,
            ClassPKWar = 10519,
            GuildWar = 10515,
            WeeklyPk = 10529;
        
        public static void Handle(byte[] Data, Client.GameState client)
        {

            EventAlert alert = new EventAlert(Data);

            switch (client.Entity.StrResID)
            {
                case ClassPKWar:
                    {
                        if (client.Entity.Class >= 10 && client.Entity.Class <= 15)
                        {
                            client.Entity.Teleport(7001, 25, 40);
                        }
                        if (client.Entity.Class >= 20 && client.Entity.Class <= 25)
                        {
                            client.Entity.Teleport(4500, 25, 40);
                        }
                        if (client.Entity.Class >= 40 && client.Entity.Class <= 45)
                        {
                            client.Entity.Teleport(4501, 25, 40);
                        }
                        if (client.Entity.Class >= 50 && client.Entity.Class <= 55)
                        {
                            client.Entity.Teleport(4502, 25, 40);
                        }
                        if (client.Entity.Class >= 60 && client.Entity.Class <= 65)
                        {
                            client.Entity.Teleport(4503, 25, 40);
                        }
                        if (client.Entity.Class >= 70 && client.Entity.Class <= 75)
                        {
                            client.Entity.Teleport(4504, 25, 40);
                        }
                        if (client.Entity.Class >= 132 && client.Entity.Class <= 135)
                        {
                            client.Entity.Teleport(4505, 25, 40);
                        }
                        if (client.Entity.Class >= 142 && client.Entity.Class <= 145)
                        {
                            client.Entity.Teleport(4506, 25, 40);
                        }
                        Data data = new Data(true);
                        data.ID = GamePackets.Data.OpenCustom;
                        data.UID = client.Entity.UID;
                        data.TimeStamp = Time32.Now;
                        data.dwParam = 3378;
                        data.wParam1 = client.Entity.X;
                        data.wParam2 = client.Entity.Y;
                        client.Send(data);
                        EventAlert alert2 = new EventAlert
                        {
                            StrResID = 10520,
                            Countdown = 4,
                            UK12 = 1
                        };
                        client.Entity.StrResID = 0;
                        client.Send((byte[])alert2);
                        break;
                    }
                case CaptureFlag:
                    client.Entity.Teleport(1002, 384, 348);
                    EventAlert alert3 = new EventAlert
                        {
                            StrResID = 10536,
                            Countdown = 4,
                            UK12 = 1
                        };
                        client.Entity.StrResID = 0;
                        client.Send((byte[])alert3);
                    break;
                case SkillTeam:
                    client.Entity.Teleport(1002, 460, 367);
                    EventAlert alert4 = new EventAlert
                        {
                            StrResID = 10542,
                            Countdown = 4,
                            UK12 = 1
                        };
                        client.Entity.StrResID = 0;
                        client.Send((byte[])alert4);
                    break;
                case PowerArena:
                    {
                        client.Entity.Teleport(8877, 52, 44);
                        Data datas = new Data(true);
                        datas.ID = GamePackets.Data.OpenCustom;
                        datas.UID = client.Entity.UID;
                        datas.TimeStamp = Time32.Now;
                        datas.dwParam = 3378;
                        datas.wParam1 = client.Entity.X;
                        datas.wParam2 = client.Entity.Y;
                        client.Send(datas);
                        EventAlert alert5 = new EventAlert
                        {
                            StrResID = 10532,
                            Countdown = 4,
                            UK12 = 1
                        };
                        client.Entity.StrResID = 0;
                        client.Send((byte[])alert5);
                        break;
                    }
                case ElitePk:
                    {
                        Game.Tournaments.EliteTournament.AddMap(client);
                        Data datass = new Data(true);
                        datass.ID = GamePackets.Data.OpenCustom;
                        datass.UID = client.Entity.UID;
                        datass.TimeStamp = Time32.Now;
                        datass.dwParam = 3378;
                        datass.wParam1 = client.Entity.X;
                        datass.wParam2 = client.Entity.Y;
                        client.Send(datass);
                        EventAlert alert2 = new EventAlert
                        {
                            StrResID = 10534,
                            Countdown = 4,
                            UK12 = 1
                        };
                        client.Entity.StrResID = 0;
                        client.Send((byte[])alert2);
                        break;
                    }
                case HorseRace:
                    client.Entity.Teleport(1950, 136, 245);
                    EventAlert alert6 = new EventAlert
                        {
                            StrResID = 10526,
                            Countdown = 4,
                            UK12 = 1
                        };
                        client.Entity.StrResID = 0;
                        client.Send((byte[])alert6);
                    break;
                case MonthlyPk:
                    client.Entity.Teleport(1002, 428, 243);
                    EventAlert alert7 = new EventAlert
                        {
                            StrResID = 10524,
                            Countdown = 4,
                            UK12 = 1
                        };
                        client.Entity.StrResID = 0;
                        client.Send((byte[])alert7);
                    break;
                case GuildWar:
                    client.Entity.Teleport(1038, 340, 331);
                    EventAlert alert8 = new EventAlert
                        {
                            StrResID = 10516,
                            Countdown = 4,
                            UK12 = 1
                        };
                        client.Entity.StrResID = 0;
                        client.Send((byte[])alert8);
                    break;
                case WeeklyPk:
                    client.Entity.Teleport(1002, 453, 294);
                    EventAlert alert9 = new EventAlert
                        {
                            StrResID = 10530,
                            Countdown = 4,
                            UK12 = 1
                        };
                        client.Entity.StrResID = 0;
                        client.Send((byte[])alert9);
                    break;
            }
        }
    }
}
