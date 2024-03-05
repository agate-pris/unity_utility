using System.Runtime.CompilerServices;

namespace AgatePris.UnityUtility {
    public static partial class Math {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Repeat(int value, int length) {
            var rem = value % length;
            if (rem < 0) {
                return rem + length;
            }
            return rem;
        }
    }
}
