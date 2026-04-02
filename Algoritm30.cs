using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.MathEx.Algebra;
using GCS.Mathematics;

namespace ImpDistanceCalculation
{
    class Algoritm30
    {
        //вычисление AE_X, AE_Y, AE_Z (из Calc30 -> CalcAlgorithm30main)
        public static Coordinates getAECoordinates(Impulse[] antenna, double velocity)
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
                //MessageBox.Show("Ошибка решения СЛАУ");
                return null;
            }


            // pешение 2 / подчасть 2, умножаемая на R
            decision2 = Matrix.Gause(matrix33b, right2);
            if (decision2 == null)
            {
                //MessageBox.Show("Ошибка решения СЛАУ");
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

                    //MessageBox.Show("Дискриминант квадратного уравнения < 0. Решение отсутствует! ");
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
                        //MessageBox.Show("Дискриминант квадратного уравнения = 0. Ошибка деления на a!");
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
                            //MessageBox.Show("Задан неверный номер решения!");
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

        public static bool DirectData(bool UseSphereModelRestriction, Impulse[] antenna, Coordinates AE_result, double Velocity)
        {
            // Определяем расстояние (в метрах), соответствующее параметру S0TimeError и
            // текущей скорости сигнала АЭ
            // Необходимо учесть, что S0TimeError задается в микросекундах...
            /*
            double mS0TimeErrorDistance = ((double)GlbDefs.MySettings.S0TimeError / 1000000.0) * Velocity;




            // Определяем расстояния между найденным решением и скважинами и (если задано)
            // проверяем на соответствие ограничениям сферической модели распространения сигнала АЭ

            */
            if (AE_result == null)
            {
                return false;
            }
            double Rmin = double.MaxValue;
            int AntennaLenght = antenna.Length - 1;
            Point3 AE = new Point3((double)AE_result.x, (double)AE_result.y, (double)AE_result.z);

            antenna[0].Ri = Point3.DistancePoints(new Point3(antenna[0].coordinates.x, antenna[0].coordinates.y, antenna[0].coordinates.z),
                                                     AE);
            for (int i = 1; i < AntennaLenght; i++)
            {
                antenna[i].Ri = Point3.DistancePoints(new Point3(antenna[i].coordinates.x, antenna[i].coordinates.y, antenna[i].coordinates.z), AE);
            }


            // Определяем минимальное расстояние между найденным решением и скважинами
            for (int i = 0; i < AntennaLenght; i++)
                Rmin = Math.Min(Rmin, (double)antenna[i].Ri);

            // Определяем РВП
            for (int i = 0; i < AntennaLenght; i++)
                antenna[i].DTd = (float)Math.Round((double)(((antenna[i].Ri - Rmin) / Velocity) * 1000000.0), 0);

            //debugArray.Add(antenna[1].DTd.ToString());

            return true;
        }

        public static float TimeErrorClassic(Impulse[] antenna)
        {
            double Temp = 0;
            int AntennaLenght = antenna.Length - 1;
            Temp = 0;
            for (int i = 0; i < AntennaLenght; i++)
                Temp += Math.Pow((double)(antenna[i].DT - antenna[i].DTd), 2);

            //return (float)Math.Round((Math.Sqrt(Temp) / (AntennaLenght - 1)), 0);  // Было до 12.07.2009
            return (float)(Math.Sqrt(Temp) / (AntennaLenght /*- 1 было до 25.01.2014*/));
        }


    }
}
