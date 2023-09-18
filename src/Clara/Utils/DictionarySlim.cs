using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Clara.Utils
{
    [DebuggerTypeProxy(typeof(DictionarySlimDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
#pragma warning disable CA1710 // Identifiers should have correct suffix
    public sealed class DictionarySlim<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>
#pragma warning restore CA1710 // Identifiers should have correct suffix
        where TKey : notnull, IEquatable<TKey>
    {
        private const int MinimumSize = 4;
        private const int StackAllocThreshold = 512;
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

#pragma warning disable CA1000 // Do not declare static members on generic types
        public static ObjectPool<DictionarySlim<TKey, TValue>> ObjectPool { get; } = new(() => new(), value => value.Clear());
#pragma warning restore CA1000 // Do not declare static members on generic types

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
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var entries = this.entries;
            var collisionCount = 0;

            for (var i = this.buckets[key.GetHashCode() & this.size - 1] - 1; i >= 0; i = entries[i].Next)
            {
                if (key.Equals(entries[i].Key))
                {
                    value = entries[i].Value;

                    return true;
                }

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
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var entries = this.entries;
            var bucketIndex = key.GetHashCode() & this.size - 1;
            var entryIndex = this.buckets[bucketIndex] - 1;

            var lastIndex = -1;
            var collisionCount = 0;

            while (entryIndex != -1)
            {
                var candidate = entries[entryIndex];

                if (candidate.Key.Equals(key))
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

        public ref TValue GetValueRefOrAddDefault(TKey key, out bool exists)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var entries = this.entries;
            var collisionCount = 0;
            var bucketIndex = key.GetHashCode() & this.size - 1;

            for (var i = this.buckets[bucketIndex] - 1; i >= 0; i = entries[i].Next)
            {
                if (key.Equals(entries[i].Key))
                {
                    exists = true;

                    return ref entries[i].Value;
                }

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            exists = false;

            return ref this.AddKey(key, bucketIndex);
        }

        public void IntersectWith(IEnumerable<KeyValuePair<TKey, TValue>> enumerable, Func<TValue, TValue, TValue> valueCombiner)
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
                            entry.Value = valueCombiner(entry.Value, value);
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

            void IntersectWith(IEnumerable<KeyValuePair<TKey, TValue>> enumerable, ref BitHelper bitHelper, int lastIndex)
            {
                foreach (var item in enumerable)
                {
                    var index = this.FindEntry(item.Key);

                    if (index >= 0)
                    {
                        bitHelper.MarkBit(index);

                        ref var entry = ref this.entries[index];

                        entry.Value = valueCombiner(entry.Value, item.Value);
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

        public void UnionWith(IEnumerable<KeyValuePair<TKey, TValue>> enumerable, Func<TValue, TValue, TValue> valueCombiner)
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

                        ref var value = ref this.GetValueRefOrAddDefault(entry.Key, out _);

                        value = valueCombiner(value, entry.Value);
                    }
                }

                return;
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

                value = valueCombiner(value, item.Value);
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
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
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
                    bucketIndex = key.GetHashCode() & this.size - 1;
                }

                entryIndex = this.count;
                this.lastIndex++;
            }

            entries[entryIndex].Key = key;
            entries[entryIndex].Value = default!;
            entries[entryIndex].Next = this.buckets[bucketIndex] - 1;

            this.buckets[bucketIndex] = entryIndex + 1;
            this.count++;

            return ref entries[entryIndex].Value;
        }

        private int FindEntry(TKey key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var entries = this.entries;
            var collisionCount = 0;

            for (var i = this.buckets[key.GetHashCode() & this.size - 1] - 1; i >= 0; i = entries[i].Next)
            {
                if (key.Equals(entries[i].Key))
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
                    var bucketIndex = entry.Key.GetHashCode() & newSize - 1;

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

        public struct KeysEnumerator : IEnumerator<TKey>
        {
            private readonly DictionarySlim<TKey, TValue> source;
            private int index;
            private int count;
            private TKey current;

            internal KeysEnumerator(DictionarySlim<TKey, TValue> source)
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

        [DebuggerDisplay("({Key}, {Value})->{Next}")]
        private struct Entry
        {
            public TKey Key;
            public TValue Value;
            public int Next;
        }

#pragma warning disable CA1034 // Nested types should not be visible
        public class KeysCollection : IReadOnlyCollection<TKey>, IReadOnlyHashCollection<TKey>
#pragma warning restore CA1034 // Nested types should not be visible
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

            public KeysEnumerator GetEnumerator()
            {
                return new KeysEnumerator(this.source);
            }

            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
            {
                return new KeysEnumerator(this.source);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new KeysEnumerator(this.source);
            }
        }
    }

    internal sealed class DictionarySlimDebugView<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        private readonly DictionarySlim<TKey, TValue> source;

        public DictionarySlimDebugView(DictionarySlim<TKey, TValue> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            this.source = source;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public KeyValuePair<TKey, TValue>[] Items
        {
            get
            {
                return this.source.ToArray();
            }
        }
    }
}
