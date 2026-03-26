using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpDistanceCalculation
{
    class ListHoles
    {
        public String name;
        public List<ExcelMerge> excelRows;

        public ListHoles(string name)
        {
            this.name = name;
            this.excelRows = new List<ExcelMerge>();
        }
    }

}
