using System.Text;

namespace Clara.Analysis
{
    public struct Token : IEquatable<Token>
    {
        public const int MaximumLength = 255;

        private readonly string? value;
        private readonly char[]? chars;
        private int length;

        public Token(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!(value.Length <= MaximumLength))
            {
                throw new ArgumentException("Read only tokens must have length less than or equal to maximum token length.", nameof(value));
            }

            this.value = value;
            this.length = value.Length;
        }

        public Token(char[] chars, int count)
        {
            if (chars is null)
            {
                throw new ArgumentNullException(nameof(chars));
            }

            if (count < 0 || count > chars.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (!(chars.Length >= MaximumLength))
            {
                throw new ArgumentException("Writeable tokens must have character buffer length greater than or equal to maximum token length.", nameof(chars));
            }

            this.chars = chars;
            this.length = count;
        }

        public readonly bool IsEmpty
        {
            get
            {
                return this.length == 0;
            }
        }

        public readonly int Length
        {
            get
            {
                return this.length;
            }
        }

        public readonly char this[int index]
        {
            get
            {
                if (index < 0 || index >= this.length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                if (this.value is not null)
                {
                    return this.value[index];
                }

                if (this.chars is not null)
                {
                    return this.chars[index];
                }

                throw new ArgumentOutOfRangeException(nameof(index));
            }

            set
            {
                if (this.chars is null)
                {
                    throw new InvalidOperationException("Read only tokens cannot be modified.");
                }

                if (index < 0 || index >= this.length)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                this.chars[index] = value;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "AsReadOnlySpan is defined")]
        public static implicit operator ReadOnlySpan<char>(Token token)
        {
            return token.AsReadOnlySpan();
        }

        public static bool operator ==(Token left, Token right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Token left, Token right)
        {
            return !left.Equals(right);
        }

        public readonly Span<char> AsSpan()
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            return this.chars.AsSpan(0, this.length);
        }

        public readonly ReadOnlySpan<char> AsReadOnlySpan()
        {
            if (this.value is not null)
            {
                return this.value.AsSpan();
            }

            if (this.chars is not null)
            {
                return this.chars.AsSpan(0, this.length);
            }

            return ReadOnlySpan<char>.Empty;
        }

        public readonly Token ToReadOnly()
        {
            if (this.chars is not null)
            {
                return new Token(this.ToString());
            }

            return this;
        }

        public readonly void CopyTo(StringBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (this.value is not null)
            {
                builder.Append(this.value);
            }
            else if (this.chars is not null)
            {
                builder.Append(this.chars, 0, this.length);
            }
        }

        public void Set(string chars)
        {
            this.Set(chars.AsSpan());
        }

        public void Set(ReadOnlySpan<char> chars)
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            if (chars.Length > MaximumLength)
            {
                throw new ArgumentException("Written length would exceed token maximum length.", nameof(chars));
            }

            chars.CopyTo(this.chars);

            this.length = chars.Length;
        }

        public void Set(StringBuilder builder)
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder.Length > MaximumLength)
            {
                throw new ArgumentException("Written length would exceed token maximum length.", nameof(builder));
            }

            builder.CopyTo(0, this.chars, 0, builder.Length);

            this.length = builder.Length;
        }

        public void Append(string chars)
        {
            this.Append(chars.AsSpan());
        }

        public void Append(ReadOnlySpan<char> chars)
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            var startIndex = this.length;
            var writtenLength = startIndex + chars.Length;

            if (writtenLength > MaximumLength)
            {
                throw new ArgumentException("Written length would exceed token maximum length.", nameof(chars));
            }

            chars.CopyTo(this.chars.AsSpan(startIndex));

            this.length = writtenLength;
        }

        public void Write(int startIndex, string chars)
        {
            this.Write(startIndex, chars.AsSpan());
        }

        public void Write(int startIndex, ReadOnlySpan<char> chars)
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            if (startIndex < 0 || startIndex > this.length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            var count = chars.Length;
            var writtenLength = startIndex + count;

            if (writtenLength > MaximumLength)
            {
                throw new ArgumentException("Written length would exceed token maximum length.", nameof(chars));
            }

            chars.CopyTo(this.chars.AsSpan(startIndex));

            this.length = writtenLength;
        }

        public void Insert(int startIndex, string chars)
        {
            this.Insert(startIndex, chars.AsSpan());
        }

        public void Insert(int startIndex, ReadOnlySpan<char> chars)
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            if (startIndex < 0 || startIndex > this.length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            var count = chars.Length;
            var insertedLength = this.length + count;

            if (insertedLength > MaximumLength)
            {
                throw new ArgumentException("Inserted length would exceed token maximum length.", nameof(chars));
            }

            Array.Copy(this.chars, startIndex, this.chars, startIndex + count, this.length - startIndex);

            chars.CopyTo(this.chars.AsSpan(startIndex));

            this.length = insertedLength;
        }

        public void Delete(int startIndex, int count)
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            if (startIndex < 0 || startIndex > this.length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            if (startIndex + count > this.length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            Array.Copy(this.chars, startIndex + count, this.chars, startIndex, this.length - (startIndex + count));

            this.length -= count;
        }

        public void Remove(int startIndex)
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            if (startIndex < 0 || startIndex > this.length)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }

            this.length = startIndex;
        }

        public void Clear()
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            this.length = 0;
        }

        public override readonly bool Equals(object? obj)
        {
            if (obj is not Token other)
            {
                return false;
            }

            return this.Equals(other);
        }

        public readonly bool Equals(Token other)
        {
            if (this.length != other.length)
            {
                return false;
            }

            if (this.length == 0)
            {
                return true;
            }

            return this.AsReadOnlySpan().SequenceEqual(other.AsReadOnlySpan());
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1307:Specify StringComparison for clarity", Justification = "Intended comparison mode StringComparison.Ordinal ends up in the same call")]
        public override readonly int GetHashCode()
        {
#if NET6_0_OR_GREATER
            return string.GetHashCode(this.AsReadOnlySpan());
#else
            var span = this.AsReadOnlySpan();

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

        public override readonly string ToString()
        {
            if (this.value is not null)
            {
                return this.value;
            }

            if (this.chars is not null)
            {
                if (this.length > 0)
                {
                    return new string(this.chars, 0, this.length);
                }
            }

            return string.Empty;
        }
    }
}
