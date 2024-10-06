namespace Clara.Analysis
{
    public readonly struct PhraseTerm : IEquatable<PhraseTerm>
    {
        private readonly Token? token;
        private readonly PhraseGroup? phrases;
        private readonly Position position;

        public PhraseTerm(Token token, Position position)
        {
            this.token = token;
            this.position = position;
        }

        public PhraseTerm(PhraseGroup phrases, Position position)
        {
            this.phrases = phrases;
            this.position = position;
        }

        public Token? Token
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

        public static bool operator ==(PhraseTerm left, PhraseTerm right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PhraseTerm left, PhraseTerm right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is PhraseTerm other && this == other;
        }

        public bool Equals(PhraseTerm other)
        {
            return this.token == other.token
                && this.phrases == other.phrases
                && this.position == other.position;
        }

        public override int GetHashCode()
        {
            var hash = default(HashCode);

            hash.Add(this.token);
            hash.Add(this.phrases);
            hash.Add(this.position);

            return hash.ToHashCode();
        }
    }
}
