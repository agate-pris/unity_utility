using NUnit.Framework;

namespace AgatePris.UnityUtility {
    public static class MathTests {
        [TestCase(9, -11, 10)]
        [TestCase(0, -10, 10)]
        [TestCase(1, -9, 10)]
        [TestCase(9, -1, 10)]
        [TestCase(0, 0, 10)]
        [TestCase(1, 1, 10)]
        [TestCase(9, 9, 10)]
        [TestCase(0, 10, 10)]
        [TestCase(1, 11, 10)]
        public static void TestRepeat(int expected, int value, int length) => Assert.AreEqual(expected, Math.Repeat(value, length));
    }
}
