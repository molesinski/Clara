﻿using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
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

        public IReadOnlyList<string> Tokens
        {
            get
            {
                return this.tokens;
            }
        }
    }
}
