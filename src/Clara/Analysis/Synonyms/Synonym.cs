using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public abstract class Synonym
    {
        private readonly HashSetSlim<string> phrases = new();

        internal Synonym(IEnumerable<string> phrases)
        {
            if (phrases is null)
            {
                throw new ArgumentNullException(nameof(phrases));
            }

            foreach (var phrase in phrases)
            {
                if (string.IsNullOrWhiteSpace(phrase))
                {
                    throw new ArgumentException("Phrases cannot be empty or whitespace.", nameof(phrases));
                }

                this.phrases.Add(phrase);
            }
        }

        public IReadOnlyCollection<string> Phrases
        {
            get
            {
                return this.phrases;
            }
        }
    }
}
