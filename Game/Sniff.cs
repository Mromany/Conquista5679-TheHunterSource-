namespace sniff
{
    using System;
    using System.IO;
    using System.Text;
    using PhoenixProject;

    public class PacketSniffing
    {
        private static uint _count = 0;
        private static DateTime _time = DateTime.Now;

        public static StreamWriter SW;
        private static bool _sniffing = false;
        public static bool Sniffing
        {
            get { return _sniffing; }
            set
            {
                _time = DateTime.Now;
                if (value)
                    SW = new StreamWriter(Program.PacketSniffingPath + _time.ToString("Year/Minute/Day") + ".txt", true);
                else
                    SW.Dispose();
                _sniffing = value;
            }
        }
        private static Action<byte[], bool> SniffThis = new Action<byte[], bool>(_sniff);
        public static void Sniff(byte[] data, bool client)
        {
            _sniff(data, client);
        }
        private static void _sniff(byte[] data, bool client)
        {
            string dataString = "";
            string from = "";
            if (client)
                from = "TQServer";
            else
                from = "TQClient";
            dataString += "Packet " + _count + " -- " + from + " -- Length: " + PhoenixProject.BitConverter.ToUInt16(data, 0) + " | "
                + data.Length + " -- Type: " + PhoenixProject.BitConverter.ToUInt16(data, 2) + Environment.NewLine;

            for (int i = 0; i < Math.Ceiling((double)data.Length / 16); i++)
            {
                int t = 16;
                if ((i + 1) * 16 > data.Length)
                    t = data.Length - (i * 16);
                for (int a = 0; a < t; a++)
                    dataString += data[i * 16 + a].ToString("X2") + " ";
                if (t < 16)
                    for (int a = t; a < 16; a++)
                        dataString += "   ";
                dataString += ";   ";
                for (int a = 0; a < t; a++)
                    dataString += Convert.ToChar(data[i * 16 + a]);
                dataString += Environment.NewLine;
            }
            dataString.Replace(Convert.ToChar(0), '.');
            dataString += Environment.NewLine;
            Write(dataString);
        }
        private static void Write(string data)
        {
        Again:
            bool written = false;
            if (SW == null)
                SW = new StreamWriter(Program.PacketSniffingPath + _time.ToString("Year/Minute/Day") + ".txt", true);
            lock (SW)
            {
                try
                {
                    SW.WriteLine(data);
                    written = true;
                    SW.Flush();
                    _count++;
                }
                catch { goto Again; }
            }
            if (!written)
                goto Again;
        }
    }
}