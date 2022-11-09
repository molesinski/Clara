namespace Clara.Analysis
{
    public class RequireLengthTokenFilter : ITokenFilter
    {
        private readonly int minimumLength;
        private readonly int maximumLength;

        public RequireLengthTokenFilter(int minimumLength = 1, int maximumLength = Token.MaximumLength)
        {
            if (minimumLength < 0 || minimumLength > Token.MaximumLength)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            }

            if (maximumLength < 0 || maximumLength > Token.MaximumLength)
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

            var length = token.Length;

            if (length < this.minimumLength || length > this.maximumLength)
            {
                return token;
            }

            return next(token);
        }
    }
}
