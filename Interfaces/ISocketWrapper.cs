using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoenixProject.Network.Sockets;

namespace PhoenixProject.Interfaces
{
    public interface ISocketWrapper
    {
        int BufferSize { get; set; }
        byte[] Buffer { get; set; }
        WinSocket Socket { get; set; }
        object Connector { get; set; }
        ISocketWrapper Create(System.Net.Sockets.Socket socket);

    }
}
