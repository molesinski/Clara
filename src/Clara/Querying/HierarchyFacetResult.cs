using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFacetResult : TokenFacetResult<HierarchyFacetValue>, IDisposable
    {
        private readonly IDisposable? disposable;

        internal HierarchyFacetResult(
            HierarchyField field,
            IEnumerable<HierarchyFacetValue> values,
            IDisposable? disposable)
                : base(field, values)
        {
            this.disposable = disposable;
        }

        public void Dispose()
        {
            this.disposable?.Dispose();
        }
    }
}
