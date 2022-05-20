using System;
using System.Collections.Generic;

namespace Clara.Analysis
{
    public sealed class Tokenizer : ITokenizer
    {
        private readonly ITokenizer tokenizer;
        private readonly List<ITokenFilter> tokenFilters;

        public Tokenizer(ITokenizer tokenizer, params ITokenFilter[] tokenFilters)
            : this(tokenizer, (IEnumerable<ITokenFilter>)tokenFilters)
        {
        }

        public Tokenizer(ITokenizer tokenizer, IEnumerable<ITokenFilter> tokenFilters)
        {
            if (tokenizer is null)
            {
                throw new ArgumentNullException(nameof(tokenizer));
            }

            if (tokenFilters is null)
            {
                throw new ArgumentNullException(nameof(tokenFilters));
            }

            this.tokenizer = tokenizer;
            this.tokenFilters = new List<ITokenFilter>(tokenFilters);
        }

        public IEnumerable<string> GetTokens(string text)
        {
            var tokens = this.tokenizer.GetTokens(text);

            foreach (var tokenFilter in this.tokenFilters)
            {
                tokens = tokenFilter.Filter(tokens);
            }

            return tokens;
        }
    }
}
