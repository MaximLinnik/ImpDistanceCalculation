using Common.MathEx.Algebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImpHoleCalculation
{
    class Alg30
    {


        //вычисление и получение массива невязок
        public double[] getDT(DataGridView impulseGrid)
        {
            int count = impulseGrid.RowCount - 1;
            double[] DT = new double[count];
            DT[0] = 0;
            for(int i = 1; i < count; i++)
            {
                DT[i] = Math.Abs((long.Parse(impulseGrid.Rows[0].Cells[7].Value.ToString()) - long.Parse(impulseGrid.Rows[i].Cells[7].Value.ToString()))/10);
            }
            return DT;
        }

        //получение координат импульсов по скважинам
        public Coordinates [] getImpulsesCoordinates(DataGridView impulseGrid)
        {
            int count = impulseGrid.RowCount - 1;
            Coordinates []coordinates = new Coordinates [count];
            double x, y, z;
            for(int i = 0; i<count; i++)
            {
                x = Double.Parse(impulseGrid.Rows[i].Cells[10].Value.ToString());
                y = Double.Parse(impulseGrid.Rows[i].Cells[11].Value.ToString());
                z = Double.Parse(impulseGrid.Rows[i].Cells[12].Value.ToString());
                coordinates[i] = new Coordinates(x, y, z);
            }
            return coordinates;
        }

        //создание массива выбранных импульсов (антенна) с необходимыми для расчета параметрами
        public Impulse[] setAntenna(DataGridView impulseGrid)
        {
            int count = impulseGrid.RowCount - 1;
            Impulse[] antenna = new Impulse[count];
            Coordinates[] coordinates = getImpulsesCoordinates(impulseGrid);
            double[] DT = getDT(impulseGrid);


            for (int i = 0; i< count; i++)
            {

                double id = Double.Parse(impulseGrid.Rows[i].Cells[1].Value.ToString());
                String hwid = impulseGrid.Rows[i].Cells[2].Value.ToString();
                DateTime date = DateTime.Parse(impulseGrid.Rows[i].Cells[3].Value.ToString());
                String holeName = impulseGrid.Rows[i].Cells[4].Value.ToString();
                double amplitude = Double.Parse(impulseGrid.Rows[i].Cells[5].Value.ToString());
                double duration = Double.Parse(impulseGrid.Rows[i].Cells[6].Value.ToString());
                antenna[i] = new Impulse(id, hwid, date, holeName, amplitude, duration, coordinates [i], DT [i]);
            }
            return antenna;
        }

        //вычисление AE_X, AE_Y, AE_Z (из Calc30 -> CalcAlgorithm30main)
        public Coordinates getAECoordinates(Impulse[] antenna, double velocity)
        {
            double AE_X = 0, AE_Y = 0, AE_Z = 0;
            byte NumberOfDecisions = 0; //?????????
            byte DecisionNumber = 1; //?????????????????
            //int AntennaLenght = GlbDefs.allHoles.AntennaLenght - 1; // Длина антенны, уменьшенная на 1
            int AntennaLenght = antenna.Length - 1; // Длина антенны, уменьшенная на 1
            Matrix matrix1 = new Matrix(AntennaLenght, 3);
            Matrix matrix1T, matrix33a, matrix33b, right1, right2, decision1, decision2;
            Matrix vector1 = new Matrix(AntennaLenght, 1);
            Matrix vector2 = new Matrix(AntennaLenght, 1);

            // фоpмиpование матpицы. Объект allHoles уже отсортирован по РВП -> см. "EnterByAntennaName"
            for (int i = 0; i < AntennaLenght; i++)
            {
                matrix1[i, 0] = antenna[i + 1].coordinates.x - antenna[0].coordinates.x;
                matrix1[i, 1] = antenna[i + 1].coordinates.y - antenna[0].coordinates.y;
                matrix1[i, 2] = antenna[i + 1].coordinates.z - antenna[0].coordinates.z;
            }

            // фоpмиpование вектоpов 1 и 2 пpавой части
            for (int i = 0; i < AntennaLenght; i++)
            {
                vector1[i, 0] = (Math.Pow(antenna[i + 1].coordinates.x - antenna[0].coordinates.x, 2) +
                                 Math.Pow(antenna[i + 1].coordinates.y - antenna[0].coordinates.y, 2) +
                                 Math.Pow(antenna[i + 1].coordinates.z - antenna[0].coordinates.z, 2) -
                                 Math.Pow(velocity *
                                          (double)antenna[i + 1].DT / 1000000, 2)) / 2;
                vector2[i, 0] = -velocity * (double)antenna[i + 1].DT / 1000000;
            }

            // Фоpмиpование тpанспониpованной матpицы
            matrix1T = matrix1.MatrixTransform();

            // Умножение тpанспониpованной матpицы на матpицу т.е. фоpмиpование матpицы СЛАУ и создание копии
            // получившейся матрицы (т.к. метод Гаусса ИЗМЕНЯЕТ исходную матрицу -> приводит к диагональному виду)!
            // matrix33a - для решения с Правой частью 1;  matrix33b - для решения с Правой частью 2;
            matrix33a = matrix1T.MatrixMatrixMultiply(matrix1);
            matrix33b = matrix33a.MatrixCopy();

            // Умножение тpанспониpованной матpицы на 1-ый вектоp пpавой части - фоpмиpование пpавой части 1
            // Копии правых частей можно не изменять ...
            right1 = matrix1T.MatrixMatrixMultiply(vector1);

            // Умножение тpанспониpованной матpицы на 2-ой вектоp пpавой части - фоpмиpование пpавой части 2
            right2 = matrix1T.MatrixMatrixMultiply(vector2);

            // Решение линейной части уpавнений
            // pешение 1 / подчасть 1
            decision1 = Matrix.Gause(matrix33a, right1);
            if (decision1 == null)
            {
                MessageBox.Show("Ошибка решения СЛАУ");
                return null;
            }


            // pешение 2 / подчасть 2, умножаемая на R
            decision2 = Matrix.Gause(matrix33b, right2);
            if (decision2 == null)
            {
                MessageBox.Show("Ошибка решения СЛАУ");
                return null;
            }

            // Решение HЕлинейной части уpавнений / опpеделение R
            double a, b, c, D;
            a = Math.Pow(decision2[0, 0], 2) + Math.Pow(decision2[1, 0], 2) + Math.Pow(decision2[2, 0], 2) - 1;
            b = 2 * (decision1[0, 0] * decision2[0, 0] + decision1[1, 0] * decision2[1, 0] +
                     decision1[2, 0] * decision2[2, 0]);
            c = Math.Pow(decision1[0, 0], 2) + Math.Pow(decision1[1, 0], 2) + Math.Pow(decision1[2, 0], 2);
            // Решение уравнения
            double R, R1, R2; R2 = R1 = R = 0;

            if (a == 0 && b != 0)
            {
                R = -c / b;
                NumberOfDecisions = 1;
            }
            else
            {
                D = Math.Pow(b, 2) - 4 * a * c;



                if (D < 0)
                {

                    MessageBox.Show("Дискриминант квадратного уравнения < 0. Решение отсутствует! ");
                    return null;
                }
                else if (D == 0)
                {
                    try
                    {
                        R = -b / (2 * a);
                        NumberOfDecisions = 1;
                    }
                    catch
                    {
                        MessageBox.Show("Дискриминант квадратного уравнения = 0. Ошибка деления на a!");
                        return null;

                    }
                }
                else
                {
                    NumberOfDecisions = 2;  // D > 0
                    try
                    {
                        R1 = (-b - Math.Sqrt(D)) / (2 * a);
                        R2 = (-b + Math.Sqrt(D)) / (2 * a);

                        if (DecisionNumber == 1)
                        {
                            R = R1;
                        }
                        else if (DecisionNumber == 2)
                        {
                            R = R2;
                        }
                        else
                        {
                            MessageBox.Show("Задан неверный номер решения!");
                            return null;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Дискриминант квадратного уравнения > 0. Ошибка деления на a!");
                        return null;
                    }
                }
            }

            // Фоpмиpование вектоpа pешения
            AE_X = Math.Round(decision1[0, 0] + R * decision2[0, 0] +
                                    antenna[0].coordinates.x, 3);
            AE_Y = Math.Round(decision1[1, 0] + R * decision2[1, 0] +
                                    antenna[0].coordinates.y, 3);
            AE_Z = Math.Round(decision1[2, 0] + R * decision2[2, 0] +
                                    antenna[0].coordinates.z, 3);
            /*














           

            // Вычисление РВП от полученного решения (обратный рассчет)
            // Вычисляем РВП всегда (независимо от "GlbVars.MySettings.UseSphereModelRestriction")
            GlbDefs.allHoles.DirectData(false);

            // Вычисление Невязки
            GlbDefs.allHoles.TimeError = GlbDefs.allHoles.TimeErrorClassic();

            if (GlbDefs.MySettings.WriteProtocol == 1)
            {
                strProtocol += String.Format("X: {0,10:F2};  ", GlbDefs.allHoles.AE_X);
                strProtocol += String.Format("Y: {0,10:F2};  ", GlbDefs.allHoles.AE_Y);
                strProtocol += String.Format("Z: {0,10:F2};  ", GlbDefs.allHoles.AE_Z);

                strProtocol += String.Format("Невязка (мкс.): {0,8:F8};  ", GlbDefs.allHoles.TimeError);
            }
            // Вычисление Энергии
            GlbDefs.allHoles.calcEnergy(EnergyParamsType);

            // Время окончания расчета
            endTime = DateTime.Now;
*/

            Coordinates AE = new Coordinates(AE_X, AE_Y, AE_Z);
            return AE;

        }

        //вычисление итогового алгоритма 30


        //

        /*
        public List<string> debugArray;

        public int Lenght;              // Длина массива скважин
        public Hole[] HolesArray;       // Массив скважин       

        public string AntennaName;      // Имя антенны (по порядку прихода сигналов на скважины).
                                        // Состоит из имен скважин, разделенных символом '-'.
        public int AntennaLenght;       // Кол-во скважин в антенне (длина антенны) (либо количество используемых скважин)
        public int RealAntennaLenght;   // Кол-во скважин в антенне (длина антенны) (реальное количество, заполняется, если исопльзуютсяне все)
        // Дополнительные параметры, определяющие строку в таблице событий и расчетов
        public long EventDateTime;       // Время события в тиках системных (100 нсек.)
        public DateTime ImportDateTime;  // Время импорта (подкачки) события
        public DateTime? CalcDateTime;   // Время расчета результата
        public short Velocity;           // Скорость звука в м/сек.
        public double? AE_X;             // Координата X рассчитанного ИАЭ
        public double? AE_Y;             // Координата Y рассчитанного ИАЭ
        public double? AE_Z;             // Координата Z рассчитанного ИАЭ
        public float? Energy;            // Энергия события АЭ решения
        public float Kyc;                // Коэффициент усиления. Используется при расчете энергии
        public float? TimeError;         // Невязка в мкс. Это квадратный корень из суммы по всем скважинам антенны
                                         // квадратов разниц измеренных и вычисленных РВП, 
                                         // деленный на длину антенны минус 1
        public byte? Algorithm;          // Номер алгоритма расчета
        public double Rmin;              // Мин. расстояние от ИАЭ и скважинами в текущей антенне.


        public static void CalcAlgorithm30(GCSDataSet.AE_EventsRow rowAE_Events, GCSDataSet gcsDataSet)
        {

            //if (GlbDefs.MySettings.UseCalc30 == 0) return;
            //if (GlbDefs.MySettings.UseCalc30InMin == 0 && GlbDefs.allHoles.AntennaLenght < 5) return;
           // if (GlbDefs.allHoles.AntennaLenght > 8) return;// Данный алгоритм плохо работает для длинных серий

           // GlbDefs.allHoles.ClearCalculation(); // Очистка результатов вычислений

            string strProtocol = string.Empty;
            long EventID = rowAE_Events.EventID;
            byte NumberOfDecisions = 0;

            try
            {
                if (CalcAlgorithm30main(GlbDefs.eAlgorithm.МНК_для_РВП, rowAE_Events.EnergyParamsType,
                                        out strProtocol, 1, out NumberOfDecisions))
                {
                    // Запись в таблицу "Решений" AE_XYZ (добавление новой записи)
                    bool Success = DoOneEventGridViewSelectedRow.WriteTableXYZ(EventID, gcsDataSet, ref strProtocol);


                    if (GlbDefs.MySettings.WriteNotes == 1)
                    {
                        if (Success)
                        {
                            rowAE_Events.Details += "Расчет по алгоритму \"" + GlbDefs.eAlgorithm.МНК_для_РВП +
                                                    "\" успешно завершен! Длина антенны = " + GlbDefs.allHoles.AntennaLenght.ToString() + GlbDefs.NL;
                        }
                        else
                        {
                            rowAE_Events.Details += "Решение по алгоритму \"" + GlbDefs.eAlgorithm.МНК_для_РВП +
                                                    "\" отфиьтровано :" + strProtocol + " Длина антенны = " + GlbDefs.allHoles.AntennaLenght.ToString() + GlbDefs.NL;
                        }
                    }
                }
                else
                {
                    if (GlbDefs.MySettings.WriteNotes == 1)
                    {
                        rowAE_Events.Details += "Расчет по алгоритму \"" + GlbDefs.eAlgorithm.МНК_для_РВП +
                                                "\" завершен с ошибкой! Длинна антенны = " + GlbDefs.allHoles.AntennaLenght.ToString() + GlbDefs.NL;
                    }
                    // Запись информации о плохом решении в таблицу "Решений"
                    DoOneEventGridViewSelectedRow.WriteTableXYZ_Bad_Calculation(EventID, gcsDataSet, strProtocol,
                                                                                (byte)GlbDefs.eAlgorithm.МНК_для_РВП);
                    if (GlbDefs.MySettings.EventLogWriteEntry == 1)
                    {
                        EventLog.WriteEntry(Application.ProductName,
                                             "Расчет по алгоритму \"" + GlbDefs.eAlgorithm.МНК_для_РВП +
                                             "\" завершен с ошибкой! Длинна антенны = " + GlbDefs.allHoles.AntennaLenght.ToString(),
                                             EventLogEntryType.Error,
                                             (int)GlbDefs.eventID.Error,
                                             (int)GlbDefs.eCategory.AlgorithmsError);
                    }
                }
            }
            catch (Exception ex)
            {
                if (GlbDefs.MySettings.WriteProtocol == 1)
                {
                    strProtocol += GlbDefs.NL + "Непредвиденная ошибка расчета при использовании алгоритма " +
                                    ((byte)GlbDefs.eAlgorithm.МНК_для_РВП).ToString() + " -> " +
                                    GlbDefs.eAlgorithm.МНК_для_РВП + " : " + ex.Message + GlbDefs.NL;
                    if (GlbDefs.MySettings.EventLogWriteEntry == 1)
                    {
                        EventLog.WriteEntry(Application.ProductName,
                                            "Непредвиденная ошибка расчета при использовании алгоритма " +
                                            ((byte)GlbDefs.eAlgorithm.МНК_для_РВП).ToString() + " -> " +
                                            GlbDefs.eAlgorithm.МНК_для_РВП + " : " + ex.Message,
                                            EventLogEntryType.Error,
                                            (int)GlbDefs.eventID.Error,
                                            (int)GlbDefs.eCategory.AlgorithmsError);
                    }
                    if (GlbDefs.MySettings.WriteNotes == 1)
                    {
                        rowAE_Events.Details += "Непредвиденная ошибка расчета при использовании алгоритма " +
                                                ((byte)GlbDefs.eAlgorithm.МНК_для_РВП).ToString() + " -> " +
                                                GlbDefs.eAlgorithm.МНК_для_РВП + " : " + ex.Message + GlbDefs.NL;
                    }
                    // Запись информации о плохом решении в таблицу "Решений"
                    DoOneEventGridViewSelectedRow.WriteTableXYZ_Bad_Calculation(EventID, gcsDataSet, strProtocol,
                                                                               (byte)GlbDefs.eAlgorithm.МНК_для_РВП);
                }
            }
        }

        public bool DirectData(bool UseSphereModelRestriction)
        {
            // Определяем расстояние (в метрах), соответствующее параметру S0TimeError и
            // текущей скорости сигнала АЭ
            // Необходимо учесть, что S0TimeError задается в микросекундах...
            double mS0TimeErrorDistance = ((double)GlbDefs.MySettings.S0TimeError / 1000000.0) * Velocity;

            Rmin = double.MaxValue;
            Point3 AE = new Point3((double)AE_X, (double)AE_Y, (double)AE_Z);


            // Определяем расстояния между найденным решением и скважинами и (если задано)
            // проверяем на соответствие ограничениям сферической модели распространения сигнала АЭ
            HolesArray[0].Ri = Point3.DistancePoints(new Point3(HolesArray[0].X, HolesArray[0].Y, HolesArray[0].Z),
                                                     AE);
            for (int i = 1; i < AntennaLenght; i++)
            {
                HolesArray[i].Ri = Point3.DistancePoints(new Point3(HolesArray[i].X, HolesArray[i].Y, HolesArray[i].Z),
                                                         AE);
                if (UseSphereModelRestriction && HolesArray[i].Ri < HolesArray[i - 1].Ri - mS0TimeErrorDistance)
                {
                    GlbDefs.NumberOfBadPoints++;
                    return false;
                }
            }

            // Определяем минимальное расстояние между найденным решением и скважинами
            for (int i = 0; i < AntennaLenght; i++)
                Rmin = Math.Min(Rmin, (double)HolesArray[i].Ri);

            // Определяем РВП
            for (int i = 0; i < AntennaLenght; i++)
                HolesArray[i].DTd = (float)Math.Round((double)(((HolesArray[i].Ri - Rmin) / Velocity) * 1000000.0), 0);

            debugArray.Add(HolesArray[1].DTd.ToString());

            return true;
        }
        */
    }
}
