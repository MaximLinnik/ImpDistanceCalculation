using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpDistanceCalculation
{
    class ExcelRow
    {
        public double number;
        public DateTime date;
        public double count;

        public ExcelRow(double number, DateTime date, double count)
        {
            this.number = number;
            this.date = date;
            this.count = count;
        }
    }
}
