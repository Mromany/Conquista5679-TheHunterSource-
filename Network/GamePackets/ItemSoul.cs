using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class Game_ItemSoul : Interfaces.IPacket
    {
        private Byte[] mData;

        public Game_ItemSoul()
        {
            this.mData = new Byte[36 + 8];
            Writer.WriteUInt16((UInt16)(this.mData.Length - 8), 0, mData);
            Writer.WriteUInt16((UInt16)2077, 2, mData);
        }

        public UInt32 Type
        {
            get { return BitConverter.ToUInt32(mData, 4); }
            set { Writer.WriteUInt32(value, 4, mData); }
        }

        public UInt32 Identifier
        {
            get { return BitConverter.ToUInt32(mData, 8); }
            set { Writer.WriteUInt32(value, 8, mData); }
        }

        public Types Mode
        {
            get { return (Types)mData[12]; }
            set { mData[12] = (Byte)value; }
        }

        public UInt32 ID
        {
            get { return BitConverter.ToUInt32(mData, 16); }
            set { Writer.WriteUInt32(value, 16, mData); }
        }

        public UInt32 Level
        {
            get { return BitConverter.ToUInt32(mData, 20); }
            set { Writer.WriteUInt32(value, 20, mData); }
        }
        public UInt32 Percent
        {
            get { return BitConverter.ToUInt32(mData, 24); }
            set { Writer.WriteUInt32(value, 24, mData); }
        }
        public UInt32 Time
        {
            get { return BitConverter.ToUInt32(mData, 28); }
            set { Writer.WriteUInt32(value, 28, mData); }
        }
        public void Expired(Client.GameState c)
        {
            mData[12] = 7;
            c.Send(this);
        }

        public Byte[] Serialize { get { return this.mData; } }
        public void Deserialize(Byte[] d)
        {
            this.mData = new Byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public enum Types : byte
        {
            Refine = 0,
            DragonSoul = 5
        }
        public byte[] ToArray()
        {
            return mData;
        }

        public void Send(Client.GameState client)
        {
            client.Send(mData);
        }
    }
}