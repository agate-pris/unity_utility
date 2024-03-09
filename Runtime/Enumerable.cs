using System.Collections.Generic;

namespace AgatePris.UnityUtility {
    public static class Enumerable {
        public static IEnumerable<T> Once<T>(T value) {
            yield return value;
        }
    }
}
