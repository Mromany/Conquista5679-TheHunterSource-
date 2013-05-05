using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Interfaces;
using PhoenixProject.Network.GamePackets;

namespace PhoenixProject.Generated.Interfaces
{
    public class Table : ILocatableObject
    {
        public Table()
        {
            Mesh = 0x6e232f;
            UID = 101941;
            ID = 10;
            CurrentPot = 0;
            MinimumBet = 10000;
        }
        public List<Client.GameState> Users = new List<Client.GameState>();
        public uint Mesh = 0x6e232f;
       // public IMapObject Map { get; set; }
        public uint ID { get; set; }
        public uint UID { get; set; }
        public ushort X { get; set; }
        public ushort Y { get; set; }
        public ushort MapID { get; set; }
        public uint CurrentPot, MinimumBet;
        public PokerBetType BetType = PokerBetType.FixedBet;
        public PokerCurrency CurrencyType = PokerCurrency.Gold;
        private TableState _state;
       // public static void SendSpawn(Client.GameState Client);
       // public static void SendSpawn(Client.GameState Client, bool checkScreen);
        public TableState State
        {
            get { return _state; }
            set
            {
                _state = value;

               
            }
        }
       
        public void ToPlayers(byte[] data)
        {
            lock (Users)
                foreach (Client.GameState p in Users)
                {
                    Data datas = new Data(true);
                    datas.UID = UID;
                    datas.ID = (uint)_state;
                    datas.dwParam = Data.TableState;
                    p.Send(datas);
                }
        }
        public void HandleAction(PokerAction receive, Client.GameState user)
        {

        }

        public Game.Enums.NpcType Type
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        uint ILocatableObject.Mesh
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public uint TableUID
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ushort BE
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public uint Other
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void SendSpawn(Client.GameState Client)
        {
            throw new NotImplementedException();
        }

        public void SendSpawn(Client.GameState Client, bool checkScreen)
        {
            throw new NotImplementedException();
        }
    }
}
