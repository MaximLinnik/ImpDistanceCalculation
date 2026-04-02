using Common.MathEx.Algebra;
using GCS.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace ImpDistanceCalculation
{
    class AntennaCalculation
    {


        //вычисление и получение массива невязок
        public double[] getDT(DataGridView impulseGrid)
        {
            int count = impulseGrid.RowCount - 1;
            double[] DT = new double[count];
            DT[0] = 0;
            for(int i = 1; i < count; i++)
            {
                DT[i] = Math.Abs((long.Parse(impulseGrid.Rows[0].Cells["data_Date_Ticks"].Value.ToString()) - long.Parse(impulseGrid.Rows[i].Cells["data_Date_Ticks"].Value.ToString()))/10);
            }
            return DT;
        }

        public double[] getDT(List<DataGridViewRow> rows, int parametrTime)
        {
            int count = rows.Count;
            double[] DT = new double[count];
            DT[0] = 0;
            for (int i = 1; i < count; i++)
            {
                if (parametrTime == 1)
                {
                    DT[i] = Math.Abs((long.Parse(rows[0].Cells["data_Date_Ticks"].Value.ToString()) - long.Parse(rows[i].Cells["data_Date_Ticks"].Value.ToString())) / 10);
                }
                else if (parametrTime == 2)
                {
                    double ms0 = double.Parse(rows[0].Cells["data_ms_Akaike"].Value.ToString());
                    double ms_i = double.Parse(rows[i].Cells["data_ms_Akaike"].Value.ToString());
                    long date0 = long.Parse(rows[0].Cells["data_Date_Ticks"].Value.ToString());
                    long date_i = long.Parse(rows[i].Cells["data_Date_Ticks"].Value.ToString());
                    long res0 = date0 - (long)Math.Round(ms0 * TimeSpan.TicksPerMillisecond);
                    long res_i = date_i - (long)Math.Round(ms_i * TimeSpan.TicksPerMillisecond);
                    DT[i] = Math.Abs((res0 - res_i) / 10);
                }
             }
            return DT;
        }

        //получение координат импульсов по скважинам
        public Coordinates[] getImpulsesCoordinates(DataGridView impulseGrid)
        {
            int count = impulseGrid.RowCount - 1;
            Coordinates[] coordinates = new Coordinates[count];
            double x, y, z;
            for (int i = 0; i < count; i++)
            {
                x = Double.Parse(impulseGrid.Rows[i].Cells["data_X"].Value.ToString());
                y = Double.Parse(impulseGrid.Rows[i].Cells["data_Y"].Value.ToString());
                z = Double.Parse(impulseGrid.Rows[i].Cells["data_Z"].Value.ToString());
                coordinates[i] = new Coordinates(x, y, z);
            }
            return coordinates;
        }

        public Coordinates[] getImpulsesCoordinates(List<DataGridViewRow> rows)
        {
            int count = rows.Count;
            Coordinates[] coordinates = new Coordinates[count];
            double x, y, z;
            for (int i = 0; i < count; i++)
            {
                x = Double.Parse(rows[i].Cells["data_X"].Value.ToString());
                y = Double.Parse(rows[i].Cells["data_Y"].Value.ToString());
                z = Double.Parse(rows[i].Cells["data_Z"].Value.ToString());
                coordinates[i] = new Coordinates(x, y, z);
            }
            return coordinates;
        }

        //создание массива выбранных импульсов (антенна) с необходимыми для расчета параметрами
        public Impulse[] setAntenna(DataGridView impulseGrid, int parametrTime)
        {
            int count = impulseGrid.RowCount - 1;
            Impulse[] antenna = new Impulse[count];
            Coordinates[] coordinates = getImpulsesCoordinates(impulseGrid);
            double[] DT = getDT(impulseGrid);
            for (int i = 0; i < count; i++)
            {

                double id = Double.Parse(impulseGrid.Rows[i].Cells["data_ID"].Value.ToString());
                String hwid = impulseGrid.Rows[i].Cells["data_HWID"].Value.ToString();
                DateTime date = default;
                if (parametrTime == 1)//стд время
                {
                    date = DateTime.Parse(impulseGrid.Rows[i].Cells["data_ImpDate_DB"].Value.ToString());
                }
                else if (parametrTime == 2) //Акаике
                {
                    date = DateTime.Parse(impulseGrid.Rows[i].Cells["ImpDate_DB_Akaike"].Value.ToString());
                }
                String holeName = impulseGrid.Rows[i].Cells["data_HoleName"].Value.ToString();
                double amplitude = Double.Parse(impulseGrid.Rows[i].Cells["data_Amplitude"].Value.ToString());
                double duration = Double.Parse(impulseGrid.Rows[i].Cells["data_Duration"].Value.ToString());
                double freq = Double.Parse(impulseGrid.Rows[i].Cells["data_Freq"].Value.ToString());
                antenna[i] = new Impulse(id, hwid, date, holeName, amplitude, duration, freq, coordinates[i], DT[i]);
            }
            return antenna;
        }

        //вариант для случая конкретных (четырех) строк
        public Impulse[] setAntenna(List<DataGridViewRow> rows, int parametrTime)
        {
            int count = rows.Count;
            Impulse[] antenna = new Impulse[count];
            Coordinates[] coordinates = getImpulsesCoordinates(rows);
            double[] DT = getDT(rows, parametrTime);
            for (int i = 0; i < count; i++)
            {
                double id = Double.Parse(rows[i].Cells["data_ID"].Value.ToString());
                String hwid = rows[i].Cells["data_HWID"].Value.ToString();
                DateTime date1 = default;
                if (parametrTime == 1)//стд время
                {
                    date1 = (DateTime)rows[i].Cells["data_ImpDate_DB"].Value;
                }
                else if (parametrTime == 2) //Акаике
                {
                    date1 = (DateTime)rows[i].Cells["data_ImpDate_DB_Akaike"].Value;
                }
                string testWithMs = date1.ToString("dd.MM.yyyy HH:mm:ss.fff");
                DateTime date = DateTime.ParseExact(
    testWithMs,
    "dd.MM.yyyy HH:mm:ss.fff",
    CultureInfo.InvariantCulture
);
                String holeName = rows[i].Cells["data_HoleName"].Value.ToString();
                double amplitude = Double.Parse(rows[i].Cells["data_Amplitude"].Value.ToString());
                double duration = Double.Parse(rows[i].Cells["data_Duration"].Value.ToString());
                double freq = Double.Parse(rows[i].Cells["data_Freq"].Value.ToString());
                antenna[i] = new Impulse(id, hwid, date, holeName, amplitude, duration, freq, coordinates[i], DT[i]);
            }
            return antenna;
        }

        //вычисление по комбинациям по 4 элемента
        //parametrTime - способ вычисления времени импульса
        //1 - стандарт, 2 - по Акаике
        public void combinationCalc(DataGridView impulseGrid, DataGridView resultGrid, decimal velocityBefore, decimal velocityAfter, decimal step, Coordinates location, int parametrTime)
        {

            resultGrid.Rows.Clear();
            resultGrid.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff"; //для миллисекунд

            int n = impulseGrid.RowCount - 1;
            int s = 0;
            //int count = (int)((velocityAfter - velocityBefore) / step); //чтобы не было моментов типа 0000000000.1
            //int before = (int)(velocityBefore * 10);         // 5000 → 50000
            //int after  = (int)(velocityAfter * 10);        // 5010 → 50100
            DateTime firstImp = DateTime.MinValue;
            for (int i = 0; i < n - 3; i++)
            {
                for (int j = i + 1; j < n - 2; j++)
                {
                    for (int k = j + 1; k < n - 1; k++)
                    {
                        for (int l = k + 1; l < n; l++)
                        {
                            var indexes = new List<int> { i, j, k, l };
                            var selectedRows = indexes
                                .Select(index => impulseGrid.Rows[index])
                                .Where(r => !r.IsNewRow)
                                .ToList();
                            Impulse[] antenna = setAntenna(selectedRows, parametrTime);
                            //for (double velocity = velocityBefore; velocity < velocityAfter; velocity += step)
                            decimal velocity = velocityBefore;
                            double Rmin = Double.MaxValue, AE_Xmin = 0, AE_Ymin = 0, AE_Zmin = 0, X0 = 0, Y0 = 0, Z0 = 0;
                            decimal velocityMin = 0;
                            float minTimeError = 0;
                            String antennaName = "";
                            //while (velocity <= velocityAfter)
                            //while (s <= count)
                            //while (before <= after)
                            while (velocity <= velocityAfter)
                            {
                                //velocity = velocityBefore + k * step; //чтобы не было моментов типа 0000000000.1
                                //velocity = before/10; //чтобы не было моментов типа 0000000000.1
                                //алг 30
                                Coordinates AE = Algoritm30.getAECoordinates(antenna, (double)velocity);
                                Algoritm30.DirectData(false, antenna, AE, (double)velocity);
                                float TimeError = Algoritm30.TimeErrorClassic(antenna);
                                double R = deltaR(location, AE);
                                if (R < Rmin)
                                {
                                    firstImp = antenna[0].date;
                                    Rmin = R;
                                    AE_Xmin = AE.x;
                                    AE_Ymin = AE.y;
                                    AE_Zmin = AE.z;
                                    X0 = location.x;
                                    Y0 = location.y;
                                    Z0 = location.z;
                                    velocityMin = velocity;
                                    minTimeError = TimeError;
                                    antennaName = antenna[0].holeName+"-"+ antenna[1].holeName + "-" + antenna[2].holeName + "-" + antenna[3].holeName;
                                }
                                //velocity += step;
                                //s++; //чтобы не было моментов типа 0000000000.1
                                velocity += step;
                            }
                            //сохранение названия антенны, значений скорости, Rмин, координат AE
                            if (antennaName != "")
                            {
                                double RtoX0 = deltaR(location, antenna[0].coordinates);
                                double freqGeom = avgFreq(antenna);
                                double avgDistance = avgDeltaR(antenna, location);
                                resultGrid.Rows.Add(antennaName, firstImp, velocityMin, minTimeError, Rmin, AE_Xmin, AE_Ymin, AE_Zmin, X0, Y0, Z0, RtoX0, freqGeom, avgDistance);
                            }
                        }
                    }
                }
            }
        }

        //вычисление расстояния между вычисленными координатами и искомыми 
        public double deltaR(Coordinates location, Coordinates AE)
        {
            if (AE == null)
            {
                return Double.MaxValue;
            }
            else
            {
                double R = Math.Sqrt(Math.Pow(location.x - AE.x, 2) + Math.Pow(location.y - AE.y, 2) + Math.Pow(location.z - AE.z, 2));
                return R;
            }
        }

        //среднее расстояние до датчиков
        public double avgDeltaR(Impulse[] antenna, Coordinates location)
        {
            double R = 0;
            for (int i = 0; i < antenna.Length; i++)
            {
                R += Math.Sqrt(Math.Pow(location.x - antenna[i].coordinates.x, 2) + Math.Pow(location.y - antenna[i].coordinates.y, 2) + Math.Pow(location.z - antenna[i].coordinates.z, 2));
            }

            double avg = R / antenna.Length;
            return avg;
        }


        //вычисление среднегеометрического значения частоты
        public double avgFreq(Impulse[] antenna)
        {
            double avg = 0;
            double length = antenna.Length;
            double freqMult = 1;
            for (int i = 0; i < length; i++)
            {
                freqMult *= antenna[i].freq;
            }
            avg = Math.Pow(freqMult, 1.0 / length);
            return avg;
        }
    }
}
