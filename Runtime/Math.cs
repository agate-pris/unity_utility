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

        /// <summary>
        /// pi / 2
        /// </summary>
        /// <param name="right">The right angle.</param>
        /// <returns>(int)Round(PI / 2 * right)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int SinP5K(int right) => (int)Round(PI / 2 * right);

        /// <summary>
        /// 5 * (1 - 3 / pi)
        /// </summary>
        /// <param name="right">The right angle.</param>
        /// <returns>(int)Round(5 * (1 - 3 / PI) * right)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CosP4OK(int right) => (int)Round(
            5 * (1 - 3 / PI) * right
        );

        /// <summary>
        /// 4 * (3 / pi - 9 / 16)
        /// </summary>
        /// <param name="right">The right angle.</param>
        /// <returns>(int)Round(4 * (3 / PI - 9 / 16.0) * right)</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int SinP5OK(int right) => (int)Round(
            4 * (3 / PI - 9 / 16.0) * right
        );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int SinP3CosP4Impl(int a, int b, int z_2, int right) => a - z_2 * b / right;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int CosP4SinP5Impl(int a, int b, int z, int right) {
            var z_2 = z * z / right;
            return SinP3CosP4Impl(a, b, z_2, right) * z_2;
        }

        /// <summary>
        /// 1 + k - k * x ^ 2
        /// </summary>
        /// <param name="k"></param>
        /// <param name="x"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static int SinP3Impl(int k, int x, int right) {
            var z = SinP1(x, right);
            return SinP3CosP4Impl(right + k, k, z * z / right, right) * z;
        }

        /// <summary>
        /// (k + 1) * z ^ 2 - k * z ^ 4
        /// </summary>
        /// <param name="f"></param>
        /// <param name="z"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int CosP4Impl(delegate*<int, int> f, int z, int right) {
            var k = f(right);
            return CosP4SinP5Impl(k + right, k, z, right);
        }

        /// <summary>
        /// k * x - (2 * k - 2.5) * x ^ 3 + (k - 1.5) * x ^ 5
        /// </summary>
        /// <param name="f"></param>
        /// <param name="x"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int SinP5Impl(delegate*<int, int> f, int x, int right) {
            var k = f(right);
            var z = SinP1(x, right);
            var a = k * 2 - right * 5 / 2;
            var b = k - right * 3 / 2;
            return (k - CosP4SinP5Impl(a, b, z, right) / right) * z;
        }

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

        /// <summary>
        /// 1 - x ^ 2
        /// </summary>
        /// <param name="x"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CosP2(int x, int right) {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static int f(int z, int _) => z * z;
            unsafe {
                return EvenCosImpl(x, right, &f);
            }
        }

        /// <summary>
        /// 1.5 * x - 0.5 * x ^ 3
        /// </summary>
        /// <param name="x"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SinP3(int x, int right) {
            // 1.5 * x - 0.5 * x ^ 3
            // = (1.5 - 0.5 * x ^ 2) * x
            return SinP3Impl(right / 2, x, right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CosP4(int x, int right) {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static int f(int z, int right) {
                unsafe {
                    return CosP4Impl(&CosP4K, z, right);
                }
            }
            unsafe {
               return EvenCosImpl(x, right, &f);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SinP5(int x, int right) {
            unsafe {
                return SinP5Impl(&SinP5K, x, right);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int CosP4O(int x, int right) {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static int f(int z, int right) {
                unsafe {
                    return CosP4Impl(&CosP4OK, z, right);
                }
            }
            unsafe {
                return EvenCosImpl(x, right, &f);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SinP5O(int x, int right) {
            unsafe {
                return SinP5Impl(&SinP5OK, x, right);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP2(int x, int right) => CosP2(EvenSinImpl(x, right), right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP4(int x, int right) => CosP4(EvenSinImpl(x, right), right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP1(int x, int right) => SinP1(OddCosImpl(x, right), right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP3(int x, int right) => SinP3(OddCosImpl(x, right), right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP5(int x, int right) => SinP5(OddCosImpl(x, right), right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP4O(int x, int right) => CosP4O(EvenSinImpl(x, right), right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP5O(int x, int right) => SinP5O(OddCosImpl(x, right), right);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP1(int x) => SinP1(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP2(int x) => SinP2(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP3(int x) => SinP3(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP4(int x) => SinP4(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP5(int x) => SinP5(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP1(int x) => CosP1(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP2(int x) => CosP2(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP3(int x) => CosP3(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP4(int x) => CosP4(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP5(int x) => CosP5(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP4O(int x) => SinP4O(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int SinP5O(int x) => SinP5O(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP4O(int x) => CosP4O(x, DefaultRight);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static int CosP5O(int x) => CosP5O(x, DefaultRight);
    }
}
