using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace PhoenixProject.Network.Sockets
{
    public class WinSocket
    {
        private Socket Connection;
        private bool disconnected = false;
        public bool Disabled = false;
        public WinSocket()
        {
            Connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public WinSocket(Socket socket)
        {
            Connection = socket;
        }
        public void Bind(EndPoint point)
        {
            if (disconnected) return;
            Connection.Bind(point);
        }
        public void Listen(int backlog)
        {
            if (disconnected) return;
            Connection.Listen(backlog);
        }
        public void BeginAccept(AsyncCallback async, object state)
        {
            if (Disabled) return;
            try
            {
                if (disconnected) return;
                Connection.BeginAccept(async, state);
            }
            catch
            {
                Disabled = true;
            }
        }
        public void BeginReceive(byte[] buffer, int offset, int size, SocketFlags flag, AsyncCallback callback, object state)
        {
            if (Disabled) return;
            try
            {
                if (disconnected || !Connected) return;
                Connection.BeginReceive(buffer, offset, size, flag, callback, state);
            }
            catch
            {
                Disabled = true;
            }
        }
        public int EndReceive(IAsyncResult res, out SocketError err)
        {
            err = SocketError.Disconnecting;
            if (Disabled) return 0;
            try
            {
                if (!Usable) return 0;
                return Connection.EndReceive(res, out err);
            }
            catch
            {
                Disabled = true;
            }
            return 0;
        }
        public Socket EndAccept(IAsyncResult async)
        {
            if (Disabled) return null;
            try
            {
                if (disconnected) return null;
                return Connection.EndAccept(async);
            }
            catch
            {
                Disabled = true;
            }
            return null;
        }
        public Socket Accept()
        {
            if (Disabled) return null;
            try
            {
                if (disconnected) return null;
                return Connection.Accept();
            }
            catch
            {
                Disabled = true;
            }
            return null;
        }
        public void Close()
        {
            //Connection.Close();
        }
        public void Send(byte[] buffer)
        {
            if (Disabled) return;
            try
            {
                if (disconnected) return;
                Connection.Send(buffer);
            }
            catch
            {
                Disabled = true;
            }
        }
        public void Disconnect(bool reuse)
        {
            //Console.WriteLine("Close");
            if (Disabled) return;
            try
            {
                if (!disconnected)
                    disconnected = true;
                else
                    return;
                //System.Threading.Thread.Sleep(4);


                //Connection.Shutdown(SocketShutdown.Both);
                //Connection.Close();
               // Connection.Dispose();
                //Connection = null;
                Connection.Disconnect(false);

            }
            catch (Exception e)
            {
                Program.SaveException(e);
                Disabled = true;
            }
        }
        public void Shutdown(SocketShutdown type)
        {
            Connection.Shutdown(type);
        }
        public bool Connected
        {
            get
            {
                if (Disabled) return false;
                try
                {
                    if (disconnected) return false;
                    return Connection.Connected;
                }
                catch
                {
                    Disabled = true;
                }
                return false;
            }
        }
        public int Receive(byte[] buffer)
        {
            if (disconnected) return 0;
            return Connection.Receive(buffer);
        }
        public EndPoint RemoteEndPoint
        {
            get
            {
                if (Disabled) return new IPEndPoint(1, 1);
                try
                {
                    return Connection.RemoteEndPoint;
                }
                catch
                {
                    Disabled = true;
                }
                return new IPEndPoint(1, 1);
            }
        }
        private bool Usable
        {
            get
            {
                return !disconnected;
            }
        }
        public void Disable()
        {
            try
            {
                Disabled = true;
                Connection.Close();
            }
            catch
            {
                Disabled = true;
            }
        }
    }
}
