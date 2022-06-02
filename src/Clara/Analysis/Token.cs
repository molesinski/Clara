using System;

namespace Clara.Analysis
{
    public readonly struct Token : IEquatable<Token>
    {
        private readonly string? value;
        private readonly char[]? chars;
        private readonly int index;
        private readonly int length;

        public Token(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.value = value;
            this.chars = null;
            this.index = 0; 
            this.length = 0;
        }

        public Token(char[] chars, int index, int length)
        {
            if (chars is null)
            {
                throw new ArgumentNullException(nameof(chars));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index + length > chars.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            this.value = null;
            this.chars = chars;
            this.index = index;
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

                return this.chars.AsSpan(this.index, this.length);
            }
        }

        public char[] Chars
        {
            get
            {
                if (this.chars is null)
                {
                    throw new InvalidOperationException("Read only tokens cannot be modified.");
                }

                return this.chars;
            }
        }

        public int Index
        {
            get
            {
                if (this.chars is null)
                {
                    throw new InvalidOperationException("Read only tokens cannot be modified.");
                }

                return this.index;
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
        }

        public static bool operator ==(Token left, Token right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Token left, Token right)
        {
            return !left.Equals(right);
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
                    return new string(this.chars.AsSpan(this.index, this.length));
#else
                    return new string(this.chars, this.index, this.length);
#endif
                }
            }

            return string.Empty;
        }
    }
}
