using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpHoleCalculation
{
    public class HoleParametrs
    {
        private int name;
        private double X;
        private double Y;
        private double Z;

        public HoleParametrs setHoleParametrs(int name, double X, double Y, double Z)
        {
            this.name = name;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            return this;
        }

        public int getName()
        {
            return name;
        }

        public double getX()
        {
            return X;
        }

        public double getY()
        {
            return Y;
        }

        public double getZ()
        {
            return Z;
        }
    }
}
