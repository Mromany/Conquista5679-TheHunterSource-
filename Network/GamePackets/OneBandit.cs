//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using KinSocket;

//namespace PhoenixProject.Network.GamePackets
//{
//    public class OneBandit
//    {
//        private byte[] mData;

//        public OneBandit()
//        {
//            this.mData = new byte[24 + 8];
//            PacketConstructor.Write(24, 0, this.mData);
//            PacketConstructor.Write((ushort)1352, 2, this.mData);
//        }

//        public OneBandit(byte[] d)
//        {
//            this.mData = new byte[d.Length];
//            d.CopyTo(this.mData, 0);
//        }

//        public static implicit operator byte[](OneBandit d)
//        {
//            return d.mData;
//        }

//        public uint Identifier
//        {
//            get
//            {
//                return BitConverter.ToUInt32(this.mData, 8);
//            }
//            set
//            {
//                PacketConstructor.Write(value, 8, this.mData);
//            }
//        }

//        public uint Type
//        {
//            get
//            {
//                return BitConverter.ToUInt32(this.mData, 4);
//            }
//            set
//            {
//                PacketConstructor.Write(value, 4, this.mData);
//            }
//        }

//        public uint Unknown2
//        {
//            get
//            {
//                return BitConverter.ToUInt32(this.mData, 6);
//            }
//            set
//            {
//                PacketConstructor.Write(value, 6, this.mData);
//            }
//        }

//        public uint Unknown3
//        {
//            get
//            {
//                return BitConverter.ToUInt32(this.mData, 10);
//            }
//            set
//            {
//                PacketConstructor.Write(value, 10, this.mData);
//            }
//        }
//    }
//}

