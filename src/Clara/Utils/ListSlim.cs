using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Clara.Utils
{
    [DebuggerTypeProxy(typeof(ListDebugView<>))]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class ListSlim<TItem> : IReadOnlyList<TItem>, IDisposable
        where TItem : notnull
    {
        private static readonly TItem[] InitialEntries = new TItem[1];

        private readonly Allocator allocator;
        private int count;
        private TItem[] entries;

        public ListSlim(Allocator allocator)
        {
            if (allocator is null)
            {
                throw new ArgumentNullException(nameof(allocator));
            }

            this.allocator = allocator;
            this.count = 0;
            this.entries = InitialEntries;
        }

        public ListSlim(Allocator allocator, int capacity)
        {
            if (allocator is null)
            {
                throw new ArgumentNullException(nameof(allocator));
            }

            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            var size = HashHelper.PowerOf2(Math.Max(capacity, allocator.MinimumSize));

            this.allocator = allocator;
            this.count = 0;
            this.entries = this.allocator.Allocate<TItem>(size);
        }

        public ListSlim(Allocator allocator, IEnumerable<TItem> source)
        {
            if (allocator is null)
            {
                throw new ArgumentNullException(nameof(allocator));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ListSlim<TItem> other && other.count > 0)
            {
                var size = HashHelper.PowerOf2(Math.Max(other.count, allocator.MinimumSize));

                this.allocator = allocator;
                this.count = other.count;
                this.entries = this.allocator.Allocate<TItem>(size);

                Array.Copy(other.entries, 0, this.entries, 0, other.count);

                return;
            }

            if (source is IReadOnlyCollection<TItem> collection && collection.Count > 0)
            {
                var size = HashHelper.PowerOf2(Math.Max(collection.Count, allocator.MinimumSize));

                this.allocator = allocator;
                this.count = 0;
                this.entries = this.allocator.Allocate<TItem>(size);
            }
            else
            {
                this.allocator = allocator;
                this.count = 0;
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
            if (this.count == this.entries.Length || this.entries.Length == 1)
            {
                this.EnsureCapacity(this.count + 1);
            }

            this.entries[this.count] = item;
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
                if (RuntimeHelpers.IsReferenceOrContainsReferences<TItem>())
                {
                    Array.Clear(this.entries, 0, this.count);
                }
#else
                Array.Clear(this.entries, 0, this.count);
#endif

                this.allocator.Release(this.entries);
            }

            this.count = 0;
            this.entries = InitialEntries;
        }

        private void EnsureCapacity(int capacity)
        {
            var newSize = HashHelper.PowerOf2(Math.Max(capacity, this.allocator.MinimumSize));

            if (newSize <= this.entries.Length)
            {
                return;
            }

            var newEntries = this.allocator.Allocate<TItem>(newSize);

            Array.Copy(this.entries, 0, newEntries, 0, this.count);

            if (this.entries.Length > 1)
            {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                if (RuntimeHelpers.IsReferenceOrContainsReferences<TItem>())
                {
                    Array.Clear(this.entries, 0, this.count);
                }
#else
                Array.Clear(this.entries, 0, this.count);
#endif

                this.allocator.Release(this.entries);
            }

            this.entries = newEntries;
        }

        public struct Enumerator : IEnumerator<TItem>
        {
            private readonly TItem[] entries;
            private readonly int offset;
            private readonly int count;
            private int index;
            private TItem current;

            public Enumerator(ListSlim<TItem> source)
            {
                this.entries = source.entries;
                this.offset = 0;
                this.count = source.count;
                this.index = 0;
                this.current = default!;
            }

            public Enumerator(ListSlim<TItem> source, int offset, int count)
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
            private readonly ListSlim<TItem> source;
            private readonly int offset;
            private readonly int count;

            public RangeEnumerable(ListSlim<TItem> source, int offset, int count)
            {
                this.source = source;
                this.offset = offset;
                this.count = count;
            }

            public Enumerator GetEnumerator()
            {
                return new Enumerator(this.source, this.offset, this.count);
            }

            IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
            {
                return new Enumerator(this.source, this.offset, this.count);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new Enumerator(this.source, this.offset, this.count);
            }
        }
    }

    internal sealed class ListDebugView<TItem>
        where TItem : notnull
    {
        private readonly ListSlim<TItem> source;

        public ListDebugView(ListSlim<TItem> source)
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
