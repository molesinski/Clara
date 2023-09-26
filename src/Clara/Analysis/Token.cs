using System.Text;

namespace Clara.Analysis
{
    public struct Token : IEquatable<Token>
    {
        public const int MaximumLength = 100;

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
            this.chars = null;
            this.length = 0;
        }

        public Token(char[] chars, int length)
        {
            if (chars is null)
            {
                throw new ArgumentNullException(nameof(chars));
            }

            if (length < 0 || length > chars.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (!(chars.Length >= MaximumLength))
            {
                throw new ArgumentException("Writeable tokens must have character buffer length greater than or equal to maximum token length.", nameof(chars));
            }

            this.value = null;
            this.chars = chars;
            this.length = length;
        }

        public readonly int Length
        {
            get
            {
                if (this.chars is not null)
                {
                    return this.length;
                }
                else if (this.value is not null)
                {
                    return this.value.Length;
                }
                else
                {
                    return 0;
                }
            }
        }

        public readonly bool IsReadOnly
        {
            get
            {
                return this.chars is null;
            }
        }

        public readonly ReadOnlySpan<char> Span
        {
            get
            {
                if (this.chars is not null)
                {
                    return this.chars.AsSpan(0, this.length);
                }
                else if (this.value is not null)
                {
                    return this.value.AsSpan();
                }
                else
                {
                    return ReadOnlySpan<char>.Empty;
                }
            }
        }

        public readonly char this[int index]
        {
            get
            {
                if (this.chars is not null)
                {
                    if (index < 0 || index >= this.length)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }

                    return this.chars[index];
                }
                else if (this.value is not null)
                {
                    if (index < 0 || index >= this.value.Length)
                    {
                        throw new ArgumentOutOfRangeException(nameof(index));
                    }

                    return this.value[index];
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }
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

        public static bool operator ==(Token left, Token right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Token left, Token right)
        {
            return !left.Equals(right);
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

            for (var i = this.length - 1; i >= startIndex; i--)
            {
                this.chars[i + count] = this.chars[i];
            }

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

            for (var i = startIndex; i < this.length - count; i++)
            {
                this.chars[i] = this.chars[i + count];
            }

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

        public readonly Token AsReadOnly()
        {
            if (!this.IsReadOnly)
            {
                return new Token(this.ToString());
            }

            return this;
        }

        public readonly bool Equals(Token other)
        {
            var a = this.Span;
            var b = other.Span;

            if (a.Length != b.Length)
            {
                return false;
            }

            return a.SequenceEqual(b);
        }

        public override readonly bool Equals(object? obj)
        {
            if (obj is not Token other)
            {
                return false;
            }

            return this.Equals(other);
        }

        public override readonly int GetHashCode()
        {
            var span = this.Span;

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
