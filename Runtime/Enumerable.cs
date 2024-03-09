using System;
using System.Collections.Generic;

namespace AgatePris.UnityUtility {
    public static class Enumerable {
        public static IEnumerable<T> Once<T>(T value) {
            yield return value;
        }
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> source) {
            if (source == null) {
                throw new ArgumentNullException("source");
            }
            while (true) {
                using var e = source.GetEnumerator();
                while (e.MoveNext()) {
                    yield return e.Current;
                }
            }
        }
        public static IEnumerable<int> RangeExclusive(int start, int end) {
            for (var i = start; i < end; i++) {
                yield return i;
            }
        }
        public static IEnumerable<int> RangeInclusive(int start, int end) {
            if (end == int.MaxValue) {
                for (var i = start; i < end; i++) {
                    yield return i;
                }
                yield return int.MaxValue;
                yield break;
            }
            for (var i = start; i <= end; i++) {
                yield return i;
            }
        }
    }
}
