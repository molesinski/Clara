using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Clara.Utils
{
    [DebuggerTypeProxy(typeof(HashSetSlimDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public sealed class HashSetSlim<TItem> : IReadOnlyCollection<TItem>
#pragma warning restore CA1710 // Identifiers should have correct suffix
        where TItem : notnull, IEquatable<TItem>
    {
        private const int MinimumSize = 4;
        private const int StackAllocThreshold = 512;

        private static readonly Entry[] InitialEntries = new Entry[1];

        private int size;
        private int count;
        private int lastIndex;
        private int freeList;
        private int[] buckets;
        private Entry[] entries;

        public HashSetSlim()
        {
            this.size = 1;
            this.count = 0;
            this.lastIndex = 0;
            this.freeList = -1;
            this.buckets = HashHelper.InitialBuckets;
            this.entries = InitialEntries;
        }

        public HashSetSlim(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            this.size = HashHelper.PowerOf2(Math.Max(capacity, MinimumSize));
            this.count = 0;
            this.lastIndex = 0;
            this.freeList = -1;
            this.buckets = new int[this.size];
            this.entries = new Entry[this.size];
        }

        public HashSetSlim(IEnumerable<TItem> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is HashSetSlim<TItem> other && other.size > 1)
            {
                this.size = other.size;
                this.count = other.count;
                this.lastIndex = other.lastIndex;
                this.freeList = other.freeList;
                this.buckets = new int[this.size];
                this.entries = new Entry[this.size];

                Array.Copy(other.buckets, 0, this.buckets, 0, this.size);
                Array.Copy(other.entries, 0, this.entries, 0, this.lastIndex);

                return;
            }

            if (source is IReadOnlyCollection<TItem> collection && collection.Count > 0)
            {
                this.size = HashHelper.PowerOf2(Math.Max(collection.Count, MinimumSize));
                this.count = 0;
                this.lastIndex = 0;
                this.freeList = -1;
                this.buckets = new int[this.size];
                this.entries = new Entry[this.size];
            }
            else
            {
                this.size = 1;
                this.count = 0;
                this.lastIndex = 0;
                this.freeList = -1;
                this.buckets = HashHelper.InitialBuckets;
                this.entries = InitialEntries;
            }

            foreach (var item in source)
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
            if (this.count > 0)
            {
                Array.Clear(this.buckets, 0, this.size);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                if (RuntimeHelpers.IsReferenceOrContainsReferences<Entry>())
                {
                    Array.Clear(this.entries, 0, this.lastIndex);
                }
#else
                Array.Clear(this.entries, 0, this.lastIndex);
#endif

                this.count = 0;
                this.lastIndex = 0;
                this.freeList = -1;
            }
        }

        public bool Contains(TItem item)
        {
            return this.FindItem(item) >= 0;
        }

        public bool Add(TItem item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var entries = this.entries;
            var collisionCount = 0;
            var bucketIndex = item.GetHashCode() & this.size - 1;

            for (var i = this.buckets[bucketIndex] - 1; i >= 0; i = entries[i].Next)
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
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var entries = this.entries;
            var bucketIndex = item.GetHashCode() & this.size - 1;
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

                    if (this.count == 0)
                    {
                        this.lastIndex = 0;
                        this.freeList = -1;
                    }

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

        public void IntersectWith(IEnumerable<TItem> enumerable)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (this.Count == 0 || enumerable == this)
            {
                return;
            }

            if (enumerable is IReadOnlyCollection<TItem> collection && collection.Count == 0)
            {
                this.Clear();

                return;
            }

            if (enumerable is HashSetSlim<TItem> other)
            {
                var count = this.count;

                for (var i = 0; count > 0; i++)
                {
                    ref var entry = ref this.entries[i];

                    if (entry.Next >= -1)
                    {
                        count--;

                        var item = entry.Item;

                        if (!other.Contains(item))
                        {
                            this.Remove(item);
                        }
                    }
                }
            }
            else
            {
                var lastIndex = this.lastIndex;
                var intArrayLength = BitHelper.ToIntArrayLength(lastIndex);

                if (intArrayLength <= StackAllocThreshold)
                {
                    Span<int> span = stackalloc int[intArrayLength];
                    var bitHelper = new BitHelper(span.Slice(0, intArrayLength), clear: true);

                    IntersectWith(enumerable, ref bitHelper, lastIndex);
                }
                else
                {
                    var array = ArrayPool<int>.Shared.Rent(intArrayLength);
                    var bitHelper = new BitHelper(array.AsSpan(0, intArrayLength), clear: true);

                    IntersectWith(enumerable, ref bitHelper, lastIndex);

                    ArrayPool<int>.Shared.Return(array);
                }
            }

            void IntersectWith(IEnumerable<TItem> enumerable, ref BitHelper bitHelper, int lastIndex)
            {
                foreach (var item in enumerable)
                {
                    var index = this.FindItem(item);

                    if (index >= 0)
                    {
                        bitHelper.MarkBit(index);
                    }
                }

                for (var i = 0; i < lastIndex; i++)
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

        public void UnionWith(IEnumerable<TItem> enumerable)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (enumerable is HashSetSlim<TItem> other)
            {
                if (other.count == 0)
                {
                    return;
                }

                if (this.size > 1)
                {
                    if (this.count == 0)
                    {
                        if (this.size < other.count)
                        {
                            this.size = 1;
                            this.count = 0;
                            this.lastIndex = 0;
                            this.freeList = -1;
                            this.buckets = HashHelper.InitialBuckets;
                            this.entries = InitialEntries;
                        }
                    }
                }

                if (this.size == 1)
                {
                    this.size = other.size;
                    this.count = other.count;
                    this.lastIndex = other.lastIndex;
                    this.freeList = other.freeList;
                    this.buckets = new int[this.size];
                    this.entries = new Entry[this.size];

                    Array.Copy(other.buckets, 0, this.buckets, 0, this.size);
                    Array.Copy(other.entries, 0, this.entries, 0, this.lastIndex);

                    return;
                }

                this.EnsureCapacity(other.count);

                var count = other.count;

                for (var i = 0; count > 0; i++)
                {
                    ref var entry = ref other.entries[i];

                    if (entry.Next >= -1)
                    {
                        count--;

                        this.Add(entry.Item);
                    }
                }

                return;
            }

            if (enumerable is IReadOnlyCollection<TItem> collection)
            {
                if (collection.Count == 0)
                {
                    return;
                }

                this.EnsureCapacity(collection.Count);
            }

            foreach (var item in enumerable)
            {
                this.Add(item);
            }
        }

        public void ExceptWith(IEnumerable<TItem> enumerable)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (this.count == 0)
            {
                return;
            }

            if (enumerable == this)
            {
                this.Clear();

                return;
            }

            if (enumerable is HashSetSlim<TItem> other)
            {
                var count = other.count;

                for (var i = 0; count > 0; i++)
                {
                    ref var entry = ref other.entries[i];

                    if (entry.Next >= -1)
                    {
                        count--;

                        this.Remove(entry.Item);
                    }
                }

                return;
            }

            foreach (var item in enumerable)
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
                    this.EnsureCapacity(this.count + 1);

                    entries = this.entries;
                    bucketIndex = item.GetHashCode() & this.size - 1;
                }

                entryIndex = this.count;
                this.lastIndex++;
            }

            entries[entryIndex].Item = item;
            entries[entryIndex].Next = this.buckets[bucketIndex] - 1;

            this.buckets[bucketIndex] = entryIndex + 1;
            this.count++;
        }

        private int FindItem(TItem item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var entries = this.entries;
            var collisionCount = 0;

            for (var i = this.buckets[item.GetHashCode() & this.size - 1] - 1; i >= 0; i = entries[i].Next)
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

        private void EnsureCapacity(int capacity)
        {
            var newSize = HashHelper.PowerOf2(Math.Max(capacity, MinimumSize));

            if (newSize <= this.size)
            {
                return;
            }

            var lastIndex = this.lastIndex;
            var newBuckets = new int[newSize];
            var newEntries = new Entry[newSize];

            Array.Copy(this.entries, 0, newEntries, 0, lastIndex);

            while (lastIndex-- > 0)
            {
                ref var entry = ref newEntries[lastIndex];

                if (entry.Next >= -1)
                {
                    var bucketIndex = entry.Item.GetHashCode() & newSize - 1;

                    entry.Next = newBuckets[bucketIndex] - 1;
                    newBuckets[bucketIndex] = lastIndex + 1;
                }
            }

            if (this.size > 1)
            {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                if (RuntimeHelpers.IsReferenceOrContainsReferences<Entry>())
                {
                    Array.Clear(this.entries, 0, this.lastIndex);
                }
#else
                Array.Clear(this.entries, 0, this.lastIndex);
#endif
            }

            this.size = newSize;
            this.buckets = newBuckets;
            this.entries = newEntries;
        }

        public struct Enumerator : IEnumerator<TItem>
        {
            private readonly HashSetSlim<TItem> source;
            private int index;
            private int count;
            private TItem current;

            internal Enumerator(HashSetSlim<TItem> source)
            {
                this.source = source;
                this.index = 0;
                this.count = this.source.count;
                this.current = default!;
            }

            public readonly TItem Current
            {
                get
                {
                    return this.current;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
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

            void IEnumerator.Reset()
            {
                this.index = 0;
                this.count = this.source.count;
                this.current = default!;
            }

            public readonly void Dispose()
            {
            }
        }

        [DebuggerDisplay("({Item})->{Next}")]
        private struct Entry
        {
            public TItem Item;
            public int Next;
        }
    }

    internal sealed class HashSetSlimDebugView<TItem>
        where TItem : IEquatable<TItem>
    {
        private readonly HashSetSlim<TItem> source;

        public HashSetSlimDebugView(HashSetSlim<TItem> source)
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
