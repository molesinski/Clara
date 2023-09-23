using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class KeywordFacetResult : TokenFacetResult<KeywordFacetValue>
    {
        private readonly ObjectPoolLease<ListSlim<KeywordFacetValue>> lease;
        private bool isDisposed;

        internal KeywordFacetResult(
            KeywordField field,
            ObjectPoolLease<ListSlim<KeywordFacetValue>> lease)
            : base(field)
        {
            this.lease = lease;
        }

        public override IEnumerable<KeywordFacetValue> Values
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.lease.Instance;
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
