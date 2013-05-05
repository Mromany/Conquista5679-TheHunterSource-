using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class ChiStats : Writer, Interfaces.IPacket
    {

        byte[] mData;

        public ChiGate ChiGate
        {
            get
            {
                return (ChiGate)mData[22];
            }
            set
            {
                mData[22] = (byte)value;
            }
        }

        public uint ChiPoints
        {
            get
            {
                return BitConverter.ToUInt32(mData, 10);
            }
            set
            {
                WriteUInt32(value, 10, mData);
            }
        }

        public uint Identifier
        {
            get
            {
                return BitConverter.ToUInt32(mData, 6);
            }
            set
            {
                WriteUInt32(value, 6, mData);
            }
        }

        public ushort Unknown1
        {
            get
            {
                return BitConverter.ToUInt16(mData, 4);
            }
            set
            {
                WriteUInt16(value, 4, mData);
            }
        }

        public uint Unknown2
        {
            get
            {
                return BitConverter.ToUInt32(mData, 14);
            }
            set
            {
                WriteUInt32(value, 14, mData);
            }
        }

        public uint Unknown3
        {
            get
            {
                return BitConverter.ToUInt32(mData, 18);
            }
            set
            {
                WriteUInt32(value, 18, mData);
            }
        }

        public uint Val1
        {
            get
            {
                return BitConverter.ToUInt32(mData, 24);
            }
            set
            {
                WriteUInt32(value, 24, mData);
            }
        }

        public uint Val2
        {
            get
            {
                return BitConverter.ToUInt32(mData, 28);
            }
            set
            {
                WriteUInt32(value, 28, mData);
            }
        }

        public uint Val3
        {
            get
            {
                return BitConverter.ToUInt32(mData, 32);
            }
            set
            {
                WriteUInt32(value, 32, mData);
            }
        }

        public uint Val4
        {
            get
            {
                return BitConverter.ToUInt32(mData, 36);
            }
            set
            {
                WriteUInt32(value, 36, mData);
            }
        }

       /* public static implicit operator byte[](ChiStats d)
        {
            return d.mData;
        }*/

        public ChiStats()
        {
            mData = new byte[39 + 8];
            WriteUInt16(39, 0, mData);
            WriteUInt16(2534, 2, mData);
        }

        public static ChiStats ChiStats22(byte[] d)
        {
            var packet = new ChiStats();
            packet.mData = d;
            return packet;
        }
        public byte[] ToArray()
        {
            return mData;
        }

        public void Send(Client.GameState client)
        {
            client.Send(mData);
        }
        public void Deserialize(byte[] buffer)
        {
            mData = buffer;
        }
    } // class ChiStats
}
