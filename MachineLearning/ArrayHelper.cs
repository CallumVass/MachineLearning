namespace MachineLearning
{
    using System.Linq;

    public static class ArrayHelper
    {
        public static T[,] ResizeArray<T>(T[,] original, int[] columnsToRemove)
        {
            var newArray = new T[original.GetLength(0), original.GetLength(1) - columnsToRemove.Length];
            int minRows = original.GetLength(0);
            for (int i = 0; i < minRows; i++)
            {
                int currentColumn = 0;
                for (int j = 0; j < original.GetLength(1); j++)
                {
                    if (columnsToRemove.Contains(j) == false)
                    {
                        newArray[i, currentColumn] = original[i, j];
                        currentColumn++;
                    }
                }
            }

            return newArray;
        }
    }
}