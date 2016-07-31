using System;

namespace PoGoBot.Logic.Extensions
{
    internal static class ArrayExtensions
    {
        public static void SwapIndex<T>(this T[] array, int index1, int index2)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (index1 < 0 || index1 >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index1));
            }
            if (index2 < 0 || index2 >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index2));
            }
            var location1 = array[index1];
            var location2 = array[index2];
            array[index1] = location2;
            array[index2] = location1;
        }

        public static void MoveFromIndexTo<T>(this T[] array, int fromIndex, int toIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (fromIndex < 0 || fromIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(fromIndex));
            }
            if (toIndex < 0 || toIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(toIndex));
            }
            var temp = array[fromIndex];
            if (fromIndex < toIndex)
            {
                for (var i = fromIndex + 1; i <= toIndex; i++)
                {
                    array[i - 1] = array[i];
                }
            }
            else
            {
                for (var i = fromIndex; i > toIndex; i--)
                {
                    array[i] = array[i - 1];
                }
            }
            array[toIndex] = temp;
        }

        public static void ReverseRange<T>(this T[] array, int startIndex, int endIndex)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (startIndex < 0 || startIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }
            if (endIndex < 0 || endIndex >= array.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(endIndex));
            }
            if (endIndex < startIndex)
            {
                var temp = endIndex;
                endIndex = startIndex;
                startIndex = temp;
            }
            while (startIndex < endIndex)
            {
                var temp = array[endIndex];
                array[endIndex] = array[startIndex];
                array[startIndex] = temp;
                startIndex++;
                endIndex--;
            }
        }
    }
}