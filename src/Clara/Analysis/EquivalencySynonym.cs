using System.Collections.Generic;

namespace Clara.Analysis
{
    public sealed class EquivalencySynonym : Synonym
    {
        public EquivalencySynonym(IEnumerable<string> phrases)
            : base(phrases)
        {
        }
    }
}
