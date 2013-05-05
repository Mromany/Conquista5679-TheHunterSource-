using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Interfaces;
using PhoenixProject.Statement;

namespace PhoenixProject.Network.GamePackets
{
    #region Class Level
    public class SubClassShowFull : Writer, Interfaces.IPacket
    {
        public const byte
        SwitchSubClass = 0,
        ActivateSubClass = 1,
        LearnSubClass = 4,
        ShowGUI = 7;

        byte[] Buffer;
        public SubClassShowFull(bool Create)
        {
            if (Create)
            {
                Buffer = new byte[8 + 26];
                WriteUInt16(26, 0, Buffer);
                WriteUInt16(2320, 2, Buffer);
            }
        }

        public ushort ID
        {
            get { return BitConverter.ToUInt16(Buffer, 4); }
            set { WriteUInt16(value, 4, Buffer); }
        }

        public byte Class
        {
            get { return Buffer[6]; }
            set { Buffer[6] = value; }
        }

        public byte Level
        {
            get { return Buffer[7]; }
            set { Buffer[7] = value; }
        }

        public void Deserialize(byte[] buffer)
        {
            this.Buffer = buffer;
        }

        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Send(Client.GameState c)
        {
            c.Send(Buffer);
        }
    }
    #endregion
    #region Class Send
    public class Game_SubClassInfo : IPacket
    {
        private Byte[] mData;
        private Int32 Offset = 26;

        public Game_SubClassInfo(Game.Entity c, Game_SubClass.Types Type)
        {
            this.mData = new Byte[26 + (c.SubClasses.Classes.Count * 3) + 8];
            Writer.WriteUInt16((UInt16)(mData.Length - 8), 0, mData);
            Writer.WriteUInt16((UInt16)2320, 2, mData);

            Writer.WriteByte((Byte)Type, 4, mData);
            Writer.WriteUInt16((UInt16)c.SubClasses.StudyPoints, 6, mData);
            Writer.WriteUInt16((UInt16)c.SubClasses.Classes.Count, 22, mData);

            foreach (Statement.SubClass subc in c.SubClasses.Classes.Values)
            {
                Writer.WriteByte((Byte)subc.ID, Offset, mData); Offset++;
                Writer.WriteByte((Byte)subc.Phase, Offset, mData); Offset++;
                Writer.WriteByte((Byte)subc.Level, Offset, mData); Offset++;
            }
        }
        public void Deserialize(byte[] buffer)
        {
            this.mData = buffer;
        }

        public byte[] ToArray()
        {
            return mData;
        }

        public void Send(Client.GameState c)
        {
            c.Send(mData);
        }

    }
    public class Game_SubClass : IPacket
    {
        private Byte[] mData;

        public Game_SubClass()
        {
            this.mData = new Byte[29 + 8];
            Writer.WriteUInt16((UInt16)(mData.Length - 8), 0, mData);
            Writer.WriteUInt16((UInt16)2320, 2, mData);
        }

        public Types Type
        {
            get { return (Types)BitConverter.ToUInt16(mData, 4); }
            set { Writer.WriteUInt16((Byte)value, 4, mData); }
        }
        public ID ClassId
        {
            get { return (ID)mData[6]; }
            set { mData[6] = (Byte)value; }
        }
        public Byte Phase
        {
            get { return mData[7]; }
            set { mData[7] = value; }
        }
        public void Deserialize(byte[] buffer)
        {
            this.mData = buffer;
        }

        public byte[] ToArray()
        {
            return mData;
        }

        public void Send(Client.GameState c)
        {
            c.Send(mData);
        }

        public enum Types : ushort
        {
            Switch = 0,
            Activate = 1,
            Upgrade = 2,
            Learn = 4,
            MartialPromoted = 5,
            Show = 6,
            Info = 7
        }
        public enum ID : byte
        {
            None = 0,
            MartialArtist = 1,
            Warlock = 2,
            ChiMaster = 3,
            Sage = 4,
            Apothecary = 5,
            Performer = 6,
            Wrangler = 9
        }
    }
    public class SubClass : Writer, Interfaces.IPacket
    {
        public const byte
        SwitchSubClass = 0,
        ActivateSubClass = 1,
        ShowSubClasses = 7,
        MartialPromoted = 5,
        LearnSubClass = 4;
        Game.Entity Owner = null;

        byte[] Buffer;
        byte Type;
        public SubClass(Game.Entity E) { Owner = E; Type = 7; }

        public void Deserialize(byte[] buffer)
        {
            this.Buffer = buffer;
        }

        public byte[] ToArray()
        {
            Buffer = new byte[8 + 26 + (Owner.SubClasses.Classes.Count * 3)];
            WriteUInt16((ushort)(Buffer.Length - 8), 0, Buffer);
            WriteUInt16(2320, 2, Buffer);
            WriteUInt16((ushort)Type, 4, Buffer);
            WriteUInt32(Owner.SubClasses.StudyPoints, 6, Buffer);
            WriteUInt16((ushort)Owner.SubClasses.Classes.Count, 22, Buffer);
            int Position = 26;
            if (Owner.SubClasses.Classes.Count > 0)
            {
                Statement.SubClass[] Classes = new Statement.SubClass[Owner.SubClasses.Classes.Count];
                Owner.SubClasses.Classes.Values.CopyTo(Classes, 0);
                foreach (Statement.SubClass Class in Classes)
                {
                    WriteByte(Class.ID, Position, Buffer); Position++;
                    WriteByte(Class.Phase, Position, Buffer); Position++;
                    WriteByte(Class.Level, Position, Buffer); Position++;
                }
            }
            WriteString("TQServer", (Buffer.Length - 8), Buffer);
            return Buffer;
        }

        public void Send(Client.GameState c)
        {
            c.Send(Buffer);
        }
    }
    #endregion
}
