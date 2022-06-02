using System;
using System.Collections.Generic;

namespace Clara.Analysis
{
    public class DelegateTokenizer : ITokenizer
    {
        private readonly Func<string, IEnumerable<Token>> tokenizer;

        public DelegateTokenizer(Func<string, IEnumerable<Token>> tokenizer)
        {
            if (tokenizer is null)
            {
                throw new ArgumentNullException(nameof(tokenizer));
            }

            this.tokenizer = tokenizer;
        }

        public IEnumerable<Token> GetTokens(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<Token>();
            }

            return this.tokenizer(text);
        }
    }
}
