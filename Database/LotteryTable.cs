﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PhoenixProject.Database
{
    public class LotteryTable
    {
        public struct LotteryItem
        {
            public int Rank, Chance;
            public string Name;
            public uint ID;
            public byte Color;
            public byte Sockets;
            public byte Plus;
        }
        public static List<LotteryItem> LotteryItems = new List<LotteryItem>();
        public static void Load()
        {
            MySqlCommand command = new MySqlCommand(MySqlCommandType.SELECT);
            command.Select("lottery");
            MySqlReader reader = new MySqlReader(command);
            while (reader.Read())
            {
                LotteryItem item = new LotteryItem();
                item.Rank = reader.ReadInt32("rank");
                item.Chance = reader.ReadInt32("chance");
                item.Name = reader.ReadString("prize_name");
                item.ID = reader.ReadUInt32("prize_item");
                item.Color = reader.ReadByte("color");
                item.Sockets = reader.ReadByte("hole_num");
                item.Plus = reader.ReadByte("addition_lev");
                LotteryItems.Add(item);
            }
            reader.Close();
            reader.Dispose();
            Console.WriteLine("Lottery items loaded.");
        }
    }
}
