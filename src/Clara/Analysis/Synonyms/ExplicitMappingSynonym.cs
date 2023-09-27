namespace Clara.Analysis.Synonyms
{
    public sealed class ExplicitMappingSynonym : Synonym
    {
        public ExplicitMappingSynonym(IEnumerable<string> phrases, string mappedPhrase)
            : base(phrases)
        {
            if (mappedPhrase is null)
            {
                throw new ArgumentNullException(nameof(mappedPhrase));
            }

            if (string.IsNullOrWhiteSpace(mappedPhrase))
            {
                throw new ArgumentException("Mapped phrase cannot be empty or whitespace.", nameof(mappedPhrase));
            }

            this.MappedPhrase = mappedPhrase;
        }

        public string MappedPhrase { get; }
    }
}
