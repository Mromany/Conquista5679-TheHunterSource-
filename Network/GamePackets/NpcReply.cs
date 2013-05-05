using System;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
    public class NpcReply : Writer, Interfaces.IPacket
    {
        public const byte
            Dialog = 1,
            Option = 2,
            Input = 3,
            Avatar = 4,
            MessageBox = 6,
            Finish = 100;

        private byte[] Buffer;

        public NpcReply()
        {
            Buffer = new byte[24];
            WriteUInt16((ushort)16, 0, Buffer);
            WriteUInt16(2032, 2, Buffer);
        }

        public static byte[] NPCLink5(string NPC_say, byte NPC_dialognumber)
        {
            byte[] PacketData = new byte[(0xf + NPC_say.Length) + 2 + 8];
            PacketData[0x0] = (byte)(PacketData.Length - 8 & 0xff);
            PacketData[0x1] = (byte)(PacketData.Length - 8 >> 8);
            PacketData[0x2] = 0xf0;
            PacketData[0x3] = 0x07;
            PacketData[0x4] = 0x00;
            PacketData[0x5] = 0x00;
            PacketData[0x6] = 0x00;
            PacketData[0x7] = 0x00;
            PacketData[0x8] = 0x00;
            PacketData[0x9] = 0x00;
            PacketData[0xa] = (byte)(NPC_dialognumber & 0xff); // Responce ID for NPC
            PacketData[0xb] = 0x06;
            PacketData[0xc] = 0x02;
            PacketData[0xd] = (byte)(NPC_say.Length);  //Length of the NPC text to say
            for (int x = 0; x < NPC_say.Length; x++)
            {
                PacketData[0xe + x] = Convert.ToByte(NPC_say[x]);
            }
            string s = "TQServer";
            ASCIIEncoding encoding = new ASCIIEncoding();
            encoding.GetBytes(s).CopyTo(PacketData, (int)(PacketData.Length - 8));

            return PacketData;
        }
        public NpcReply(byte interactType, string text)
        {
            Buffer = new byte[25];
            WriteUInt16((ushort)(17 + text.Length), 0, Buffer);
            WriteUInt16(2032, 2, Buffer);
            InteractType = interactType;
            OptionID = 255;
            DontDisplay = true;
            Text = text;
        }
        public void Reset()
        {
            OptionID = 255;
            DontDisplay = true;
            Text = "";
        }

        public byte[] ToArray()
        {
            return Buffer;
        }
        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
        /// <summary>
        /// This should be the max length of the input string if the interact type is
        /// `Input`. Otherwise, if it is neither of these two, it should be 0.
        /// </summary>
        public ushort InputMaxLength
        {
            get { return BitConverter.ToUInt16(Buffer, 8); }
            set { WriteUInt16(value, 8, Buffer); }
        }
        public byte OptionID
        {
            get { return Buffer[10]; }
            set { Buffer[10] = value; }
        }
        public byte InteractType
        {
            get { return Buffer[11]; }
            set { Buffer[11] = value; }
        }
        /// <summary>
        /// This should be set to false when your sending the packet with the
        /// interaction type `Finish`, otherwise true
        /// </summary>
        public bool DontDisplay
        {
            get { return (Buffer[12] == 1); }
            set { Buffer[12] = (byte)(value ? 1 : 0); }
        }
        public string Text
        {
            get { return Encoding.UTF7.GetString(Buffer, 14, Buffer[13]); }
            set
            {
                int realloc = value.Length + 8 + 17;
                if (realloc != Buffer.Length)
                {
                    byte[] new_Packet = new byte[realloc];
                    System.Buffer.BlockCopy(Buffer, 0, new_Packet, 0, 24);
                    Buffer = new_Packet;
                }
                WriteUInt16((ushort)(value.Length + 17), 0, Buffer);
                WriteStringWithLength(value, 13, Buffer);
            }
        }
        public byte times
        {
            get { return Buffer[6]; }
            set { Buffer[6] = value; }
        }
        public void Send(Client.GameState client)
        {
            client.Send(Buffer);
        }
    }
}
