using NUnit.Framework;
using System.Linq;
using static AgatePris.UnityUtility.Enumerable;

namespace AgatePris.UnityUtility {
    public static class EnumerableTests {
        [Test]
        public static void TestOnce() {
            var l = Once(1).ToList();
            Assert.AreEqual(1, l.Count);
            Assert.AreEqual(1, l[0]);
        }

        [Test]
        public static void TestCycle() {
            var l = new[] { 1, 2, 3 }.Cycle().Take(5).ToList();
            Assert.AreEqual(5, l.Count);
            Assert.AreEqual(1, l[0]);
            Assert.AreEqual(2, l[1]);
            Assert.AreEqual(3, l[2]);
            Assert.AreEqual(1, l[3]);
            Assert.AreEqual(2, l[4]);
        }

        [Test]
        public static void TestRangeExclusive() {
            var l = RangeExclusive(int.MaxValue - 1, int.MaxValue).ToList();
            Assert.AreEqual(1, l.Count);
            Assert.AreEqual(int.MaxValue - 1, l[0]);
        }
    }
}
