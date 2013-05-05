namespace PhoenixProject.Network.GamePackets
{
    using PhoenixProject.Interfaces;
    using PhoenixProject.Network;
    using System;

    public class ItemView : Writer, IPacket
    {
        private byte[] Buffer;
        private Client.GameState client;

        public ItemView(Client.GameState _client)
        {
            this.client = _client;
        }

        public void Deserialize(byte[] buffer)
        {
            this.Buffer = buffer;
        }

        public void Send(Client.GameState client)
        {
            client.Send(this.ToArray());
        }

        public byte[] ToArray()
        {
            this.Buffer = new byte[0x54];
            Writer.WriteUInt16((ushort)(this.Buffer.Length - 8), 0, this.Buffer);
            Writer.WriteUInt16(0x3f1, 2, this.Buffer);
            Writer.WriteUInt32(0x2e, 12, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(1, this.client), 0x20, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(2, this.client), 0x24, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(3, this.client), 40, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(4, this.client), 0x2c, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(5, this.client), 0x30, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(6, this.client), 0x34, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(7, this.client), 0x38, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(8, this.client), 60, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(9, this.client), 0x40, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(10, this.client), 0x44, this.Buffer);
            Writer.WriteUInt32(this.client.Equipment.GetGear(11, this.client), 0x48, this.Buffer);
            return this.Buffer;
        }
    }
}

