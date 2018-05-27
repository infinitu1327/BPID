using System;
using System.Collections.Generic;
using System.Linq;

namespace DES
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> sequence, int size)
        {
            var block = sequence as T[] ?? sequence.ToArray();

            for (var i = 0; i < (float) block.Length / size; i++)
                yield return block.Skip(i * size).Take(size);
        }

        public static IEnumerable<T> ShiftLeft<T>(this IEnumerable<T> sequence, int count)
        {
            var block = sequence as T[] ?? sequence.ToArray();

            if (count > block.Length) throw new Exception();
            return block.Skip(count).Concat(block.Take(count));
        }

        public static IEnumerable<T> ShiftRight<T>(this IEnumerable<T> sequence, int count)
        {
            var block = sequence as T[] ?? sequence.ToArray();

            if (count > block.Length) throw new Exception();
            return block.Skip(block.Length - count).Concat(block.Take(block.Count() - count));
        }

        public static IEnumerable<bool> FixLength(this IEnumerable<bool> block, int size)
        {
            var list = block.ToList();

            while (list.Count < size) list.Insert(0, default(bool));

            return list;
        }

        public static IEnumerable<char> FixLength(this IEnumerable<char> block, int size)
        {
            var list = block.ToList();

            while (list.Count < size) list.Insert(0, '0');

            return list;
        }

        public static T[] Reshuffle<T>(this IEnumerable<T> block, IEnumerable<int> table)
        {
            var pushedBlock = new[] {default(T)}.Concat(block);
            return table.Select(index => pushedBlock.ElementAt(index)).ToArray();
        }

        public static IEnumerable<T> TakeElements<T>(this IEnumerable<T> block, int count)
        {
            return block.Take(count);
        }

        public static IEnumerable<T> TakeLastElements<T>(this T[] block, int count)
        {
            return block.Skip(block.Length - count);
        }
    }
}