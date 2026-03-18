using IHaveLocation = AntennaCalc.Models.Input.IHaveLocation;
using Matrix = Common.MathEx.Algebra.Matrix;
using Common.MathEx.Geometry3D; // Point3DExtensions.Round()
using System;
using System.Windows.Media.Media3D;

namespace AntennaCalc.Models.Algorithms
{
    // adaptation for:
    //   Алгоритм "Дальстандартовский".
    //   Несколько модифицированный вариант. (GlbDefs.eAlgorithm.МНК_для_РВП)
    //   Решил избавиться от больших координат (типа Ri^2 - Rj^2) => переношу начало системы координат 
    //   в первая принявшая скважины и т.п. Надеялся, что точность решения улучшится...
    //   Получилось проще, а точность решения такая же, как и в классическом варианте...
    //
    // Данный алгоритм плохо работает для длинных серий (>8)
    //
    static class Calc30_adaptation
    {
        public const int Id = 30;
        public const string Name = "Алгоритм с использованием МНК для РВП (№ 30)";
        public const int DigitsToRound = 3;

        // TDOAs - time difference of arrivals - массив РВП (в мкс) для соответствующих receivers (геофонов с известными координатами)
        //     Геофоны должны быть отсортированы по возрастанию РВП, так что TDOAs[0]==0
        // на выходе - массив возможных решений или null
        public static Point3D[] CalcAlgorithm30(IHaveLocation[] receivers, int[] TDOAs, double velocity)
        {
            if (receivers.Length!=TDOAs.Length || receivers.Length<4) throw new ArgumentException("Неверные массивы датчиков и РВП");

            int AntennaLenght_decremented_by_1 = receivers.Length - 1;
            Matrix matrix1 = new Matrix(AntennaLenght_decremented_by_1, 3);
            Matrix vector1 = new Matrix(AntennaLenght_decremented_by_1, 1);
            Matrix vector2 = new Matrix(AntennaLenght_decremented_by_1, 1);

            // фоpмиpование матpицы и вектоpов 1 и 2 пpавой части. receivers уже отсортированы по РВП
            for (int i = 0; i < AntennaLenght_decremented_by_1; i++)
            {
                double dx = receivers[i + 1].Location.X - receivers[0].Location.X;
                double dy = receivers[i + 1].Location.Y - receivers[0].Location.Y;
                double dz = receivers[i + 1].Location.Z - receivers[0].Location.Z;
                
                matrix1[i, 0] = dx;
                matrix1[i, 1] = dy;
                matrix1[i, 2] = dz;
                
                double dist = velocity * ((double)TDOAs[i + 1] / 1000000);
                vector1[i, 0] = (dx*dx + dy*dy + dz*dz - dist*dist) / 2;
                vector2[i, 0] = -dist;
            }

            // Фоpмиpование тpанспониpованной матpицы
            Matrix matrix1T = matrix1.MatrixTransform();

            // Умножение тpанспониpованной матpицы на матpицу т.е. фоpмиpование матpицы СЛАУ и создание копии
            // получившейся матрицы (т.к. метод Гаусса ИЗМЕНЯЕТ исходную матрицу -> приводит к диагональному виду)!
            // matrix33a - для решения с Правой частью 1;  matrix33b - для решения с Правой частью 2;
            Matrix matrix33a = matrix1T.MatrixMatrixMultiply(matrix1);
            Matrix matrix33b = matrix33a.MatrixCopy();

            // Умножение тpанспониpованной матpицы на 1-ый вектоp пpавой части - фоpмиpование пpавой части 1
            // Копии правых частей можно не изменять ...
            Matrix right1 = matrix1T.MatrixMatrixMultiply(vector1);

            // Умножение тpанспониpованной матpицы на 2-ой вектоp пpавой части - фоpмиpование пpавой части 2
            Matrix right2 = matrix1T.MatrixMatrixMultiply(vector2);

            // Решение линейной части уpавнений
            // pешение 1 / подчасть 1
            Matrix decision1 = Matrix.Gause(matrix33a, right1);
            if (decision1 == null) return null;

            // pешение 2 / подчасть 2, умножаемая на R
            Matrix decision2 = Matrix.Gause(matrix33b, right2);
            if (decision2 == null) return null;

            // Решение HЕлинейной части уpавнений
            double a, b, c; // коэффициенты линейного уравнения
            a = Math.Pow(decision2[0,0], 2) + Math.Pow(decision2[1,0], 2) + Math.Pow(decision2[2,0], 2) - 1;
            b = 2 * (decision1[0,0] * decision2[0,0] + decision1[1,0] * decision2[1,0] + 
                     decision1[2,0] * decision2[2,0]);
            c = Math.Pow(decision1[0,0], 2) + Math.Pow(decision1[1,0], 2) + Math.Pow(decision1[2,0], 2);
            
            double R; // расстояние

            if (a == 0)
            {
                if (b != 0)
                {
                    R = -c / b;
                    return CombineOutput1(R);
                }
                else return null;
            }
            else
            {
                double D = b*b - 4*a*c; // дискриминант

                if (D < 0)
                {
                    return null;
                }
                else if (D == 0)
                {
                    R = -b / (2 * a);
                    return CombineOutput1(R);
                }
                else
                {
                    double sqrtD = Math.Sqrt(D);
                    double R1 = (-b - sqrtD) / (2 * a);
                    double R2 = (-b + sqrtD) / (2 * a);
                    return CombineOutput2(R1, R2);
                }
            }

            Point3D CombineDecision(double r)
                => new Point3D(decision1[0, 0] + r * decision2[0, 0] + receivers[0].Location.X,
                               decision1[1, 0] + r * decision2[1, 0] + receivers[0].Location.Y,
                               decision1[2, 0] + r * decision2[2, 0] + receivers[0].Location.Z).Round(DigitsToRound);
            Point3D[] CombineOutput1(double r1) => new Point3D[] { CombineDecision(r1) };
            Point3D[] CombineOutput2(double r1, double r2) => new Point3D[] { CombineDecision(r1), CombineDecision(r2) };
        }
    }
}
