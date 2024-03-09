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
    }
}
