using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Client;
using PhoenixProject.Interfaces;

namespace PhoenixProject.Network.GamePackets
{
    public class ClientEquip : Interfaces.IPacket
    {
        private Byte[] mData;

        public ClientEquip()
        {
            this.mData = new Byte[84 + 8];
            Writer.WriteUInt16((UInt16)(this.mData.Length - 8), 0, mData);
            Writer.WriteUInt16((UInt16)1009, 2, mData);
            KinSocket.PacketConstructor.Write(0, 8, mData);
            Writer.WriteUInt16((UInt16)0x2E, 12, mData);
        }

        public ClientEquip(GameState c)
        {
            this.mData = new Byte[84 + 8];
            Writer.WriteUInt16((UInt16)(this.mData.Length - 8), 0, mData);
            Writer.WriteUInt16((UInt16)1009, 2, mData);
            KinSocket.PacketConstructor.Write(Convert.ToByte(c.AlternateEquipment), 8, mData);
            Writer.WriteUInt16((UInt16)0x2E, 12, mData);

            DoEquips2(c);
        }

        public void DoEquips(GameState c)
        {
            if (c.Equipment == null) return;

            IConquerItem[] Items = c.Equipment.Objects;
            foreach (IConquerItem i in Items)
            {
                if (i == null) continue;
                switch (i.Position)
                {
                    case (UInt16)PacketHandler.Positions.Head: this.Helm = i.UID; break;
                    case (UInt16)PacketHandler.Positions.Necklace: this.Necklace = i.UID; break;
                    case (UInt16)PacketHandler.Positions.Armor: this.Armor = i.UID; break;
                    case (UInt16)PacketHandler.Positions.Right: this.RHand = i.UID; break;
                    case (UInt16)PacketHandler.Positions.Left: this.LHand = i.UID; break;
                    case (UInt16)PacketHandler.Positions.Ring: this.Ring = i.UID; break;
                    case (UInt16)PacketHandler.Positions.Bottle: this.Talisman = i.UID; break;
                    case (UInt16)PacketHandler.Positions.Boots: this.Boots = i.UID; break;
                    case (UInt16)PacketHandler.Positions.Garment: this.Garment = i.UID; break;
                    case (UInt16)PacketHandler.Positions.SteedArmor: this.SteedArmor = i.UID; break;
                    case (UInt16)PacketHandler.Positions.SteedTalisman: this.SteedTalisman = i.UID; break;
                    case (UInt16)PacketHandler.Positions.RightAccessory: this.RightAccessory = i.UID; break;
                    case (UInt16)PacketHandler.Positions.LeftAccessory: this.LeftAccessory = i.UID; break;
                }
            }
        }
        public void DoEquips2(GameState c)
        {
            if (c.Equipment == null) return;

            IConquerItem[] Items = c.Equipment.Objects;
            foreach (IConquerItem i in Items)
            {
                if (i == null) continue;
                switch (i.Position)
                {
                    case (UInt16)PacketHandler.Positions.AltHead: this.Helm = i.UID; break;
                    case (UInt16)PacketHandler.Positions.AltNecklace: this.Necklace = i.UID; break;
                    case (UInt16)PacketHandler.Positions.AltArmor: this.Armor = i.UID; break;
                    case (UInt16)PacketHandler.Positions.AltRightHand: this.RHand = i.UID; break;
                    case (UInt16)PacketHandler.Positions.AltLeftHand: this.LHand = i.UID; break;
                    case (UInt16)PacketHandler.Positions.AltRing: this.Ring = i.UID; break;
                    case (UInt16)PacketHandler.Positions.AltBottle: this.Talisman = i.UID; break;
                    case (UInt16)PacketHandler.Positions.AltBoots: this.Boots = i.UID; break;
                    case (UInt16)PacketHandler.Positions.AltGarment: this.Garment = i.UID; break;
                    case (UInt16)PacketHandler.Positions.SteedArmor: this.SteedArmor = i.UID; break;
                    case (UInt16)PacketHandler.Positions.SteedTalisman: this.SteedTalisman = i.UID; break;
                    case (UInt16)PacketHandler.Positions.RightAccessory: this.RightAccessory = i.UID; break;
                    case (UInt16)PacketHandler.Positions.LeftAccessory: this.LeftAccessory = i.UID; break;
                }
            }
        }

        public void Deserialize(byte[] buffer) { this.mData = buffer; }
        public byte[] ToArray()
        { return mData; }
        public void Send(Client.GameState client) { client.Send(mData); }

        public UInt32 Helm
        {
            get { return BitConverter.ToUInt32(this.mData, 32); }
            set { Writer.WriteUInt32(value, 32, mData); }
        }

        public UInt32 Necklace
        {
            get { return BitConverter.ToUInt32(this.mData, 36); }
            set { Writer.WriteUInt32(value, 36, mData); }
        }

        public UInt32 Armor
        {
            get { return BitConverter.ToUInt32(this.mData, 40); }
            set { Writer.WriteUInt32(value, 40, mData); }
        }

        public UInt32 RHand
        {
            get { return BitConverter.ToUInt32(this.mData, 44); }
            set { Writer.WriteUInt32(value, 44, mData); }
        }

        public UInt32 LHand
        {
            get { return BitConverter.ToUInt32(this.mData, 48); }
            set { Writer.WriteUInt32(value, 48, mData); }
        }

        public UInt32 Ring
        {
            get { return BitConverter.ToUInt32(this.mData, 52); }
            set { Writer.WriteUInt32(value, 52, mData); }
        }

        public UInt32 Talisman
        {
            get { return BitConverter.ToUInt32(this.mData, 56); }
            set { Writer.WriteUInt32(value, 56, mData); }
        }

        public UInt32 Boots
        {
            get { return BitConverter.ToUInt32(this.mData, 60); }
            set { Writer.WriteUInt32(value, 60, mData); }
        }

        public UInt32 Garment
        {
            get { return BitConverter.ToUInt32(this.mData, 64); }
            set { Writer.WriteUInt32(value, 64, mData); }
        }

        public UInt32 RightAccessory
        {
            get { return BitConverter.ToUInt32(this.mData, 68); }
            set { Writer.WriteUInt32(value, 68, mData); }
        }

        public UInt32 LeftAccessory
        {
            get { return BitConverter.ToUInt32(this.mData, 72); }
            set { Writer.WriteUInt32(value, 72, mData); }
        }

        public UInt32 SteedArmor
        {
            get { return BitConverter.ToUInt32(this.mData, 76); }
            set { Writer.WriteUInt32(value, 76, mData); }
        }

        public UInt32 SteedTalisman
        {
            get { return BitConverter.ToUInt32(this.mData, 80); }
            set { Writer.WriteUInt32(value, 80, mData); }
        }
    }
}

