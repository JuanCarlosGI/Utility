using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility.Algorithms;

namespace UnitTests
{
    [TestClass]
    public class BinarySearchTests
    {
        [TestMethod]
        public void IndexOfFindEven()
        {
            var arr = new int[8];
            for (var i = 0; i < 8; i++)
                arr[i] = i;

            Assert.AreEqual(BinarySearch.IndexOf(arr, 4, (i, j) => i - j), 4);
        }

        [TestMethod]
        public void IndexOfFindOdd()
        {
            var arr = new int[7];
            for (var i = 0; i < 7; i++)
                arr[i] = i;

            Assert.AreEqual(BinarySearch.IndexOf(arr, 4, (i, j) => i - j), 4);
        }

        [TestMethod]
        public void IndexOfNoFind()
        {
            var arr = new int[7];
            for (var i = 0; i < 7; i++)
                arr[i] = i;

            Assert.AreEqual(BinarySearch.IndexOf(arr, 10, (i, j) => i - j), -1);
        }

        [TestMethod]
        public void IndexOfLastEven()
        {
            var arr = new int[8];
            for (var i = 0; i < 8; i++)
                arr[i] = i;

            Assert.AreEqual(BinarySearch.IndexOfLast(arr, i => i <= 4), 4);
        }

        [TestMethod]
        public void IndexOfLastOdd()
        {
            var arr = new int[7];
            for (var i = 0; i < 7; i++)
                arr[i] = i;

            Assert.AreEqual(BinarySearch.IndexOfLast(arr, i => i <= 4), 4);
        }

        [TestMethod]
        public void IndexOfLastNoFind()
        {
            var arr = new int[7];
            for (var i = 0; i < 7; i++)
                arr[i] = i;

            Assert.AreEqual(BinarySearch.IndexOfLast(arr, i => i < 0), -1);
        }

        [TestMethod]
        public void LastInRange()
        {
            Assert.AreEqual(BinarySearch.LastInRange(5, 10, l => l <= 8), 8);
        }
    }
}
