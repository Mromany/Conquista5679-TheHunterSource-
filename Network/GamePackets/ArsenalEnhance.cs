using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class ArsenalEnhance : Writer, PhoenixProject.Interfaces.IPacket
    {
        private byte[] Buffer;
        public const byte Enhance = 3;
        public const byte MainInfo = 4;

        public ArsenalEnhance(bool Create)
        {
            if (Create)
            {
                this.Buffer = new byte[0x24];
                Writer.WriteUInt16((ushort)(this.Buffer.Length - 8), 0, this.Buffer);
                Writer.WriteUInt16(0x89b, 2, this.Buffer);
            }
        }

        public void Deserialize(byte[] buffer)
        {
            this.Buffer = buffer;
        }

        public void Send(Client.GameState client)
        {
            client.Send(this.Buffer);
        }

        public byte[] ToArray()
        {
            return this.Buffer;
        }

        public byte A_TypeTotal_Enhance
        {
            get
            {
                return this.Buffer[0x10];
            }
            set
            {
                this.Buffer[0x10] = value;
            }
        }

        public byte Arsenal_Type
        {
            get
            {
                return this.Buffer[8];
            }
            set
            {
                this.Buffer[8] = value;
            }
        }

        public byte Type
        {
            get
            {
                return this.Buffer[4];
            }
            set
            {
                this.Buffer[4] = value;
            }
        }
    }
}
