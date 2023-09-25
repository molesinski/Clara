using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal ref struct DocumentResultBuilder
    {
        private readonly HashSetSlim<int> allDocuments;
        private ObjectPoolLease<ListSlim<DocumentResultBuilderFacet>>? facets;
        private ObjectPoolLease<HashSetSlim<int>>? documents;

        public DocumentResultBuilder(HashSetSlim<int> allDocuments)
        {
            if (allDocuments is null)
            {
                throw new ArgumentNullException(nameof(allDocuments));
            }

            this.allDocuments = allDocuments;
            this.documents = null;
        }

        public readonly HashSetSlim<int> Documents
        {
            get
            {
                return this.documents?.Instance ?? this.allDocuments;
            }
        }

        public readonly HashSetSlim<int> GetFacetDocuments(Field field)
        {
            if (this.facets is not null)
            {
                foreach (var item in this.facets.Value.Instance)
                {
                    if (item.Field == field)
                    {
                        return item.FacetDocuments;
                    }
                }
            }

            return this.documents?.Instance ?? this.allDocuments;
        }

        public void Facet(Field field)
        {
            this.facets ??= SharedObjectPools.QueryResultBuilderFacets.Lease();

            if (this.documents is null)
            {
                this.facets.Value.Instance.Add(new DocumentResultBuilderFacet(field, this.allDocuments));
            }
            else
            {
                var facetDocuments = SharedObjectPools.DocumentSets.Lease();

                facetDocuments.Instance.UnionWith(this.documents.Value.Instance);

                this.facets.Value.Instance.Add(new DocumentResultBuilderFacet(field, facetDocuments));
            }
        }

        public void Clear()
        {
            if (this.documents is null)
            {
                this.documents = SharedObjectPools.DocumentSets.Lease();
            }
            else
            {
                this.documents.Value.Instance.Clear();
            }

            if (this.facets is not null)
            {
                foreach (var item in this.facets.Value.Instance)
                {
                    item.Dispose();
                }

                this.facets.Value.Instance.Clear();
            }
        }

        public void IntersectWith(Field? field, IEnumerable<int> documents)
        {
            if (this.documents is null)
            {
                this.documents = SharedObjectPools.DocumentSets.Lease();
                this.documents.Value.Instance.UnionWith(documents);
            }
            else
            {
                this.documents.Value.Instance.IntersectWith(documents);

                if (this.facets is not null)
                {
                    var count = this.facets.Value.Instance.Count;

                    for (var i = 0; i < count; i++)
                    {
                        var item = this.facets.Value.Instance[i];

                        if (item.Field != field)
                        {
                            this.facets.Value.Instance[i] = item.IntersectWith(documents);
                        }
                    }
                }
            }
        }

        public void ExceptWith(IEnumerable<int> documents)
        {
            if (this.documents is null)
            {
                this.documents = SharedObjectPools.DocumentSets.Lease();
                this.documents.Value.Instance.UnionWith(this.allDocuments);
            }

            this.documents.Value.Instance.ExceptWith(documents);

            if (this.facets is not null)
            {
                var count = this.facets.Value.Instance.Count;

                for (var i = 0; i < count; i++)
                {
                    var item = this.facets.Value.Instance[i];

                    this.facets.Value.Instance[i] = item.ExceptWith(documents);
                }
            }
        }

        public void Dispose()
        {
            if (this.facets is not null)
            {
                foreach (var item in this.facets.Value.Instance)
                {
                    item.Dispose();
                }
            }

            this.facets?.Dispose();
            this.documents?.Dispose();
        }
    }
}
