using System;
using System.Runtime.CompilerServices;
using static System.Math;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int EvenCosImpl(int x, int right, delegate*<int, int, int> f) {
            var rem = Repeat(x, right);
            var k = right * right;
            return CalcQuadrant(x, right) switch {
                1 => -k + f(right - rem, right),
                3 => k - f(right - rem, right),
                2 => -k + f(rem, right),
                0 => k - f(rem, right),
                _ => throw new InvalidOperationException("Invalid quadrant"),
            };
        }

        /// <summary>
        /// 1 - pi / 4
        /// </summary>
        /// <param name="right">The right angle.</param>
        /// <returns>(int)Round((1 - PI / 4) * right)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CosP4K(int right) => (int)Round((1 - PI / 4) * right);

        /// <summary>x</summary>
        /// <param name="x"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SinP1(int x, int right) {
            var rem = Repeat(x, right);
            return CalcQuadrant(x, right) switch {
                1 => -rem + right,
                3 => rem - right,
                2 => -rem,
                0 => rem,
                _ => throw new InvalidOperationException("Invalid quadrant"),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP1(int x, int right) => SinP1(OddCosImpl(x, right), right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP1(int x) => SinP1(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP1(int x) => CosP1(x, DefaultRight);
    }
}
