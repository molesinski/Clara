namespace Clara.Analysis.Synonyms
{
    public sealed class EquivalencySynonym : Synonym
    {
        public EquivalencySynonym(IEnumerable<string> phrases)
            : base(phrases)
        {
            if (!(this.Phrases.Count >= 2))
            {
                throw new ArgumentException("At least two phrases must be specified.");
            }
        }
    }
}
