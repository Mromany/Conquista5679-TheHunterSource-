using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KinSocket;

namespace PhoenixProject.Game
{
    class ProxyCryptography
    {
        public static byte[] Encrypt(byte[] packet, bool client, bool send)
        {
            if (send)
            {
                // the packet is sending
                if (client)
                {
                    // the packet is send to the client
                }
                else
                {
                    // the packet is send to the server
                }
            }
            else
            {
                // the packet is receiving
                if (client)
                {
                    // the packet is received from the client
                }
                else
                {
                    // the packet is received from the server
                }
            }
            return packet;
        }
       
        public static byte[] Decrypt(byte[] packet, bool client, bool send)
        {
            if (send)
            {
                // the packet is sending
                if (client)
                {
                    // the packet is send to the client
                }
                else
                {
                    // the packet is send to the server
                }
            }
            else
            {
                // the packet is receiving
                if (client)
                {
                    // the packet is received from the client
                }
                else
                {
                    // the packet is received from the server
                }
            }
            return packet;
        }
    }
}
