using System.Buffers;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Clara.Utils
{
    [DebuggerTypeProxy(typeof(DictionarySlimDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    public sealed class PooledDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IDisposable
        where TKey : notnull, IEquatable<TKey>
    {
        private static readonly Entry[] InitialEntries = new Entry[1];

        private readonly Allocator allocator;
        private int size;
        private int count;
        private int lastIndex;
        private int freeList;
        private int[] buckets;
        private Entry[] entries;

        public PooledDictionary(Allocator allocator)
        {
            if (allocator is null)
            {
                throw new ArgumentNullException(nameof(allocator));
            }

            this.allocator = allocator;
            this.size = 1;
            this.count = 0;
            this.lastIndex = 0;
            this.freeList = -1;
            this.buckets = HashHelper.InitialBuckets;
            this.entries = InitialEntries;
        }

        public PooledDictionary(Allocator allocator, int capacity)
        {
            if (allocator is null)
            {
                throw new ArgumentNullException(nameof(allocator));
            }

            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity));
            }

            this.allocator = allocator;
            this.size = HashHelper.PowerOf2(Math.Max(capacity, this.allocator.MinimumSize));
            this.count = 0;
            this.lastIndex = 0;
            this.freeList = -1;
            this.buckets = this.allocator.Allocate<int>(this.size, clearArray: true);
            this.entries = this.allocator.Allocate<Entry>(this.size);
        }

        public PooledDictionary(Allocator allocator, IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (allocator is null)
            {
                throw new ArgumentNullException(nameof(allocator));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source is PooledDictionary<TKey, TValue> other && other.count > 0)
            {
                this.allocator = allocator;
                this.size = other.size;
                this.count = other.count;
                this.lastIndex = other.lastIndex;
                this.freeList = other.freeList;
                this.buckets = this.allocator.Allocate<int>(this.size);
                this.entries = this.allocator.Allocate<Entry>(this.size);

                Array.Copy(other.buckets, 0, this.buckets, 0, this.size);
                Array.Copy(other.entries, 0, this.entries, 0, this.lastIndex);

                return;
            }

            if (source is IReadOnlyCollection<KeyValuePair<TKey, TValue>> collection && collection.Count > 0)
            {
                this.allocator = allocator;
                this.size = HashHelper.PowerOf2(Math.Max(collection.Count, this.allocator.MinimumSize));
                this.count = 0;
                this.lastIndex = 0;
                this.freeList = -1;
                this.buckets = this.allocator.Allocate<int>(this.size, clearArray: true);
                this.entries = this.allocator.Allocate<Entry>(this.size);
            }
            else
            {
                this.allocator = allocator;
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
                    return true;
                }

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            return false;
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

        public void Dispose()
        {
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

                this.allocator.Release(this.buckets);
                this.allocator.Release(this.entries);
            }

            this.size = 1;
            this.count = 0;
            this.lastIndex = 0;
            this.freeList = -1;
            this.buckets = HashHelper.InitialBuckets;
            this.entries = InitialEntries;
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

        private void EnsureCapacity(int capacity)
        {
            var newSize = HashHelper.PowerOf2(Math.Max(capacity, this.allocator.MinimumSize));

            if (newSize <= this.size)
            {
                return;
            }

            var lastIndex = this.lastIndex;
            var newBuckets = this.allocator.Allocate<int>(newSize, clearArray: true);
            var newEntries = this.allocator.Allocate<Entry>(newSize);

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
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                if (RuntimeHelpers.IsReferenceOrContainsReferences<Entry>())
                {
                    Array.Clear(this.entries, 0, this.lastIndex);
                }
#else
                Array.Clear(this.entries, 0, this.lastIndex);
#endif

                this.allocator.Release(this.buckets);
                this.allocator.Release(this.entries);
            }

            this.size = newSize;
            this.buckets = newBuckets;
            this.entries = newEntries;
        }

        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            private readonly PooledDictionary<TKey, TValue> source;
            private int index;
            private int count;
            private KeyValuePair<TKey, TValue> current;

            internal Enumerator(PooledDictionary<TKey, TValue> source)
            {
                this.source = source;
                this.index = 0;
                this.count = this.source.count;
                this.current = default;
            }

            public KeyValuePair<TKey, TValue> Current
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

            public void Dispose()
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
    }

    internal sealed class DictionarySlimDebugView<TKey, TValue>
        where TKey : IEquatable<TKey>
    {
        private readonly PooledDictionary<TKey, TValue> source;

        public DictionarySlimDebugView(PooledDictionary<TKey, TValue> source)
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
