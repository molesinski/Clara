using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class HierarchyFacetResult : FacetResult
    {
        private readonly HierarchyFacetValueCollection items;
        private bool isDisposed;

        internal HierarchyFacetResult(
            HierarchyField field,
            ObjectPoolLease<ListSlim<HierarchyFacetValue>> items,
            int offset,
            int count)
                : base(field)
        {
            this.items = new HierarchyFacetValueCollection(items, offset, count);
        }

        public HierarchyFacetValueCollection Values
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.items;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                this.items.Dispose();

                this.isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}
