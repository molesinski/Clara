using System;

namespace Clara.Analysis
{
    public class NonDigitsStemmerDecorator : IStemmer
    {
        private readonly IStemmer stemmer;

        public NonDigitsStemmerDecorator(IStemmer stemmer)
        {
            if (stemmer is null)
            {
                throw new ArgumentNullException(nameof(stemmer));
            }

            this.stemmer = stemmer;
        }

        public Token Stem(Token token)
        {
            var length = token.Length;

            for (var i = 0; i < length; i++)
            {
                if (char.IsDigit(token[i]))
                {
                    return token;
                }
            }

            return this.stemmer.Stem(token);
        }
    }
}
