using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PhoenixProject.Game
{
    public class NightDay
    {
        public static bool IsNight = false;
        public static void SendTimer()
        {
#if NIGHT
            System.Timers.Timer DayTimer = new System.Timers.Timer(10000.0);
            DayTimer.Start();
            DayTimer.Elapsed += delegate { Day(); };
            System.Timers.Timer NTimer = new System.Timers.Timer(10000.0);
            NTimer.Start();
            NTimer.Elapsed += delegate { Night(); };
#endif
        }
        public static System.Timers.Timer DiscoTimer;
 
        public static void Day()
        {
            if (DateTime.Now.Hour == 00 || 
                DateTime.Now.Hour == 02 || 
                DateTime.Now.Hour == 04 || 
                DateTime.Now.Hour == 06 || 
                DateTime.Now.Hour == 08 || 
                DateTime.Now.Hour == 10 ||
                DateTime.Now.Hour == 12 || 
                DateTime.Now.Hour == 14 || 
                DateTime.Now.Hour == 16 || 
                DateTime.Now.Hour == 18 || 
                DateTime.Now.Hour == 22)
            {
                IsNight = false;
            }
        }
        public static void Night()
        {
            if (DateTime.Now.Hour == 01 ||
               DateTime.Now.Hour == 03 ||
               DateTime.Now.Hour == 05 ||
               DateTime.Now.Hour == 07 ||
               DateTime.Now.Hour == 09 ||
               DateTime.Now.Hour == 11 ||
               DateTime.Now.Hour == 13 ||
               DateTime.Now.Hour == 15 ||
               DateTime.Now.Hour == 17 ||
               DateTime.Now.Hour == 19 ||
               DateTime.Now.Hour == 21 ||
               DateTime.Now.Hour == 23)
            {
                IsNight = true;
            }
        }
    }
}
