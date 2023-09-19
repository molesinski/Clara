using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class HierarchyFacetResult : TokenFacetResult<HierarchyFacetValue>, IDisposable
    {
        private readonly ObjectPoolLease<ListSlim<HierarchyFacetValue>> lease;

        internal HierarchyFacetResult(
            HierarchyField field,
            IEnumerable<HierarchyFacetValue> values,
            ObjectPoolLease<ListSlim<HierarchyFacetValue>> lease)
                : base(field, values)
        {
            this.lease = lease;
        }

        public void Dispose()
        {
            this.lease.Dispose();
        }
    }
}
