using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility.Structures;

namespace UnitTests
{
    [TestClass]
    public class SegmentTreeTests
    {
        [TestMethod]
        public void PartialSums()
        {
            var tree = new SegmentTree<long>(new long[]{1, 1, 1, 1, 1, 1, 1, 1, 1}, 0, (x, y) => x + y);
            tree.UpdateRange(0, 2, (x, s, e) => x + e - s + 1);
            Assert.AreEqual(tree.QueryRange(0, 9),12);
        }
    }
}
