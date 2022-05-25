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

        public StemResult Stem(string token)
        {
            var length = token.Length;

            for (var i = 0; i < length; i++)
            {
                if (char.IsDigit(token[i]))
                {
                    return default;
                }
            }

            return this.stemmer.Stem(token);
        }
    }
}
