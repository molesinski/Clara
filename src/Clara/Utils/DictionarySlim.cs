using System.Buffers;
using System.Collections;
using System.Runtime.CompilerServices;

namespace Clara.Utils
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "By design")]
    public sealed class DictionarySlim<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IResettable
        where TKey : notnull
    {
        private const int MinimumSize = 4;

        private static readonly EqualityComparer<TKey> Comparer = EqualityComparer<TKey>.Default;
        private static readonly Entry[] InitialEntries = new Entry[1];

        private readonly KeysCollection keys;

        private int size;
        private int count;
        private int lastIndex;
        private int freeList;
        private int[] buckets;
        private Entry[] entries;

        public DictionarySlim()
        {
            this.keys = new KeysCollection(this);
            this.size = 1;
            this.count = 0;
            this.lastIndex = 0;
            this.freeList = -1;
            this.buckets = HashHelper.InitialBuckets;
            this.entries = InitialEntries;
        }

        public DictionarySlim(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            this.keys = new KeysCollection(this);
            this.size = HashHelper.PowerOf2(Math.Max(capacity, MinimumSize));
            this.count = 0;
            this.lastIndex = 0;
            this.freeList = -1;
            this.buckets = new int[this.size];
            this.entries = new Entry[this.size];
        }

        public DictionarySlim(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is DictionarySlim<TKey, TValue> other && other.count > 0)
            {
                this.keys = new KeysCollection(this);
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

            if (source is IReadOnlyCollection<KeyValuePair<TKey, TValue>> collection && collection.Count > 0)
            {
                this.keys = new KeysCollection(this);
                this.size = HashHelper.PowerOf2(Math.Max(collection.Count, MinimumSize));
                this.count = 0;
                this.lastIndex = 0;
                this.freeList = -1;
                this.buckets = new int[this.size];
                this.entries = new Entry[this.size];
            }
            else
            {
                this.keys = new KeysCollection(this);
                this.size = 1;
                this.count = 0;
                this.lastIndex = 0;
                this.freeList = -1;
                this.buckets = HashHelper.InitialBuckets;
                this.entries = InitialEntries;
            }

            foreach (var pair in source)
            {
                ref var value = ref this.GetValueRefOrAddDefault(pair.Key, out var exists);

                if (exists)
                {
                    throw new ArgumentException("Collection contains one or more duplicated keys.", nameof(source));
                }

                value = pair.Value;
            }
        }

        public int Count
        {
            get
            {
                return this.count;
            }
        }

        public KeysCollection Keys
        {
            get
            {
                return this.keys;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                var index = this.FindEntry(key);

                if (index >= 0)
                {
                    return this.entries[index].Value;
                }

                throw new KeyNotFoundException($"The given key '{key}' was not present in the dictionary.");
            }

            set
            {
                ref var valueRef = ref this.GetValueRefOrAddDefault(key, out _);

                valueRef = value;
            }
        }

        public void Clear()
        {
            if (this.count > 0)
            {
                Array.Clear(this.buckets, 0, this.size);

#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
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

        public bool TryGetValue(TKey key, out TValue value)
        {
            var entries = this.entries;
            var i = this.buckets[Comparer.GetHashCode(key) & this.size - 1] - 1;
            var collisionCount = 0;

            while (i >= 0)
            {
                ref var entry = ref entries[i];

                if (Comparer.Equals(key, entry.Key))
                {
                    value = entry.Value;

                    return true;
                }

                i = entry.Next;

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            value = default!;

            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return this.FindEntry(key) >= 0;
        }

        public bool Remove(TKey key)
        {
            var entries = this.entries;
            var bucketIndex = Comparer.GetHashCode(key) & this.size - 1;
            var i = this.buckets[bucketIndex] - 1;
            var last = -1;
            var collisionCount = 0;

            while (i != -1)
            {
                ref var entry = ref entries[i];

                if (Comparer.Equals(entry.Key, key))
                {
                    if (last != -1)
                    {
                        entries[last].Next = entry.Next;
                    }
                    else
                    {
                        this.buckets[bucketIndex] = entry.Next + 1;
                    }

                    entry.Key = default!;
                    entry.Value = default!;
                    entry.Next = -3 - this.freeList;
                    this.freeList = i;
                    this.count--;

                    if (this.count == 0)
                    {
                        this.lastIndex = 0;
                        this.freeList = -1;
                    }

                    return true;
                }

                last = i;
                i = entry.Next;

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            return false;
        }

        public ref TValue GetValueRefOrAddDefault(TKey key, out bool exists)
        {
            var entries = this.entries;
            var bucketIndex = Comparer.GetHashCode(key) & this.size - 1;
            var i = this.buckets[bucketIndex] - 1;
            var collisionCount = 0;

            while (i >= 0)
            {
                ref var entry = ref entries[i];

                if (Comparer.Equals(key, entry.Key))
                {
                    exists = true;

                    return ref entry.Value;
                }

                i = entry.Next;

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            exists = false;

            return ref this.AddKey(key, bucketIndex);
        }

        public void IntersectWith(IEnumerable<KeyValuePair<TKey, TValue>> enumerable, Func<TValue, TValue, TValue, TValue> valueCombiner, TValue parameter)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (valueCombiner is null)
            {
                throw new ArgumentNullException(nameof(valueCombiner));
            }

            if (this.Count == 0 || enumerable == this)
            {
                return;
            }

            if (enumerable is IReadOnlyCollection<KeyValuePair<TKey, TValue>> collection && collection.Count == 0)
            {
                this.Clear();

                return;
            }

            if (enumerable is DictionarySlim<TKey, TValue> other)
            {
                var count = this.count;

                for (var i = 0; count > 0; i++)
                {
                    ref var entry = ref this.entries[i];

                    if (entry.Next >= -1)
                    {
                        count--;

                        var key = entry.Key;

                        if (other.TryGetValue(key, out var value))
                        {
                            entry.Value = valueCombiner(entry.Value, value, parameter);
                        }
                        else
                        {
                            this.Remove(key);
                        }
                    }
                }
            }
            else
            {
                var lastIndex = this.lastIndex;
                var intArrayLength = BitHelper.ToIntArrayLength(lastIndex);

                if (intArrayLength <= BitHelper.StackAllocThreshold)
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

            void IntersectWith(IEnumerable<KeyValuePair<TKey, TValue>> enumerable, ref BitHelper bitHelper, int lastIndex)
            {
                foreach (var item in enumerable)
                {
                    var index = this.FindEntry(item.Key);

                    if (index >= 0)
                    {
                        bitHelper.MarkBit(index);

                        ref var entry = ref this.entries[index];

                        entry.Value = valueCombiner(entry.Value, item.Value, parameter);
                    }
                }

                for (var i = 0; i < lastIndex; i++)
                {
                    ref var entry = ref this.entries[i];

                    if (entry.Next >= -1)
                    {
                        if (!bitHelper.IsMarked(i))
                        {
                            this.Remove(entry.Key);
                        }
                    }
                }
            }
        }

        public void UnionWith(IEnumerable<KeyValuePair<TKey, TValue>> enumerable, Func<TValue, TValue, TValue, TValue> valueCombiner, TValue parameter)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (valueCombiner is null)
            {
                throw new ArgumentNullException(nameof(valueCombiner));
            }

            if (enumerable is DictionarySlim<TKey, TValue> other)
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

                    var count = this.count;

                    for (var i = 0; count > 0; i++)
                    {
                        ref var entry = ref this.entries[i];

                        if (entry.Next >= -1)
                        {
                            count--;

                            entry.Value = valueCombiner(default!, entry.Value, parameter);
                        }
                    }

                    return;
                }
                else
                {
                    this.EnsureCapacity(other.count);

                    var count = other.count;

                    for (var i = 0; count > 0; i++)
                    {
                        ref var entry = ref other.entries[i];

                        if (entry.Next >= -1)
                        {
                            count--;

                            ref var value = ref this.GetValueRefOrAddDefault(entry.Key, out _);

                            value = valueCombiner(value, entry.Value, parameter);
                        }
                    }

                    return;
                }
            }

            if (enumerable is IReadOnlyCollection<KeyValuePair<TKey, TValue>> collection)
            {
                if (collection.Count == 0)
                {
                    return;
                }

                this.EnsureCapacity(collection.Count);
            }

            foreach (var item in enumerable)
            {
                ref var value = ref this.GetValueRefOrAddDefault(item.Key, out _);

                value = valueCombiner(value, item.Value, parameter);
            }
        }

        public void ExceptWith(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
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

            if (enumerable is DictionarySlim<TKey, TValue> other)
            {
                var count = other.count;

                for (var i = 0; count > 0; i++)
                {
                    ref var entry = ref other.entries[i];

                    if (entry.Next >= -1)
                    {
                        count--;

                        this.Remove(entry.Key);
                    }
                }

                return;
            }

            foreach (var item in enumerable)
            {
                this.Remove(item.Key);
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        void IResettable.Reset()
        {
            this.Clear();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private ref TValue AddKey(TKey key, int bucketIndex)
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
                    bucketIndex = Comparer.GetHashCode(key) & this.size - 1;
                }

                entryIndex = this.count;
                this.lastIndex++;
            }

            ref var entry = ref entries[entryIndex];

            entry.Key = key;
            entry.Value = default!;
            entry.Next = this.buckets[bucketIndex] - 1;

            this.buckets[bucketIndex] = entryIndex + 1;
            this.count++;

            return ref entries[entryIndex].Value;
        }

        private int FindEntry(TKey key)
        {
            var entries = this.entries;
            var i = this.buckets[Comparer.GetHashCode(key) & this.size - 1] - 1;
            var collisionCount = 0;

            while (i >= 0)
            {
                ref var entry = ref entries[i];

                if (Comparer.Equals(key, entry.Key))
                {
                    return i;
                }

                i = entry.Next;

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
                    var bucketIndex = Comparer.GetHashCode(entry.Key) & newSize - 1;

                    entry.Next = newBuckets[bucketIndex] - 1;
                    newBuckets[bucketIndex] = lastIndex + 1;
                }
            }

            if (this.size > 1)
            {
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
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

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly DictionarySlim<TKey, TValue> source;
            private int index;
            private int count;
            private KeyValuePair<TKey, TValue> current;

            internal Enumerator(DictionarySlim<TKey, TValue> source)
            {
                this.source = source;
                this.index = 0;
                this.count = this.source.count;
                this.current = default;
            }

            public readonly KeyValuePair<TKey, TValue> Current
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
                    this.current = default;
                    return false;
                }

                this.count--;

                while (this.source.entries[this.index].Next < -1)
                {
                    this.index++;
                }

                ref var entry = ref this.source.entries[this.index];

                this.index++;
                this.current = new KeyValuePair<TKey, TValue>(entry.Key, entry.Value);

                return true;
            }

            void IEnumerator.Reset()
            {
                this.index = 0;
                this.count = this.source.count;
                this.current = default;
            }

            public readonly void Dispose()
            {
            }
        }

        private struct Entry
        {
            public TKey Key;
            public TValue Value;
            public int Next;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "By design")]
        public sealed class KeysCollection : IReadOnlyCollection<TKey>, IReadOnlyHashCollection<TKey>
        {
            private readonly DictionarySlim<TKey, TValue> source;

            internal KeysCollection(DictionarySlim<TKey, TValue> source)
            {
                this.source = source;
            }

            public int Count
            {
                get
                {
                    return this.source.Count;
                }
            }

            public bool Contains(TKey item)
            {
                return this.source.ContainsKey(item);
            }

            public Enumerator GetEnumerator()
            {
                return new Enumerator(this.source);
            }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            public struct Enumerator : IEnumerator<TKey>
            {
                private readonly DictionarySlim<TKey, TValue> source;
                private int index;
                private int count;
                private TKey current;

                internal Enumerator(DictionarySlim<TKey, TValue> source)
                {
                    this.source = source;
                    this.index = 0;
                    this.count = this.source.count;
                    this.current = default!;
                }

                public readonly TKey Current
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
                    this.current = entry.Key;

                    return true;
                }

                public void Reset()
                {
                    this.index = 0;
                    this.count = this.source.count;
                    this.current = default!;
                }

                public void Dispose()
                {
                    this.Reset();
                }
            }
        }
    }
}
