using System;

namespace Clara.Analysis.Stemming
{
    public class StemTokenFilter : ITokenFilter
    {
        private readonly IStemmer stemmer;
        private readonly bool tokenOnEmptyStem;

        public StemTokenFilter(IStemmer stemmer, bool tokenOnEmptyStem = true)
        {
            if (stemmer is null)
            {
                throw new ArgumentNullException(nameof(stemmer));
            }

            this.stemmer = stemmer;
            this.tokenOnEmptyStem = tokenOnEmptyStem;
        }

        public Token Filter(Token token)
        {
            var stem = this.stemmer.Stem(token);

            if (!stem.IsEmpty)
            {
                return stem;
            }

            if (this.tokenOnEmptyStem)
            {
                return token;
            }

            return default;
        }
    }
}
