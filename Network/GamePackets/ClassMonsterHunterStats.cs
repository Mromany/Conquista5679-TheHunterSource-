using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Database;

namespace PhoenixProject.Network.GamePackets
{
    public class MonsterHunterStats
    {
        public static ushort GetKillAmount(int targetid)
        {
            switch (targetid)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    return 50;
            }
            return 100;
        }
        public byte CompletedTimes = 0;
        private ushort mKills = 0;
        public Client.GameState Owner;
        public uint QuestGiver = 0;

        public MonsterHunterStats(Client.GameState h)
        {
            this.Owner = h;
        }

        public void Load(Client.GameState h)
        {
            MySqlCommand Command = new MySqlCommand(MySqlCommandType.SELECT);
            Command.Select("heroquests").Where("hero", h.Entity.UID).And("quest", 0xb71b0);
            PhoenixProject.Database.MySqlReader cmd2 = new PhoenixProject.Database.MySqlReader(Command);
            
            if (cmd2.Read())
            {
                this.QuestGiver = cmd2.ReadUInt32("npc");
                this.Kills = cmd2.ReadUInt16("step");
                this.CompletedTimes = cmd2.ReadByte("dailyfinishes");
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand(MySqlCommandType.INSERT);
                cmd.Insert("heroquests").Insert("hero", h.Entity.UID).Insert("quest", 0xb71b0).Execute();

            }
            
        }

        public void Save(Client.GameState h)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd3.Update("heroquests").Set("step", h.MonsterHunterStats.Kills).Set("dailyfinishes", h.MonsterHunterStats.CompletedTimes).Set("npc", h.MonsterHunterStats.QuestGiver).Where("hero", h.Entity.UID).And("quest", 0xb71b0).Execute();
           
        }

        public void UpdateKills(Client.GameState h)
        {
            MySqlCommand cmd3 = new MySqlCommand(MySqlCommandType.UPDATE);
            cmd3.Update("heroquests").Set("step", h.MonsterHunterStats.Kills).Where("hero", h.Entity.UID).And("quest", 0xb71b0).Execute();
            
        }

        public Interfaces.IConquerItem Jar
        {
            get
            {
                Interfaces.IConquerItem item;
                this.Owner.Inventory.TryGetItem(0xb71b0, out item);
                return item;
            }
        }

        public ushort Kills
        {
            get
            {
                return this.mKills;
            }
            set
            {
                this.mKills = value;
            }
        }

        public ushort RequiredKills
        {
            get
            {
                if (this.Jar != null)
                {
                    return this.Jar.Durability;
                }
                return 0;
            }
        }

        public ushort Target
        {
            get
            {
                if (this.Jar != null)
                {
                    return this.Jar.MaximDurability;
                }
                return 0;
            }
        }
    }
}
