using System.Text;
using Clara.Utils;

namespace Clara.Analysis.Synonyms
{
    public sealed class EquivalencySynonym : Synonym
    {
        public EquivalencySynonym(IEnumerable<string> phrases)
            : base(phrases)
        {
            if (!(this.Phrases.Count >= 2))
            {
                throw new ArgumentException("At least two unique phrases must be specified.");
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var isFirst = true;

            foreach (var phrase in (HashSetSlim<string>)this.Phrases)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append(phrase);
                isFirst = false;
            }

            return builder.ToString();
        }
    }
}
