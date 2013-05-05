using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets
{
    public enum PokerActionType : uint
    {
        JoinTable = 0,
        LeaveTable = 1,
        Watch = 4,
    }
    public class PokerAction
    {
        public PokerActionType ActionType;
        private byte[] mData;

        public PokerAction()
        {
            this.mData = new byte[20+8];
            PacketConstructor.Write(20, 0, this.mData);
            PacketConstructor.Write((ushort)0x87b, 2, this.mData);
        }

        public PokerAction(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public static implicit operator byte[](PokerAction d)
        {
            return d.mData;
        }

        public uint Character
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 8);
            }
            set
            {
                PacketConstructor.Write(value, 8, this.mData);
            }
        }

        public uint Seat
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 0x10);
            }
            set
            {
                PacketConstructor.Write(value, 0x10, this.mData);
            }
        }

        public uint Target
        {
            get
            {
                return BitConverter.ToUInt32(this.mData, 12);
            }
            set
            {
                PacketConstructor.Write(value, 12, this.mData);
            }
        }

        public PokerActionType Type
        {
            get
            {
                return (PokerActionType)BitConverter.ToUInt32(this.mData, 4);
            }
            set
            {
                PacketConstructor.Write((uint)value, 4, this.mData);
            }
        }

    }
}
