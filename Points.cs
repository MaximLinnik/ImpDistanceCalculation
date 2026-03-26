// Mathematics.cs
// Хабаровск ИГД РАН
// математическая библиотека

using System;

namespace GCS.Mathematics
{
    public class Point3   // Класс myPoint => Point3
    {
        private double m_x, m_y, m_z;
        
        public Point3()
        {
            //m_x = 0.0;
            //m_y = 0.0;
            //m_z = 0.0;
        } 
        
        /// <summary>
        /// Конструктор. Параметры - координаты X, Y, Z.
        /// </summary>
        public Point3(double X, double Y, double Z)
        {
            m_x = X;
            m_y = Y;
            m_z = Z;
        }

        /// <summary>
        /// Конструктор. Параметр - точка.
        /// </summary>
        public Point3(Point3 pt)
        {
            m_x = pt.X;
            m_y = pt.Y; ;
            m_z = pt.Z; ;
        }

        /// <summary>
        /// Координата X.
        /// </summary>
        public double X
        {
            get { return (double)m_x; }
            set { m_x = (double)value; }
        }
        
        /// <summary>
        /// Координата Y.
        /// </summary>
        public double Y
        {
            get { return (double)m_y; }
            set { m_y = (double)value; }
        }
        
        /// <summary>
        /// Координата Z.
        /// </summary>
        public double Z
        {
            get { return (double)m_z; }
            set { m_z = (double)value; }
        }

        /// <summary>
        ///Переопределенный метод ToString().
        /// </summary>
        public override string ToString()
        {
            return String.Format("X: {0,12:F2} ", m_x) +
                   String.Format("Y: {0,12:F2} ", m_y) +
                   String.Format("Z: {0,12:F2} ", m_z);
        }
        
        /// <summary>
        /// Перегруженный оператор "+".
        /// </summary>
        public static Point3 operator +(Point3 p1, Point3 p2) 
        {
            return new Point3(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }
        
        /// <summary>
        /// Перегруженный оператор "-".
        /// </summary>
        public static Point3 operator -(Point3 p1, Point3 p2)
        {
            return new Point3(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }
        
        /// <summary>
        /// Static метод сложения 2-х точек.
        /// </summary>
        public static Point3 AddPoints(Point3 p1, Point3 p2)
        {
            return new Point3(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }
        
        /// <summary>
        /// Static метод вычитания 2-х точек.
        /// </summary>
        public static Point3 SubtractPoints(Point3 p1, Point3 p2)
        {
            return new Point3(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }
        
        /// <summary>
        /// Static метод вычисления расстояния между двумя точками.
        /// </summary>
        public static double DistancePoints(Point3 p1, Point3 p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X,2) + Math.Pow(p1.Y - p2.Y,2) + Math.Pow(p1.Z - p2.Z,2));
        }

        //Получение параметров плоскости по трем точкам
        public static double getPlane_A(Point3 p1, Point3 p2, Point3 p3)
        {
            return p3.Y * (p1.Z - p2.Z) + p1.Y * (p2.Z - p3.Z) + p2.Y * (p3.Z - p1.Z);
        }
        public static double getPlane_B(Point3 p1, Point3 p2, Point3 p3)
        {
            return p3.Z * (p1.X - p2.X) + p1.Z * (p2.X - p3.X) + p2.Z * (p3.X - p1.X);
        }
        public static double getPlane_C(Point3 p1, Point3 p2, Point3 p3)
        {
            return p3.X * (p1.Y - p2.Y) + p1.X * (p2.Y - p3.Y) + p2.X * (p3.Y - p1.Y);
        }
        public static double getPlane_Dneg(Point3 p1, Point3 p2, Point3 p3)
        {
            return p3.X * (p1.Y * p2.Z - p2.Y * p1.Z) + p1.X * (p2.Y * p3.Z - p3.Y * p2.Z) + p2.X * (p3.Y * p1.Z - p1.Y * p3.Z);
        }
    }
}
