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

        public virtual DocumentScoring Search(SearchExpression searchExpression, DocumentSet documentSet)
        {
            throw new InvalidOperationException("Field does not support searching.");
        }

        public virtual void Filter(FilterExpression filterExpression, DocumentSet documentSet)
        {
            throw new InvalidOperationException("Field does not support filtering.");
        }

        public virtual FacetResult? Facet(FacetExpression facetExpression, FilterExpression? filterExpression, IEnumerable<int> documents)
        {
            throw new InvalidOperationException("Field does not support faceting.");
        }

        public virtual SortedDocumentSet Sort(SortExpression sortExpression, DocumentSet documentSet)
        {
            throw new InvalidOperationException("Field does not support sorting.");
        }
    }
}
