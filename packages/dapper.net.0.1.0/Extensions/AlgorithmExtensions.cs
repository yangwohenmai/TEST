using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dapper.Net.Security.Cryptography;

namespace Dapper.Net.Extensions {

    /// <summary>
    /// Algorithmic extension methods
    /// </summary>
    public static class AlgorithmExtensions {
        private static readonly ThreadLocal<Random> Rng = new ThreadLocal<Random>(() => new Random());
        private static readonly ThreadLocal<CryptoRandom> RngCrypto = new ThreadLocal<CryptoRandom>(() => new CryptoRandom());

        /// <summary>
        /// Shuffle a sequence using Fisher-Yates shuffle algorithm.
        /// </summary>
        /// <typeparam name="T">The type of sequence to shuffle.</typeparam>
        /// <param name="sequence">The sequence to shuffle.</param>
        /// <param name="rng">The random number generator to use (defaults to thread-local Random).</param>
        /// <returns>The same sequence shuffled randomly.</returns>
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> sequence, Random rng = null) {
            var elements = sequence.ToArray();
            rng = rng ?? Rng.Value;
            for (var i = elements.Length - 1; i >= 0; i--) {
                var swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        /// <summary>
        /// Shuffle a sequence using Fisher-Yates shuffle algorithm and a crypto random generator.
        /// </summary>
        /// <typeparam name="T">The type of the sequence to crypto shuffle.</typeparam>
        /// <param name="sequence">The sequence to crypto shuffle.</param>
        /// <param name="rng">The crypto random number generator to use (defaults to thread-local CryptoRandom).</param>
        /// <returns>The same sequence crypto shuffled randomly.</returns>
        public static IEnumerable<T> ShuffleCrypto<T>(this IEnumerable<T> sequence, CryptoRandom rng = null) {
            var elements = sequence.ToArray();
            rng = rng ?? RngCrypto.Value;
            for (var i = elements.Length - 1; i >= 0; i--) {
                var swapIndex = rng.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }

        /// <summary>
        /// Calculates the mean of a sequence.
        /// </summary>
        /// <param name="sequence">The doubles to calculate the mean of.</param>
        /// <returns>The mean of the sequence.</returns>
        public static double Mean(this IEnumerable<double> sequence) => sequence.ToList().Mean();

        /// <summary>
        /// Calculates the mean of a list.
        /// </summary>
        /// <param name="list">The doubles to calculate the mean of.</param>
        /// <returns>The mean of the list.</returns>
        public static double Mean(this IList<double> list) => Mean(list, d => d);

        /// <summary>
        /// Calculates the mean of a generic sequence.
        /// </summary>
        /// <param name="sequence">The generic sequence to calculate the mean of.</param>
        /// <param name="toDouble">The function to convert each item to double.</param>
        /// <returns>The mean of the sequence.</returns>
        public static double Mean<T>(this IEnumerable<T> sequence, Func<T, double> toDouble) => sequence.ToList().Mean(toDouble);

        /// <summary>
        /// Calculates the mean of a generic list.  
        /// </summary>
        /// <typeparam name="T">The type of list to calculate the mean of.</typeparam>
        /// <param name="list">The generic list to calculate the mean of.</param>
        /// <param name="toDouble">The function to convert each item to double.</param>
        /// <returns>The mean of the sequence.</returns>
        public static double Mean<T>(this IList<T> list, Func<T, double> toDouble) => list.Select(toDouble).Aggregate(0d, (agg, item) => agg + item, total => total/Enumerable.Count(list));

        /// <summary>
        /// Calculates the variance of a sequence of doubles.
        /// </summary>
        /// <param name="sequence">The doubles to calculate the variance of.</param>
        /// <returns>The variance of the list.</returns>
        public static double Variance(this IEnumerable<double> sequence) => sequence.ToList().Variance();

        /// <summary>
        /// Calculates the variance of a list of doubles.
        /// </summary>
        /// <param name="list">The doubles to calculate the variance of.</param>
        /// <returns>The variance of the list.</returns>
        public static double Variance(this IList<double> list) => list.Variance(list.Mean());

        /// <summary>
        /// Calculates the variance of a list of doubles.
        /// </summary>
        /// <param name="list">The doubles to calculate the variance of.</param>
        /// <param name="mean">The mean value for the list.</param>
        /// <returns>The variance of the list.</returns>
        public static double Variance(this IList<double> list, double mean) => list.Variance(mean, d => d);

        /// <summary>
        /// Calculates the variance of a generic sequence.
        /// </summary>
        /// <typeparam name="T">The type of generic sequence to calculate the variance of.</typeparam>
        /// <param name="sequence">The generic sequence to calculate the variance of.</param>
        /// <param name="toDouble">The function to convert each item to double.</param>
        /// <returns>The variance of the list.</returns>
        public static double Variance<T>(this IEnumerable<T> sequence, Func<T, double> toDouble) => sequence.ToList().Variance(toDouble);

        /// <summary>
        /// Calculates the variance of a generic sequence.
        /// </summary>
        /// <typeparam name="T">The type of generic sequence to calculate the variance of.</typeparam>
        /// <param name="sequence">The generic sequence to calculate the variance of.</param>
        /// <param name="mean">The mean value for the list.</param>
        /// <param name="toDouble">The function to convert each item to double.</param>
        /// <returns>The variance of the list.</returns>
        public static double Variance<T>(this IEnumerable<T> sequence, double mean, Func<T, double> toDouble) => sequence.ToList().Variance(mean, toDouble);

        /// <summary>
        /// Calculates the variance of a generic list.
        /// </summary>
        /// <typeparam name="T">The type of generic list to calculate the variance of.</typeparam>
        /// <param name="list">The generic list to calculate the variance of.</param>
        /// <param name="toDouble">The function to convert each item to double.</param>
        /// <returns>The variance of the list.</returns>
        public static double Variance<T>(this IList<T> list, Func<T, double> toDouble) => list.Variance(list.Mean(toDouble), toDouble);

        /// <summary>
        /// Calculates the variance of a generic list.
        /// </summary>
        /// <typeparam name="T">The type of generic list to calculate the variance of.</typeparam>
        /// <param name="list">The generic list to calculate the variance of.</param>
        /// <param name="mean">The mean value for the list.</param>
        /// <param name="toDouble">The function to convert each item to double.</param>
        /// <returns>The variance of the list.</returns>
        public static double Variance<T>(this IList<T> list, double mean, Func<T, double> toDouble) => list.Count == 0 ? 0 : list.Select(toDouble).Aggregate(0d, (agg, item) => Math.Pow(item - mean, 2), total => total/(list.Count - 1));

        /// <summary>
        /// Calculates the median of a sequence of doubles.
        /// </summary>
        /// <param name="sequence">The sequence to operate on.</param>
        /// <returns>The median of the sequence.</returns>
        public static double Median(this IEnumerable<double> sequence) {
            var list = sequence.ToList();
            var mid = (list.Count - 1)/2;
            return list.NthOrderStatistic(mid);
        }

        /// <summary>
        /// Calculates the median of a sequence of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in sequence.</typeparam>
        /// <param name="sequence">The sequence to operate on.</param>
        /// <param name="getValue">Logic to get a double from each element.</param>
        /// <returns>The median of the sequence.</returns>
        public static double Median<T>(this IEnumerable<T> sequence, Func<T, double> getValue) => Median(sequence.Select(getValue));

        /// <summary>
        /// Gets the median member of a list of elements.
        /// </summary>
        /// <typeparam name="T">Type of elements in the list.</typeparam>
        /// <param name="list">The list to operate on.</param>
        /// <returns>The median of the list.</returns>
        public static T Median<T>(this IList<T> list) where T : IComparable<T> => list.NthOrderStatistic((list.Count - 1)/2);

        /// <summary>
        /// Calculates the standard deviation of a sequence of doubles.
        /// </summary>
        /// <param name="sequence">The sequence to operate on.</param>
        /// <returns>The standard deviation of the sequence.</returns>
        public static double StandardDeviation(this IEnumerable<double> sequence) {
            var list = sequence.ToList();
            var avg = list.Average();
            return Math.Sqrt(list.Average(v => Math.Pow(v - avg, 2)));
        }

        /// <summary>
        /// Calculates the standard deviation of a sequence of elements.
        /// </summary>
        /// <typeparam name="T">The type of elements in sequence.</typeparam>
        /// <param name="sequence">The sequence to operate on.</param>
        /// <param name="getValue">Logic to get a double from each element.</param>
        /// <returns>The standard deviation of the sequence.</returns>
        public static double StandardDeviation<T>(this IEnumerable<T> sequence, Func<T, double> getValue) => sequence.Select(getValue).StandardDeviation();


        /// <summary>
        /// Partitions the given list around a pivot element such that all elements on left of pivot are less than or equal to pivot
        /// Elements to right of the pivot are guaranteed greater than the pivot. Can be used for sorting N-order statistics such
        /// as median finding algorithms.
        /// Pivot is selected randomly if random number generator is supplied else its selected as last element in the list.
        /// </summary>
        private static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) where T : IComparable<T> {
            if (rnd != null) list.Swap(end, rnd.Next(start, end));
            var pivot = list[end];
            var lastLow = start - 1;
            for (var i = start; i < end; i++)
                if (list[i].CompareTo(pivot) <= 0) list.Swap(i, ++lastLow);
            list.Swap(end, ++lastLow);
            return lastLow;
        }

        /// <summary>
        /// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
        /// Note: specified list would be mutated in the process.
        /// </summary>
        public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) where T : IComparable<T> => NthOrderStatistic(list, n, 0, list.Count - 1, rnd);

        private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) where T : IComparable<T> {
            while (true) {
                var pivotIndex = list.Partition(start, end, rnd);
                if (pivotIndex == n) return list[pivotIndex];
                if (n < pivotIndex) end = pivotIndex - 1;
                else start = pivotIndex + 1;
            }
        }

        /// <summary>
        /// Swap two elements positions in a list.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="list">The list to swap on.</param>
        /// <param name="i">The first element position to swap.</param>
        /// <param name="j">The second element position to swap.</param>
        public static void Swap<T>(this IList<T> list, int i, int j) {
            if (i == j) return;
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

    }

}