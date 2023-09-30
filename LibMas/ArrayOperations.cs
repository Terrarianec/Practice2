using System;
using System.IO;

namespace LibMas
{
    public class ArrayOperations
    {
        const char columnEnd = ';';
        private static Random _random = new Random(0);

        /// <summary>
        /// ��������� ������ �� �����
        /// </summary>
        /// <param name="path">���� � �����</param>
        /// <returns>������ ����� �� �����</returns>
        public static int[] Open(string path)
        {
            if (!File.Exists(path)) return null;

            using (StreamReader file = new StreamReader(path))
            {
                string data = file.ReadToEnd();

                int columns = 1;

                for (int i = 0; i < data.Length; ++i)
                {
                    if (data[i] == columnEnd) ++columns;
                }

                string stringifiedNumber = string.Empty;
                int[] array = new int[columns];
                int index = 0;

                for (int i = 0; i < data.Length; ++i)
                {
                    if (data[i] == columnEnd)
                    {
                        if (!int.TryParse(stringifiedNumber, out int number))
                            throw new FileFormatException("���� ��������");

                        array[index++] = number;
                        stringifiedNumber = string.Empty;
                        continue;
                    }

                    stringifiedNumber += data[i];
                }

                if (stringifiedNumber == string.Empty)
                    throw new FileFormatException("���� ��������");

                array[index] = Convert.ToInt32(stringifiedNumber);

                return array;
            }
        }

        /// <summary>
        /// ��������� ������ � ����
        /// </summary>
        /// <param name="array">����������� ������</param>
        /// <param name="path">���� � �����</param>
        /// <returns>������� �� ����������� ����������</returns>
        public static bool Save(int[] array, string path)
        {
            if (array == null || array.Length == 0) return false;

            if (!File.Exists(path))
                File.Create(path);

            try
            {
                using (StreamWriter file = new StreamWriter(path))
                {
                    file.Write(string.Join(columnEnd, array));
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ��������� ������
        /// </summary>
        /// <param name="array">������</param>
        public static void Fill(in int[] array, int min, int max)
        {
            if (min > max)
                (min, max) = (max, min);

            for (int index = 0; index < array.Length; ++index)
            {
                int value;

                do
                {
                    value = _random.Next(min, max + 1);
                } while (value == 0);

                array[index] = value;
            }
        }

        /// <summary>
        /// ������� ������
        /// </summary>
        /// <param name="array">������</param>
        public static void Clear(in int[] array)
        {
            Fill(array, 0, 0);
        }
    }
}
