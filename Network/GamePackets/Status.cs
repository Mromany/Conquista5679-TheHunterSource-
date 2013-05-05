using System;

namespace PhoenixProject.Network.GamePackets
{
    public class Status : Writer, Interfaces.IPacket
    {
        Client.GameState client;
        byte[] Buffer;
        public Status(Client.GameState _client)
        {
            client = _client;

        }
        public byte[] ToArray()
        {
            Buffer = new byte[144];
            WriteUInt16(144 - 8, 0, Buffer);
            WriteUInt16(1040, 2, Buffer);
            WriteUInt32(client.Entity.UID, 4, Buffer);//UID
            WriteUInt32(client.Entity.MaxHitpoints, 8, Buffer);//MaxHp
            WriteUInt32(client.Entity.MaxMana, 12, Buffer);//MaxMana
            WriteUInt32(client.Entity.MinAttack, 20, Buffer);//MinAtk
            WriteUInt32(client.Entity.MaxAttack, 16, Buffer);//MaxAtk
            WriteUInt32(client.Entity.Defence, 24, Buffer);// Defense
            WriteUInt32(client.Entity.MagicAttack, 28, Buffer);// Magic Attack
            WriteUInt32(client.Entity.MagicDefence, 32, Buffer);//Magic Deffense
            WriteUInt32(client.Entity.Dodge, 36, Buffer);//Dodge
            WriteUInt32(client.Entity.Agility, 40, Buffer);//ExtraAgility/whatever
            WriteUInt32(200, 44, Buffer);// accuracy
            WriteUInt32((uint)client.Entity.DragonGems, 48, Buffer);// SDG extra attack %
            WriteUInt32((uint)client.Entity.PhoenixGems, 52, Buffer);//MagicAttack %
            WriteUInt32((uint)client.Entity.MagicDefencePercent, 56, Buffer);//Magic Defense %
            WriteUInt32(200, 60, Buffer);//Damage
            WriteUInt32((uint)(client.Entity.Immunity), 80, Buffer);// Immunity
            WriteUInt32((uint)(client.Entity.getFan(false)), 100, Buffer);
            WriteUInt32((uint)(client.Entity.getFan(true)), 104, Buffer);
            WriteUInt32((uint)(client.Entity.getTower(false)), 108, Buffer);
            WriteUInt32((uint)(client.Entity.getTower(true)), 112, Buffer);
            WriteUInt32(15, 116, Buffer);
            WriteUInt32(15, 120, Buffer);
            WriteUInt32(15, 124, Buffer);
            WriteUInt32(15, 128, Buffer);
            WriteUInt32(15, 132, Buffer);
            return Buffer;
        }
        public void Deserialize(byte[] buffer)
        {
            Buffer = buffer;
        }
        public void Send(Client.GameState client)
        {
            client.Send(ToArray());
        }
    }
}