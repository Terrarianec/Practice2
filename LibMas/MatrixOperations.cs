using System;
using System.IO;

namespace LibMas
{
    public class MatrixOperations
    {
        const char rowEnd = '\n';
        const char columnEnd = ';';
        private static Random _random = new Random(0);

        /// <summary>
        /// Открывает матрицу из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Матрица чисел из файла</returns>
        public static int[,] Open(string path)
        {
            if (!File.Exists(path)) return null;

            using (StreamReader file = new StreamReader(path))
            {
                string data = file.ReadToEnd();

                int rows = 1;
                int columns = 1;

                for (int i = 0; i < data.Length; ++i)
                {
                    if (data[i] == columnEnd) ++columns;
                    if (data[i] == rowEnd)
                    {
                        ++rows;
                        columns = 1;
                    }
                }

                string stringifiedNumber = string.Empty;
                int[,] matrix = new int[rows, columns];
                int row = 0;
                int column = 0;

                for (int i = 0; i < data.Length; ++i)
                {
                    if (data[i] == columnEnd || data[i] == rowEnd)
                    {
                        if (!int.TryParse(stringifiedNumber, out int number))
                            throw new FileFormatException("Файл повреждён");

                        matrix[row, column] = number;

                        switch (data[i])
                        {
                            case rowEnd:
                                ++row;
                                column = 0;
                                break;
                            case columnEnd:
                                ++column;
                                break;
                        }

                        stringifiedNumber = string.Empty;
                        continue;
                    }

                    stringifiedNumber += data[i];
                }

                if (stringifiedNumber == string.Empty)
                    throw new FileFormatException("Файл повреждён");

                matrix[row, column] = Convert.ToInt32(stringifiedNumber);

                return matrix;
            }
        }

        /// <summary>
        /// Сохраняет матрицу в файл
        /// </summary>
        /// <param name="matrix">Сохраняемая матрица</param>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Успешно ли закончилось сохранение</returns>
        public static bool Save(int[,] matrix, string path)
        {
            if (matrix == null || matrix.Length == 0) return false;

            if (!File.Exists(path))
                File.Create(path);
            try
            {
                using (StreamWriter file = new StreamWriter(path))
                {
                    for (int row = 0; row < matrix.GetLength(0); ++row)
                    {
                        for (int column = 0; column < matrix.GetLength(1); ++column)
                        {
                            file.Write(matrix[row, column]);

                            if (column < matrix.GetLength(1) - 1) file.Write(columnEnd);
                        }

                        if (row < matrix.GetLength(0) - 1) file.Write(rowEnd);
                    }

                    return true;
                }
            }
            catch
            {
                return false;

            }
        }

        /// <summary>
        /// Заполняет матрицу
        /// </summary>
        /// <param name="matrix">Матрица</param>
        public static void Fill(in int[,] matrix, int min, int max)
        {
            if (min > max)
                (min, max) = (max, min);

            for (int i = 0; i < matrix.GetLength(0); ++i)
            {
                for (int j = 0; j < matrix.GetLength(1); ++j)
                {
                    matrix[i, j] = _random.Next(min, max + 1);
                }
            }
        }

        /// <summary>
        /// Очищает матрицу
        /// </summary>
        /// <param name="matrix">Матрица</param>
        public static void Clear(in int[,] matrix)
        {
            Fill(matrix, 0, 0);
        }
    }
}
