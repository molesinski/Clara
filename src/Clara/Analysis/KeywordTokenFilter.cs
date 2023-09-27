﻿namespace Clara.Analysis
{
    public class KeywordTokenFilter : ITokenFilter
    {
        private readonly HashSet<Token> keywords = new();

        public KeywordTokenFilter(IEnumerable<string> keywords)
        {
            if (keywords is null)
            {
                throw new ArgumentNullException(nameof(keywords));
            }

            foreach (var keyword in keywords)
            {
                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    this.keywords.Add(new Token(keyword));
                }
            }
        }

        public Token Process(Token token, TokenFilterDelegate next)
        {
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (this.keywords.Contains(token))
            {
                return token;
            }

            return next(token);
        }
    }
}
