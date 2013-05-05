using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject
{
    public static class Console
    {
        private static DateTime NOW = DateTime.Now;
        private static Time32 NOW32 = Time32.Now;
        public static string Title
        {
            get
            {
                return System.Console.Title;
            }
            set
            {
                System.Console.Title = value;
            }
        }

        public static int WindowWidth
        {
            get
            {
                return System.Console.WindowWidth;
            }
            set
            {
                System.Console.WindowWidth = value; ;
            }
        }

        public static int WindowHeight
        {
            get
            {
                return System.Console.WindowHeight;
            }
            set
            {
                System.Console.WindowHeight = value; ;
            }
        }

        public static void WriteLine(object line)
        {
            if (line.ToString() == "" || line.ToString() == " ")
                Console.WriteLine();
            else
            {
                ForegroundColor = ConsoleColor.Green;
                System.Console.Write(TimeStamp() + " ");
                ForegroundColor = ConsoleColor.DarkCyan;
                System.Console.Write(line + "\n");
            }
        }

        public static void WriteLine()
        {
            Console.WriteLine();
        }

        public static string ReadLine()
        {
            return System.Console.ReadLine();
        }

        public static ConsoleColor BackgroundColor
        {
            get
            {
                return System.Console.BackgroundColor;
            }
            set
            {
                System.Console.BackgroundColor = value;
            }
        }

        public static void Clear()
        {
            System.Console.Clear();
        }

        public static ConsoleColor ForegroundColor
        {
            get
            {
                return System.Console.ForegroundColor;
            }
            set
            {
                System.Console.ForegroundColor = value;
            }
        }

        public static string TimeStamp()
        {
            return "[" + NOW.AddMilliseconds((Time32.Now - NOW32).GetHashCode()).ToString("hh:mm:ss") + "]";
        }
    }
}
