using System;
using System.Collections.Generic;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class FieldFacetResult : IDisposable
    {
        private readonly IEnumerable<IDisposable> disposables;

        public FieldFacetResult(FacetResult facetResult)
            : this(facetResult, Array.Empty<IDisposable>())
        {
        }

        public FieldFacetResult(FacetResult facetResult, IEnumerable<IDisposable> disposables)
        {
            if (facetResult is null)
            {
                throw new ArgumentNullException(nameof(facetResult));
            }

            if (disposables is null)
            {
                throw new ArgumentNullException(nameof(disposables));
            }

            this.FacetResult = facetResult;
            this.disposables = disposables;
        }

        public FacetResult FacetResult { get; }

        public void Dispose()
        {
            foreach (var disposable in this.disposables)
            {
                disposable.Dispose();
            }
        }
    }
}
