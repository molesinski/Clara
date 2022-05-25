using System;

namespace Clara.Analysis.Stemming
{
    public class MinimumLengthStemmerDecorator : IStemmer
    {
        private readonly IStemmer stemmer;
        private readonly int minimumLength;

        public MinimumLengthStemmerDecorator(IStemmer stemmer, int minimumLength = 3)
        {
            if (stemmer is null)
            {
                throw new ArgumentNullException(nameof(stemmer));
            }

            if (minimumLength < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            }

            this.stemmer = stemmer;
            this.minimumLength = minimumLength;
        }

        public StemResult Stem(string token)
        {
            var length = token.Length;

            if (length < this.minimumLength)
            {
                return default;
            }

            return this.stemmer.Stem(token);
        }
    }
}
