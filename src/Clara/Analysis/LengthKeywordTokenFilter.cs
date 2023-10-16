namespace Clara.Analysis
{
    public sealed class LengthKeywordTokenFilter : ITokenFilter
    {
        private readonly int minimumLength;
        private readonly int maximumLength;

        public LengthKeywordTokenFilter(int minimumLength = 1, int maximumLength = Token.MaximumLength)
        {
            if (minimumLength < 1 || minimumLength > Token.MaximumLength)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            }

            if (maximumLength < 1 || maximumLength > Token.MaximumLength)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            }

            if (!(minimumLength <= maximumLength))
            {
                throw new ArgumentException("Minimum length must be less or equal to maximum length.", nameof(minimumLength));
            }

            this.minimumLength = minimumLength;
            this.maximumLength = maximumLength;
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (token.Length < this.minimumLength || token.Length > this.maximumLength)
            {
                return token;
            }

            return next(token);
        }
    }
}
