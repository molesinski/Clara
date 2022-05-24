using System;
using System.Buffers;
using System.Text;

namespace Clara.Analysis
{
    internal static class StringHelper
    {
        private const int StackAllocThreshold = 256;

        private static readonly ArrayPool<char> CharPool = ArrayPool<char>.Shared;

        public static string Create(StringBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var count = builder.Length;

            if (count == 0)
            {
                return string.Empty;
            }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            if (count <= StackAllocThreshold)
            {
                Span<char> chars = stackalloc char[count];
                builder.CopyTo(0, chars, count);
                var span = chars;

                return new string(span);
            }
            else
#endif
            {
                var chars = CharPool.Rent(count);

                try
                {
                    builder.CopyTo(0, chars, 0, count);

                    return new string(chars, 0, count);
                }
                finally
                {
                    CharPool.Return(chars);
                }
            }
        }

        public static string Create(byte[] bytes, int index, int count, Encoding encoding)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (index < 0)
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

            if (count == 0)
            {
                return string.Empty;
            }

            var charCount = encoding.GetCharCount(bytes, index, count);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
            if (charCount <= StackAllocThreshold)
            {
                Span<char> chars = stackalloc char[charCount];
                charCount = encoding.GetChars(bytes.AsSpan(index, count), chars);
                var span = chars.Slice(0, charCount);

                return new string(span);
            }
            else
#endif
            {
                var chars = CharPool.Rent(charCount);

                try
                {
                    charCount = encoding.GetChars(bytes, index, count, chars, 0);

                    return new string(chars, 0, charCount);
                }
                finally
                {
                    CharPool.Return(chars);
                }
            }
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
        public static string Create(ReadOnlySpan<byte> bytes, Encoding encoding)
        {
            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            var charCount = encoding.GetCharCount(bytes);

            if (charCount <= StackAllocThreshold)
            {
                Span<char> chars = stackalloc char[charCount];
                charCount = encoding.GetChars(bytes, chars);
                var span = chars.Slice(0, charCount);

                return new string(span);
            }
            else
            {
                var chars = CharPool.Rent(charCount);

                try
                {
                    charCount = encoding.GetChars(bytes, chars);
                    var span = chars.AsSpan(0, charCount);

                    return new string(span);
                }
                finally
                {
                    CharPool.Return(chars);
                }
            }
        }
#endif
    }
}
