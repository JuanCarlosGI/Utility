using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Utility.Structures
{
    /// <summary>
    /// Structure used for querrying or modifying segments in a range of vaules.
    /// </summary>
    /// <typeparam name="T">The tyoe of elements the tree stores.</typeparam>
    public class SegmentTree<T>
    {
        private readonly T[] _tree;
        private readonly Func<T, T, T> _joinFunc;
        private readonly T _default;
        private readonly Func<T, long, long, T>[] _lazy;
        private readonly long _realLength;

        /// <summary>
        /// Gets the total amount of leaf nodes that the tree has, before padding.
        /// </summary>
        public long Length { get; }

        /// <summary>
        /// Initializes an instance of the <see cref="SegmentTree{T}"/> class. Applies padding up to the closest power
        /// of two.
        /// </summary>
        /// <param name="source">The set of elements contained in the tree.</param>
        /// <param name="defaultValue">The value that will be used as padding. Make sure that this element is neutral 
        /// to the join function.</param>
        /// <param name="joinFunction">The function that determines the new value resulting from joining its
        /// two children nodes. Receives its two children (left, right), and returns the result of joining them.</param>
        public SegmentTree(IReadOnlyList<T> source, T defaultValue, Func<T,T,T> joinFunction)
        {
            Length = source.Count;
            _realLength = (long)Math.Pow(2, Math.Ceiling(Math.Log(source.Count, 2)));
            var src = new T[_realLength];
            for (var i = 0; i < src.Length; i++) src[i] = i < source.Count ? source[i] : defaultValue;
            _default = defaultValue;
            _tree = new T[_realLength * 2 - 1];
            _lazy = new Func<T, long, long, T>[_tree.Length];
            _joinFunc = joinFunction;

            Build(0, 0, _realLength - 1, src);
        }

        private void Build(long node, long start, long end, T[] source)
        {
            if (start == end)
                _tree[node] = source[start];
            else
            {
                var mid = start + (end - start) / 2;
                Build(Left(node), start, mid, source);
                Build(Right(node), mid + 1, end, source);
                _tree[node] = _joinFunc(_tree[Left(node)], _tree[Left(node) + 1]);
            }
        }

        /// <summary>
        /// Updates a range of elements in the tree.
        /// </summary>
        /// <param name="left">Left bound of the range. 0-indexed.</param>
        /// <param name="right">Right bound of the range. 0-indexed.</param>
        /// <param name="updateFunc">The function used to update the nodes. To enable lazy updating, it receives
        /// both the value of the node it is visiting, and the range which it represents.</param>
        /// <param name="beLazy">Whether the operation can be executed in a lazy manner. True by default.</param>
        public void UpdateRange(long left, long right, Func<T, long, long, T> updateFunc, bool beLazy = true)
        {
            UpdateRange(0, 0, _realLength - 1, left, right, updateFunc, beLazy);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateRange(long node, long start, long end, long l, long r, Func<T, long, long, T> updateFunc, bool beLazy)
        {
            ExecuteLazy(node, start, end);

            if (start > end || start > r || end < l)
                return;
            
            if (start >= l && end <= r)
            {
                if (start == end)
                {
                    _tree[node] = updateFunc(_tree[node], start, end);
                    return;
                }

                if (beLazy)
                {
                    _lazy[node] = updateFunc;
                    ExecuteLazy(node, start, end);
                    return;
                }
            }

            var mid = start + (end - start) / 2;
            UpdateRange(Left(node), start, mid, l, r, updateFunc, beLazy);
            UpdateRange(Right(node), mid + 1, end, l, r, updateFunc, beLazy);
            _tree[node] = _joinFunc(_tree[Left(node)], _tree[Right(node)]);
        }

        /// <summary>
        /// Queries a range of the segment tree.
        /// </summary>
        /// <param name="left">Left bound of the range. 0-based.</param>
        /// <param name="right">Right bound of the range. 0-based.</param>
        /// <returns>The value representing the complete range.</returns>
        public T QueryRange(long left, long right)
        {
            return QueryRange(0, 0, _realLength - 1, left, right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T QueryRange(long node, long start, long end, long l, long r)
        {
            if (start > end || start > r || end < l)
                return _default;

            ExecuteLazy(node, start, end);

            if (start >= l && end <= r)
                return _tree[node];

            var mid = start + (end - start) / 2;
            var p1 = QueryRange(Left(node), start, mid, l, r);
            var p2 = QueryRange(Right(node), mid + 1, end, l, r);

            return _joinFunc(p1, p2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ExecuteLazy(long node, long start, long end)
        {
            if (_lazy[node] == null) return;

            var op = _lazy[node];
            _tree[node] = op(_tree[node], start, end);
            if (start != end)
            {
                var opl = _lazy[Left(node)];
                _lazy[Left(node)] = opl == null ? op : (x, s, e) => op(opl(x, s, e), s, e);
                var opr = _lazy[Right(node)];
                _lazy[Right(node)] = opr == null ? op : (x, s, e) => op(opr(x, s, e), s, e);
            }
            _lazy[node] = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long Left(long node)
        {
            return node * 2 + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static long Right(long node)
        {
            return node * 2 + 2;
        }
    }
}
