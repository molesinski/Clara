using System.Runtime.CompilerServices;

namespace Clara.Utils
{
    public sealed class StringPoolSlim
    {
        private const int MinimumSize = 1024;

        private static readonly Entry[] InitialEntries = new Entry[1];

        private int size;
        private int count;
        private int lastIndex;
        private int freeList;
        private int[] buckets;
        private Entry[] entries;

        public StringPoolSlim()
        {
            this.size = 1;
            this.count = 0;
            this.lastIndex = 0;
            this.freeList = -1;
            this.buckets = HashHelper.InitialBuckets;
            this.entries = InitialEntries;
        }

        public StringPoolSlim(StringPoolSlim other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (other.size > 1)
            {
                this.size = other.size;
                this.count = other.count;
                this.lastIndex = other.lastIndex;
                this.freeList = other.freeList;
                this.buckets = new int[this.size];
                this.entries = new Entry[this.size];

                Array.Copy(other.buckets, 0, this.buckets, 0, this.size);
                Array.Copy(other.entries, 0, this.entries, 0, this.lastIndex);
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
        }

        public bool TryGet(ReadOnlySpan<char> span, out string value)
        {
            var entries = this.entries;
            var hashCode = SpanHelper.GetHashCode(span);
            var i = this.buckets[hashCode & this.size - 1] - 1;
            var collisionCount = 0;

            while (i >= 0)
            {
                ref var entry = ref entries[i];

                if (hashCode == entry.HashCode && span.SequenceEqual(entry.Value.AsSpan()))
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

        public string GetOrAdd(ReadOnlySpan<char> span)
        {
            var entries = this.entries;
            var hashCode = SpanHelper.GetHashCode(span);
            var bucketIndex = hashCode & this.size - 1;
            var i = this.buckets[bucketIndex] - 1;
            var collisionCount = 0;

            while (i >= 0)
            {
                ref var entry = ref entries[i];

                if (hashCode == entry.HashCode && span.SequenceEqual(entry.Value.AsSpan()))
                {
                    return entry.Value;
                }

                i = entry.Next;

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            var value = span.ToString();

            this.Add(value, hashCode, bucketIndex);

            return value;
        }

        public string GetOrAdd(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var entries = this.entries;
            var hashCode = SpanHelper.GetHashCode(value.AsSpan());
            var bucketIndex = hashCode & this.size - 1;
            var i = this.buckets[bucketIndex] - 1;
            var collisionCount = 0;

            while (i >= 0)
            {
                ref var entry = ref entries[i];

                if (hashCode == entry.HashCode && value.AsSpan().SequenceEqual(entry.Value.AsSpan()))
                {
                    return entry.Value;
                }

                i = entry.Next;

                if (collisionCount == this.size)
                {
                    throw new InvalidOperationException("Concurrent operations are not supported.");
                }

                collisionCount++;
            }

            this.Add(value, hashCode, bucketIndex);

            return value;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Add(string value, int hashCode, int bucketIndex)
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
                    bucketIndex = hashCode & this.size - 1;
                }

                entryIndex = this.count;
                this.lastIndex++;
            }

            ref var entry = ref entries[entryIndex];

            entry.HashCode = hashCode;
            entry.Value = value;
            entry.Next = this.buckets[bucketIndex] - 1;

            this.buckets[bucketIndex] = entryIndex + 1;
            this.count++;
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
                    var bucketIndex = entry.HashCode & newSize - 1;

                    entry.Next = newBuckets[bucketIndex] - 1;
                    newBuckets[bucketIndex] = lastIndex + 1;
                }
            }

            if (this.size > 1)
            {
                Array.Clear(this.entries, 0, this.lastIndex);
            }

            this.size = newSize;
            this.buckets = newBuckets;
            this.entries = newEntries;
        }

        private struct Entry
        {
            public int HashCode;
            public string Value;
            public int Next;
        }
    }
}
