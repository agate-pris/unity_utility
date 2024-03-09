using NUnit.Framework;
using System.Collections.Generic;
using static AgatePris.UnityUtility.Math;

namespace AgatePris.UnityUtility {
    public static class MathTests {
        static IEnumerable<object> TestRepeatSource() {
            const int length = 10;
            for (var i = -9; i <= 9; i++) {
                yield return new object[] { 9, length * i - 1, length };
                yield return new object[] { 0, length * i + 0, length };
                yield return new object[] { 1, length * i + 1, length };
            }
        }

        [TestCaseSource(nameof(TestRepeatSource))]
        public static void TestRepeat(int expected, int value, int length) => Assert.AreEqual(
            expected, Repeat(value, length)
        );

        static IEnumerable<object> TestCalcQuadrantSource() {
            const int right = 25;
            for (var i = -9; i <= 9; i++) {
                var offset = 4 * i * right;
                for (var expected = 0; expected < 4; expected++) {
                    yield return new object[] { (byte)expected, offset + right * (expected + 0) + 0, right };
                    yield return new object[] { (byte)expected, offset + right * (expected + 0) + 1, right };
                    yield return new object[] { (byte)expected, offset + right * (expected + 1) - 1, right };
                }
            }
        }

        [TestCaseSource(nameof(TestCalcQuadrantSource))]
        public static void TestCalcQuadrant(byte expected, int x, int right) => Assert.AreEqual(
            expected, CalcQuadrant(x, right)
        );
    }
}
