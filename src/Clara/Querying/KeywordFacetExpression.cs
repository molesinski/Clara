using System;
using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFacetExpression : TokenFacetExpression<KeywordValue>
    {
        public KeywordFacetExpression(KeywordField field, IComparer<KeywordValue>? comparer = null)
            : base(field, comparer ?? KeywordValueComparer.Default)
        {
        }

        public override FacetResult CreateResult(IEnumerable<KeywordValue> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return new KeywordFacetResult((KeywordField)this.Field, values);
        }
    }
}
