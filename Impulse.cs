using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpHoleCalculation
{
    class Impulse
    {
        double id;
        double hwid;
        DateTime date;
        double holeName;
        double amplitude;
        double duration;
        double position; 
        DataGridViewRow row;// для таблицы

        public Impulse(double id, double hwid, DateTime date, double holeName, double amplitude, double duration, DataGridViewRow row)
        {
            this.id = id;
            this.hwid = hwid;
            this.date = date;
            this.holeName = holeName;
            this.amplitude = amplitude;
            this.duration = duration;
            this.row = row;
        }
    }
}
