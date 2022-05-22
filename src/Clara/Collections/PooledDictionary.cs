﻿using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Clara.Collections
{
    [DebuggerTypeProxy(typeof(PooledDictionaryDebugView<,>))]
    [DebuggerDisplay("Count = {Count}")]
    internal sealed class PooledDictionary<TKey, TValue> : IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IDisposable
        where TKey : notnull, IEquatable<TKey>
    {
        private const int MinimumCapacity = 4;

        private static readonly ArrayPool<int> BucketsPool = ArrayPool<int>.Shared;
        private static readonly ArrayPool<Entry> EntriesPool = ArrayPool<Entry>.Shared;
        private static readonly Entry[] InitialEntries = new Entry[1];

        private int size;
        private int count;
        private int freeList;
        private int[] buckets;
        private Entry[] entries;

        [DebuggerDisplay("({Key}, {Value})->{Next}")]
        private struct Entry
        {
            public TKey Key;
            public TValue Value;
            public int Next;
        }

        public PooledDictionary()
        {
            this.size = 1;
            this.count = 0;
            this.freeList = -1;
            this.buckets = HashHelpers.SizeOneIntArray;
            this.entries = InitialEntries;
        }

        public PooledDictionary(int capacity)
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

        public PooledDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (collection is PooledDictionary<TKey, TValue> source)
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

            if (collection is IReadOnlyCollection<KeyValuePair<TKey, TValue>> readOnlyCollection)
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

            foreach (var pair in collection)
            {
                ref var value = ref this.GetValueRefOrAddDefault(pair.Key, out var exists);

                if (exists)
                {
                    throw new ArgumentException("Collection contains one or more duplicated keys.", nameof(collection));
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

        public TValue GetValueOrDefault(TKey key)
        {
            if (this.TryGetValue(key, out var value))
            {
                return value;
            }

            return default!;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var entries = this.entries;
            var collisionCount = 0;

            for (var i = this.buckets[key.GetHashCode() & (this.size - 1)] - 1; (uint)i < (uint)this.size; i = entries[i].Next)
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
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var entries = this.entries;
            var collisionCount = 0;

            for (var i = this.buckets[key.GetHashCode() & (this.size - 1)] - 1; (uint)i < (uint)this.size; i = entries[i].Next)
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

        public void Add(TKey key, TValue value)
        {
            ref var valueRef = ref this.GetValueRefOrAddDefault(key, out var exists);

            if (exists)
            {
                throw new ArgumentException("An element with the same key already exists", nameof(key));
            }

            valueRef = value;
        }

        public bool Remove(TKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var entries = this.entries;
            var bucketIndex = key.GetHashCode() & (this.size - 1);
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
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var entries = this.entries;
            var collisionCount = 0;
            var bucketIndex = key.GetHashCode() & (this.size - 1);

            for (var i = this.buckets[bucketIndex] - 1; (uint)i < (uint)this.size; i = entries[i].Next)
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
                    entries = this.Resize();
                    bucketIndex = key.GetHashCode() & (this.size - 1);
                }

                entryIndex = this.count;
            }

            entries[entryIndex].Key = key;
            entries[entryIndex].Next = this.buckets[bucketIndex] - 1;

            this.buckets[bucketIndex] = entryIndex + 1;
            this.count++;

            return ref entries[entryIndex].Value;
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
                var bucketIndex = newEntries[count].Key.GetHashCode() & (newSize - 1);

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
    }

    internal sealed class PooledDictionaryDebugView<TKey, TValue> where TKey : IEquatable<TKey>
    {
        private readonly PooledDictionary<TKey, TValue> source;

        public PooledDictionaryDebugView(PooledDictionary<TKey, TValue> source)
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