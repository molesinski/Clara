using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public abstract class Synonym
    {
        private readonly ListSlim<string> phrases;

        internal Synonym(IEnumerable<string> phrases)
        {
            if (phrases is null)
            {
                throw new ArgumentNullException(nameof(phrases));
            }

            this.phrases = new ListSlim<string>();

            foreach (var phrase in phrases)
            {
                if (string.IsNullOrWhiteSpace(phrase))
                {
                    throw new ArgumentException("Phrases cannot be empty or whitespace.", nameof(phrases));
                }

                this.phrases.Add(phrase);
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
