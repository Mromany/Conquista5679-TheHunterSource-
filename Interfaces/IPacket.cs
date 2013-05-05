using System;

namespace PhoenixProject.Interfaces
{
    public interface IPacket
    {
        byte[] ToArray();
        void Deserialize(byte[] buffer);
        void Send(Client.GameState client);
    }
}
