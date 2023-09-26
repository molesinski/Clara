using Clara.Utils;

namespace Clara.Querying
{
    public abstract class TokensMatchExpression : MatchExpression
    {
        private readonly ListSlim<string> tokens;

        internal TokensMatchExpression(ListSlim<string> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            this.tokens = tokens;
        }

        public IReadOnlyCollection<string> Tokens
        {
            get
            {
                return this.tokens;
            }
        }
    }
}
