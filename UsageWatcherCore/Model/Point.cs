using System;
using System.Collections.Generic;
using System.Text;

namespace UsageWatcherCore.Model
{
    internal class Point
    {
        double X { get; }
        double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
