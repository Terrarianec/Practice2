namespace Lib_9
{
	public class SolutionFor3
	{
		/// <summary>
		/// Поиск номера последней из строк матрицы, содержащих максимальное количество одинаковых элементов.
		/// </summary>
		/// <param name="matrix">Матрица</param>
		/// <returns>Номер последней строки с наибольшим количеством одинаковых элементов</returns>
		public static int FindRowWithMaxSimilarItems(int[,] matrix)
		{
			int row = -1;
			int maxSimilar = 0;

			for (int i = 0; i < matrix.GetLength(0); ++i)
			{
				int maxSimilarInCurrentRow = 0;

				for (int j = 0; j < matrix.GetLength(1); ++j)
				{
					int currentValue = matrix[i, j];
					int similarCount = 0;

					for (int k = 0; k < matrix.GetLength(1); ++k)
					{
						if (matrix[i, k] == currentValue)
							++similarCount;
					}

					if (similarCount > maxSimilarInCurrentRow)
						maxSimilarInCurrentRow = similarCount;
				}

				if (maxSimilarInCurrentRow >= maxSimilar)
				{
					maxSimilar = maxSimilarInCurrentRow;
					row = i;
				}
			}

			return row;
		}
	}
}
