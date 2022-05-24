using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFacetResult : TokenFacetResult<KeywordFacetValue>
    {
        public KeywordFacetResult(KeywordField field, IEnumerable<KeywordFacetValue> values)
            : base(field, values)
        {
        }
    }
}
