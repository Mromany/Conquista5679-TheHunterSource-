using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Network.GamePackets
{
    public class PokerScreen
    {
        private byte[] mData;
        public enum PokerPlayerStatus : uint
        {
            inz = 2,
            Normal = 1,
            Offline = 0
        }
        public PokerScreen()
        {
            this.mData = new byte[20+8];
            PacketConstructor.Write((ushort)20, 0, this.mData);
            PacketConstructor.Write((ushort)2090, 2, this.mData);
        }

        public PokerScreen(byte[] d)
        {
            this.mData = new byte[d.Length];
            d.CopyTo(this.mData, 0);
        }

        public static implicit operator byte[](PokerScreen d)
        {
            return d.mData;
        }

        public uint Character
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

        public PokerPlayerStatus PlayerStatus
        {
            get
            {
                return (PokerPlayerStatus)BitConverter.ToUInt16(this.mData, 4);
            }
            set
            {
                PacketConstructor.Write((ushort)value, 4, this.mData);
            }
        }

        public uint Seat
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

        public uint Table
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
        public enum PokerTableState : byte
        {
            Closed = 0,
            Opened = 1
        }
        public PokerTableState TableState
        {
            get
            {
                return (PokerTableState)((byte)BitConverter.ToUInt16(this.mData, 6));
            }
            set
            {
                PacketConstructor.Write((ushort)value, 6, this.mData);
            }
        }
    }
    public class poker2171 : Writer, Interfaces.IPacket
    {
        public const uint NotSignedUp = 0,
                          WaitingForOpponent = 1,
                          WaitingInactive = 2;
        byte[] Buffer;
        public poker2171(bool Create)
        {
            Buffer = new byte[28];
            WriteUInt16(20, 0, Buffer);
            WriteUInt16(2171, 2, Buffer);
        }

        public uint show
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { WriteUInt32(value, 4, Buffer); }
        }

        public uint show1
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public uint show2
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }
        public uint show3
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { WriteUInt32(value, 16, Buffer); }
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
    public class poker2090 : Writer, Interfaces.IPacket
    {
        public const uint NotSignedUp = 0,
                          WaitingForOpponent = 1,
                          WaitingInactive = 2;
        byte[] Buffer;
        public poker2090(bool Create)
        {
            Buffer = new byte[28];
            WriteUInt16(20, 0, Buffer);
            WriteUInt16(2090, 2, Buffer);
        }

        public ushort show
        {
            get { return BitConverter.ToUInt16(Buffer, 4); }
            set { WriteUInt16(value, 4, Buffer); }
        }

        public ushort show1
        {
            get { return BitConverter.ToUInt16(Buffer, 6); }
            set { WriteUInt16(value, 6, Buffer); }
        }

        public uint show2
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public uint show3
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }

        public uint show4
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { WriteUInt32(value, 16, Buffer); }
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
    public class poker2091 : Writer, Interfaces.IPacket
    {
        public const uint NotSignedUp = 0,
                          WaitingForOpponent = 1,
                          WaitingInactive = 2;
        byte[] Buffer;
        public poker2091(bool Create)
        {
            Buffer = new byte[52];
            WriteUInt16(44, 0, Buffer);
            WriteUInt16(2091, 2, Buffer);
        }

        public ushort show
        {
            get { return BitConverter.ToUInt16(Buffer, 4); }
            set { WriteUInt16(value, 4, Buffer); }
        }

        public ushort show1
        {
            get { return BitConverter.ToUInt16(Buffer, 6); }
            set { WriteUInt16(value, 6, Buffer); }
        }

        public uint show2
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public uint show3
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }

        public uint show4
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { WriteUInt32(value, 16, Buffer); }
        }
        public uint show5
        {
            get { return BitConverter.ToUInt32(Buffer, 20); }
            set { WriteUInt32(value, 20, Buffer); }
        }
        public uint show6
        {
            get { return BitConverter.ToUInt32(Buffer, 24); }
            set { WriteUInt32(value, 24, Buffer); }
        }
        public uint show7
        {
            get { return BitConverter.ToUInt32(Buffer, 26); }
            set { WriteUInt32(value, 26, Buffer); }
        }
        public uint show8
        {
            get { return BitConverter.ToUInt32(Buffer, 30); }
            set { WriteUInt32(value, 30, Buffer); }
        }
        public uint show9
        {
            get { return BitConverter.ToUInt32(Buffer, 34); }
            set { WriteUInt32(value, 34, Buffer); }
        }
        public uint show10
        {
            get { return BitConverter.ToUInt32(Buffer, 38); }
            set { WriteUInt32(value, 38, Buffer); }
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
    public class poker2092 : Writer, Interfaces.IPacket
    {
        public const uint NotSignedUp = 0,
                          WaitingForOpponent = 1,
                          WaitingInactive = 2;
        byte[] Buffer;
        public poker2092(bool Create)
        {
            Buffer = new byte[28];
            WriteUInt16(20, 0, Buffer);
            WriteUInt16(2092, 2, Buffer);
        }

        public ushort show
        {
            get { return BitConverter.ToUInt16(Buffer, 4); }
            set { WriteUInt16(value, 4, Buffer); }
        }

        public ushort show1
        {
            get { return BitConverter.ToUInt16(Buffer, 6); }
            set { WriteUInt16(value, 6, Buffer); }
        }

        public uint show2
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public uint show3
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }

        public uint show4
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { WriteUInt32(value, 16, Buffer); }
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
    public class poker2093 : Writer, Interfaces.IPacket
    {
        public const uint NotSignedUp = 0,
                          WaitingForOpponent = 1,
                          WaitingInactive = 2;
        byte[] Buffer;
        public poker2093(bool Create)
        {
            Buffer = new byte[28];
            WriteUInt16(20, 0, Buffer);
            WriteUInt16(2093, 2, Buffer);
        }

        public ushort show
        {
            get { return BitConverter.ToUInt16(Buffer, 4); }
            set { WriteUInt16(value, 4, Buffer); }
        }

        public ushort show1
        {
            get { return BitConverter.ToUInt16(Buffer, 6); }
            set { WriteUInt16(value, 6, Buffer); }
        }

        public uint show2
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public uint show3
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }

        public uint show4
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { WriteUInt32(value, 16, Buffer); }
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
    public class poker2094 : Writer, Interfaces.IPacket
    {
        public const uint NotSignedUp = 0,
                          WaitingForOpponent = 1,
                          WaitingInactive = 2;
        byte[] Buffer;
        public poker2094(bool Create)
        {
            Buffer = new byte[40];
            WriteUInt16(32, 0, Buffer);
            WriteUInt16(2094, 2, Buffer);
        }
        public ushort show
        {
            get { return BitConverter.ToUInt16(Buffer, 4); }
            set { WriteUInt16(value, 4, Buffer); }
        }

        public ushort show1
        {
            get { return BitConverter.ToUInt16(Buffer, 6); }
            set { WriteUInt16(value, 6, Buffer); }
        }

        public uint show2
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public uint show3
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }

        public uint show4
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { WriteUInt32(value, 16, Buffer); }
        }
        public uint show5
        {
            get { return BitConverter.ToUInt32(Buffer, 20); }
            set { WriteUInt32(value, 20, Buffer); }
        }
        public uint show6
        {
            get { return BitConverter.ToUInt32(Buffer, 24); }
            set { WriteUInt32(value, 24, Buffer); }
        }
        public uint show7
        {
            get { return BitConverter.ToUInt32(Buffer, 26); }
            set { WriteUInt32(value, 26, Buffer); }
        }
        public uint show8
        {
            get { return BitConverter.ToUInt32(Buffer, 30); }
            set { WriteUInt32(value, 30, Buffer); }
        }
        public uint show9
        {
            get { return BitConverter.ToUInt32(Buffer, 34); }
            set { WriteUInt32(value, 34, Buffer); }
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
    public class poker2095 : Writer, Interfaces.IPacket
    {
        public const uint NotSignedUp = 0,
                          WaitingForOpponent = 1,
                          WaitingInactive = 2;
        byte[] Buffer;
        public poker2095(bool Create)
        {
            Buffer = new byte[46];
            WriteUInt16(38, 0, Buffer);
            WriteUInt16(2095, 2, Buffer);
        }

        public ushort show
        {
            get { return BitConverter.ToUInt16(Buffer, 4); }
            set { WriteUInt16(value, 4, Buffer); }
        }

        public ushort show1
        {
            get { return BitConverter.ToUInt16(Buffer, 6); }
            set { WriteUInt16(value, 6, Buffer); }
        }

        public uint show2
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public uint show3
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }

        public uint show4
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { WriteUInt32(value, 16, Buffer); }
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
    public class poker2099 : Writer, Interfaces.IPacket
    {
        public const uint NotSignedUp = 0,
                          WaitingForOpponent = 1,
                          WaitingInactive = 2;
        byte[] Buffer;
        public poker2099(bool Create)
        {
            Buffer = new byte[20];
            WriteUInt16(12, 0, Buffer);
            WriteUInt16(2099, 2, Buffer);
        }
        public ushort show
        {
            get { return BitConverter.ToUInt16(Buffer, 4); }
            set { WriteUInt16(value, 4, Buffer); }
        }

        public ushort show1
        {
            get { return BitConverter.ToUInt16(Buffer, 6); }
            set { WriteUInt16(value, 6, Buffer); }
        }

        public uint show2
        {
            get { return BitConverter.ToUInt32(Buffer, 8); }
            set { WriteUInt32(value, 8, Buffer); }
        }

        public uint show3
        {
            get { return BitConverter.ToUInt32(Buffer, 12); }
            set { WriteUInt32(value, 12, Buffer); }
        }

        public uint show4
        {
            get { return BitConverter.ToUInt32(Buffer, 16); }
            set { WriteUInt32(value, 16, Buffer); }
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
    }
}
