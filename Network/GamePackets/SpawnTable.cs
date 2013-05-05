using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public unsafe class SpawnTable
    {
        public uint Id;
        public uint Unknown8;
        public uint Unknown12;
        public ushort PositionX;
        public ushort PositionY;
        public uint Lookface;
        public ushort Unknown24;
        public uint TableIndex;
        public PokerCurrency CurrencyType;
        public PokerBetType BetType;
        public uint BetMinimum;
        public TableState State;
        public long PotAmount;
        public byte PlayerAmount;
        public static SpawnTable Create(PhoenixProject.Generated.Interfaces.Table table)
        {
            var packet = new SpawnTable();
            packet.Id = table.UID;
            packet.PositionX = table.X;
            packet.PositionY = table.Y;
            packet.Lookface = table.Mesh;
            packet.CurrencyType = table.CurrencyType;
            packet.BetType = table.BetType;
            packet.BetMinimum = table.MinimumBet;
            packet.State = table.State;
            packet.PotAmount = table.CurrentPot;
            packet.TableIndex = (uint)(table.ID - 1);
            packet.PlayerAmount = (byte)(table.Users.Count);

            return packet;
        }
        public static implicit operator byte[](SpawnTable packet)
        {
            var buffer = new byte[52 + 8];
            fixed (byte* ptr = buffer)
            {
                //PacketBuilder.AppendHeader(ptr, buffer.Length, 2172);
                *((uint*)(ptr + 2)) = 2172;
                *((uint*)(ptr + 4)) = packet.Id;
                *((uint*)(ptr + 8)) = packet.Unknown8;
                *((uint*)(ptr + 12)) = packet.Unknown12;
                *((ushort*)(ptr + 16)) = packet.PositionX;
                *((ushort*)(ptr + 18)) = packet.PositionY;
                *((uint*)(ptr + 20)) = packet.Lookface;
                *((ushort*)(ptr + 24)) = packet.Unknown24;
                *((uint*)(ptr + 26)) = packet.TableIndex;
                *((uint*)(ptr + 30)) = (uint)packet.BetType;
                *((uint*)(ptr + 34)) = (uint)packet.CurrencyType;
                *((uint*)(ptr + 38)) = packet.BetMinimum;
                *((byte*)(ptr + 42)) = (byte)packet.State;
                *((long*)(ptr + 43)) = packet.PotAmount;
                *((byte*)(ptr + 51)) = (byte)packet.PlayerAmount;
            }
            return buffer;
        }

    }
}
