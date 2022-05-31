using System;
using System.Collections.Generic;

namespace Clara.Analysis
{
    public class DelegateTokenizer : ITokenizer
    {
        private readonly Func<string, IEnumerable<string>> tokenizer;

        public DelegateTokenizer(Func<string, IEnumerable<string>> tokenizer)
        {
            if (tokenizer is null)
            {
                throw new ArgumentNullException(nameof(tokenizer));
            }

            this.tokenizer = tokenizer;
        }

        public IEnumerable<string> GetTokens(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Array.Empty<string>();
            }

            return this.tokenizer(text);
        }
    }
}
