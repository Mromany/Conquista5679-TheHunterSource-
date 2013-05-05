using System;
using System.Threading;
using PhoenixProject.Network.GamePackets;
using PhoenixProject.Game.ConquerStructures.Society;
using System.IO;
using PhoenixProject.Network;
using PhoenixProject.Game;

namespace PhoenixProject.ServerBase
{
    public class Threads
    {
#if BASETHREADS
        private System.Threading.Thread base_thread;
#else
        private Timer base_timer;
#endif
        public Client.GameState client;
        public event Action Execute;
        //public static Client.Evento ServerBase1 = new Client.Evento();
        int Milliseconds;
        public Threads(int milliseconds)
        {
            Closed = false;
            Milliseconds = milliseconds;
        }
        public void Start()
        {
            
            base_thread = new System.Threading.Thread(new ThreadStart(Loop));
            base_thread.Start();
        }
        public DateTime ola;
        public DateTime SalidadeBoss;
        public static ServerBase.Counter EntityUIDCounter = new PhoenixProject.ServerBase.Counter(400000);
        public bool Closed
        {
            get;
            set;
        }
#if BASETHREADS
        private void Loop()
        {
            Sleep(500);
            while (1 > 0)
            {
                try
                {
                    
                    try
                    {
                        if (Execute != null)
                            Execute.Invoke();
                    }
                    catch (Exception e)
                    {
                        Program.SaveException(e);
                        Console.WriteLine(e);
                    }
                }
                catch { }
                Sleep(Milliseconds);
            }
        }
        public void Sleep(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }
#else
        private void Loop(object ob)
        {
            if (Close)
            {
                base_timer.Change(Timeout.Infinite, Timeout.Infinite);
                return;
            }
            try
            {
                if (Execute != null)
                    Execute.Invoke();
            }
            catch (Exception e)
            {
                Program.SaveException(e);
            }
        }
#endif
        public uint UID = 845421;
    }
}
