using System;
using System.Buffers;
using System.Text;

namespace Clara.Analysis
{
    public class DefaultStringFactory : IStringFactory
    {
        private static readonly ArrayPool<char> BufferPool = ArrayPool<char>.Shared;

        private DefaultStringFactory()
        {
        }

        public static IStringFactory Instance { get; } = new DefaultStringFactory();

        public string Create(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return value;
        }

        public string Create(StringBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder.ToString();
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

            return new string(chars, index, count);
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

            var value = new string(chars, 0, charCount);

            BufferPool.Return(chars);

            return value;
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        public string Create(ReadOnlySpan<char> chars)
        {
            if (chars.Length == 0)
            {
                return string.Empty;
            }

            return new string(chars);
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
            encoding.GetChars(bytes, chars);

            return this.Create(chars.Slice(0, charCount));
        }
#endif
    }
}
