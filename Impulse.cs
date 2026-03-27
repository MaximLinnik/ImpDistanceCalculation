using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpDistanceCalculation
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
        public double DTd;
        public Coordinates coordinates;
        public double Ri;

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


        public static short FrontLength = 128 * 2;
        public static short ImpulseLength = 2048 * 2;
        public static uint count_of_points_in_s = 40000;


        public static double CalcFrequencyNew(SqlConnection con, String id)
        {
            String query = @"select Impulses.HWID, Impulses.ImpulseTime, Impulses.Duration, 
                                    Impulses.Amplitude,  Fronts.Data, ImpulsesData.Data
                             from Impulses, Fronts, ImpulsesData
                             where    
							 Fronts.ID = Impulses.FrontID 
							 AND ImpulsesData.ID = Impulses.ImpulseDataID
							 AND Impulses.ID = " + id;
            SqlCommand command = new SqlCommand(query, con);
            String HWID = "", impulseTime = "", duration = "", amplitude = "";
            byte[] DI = new byte[FrontLength + ImpulseLength];
            con.Open();
            SqlDataReader reader = command.ExecuteReader();
            double res = 0;
            while (reader.Read())
            {
                HWID = reader[0].ToString();
                impulseTime = reader[1].ToString();
                duration = reader[2].ToString();
                amplitude = reader[3].ToString();
                Array.Copy((byte[])reader[4], DI, FrontLength);
                Array.Copy((byte[])reader[5], 0, DI, FrontLength, ImpulseLength);
            }
            //DataRowView row = ImpulsesBindingSource[imp] as DataRowView;
            //DataRow impulseData = dtImpulseData.Rows[imp];//обе таблицы отсортированы по ID
            if (HWID == "" || impulseTime == "" || duration == "" || amplitude == "")
                res = -1;
            else
                res = Math.Round(Freq(DI, Convert.ToInt32(HWID), Convert.ToInt64(impulseTime), Convert.ToInt32(duration), Convert.ToInt32(amplitude)) / 1000, 3);
            //MessageBox.Show(res.ToString());
            con.Close();
            //}
            return res;
        }
        /*
        public static double Freq(byte[] mData, int mHWID, long mImpulseTime, int Duration, int Amplitude)
        {

            //int FreqLength = (int)Math.Round((double)((10.0 * GCS.Classes.Impulses.CSpectr.count_of_points_in_s) / 1000.0));
            int FreqLength = (int)Math.Round((double)((10.0 * count_of_points_in_s) / 1000.0));
            return Freq(mData, mHWID, mImpulseTime, Duration, Amplitude, true, FreqLength);
        }


        private static double Freq(byte[] mData, int mHWID, long mImpulseTime, int Duration, int Amplitude, bool isstart, int FreqLength)
        {
            if (Duration < 0)
                Duration = (int)(Duration + ushort.MaxValue + 1);
            int Length = mData.Length / 2;
            //int mPackVersion = formImpulses.GetHWIDVersion(mHWID, mImpulseTime);
            int mPackVersion = GetHWIDVersion(mHWID, mImpulseTime);

            double[] XP = new double[Length];
            //int[] YPint = TrembleMeasureSystem.Moxa.CPack.UnPack(mData, mData.Length, mPackVersion);
            int[] YPint = TrembleMeasureSystem.Moxa.CPack.UnPack(mData, mData.Length, mPackVersion);
            if (isstart) Array.Resize<int>(ref YPint, Duration);
            //TrembleMeasureSystem.Moxa.CPack.DeleteArtefact(ref mData, mPackVersion, Amplitude);
            TrembleMeasureSystem.Moxa.CPack.DeleteArtefact(ref mData, mPackVersion, Amplitude);

            int[] data;
            double Freq = 0;
            if (YPint.Length < FreqLength)
            {

                data = ExpMovingAverage(YPint, YPint.Length, 5 , isstart);
                //Freq = Math.Round(GCS.Classes.Impulses.CSpectr.Freq(data, YPint.Length, isstart), 2);
                //Freq = Math.Round(Freq(data, YPint.Length, isstart), 2);
                Freq = Math.Round(Freq1(data, YPint.Length, isstart), 2);
            }
            else
            {
                data = ExpMovingAverage(YPint, FreqLength, 5, isstart);
                //Freq = Math.Round(GCS.Classes.Impulses.CSpectr.Freq(data, FreqLength, isstart), 2);
                Freq = Math.Round(Freq1(data, FreqLength, isstart), 2);
            }
            if (Freq == 0) Freq = 100;
            return Freq;
        }
    */
        public static double Freq(int[] data, int len, bool fromStart)
        {
            if ((data == null) || (data.Length <= 0))
            {
                return 100;
            }
            int length = data.Length;
            if ((fromStart && (len > 0) && (length > len))
                || (!fromStart && (len > length)))
            {
                length = len;
            }
            double L = -1;
            int N = 0;
            if (fromStart)
            {
                L = length;
                for (int i = 1; i < length; i++)
                {
                    if ((data[i] >= 0) ^ (data[i - 1] >= 0))
                    {
                        N++;
                    }
                }
            }
            else
            {
                L = len;
                for (int i = data.Length - 1; i > data.Length - len; i--)
                {
                    if ((data[i] >= 0) ^ (data[i - 1] >= 0))
                    {
                        N++;
                    }
                }
            }
            double Freq = (((double)((N >> 1) * count_of_points_in_s)) / L);
            if (Freq == 0) Freq = 100;
            return Freq;
        }

        public static double Freq(byte[] mData, int mHWID, long mImpulseTime, int Duration, int Amplitude)
        {
            int FreqLength = (int)Math.Round((double)((10.0 * count_of_points_in_s) / 1000.0));
            return Freq(mData, mHWID, mImpulseTime, Duration, Amplitude, true, FreqLength);
        }

        private static double Freq(byte[] mData, int mHWID, long mImpulseTime, int Duration, int Amplitude, bool isstart, int FreqLength)
        {
            if (Duration < 0)
                Duration = (int)(Duration + ushort.MaxValue + 1);
            int Length = mData.Length / 2;
            int mPackVersion = GetHWIDVersion(mHWID, mImpulseTime);

            double[] XP = new double[Length];
            int[] YPint = TrembleMeasureSystem.Moxa.CPack.UnPack(mData, mData.Length, mPackVersion);
            if (isstart) Array.Resize<int>(ref YPint, Duration);
            TrembleMeasureSystem.Moxa.CPack.DeleteArtefact(ref mData, mPackVersion, Amplitude);

            int[] data;
            double freq = 0;
            if (YPint.Length < FreqLength)
            {
                data = ExpMovingAverage(YPint, YPint.Length, 5, isstart);
                freq = Math.Round(Freq(data, YPint.Length, isstart), 2);
            }
            else
            {
                data = ExpMovingAverage(YPint, FreqLength, 5, isstart);
                freq = Math.Round(Freq(data, FreqLength, isstart), 2);
            }
            if (freq == 0) freq = 100;
            return freq;
        }


        private static int[] ExpMovingAverage(int[] data, int len, int n, bool fromStart)
        {
            if (((data == null) || (data.Length <= 0)) || (n < 1))
            {
                return null;
            }
            int length = data.Length;
            if ((fromStart && (len > 0) && (length > len))
                || (!fromStart && (len > length)))
            {
                length = len;
            }
            int[] numArray = new int[length];
            double num2 = 2.0 / ((double)(n + 1));
            numArray[0] = data[0];
            for (int i = 1; i < length; i++)
            {
                numArray[i] = (int)Math.Round((double)((num2 * data[i]) + ((1.0 - num2) * numArray[i - 1])));
            }
            return numArray;
        }

        //public static double Freq(int[] data, int len, bool fromStart)
        public static double Freq1(int[] data, int len, bool fromStart)
        {
            if ((data == null) || (data.Length <= 0))
            {
                return 100;
            }
            int length = data.Length;
            if ((fromStart && (len > 0) && (length > len))
                || (!fromStart && (len > length)))
            {
                length = len;
            }
            double L = -1;
            int N = 0;
            if (fromStart)
            {
                L = length;
                for (int i = 1; i < length; i++)
                {
                    if ((data[i] >= 0) ^ (data[i - 1] >= 0))
                    {
                        N++;
                    }
                }
            }
            else
            {
                L = len;
                for (int i = data.Length - 1; i > data.Length - len; i--)
                {
                    if ((data[i] >= 0) ^ (data[i - 1] >= 0))
                    {
                        N++;
                    }
                }
            }
            double Freq = (((double)((N >> 1) * count_of_points_in_s)) / L);
            if (Freq == 0) Freq = 100;
            return Freq;
        }

        static public int GetHWIDVersion(int mHWID, long ImpulseTime)
        {
            int mPackVersion;
            /*
            if ((mHWID / 256) <= 1) // Старый датчик 
            {
                // Анализируем колонку "OldSensor" таблицы "Holes" !
                string cnString = global::GCS.Properties.Settings.Default.GCSConnectionString;
                SqlConnection cn = new SqlConnection(cnString);
                SqlCommand cmd = cn.CreateCommand();
                cmd.CommandTimeout = GlbDefs.MySettings.Timeout;

                cmd.Parameters.Add("TicKsTime", System.Data.SqlDbType.BigInt);
                cmd.Parameters["TicKsTime"].Value = ImpulseTime;
                cmd.Parameters.Add("HWID", System.Data.SqlDbType.Int);
                cmd.Parameters["HWID"].Value = mHWID;

                cmd.CommandText = "spOldSensor";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = GlbDefs.MySettings.Timeout;

                try
                {
                    cn.Open();
                    if (cmd.ExecuteNonQuery() == 1 && (bool)cmd.ExecuteScalar())
                    {
                        mPackVersion = 0;
                    }
                    else
                    {
                        mPackVersion = 1;
                    }
                }
                catch (Exception ex)
                {
                    mPackVersion = 2;
                    MessageBox.Show("Произошла следующая ошибка при определении версии датчика:\n" + ex.Message +
                                    "\nСчитаем что версия датчика = 2",
                                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    
                    if (GlbDefs.MySettings.EventLogWriteEntry == 1)
                    {
                        EventLog.WriteEntry(Application.ProductName,
                                            "Произошла следующая ошибка при определении версии датчика:\n" + ex.Message,
                                            EventLogEntryType.Error,
                                            (int)GlbDefs.eventID.Error,
                                            (int)GlbDefs.eCategory.ConnectionError);
                                            
                    }
                }
                finally
                {
                    if (cn.State == ConnectionState.Open) cn.Close();
                }
            }
            */
            //else // Новый датчик
            // {
            mPackVersion = 2;
            //}
            return mPackVersion;
        }
    }


}
