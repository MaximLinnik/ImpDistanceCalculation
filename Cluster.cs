using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpHoleCalculation
{
    public class Cluster
    {
        public decimal amplitudeBefore;
        public decimal amplitudeAfter;
        public decimal durationBefore;
        public decimal durationAfter;
        public decimal friequencyBefore;
        public decimal friequencyAfter;
        public double countAll; // подсчет всех импульсов
        public double count30 ; // подсчет импульсов с идентификатором 30

        public Cluster(decimal ab, decimal aa, decimal db, decimal da, decimal fb, decimal fa)
        {
            amplitudeBefore = ab;
            amplitudeAfter = aa;
            durationBefore = db;
            durationAfter = da;
            friequencyBefore = fb;
            friequencyAfter = fa;
            countAll = 0;
            count30 = 0;
        }
    }
}
