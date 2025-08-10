using System.Collections;
using System.Runtime.CompilerServices;

namespace Clara.Utils
{
    public sealed class ListSlim<TItem> : IList<TItem>, IReadOnlyList<TItem>, IResettable
        where TItem : notnull
    {
        private const int MinimumSize = 4;
        private static readonly TItem[] InitialEntries = new TItem[1];

        private int count;
        private TItem[] entries;

        public ListSlim()
        {
            this.count = 0;
            this.entries = InitialEntries;
        }

        public ListSlim(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            var size = HashHelper.PowerOf2(Math.Max(capacity, MinimumSize));

            this.count = 0;
            this.entries = new TItem[size];
        }

        public ListSlim(IEnumerable<TItem> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is ListSlim<TItem> other && other.count > 0)
            {
                var size = HashHelper.PowerOf2(Math.Max(other.count, MinimumSize));

                this.count = other.count;
                this.entries = new TItem[size];

                Array.Copy(other.entries, 0, this.entries, 0, other.count);

                return;
            }

            if (source is IReadOnlyCollection<TItem> collection && collection.Count > 0)
            {
                var size = HashHelper.PowerOf2(Math.Max(collection.Count, MinimumSize));

                this.count = 0;
                this.entries = new TItem[size];
            }
            else
            {
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

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public TItem this[int index]
        {
            get
            {
                if (index < 0 || index >= this.count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                return this.entries[index];
            }

            set
            {
                if (index < 0 || index >= this.count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                this.entries[index] = value;
            }
        }

        public Span<TItem> AsSpan()
        {
            return this.entries.AsSpan(0, this.count);
        }

        public bool Contains(TItem item)
        {
            return this.IndexOf(item) != -1;
        }

        public int IndexOf(TItem item)
        {
            return Array.IndexOf(this.entries, item, 0, this.count);
        }

        public void CopyTo(TItem[] array, int arrayIndex)
        {
            Array.Copy(this.entries, 0, array, arrayIndex, this.count);
        }

        public void Add(TItem item)
        {
            if (this.entries.Length == this.count || this.entries.Length == 1)
            {
                this.EnsureCapacity(this.count + 1);
            }

            this.entries[this.count] = item;
            this.count++;
        }

        public void AddRange(IEnumerable<TItem> items)
        {
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            if (items is ListSlim<TItem> other)
            {
                var count = other.count;

                if (count > 0)
                {
                    if (this.entries.Length - this.count < count || this.entries.Length == 1)
                    {
                        this.EnsureCapacity(this.count + count);
                    }

                    Array.Copy(other.entries, 0, this.entries, this.count, count);
                    this.count += count;
                }
            }
            else if (items is ICollection<TItem> collection)
            {
                var count = collection.Count;

                if (count > 0)
                {
                    if (this.entries.Length - this.count < count || this.entries.Length == 1)
                    {
                        this.EnsureCapacity(this.count + count);
                    }

                    collection.CopyTo(this.entries, this.count);
                    this.count += count;
                }
            }
            else
            {
                foreach (var item in items)
                {
                    this.Add(item);
                }
            }
        }

        public void Insert(int index, TItem item)
        {
            if (index < 0 || index > this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (this.entries.Length == this.count || this.entries.Length == 1)
            {
                this.EnsureCapacity(this.count + 1);
            }

            if (index < this.count)
            {
                Array.Copy(this.entries, index, this.entries, index + 1, this.count - index);
            }

            this.entries[index] = item;
            this.count++;
        }

        public bool Remove(TItem item)
        {
            var index = this.IndexOf(item);

            if (index >= 0)
            {
                this.RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            this.count--;

            if (index < this.count)
            {
                Array.Copy(this.entries, index + 1, this.entries, index, this.count - index);
            }

#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            if (RuntimeHelpers.IsReferenceOrContainsReferences<TItem>())
            {
                this.entries[this.count] = default!;
            }
#else
            this.entries[this.count] = default!;
#endif
        }

        public void Clear()
        {
            if (this.count > 0)
            {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
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

            ArraySortHelper<TItem>.Sort(this.entries, offset, count, comparer);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<TItem> IEnumerable<TItem>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Enumerator GetRangeEnumerator(int offset, int count)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (offset + count > this.count)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return new Enumerator(this, offset, count);
        }

        void IResettable.Reset()
        {
            this.Clear();
        }

        private void EnsureCapacity(int capacity)
        {
            var newSize = HashHelper.PowerOf2(Math.Max(capacity, MinimumSize));

            if (newSize <= this.entries.Length)
            {
                return;
            }

            var newEntries = new TItem[newSize];

            Array.Copy(this.entries, 0, newEntries, 0, this.count);

            if (this.entries.Length > 1)
            {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                if (RuntimeHelpers.IsReferenceOrContainsReferences<TItem>())
                {
                    Array.Clear(this.entries, 0, this.count);
                }
#else
                Array.Clear(this.entries, 0, this.count);
#endif
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

            internal Enumerator(ListSlim<TItem> source)
            {
                this.entries = source.entries;
                this.offset = 0;
                this.count = source.count;
                this.index = 0;
                this.current = default!;
            }

            internal Enumerator(ListSlim<TItem> source, int offset, int count)
            {
                this.entries = source.entries;
                this.offset = offset;
                this.count = count;
                this.index = 0;
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
                if (this.index < this.count)
                {
                    this.current = this.entries[this.offset + this.index];
                    this.index++;

                    return true;
                }

                this.index = this.count;
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
                this.Reset();
            }
        }
    }
}
