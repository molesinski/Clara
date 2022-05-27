using System;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class FieldFacetResult : IDisposable
    {
        private readonly IDisposable? disposable;

        public FieldFacetResult(FacetResult facetResult)
            : this(facetResult, default)
        {
        }

        public FieldFacetResult(FacetResult facetResult, IDisposable? disposable)
        {
            if (facetResult is null)
            {
                throw new ArgumentNullException(nameof(facetResult));
            }

            this.FacetResult = facetResult;
            this.disposable = disposable;
        }

        public FacetResult FacetResult { get; }

        public void Dispose()
        {
            this.disposable?.Dispose();
        }
    }
}
