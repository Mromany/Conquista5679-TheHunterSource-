namespace PhoenixProject.Network.GamePackets
{
    using PhoenixProject.Game.Features.Flowers;
    using PhoenixProject.Interfaces;
    using PhoenixProject.Network;
    using System;

    public class FlowerPacket3 : Writer, IPacket
    {
        private byte[] Buffer;
        public const ushort LoveLetters = 1;
        public const ushort TinOfBeer = 2;
        public const ushort Kisses = 0;
        public const ushort Jades = 3;

        public FlowerPacket3(Client.GameState client)//MaleRank
        {
            Buffer = new byte[68];
            Writer.WriteUInt16(60, 0, Buffer);
            Writer.WriteUInt16(1150, 2, Buffer);
            Writer.WriteUInt32(3, 4, Buffer);
            Writer.WriteUInt32(client.Entity.Flowers.RedRoses, 16, Buffer);
            Writer.WriteUInt32(client.Entity.Flowers.RedRoses2day, 20, Buffer);
            Writer.WriteUInt32(client.Entity.Flowers.Lilies, 24, Buffer);
            Writer.WriteUInt32(client.Entity.Flowers.Lilies2day, 28, Buffer);
            Writer.WriteUInt32(client.Entity.Flowers.Orchads, 32, Buffer);
            Writer.WriteUInt32(client.Entity.Flowers.Orchads2day, 36, Buffer);
            Writer.WriteUInt32(client.Entity.Flowers.Tulips, 40, Buffer);
            Writer.WriteUInt32(client.Entity.Flowers.Tulips2day, 44, Buffer);

        }

        public FlowerPacket3(bool Create)
        {
            if (Create)
            {
                Buffer = new byte[68];
                Writer.WriteUInt16(60, 0, Buffer);
                Writer.WriteUInt16(1150, 2, Buffer);
            }
        }

        public void Deserialize(byte[] buffer)
        {
            this.Buffer = buffer;
        }

        public void Send(Client.GameState Client)
        {
            Client.Send(Buffer);
        }

        public byte[] ToArray()
        {
            return this.Buffer;
        }

        public uint Amount
        {
            get
            {
                return BitConverter.ToUInt32(Buffer, 20);
            }
            set
            {
                Writer.WriteUInt32(value, 20, Buffer);
            }
        }

        public PhoenixProject.Game.Features.Flowers.FlowerType FlowerType
        {
            get
            {
                return (PhoenixProject.Game.Features.Flowers.FlowerType)BitConverter.ToUInt32(Buffer, 24);
            }
        }

        public uint ItemUID
        {
            get
            {
                return BitConverter.ToUInt32(Buffer, 12);
            }
            set
            {
                Writer.WriteUInt32(value, 12, Buffer);
            }
        }

        public string ReceiverName
        {
            get
            {
                return BitConverter.ToString(Buffer, 32, 16);
            }
            set
            {
                Writer.WriteString(value, 32, Buffer);
            }
        }

        public uint SendAmount
        {
            get
            {
                return BitConverter.ToUInt32(Buffer, 48);
            }
            set
            {
                Writer.WriteUInt32(value, 48, Buffer);
            }
        }

        public string SenderName
        {
            get
            {
                return BitConverter.ToString(Buffer, 16, 16);
            }
            set
            {
                Writer.WriteString(value, 16, Buffer);
            }
        }

        public PhoenixProject.Game.Features.Flowers.FlowerType SendFlowerType
        {
            get
            {
                return (PhoenixProject.Game.Features.Flowers.FlowerType)BitConverter.ToUInt32(Buffer, 52);
            }
            set
            {
                Writer.WriteUInt32((uint)value, 52, Buffer);
            }
        }

        public uint UID1
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public uint UID2
        {
            get { return BitConverter.ToUInt32(Buffer, 10); }
            set { WriteUInt32(value, 10, Buffer); }
        }
    }
}

