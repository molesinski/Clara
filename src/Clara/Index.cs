using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Clara.Utils;

namespace Clara
{
    public abstract class Index
    {
        protected internal Index()
        {
        }

        internal abstract bool HasField(Field field);
    }

    public sealed class Index<TDocument> : Index
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, TDocument> documents;
        private readonly Dictionary<Field, FieldStore> fieldStores;
        private readonly HashSetSlim<int> allDocuments;

        internal Index(
            ITokenEncoder tokenEncoder,
            DictionarySlim<int, TDocument> documents,
            Dictionary<Field, FieldStore> fieldStores)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            if (fieldStores is null)
            {
                throw new ArgumentNullException(nameof(fieldStores));
            }

            this.tokenEncoder = tokenEncoder;
            this.documents = documents;
            this.fieldStores = fieldStores;
            this.allDocuments = new HashSetSlim<int>(capacity: documents.Count);

            foreach (var pair in documents)
            {
                this.allDocuments.Add(pair.Key);
            }
        }

        public QueryBuilder QueryBuilder()
        {
            return new QueryBuilder(this);
        }

        public QueryResult<TDocument> Query(Query query)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var documentSet = default(DocumentSet);

            if (query.IncludeDocuments is not null)
            {
                var includedDocuments = default(ObjectPoolLease<HashSetSlim<int>>?);

                foreach (var includedDocument in query.IncludeDocuments)
                {
                    if (includedDocument is not null)
                    {
                        includedDocuments ??= SharedObjectPools.DocumentSets.Lease();

                        if (this.tokenEncoder.TryEncode(includedDocument, out var documentId))
                        {
                            includedDocuments.Value.Instance.Add(documentId);
                        }
                    }
                }

                if (includedDocuments is not null)
                {
                    documentSet = new DocumentSet(includedDocuments.Value);
                }
            }

            documentSet ??= new DocumentSet(this.allDocuments);

            var documentScoring = DocumentScoring.Empty;

            if (query.Search is SearchExpression searchExpression)
            {
                var field = searchExpression.Field;

                if (!this.fieldStores.TryGetValue(field, out var store))
                {
                    throw new InvalidOperationException("Search expression references field not belonging to current index.");
                }

                documentScoring = store.Search(searchExpression, documentSet);
            }

            if (query.Filters.Count > 0)
            {
                using var facetFields = SharedObjectPools.FieldSets.Lease();

                if (query.Facets.Count > 0)
                {
                    foreach (var facet in query.Facets)
                    {
                        facetFields.Instance.Add(facet.Field);
                    }
                }

                using var filterExpressions = SharedObjectPools.FilterExpressions.Lease();

                foreach (var filterExpression in query.Filters)
                {
                    filterExpressions.Instance.Add(filterExpression);
                }

                filterExpressions.Instance.Sort(new FilterExpressionComparer(facetFields.Instance, this.fieldStores));

                foreach (var filterExpression in filterExpressions.Instance)
                {
                    if (documentSet.Count == 0)
                    {
                        break;
                    }

                    var field = filterExpression.Field;

                    if (!this.fieldStores.TryGetValue(field, out var store))
                    {
                        throw new InvalidOperationException("Filter expression references field not belonging to current index.");
                    }

                    if (filterExpression.IsBranchingRequiredForFaceting)
                    {
                        if (facetFields.Instance.Contains(field))
                        {
                            documentSet.Facet(field);
                        }
                    }

                    store.Filter(filterExpression, documentSet);
                }
            }

            if (query.ExcludeDocuments is not null)
            {
                var excludeDocuments = default(ObjectPoolLease<HashSetSlim<int>>?);

                try
                {
                    foreach (var excludeDocument in query.ExcludeDocuments)
                    {
                        if (excludeDocument is not null)
                        {
                            excludeDocuments ??= SharedObjectPools.DocumentSets.Lease();

                            if (this.tokenEncoder.TryEncode(excludeDocument, out var documentId))
                            {
                                excludeDocuments.Value.Instance.Add(documentId);
                            }
                        }
                    }

                    if (excludeDocuments is not null)
                    {
                        documentSet.ExceptWith(excludeDocuments.Value.Instance);
                    }
                }
                finally
                {
                    excludeDocuments?.Dispose();
                }
            }

            var facetResults = SharedObjectPools.FacetResults.Lease();

            if (query.Facets.Count > 0)
            {
                foreach (var facetExpression in query.Facets)
                {
                    var field = facetExpression.Field;
                    var facetDocuments = documentSet.GetFacetDocuments(field);

                    if (facetDocuments.Count > 0)
                    {
                        if (!this.fieldStores.TryGetValue(field, out var store))
                        {
                            throw new InvalidOperationException("Facet expression references field not belonging to current index.");
                        }

                        var filterExpression = default(FilterExpression);

                        foreach (var item in query.Filters)
                        {
                            if (item.Field == field)
                            {
                                filterExpression = item;
                                break;
                            }
                        }

                        var facetResult = store.Facet(facetExpression, filterExpression, facetDocuments);

                        if (facetResult is not null)
                        {
                            facetResults.Instance.Add(facetResult);
                        }
                    }
                }
            }

            var sortedDocumentSet = (IDocumentSet)documentSet;

            if (query.Sort is SortExpression sortExpression)
            {
                var field = sortExpression.Field;

                if (!this.fieldStores.TryGetValue(field, out var store))
                {
                    throw new InvalidOperationException("Sort expression references field not belonging to current index.");
                }

                sortedDocumentSet = store.Sort(sortExpression, documentSet);
            }
            else if (!documentScoring.IsEmpty)
            {
                sortedDocumentSet = new SortedDocumentSet<float>(
                    documentSet,
                    documentScoring.GetScore,
                    DocumentValueComparer<float>.Descending);
            }

            return
                new QueryResult<TDocument>(
                    this.tokenEncoder,
                    this.documents,
                    sortedDocumentSet,
                    documentScoring,
                    facetResults);
        }

        internal override bool HasField(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            return this.fieldStores.ContainsKey(field);
        }

        private readonly struct FilterExpressionComparer : IComparer<FilterExpression>
        {
            private readonly HashSetSlim<Field> facetFields;
            private readonly Dictionary<Field, FieldStore> fieldStores;

            public FilterExpressionComparer(
                HashSetSlim<Field> facetFields,
                Dictionary<Field, FieldStore> fieldStores)
            {
                if (facetFields is null)
                {
                    throw new ArgumentNullException(nameof(facetFields));
                }

                if (fieldStores is null)
                {
                    throw new ArgumentNullException(nameof(fieldStores));
                }

                this.facetFields = facetFields;
                this.fieldStores = fieldStores;
            }

            public int Compare(FilterExpression? x, FilterExpression? y)
            {
                if (x is null)
                {
                    throw new ArgumentNullException(nameof(x));
                }

                if (y is null)
                {
                    throw new ArgumentNullException(nameof(y));
                }

                var a = x.IsBranchingRequiredForFaceting && this.facetFields.Contains(x.Field) ? 1 : 0;
                var b = y.IsBranchingRequiredForFaceting && this.facetFields.Contains(y.Field) ? 1 : 0;

                var result = a.CompareTo(b);

                if (result != 0)
                {
                    return result;
                }

                var c = this.fieldStores.TryGetValue(x.Field, out var store1) ? store1.FilterOrder : double.MinValue;
                var d = this.fieldStores.TryGetValue(y.Field, out var store2) ? store2.FilterOrder : double.MinValue;

                return c.CompareTo(d);
            }
        }
    }
}
