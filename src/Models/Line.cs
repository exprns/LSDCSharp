
using System;

namespace LineSegmentDetectorCSharp.Models
{
    public class Line
    {
        public Point Pnt1 { get; set; }
        public Point Pnt2 { get; set; }

        public Line(Point pnt1, Point pnt2)
        {
            Pnt1 = pnt1;
            Pnt2 = pnt2;
        }
    }

    public class Point
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Point(double x, double y)
        {
            X = (float)x;
            Y = (float)y;
        }

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Point()
        {
        }

        public bool IsEmpty() => X == -1 && Y == -1;

        public static Point Empty()
        {
            return new Point(-1, -1);
        }

        public bool IsEqual(Point pnt, double tol = 0.0001) => Math.Abs(X - pnt.X) < tol && Math.Abs(Y - pnt.Y) < tol;

        public double DistanceTo(Point pnt) => Math.Sqrt((Math.Pow(X - pnt.X, 2) + Math.Pow(Y - pnt.Y, 2)));
    }

}
