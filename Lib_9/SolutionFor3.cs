namespace Lib_9
{
    public class SolutionFor3
    {
        /// <summary>
        /// Поиск строки матрицы с наибольшей суммой
        /// </summary>
        /// <param name="matrix">Матрица</param>
        /// <param name="row">Номер строки с наибольшей суммой</param>
        /// <returns>Наибольшее значение суммы</returns>
        public static int MaxRowSumIn(int[,] matrix, out int row)
        {
            row = 0;
            int maxSum = int.MinValue;

            for (int line = 0; line < matrix.GetLength(0); ++line)
            {
                int sum = 0;

                for (int column = 0; column < matrix.GetLength(1); ++column)
                {
                    sum += matrix[line, column];
                }

                if (sum > maxSum)
                {
                    maxSum = sum;
                    row = line;
                }
            }

            return maxSum;
        }
    }
}
