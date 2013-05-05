using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Game.ConquerStructures.Society;
using PhoenixProject.Interfaces;
using PhoenixProject.Client;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Game.Features
{
    public class Arsenal_State
    {
        public Arsenal_ID ID;
        public bool Unlocked;
        public ulong Donation;
        public byte Potency;
        public Dictionary<uint, Arsenal_Client> Inscribed;
        public Arsenal_State()
        {
            Inscribed = new Dictionary<uint, Arsenal_Client>();
        }
    }

    public class Arsenal_Client
    {
        public IConquerItem Item;
        public string Name;
        public uint UID;
        public uint iUID;
    }

    public enum Arsenal_ID : byte
    {
        None = 0,
        Headgear = 8,
        Armor = 1,
        Weapon = 2,
        Ring = 3,
        Boots = 4,
        Necklace = 5,
        Fan = 6,
        Tower = 7,
    }
    public class Arsenal
    {
        ushort gOwner = 0;
        public Dictionary<Arsenal_ID, Arsenal_State> Arsenals = null;
        public byte BattlePower;

        public Arsenal(ushort g_owner)
        {
            gOwner = g_owner;
            Arsenals = new Dictionary<Arsenal_ID, Arsenal_State>();
        }

        public void InscribeItem(GameState Client, IConquerItem Item, Arsenal_ID ID)
        {
            Arsenal_State _Arsenal = null;
            if (Arsenals.TryGetValue(ID, out _Arsenal))
            {
                if (!_Arsenal.Inscribed.ContainsKey(Item.UID))
                {
                    Item.Inscribed = true;
                    Item.Mode = PhoenixProject.Game.Enums.ItemMode.Update;
                    Item.Send(Client);
                    Item.Mode = PhoenixProject.Game.Enums.ItemMode.Default;

                    lock (_Arsenal.Inscribed)
                        _Arsenal.Inscribed.Add(Item.UID, new Arsenal_Client() { iUID = Item.UID, Item = Item, UID = Client.Entity.UID, Name = Client.Entity.Name });
                    Arsenals[ID].Donation += GetDonation(Item);
                    Client.Arsenal_Donation += GetDonation(Item);
                    Database.ConquerItemTable.UpdateInscre1(Item);
                    Database.ArsenalTable.UpdateArsenal(Arsenals[ID].Donation, ID);
                    Database.ArsenalTable.CreateArsenalItem(Client.Entity.UID, Client.Entity.Name, Item.UID, Client.Entity.GuildID, ID);
                }
            }
        }

        public void RemoveItem(GameState Client, IConquerItem Item, Arsenal_ID ID)
        {
            Arsenal_State _Arsenal = null;
            if (Arsenals.TryGetValue(ID, out _Arsenal))
            {
                if (_Arsenal.Inscribed.ContainsKey(Item.UID))
                {
                    lock (_Arsenal.Inscribed)
                        _Arsenal.Inscribed.Remove(Item.UID);

                    Item.Inscribed = false;
                    Item.Mode = PhoenixProject.Game.Enums.ItemMode.Update;
                    Item.Send(Client);
                    Item.Mode = PhoenixProject.Game.Enums.ItemMode.Default;
                    Arsenals[ID].Donation -= GetDonation(Item);
                    Client.Arsenal_Donation -= GetDonation(Item);
                    Database.ArsenalTable.UpdateArsenal(Arsenals[ID].Donation, ID);
                    Database.ConquerItemTable.UpdateInscre2(Item);
                    Database.ArsenalTable.DeleteArsenalItem(Client.Entity.UID, Item.UID, ID);
                }
            }
        }
        public void Update(Guild Guild)
        {
            #region Potencia Arsenal
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Armor))
            {
                if (Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Donation >= 2000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Donation < 4000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Potency = 1;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Donation >= 4000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Donation < 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Potency = 2;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Donation >= 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Potency = 3;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Boots))
            {
                if (Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Donation >= 2000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Donation < 4000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Potency = 1;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Donation >= 4000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Donation < 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Potency = 2;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Donation >= 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Potency = 3;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Fan))
            {
                if (Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Donation >= 2000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Donation < 4000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Potency = 1;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Donation >= 4000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Donation < 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Potency = 2;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Donation >= 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Potency = 3;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Headgear))
            {
                if (Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Donation >= 2000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Donation < 4000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Potency = 1;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Donation >= 4000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Donation < 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Potency = 2;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Donation >= 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Potency = 3;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Necklace))
            {
                if (Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Donation >= 2000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Donation < 4000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Potency = 1;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Donation >= 4000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Donation < 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Potency = 2;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Donation >= 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Potency = 3;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Ring))
            {
                if (Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Donation >= 2000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Donation < 4000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Potency = 1;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Donation >= 4000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Donation < 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Potency = 2;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Donation >= 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Potency = 3;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Tower))
            {
                if (Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Donation >= 2000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Donation < 4000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Potency = 1;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Donation >= 4000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Donation < 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Potency = 2;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Donation >= 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Potency = 3;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Weapon))
            {
                if (Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Donation >= 2000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Donation < 4000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Potency = 1;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Donation >= 4000000 && Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Donation < 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Potency = 2;
                else if (Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Donation >= 10000000)
                    Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Potency = 3;
            }
            #endregion
            #region Potencia Guild
            Guild.Arsenal.BattlePower = 0;
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Armor))
            {
                Guild.Arsenal.BattlePower += Guild.Arsenal.Arsenals[Arsenal_ID.Armor].Potency;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Boots))
            {
                Guild.Arsenal.BattlePower += Guild.Arsenal.Arsenals[Arsenal_ID.Boots].Potency;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Fan))
            {
                Guild.Arsenal.BattlePower += Guild.Arsenal.Arsenals[Arsenal_ID.Fan].Potency;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Headgear))
            {
                Guild.Arsenal.BattlePower += Guild.Arsenal.Arsenals[Arsenal_ID.Headgear].Potency;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Necklace))
            {
                Guild.Arsenal.BattlePower += Guild.Arsenal.Arsenals[Arsenal_ID.Necklace].Potency;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Ring))
            {
                Guild.Arsenal.BattlePower += Guild.Arsenal.Arsenals[Arsenal_ID.Ring].Potency;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Tower))
            {
                Guild.Arsenal.BattlePower += Guild.Arsenal.Arsenals[Arsenal_ID.Tower].Potency;
            }
            if (Guild.Arsenal.Arsenals.ContainsKey(Arsenal_ID.Weapon))
            {
                Guild.Arsenal.BattlePower += Guild.Arsenal.Arsenals[Arsenal_ID.Weapon].Potency;
            }
            #endregion
        }
        public void UnlockArsenal(GameState Client, Arsenal_ID ID)
        {
            if (Client.AsMember.Rank != Enums.GuildMemberRank.GuildLeader)
                return;
            if (Arsenals.ContainsKey(ID))
                return;
            if (Client.Guild.SilverFund < getUnlockValue())
                return;
            if (ID == Arsenal_ID.None) ID = Arsenal_ID.Headgear;
            Client.Guild.SilverFund -= getUnlockValue();
            Arsenals.Add(ID, new Arsenal_State() { ID = ID, Unlocked = true });
            Client.Send(ArsenalPacket.GuildArsenal(Client));
            Database.ArsenalTable.CreateArsenal(Client.Guild.ID, ID);
            //Database.UpdateGuild(Syn);
        }

        public uint getUnlockValue()
        {
            switch (Arsenals.Count)
            {
                case 0:
                case 1: return 5000000;
                case 2:
                case 3:
                case 4: return 10000000;
                case 5:
                case 6: return 15000000;
                case 7: return 20000000;
            }

            return 0;
        }

        public uint GetDonation(IConquerItem Item)
        {
            uint Return = 0;
            int id = (int)(Item.ID % 10);
            switch (id)
            {
                case 8: Return = 1000; break;
                case 9: Return = 16660; break;
            }
            if (Item.SocketOne > 0 && Item.SocketTwo == 0)
                Return += 33330;
            if (Item.SocketOne > 0 && Item.SocketTwo > 0)
                Return += 133330;

            switch (Item.Plus)
            {
                case 1: Return += 90; break;
                case 2: Return += 490; break;
                case 3: Return += 1350; break;
                case 4: Return += 4070; break;
                case 5: Return += 12340; break;
                case 6: Return += 37030; break;
                case 7: Return += 111110; break;
                case 8: Return += 333330; break;
                case 9: Return += 1000000; break;
                case 10: Return += 1033330; break;
                case 11: Return += 1101230; break;
                case 12: Return += 1212340; break;
                default: break;
            }

            return Return;
        }
    }
}