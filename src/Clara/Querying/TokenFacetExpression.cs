using System;
using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class TokenFacetExpression<TValue> : FacetExpression
        where TValue : notnull
    {
        protected internal TokenFacetExpression(TokenField field, IComparer<TValue> comparer)
            : base(field)
        {
            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            this.Comparer = comparer;
        }

        public IComparer<TValue> Comparer { get; }

        public abstract FacetResult CreateResult(IEnumerable<TValue> values);
    }
}
