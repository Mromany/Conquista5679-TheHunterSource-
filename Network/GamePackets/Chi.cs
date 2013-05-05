using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class ChiSystem : Writer, Interfaces.IPacket
    {

        byte[] mData;

        public ChiSystem()
        {
           
                mData = new byte[16];
                WriteUInt16((ushort)mData.Length, 0, mData);
                WriteUInt16(2533, 2, mData);
           
        }

        public uint Identifier
        {
            get
            {
                return BitConverter.ToUInt32(mData, 4);
            }
            set
            {
                Writer.WriteUInt32(value, 4, mData);
            }
        }

        public byte StudyFlag
        {
            get
            {
                return mData[11];
            }
            set
            {
                mData[11] = value;
            }
        }

        public ChiSystemType Type
        {
            get
            {
                return (ChiSystemType)BitConverter.ToUInt16(mData, 8);
            }
            set
            {
                Writer.WriteUInt32((ushort)value, 8, mData);
            }
        }

        public byte Unknown1
        {
            get
            {
                return mData[10];
            }
            set
            {
                mData[10] = value;
            }
        }

        public uint Unknown2
        {
            get
            {
                return BitConverter.ToUInt32(mData, 12);
            }
            set
            {
                Writer.WriteUInt32(value, 12, mData);
            }
        }

       /* public static implicit operator byte[](ChiSystem d)
        {
            return d.mData;
        }*/

       

        public ChiSystem(byte[] d)
        {
            mData = new byte[d.Length];
            d.CopyTo(mData, 0);
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

    } // class ChiSystem
}
