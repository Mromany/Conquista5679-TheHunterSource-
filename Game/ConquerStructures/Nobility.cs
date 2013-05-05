using System;
using System.Linq;
using System.Collections.Generic;
using PhoenixProject.Network.GamePackets;
using System.IO;
using KinSocket;

namespace PhoenixProject.Game.ConquerStructures
{
    public class Nobility
    {
        public static PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks GetFemale(PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks rank)
        {
            switch (rank)
            {
                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Knight:
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Lady;

                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Lady:
                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Baroness:
                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Countess:
                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Dutchess:
                    return rank;

                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Baron:
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Baroness;

                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Earl:
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Countess;

                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Duke:
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Dutchess;

                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Prince:
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Princess;

                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.King:
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Queen;
            }
            return rank;
        }

        public static ulong GetMinimumDonation(PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks Rank)
        {
            switch (Rank)
            {
                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Knight:
                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Lady:
                    return 0x1c9c380L;

                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Baron:
                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Baroness:
                    return 0x5f5e100L;

                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Earl:
                case PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Countess:
                    return 0xbebc200L;
            }
            return MinimumDonationFor(Rank);
        }
       
        public static SafeDictionary<uint, NobilityInformation> Board = new SafeDictionary<uint, NobilityInformation>(10000);
        public static List<NobilityInformation> BoardList = new List<NobilityInformation>(10000);
        public static ulong MinimumDonationFor(PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks Rank)
        {
            ulong num = 0L;
            for (int c = 0; c < BoardList.Count; c++)
            {
                

                sbyte place = 0;
                PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks commoner = PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Commoner;
                uint identifier = BoardList[c].EntityUID;
                ulong donation = BoardList[c].Donation;
                GetNobilityStats(identifier, donation, ref place, ref commoner);
                if ((commoner == Rank) && ((donation < num) || (num == 0L)))
                {
                    num = donation;
                }

            }
            return (num + ((ulong)1L));
        }
        public static void GetNobilityStats(uint Identifier, ulong Donation, ref sbyte Place, ref PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks Rank)
        {
            sbyte place = 0;
            PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks commoner = PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Commoner;
            for (int c = 0; c < 50; c++)
            {

                if (BoardList[c].EntityUID == Identifier)
                {
                    break;
                }
                if (place < 50)
                {
                    place = (sbyte)(place + 1);
                }
            }
            
            Place = (place < 50) ? place : ((sbyte)(-1));
            if (Donation == 0L)
            {
                Place = -1;
            }
            commoner = GetRanking(Donation, place);
            Rank = commoner;
        }
        public static PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks GetRanking(ulong Donation, sbyte Place)
        {
            if ((Donation != 0L) && (Place >= 0))
            {
                if ((Place < Database.rates.king) && (Donation > 0L))
                {
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.King;
                }
                if ((Place < 0x12) && (Donation > 0L))
                {
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Prince;
                }
                if ((Place < 0x35) && (Donation > 0L))
                {
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Duke;
                }
                if (Donation >= 0xbebc200L)
                {
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Earl;
                }
                if (Donation >= 0x5f5e100L)
                {
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Baron;
                }
                if (Donation >= 0x1c9c380L)
                {
                    return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Knight;
                }
            }
            return PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks.Commoner;
        }
        public static void Handle(NobilityInfo information, Client.GameState client)
        {
            switch (information.Type)
            {
                   
                case NobilityInfo.Donate:
                    {

                        if (client.Trade.InTrade)
                            return;
                        if (client.Entity.MapID == 3031)
                            return;
                        uint silvers = information.dwParam;
                        bool newDonator = false;
                        if (client.NobilityInformation.Donation == 0)
                            newDonator = true;
                        if (client.Entity.Money < information.dwParam)
                        {
                            uint cps = silvers / 50000;
                            if (client.Entity.ConquerPoints >= cps)
                            {
                                if (client.Entity.NobalityDonation == client.NobilityInformation.Donation)
                                {
                                    client.Entity.ConquerPoints -= cps;
                                    Database.EntityTable.UpdateCps(client);
                                    client.NobilityInformation.Donation += silvers;
                                    client.Entity.NobalityDonation += silvers;
                                    Database.EntityTable.UpdateDonation(client);
                                }
                                else
                                {
                                    if (client.Entity.NobalityDonation > client.NobilityInformation.Donation)
                                    {
                                        client.Entity.ConquerPoints -= cps;
                                        Database.EntityTable.UpdateCps(client);
                                        client.Entity.NobalityDonation += silvers;
                                        client.NobilityInformation.Donation = client.Entity.NobalityDonation;
                                        Database.EntityTable.UpdateDonation(client);
                                    }
                                    else
                                    {
                                        if (client.Entity.NobalityDonation < client.NobilityInformation.Donation)
                                        {
                                            client.Entity.ConquerPoints -= cps;
                                            Database.EntityTable.UpdateCps(client);
                                            client.NobilityInformation.Donation += silvers;
                                            client.Entity.NobalityDonation = client.NobilityInformation.Donation;
                                            Database.EntityTable.UpdateDonation(client);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (client.Entity.Money >= silvers)
                            {
                                if (client.Entity.NobalityDonation == client.NobilityInformation.Donation)
                                {
                                    client.Entity.Money -= silvers;
                                    client.NobilityInformation.Donation += silvers;
                                    client.Entity.NobalityDonation += silvers;
                                    Database.EntityTable.UpdateDonation(client);
                                }
                                else
                                {
                                    if (client.Entity.NobalityDonation > client.NobilityInformation.Donation)
                                    {
                                        client.Entity.Money -= silvers;
                                        client.Entity.NobalityDonation += silvers;
                                        client.NobilityInformation.Donation = client.Entity.NobalityDonation;
                                        Database.EntityTable.UpdateDonation(client);
                                    }
                                    else
                                    {
                                        if (client.Entity.NobalityDonation < client.NobilityInformation.Donation)
                                        {
                                            client.Entity.Money -= silvers;
                                            client.NobilityInformation.Donation += silvers;
                                            client.Entity.NobalityDonation = client.NobilityInformation.Donation;
                                            Database.EntityTable.UpdateDonation(client);
                                        }
                                    }
                                }
                            }
                        }

                        if (!Board.ContainsKey(client.Entity.UID) && newDonator)
                        {
                            Board.Add(client.Entity.UID, client.NobilityInformation);
                            try
                            {
                                Database.NobilityTable.InsertNobilityInformation(client.NobilityInformation);
                            }
                            catch
                            {
                                Database.NobilityTable.UpdateNobilityInformation(client.NobilityInformation);
                            }
                        }
                        else
                        {
                            Database.NobilityTable.UpdateNobilityInformation(client.NobilityInformation);
                        }
                        Sort(client.Entity.UID);

                        Network.GamePackets.NpcReply npc = new Network.GamePackets.NpcReply(6, "You've donated " + silvers + " Silver. Your total donation is " + client.NobilityInformation.Donation + " silver. You rank at " + client.NobilityInformation.Position+ " place in the Donation Ranking.");
                                npc.OptionID = 255;
                                client.Send(npc.ToArray());
                                //return;

                        break;
                    }
                case NobilityInfo.List:
                    {
                        byte Count = 0;
                        MemoryStream strm = new MemoryStream();
                        BinaryWriter wtr = new BinaryWriter(strm);
                        wtr.Write((ushort)0);
                        wtr.Write((ushort)2064);
                        wtr.Write((uint)NobilityInfo.List);
                        wtr.Write((ushort)information.wParam1);
                        wtr.Write((ushort)10);
                        wtr.Write((uint)0);
                        wtr.Write((uint)0);
                        wtr.Write((uint)0);
                        wtr.Write((uint)0);
                        wtr.Write((uint)0);
                        wtr.Write((uint)0);
                       // wtr.Write((uint)0);
                        for (int c = 0; c < 20; c++)
                        {
                            if (BoardList[1].Name.Length > c)
                            {
                                wtr.Write((byte)(BoardList[1].Name[c]));
                            }
                            else
                            {
                                wtr.Write((byte)(0));
                            }
                        }
                        wtr.Write((ulong)BoardList[1].Donation);
                        wtr.Write((uint)BoardList[1].Rank);
                        wtr.Write((uint)BoardList[1].Position);

                        wtr.Write((uint)BoardList[1].EntityUID);
                        wtr.Write((uint)BoardList[1].Gender);
                        wtr.Write((uint)BoardList[1].Mesh);
                        for (int c = 0; c < 20; c++)
                        {
                            if (BoardList[1].Name.Length > c)
                            {
                                wtr.Write((byte)(BoardList[1].Name[c]));
                            }
                            else
                            {
                                wtr.Write((byte)(0));
                            }
                        }
                        wtr.Write((ulong)BoardList[1].Donation);
                        wtr.Write((uint)BoardList[1].Rank);
                        wtr.Write((uint)BoardList[1].Position);

                        for (int i = (int)(information.wParam1 * 10); i < information.wParam1 * 10 + 10; i++)
                        {
                            
                            if (BoardList.Count > i)
                            {
                                Count++;
                                wtr.Write((uint)BoardList[i].EntityUID);
                                wtr.Write((uint)BoardList[i].Gender);
                                wtr.Write((uint)BoardList[i].Mesh);
                                for (int c = 0; c < 20; c++)
                                {
                                    if (BoardList[i].Name.Length > c)
                                        wtr.Write((byte)(BoardList[i].Name[c]));
                                    else
                                        wtr.Write((byte)(0));
                                }
                                wtr.Write((ulong)BoardList[i].Donation);
                                wtr.Write((uint)BoardList[i].Rank);
                                wtr.Write((uint)BoardList[i].Position);
                            }
                        }
                        
                        int packetlength = (int)strm.Length;
                        strm.Position = 0;
                        wtr.Write((ushort)packetlength);
                        strm.Position = strm.Length;
                        wtr.Write(System.Text.Encoding.ASCII.GetBytes("TQServer"));
                        strm.Position = 0;
                        byte[] buf = new byte[strm.Length];
                        strm.Read(buf, 0, buf.Length);
                        Network.Writer.WriteUInt32(Count, 12, buf);
                        client.Send(buf);

                        information.Type = NobilityInfo.NextRank;
                        ulong value = 0;
                        information.dwParam2 = 0;
                        if (client.NobilityInformation.Rank == NobilityRank.Prince)
                            value = (ulong)(BoardList[02].Donation - client.NobilityInformation.Donation + 1);
                        if (client.NobilityInformation.Rank == NobilityRank.Duke)
                            value = (ulong)(BoardList[14].Donation - client.NobilityInformation.Donation + 1);
                        if (client.NobilityInformation.Rank == NobilityRank.Earl)
                            value = (ulong)(BoardList[49].Donation - client.NobilityInformation.Donation + 1);
                        Network.Writer.WriteUInt64(value, 8, information.ToArray());
                        information.dwParam3 = 50;
                        information.dwParam4 = uint.MaxValue;
                        client.Send(information);
                        break;
                    }
                case NobilityInfo.NextRank:
                    {
                        PhoenixProject.Network.GamePackets.nobility.NobilityIcon icon = new PhoenixProject.Network.GamePackets.nobility.NobilityIcon(0xa8)
                        {
                            Type = PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityTypes.Minimum
                        };
                        for (int i = 1; i < 12; i += 2)
                        {
                            if (i == 11)
                            {
                                i++;
                            }
                            PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks rank = (PhoenixProject.Network.GamePackets.nobility.NobilityIcon.NobilityRanks)i;
                            icon.AddMinimum(rank, Nobility.GetMinimumDonation(rank));
                        }
                        client.Send((byte[])icon);
                        break;
                    }

            }
        }

        public static void Sort(uint updateUID)
        {
            SortedDictionary<ulong, SortEntry<uint, NobilityInformation>> sortdict = new SortedDictionary<ulong, SortEntry<uint, NobilityInformation>>();

            foreach (NobilityInformation info in Board.Values)
            {
                if (sortdict.ContainsKey(info.Donation))
                {
                    SortEntry<uint, NobilityInformation> entry = sortdict[info.Donation];
                    entry.Values.Add(info.EntityUID, info);
                }
                else
                {
                    SortEntry<uint, NobilityInformation> entry = new SortEntry<uint, NobilityInformation>();
                    entry.Values = new Dictionary<uint, NobilityInformation>();
                    entry.Values.Add(info.EntityUID, info);
                    sortdict.Add(info.Donation, entry);
                }
            }

            SafeDictionary<uint, NobilityInformation> sortedBoard = new SafeDictionary<uint, NobilityInformation>(1000000);
            sortedBoard.Clear();
            int Place = 0;
            foreach (KeyValuePair<ulong, SortEntry<uint, NobilityInformation>> entries in sortdict.Reverse())
            {
                foreach (KeyValuePair<uint, NobilityInformation> value in entries.Value.Values)
                {
                    Client.GameState client = null;
                    try
                    {
                        int previousPlace = value.Value.Position;
                        value.Value.Position = Place;
                        NobilityRank Rank = NobilityRank.Serf;

                        if (Place > 100)
                        {
                            if (value.Value.Donation >= 200000000)
                            {
                                Rank = NobilityRank.Earl;
                            }
                            else if (value.Value.Donation >= 100000000)
                            {
                                Rank = NobilityRank.Baron;
                            }
                            else if (value.Value.Donation >= 30000000)
                            {
                                Rank = NobilityRank.Knight;
                            }
                        }
                        else
                        {
                            if (Place < PhoenixProject.Database.rates.king)
                            {
                                //PhoenixProject.ServerBase.Kernel.SendWorldMessage(new PhoenixProject.Network.GamePackets.Message("Congratulation! " + client.Entity.Name + "Donation To King in Nobility Rank!", System.Drawing.Color.White, 2011), PhoenixProject.ServerBase.Kernel.GamePool.Values);
                                Rank = NobilityRank.King;
                                //PhoenixProject.Clan.nobmas(client);
                                // ServerBase.Kernel.SendWorldMessage(new Message("Congratulation! " + client.Entity.Name + "Donation To King/Queen in Nobility Rank.", System.Drawing.Color.White, Message.Center), ServerBase.Kernel.GamePool.Values);
                                //Rank = NobilityRank.King;
                            }
                            else if (Place < PhoenixProject.Database.rates.prince)
                            {
                                Rank = NobilityRank.Prince;
                                //PhoenixProject.Clan.nobmas(client);
                                // ServerBase.Kernel.SendWorldMessage(new Message("Congratulation! " + client.Entity.Name + "Donation To Prince in Nobility Rank.", System.Drawing.Color.White, Message.Center), ServerBase.Kernel.GamePool.Values);
                                // Rank = NobilityRank.Prince;
                            }
                            else
                            {
                                Rank = NobilityRank.Duke;
                                //PhoenixProject.Clan.nobmas(client);
                                //ServerBase.Kernel.SendWorldMessage(new Message("Congratulation! " + client.Entity.Name + "Donation To Duke in Nobility Rank.", System.Drawing.Color.White, Message.Center), ServerBase.Kernel.GamePool.Values);
                                //Rank = NobilityRank.Duke;
                            }
                        }
                        var oldRank = value.Value.Rank;
                        value.Value.Rank = Rank;
                        if (ServerBase.Kernel.GamePool.TryGetValue(value.Key, out client))
                        {
                            bool updateTheClient = false;
                            if (oldRank != Rank)
                            {
                                updateTheClient = true;
                                if (Rank == NobilityRank.Baron)
                                {
                                    ServerBase.Kernel.SendWorldMessage(new Message("Congratulation! " + client.Entity.Name + " Donation To Baron in Nobility Rank.", System.Drawing.Color.White, Message.TopLeft), ServerBase.Kernel.GamePool.Values);
                                }
                                if (Rank == NobilityRank.Earl)
                                {
                                    ServerBase.Kernel.SendWorldMessage(new Message("Congratulation! " + client.Entity.Name + " Donation To Earl in Nobility Rank.", System.Drawing.Color.White, Message.TopLeft), ServerBase.Kernel.GamePool.Values);
                                }
                                if (Rank == NobilityRank.Duke)
                                {
                                    ServerBase.Kernel.SendWorldMessage(new Message("Congratulation! " + client.Entity.Name + " Donation To Duke in Nobility Rank.", System.Drawing.Color.White, Message.Center), ServerBase.Kernel.GamePool.Values);
                                }
                                if (Rank == NobilityRank.Prince)
                                {
                                    ServerBase.Kernel.SendWorldMessage(new Message("Congratulation! " + client.Entity.Name + " Donation To Prince in Nobility Rank.", System.Drawing.Color.White, Message.Center), ServerBase.Kernel.GamePool.Values);
                                }
                                if (Rank == NobilityRank.King)
                                {
                                    ServerBase.Kernel.SendWorldMessage(new Message("Congratulation! " + client.Entity.Name + " has become the new King/Queen in "+Database.rates.servername+"!", System.Drawing.Color.White, Message.Center), ServerBase.Kernel.GamePool.Values);
                                }
                                if (Rank == NobilityRank.Knight)
                                {
                                    ServerBase.Kernel.SendWorldMessage(new Message("Congratulation! " + client.Entity.Name + " Donation To Knight in Nobility Rank.", System.Drawing.Color.White, Message.TopLeft), ServerBase.Kernel.GamePool.Values);
                                }
                            }
                            else
                            {
                                if (previousPlace != Place)
                                {
                                    updateTheClient = true;
                                }
                            }
                            if (updateTheClient || client.Entity.UID == updateUID)
                            {
                                NobilityInfo update = new NobilityInfo(true);
                                update.Type = NobilityInfo.Icon;
                                update.dwParam = value.Key;
                                update.UpdateString(value.Value);
                                client.SendScreen(update, true);
                                client.Entity.NobilityRank = value.Value.Rank;
                            }
                        }
                        sortedBoard.Add(value.Key, value.Value);
                        Place++;
                    }
                    catch { }
                }

            }

            Board = sortedBoard;
         
            lock (BoardList)
            {
                BoardList = Board.Values.ToList();
            }
              
        }
    }
    public class NobilityInformation
    {
        public string Name;
        public uint EntityUID;
        public uint Mesh;
        public ulong Donation;
        public byte Gender;
        public int Position;
        public NobilityRank Rank;
    }

    public enum NobilityRank : byte
    {
        Serf = 0,
        Knight = 1,
        Baron = 3,
        Earl = 5,
        Duke = 7,
        Prince = 9,
        King = 12
    }
}
