namespace Clara.Analysis
{
    public readonly struct TokenTerm : IEquatable<TokenTerm>
    {
        private readonly Token token;
        private readonly Position position;

        public TokenTerm(Token token, Position position)
        {
            this.token = token;
            this.position = position;
        }

        public Token Token
        {
            get
            {
                return this.token;
            }
        }

        public Position Position
        {
            get
            {
                return this.position;
            }
        }

        public static bool operator ==(TokenTerm left, TokenTerm right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TokenTerm left, TokenTerm right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is TokenTerm other && this == other;
        }

        public bool Equals(TokenTerm other)
        {
            return this.token == other.token
                && this.position == other.position;
        }

        public override int GetHashCode()
        {
            var hash = default(HashCode);

            hash.Add(this.token);
            hash.Add(this.position);

            return hash.ToHashCode();
        }
    }
}
