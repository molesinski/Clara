using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class KeywordFacetResult : FacetResult
    {
        private readonly KeywordFacetValueCollection items;
        private bool isDisposed;

        internal KeywordFacetResult(
            KeywordField field,
            ObjectPoolLease<ListSlim<KeywordFacetValue>> items)
            : base(field)
        {
            this.items = new KeywordFacetValueCollection(items);
        }

        public KeywordFacetValueCollection Values
        {
            get
            {
                this.ThrowIfDisposed();

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

        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
