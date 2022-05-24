using System.Collections.Generic;

namespace Clara.Analysis.Synonyms
{
    public sealed class EquivalencySynonym : Synonym
    {
        public EquivalencySynonym(IEnumerable<string> phrases)
            : base(phrases)
        {
        }
    }
}
