using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace PhoenixProject.Game.Features
{
    public class DisCity
    {
        public static Hashtable RightFlank = new Hashtable();
        public static Hashtable LeftFlank = new Hashtable();

        public static uint RightKills = 0;
        public static uint LeftKills = 0;

        public static bool dis = false;
        public static bool dis2 = false;
        public static byte DisMax1 = 0;
        public static byte DisMax2 = 0;
        public static byte DisMax3 = 0;
        public int DisKO = 0;
        public bool DisQuest = false;
        public uint TopDlClaim = 0;
        public uint TopGlClaim = 0;
    }
}
