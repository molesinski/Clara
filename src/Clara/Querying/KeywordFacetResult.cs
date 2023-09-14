using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFacetResult : TokenFacetResult<KeywordFacetValue>, IDisposable
    {
        private readonly IDisposable? disposable;

        internal KeywordFacetResult(
            KeywordField field,
            IEnumerable<KeywordFacetValue> values,
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
