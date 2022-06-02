using System;

namespace Clara.Analysis.Stemming
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
            var span = token.ValueSpan;
            var length = span.Length;

            for (var i = 0; i < length; i++)
            {
                if (char.IsDigit(span[i]))
                {
                    return default;
                }
            }

            return this.stemmer.Stem(token);
        }
    }
}
