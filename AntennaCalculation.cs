using Common.MathEx.Algebra;
using GCS.Mathematics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace ImpDistanceCalculation
{
    public class AntennaCalculation
    {
        public int no { get; set; }
        public double id { get; set; }
        public double hwid { get; set; }
        public DateTime date { get; set; }
        public DateTime dateAkaike { get; set; }
        public int pointAkaike { get; set; }
        public double msAkaike { get; set; }
        public int holeName { get; set; }
        public double amplitude { get; set; }
        public double duration { get; set; }
        public double freq { get; set; }
        public long dateTicks { get; set; }
        public Coordinates coordinates { get; set; }
        public Coordinates location0 { get; set; } //локация относительно которой считается
        public double RtoLocation { get; set; } //расстояние до взрыва (X0, ...)
        public double energy { get; set; }

        public AntennaCalculation(int no, double id, double hwid, DateTime date, int holeName, double amplitude, double duration, double freq, Coordinates coordinates, double RtoLocation, double energy)
        {
            this.no = no;
            this.id = id;
            this.hwid = hwid;
            this.date = date;
            this.holeName = holeName;
            this.amplitude = amplitude;
            this.duration = duration;
            this.freq = freq;
            this.coordinates = coordinates;
            this.RtoLocation = RtoLocation;
            this.energy = energy;
        }

        public AntennaCalculation() { }




        //получение массива импульсов и занесение строки в DataGrid
        public static void setEvents(List<DataGridViewRow> rows, DataGridView EventGrid, DateTime dateBefore, DateTime dateAfter, Coordinates location)
        {
            String antennaName = setEventsName(rows);
            int numberOfEvents = 1;
            EventGrid.Columns["DateBefore_Events"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff";
            EventGrid.Columns["DateAfter_Events"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff";
            int lastRowIndex = EventGrid.Rows.Count - 1;
            //for (int i = lastRowIndex; i < numberOfEvents; i++)
            //{
                EventGrid.Rows.Add();
                //EventGrid.Rows[lastRowIndex].Cells["No_Events"].Value = lastRowIndex + 1;
                EventGrid.Rows[lastRowIndex].Cells["Antenna_Events"].Value = antennaName;
                EventGrid.Rows[lastRowIndex].Cells["Imp_Events"].Value = setImpulses(rows);
                EventGrid.Rows[lastRowIndex].Cells["DateBefore_Events"].Value = dateBefore;
                EventGrid.Rows[lastRowIndex].Cells["DateAfter_Events"].Value = dateAfter;
                EventGrid.Rows[lastRowIndex].Cells["LocationX0_Events"].Value = location.x;
                EventGrid.Rows[lastRowIndex].Cells["LocationY0_Events"].Value = location.y;
                EventGrid.Rows[lastRowIndex].Cells["LocationZ0_Events"].Value = location.z;

            //}
        }

        //для класса антенн
        public static AntennaCalculation [] getAntennaImpulses(List<DataGridViewRow> rows)
        {
            int count = rows.Count;
            AntennaCalculation []data = new AntennaCalculation [count]; 
            for (int i = 0; i < count; i++)
            {
                AntennaCalculation antenna = new AntennaCalculation();
                antenna.no = i + 1;
                antenna.id = double.Parse(rows[i].Cells["ID"].Value.ToString());
                antenna.hwid = double.Parse(rows[i].Cells["HWID"].Value.ToString());
                antenna.date = DateTime.Parse(rows[i].Cells["ImpDate_DB"].Value.ToString());
                antenna.dateAkaike = DateTime.Parse(rows[i].Cells["ImpDate_DB_Akaike"].Value.ToString());
                antenna.pointAkaike = int.Parse(rows[i].Cells["pointX_Akaike"].Value.ToString());
                antenna.msAkaike = double.Parse(rows[i].Cells["ms_Akaike"].Value.ToString());
                antenna.holeName = int.Parse(rows[i].Cells["HoleName"].Value.ToString()); ; // имя скважины
                antenna.amplitude = double.Parse(rows[i].Cells["Amplitude"].Value.ToString()); // амплитуда
                antenna.duration = double.Parse(rows[i].Cells["Duration"].Value.ToString());// длительность
                antenna.freq = double.Parse(rows[i].Cells["Freq"].Value.ToString());
                antenna.dateTicks = long.Parse(rows[i].Cells["Date_ticks"].Value.ToString()); // тики

                //координаты скважины
                double X = double.Parse(rows[i].Cells["X"].Value.ToString());
                double Y = double.Parse(rows[i].Cells["Y"].Value.ToString());
                double Z = double.Parse(rows[i].Cells["Z"].Value.ToString());
                antenna.coordinates = new Coordinates(X, Y, Z);
                data[i] = antenna;
            }
            return data;
        }

        //сохранение массива импульсов в событии
        public static String [] setImpulses(List<DataGridViewRow> rows)
        {

            int count = rows.Count;
            String[] data = new String[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = rows[i].Cells["ID"].Value.ToString();
            
            }
            return data;
        }


        //задание имени импульсов (название события)
        public static String setEventsName(List<DataGridViewRow> rows)
        {
            String name = "";
            int count = rows.Count;
            for (int i = 0; i < count-1; i++)
            {
                name+= rows[i].Cells["HoleName"].Value.ToString();
                name += "-";
            }
            name += rows[count - 1].Cells["HoleName"].Value.ToString();
            return name;
        }

        public static String setEventsName(AntennaCalculation[] eventImp)
        {
            String name = "";
            int count = eventImp.Length;
            for (int i = 0; i < count - 1; i++)
            {
                name += eventImp[i].holeName;
                name += "-";
            }
            name += eventImp[count - 1].holeName;
            return name;
        }




        //вычисление и получение массива невязок (вариант с DataGrid)
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

        //вычисление и получение массива невязок (вариант со строками таблицы)
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

        //вычисление и получение массива невязок (вариант с объектом класса)
        public double[] getDT(List<int> indexes, AntennaCalculation[] impEvent, int parametrTime)
        {
            int count = indexes.Count;
            double[] DT = new double[count];
            DT[0] = 0;
            for (int i = 1; i < count; i++)
            {
                if (parametrTime == 1)
                {
                    DT[i] = Math.Abs((impEvent[indexes[0]].dateTicks - impEvent[indexes[i]].dateTicks) / 10);
                }
                else if (parametrTime == 2)
                {
                    double ms0 = impEvent[indexes[0]].msAkaike;
                    double ms_i = impEvent[indexes[i]].msAkaike;
                    long date0 = impEvent[indexes[0]].dateTicks;
                    long date_i = impEvent[indexes[i]].dateTicks;
                    long res0 = date0 - (long)Math.Round(ms0 * TimeSpan.TicksPerMillisecond);
                    long res_i = date_i - (long)Math.Round(ms_i * TimeSpan.TicksPerMillisecond);
                    DT[i] = Math.Abs((res0 - res_i) / 10);
                }
            }
            return DT;
        }

        //получение координат импульсов (вариант с DataGrid)
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

        //получение координат импульсов (вариант со строками табл)
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

        //получение координат импульсов (вариант с объектом класса)
        public Coordinates[] getImpulsesCoordinates(List<int> indexes, AntennaCalculation[] impEvent)
        {
            int count = indexes.Count;
            Coordinates[] coordinates = new Coordinates[count];
            double x, y, z;
            for (int i = 0; i < count; i++)
            {
                x = impEvent[indexes[i]].coordinates.x;
                y = impEvent[indexes[i]].coordinates.y;
                z = impEvent[indexes[i]].coordinates.z;
                coordinates[i] = new Coordinates(x, y, z);
            }
            return coordinates;
        }
        //вычисление энергии (более универсально, поэтому без this)
        public double energyCalc(double area, double R)
        {
            double K = 10000;
            double E = (area * Math.Pow(R, 2)) / Math.Pow(K, 2);
            return E;
        }

        //создание массива выбранных импульсов (антенна) с необходимыми для расчета параметрами (вариант с DataGrid)
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

        //вариант для случая конкретных (четырех) строк (вариант со строками табл)
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

        //вариант для случая конкретных (четырех) строк (вариант с объектом класса)
        public Impulse[] setAntenna(List<int> indexes, AntennaCalculation[] impEvent, int parametrTime)
        {
            int count = indexes.Count;
            Impulse[] antenna = new Impulse[count];
            Coordinates[] coordinates = getImpulsesCoordinates(indexes, impEvent);
            double[] DT = getDT(indexes, impEvent, parametrTime);
            for (int i = 0; i < count; i++)
            {
                double id = impEvent[indexes[i]].id;
                String hwid = impEvent[indexes[i]].hwid.ToString();
                DateTime date1 = default;
                if (parametrTime == 1)//стд время
                {
                    date1 = (DateTime)impEvent[indexes[i]].date;
                }
                else if (parametrTime == 2) //Акаике
                {
                    date1 = (DateTime)impEvent[indexes[i]].dateAkaike;
                }
                string testWithMs = date1.ToString("dd.MM.yyyy HH:mm:ss.fff");
                DateTime date = DateTime.ParseExact(
    testWithMs,
    "dd.MM.yyyy HH:mm:ss.fff",
    CultureInfo.InvariantCulture
);
                String holeName = impEvent[indexes[i]].holeName.ToString();
                double amplitude = impEvent[indexes[i]].amplitude;
                double duration = impEvent[indexes[i]].duration;
                double freq = impEvent[indexes[i]].freq;
                antenna[i] = new Impulse(id, hwid, date, holeName, amplitude, duration, freq, coordinates[i], DT[i]);
            }
            return antenna;
        }

        //универсальный перебор вариантов (k - количество элементов в комбинации (4), n - 
        public static IEnumerable<int[]> GetCombinations(int n, int k)
        {
            if (k <= 0 || k > n) yield break;

            var indices = new int[k];
            for (int i = 0; i < k; i++) indices[i] = i;

            while (true)
            {
                yield return (int[])indices.Clone();

                int pos = k - 1;
                while (pos >= 0 && indices[pos] == n - k + pos) pos--;
                if (pos < 0) yield break;

                indices[pos]++;
                for (int i = pos + 1; i < k; i++)
                    indices[i] = indices[i - 1] + 1;
            }
        }
        /// <summary>
        ///вычисление по комбинациям по 4 элементам
        ///parametrTime - способ вычисления времени импульса
        ///1 - стандарт, 2 - по Акаике
        /// <summary>    
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

        //расчет через объект класса (по 4 элемента)
        public void combinationCalc(AntennaCalculation[] impEvent, DataGridView resultGrid, decimal velocityBefore, decimal velocityAfter, decimal step, Coordinates location, int parametrTime)
        {

            resultGrid.Rows.Clear();
            resultGrid.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff"; //для миллисекунд

            int n = impEvent.Length;
            int s = 0;
            for (int i = 0; i < n - 3; i++)
            {
                for (int j = i + 1; j < n - 2; j++)
                {
                    for (int k = j + 1; k < n - 1; k++)
                    {
                        for (int l = k + 1; l < n; l++)
                        {

                            var indexes = new List<int> { i, j, k, l };
                            /*
                            var selectedRows = indexes
                                .Select(index => impulseGrid.Rows[index])
                                .Where(r => !r.IsNewRow)
                                .ToList();
                            */
                            Impulse[] antenna = setAntenna(indexes, impEvent, parametrTime);
                            DateTime firstImp = DateTime.MinValue;
                            //for (double velocity = velocityBefore; velocity < velocityAfter; velocity += step)
                            decimal velocity = velocityBefore;
                            double Rmin = Double.MaxValue, AE_Xmin = 0, AE_Ymin = 0, AE_Zmin = 0, X0 = 0, Y0 = 0, Z0 = 0;
                            decimal velocityMin = 0;
                            float minTimeError = 0;
                            String antennaName = "";

                            while (velocity <= velocityAfter)
                            {
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
                                    antennaName = antenna[0].holeName + "-" + antenna[1].holeName + "-" + antenna[2].holeName + "-" + antenna[3].holeName;
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

        //расчет через объект класса (универсальный вариант)
        public void combinationCalc(int combintationNumber, AntennaCalculation[] impEvent, DataGridView resultGrid, decimal velocityBefore, decimal velocityAfter, decimal step, Coordinates location, int parametrTime)
        {

            //resultGrid.Rows.Clear();
            resultGrid.Columns[1].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss.fff"; //для миллисекунд

            int n = impEvent.Length;
            int s = 0;
            foreach (int[] idx in GetCombinations(n, combintationNumber))
            {
                var indexes = idx.ToList();
                Impulse[] antenna = setAntenna(indexes, impEvent, parametrTime);
                DateTime firstImp = DateTime.MinValue;
                //for (double velocity = velocityBefore; velocity < velocityAfter; velocity += step)
                decimal velocity = velocityBefore;
                double Rmin = Double.MaxValue, AE_Xmin = 0, AE_Ymin = 0, AE_Zmin = 0, X0 = 0, Y0 = 0, Z0 = 0;
                decimal velocityMin = 0;
                float minTimeError = 0;
                String antennaName = "";

                while (velocity <= velocityAfter)
                {
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
                        antennaName = antenna[0].holeName + "-" + antenna[1].holeName + "-" + antenna[2].holeName + "-" + antenna[3].holeName;
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
