using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpHoleCalculation
{
    class ImpDifference
    {
        String eventId;
        String date;
        String difference;
        String rvp; //РВП (разница по времени)
        String ampl1;
        String ampl2;
        String duration1;
        String duration2;
        String durationFront1; //Т (?)
        String durationFront2;
        String koeff;

        public ImpDifference(string eventId, string date, string difference, string rvp, string ampl1, string ampl2, string duration1, string duration2, string durationFront1, string durationFront2, string koeff)
        {
            this.eventId = eventId;
            this.date = date;
            this.difference = difference;
            this.rvp = rvp;
            this.ampl1 = ampl1;
            this.ampl2 = ampl2;
            this.duration1 = duration1;
            this.duration2 = duration2;
            this.durationFront1 = durationFront1;
            this.durationFront2 = durationFront2;
            this.koeff = koeff;
        }
    }
}
