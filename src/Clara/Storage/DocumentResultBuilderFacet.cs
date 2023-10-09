using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal readonly struct DocumentResultBuilderFacet : IDisposable
    {
        private static readonly HashSetSlim<int> Empty = new();

        private readonly Field field;
        private readonly HashSetSlim<int> allDocuments;
        private readonly ObjectPoolLease<HashSetSlim<int>>? facetDocuments;

        public DocumentResultBuilderFacet(Field field, HashSetSlim<int> allDocuments)
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

        public DocumentResultBuilderFacet(Field field, ObjectPoolLease<HashSetSlim<int>> facetDocuments)
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

        public readonly DocumentResultBuilderFacet IntersectWith<TValue>(DictionarySlim<int, TValue> documents)
        {
            if (this.facetDocuments is null)
            {
                var facetDocuments = SharedObjectPools.DocumentSets.Lease();

                foreach (var key in documents.Keys)
                {
                    facetDocuments.Instance.Add(key);
                }

                return new DocumentResultBuilderFacet(this.field, facetDocuments);
            }
            else
            {
                this.facetDocuments.Value.Instance.IntersectWith(documents.Keys);

                return this;
            }
        }

        public readonly DocumentResultBuilderFacet IntersectWith(HashSetSlim<int> documents)
        {
            if (this.facetDocuments is null)
            {
                var facetDocuments = SharedObjectPools.DocumentSets.Lease();

                facetDocuments.Instance.UnionWith(documents);

                return new DocumentResultBuilderFacet(this.field, facetDocuments);
            }
            else
            {
                this.facetDocuments.Value.Instance.IntersectWith(documents);

                return this;
            }
        }

        public readonly DocumentResultBuilderFacet ExceptWith(HashSetSlim<int> documents)
        {
            if (this.facetDocuments is null)
            {
                var facetDocuments = SharedObjectPools.DocumentSets.Lease();

                facetDocuments.Instance.UnionWith(this.allDocuments);
                facetDocuments.Instance.ExceptWith(documents);

                return new DocumentResultBuilderFacet(this.field, facetDocuments);
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
