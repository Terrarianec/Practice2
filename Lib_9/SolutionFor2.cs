using System;

namespace Lib_9
{
    public class SolutionFor2
    {
        /// <summary>
        /// ������� ������������ �����
        /// </summary>
        /// <param name="array">������ �����</param>
        /// <returns>������������ �����</returns>
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
