using System;

namespace Lib_9
{
    public class SolutionFor2
    {
        /// <summary>
        /// Находит произведение чисел
        /// </summary>
        /// <param name="array">Массив чисел</param>
        /// <returns>Произведение чисел</returns>
        public static long Multiply(int[] array)
        {
            long product = 1;

            for (int i = 0; i < array.Length; i++)
            {
                product *= array[i];
            }

            return product;
        }
    }
}
