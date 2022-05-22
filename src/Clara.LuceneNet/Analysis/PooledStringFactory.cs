using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Clara.Analysis
{
    public class PooledStringFactory : IStringFactory
    {
        private static readonly ArrayPool<char> BufferPool = ArrayPool<char>.Shared;

        private readonly ConcurrentDictionary<ReadOnlyMemory<char>, string> lookup = new(new ReadOnlyMemoryEquaityComparer());
        private readonly int maximumLength;
        private readonly int maximumCapacity;
        private int count;

        public PooledStringFactory(int maximumLength = 256, int maximumCapacity = 65536)
        {
            this.maximumLength = maximumLength;
            this.maximumCapacity = maximumCapacity;
        }

        public string Create(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length > this.maximumLength)
            {
                return value;
            }

            if (!this.lookup.TryGetValue(value.AsMemory(), out var replacedValue))
            {
                replacedValue = value;

                if (this.count < this.maximumCapacity)
                {
                    if (this.lookup.TryAdd(value.AsMemory(), value))
                    {
                        Interlocked.Increment(ref this.count);
                    }
                }
            }

            return replacedValue;
        }

        public string Create(StringBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder.Length > this.maximumLength)
            {
                return builder.ToString();
            }

            var buffer = BufferPool.Rent(builder.Length);

            builder.CopyTo(0, buffer, 0, builder.Length);

            if (!this.lookup.TryGetValue(buffer.AsMemory(), out var value))
            {
                value = builder.ToString();

                if (this.count < this.maximumCapacity)
                {
                    if (this.lookup.TryAdd(value.AsMemory(), value))
                    {
                        Interlocked.Increment(ref this.count);
                    }
                }
            }

            BufferPool.Return(buffer);

            return value;
        }

        public string Create(char[] chars, int index, int count)
        {
            if (chars is null)
            {
                throw new ArgumentNullException(nameof(chars));
            }

            if (index < 0 || index >= chars.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index + count > chars.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (!this.lookup.TryGetValue(chars.AsMemory(index, count), out var value))
            {
                value = new string(chars, index, count);

                if (this.count < this.maximumCapacity)
                {
                    if (this.lookup.TryAdd(value.AsMemory(), value))
                    {
                        Interlocked.Increment(ref this.count);
                    }
                }
            }

            return value;
        }

        public string Create(byte[] bytes, int index, int count, Encoding encoding)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (index < 0 || index >= bytes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index + count > bytes.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var charCount = encoding.GetCharCount(bytes, index, count);

            if (charCount == 0)
            {
                return string.Empty;
            }

            var chars = BufferPool.Rent(charCount);
            charCount = encoding.GetChars(bytes, index, count, chars, 0);

            if (!this.lookup.TryGetValue(chars.AsMemory(0, charCount), out var value))
            {
                value = new string(chars, 0, charCount);

                if (this.count < this.maximumCapacity)
                {
                    if (this.lookup.TryAdd(value.AsMemory(), value))
                    {
                        Interlocked.Increment(ref this.count);
                    }
                }
            }

            BufferPool.Return(chars);

            return value;
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        public string Create(ReadOnlySpan<char> chars)
        {
            if (chars.Length > this.maximumLength)
            {
                return new string(chars);
            }

            var buffer = BufferPool.Rent(chars.Length);
            var memory = buffer.AsMemory(0, chars.Length);

            chars.CopyTo(memory.Span);

            if (!this.lookup.TryGetValue(memory, out var value))
            {
                value = new string(chars);

                if (this.count < this.maximumCapacity)
                {
                    if (this.lookup.TryAdd(value.AsMemory(), value))
                    {
                        Interlocked.Increment(ref this.count);
                    }
                }
            }

            BufferPool.Return(buffer);

            return value;
        }

        public string Create(ReadOnlySpan<byte> bytes, Encoding encoding)
        {
            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            var charCount = encoding.GetCharCount(bytes);

            if (charCount == 0)
            {
                return string.Empty;
            }

            Span<char> chars = stackalloc char[charCount];
            charCount = encoding.GetChars(bytes, chars);

            return this.Create(chars.Slice(0, charCount));
        }
#endif

        private class ReadOnlyMemoryEquaityComparer : IEqualityComparer<ReadOnlyMemory<char>>
        {
            public bool Equals(ReadOnlyMemory<char> x, ReadOnlyMemory<char> y)
            {
                var a = x.Span;
                var b = y.Span;

                if (a.Length != b.Length)
                {
                    return false;
                }

                return a.SequenceEqual(b);
            }

            public int GetHashCode(ReadOnlyMemory<char> obj)
            {
                var span = obj.Span;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
                var hashCode = default(HashCode);

                for (var i = 0; i < span.Length; i++)
                {
                    hashCode.Add(span[i]);
                }

                return hashCode.ToHashCode();
#else
                unchecked
                {
                    var hash1 = 5381;
                    var hash2 = hash1;

                    for (var i = 0; i < span.Length && span[i] != '\0'; i += 2)
                    {
                        hash1 = ((hash1 << 5) + hash1) ^ span[i];

                        if (i == span.Length - 1 || span[i + 1] == '\0')
                        {
                            break;
                        }

                        hash2 = ((hash2 << 5) + hash2) ^ span[i + 1];
                    }

                    return hash1 + (hash2 * 1566083941);
                }
#endif
            }
        }
    }
}
