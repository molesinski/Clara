using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal abstract class FieldStore
    {
        protected FieldStore()
        {
        }

        public virtual double? FilterOrder
        {
            get
            {
                return null;
            }
        }

        public virtual DocumentScoring Search(SearchExpression searchExpression)
        {
            throw new InvalidOperationException("Field does not support searching.");
        }

        public virtual DocumentSet Filter(FilterExpression filterExpression)
        {
            throw new InvalidOperationException("Field does not support filtering.");
        }

        public virtual FacetResult Facet(FacetExpression facetExpression, FilterExpression? filterExpression, HashSetSlim<int> documents)
        {
            throw new InvalidOperationException("Field does not support faceting.");
        }

        public virtual DocumentList Sort(SortExpression sortExpression, HashSetSlim<int> documents)
        {
            throw new InvalidOperationException("Field does not support sorting.");
        }
    }
}
