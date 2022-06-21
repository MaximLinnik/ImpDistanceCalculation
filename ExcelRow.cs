using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpHoleCalculation
{
    class ExcelRow
    {
        public double number;
        public String date;
        public double count;

        public ExcelRow(double number, String date, double count)
        {
            this.number = number;
            this.date = date;
            this.count = count;
        }
    }
}
