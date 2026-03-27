using System;

//
// в классе CPack нужно использовать функцию
// public static int[] UnPack(byte[] Pvalues_array, int Plength, int version) !!! Вызывать
//        Pvalues_array - сигналограмма или фронт или все вместе
//        Plength       - количество байт в массиве Pvalues_array, которое нужно обработать
//        version       - версия датчика, принимает следующие значения:
//           = 0 для старых датчиков и старых чувствительных элементов
//           = 1 для старых датчиков и новых чувствительных элементов
//           = 2 для новых датчиков
// функция возвращает массив амплитуд
//

namespace TrembleMeasureSystem.Moxa
{
    //#########################################################################################\
    //==========================================================================================\
    #region Класс CPack предоставляет методы упаковки и распаковки чисел
    //==========================================================================================/
    //#########################################################################################/

    public static class CPack
    {
        //#########################################################################################\
        //==========================================================================================\
        //   Константы класса
        //==========================================================================================/
        //#########################################################################################/

        public const bool TEST = false;

        //#########################################################################################\
        //==========================================================================================\
        //   Основные рабочие методы
        //==========================================================================================/
        //#########################################################################################/

        // распаковка одного слова
        public static int UnPack(ushort Pvalue)
        {
            return UnPack(Pvalue, false);
        }
        public static int UnPack(ushort Pvalue, bool is_old_sensor)
        {
            if ((Pvalue & 0x7FF) == 0) return 0;

            int result = Pvalue & 0x00000FFF;
            byte b = (byte)(Pvalue >> 8);
            bool neg = false;
            if ((b & 0x08) > 0)
            {
                result = ((~result) + 1) & 0x00000FFF;
                neg = true;
            }
            /*switch ((b >> 4) & 0x03)
            {
                case 0: result <<= 3; break;
                case 1: result <<= 2; break;
                case 2: result <<= 1; break;
            }*/
            result <<= (3 - ((b >> 4) & 0x03)); // равносильно закомментированному выше
            if ((b & 0x40) > 0)
            {
                // преобразование амплитуды для старых чувствительных элементов
                if (is_old_sensor) result <<= 7;
                else result <<= 5;
            }
            else if (is_old_sensor) result <<= 3; // преобразование амплитуды для старых чувствительных элементов

            if (neg) result = -result;

            return result;
        }
        public static int UnPack(ushort Pvalue, int version)   // !!! В цикле
        {
            if (version < 2)
            {
                if (version == 0) return UnPack(Pvalue, true);
                else return UnPack(Pvalue, false);
            }

            int result = Pvalue & 0x00003FFF;
            byte b = (byte)(Pvalue >> 8);
            bool neg = false;
            if ((b & 0x80) > 0)
            {
                result = ((~result) + 1) & 0x00003FFF;
                neg = true;
            }

            if ((b & 0x40) > 0)
            {
                result <<= 5;
            }

            if (neg) result = -result;

            return result;
        }
        public static ushort Pack(int Pvalue, int version)
        {
            if (version < 2)
            {
                if (version == 0)
                {
                    return Pack(Pvalue, true);
                }
                return Pack(Pvalue, false);
            }
            int num = 0;
            int num2 = 0;
            int num3 = Math.Abs(Pvalue);
            if (num3 <= 0x3fff)
            {
                num2 = 0;
            }
            else if (num3 <= 0x7ffe0)
            {
                num2 = 0x4000;
                num3 = num3 >> 5;
            }
            else
            {
                num2 = 0x4000;
                num3 = 0x3fff;
            }
            if (Pvalue < 0)
            {
                num = 0x8000;
                num3 = ~(num3 - 1) & 0x3fff;
            }
            return (ushort)((num3 | num) | num2);
        }

        // упаковка одного слова
        public static ushort Pack(int Pvalue, bool is_old_sensor)
        {
            int neg = 0;
            int mult = 0;
            int value = Math.Abs(Pvalue);

            if (is_old_sensor) value >>= 3;

            if (value <= 0x7FF)
            {
                mult = 0x03 << 12;
            }
            else if (value <= (0x7FF << 1))
            {
                mult = 0x02 << 12;
                value >>= 1;
            }
            else if (value <= (0x7FF << 2))
            {
                mult = 0x01 << 12;
                value >>= 2;
            }
            else if (value <= (0x7FF << 3))
            {
                mult = 0x00;
                value >>= 3;
            }
            else
            {
                if (is_old_sensor) value >>= 4;
                else value >>= 5;

                if (value <= 0x7FF)
                {
                    mult = (0x03 | 0x04) << 12;
                }
                else if (value <= (0x7FF << 1))
                {
                    mult = (0x02 | 0x04) << 12;
                    value >>= 1;
                }
                else if (value <= (0x7FF << 2))
                {
                    mult = (0x01 | 0x04) << 12;
                    value >>= 2;
                }
                else if (value <= (0x7FF << 3))
                {
                    mult = 0x04 << 12;
                    value >>= 3;
                }
                else
                {
                    mult = 0x04 << 12;
                    value = 0x7FF;
                }
            }

            if (Pvalue < 0)
            {
                neg = 1 << 11;
                value = (~(value - 1)) & 0xFFF;
            }

            ushort result = (ushort)(value | neg | mult);
            return result;
        }

        // распаковка массива
        public static int[] UnPack(byte[] Pvalues_array, int Plength)
        {
            return UnPack(Pvalues_array, Plength, false);
        }
        public static int[] UnPack(byte[] Pvalues_array, int Plength, bool is_old_sensor)
        {
            int version = 1;
            if (is_old_sensor) version = 0;
            return UnPack(Pvalues_array, Plength, version);
        }
        public static int[] UnPack(byte[] Pvalues_array, int Plength, int version)
        {
            int length = Plength;
            if (length > Pvalues_array.Length) length = Pvalues_array.Length;
            if (length <= 0) return null;

            int[] result = new int[length >> 1];

            for (int i = 0, j = 0; i < length - 1; i += 2, j++)
                result[j] = UnPack((ushort)(Pvalues_array[i + 1] + (Pvalues_array[i] << 8)), version);

            return result;
        }
        public static int[] UnPack(ushort[] Pvalues_array, int Plength)
        {
            return UnPack(Pvalues_array, Plength, false);
        }
        public static int[] UnPack(ushort[] Pvalues_array, int Plength, bool is_old_sensor)
        {
            int length = Plength;
            if (length > Pvalues_array.Length) length = Pvalues_array.Length;
            if (length <= 0) return null;

            int[] result = new int[length];
            for (int i = 0; i < length; i++) result[i] = UnPack(Pvalues_array[i], is_old_sensor);
            return result;
        }
        // упаковка массива
        public static byte[] Pack(int[] Pvalues_array, int Plength, bool is_old_sensor)
        {
            int length = Plength;
            if (length > Pvalues_array.Length) length = Pvalues_array.Length;
            if (length <= 0) return null;

            byte[] result = new byte[length << 1];
            int j = 0;
            for (int i = 0; i < length; i++)
            {
                ushort sh = Pack(Pvalues_array[i], is_old_sensor);
                result[j++] = (byte)(sh >> 8);
                result[j++] = (byte)sh;
            }
            return result;
        }
        public static ushort[] PackToUshort(int[] Pvalues_array, int Plength)
        {
            int length = Plength;
            if (length > Pvalues_array.Length) length = Pvalues_array.Length;
            if (length <= 0) return null;

            ushort[] result = new ushort[length];
            for (int i = 0; i < length; i++) result[i] = Pack(Pvalues_array[i], false);
            return result;
        }

        // корректировка площади с учетом типа чувствительного элемента
        public static ulong UnPackArea(ulong Area, int Amplitude, bool is_old_sensor)
        {
            ulong result = Area;

            if (is_old_sensor)
            {
                if (Math.Abs(Amplitude) >= 0x0000FFFF) result <<= 2;
                else result <<= 3;
            }

            return result;
        }
        public static void DeleteArtefact(ref byte[] Pvalues_array, int version, int AmplitudeLimit)
        {
            int length = Pvalues_array.Length;
            if (length > 0)
            {
                int num2 = 0;
                int index = 0;
                while (index < (length - 1))
                {
                    int num4 = UnPack((ushort)(Pvalues_array[index + 1] + (Pvalues_array[index] << 8)), version);
                    if (Math.Abs(num4) > AmplitudeLimit)
                    {
                        int num5 = index + 2;
                        int num6 = num4;
                        int num7 = 0;
                        while (Math.Abs(num6) > AmplitudeLimit)
                        {
                            num7++;
                            if (num5 >= (length - 1))
                            {
                                num6 = 0;
                                break;
                            }
                            num6 = UnPack((ushort)(Pvalues_array[num5 + 1] + (Pvalues_array[num5] << 8)), version);
                            num5 += 2;
                        }
                        int pvalue = 0;
                        int num9 = 0;
                        for (num5 = index; num9 < num7; num5 += 2)
                        {
                            num9++;
                            pvalue = ((num6 - num2) / (num7 + 1)) * num9;
                            ushort num10 = Pack(pvalue, version);
                            Pvalues_array[num5 + 1] = (byte)num10;
                            Pvalues_array[num5] = (byte)(num10 >> 8);
                        }
                        index += num7 << 1;
                    }
                    else
                    {
                        num2 = num4;
                        index += 2;
                    }
                }
            }
        }
        // убираем артефакты (значения которые превышают лимит)
        public static void DeleteArtefact(ref byte[] Pvalues_array, bool is_old_sensor, int AmplitudeLimit)
        {
            int length = Pvalues_array.Length;
            if (length <= 0) return;
            int lastiamp = 0;

            int i = 0;
            while (i < (length - 1))
            {
                int iamp = UnPack((ushort)(Pvalues_array[i + 1] + (Pvalues_array[i] << 8)), is_old_sensor);
                if (Math.Abs(iamp) > AmplitudeLimit)
                {
                    // ищем следующую нормальную точку
                    int i2 = i + 2;
                    int nextiamp = iamp;
                    int count = 0;

                    while (Math.Abs(nextiamp) > AmplitudeLimit)
                    {
                        count++;

                        if (i2 >= (length - 1))
                        {
                            nextiamp = 0;
                            break;
                        }
                        nextiamp = UnPack((ushort)(Pvalues_array[i2 + 1] + (Pvalues_array[i2] << 8)), is_old_sensor);

                        i2 += 2;
                    }

                    // линейная интерполяция для всех точек
                    int new_iamp = 0;
                    int cur = 0;
                    i2 = i;
                    while (cur < count)
                    {
                        cur++;
                        new_iamp = ((nextiamp - lastiamp) / (count + 1)) * cur;

                        ushort u = Pack(new_iamp, is_old_sensor);
                        Pvalues_array[i2 + 1] = (byte)u;
                        Pvalues_array[i2] = (byte)(u >> 8);

                        i2 += 2;
                    }

                    i += count << 1;
                }
                else
                {
                    lastiamp = iamp;
                    i += 2;
                }
            }
        }
    }
    #endregion

}