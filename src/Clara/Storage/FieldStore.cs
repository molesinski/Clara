using System;
using System.Collections.Generic;
using Clara.Querying;

namespace Clara.Storage
{
    internal abstract class FieldStore
    {
        protected FieldStore()
        {
        }

        public virtual double FilterOrder
        {
            get
            {
                return double.MaxValue;
            }
        }

        public virtual void Filter(FilterExpression filterExpression, DocumentSet documentSet)
        {
            throw new InvalidOperationException("Field does not support filtering.");
        }

        public virtual FacetResult? Facet(FacetExpression facetExpression, IEnumerable<FilterExpression> filterExpressions, IEnumerable<int> documents)
        {
            throw new InvalidOperationException("Field does not support faceting.");
        }

        public virtual void Sort(SortExpression sortExpression, DocumentSort documentSort)
        {
            throw new InvalidOperationException("Field does not support sorting.");
        }
    }
}
