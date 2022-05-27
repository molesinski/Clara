using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Clara.Utils
{
    [DebuggerTypeProxy(typeof(PooledListDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class PooledList<TItem> : IReadOnlyList<TItem>, IDisposable
        where TItem : notnull
    {
        private const int MinimumCapacity = 16;

        private static readonly ArrayPool<TItem> EntryPool = ArrayPool<TItem>.Shared;
        private static readonly TItem[] InitialEntries = new TItem[1];

        private int count;
        private TItem[] entries;

        public PooledList()
        {
            this.count = 0;
            this.entries = InitialEntries;
        }

        public PooledList(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            if (capacity < MinimumCapacity)
            {
                capacity = MinimumCapacity;
            }

            capacity = HashHelper.PowerOf2(capacity);

            this.count = 0;
            this.entries = EntryPool.Rent(capacity);
        }

        public PooledList(IEnumerable<TItem> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is PooledList<TItem> source)
            {
                if (source.entries.Length == 1)
                {
                    this.count = 0;
                    this.entries = InitialEntries;
                }
                else
                {
                    var capacity = source.count;

                    if (capacity < MinimumCapacity)
                    {
                        capacity = MinimumCapacity;
                    }

                    capacity = HashHelper.PowerOf2(capacity);

                    this.count = source.count;
                    this.entries = EntryPool.Rent(capacity);

                    Array.Copy(source.entries, 0, this.entries, 0, source.count);
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

                capacity = HashHelper.PowerOf2(capacity);

                this.count = 0;
                this.entries = EntryPool.Rent(capacity);
            }
            else
            {
                this.count = 0;
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

        public TItem this[int index]
        {
            get
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return this.entries[index];
            }

            set
            {
                if (index < 0 || index >= this.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                this.entries[index] = value;
            }
        }

        public void Clear()
        {
            if (this.count > 0)
            {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                if (RuntimeHelpers.IsReferenceOrContainsReferences<TItem>())
                {
                    Array.Clear(this.entries, 0, this.count);
                }
#else
                Array.Clear(this.entries, 0, this.count);
#endif

                this.count = 0;
            }
        }

        public void Add(TItem item)
        {
            var entries = this.entries;

            if (this.entries.Length == this.count || this.entries.Length == 1)
            {
                entries = this.Resize();
            }

            entries[this.count] = item;

            this.count++;
        }

        public void Sort(IComparer<TItem> comparer)
        {
            this.Sort(0, this.count, comparer);
        }

        public void Sort(int offset, int count, IComparer<TItem> comparer)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (offset + count > this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            Array.Sort(this.entries, offset, count, comparer);
        }

        public IEnumerable<TItem> Range(int offset, int count)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (offset + count > this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return new RangeEnumerable(this, offset, count);
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
            if (this.entries.Length > 1)
            {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                EntryPool.Return(this.entries, clearArray: RuntimeHelpers.IsReferenceOrContainsReferences<TItem>());
#else
                EntryPool.Return(this.entries, clearArray: true);
#endif
            }

            this.count = 0;
            this.entries = InitialEntries;
        }

        private TItem[] Resize()
        {
            Debug.Assert(this.entries.Length == this.count || this.entries.Length == 1);

            var count = this.count;
            var newSize = this.entries.Length * 2;

            if (newSize < MinimumCapacity)
            {
                newSize = MinimumCapacity;
            }

            if ((uint)newSize > int.MaxValue)
            {
                throw new InvalidOperationException("Capacity overflowed and went negative. Check load factor, capacity and the current size of the table.");
            }

            var newEntries = EntryPool.Rent(newSize);

            Array.Copy(this.entries, 0, newEntries, 0, count);

            if (this.entries.Length > 1)
            {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                EntryPool.Return(this.entries, clearArray: RuntimeHelpers.IsReferenceOrContainsReferences<TItem>());
#else
                EntryPool.Return(this.entries, clearArray: true);
#endif
            }

            this.entries = newEntries;

            return newEntries;
        }

        public struct Enumerator : IEnumerator<TItem>
        {
            private readonly TItem[] entries;
            private readonly int offset;
            private readonly int count;
            private int index;
            private TItem current;

            public Enumerator(PooledList<TItem> source)
            {
                this.entries = source.entries;
                this.offset = 0;
                this.count = source.count;
                this.index = 0;
                this.current = default!;
            }

            public Enumerator(PooledList<TItem> source, int offset, int count)
            {
                this.entries = source.entries;
                this.offset = offset;
                this.count = count;
                this.index = 0;
                this.current = default!;
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

            public bool MoveNext()
            {
                if (this.index < this.count)
                {
                    this.current = this.entries[this.offset + this.index];
                    this.index++;

                    return true;
                }

                this.index = this.count + 1;
                this.current = default!;

                return false;
            }

            public void Reset()
            {
                this.index = 0;
                this.current = default!;
            }

            public void Dispose()
            {
            }
        }

        private class RangeEnumerable : IEnumerable<TItem>
        {
            private readonly PooledList<TItem> source;
            private readonly int offset;
            private readonly int count;

            public RangeEnumerable(PooledList<TItem> source, int offset, int count)
            {
                this.source = source;
                this.offset = offset;
                this.count = count;
            }

            public IEnumerator<TItem> GetEnumerator()
            {
                return new Enumerator(this.source, this.offset, this.count);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this.source, this.offset, this.count);
            }
        }
    }

    internal sealed class PooledListDebugView<TItem>
        where TItem : notnull
    {
        private readonly PooledList<TItem> source;

        public PooledListDebugView(PooledList<TItem> source)
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
