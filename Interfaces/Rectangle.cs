using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoenixProject.Generated.Interfaces
{
    public struct Rectangle
    {
        public Point LowerBound, UpperBound;

        public Rectangle(Point p, int w, int h)
        {
            LowerBound = p;
            UpperBound = new Point(p.X + w, p.Y + h);
        }
        public Rectangle(Point a, Point b)
        {
            LowerBound = a;
            UpperBound = b;
        }

        public bool AreaContains(Point p)
        {
            return (p.X >= LowerBound.X && p.X < UpperBound.X)
                && (p.Y >= LowerBound.Y && p.Y < UpperBound.Y);
        }
    }
}
