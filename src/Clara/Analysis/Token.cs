using System;

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

            this.value = null;
            this.chars = chars;
            this.length = length;
        }

        public bool IsEmpty
        {
            get
            {
                return this.Length == 0;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return this.chars is null;
            }
        }

        public ReadOnlySpan<char> ValueSpan
        {
            get
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
        }

        public char this[int index]
        {
            get
            {
                if (this.chars is null)
                {
                    throw new InvalidOperationException("Read only tokens cannot be modified.");
                }

                return this.chars[index];
            }

            set
            {
                if (this.chars is null)
                {
                    throw new InvalidOperationException("Read only tokens cannot be modified.");
                }

                this.chars[index] = value;
            }
        }

        public int Length
        {
            get
            {
                if (this.value is not null)
                {
                    return this.value.Length;
                }

                return this.length;
            }

            set
            {
                if (this.chars is null)
                {
                    throw new InvalidOperationException("Read only tokens cannot be modified.");
                }

                if (value < 0 || value > this.chars.Length || value > MaximumLength)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                this.length = value;
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

        public static implicit operator ReadOnlySpan<char>(Token value)
        {
            return value.ValueSpan;
        }

        public void GetChars(out char[] chars)
        {
            if (this.chars is null)
            {
                throw new InvalidOperationException("Read only tokens cannot be modified.");
            }

            chars = this.chars;
        }

        public Token ToReadOnly()
        {
            if (!this.IsReadOnly)
            {
                return new Token(this.ToString());
            }

            return this;
        }

        public bool Equals(Token other)
        {
            var a = this.ValueSpan;
            var b = other.ValueSpan;

            if (a.Length != b.Length)
            {
                return false;
            }

            return a.SequenceEqual(b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Token other)
            {
                return false;
            }

            return this.Equals(other);
        }

        public override int GetHashCode()
        {
            var span = this.ValueSpan;

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

        public override string ToString()
        {
            if (this.value is not null)
            {
                return this.value;
            }

            if (this.chars is not null)
            {
                if (this.length > 0)
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
                    return new string(this.chars.AsSpan(0, this.length));
#else
                    return new string(this.chars, 0, this.length);
#endif
                }
            }

            return string.Empty;
        }
    }
}
