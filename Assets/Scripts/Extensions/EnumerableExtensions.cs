using System;
using System.Collections.Generic;

namespace Extensions
{
    public static class EnumerableExtensions
    {
        public static void Fill<T>(this ICollection<T> collection, Func<int, T> fillFunc, int size)
        {
            for (var i = 0; i < size; i++)
            {
                collection.Add(fillFunc(i));
            }
        }

        public static void Fill<T>(this T[] collection, T value)
        {
            for (var i = 0; i < collection.Length; i++)
            {
                collection[i] = value;
            }
        }

        public static void Fill<T>(this T[] collection, Func<int, T> fillFunc)
        {
            for (var i = 0; i < collection.Length; i++)
            {
                collection[i] = fillFunc(i);
            }
        }

        public static void Shuffle<T>(this T[] array)
        {
            RandomExt.Shuffle(array);
        }

        public static T[] Concat<T>(this T[] array, T elementToAdd)
        {
            var newArr = new T[array.Length + 1];
            for (int i = 0; i < array.Length; i++)
            {
                newArr[i] = array[i];
            }

            newArr[newArr.Length - 1] = elementToAdd;

            return newArr;
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, null);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer ??= Comparer<TKey>.Default;

            using var sourceIterator = source.GetEnumerator();
            if (!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            var min = sourceIterator.Current;
            var minKey = selector(min);
            while (sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if (comparer.Compare(candidateProjected, minKey) < 0)
                {
                    min = candidate;
                    minKey = candidateProjected;
                }
            }

            return min;
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, null);
        }

        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            comparer ??= Comparer<TKey>.Default;

            using var sourceIterator = source.GetEnumerator();
            if (!sourceIterator.MoveNext())
            {
                throw new InvalidOperationException("Sequence contains no elements");
            }

            var max = sourceIterator.Current;
            var maxKey = selector(max);
            while (sourceIterator.MoveNext())
            {
                var candidate = sourceIterator.Current;
                var candidateProjected = selector(candidate);
                if (comparer.Compare(candidateProjected, maxKey) > 0)
                {
                    max = candidate;
                    maxKey = candidateProjected;
                }
            }

            return max;
        }
    }
}