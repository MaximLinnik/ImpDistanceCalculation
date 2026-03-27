using System;


namespace GCS.Classes.Impulses
{
 

    public static class CSpectr
    {
        public static uint count_of_points_in_s = 40000;
        public static int FreqLength = 0x800;
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
            if (Freq == 0) Freq  = 100;
            return Freq;
        }
        public static double Freq(byte[] mData, int mHWID, long  mImpulseTime, int Duration, int Amplitude)
        {            
            int FreqLength = (int)Math.Round((double)((10.0 * GCS.Classes.Impulses.CSpectr.count_of_points_in_s) / 1000.0));
            return Freq(mData, mHWID, mImpulseTime, Duration, Amplitude, true, FreqLength);
        }
        public static double FreqEnd(byte[] mData, int mHWID, long mImpulseTime, int Duration, int Amplitude)
        {

            int FreqLength = (int)Math.Round((double)((5.0 * GCS.Classes.Impulses.CSpectr.count_of_points_in_s) / 1000.0));
            return Freq(mData, mHWID, mImpulseTime, Duration, Amplitude, false, FreqLength);
        }
        private static double Freq(byte[] mData, int mHWID, long mImpulseTime, int Duration, int Amplitude, bool isstart, int FreqLength)
        {
            if (Duration < 0)
                Duration = (int)(Duration + ushort.MaxValue + 1);
            int Length = mData.Length / 2;
            int mPackVersion = formImpulses.GetHWIDVersion(mHWID, mImpulseTime);

            double[] XP = new double[Length];
            int[] YPint = TrembleMeasureSystem.Moxa.CPack.UnPack(mData, mData.Length, mPackVersion);
            if (isstart) Array.Resize<int>(ref YPint, Duration);
            TrembleMeasureSystem.Moxa.CPack.DeleteArtefact(ref mData, mPackVersion, Amplitude);

            int[] data;
            double Freq = 0;
            if (YPint.Length < FreqLength)
            {
                data = GCS.Classes.Impulses.CSpectr.ExpMovingAverage(YPint, YPint.Length, 5, isstart);
                Freq = Math.Round(GCS.Classes.Impulses.CSpectr.Freq(data, YPint.Length, isstart), 2);
            }
            else
            {
                data = GCS.Classes.Impulses.CSpectr.ExpMovingAverage(YPint, FreqLength, 5, isstart);
                Freq = Math.Round(GCS.Classes.Impulses.CSpectr.Freq(data, FreqLength, isstart), 2);
            }
            if (Freq == 0) Freq = 100;
            return Freq;
        }
    }
}
