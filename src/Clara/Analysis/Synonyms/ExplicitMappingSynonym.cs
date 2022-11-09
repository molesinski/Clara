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

            this.MappedPhrase = mappedPhrase;
        }

        public string MappedPhrase { get; }
    }
}
