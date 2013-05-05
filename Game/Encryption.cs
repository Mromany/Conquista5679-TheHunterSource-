using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace PhoenixProject.Game
{
    public class Encryption
    {
        public Encryption()
        {
           // Class0.N47LJ78z09Kgf();
        }

        public static byte[] Encrypt(byte[] data, byte[] key)
        {
            byte[] buffer = data;
            byte[] rgbKey = key;
            RijndaelManaged managed = new RijndaelManaged();
            managed.Mode = CipherMode.ECB;
            managed.Padding = PaddingMode.Zeros;
            ICryptoTransform transform = managed.CreateEncryptor(rgbKey, null);
            MemoryStream stream = new MemoryStream(buffer);
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            byte[] buffer3 = new byte[buffer.Length];
            stream2.Read(buffer3, 0, buffer3.Length);
            stream.Close();
            stream2.Close();
            return buffer3;
        }
    }
}
