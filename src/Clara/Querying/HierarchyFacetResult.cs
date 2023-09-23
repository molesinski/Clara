using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class HierarchyFacetResult : TokenFacetResult<HierarchyFacetValue>
    {
        private readonly ObjectPoolLease<ListSlim<HierarchyFacetValue>> lease;
        private readonly IEnumerable<HierarchyFacetValue> selectedValues;
        private bool isDisposed;

        internal HierarchyFacetResult(
            HierarchyField field,
            ObjectPoolLease<ListSlim<HierarchyFacetValue>> lease,
            IEnumerable<HierarchyFacetValue> selectedValues)
                : base(field)
        {
            this.lease = lease;
            this.selectedValues = selectedValues;
        }

        public override IEnumerable<HierarchyFacetValue> Values
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.selectedValues;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                this.lease.Dispose();

                this.isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
