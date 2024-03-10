using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static AgatePris.UnityUtility.Math;

namespace AgatePris.UnityUtility {
    public static class MathTests {
        [Serializable]
        class ArrayWrapper<T> {
            public T[] root;
        }

        static T[] ArrayFromJson<T>(string text) {
            var json = "{\"root\":" + text + "}";
            return JsonUtility.FromJson<ArrayWrapper<T>>(json).root;
        }

        static void TestPeriodicity(int x, Func<int, int> f) {
            const int full = 4 * DefaultRight;
            Assert.IsTrue(x >= 0);
            Assert.IsTrue(x < full);
            var expected = (2 * DefaultRight <= x) ? f(x - full) : f(x);
            Debug.Log($"expected: {expected}");
            for (var i = x + int.MinValue; i <= int.MaxValue; i += full) {
                Assert.AreEqual(expected, f(i));
                if (i > 0 && int.MaxValue - i < full) {
                    break;
                }
            }
        }

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

        [Test] public static void TestCosP4K() => Assert.AreEqual(7032, CosP4K(DefaultRight));
        [Test] public static void TestSinP5K() => Assert.AreEqual(51472, SinP5K(DefaultRight));
        [Test] public static void TestCosP4OK() => Assert.AreEqual(7384, CosP4OK(DefaultRight));
        [Test] public static void TestSinP5OK() => Assert.AreEqual(51437, SinP5OK(DefaultRight));

        static IEnumerable<object> TestSinP1Source() {
            const int right = 25;
            for (var i = -9; i <= 9; i++) {
                yield return new object[] { 0 * right - 1, (0 + 4 * i) * right - 1, right };
                yield return new object[] { 0 * right + 0, (0 + 4 * i) * right + 0, right };
                yield return new object[] { 0 * right + 1, (0 + 4 * i) * right + 1, right };
                yield return new object[] { 1 * right - 1, (1 + 4 * i) * right - 1, right };
                yield return new object[] { 1 * right + 0, (1 + 4 * i) * right + 0, right };
                yield return new object[] { 1 * right - 1, (1 + 4 * i) * right + 1, right };
                yield return new object[] { 0 * right + 1, (2 + 4 * i) * right - 1, right };
                yield return new object[] { 0 * right + 0, (2 + 4 * i) * right + 0, right };
                yield return new object[] { 0 * right - 1, (2 + 4 * i) * right + 1, right };
                yield return new object[] { 1 - 1 * right, (3 + 4 * i) * right - 1, right };
                yield return new object[] { 0 - 1 * right, (3 + 4 * i) * right + 0, right };
                yield return new object[] { 1 - 1 * right, (3 + 4 * i) * right + 1, right };
            }
        }
        static IEnumerable<object> TestCosP1Source() {
            const int right = 25;
            for (var i = -9; i <= 9; i++) {
                yield return new object[] { 1 * right - 1, (0 + 4 * i) * right - 1, right };
                yield return new object[] { 1 * right + 0, (0 + 4 * i) * right + 0, right };
                yield return new object[] { 1 * right - 1, (0 + 4 * i) * right + 1, right };
                yield return new object[] { 0 * right + 1, (1 + 4 * i) * right - 1, right };
                yield return new object[] { 0 * right + 0, (1 + 4 * i) * right + 0, right };
                yield return new object[] { 0 * right - 1, (1 + 4 * i) * right + 1, right };
                yield return new object[] { 1 - 1 * right, (2 + 4 * i) * right - 1, right };
                yield return new object[] { 0 - 1 * right, (2 + 4 * i) * right + 0, right };
                yield return new object[] { 1 - 1 * right, (2 + 4 * i) * right + 1, right };
                yield return new object[] { 0 * right - 1, (3 + 4 * i) * right - 1, right };
                yield return new object[] { 0 * right + 0, (3 + 4 * i) * right + 0, right };
                yield return new object[] { 0 * right + 1, (3 + 4 * i) * right + 1, right };
            }
        }

        [TestCaseSource(nameof(TestSinP1Source))] public static void TestSinP1(int expected, int x, int right) => Assert.AreEqual(expected, SinP1(x, right));
        [TestCaseSource(nameof(TestCosP1Source))] public static void TestCosP1(int expected, int x, int right) => Assert.AreEqual(expected, CosP1(x, right));

        static IEnumerable<int> TestPeriodicitySource() {
            for (var i = 0; i < 4; i++) {
                yield return (0 + i) * DefaultRight + 0;
                yield return (0 + i) * DefaultRight + 1;
                yield return (1 + i) * DefaultRight - 1;
            }
        }

        [TestCaseSource(nameof(TestPeriodicitySource))] public static void TestSinP1Periodicity(int x) => TestPeriodicity(x, SinP1);
        [TestCaseSource(nameof(TestPeriodicitySource))] public static void TestCosP1Periodicity(int x) => TestPeriodicity(x, CosP1);
    }
}
