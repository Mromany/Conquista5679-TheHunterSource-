using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Generated.Interfaces
{
    public struct Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int Distance(Point to)
        {
            return Math.Max(Math.Abs(X - to.X), Math.Abs(Y - to.Y));
        }
    }
}
