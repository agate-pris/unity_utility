using System.Runtime.CompilerServices;

namespace AgatePris.UnityUtility {
    public static partial class Math {
        internal const int DefaultRight = 1 << 15;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Repeat(int value, int length) {
            var rem = value % length;
            if (rem < 0) {
                return rem + length;
            }
            return rem;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int CalcFull(int right) => right * 4;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static byte CalcQuadrant(int x, int right) => (byte)(
            Repeat(x, CalcFull(right)) / right
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int OddCosImpl(int x, int right) => (
            x % CalcFull(right)
        ) + right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int EvenSinImpl(int x, int right) => (
            x % CalcFull(right)
        ) - right;
    }
}
