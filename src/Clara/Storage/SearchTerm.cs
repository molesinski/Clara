using Clara.Analysis;

namespace Clara.Storage
{
    public readonly struct SearchTerm : IEquatable<SearchTerm>
    {
        private readonly string? token;
        private readonly PhraseGroup? phrases;
        private readonly Position position;

        public SearchTerm(string token, Position position)
        {
            if (token is null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            this.token = token;
            this.position = position;
        }

        public SearchTerm(PhraseGroup phrases, Position position)
        {
            this.phrases = phrases;
            this.position = position;
        }

        public string? Token
        {
            get
            {
                return this.token;
            }
        }

        public PhraseGroup? Phrases
        {
            get
            {
                return this.phrases;
            }
        }

        public Position Position
        {
            get
            {
                return this.position;
            }
        }

        public static bool operator ==(SearchTerm left, SearchTerm right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SearchTerm left, SearchTerm right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is SearchTerm other && this == other;
        }

        public bool Equals(SearchTerm other)
        {
            return StringComparer.Ordinal.Equals(this.token, other.token)
                && this.phrases == other.phrases
                && this.position == other.position;
        }

        public override int GetHashCode()
        {
            var hash = default(HashCode);

            hash.Add(this.token, StringComparer.Ordinal);
            hash.Add(this.phrases);
            hash.Add(this.position);

            return hash.ToHashCode();
        }
    }
}
