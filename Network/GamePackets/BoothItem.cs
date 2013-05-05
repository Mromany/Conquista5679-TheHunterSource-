using System;
using System.Drawing;
using PhoenixProject.Game;
namespace PhoenixProject.Network.GamePackets
{
    public class BoothItem2 : Interfaces.IPacket
    {
        private Byte[] mData;

        public void ParseItem(Interfaces.IConquerItem i)
        {
            ItemID = i.ID;
            ItemIdentifier = i.UID;
            Durability = i.Durability;
            MaxDurability = i.MaximDurability;
            SocketProgress = i.SocketProgress;
            SocketOne = (Byte)i.SocketOne;
            SocketTwo = (Byte)i.SocketTwo;
            Effect = (UInt16)i.Effect;
            Plus = i.Plus;
            Bless = i.Bless;
            Bound = i.Bound;
            Enchant = i.Enchant;
            Suspicious = i.Suspicious;
            Lock = Convert.ToBoolean(i.Lock);
            Color = (UInt32)i.Color;
            PlusProgress = i.PlusProgress;
            StackSize = i.StackSize;
        }

        public BoothItem2()
        {
            mData = new Byte[84 + 8];
            Writer.WriteUInt16(((UInt16)(mData.Length - 8)), 0, mData);
            Writer.WriteUInt16((UInt16)1108, 2, mData);
        }
        public UInt32 ItemIdentifier
        {
            get { return BitConverter.ToUInt32(mData, 4); }
            set { Writer.WriteUInt32(value, 4, mData); }
        }
        public UInt32 Identifier
        {
            get { return BitConverter.ToUInt32(mData, 8); }
            set { Writer.WriteUInt32(value, 8, mData); }
        }
        public UInt32 Cost
        {
            get { return BitConverter.ToUInt32(mData, 12); }
            set { Writer.WriteUInt32(value, 12, mData); }
        }
        public UInt32 ItemID
        {
            get { return BitConverter.ToUInt32(mData, 16); }
            set { Writer.WriteUInt32(value, 16, mData); }
        }
        public UInt16 Durability
        {
            get { return BitConverter.ToUInt16(mData, 20); }
            set { Writer.WriteUInt16(value, 20, mData); }
        }
        public UInt16 MaxDurability
        {
            get { return BitConverter.ToUInt16(mData, 22); }
            set { Writer.WriteUInt16(value, 22, mData); }
        }
        public CostTypes CostType
        {
            get { return (CostTypes)BitConverter.ToUInt16(mData, 24); }
            set { Writer.WriteUInt16((UInt16)value, 24, mData); }
        }
        public PacketHandler.Positions Position
        {
            get { return (PacketHandler.Positions)mData[26]; }
            set { mData[26] = (Byte)value; }
        }
        public UInt32 SocketProgress
        {
            get { return BitConverter.ToUInt32(mData, 28); }
            set { Writer.WriteUInt32(value, 28, mData); }
        }
        public Byte SocketOne
        {
            get { return mData[32]; }
            set { mData[32] = value; }
        }
        public Byte SocketTwo
        {
            get { return mData[33]; }
            set { mData[33] = value; }
        }
        public UInt16 Effect
        {
            get { return BitConverter.ToUInt16(mData, 36); }
            set { Writer.WriteUInt16((UInt16)value, 36, mData); }
        }
        public Byte Plus
        {
            get { return mData[41]; }
            set { mData[41] = value; }
        }
        public Byte Bless
        {
            get { return mData[42]; }
            set { mData[42] = value; }
        }
        public Boolean Bound
        {
            get { return mData[43] == 0 ? false : true; }
            set { mData[43] = (Byte)(value ? 1 : 0); }
        }
        public Byte Enchant
        {
            get { return mData[44]; }
            set { mData[44] = value; }
        }
        public Boolean Suspicious
        {
            get { return mData[53] == 0 ? false : true; }
            set { mData[53] = (Byte)(value ? 1 : 0); }
        }
        public Boolean Lock
        {
            get { return mData[54] == 0 ? false : true; }
            set { mData[54] = (Byte)(value ? 1 : 0); }
        }
        public UInt32 Color
        {
            get { return BitConverter.ToUInt32(mData, 56); }
            set { Writer.WriteUInt32((UInt32)value, 56, mData); }
        }
        public UInt32 PlusProgress
        {
            get { return BitConverter.ToUInt32(mData, 60); }
            set { Writer.WriteUInt32(value, 60, mData); }
        }
        public UInt16 StackSize
        {
            get { return BitConverter.ToUInt16(mData, 72); }
            set { Writer.WriteUInt16(value, 72, mData); }
        }
        public UInt32 PurificationID
        {
            get { return BitConverter.ToUInt32(mData, 76); }
            set { Writer.WriteUInt32(value, 76, mData); }
        }

        public byte[] ToArray()
        {
            return mData;
        }

        public void Deserialize(byte[] buffer)
        {
            mData = buffer;
        }

        public void Send(Client.GameState client)
        {
            client.Send(mData);
        }


        public enum CostTypes : ushort
        {
            Gold = 1,
            CPs = 3,
            ViewEquip = 4
        }
    }
    public class BoothItem : Writer, Interfaces.IPacket
    {
        byte[] Buffer;

        public BoothItem(bool Create)
        {
            if (Create)
            {
                Buffer = new byte[92];
                WriteUInt16(84, 0, Buffer);
                WriteUInt16(1108, 2, Buffer);
            }
        }
        public uint UID
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { WriteUInt32(value, 4, Buffer); }
        }
        public uint BoothID
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }
        public uint Cost
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }
        public uint ID
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { WriteUInt32(value, 16, Buffer); }
        }
        public ushort Durability
        {
            get { return BitConverter.ToUInt16(Buffer, 20); }
            set { WriteUInt16(value, 20, Buffer); }
        }
        public ushort MaximDurability
        {
            get { return BitConverter.ToUInt16(Buffer, 22); }
            set { WriteUInt16(value, 22, Buffer); }
        }
        public uint CostType
        {
            get { return BitConverter.ToUInt32(Buffer, 24); }
            set { WriteUInt32(value, 24, Buffer); }
        }
        public uint SocketProgress
        {
            get { return BitConverter.ToUInt32(Buffer, 28); }
            set { WriteUInt32(value, 28, Buffer); }
        }
        public Enums.Gem SocketOne
        {
            get { return (Enums.Gem)Buffer[32]; }
            set { Buffer[32] = (byte)value; }
        }
        public Enums.Gem SocketTwo
        {
            get { return (Enums.Gem)Buffer[33]; }
            set { Buffer[33] = (byte)value; }
        }
        public Enums.ItemEffect Effect
        {
            get { return (Enums.ItemEffect)BitConverter.ToUInt16(Buffer, 34); }
            set { WriteUInt16((ushort)value, 36, Buffer); }
        }
        public byte Plus
        {
            get { return Buffer[41]; }
            set { Buffer[41] = value; }
        }
        public byte Bless
        {
            get { return Buffer[42]; }
            set { Buffer[42] = value; }
        }
        public bool Bound
        {
            get { return Buffer[43] == 0 ? false : true; }
            set { Buffer[43] = (byte)(value ? 1 : 0); }
        }
        public byte Enchant
        {
            get { return Buffer[44]; }
            set { Buffer[44] = value; }
        }
        public bool Suspicious
        {
            get { return Buffer[53] == 0 ? false : true; }
            set { Buffer[53] = (byte)(value ? 1 : 0); }
        }
        public byte Lock
        {
            get { return Buffer[54]; }
            set { Buffer[54] = value; }
        }
        public Enums.Color Color
        {
            get { return (Enums.Color)BitConverter.ToUInt32(Buffer, 56); }
            set { WriteUInt32((uint)value, 56, Buffer); }
        }
        public uint PlusProgress
        {
            get { return BitConverter.ToUInt32(Buffer, 60); }
            set { WriteUInt32(value, 60, Buffer); }
        }
        public bool Inscribed
        {
            get
            {
                return (BitConverter.ToUInt16(this.Buffer, 64) == 1);
            }
            set
            {
                Writer.WriteUInt16(value ? ((byte)1) : ((byte)0), 64, this.Buffer);
            }
        }
        public ushort StackSize
        {
            get { return BitConverter.ToUInt16(Buffer, 72); }
            set { WriteUInt16(value, 72, Buffer); }
        }
        public uint PurificationID
        {
            get { return BitConverter.ToUInt32(Buffer, 76); }
            set { WriteUInt32(value, 76, Buffer); }
        }
        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }

        public void Send(Client.GameState client)
        {
            client.Send(Buffer);
        }

        public override int GetHashCode()
        {
            return (int)this.UID;
        }

        public void Fill(Game.ConquerStructures.BoothItem item, uint boothID)
        {
            UID = item.Item.UID;
            BoothID = boothID;
            Cost = item.Cost;
            ID = item.Item.ID;
            Durability = item.Item.Durability;
            MaximDurability = item.Item.MaximDurability;
            CostType = (byte)item.Cost_Type;
            SocketOne = item.Item.SocketOne;
            SocketTwo = item.Item.SocketTwo;
            Effect = item.Item.Effect;
            Bound = item.Item.Bound;
            Plus = item.Item.Plus;
            Bless = item.Item.Bless;
            Enchant = item.Item.Enchant;
            SocketProgress = item.Item.SocketProgress;
            Color = item.Item.Color;
            PlusProgress = item.Item.PlusProgress;
            StackSize = item.Item.StackSize;
            Inscribed = item.Item.Inscribed;
            PurificationID = item.Item.Purification.PurificationItemID;
        }
        public void Fill(ConquerItem item, uint boothID)
        {
            UID = item.UID;
            BoothID = boothID;
            ID = item.ID;
            Durability = item.Durability;
            MaximDurability = item.MaximDurability;
            Buffer[24] = (byte)4;
            Buffer[26] = (byte)item.Position;
            SocketOne = item.SocketOne;
            SocketTwo = item.SocketTwo;
            Effect = item.Effect;
            Plus = item.Plus;
            Bound = item.Bound;
            Bless = item.Bless;
            Enchant = item.Enchant;
            SocketProgress = item.SocketProgress;
            Color = item.Color;
            PlusProgress = item.PlusProgress;
            StackSize = item.StackSize;
            Inscribed = item.Inscribed;
            PurificationID = item.Purification.PurificationItemID;
        }
    }
}
