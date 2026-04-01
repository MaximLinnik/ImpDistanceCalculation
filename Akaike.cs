using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpDistanceCalculation
{
    public class Akaike
    {
        public int xPointAkaike = 0;

        //расчет по самой формуле Акаике
        public double calculationAIC(double[] waveform, double[] XP)
        {
            int n = waveform.Length;

            // формула из двух частей. Они считаются отдельно
            // Префиксные суммы
            double[] prefixSum = new double[n + 1];
            double[] prefixSumSq = new double[n + 1];

            for (int i = 0; i < n; i++)
            {
                prefixSum[i + 1] = prefixSum[i] + waveform[i];
                prefixSumSq[i + 1] = prefixSumSq[i] + waveform[i] * waveform[i];
            }

            double minAIC = double.MaxValue;
            int bestK = -1;

            // k — точка разделения
            for (int k = 1; k < n - 1; k++)
            {
                // --- Левая часть [0, k-1] 
                int len1 = k;
                double sum1 = prefixSum[k] - prefixSum[0];
                double sumSq1 = prefixSumSq[k] - prefixSumSq[0];

                double mean1 = sum1 / len1;
                double var1 = (sumSq1 / len1) - (mean1 * mean1);

                // --- Правая часть [k, n-1]
                int len2 = n - k;
                double sum2 = prefixSum[n] - prefixSum[k];
                double sumSq2 = prefixSumSq[n] - prefixSumSq[k];

                double mean2 = sum2 / len2;
                double var2 = (sumSq2 / len2) - (mean2 * mean2);

                // защита от log(0)
                if (var1 <= 0) var1 = 1e-12;
                if (var2 <= 0) var2 = 1e-12;

                double aic = len1 * Math.Log(var1) + len2 * Math.Log(var2);

                if (minAIC > aic)
                {
                    minAIC = aic;
                    bestK = k;
                }
            }

            double time = XP[128 - bestK]; //результирующее значение, от которого отнимается
            this.xPointAkaike = bestK;
            //this.xPointAkaike = bestK;
            return time;
        }

        //полное вычисление по алгоритму Акаике
        public double AIC(String connectionString, String impulseID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            byte[] rawData = Impulse.frontData(con, impulseID);
            double[] waveform = Impulse.UnpackSignal(rawData);
            double[] xp = Impulse.getTimeX(rawData);
            double time = calculationAIC(waveform, xp);
            return time;
        }
    }
}
