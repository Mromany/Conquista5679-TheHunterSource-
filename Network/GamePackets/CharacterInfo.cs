using System;

namespace PhoenixProject.Network.GamePackets
{
    public class CharacterInfo : Writer, Interfaces.IPacket
    {
        Client.GameState client;
        public CharacterInfo(Client.GameState _client)
        {
            client = _client;
        }
        public void Deserialize(byte[] buffer)
        {
            throw new NotImplementedException();
        }
        public byte[] ToArray()
        {
            byte[] Packet = new byte[122 + client.Entity.Spouse.Length + client.Entity.Name.Length + 2];
            WriteUInt16((ushort)(Packet.Length - 8), 0, Packet);
            WriteUInt16(1006, 2, Packet);
            WriteUInt32(client.Entity.UID, 4, Packet);
            WriteUInt32(client.Entity.Mesh, 10, Packet);
            WriteUInt16(client.Entity.HairStyle, 14, Packet);
            WriteUInt32(client.Entity.Money, 16, Packet);
            WriteUInt32((uint)client.Entity.ConquerPoints, 20, Packet);
            WriteUInt64(client.Entity.Experience, 24, Packet);
            WriteUInt16(client.Entity.Strength, 52, Packet);
            WriteUInt16(client.Entity.Agility, 54, Packet);
            WriteUInt16(client.Entity.Vitality, 56, Packet);
            WriteUInt16(client.Entity.Spirit, 58, Packet);
            WriteUInt16(client.Entity.Atributes, 60, Packet);
            WriteUInt16((ushort)client.Entity.Hitpoints, 62, Packet);
            WriteUInt16(client.Entity.Mana, 64, Packet);
            WriteUInt16(client.Entity.PKPoints, 66, Packet);

           // Packet[67] = client.Entity.Level;
            Packet[68] = client.Entity.Level;
            Packet[69] = client.Entity.Class;
            Packet[70] = client.Entity.FirstRebornClass;
            Packet[71] = client.Entity.SecondRebornClass;
           // Packet[72] = client.Entity.Reborn;
            Packet[73] = client.Entity.Reborn;

            WriteUInt32(client.Entity.QuizPoints, 75, Packet);
            WriteUInt16(client.Entity.EnlightenPoints, 79, Packet);
            WriteUInt16(0/*enlightened time left*/, 81, Packet);
            //WriteUInt16(0/*enlightened time left*/, 77, Packet);
            WriteUInt16(client.Entity.VIPLevel, 87, Packet);
            if (Game.Tournaments.EliteTournament.Top8.ContainsKey(client.Entity.UID))
            {
                client.Entity.TitleActivated = Game.Tournaments.EliteTournament.Top8[client.Entity.UID].MyTitle;
                WriteUInt16(client.Entity.TitleActivated, 91, Packet);
            }
           
            WriteUInt32(client.Entity.BConquerPoints, 93, Packet);
           // WriteUInt32(client.Entity.RacePoints, 89, Packet);
            Packet[109] = 1;
           // WriteByte(3, 110, Packet);
            WriteUInt16((ushort)client.Entity.CountryFlag, 110, Packet);
            WriteByte(3, 112, Packet);
            Packet[113] = (byte)client.Entity.Name.Length;
            WriteString(client.Entity.Name, 112 + 2, Packet);
            WriteByte((byte)client.Entity.Spouse.Length, 113 + 2 + client.Entity.Name.Length, Packet);
            WriteString(client.Entity.Spouse, 114 + 2 + client.Entity.Name.Length, Packet);
            return Packet;
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }
    }
}
