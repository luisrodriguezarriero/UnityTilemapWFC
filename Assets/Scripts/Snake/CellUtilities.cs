using System.Collections.Generic;

namespace SnakeGame
{
    internal static class CellUtilities
    {
        
        public static Cell[] SetDestination(this Cell[] array, Cell destination)
        {
            foreach (var cell in array)
            {
                cell.ManhattanDistance(destination);
            }

            return array;
        }
        public static Cell[] Sort(this Cell[] array)
        {
            int size = array.Length;
            if (size <= 1)
                return array;
            for (int i = size / 2 - 1; i >= 0; i--)
            {
                array.Heapify(size, i);
            }
            for (int i = size - 1; i >= 0; i--)
            {
                (array[0], array[i]) = (array[i], array[0]);
                array.Heapify(i, 0);
            }
            return array;
        }

        private static void Heapify(this Cell[] array, int size, int index)
        {
            var largestIndex = index;
            var leftChild = 2 * index + 1;
            var rightChild = 2 * index + 2;
            if (leftChild < size && array[leftChild].Distance > array[largestIndex].Distance)
            {
                largestIndex = leftChild;
            }
            if (rightChild < size && array[rightChild].Distance > array[largestIndex].Distance)
            {
                largestIndex = rightChild;
            }
            if (largestIndex != index)
            {
                (array[index], array[largestIndex]) = (array[largestIndex], array[index]);
                array.Heapify(size, largestIndex);
            }
        }

        public static (int x, int y) Pop(this List<(int x, int y)> list)
        {
            (int x, int y) item = list[0];
            list.RemoveAt(0);
            return item;
        }
    }
}