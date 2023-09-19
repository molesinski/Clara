using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class KeywordFacetResult : TokenFacetResult<KeywordFacetValue>, IDisposable
    {
        private readonly ObjectPoolLease<ListSlim<KeywordFacetValue>> lease;

        internal KeywordFacetResult(
            KeywordField field,
            IEnumerable<KeywordFacetValue> values,
            ObjectPoolLease<ListSlim<KeywordFacetValue>> lease)
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
