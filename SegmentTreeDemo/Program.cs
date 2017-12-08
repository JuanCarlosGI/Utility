using System;
using Utility.Structures;

namespace SegmentTreeDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // Start with ones. 0 is default value for padding and out-of-range queries, since 0 is neutral in additions
            var tree = new SegmentTree<long>(new long[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 }, 0, (x, y) => x + y);
            // Query individual nodes. Length does not include padding.
            for (var i = 0; i < tree.Length; i++) Console.Write($"{tree.QueryRange(i, i)} "); // Query individual nodes.
            Console.WriteLine("\n");

            // Can update range lazily by using start and end parameters. Add one for each in range.
            tree.UpdateRange(0, 2, (x, start, end) => x + end - start + 1);
            Console.WriteLine("Updated from 0 to 2, add 1:");
            for (var i = 0; i < tree.Length; i++) Console.Write($"{tree.QueryRange(i, i)} ");
            Console.WriteLine("\n");

            // Can do operations other than sum. Lazy since multiplication is distributive. However, some operations 
            //      which are not distributive could also be made, given the correct formula for updating ranges.
            tree.UpdateRange(1, 7, (x, start, end) => x * 3);
            Console.WriteLine("Updated from 1 to 7, times 3:");
            for (var i = 0; i < tree.Length; i++) Console.Write($"{tree.QueryRange(i, i)} ");
            Console.WriteLine("\n");

            // Can specify to not do operations lazily.
            tree.UpdateRange(5, 7, (x, start, end) => (long) Math.Pow(x, 3), false);
            Console.WriteLine("Updated from 5 to 7, cube:");
            for (var i = 0; i < tree.Length; i++) Console.Write($"{tree.QueryRange(i, i)} ");
            Console.WriteLine("\n");

            // Query ranges
            Console.WriteLine($"Query 4 to 8: {tree.QueryRange(4, 8)}\n");

            // Can query out-of-bound ranges
            Console.WriteLine($"Query -5 to -2: {tree.QueryRange(-5, -2)}\n");
            Console.WriteLine($"Query 0 to 1000: {tree.QueryRange(0, 1000)}\n");

            Console.Write("Press any key to continue...");
            Console.Read();
        }
    }
}
