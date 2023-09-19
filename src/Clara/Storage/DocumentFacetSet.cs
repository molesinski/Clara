using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct DocumentFacetSet : IDisposable
    {
        private static readonly HashSetSlim<int> Empty = new();

        private readonly Field field;
        private readonly HashSetSlim<int> allDocuments;
        private readonly ObjectPoolLease<HashSetSlim<int>>? facetDocuments;

        public DocumentFacetSet(Field field, HashSetSlim<int> allDocuments)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (allDocuments is null)
            {
                throw new ArgumentNullException(nameof(allDocuments));
            }

            this.field = field;
            this.allDocuments = allDocuments;
            this.facetDocuments = null;
        }

        public DocumentFacetSet(Field field, ObjectPoolLease<HashSetSlim<int>> facetDocuments)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            this.field = field;
            this.allDocuments = Empty;
            this.facetDocuments = facetDocuments;
        }

        public readonly Field Field
        {
            get
            {
                return this.field;
            }
        }

        public readonly HashSetSlim<int> FacetDocuments
        {
            get
            {
                return this.facetDocuments?.Instance ?? this.allDocuments;
            }
        }

        public DocumentFacetSet IntersectWith(IEnumerable<int> documents)
        {
            if (this.facetDocuments is null)
            {
                var facetDocuments = SharedObjectPools.DocumentSets.Lease();

                facetDocuments.Instance.UnionWith(documents);

                return new DocumentFacetSet(this.field, facetDocuments);
            }
            else
            {
                this.facetDocuments.Value.Instance.IntersectWith(documents);

                return this;
            }
        }

        public DocumentFacetSet ExceptWith(IEnumerable<int> documents)
        {
            if (this.facetDocuments is null)
            {
                var facetDocuments = SharedObjectPools.DocumentSets.Lease();

                facetDocuments.Instance.UnionWith(this.allDocuments);
                facetDocuments.Instance.ExceptWith(documents);

                return new DocumentFacetSet(this.field, facetDocuments);
            }
            else
            {
                this.facetDocuments.Value.Instance.ExceptWith(documents);

                return this;
            }
        }

        public void Dispose()
        {
            this.facetDocuments?.Dispose();
        }
    }
}
