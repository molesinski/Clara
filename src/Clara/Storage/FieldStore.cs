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

        public virtual SearchFieldStore GetSearchFieldStore(SearchField searchField)
        {
            throw new InvalidOperationException("Field does not support searching.");
        }

        public virtual void Filter(FilterExpression filterExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            throw new InvalidOperationException("Field does not support filtering.");
        }

        public virtual FacetResult Facet(FacetExpression facetExpression, FilterExpression? filterExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            throw new InvalidOperationException("Field does not support faceting.");
        }

        public virtual DocumentList Sort(SortExpression sortExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            throw new InvalidOperationException("Field does not support sorting.");
        }
    }
}
