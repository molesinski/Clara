using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed class ExplicitMappingSynonym : Synonym
    {
        private readonly HashSetSlim<string> mappedPhrases = new();

        public ExplicitMappingSynonym(IEnumerable<string> phrases, IEnumerable<string> mappedPhrases)
            : base(phrases)
        {
            if (mappedPhrases is null)
            {
                throw new ArgumentNullException(nameof(mappedPhrases));
            }

            foreach (var mappedPhrase in mappedPhrases)
            {
                if (string.IsNullOrWhiteSpace(mappedPhrase))
                {
                    throw new ArgumentException("Mapped phrases cannot be null, empty or whitespace.", nameof(mappedPhrases));
                }

                this.mappedPhrases.Add(mappedPhrase);
            }

            if (!(this.Phrases.Count >= 1))
            {
                throw new ArgumentException("At least one phrase must be specified.");
            }

            if (!(this.MappedPhrases.Count >= 1))
            {
                throw new ArgumentException("At least one mapped phrase must be specified.");
            }
        }

        public IReadOnlyCollection<string> MappedPhrases
        {
            get
            {
                return this.mappedPhrases;
            }
        }
    }
}
