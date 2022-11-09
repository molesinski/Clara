using Clara.Querying;

namespace Clara.Storage
{
    internal abstract class FieldStore : IDisposable
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

        public virtual FieldFacetResult? Facet(FacetExpression facetExpression, FilterExpression? filterExpression, IEnumerable<int> documents)
        {
            throw new InvalidOperationException("Field does not support faceting.");
        }

        public virtual SortedDocumentSet Sort(SortExpression sortExpression, DocumentSet documentSet)
        {
            throw new InvalidOperationException("Field does not support sorting.");
        }

        public abstract void Dispose();
    }
}
