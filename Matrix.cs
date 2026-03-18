using System;

namespace Common.MathEx.Algebra
{
    // Класс Матрица (2-ва измерения). Элементы матрицы передаются и хранятся по строкам
    public class Matrix  
    {
        readonly int mrows, mcols; // Размерности матрицы
        readonly private double[,] mmatrix;

        public Matrix(){}

        public Matrix(int Rows, int Columns)
        {
            if (Rows > 0 && Columns > 0)
            {
                mrows = Rows; mcols = Columns; mmatrix = new double[mrows, mcols];

                for (int i = 0; i < mrows; i++)
                {
                    for (int j = 0; j < mcols; j++)
                    {
                        mmatrix[i, j] = 0.0;
                    }
                }
            }
        }

        /// <summary>
        /// Конструктор. Параметры передаются в виде строки.
        /// Сначала передяется кол-во строк, затем кол-во столбцов 
        /// и элементы матрицы построчно.
        /// </summary>
        public Matrix(int Rows, int Columns, params double[] vars)
        {
            if (vars.Length > 0 && vars.Length == Rows * Columns)
            {
                mrows = Rows; mcols = Columns; mmatrix = new double[mrows, mcols];
                for (int i = 0; i < mrows; i++)
                {
                    for (int j = 0; j < mcols; j++)
                    {
                        mmatrix[i, j] = vars[i * mcols + j];
                    }
                }
            }
        }

        /// <summary>
        /// Возвращает количество строк матрицы.
        /// </summary>
        public int Rows
        {
            get { return (int)mrows; }
        }

        /// <summary>
        ///Возвращает количество столбцов матрицы.
        /// </summary>
        public int Columns
        {
            get { return (int)mcols; }
        }


        public double this[int indexl, int index2]
        {
            get { return mmatrix[indexl, index2]; }
            set { mmatrix[indexl, index2] = (double)value; }
        }

        /// <summary>
        /// Транспонирование матриц - Метод Экземпляра.
        /// </summary>
        public Matrix MatrixTransform()
        {
            Matrix result = new Matrix(mcols, mrows);
            for (int i = 0; i < mrows; i++)
            {
                for (int j = 0; j < mcols; j++)
                {
                    result[j, i] = mmatrix[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// Транспонирование матриц - Static метод.
        /// </summary>
        /// <param name="Original"></param>
        /// <returns></returns>
        public static Matrix MatrixTransform(Matrix Original)
        {
            Matrix result = new Matrix(Original.Columns, Original.Rows);
            for (int i = 0; i < Original.Rows; i++)
            {
                for (int j = 0; j < Original.Columns; j++)
                {
                    result[j, i] = Original[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// Умножение матриц - Метод Экземпляра.
        /// Объект Matrix - матрица слева, параметр RightM - матpица справа
        /// Результиpующая матpица - pазмеpность Matrix.Rows * RightM.Columns
        /// Если успех, то возвpащается созданная в методе матрица. В пpотивном случае возвpащается null.
        /// </summary>
        public Matrix MatrixMatrixMultiply(Matrix RightM)
        {
            if (RightM == null || mcols != RightM.Rows) return null;
            Matrix result = new Matrix(mrows, RightM.Columns);
            for (int i = 0; i < mrows; i++)
            {
                for (int j = 0; j < RightM.Columns; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < mcols; k++)
                    {
                        result[i, j] += mmatrix[i, k] * RightM[k, j];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Умножение матриц - Static метод.
        /// Параметр LeftM - матрица слева, параметр RightM - матpица справа
        /// Результиpующая матpица - pазмеpность LeftM.Rows * RightM.Columns
        /// Если успех, то возвpащается созданная в методе матрица. В пpотивном случае возвpащается null.
        /// </summary>
        public static Matrix MatrixMatrixMultiply(Matrix LeftM, Matrix RightM)
        {
            if (LeftM == null || RightM == null || LeftM.Columns != RightM.Rows) return null;
            Matrix result = new Matrix(LeftM.Rows, RightM.Columns);
            for (int i = 0; i < LeftM.Rows; i++)
            {
                for (int j = 0; j < RightM.Columns; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < LeftM.Columns; k++)
                    {
                        result[i, j] += LeftM[i, k] * RightM[k, j];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Умножение матрицы на вектор - Метод Экземпляра.
        /// Объект Matrix - матрица слева, параметр Vector - вектор справа (Matrix[M,1]).
        /// Если успех, то возвpащается вектор размерностью Matrix.Rows
        /// </summary>
        public Matrix MatrixVectorMultiply(Matrix Vector)
        {
            if (mcols != Vector.Rows || Vector.Columns != 1) return null;
            Matrix result = new Matrix(mrows, 1);
            for (int i = 0; i < mrows; i++)
            {
                result[i, 0] = 0.0;
                for (int j = 0; j < mcols; j++)
                {
                    result[i, 0] += mmatrix[i, j] * Vector[j, 0];
                }
            }
            return result;
        }

        /// <summary>
        /// Умножение матрицы на вектор - Static метод.
        /// Параметр LeftM - матрица слева, параметр Vector - вектор справа (Matrix[M,1]).
        /// Если успех, то возвpащается вектор размерностью LeftM.Rows
        /// </summary>
        public static Matrix MatrixVectorMultiply(Matrix LeftM, Matrix Vector)
        {
            if (LeftM.Columns != Vector.Rows || Vector.Columns != 1) return null;
            Matrix result = new Matrix(LeftM.Rows, 1);
            for (int i = 0; i < LeftM.Rows; i++)
            {
                result[i, 0] = 0.0;
                for (int j = 0; j < LeftM.Columns; j++)
                {
                    result[i, 0] += LeftM[i, j] * Vector[j, 0];
                }
            }
            return result;
        }

        /// <summary>
        /// Метод Гаусса решения СЛАУ.
        /// Параметр Matrix - матpица (M * M), параметр RightPart - правая часть (M * 1).
        /// Если успех, то возвращается вектоp pешения result (M * 1), в противном случае возвращается null.
        /// Исходная матрица меняется! и правая часть тоже !
        /// </summary>
        public static Matrix Gause(Matrix SLAE, Matrix RightPart) // Решение СЛАУ методом Гаусса
        {
            if (SLAE == null || RightPart == null) return null;

            int size, i1;  // Размерность системы, временная переменная
            double temp, koeff; // Временная переменная, коэффициент для приведения матриц к треугольному 

            size = SLAE.Rows;
            if ((SLAE.Columns != size) || (RightPart.Rows != size) || RightPart.Columns != 1) return null;
            Matrix result = new Matrix(size, 1);

            if (size == 1 & SLAE[0, 0] != 0.0) // Размерность системы 1 x 1
            {
                result[0, 0] = RightPart[0, 0] / SLAE[0, 0];
                return result;
            }
            // Пpеобpазование системы уpавнений к "тpеугольному виду" 
            for (int j = 0; j < size; j++) 
            {
                temp = Math.Abs(SLAE[j, j]); // Поиск MAX элемента в подстолбце матpицы
                i1 = j;
                for(int i = j+1; i < size; i++)
                {
                    if (Math.Abs(SLAE[i, j]) > temp) { temp = Math.Abs(SLAE[i, j]); i1 = i; }
                }
                if (temp == 0.0) return null; // Детерминант матрицы равен 0!
                if (i1 != j) // пеpестановка стpок  i1 и j
                {
                    for (int k = j; k < size; k++) // Перестановка строк матрицы и правой части
                    {
                        temp = SLAE[j, k]; SLAE[j, k] = SLAE[i1, k]; SLAE[i1, k] = temp;
                    }
                    temp = RightPart[j, 0]; RightPart[j, 0] = RightPart[i1, 0]; RightPart[i1, 0] = temp;
                }
                for (int k = j+1; k < size; k++) // Пpеобpазование матpицы к тpеугольному виду
                {
                    koeff = (SLAE[k, j] / SLAE[j, j]);
                    for (int l = j; l < size; l++) SLAE[k, l] -= koeff * SLAE[j, l];
                    RightPart[k, 0] -= koeff * RightPart[j, 0];
                }
            }
            // обpатная пpогонка для нахождения pешения 
            for(int i = size - 1; i >= 0; i--)
            {
                result[i, 0] = RightPart[i, 0];
                for (int j = i + 1; j < size; j++) result[i, 0] -= SLAE[i, j] * result[j, 0];
                result[i, 0] /= SLAE[i, i];
            }
            return result;
        }

        /// <summary>
        /// Копирование матрицы - Метод Экземпляра.
        /// </summary>
        public Matrix MatrixCopy()
        {
            Matrix result = new Matrix(mrows, mcols);
            for (int i = 0; i < mrows; i++)
            {
                for (int j = 0; j < mcols; j++)
                {
                    result[i, j] = mmatrix[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// Копирование матрицы - Static метод.
        /// Параметр - исходная матрица.
        /// </summary>
        public static Matrix MatrixCopy(Matrix Original)
        {
            Matrix result = new Matrix(Original.Rows, Original.Columns);
            for (int i = 0; i < Original.Rows; i++)
            {
                for (int j = 0; j < Original.Columns; j++)
                {
                    result[i, j] = Original[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// Нахождение обратной матрицы - метод Экземпляра.
        /// Если обратная матрица НЕ найдена (например, исходная матрица НЕ квадратная),
        /// то возвращаем null
        /// </summary>
        public Matrix MatrixOpposite() 
        {
            if (mrows != mcols) return null; // матрица не квадратная
            Matrix Original = new Matrix(mrows, mcols);
            Matrix Result = new Matrix(mrows, mcols);
            Matrix vOppositeColumn;// = new Matrix(mrows, 1); // Вектор Решение - это столбцы обратной матрицы
            Matrix vSingleColumn = new Matrix(mrows, 1);   // Вектор - столбцы единичной матрицы

            // Преобразовываем  mmatrix типа double[,] в GCS.Mathematics.Matrix
            // Иначе вызов Matrix.Gause(mmatrix, vSingleColumn) при компиляции дает ошибку:
            // cannot convert from 'double[*,*]' to 'GCS.Mathematics.Matrix'!
            
            for (int j = 0; j < mcols; j++)
            {
                for (int i = 0; i < mrows; i++)
                {
                    vSingleColumn[i,0] = 0;
                }
                vSingleColumn[j,0] = 1; // Сформировали j-ый столбец единичной матрицы

                for (int k = 0; k < mrows; k++)
                    for (int l = 0; l < mcols; l++)
                        Original[k, l] = mmatrix[k, l];
                
                // Внимание. Исходная матрица меняется!
                vOppositeColumn = Matrix.Gause(Original, vSingleColumn); // Сформировали j-ый столбец обратной матрицы
                if (vOppositeColumn == null) return null;

                for (int i = 0; i < mrows; i++)
                {
                    Result[i,j] = vOppositeColumn[i,0];
                }
            }
            return Result;
        }

        /// <summary>
        /// Нахождение обратной матрицы - Static метод.
        /// Если обратная матрица НЕ найдена (например, исходная матрица НЕ квадратная),
        /// то возвращаем null
        /// </summary>
        public static Matrix MatrixOpposite(Matrix Original)
        {
            int rows = Original.mrows;
            int cols = Original.mcols;

            if (rows != cols) return null; // матрица не квадратная
            Matrix result = new Matrix(rows, cols);
            Matrix vOppositeColumn;// = new Matrix(rows, 1); // Вектор Решение - это столбцы обратной матрицы
            Matrix vSingleColumn = new Matrix(rows, 1);   // Вектор - столбцы единичной матрицы

            for (int j = 0; j < cols; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    vSingleColumn[i,0] = 0;
                }
                vSingleColumn[j,0] = 1; // Сформировали j-ый столбец единичной матрицы

                vOppositeColumn = Matrix.Gause(Original, vSingleColumn); // Сформировали j-ый столбец обратной матрицы

                for (int i = 0; i < rows; i++)
                {
                    result[i,j] = vOppositeColumn[i,0];
                }
            }
            return result;

        }

        /// <summary>
        /// Определение Детерминанта матрицы - метод Экземпляра.
        /// Если Детерминант НЕ может быть определен (например, исходная матрица НЕ квадратная),
        /// то возвращаем null.
        /// Матрица - НЕ изменяется
        /// </summary>
        public double? MatrixDeterminant()
        {
            int size, i1, sign;  // Размерность системы, временная переменная, знак (учет перестановки строк)
            double temp, koeff;  // Временная переменная, коэффициент для приведения матриц к треугольному 

            if (mrows != mcols) return null;
            
            size = mrows;

            if (size == 1)    // Размерность системы 1 x 1
            {
                return mmatrix[0,0];
            }
            
            // Создаем копию исходной матрицы
            double[,] mmatrix1 = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < mrows; j++)
                    mmatrix1[i, j] = mmatrix[i, j];

            sign = 1;
            // Пpеобpазование системы уpавнений к "тpеугольному виду" 
            for (int j = 0; j < size; j++)
            {
                temp = Math.Abs(mmatrix1[j, j]); // Поиск MAX элемента в подстолбце матpицы
                i1 = j;
                for (int i = j + 1; i < size; i++)
                {
                    if (Math.Abs(mmatrix1[i, j]) > temp) { temp = Math.Abs(mmatrix1[i, j]); i1 = i; }
                }
                
                if (temp == 0.0) return 0.0; // Детерминант матрицы равен 0!
                
                if (i1 != j) // пеpестановка стpок  i1 и j
                {
                    // Перестановка любых 2-х строк изменяет знак детерминанта
                    sign *= -1;
                    for (int k = j; k < size; k++) // Перестановка строк матрицы и правой части
                    {
                        temp = mmatrix1[j, k]; mmatrix1[j, k] = mmatrix1[i1, k]; mmatrix1[i1, k] = temp;
                    }
                }
                for (int k = j + 1; k < size; k++) // Пpеобpазование матpицы к тpеугольному виду
                {
                    koeff = (mmatrix1[k, j] / mmatrix1[j, j]);
                    for (int l = j; l < size; l++) mmatrix1[k, l] -= koeff * mmatrix1[j, l];
                }
            }

            // Собственно определение Детерминанта
            temp = 1.0;
            for (int i = 0; i < size; i++) temp *= mmatrix1[i, i];
            return temp * sign;
        }

        /// <summary>
        /// Определение Детерминанта матрицы - Static метод.
        /// Если Детерминант НЕ может быть определен (например, исходная матрица НЕ квадратная),
        /// то возвращаем null.
        /// Матрица - НЕ изменяется
        /// </summary>
        public static double? MatrixDeterminant(Matrix Original)
        {
            int size, i1, sign;  // Размерность системы, временная переменная, знак (учет перестановки строк)
            double temp, koeff;  // Временная переменная, коэффициент для приведения матриц к треугольному 

            if (Original.mrows != Original.mcols) return null;

            size = Original.mrows;

            if (size == 1)    // Размерность системы 1 x 1
            {
                return Original[0, 0];
            }

            // Создаем копию исходной матрицы
            double[,] mmatrix1 = new double[size, size];
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    mmatrix1[i, j] = Original[i, j];

            sign = 1;
            // Пpеобpазование системы уpавнений к "тpеугольному виду" 
            for (int j = 0; j < size; j++)
            {
                temp = Math.Abs(mmatrix1[j, j]); // Поиск MAX элемента в подстолбце матpицы
                i1 = j;
                for (int i = j + 1; i < size; i++)
                {
                    if (Math.Abs(mmatrix1[i, j]) > temp) { temp = Math.Abs(mmatrix1[i, j]); i1 = i; }
                }
                
                if (temp == 0.0) return 0.0; // Детерминант матрицы равен 0!
                
                if (i1 != j) // пеpестановка стpок  i1 и j
                {
                    // Перестановка любых 2-х строк изменяет знак детерминанта
                    sign *= -1; 
                    for (int k = j; k < size; k++) // Перестановка строк матрицы и правой части
                    {
                        temp = mmatrix1[j, k]; mmatrix1[j, k] = mmatrix1[i1, k]; mmatrix1[i1, k] = temp;
                    }
                }
                for (int k = j + 1; k < size; k++) // Пpеобpазование матpицы к тpеугольному виду
                {
                    koeff = (mmatrix1[k, j] / mmatrix1[j, j]);
                    for (int l = j; l < size; l++) mmatrix1[k, l] -= koeff * mmatrix1[j, l];
                }
            }

            // Собственно определение Детерминанта
            temp = 1.0;
            for (int i = 0; i < size; i++) temp *= mmatrix1[i, i];
            return temp * sign;
        }

        /// <summary>
        /// Сумма 2-х матриц - метод Экземпляра.
        /// Если сумма НЕ может быть определена (например, размерности матриц разные),
        /// то возвращаем null.
        /// </summary>
        public Matrix MatrixSum(Matrix SecondMatrix) 
        {
            if (mrows != SecondMatrix.mrows || mcols != SecondMatrix.mcols) return null;

            Matrix result = new Matrix(mrows, mcols);
            for (int i = 0; i < mrows; i++)
                for (int j = 0; j < mcols; j++)
                    result[i, j] = mmatrix[i, j] + SecondMatrix[i, j];
            return result;
        }

        /// <summary>
        /// Сумма 2-х матриц - Static метод.
        /// Если сумма НЕ может быть определена (например, размерности матриц разные),
        /// то возвращаем null.
        /// </summary>
        public static Matrix MatrixSum(Matrix FirstMatrix, Matrix SecondMatrix)
        {
            if (FirstMatrix.mrows != SecondMatrix.mrows || FirstMatrix.mcols != SecondMatrix.mcols) return null;
            
            int rows = FirstMatrix.mrows;
            int cols = FirstMatrix.mcols;

            Matrix result = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    result[i, j] = FirstMatrix[i, j] + SecondMatrix[i, j];
            return result;
        }
    }
}


