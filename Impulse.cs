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
        public double id;
        public String hwid;
        public DateTime date;
        public String holeName;
        public double amplitude;
        public double duration;
        public double position;
        public DataGridViewRow row;// для таблицы

        //для алг30
        public double DT;
        public Coordinates coordinates;

        public Impulse(double id, String hwid, DateTime date, String holeName, double amplitude, double duration, DataGridViewRow row)
        {
            this.id = id;
            this.hwid = hwid;
            this.date = date;
            this.holeName = holeName;
            this.amplitude = amplitude;
            this.duration = duration;
            this.row = row;
        }

        //для алг 30
        public Impulse(double id, String hwid, DateTime date, String holeName, double amplitude, double duration, Coordinates coordinates, double DT)
        {
            this.id = id;
            this.hwid = hwid;
            this.date = date;
            this.holeName = holeName;
            this.amplitude = amplitude;
            this.duration = duration;
            this.coordinates = coordinates;
            this.DT = DT;
        }

        public Impulse(DataGridViewRow row)
        {
            this.id = double.Parse(row.Cells[1].ToString());
            this.hwid = row.Cells[2].ToString(); 
            this.date = DateTime.Parse(row.Cells[3].ToString()); 
            this.holeName = row.Cells[4].ToString(); 
            this.amplitude = double.Parse(row.Cells[5].ToString()); 
            this.duration = double.Parse(row.Cells[6].ToString()); 
            this.row = row;
        }
    }
}
