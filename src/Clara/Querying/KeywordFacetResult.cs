using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFacetResult : TokenFacetResult<KeywordValue>
    {
        public KeywordFacetResult(KeywordField field, IEnumerable<KeywordValue> values)
            : base(field, values)
        {
        }
    }
}
