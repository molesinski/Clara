namespace Clara.Analysis.Synonyms
{
    public abstract class Synonym
    {
        private readonly List<string> phrases = new();

        internal Synonym(IEnumerable<string> phrases)
        {
            if (phrases is null)
            {
                throw new ArgumentNullException(nameof(phrases));
            }

            foreach (var phrase in phrases)
            {
                if (phrase is not null)
                {
                    this.phrases.Add(phrase);
                }
            }
        }

        public IEnumerable<string> Phrases
        {
            get
            {
                return this.phrases;
            }
        }
    }
}
