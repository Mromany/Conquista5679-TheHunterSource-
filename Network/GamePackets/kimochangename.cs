using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Network.GamePackets
{
     public class NameChange : Writer, Interfaces.IPacket
    {
        public enum NameChangeAction : ushort
        {
            Request = 0,
            Success = 1,
            NameTaken = 2,
            DialogInfo = 3,
            FreeChange = 4,
        }
        private string _name;
        byte[] Buffer;
        public NameChange(bool Create)
        {
            if (Create)
            {
                Buffer = new byte[26 + 8];
                WriteUInt16((ushort)Buffer.Length, 0, Buffer);
                WriteUInt16(2080, 2, Buffer);
                //Console.WriteLine("10:" + Buffer.Length + "");
            }
        }
        public string Name
        {
            get
            {

                return _name;
            }
            set
            {
                _name = Encoding.ASCII.GetString(Buffer, 10, 16).TrimEnd('\0');
            }
        }
        private NameChangeAction _Action;
        public NameChangeAction Action
        {
            get
            {
                return _Action;
            }

            set
            {
                _Action = (NameChangeAction)Buffer[4];
            }
        }
        public ushort _EditCount ;
        public ushort EditCount
        {
            get
            {
                return _EditCount;
            }

            set
            {
                _EditCount = BitConverter.ToUInt16(Buffer, 6);
            }
        }
        public ushort _EditAllowed = 1;
        public ushort EditAllowed
        {
            get
            {
                return _EditAllowed;
            }

            set
            {
                _EditAllowed = BitConverter.ToUInt16(Buffer, 8);
            }
        }
        public byte[] ToArray()
        {
            return Buffer;
        }

        public void Send(Client.GameState client)
        {
            client.Send(Buffer);
        }
        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }

    }
}