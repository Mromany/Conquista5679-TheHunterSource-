using System;

namespace PhoenixProject.Network.GamePackets
{
    public class NpcSpawn : Writer, Interfaces.IPacket, Interfaces.INpc, Interfaces.IMapObject
    {
        private byte[] Buffer;
        private ulong _MapID;
        
        public NpcSpawn()
        {
            Buffer = new byte[32];
            WriteUInt16(24, 0, Buffer);
            WriteUInt16(2030, 2, Buffer);
           // WriteUInt16(1, 22, Buffer);
        }

        public uint UID
        {
            get { return BitConverter.ToUInt32(Buffer, 4); }
            set { WriteUInt32(value, 4, Buffer); }
        }

        public ushort X
        {
            get { return BitConverter.ToUInt16(Buffer, 12); }
            set { WriteUInt16(value, 12, Buffer); }
        }

        public ushort Y
        {
            get { return BitConverter.ToUInt16(Buffer, 14); }
            set { WriteUInt16(value, 14, Buffer); }
        }

        public ushort Mesh
        {
            get { return BitConverter.ToUInt16(Buffer, 16); }
            set { WriteUInt16(value, 16, Buffer); }
        }

        public PhoenixProject.Game.Enums.NpcType Type
        {
            get { return (PhoenixProject.Game.Enums.NpcType)Buffer[18]; }
            set { Buffer[18] = (byte)value; }
        }

        public ulong MapID { get { return _MapID; } set { _MapID = value; } }

        public PhoenixProject.Game.MapObjectType MapObjType { get { return PhoenixProject.Game.MapObjectType.Npc; } }
        
        public Client.GameState Owner { get { return null; } }

        public void SendSpawn(Client.GameState client, bool checkScreen)
        {
            if (client.Screen.Add(this) || !checkScreen)
            {
                client.Send(Buffer);
            }
        }
        public void SendSpawn(Client.GameState client)
        {
            SendSpawn(client, false);
        }

        public byte[] ToArray()
        {
            return Buffer;
        }
        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
        public void Send(Client.GameState client)
        {
            SendSpawn(client, false);
        }
    }
}
