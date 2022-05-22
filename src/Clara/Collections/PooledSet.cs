﻿using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Clara.Collections
{
    [DebuggerTypeProxy(typeof(PooledSetDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    internal sealed class PooledSet<TItem> : IReadOnlyCollection<TItem>, IDisposable
        where TItem : notnull, IEquatable<TItem>
    {
        private const int MinimumCapacity = 4;
        private const int StackAllocThreshold = 512;

        private static readonly ArrayPool<int> BucketsPool = ArrayPool<int>.Shared;
        private static readonly ArrayPool<Entry> EntriesPool = ArrayPool<Entry>.Shared;
        private static readonly Entry[] InitialEntries = new Entry[1];

        private int size;
        private int count;
        private int freeList;
        private int[] buckets;
        private Entry[] entries;

        [DebuggerDisplay("({Item})->{Next}")]
        private struct Entry
        {
            public TItem Item;
            public int Next;
        }

        public PooledSet()
        {
            this.size = 1;
            this.count = 0;
            this.freeList = -1;
            this.buckets = HashHelpers.SizeOneIntArray;
            this.entries = InitialEntries;
        }

        public PooledSet(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            if (capacity < MinimumCapacity)
            {
                capacity = MinimumCapacity;
            }

            this.size = HashHelpers.PowerOf2(capacity);
            this.count = 0;
            this.freeList = -1;
            this.buckets = BucketsPool.Rent(this.size);
            this.entries = EntriesPool.Rent(this.size);

            Array.Clear(this.buckets, 0, this.size);
            Array.Clear(this.entries, 0, this.size);
        }

        public PooledSet(IEnumerable<TItem> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is PooledSet<TItem> source)
            {
                if (source.size == 1)
                {
                    this.size = 1;
                    this.count = 0;
                    this.freeList = -1;
                    this.buckets = HashHelpers.SizeOneIntArray;
                    this.entries = InitialEntries;
                }
                else
                {
                    this.size = source.size;
                    this.count = source.count;
                    this.freeList = source.freeList;
                    this.buckets = BucketsPool.Rent(this.size);
                    this.entries = EntriesPool.Rent(this.size);

                    Array.Copy(source.buckets, 0, this.buckets, 0, this.size);
                    Array.Copy(source.entries, 0, this.entries, 0, this.size);
                }

                return;
            }

            if (collection is IReadOnlyCollection<TItem> readOnlyCollection)
            {
                var capacity = readOnlyCollection.Count;

                if (capacity < MinimumCapacity)
                {
                    capacity = MinimumCapacity;
                }

                this.size = HashHelpers.PowerOf2(capacity);
                this.count = 0;
                this.freeList = -1;
                this.buckets = BucketsPool.Rent(this.size);
                this.entries = EntriesPool.Rent(this.size);

                Array.Clear(this.buckets, 0, this.size);
                Array.Clear(this.entries, 0, this.size);
            }
            else
            {
                this.size = 1;
                this.count = 0;
                this.freeList = -1;
                this.buckets = HashHelpers.SizeOneIntArray;
                this.entries = InitialEntries;
            }

            foreach (var item in collection)
            {
                this.Add(item);
            }
        }

        public int Count
        {
            get
            {
                return this.count;
            }
        }

        public void Clear()
        {
            if (this.size > 1)
            {
                BucketsPool.Return(this.buckets, clearArray: false);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                EntriesPool.Return(this.entries, clearArray: RuntimeHelpers.IsReferenceOrContainsReferences<Entry>());
#else
                EntriesPool.Return(this.entries, clearArray: true);
#endif
            }

            this.size = 1;
            this.count = 0;
            this.freeList = -1;
            this.buckets = HashHelpers.SizeOneIntArray;
            this.entries = InitialEntries;
        }

        public bool Contains(TItem item)
        {
            return this.FindItemIndex(item) >= 0;
        }

        public bool Add(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var entries = this.entries;
            var collisionCount = 0;
            var bucketIndex = item.GetHashCode() & (this.size - 1);

            for (var i = this.buckets[bucketIndex] - 1; (uint)i < (uint)this.size; i = entries[i].Next)
            {
                if (item.Equals(entries[i].Item))
                {
                    return false;
                }

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            this.Add(item, bucketIndex);

            return true;
        }

        public bool Remove(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var entries = this.entries;
            var bucketIndex = item.GetHashCode() & (this.size - 1);
            var entryIndex = this.buckets[bucketIndex] - 1;

            var lastIndex = -1;
            var collisionCount = 0;

            while (entryIndex != -1)
            {
                var candidate = entries[entryIndex];

                if (candidate.Item.Equals(item))
                {
                    if (lastIndex != -1)
                    {
                        entries[lastIndex].Next = candidate.Next;
                    }
                    else
                    {
                        this.buckets[bucketIndex] = candidate.Next + 1;
                    }

                    entries[entryIndex] = default;
                    entries[entryIndex].Next = -3 - this.freeList;
                    this.freeList = entryIndex;

                    this.count--;
                    return true;
                }

                lastIndex = entryIndex;
                entryIndex = candidate.Next;

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            return false;
        }

        public void IntersectWith(IEnumerable<TItem> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.Count == 0 || other == this)
            {
                return;
            }

            if (other is IReadOnlyCollection<TItem> otherReadOnlyCollection)
            {
                if (otherReadOnlyCollection.Count == 0)
                {
                    this.Clear();

                    return;
                }
            }

            if (other is ICollection<TItem> otherCollection)
            {
                if (otherCollection.Count == 0)
                {
                    this.Clear();

                    return;
                }
            }

            if (other is PooledSet<TItem> source)
            {
                var count = this.count;

                for (var i = 0; count > 0; i++)
                {
                    ref var entry = ref this.entries[i];

                    if (entry.Next >= -1)
                    {
                        count--;

                        var item = entry.Item;

                        if (!source.Contains(item))
                        {
                            this.Remove(item);
                        }
                    }
                }
            }
            else
            {
                var lastIndex = this.FindLastIndex();
                var intArrayLength = BitHelper.ToIntArrayLength(lastIndex);

                Span<int> span = stackalloc int[StackAllocThreshold];

                var bitHelper = intArrayLength <= StackAllocThreshold
                    ? new BitHelper(span.Slice(0, intArrayLength), clear: true)
                    : new BitHelper(new int[intArrayLength], clear: false);

                foreach (var item in other)
                {
                    var index = this.FindItemIndex(item);

                    if (index >= 0)
                    {
                        bitHelper.MarkBit(index);
                    }
                }

                for (var i = bitHelper.FindFirstUnmarked(); i < lastIndex; i = bitHelper.FindFirstUnmarked(i + 1))
                {
                    ref var entry = ref this.entries[i];

                    if (entry.Next >= -1)
                    {
                        if (!bitHelper.IsMarked(i))
                        {
                            this.Remove(entry.Item);
                        }
                    }
                }
            }
        }

        public void UnionWith(IEnumerable<TItem> other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (other is PooledSet<TItem> source)
            {
                if (this.size == 1)
                {
                    this.size = source.size;
                    this.count = source.count;
                    this.freeList = source.freeList;
                    this.buckets = BucketsPool.Rent(this.size);
                    this.entries = EntriesPool.Rent(this.size);

                    Array.Copy(source.buckets, 0, this.buckets, 0, this.size);
                    Array.Copy(source.entries, 0, this.entries, 0, this.size);
                }
                else
                {
                    var count = source.count;

                    for (var i = 0; count > 0; i++)
                    {
                        ref var entry = ref source.entries[i];

                        if (entry.Next >= -1)
                        {
                            count--;

                            this.Add(entry.Item);
                        }
                    }
                }

                return;
            }

            foreach (var item in other)
            {
                this.Add(item);
            }
        }

        public void ExceptWith(IEnumerable<TItem> other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (this.count == 0)
            {
                return;
            }

            if (other == this)
            {
                this.Clear();

                return;
            }

            if (other is PooledSet<TItem> source)
            {
                var count = source.count;

                for (var i = 0; count > 0; i++)
                {
                    ref var entry = ref source.entries[i];

                    if (entry.Next >= -1)
                    {
                        count--;

                        this.Remove(entry.Item);
                    }
                }

                return;
            }

            foreach (var item in other)
            {
                this.Remove(item);
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public void Dispose()
        {
            this.Clear();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Add(TItem item, int bucketIndex)
        {
            var entries = this.entries;
            int entryIndex;

            if (this.freeList != -1)
            {
                entryIndex = this.freeList;
                this.freeList = -3 - entries[this.freeList].Next;
            }
            else
            {
                if (this.count == this.size || this.size == 1)
                {
                    entries = this.Resize();
                    bucketIndex = item.GetHashCode() & (this.size - 1);
                }

                entryIndex = this.count;
            }

            entries[entryIndex].Item = item;
            entries[entryIndex].Next = this.buckets[bucketIndex] - 1;

            this.buckets[bucketIndex] = entryIndex + 1;
            this.count++;
        }

        private int FindItemIndex(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var entries = this.entries;
            var collisionCount = 0;

            for (var i = this.buckets[item.GetHashCode() & (this.size - 1)] - 1; (uint)i < (uint)this.size; i = entries[i].Next)
            {
                if (item.Equals(entries[i].Item))
                {
                    return i;
                }

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            return -1;
        }

        private int FindLastIndex()
        {
            var lastIndex = -1;
            var count = this.count;

            for (var i = 0; count > 0; i++)
            {
                ref var entry = ref this.entries[i];

                if (entry.Next >= -1)
                {
                    count--;

                    lastIndex = i;
                }
            }

            lastIndex++;

            return lastIndex;
        }

        private Entry[] Resize()
        {
            Debug.Assert(this.size == this.count || this.size == 1);

            var count = this.count;
            var newSize = this.size * 2;

            if (newSize < MinimumCapacity)
            {
                newSize = MinimumCapacity;
            }

            if ((uint)newSize > (uint)int.MaxValue)
            {
                throw new InvalidOperationException("TODO Arg_HTCapacityOverflow");
            }

            var newBuckets = BucketsPool.Rent(newSize);
            var newEntries = EntriesPool.Rent(newSize);

            Array.Clear(newBuckets, 0, newSize);
            Array.Clear(newEntries, count, newSize - count);
            Array.Copy(this.entries, 0, newEntries, 0, count);

            while (count-- > 0)
            {
                var bucketIndex = newEntries[count].Item.GetHashCode() & (newSize - 1);

                newEntries[count].Next = newBuckets[bucketIndex] - 1;
                newBuckets[bucketIndex] = count + 1;
            }

            if (this.size > 1)
            {
                BucketsPool.Return(this.buckets, clearArray: false);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                EntriesPool.Return(this.entries, clearArray: RuntimeHelpers.IsReferenceOrContainsReferences<Entry>());
#else
                EntriesPool.Return(this.entries, clearArray: true);
#endif
            }

            this.size = newSize;
            this.buckets = newBuckets;
            this.entries = newEntries;

            return newEntries;
        }

        public struct Enumerator : IEnumerator<TItem>
        {
            private readonly PooledSet<TItem> source;
            private int index;
            private int count;
            private TItem current;

            internal Enumerator(PooledSet<TItem> source)
            {
                this.source = source;
                this.index = 0;
                this.count = this.source.count;
                this.current = default!;
            }

            public bool MoveNext()
            {
                if (this.count == 0)
                {
                    this.current = default!;
                    return false;
                }

                this.count--;

                while (this.source.entries[this.index].Next < -1)
                {
                    this.index++;
                }

                ref var entry = ref this.source.entries[this.index];

                this.index++;
                this.current = entry.Item;

                return true;
            }

            public TItem Current
            {
                get
                {
                    return this.current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
            }

            void IEnumerator.Reset()
            {
                this.index = 0;
                this.count = this.source.count;
                this.current = default!;
            }

            public void Dispose()
            {
            }
        }
    }

    internal sealed class PooledSetDebugView<TItem> where TItem : IEquatable<TItem>
    {
        private readonly PooledSet<TItem> source;

        public PooledSetDebugView(PooledSet<TItem> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.source = source;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public TItem[] Items
        {
            get
            {
                return this.source.ToArray();
            }
        }
    }
}