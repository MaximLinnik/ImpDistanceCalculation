using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Common.MathEx.Geometry3D
{
    static class Point3DExtensions
    {
        public static Point3D Round(this Point3D p, int digits) 
            => new Point3D(System.Math.Round(p.X, digits),
                           System.Math.Round(p.Y, digits),
                           System.Math.Round(p.Z, digits));
        public static double DistanceTo(this Point3D p1, Point3D p2) => Point3D.Subtract(p1, p2).Length;
        /*{
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            double dz = p1.Z - p2.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }*/
        public static double[] DistanceTo(this Point3D p1, IEnumerable<Point3D> points) => points?.Select(x => p1.DistanceTo(x)).ToArray() ?? null;
        public static double GetMaxDistance(this IList<Point3D> points)
        {
            double maxDistance = 0;
            for (int i = points.Count - 1; i > 0; i--)
                for (int j = i - 1; j >= 0; j--)
                {
                    double dist = points[i].DistanceTo(points[j]);
                    if (maxDistance < dist) maxDistance = dist;
                }
            return maxDistance;
        }
        public static bool AreDistinct(this IList<Point3D> points)
        {
            for (int i = points.Count - 1; i > 0; i--)
                for (int j = i - 1; j >= 0; j--)
                {
                    if (points[i].Equals(points[j])) return false;
                }
            return true;
        }
        public static bool AreCollinear(this Point3D Point1, Point3D Point2, Point3D Point3)
        {
            double Distance12 = Point1.DistanceTo(Point2);
            double Distance13 = Point1.DistanceTo(Point3);
            double Distance23 = Point2.DistanceTo(Point3);
            // largest distance must be equal sum of the other two
            if (Distance12 > Distance13)
            {
                if (Distance12 > Distance23)
                    return Distance12 == Distance13 + Distance23;
                else
                    return Distance23 == Distance13 + Distance12;
            }
            else
            {
                if (Distance13 > Distance23)
                    return Distance13 == Distance12 + Distance23;
                else
                    return Distance23 == Distance13 + Distance12;
            }
        }
        public static Point3D GetPoint(this Rect3D rect, int index)
        {
            switch (index)
            {
                case 0: return rect.Location;
                case 1: return new Point3D(rect.X + rect.SizeX, rect.Y, rect.Z);
                case 2: return new Point3D(rect.X + rect.SizeX, rect.Y + rect.SizeY, rect.Z);
                case 3: return new Point3D(rect.X, rect.Y + rect.SizeY, rect.Z);
                case 4: return new Point3D(rect.X, rect.Y, rect.Z + rect.SizeZ);
                case 5: return new Point3D(rect.X + rect.SizeX, rect.Y, rect.Z + rect.SizeZ);
                case 6: return new Point3D(rect.X + rect.SizeX, rect.Y + rect.SizeY, rect.Z + rect.SizeZ);
                case 7: return new Point3D(rect.X, rect.Y + rect.SizeY, rect.Z + rect.SizeZ);
                default: throw new ArgumentException("Rect3D has 8 points - index must be in [0;7]");
            }
        }
        public static (Point3D, Point3D) GetMinMax(this IList<Point3D> points)
        {
            if (!(points?.Count > 0)) throw new ArgumentException("Can't find (min,max) in empty list");

            double xmin, ymin, zmin, xmax, ymax, zmax;
            xmin = xmax = points[0].X;
            ymin = ymax = points[0].Y;
            zmin = zmax = points[0].Z;
            for (int i = 1; i < points.Count; i++)
            {
                xmin = Math.Min(xmin, points[i].X);
                xmax = Math.Max(xmax, points[i].X);
                ymin = Math.Min(ymin, points[i].Y);
                ymax = Math.Max(ymax, points[i].Y);
                zmin = Math.Min(zmin, points[i].Z);
                zmax = Math.Max(zmax, points[i].Z);
            }
            return (new Point3D(xmin, ymin, zmin), new Point3D(xmax, ymax, zmax));
        }

    }
}
