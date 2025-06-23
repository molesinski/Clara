using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;
using Clara.Utils;

namespace Clara
{
    public abstract class Index
    {
        internal Index()
        {
        }

        internal abstract FieldStore GetFieldStore(Field field);
    }

    public sealed class Index<TDocument> : Index
    {
        private readonly TokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, TDocument> documentMap;
        private readonly Dictionary<Field, FieldStore> fieldStores;
        private readonly HashSetSlim<int> allDocuments;

        internal Index(
            TokenEncoder tokenEncoder,
            DictionarySlim<int, TDocument> documentMap,
            Dictionary<Field, FieldStore> fieldStores)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (documentMap is null)
            {
                throw new ArgumentNullException(nameof(documentMap));
            }

            if (fieldStores is null)
            {
                throw new ArgumentNullException(nameof(fieldStores));
            }

            this.tokenEncoder = tokenEncoder;
            this.documentMap = documentMap;
            this.fieldStores = fieldStores;
            this.allDocuments = new HashSetSlim<int>(capacity: documentMap.Count);

            foreach (var pair in documentMap)
            {
                this.allDocuments.Add(pair.Key);
            }
        }

        public QueryBuilder QueryBuilder()
        {
            return new QueryBuilder(this);
        }

        public QueryResult<TDocument> Query(QueryBuilder queryBuilder)
        {
            if (queryBuilder is null)
            {
                throw new ArgumentNullException(nameof(queryBuilder));
            }

            using var query = queryBuilder.ToQuery();

            return this.Query(query);
        }

        public QueryResult<TDocument> Query(Action<QueryBuilder> configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            using var queryBuilder = this.QueryBuilder();

            configuration(queryBuilder);

            using var query = queryBuilder.ToQuery();

            return this.Query(query);
        }

        public QueryResult<TDocument> Query(Query query)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            if (query.Index != this)
            {
                throw new InvalidOperationException("Query must be built off current index.");
            }

            var documentResultBuilder = new DocumentResultBuilder(this.allDocuments);
            var documentScoring = default(DocumentScoring);
            var documentList = default(DocumentList);

            if (query.IncludeDocuments is not null)
            {
                using var includedDocuments = SharedObjectPools.DocumentSets.Lease();

                var hasValues = false;

                foreach (var includedDocument in query.IncludeDocuments)
                {
                    if (includedDocument is not null)
                    {
                        hasValues = true;

                        if (this.tokenEncoder.TryEncode(includedDocument, out var documentId))
                        {
                            includedDocuments.Instance.Add(documentId);
                        }
                    }
                }

                if (hasValues)
                {
                    documentResultBuilder.IntersectWith(includedDocuments.Instance);
                }
            }

            if (query.Search is SearchExpression searchExpression)
            {
                if (!searchExpression.IsEmpty)
                {
                    documentScoring = searchExpression.Search(this);

                    documentResultBuilder.IntersectWith(documentScoring.Value);
                }
            }

            if (query.Filters.Count > 0)
            {
                using var facetFields = SharedObjectPools.FieldSets.Lease();

                for (var i = 0; i < query.Facets.Count; i++)
                {
                    facetFields.Instance.Add(query.Facets[i].Field);
                }

                using var filterExpressions = SharedObjectPools.FilterExpressions.Lease();

                for (var i = 0; i < query.Filters.Count; i++)
                {
                    var filterExpression = query.Filters[i];

                    if (!filterExpression.IsEmpty)
                    {
                        filterExpressions.Instance.Add(filterExpression);
                    }
                }

                if (filterExpressions.Instance.Count > 0)
                {
                    if (filterExpressions.Instance.Count > 1)
                    {
                        using var comparer = SharedObjectPools.FilterExpressionComparers.Lease();

                        comparer.Instance.Initialize(facetFields.Instance, this.fieldStores);

                        filterExpressions.Instance.Sort(comparer.Instance);
                    }

                    foreach (var filterExpression in filterExpressions.Instance)
                    {
                        var field = filterExpression.Field;
                        var store = this.GetFieldStore(field);

                        if (filterExpression.HasPersistedFacets)
                        {
                            if (facetFields.Instance.Contains(field))
                            {
                                documentResultBuilder.PersistFacets(field);
                            }
                        }

                        using var documents = store.Filter(filterExpression);

                        documentResultBuilder.IntersectWith(field, documents.Value);
                    }
                }
            }

            if (query.ExcludeDocuments is not null)
            {
                using var excludeDocuments = SharedObjectPools.DocumentSets.Lease();

                foreach (var excludeDocument in query.ExcludeDocuments)
                {
                    if (excludeDocument is not null)
                    {
                        if (this.tokenEncoder.TryEncode(excludeDocument, out var documentId))
                        {
                            excludeDocuments.Instance.Add(documentId);
                        }
                    }
                }

                if (excludeDocuments.Instance.Count > 0)
                {
                    documentResultBuilder.ExceptWith(excludeDocuments.Instance);
                }
            }

            var facetResults = SharedObjectPools.FacetResults.Lease();

            for (var i = 0; i < query.Facets.Count; i++)
            {
                var facetExpression = query.Facets[i];
                var field = facetExpression.Field;
                var store = this.GetFieldStore(field);
                var fieldFilterExpression = default(FilterExpression);

                for (var j = 0; j < query.Filters.Count; j++)
                {
                    var filterExpression = query.Filters[j];

                    if (!filterExpression.IsEmpty)
                    {
                        if (filterExpression.Field == field)
                        {
                            fieldFilterExpression = filterExpression;
                            break;
                        }
                    }
                }

                facetResults.Instance.Add(store.Facet(facetExpression, fieldFilterExpression, documentResultBuilder.GetFacetDocuments(field)));
            }

            if (documentResultBuilder.Documents.Count > 0)
            {
                if (query.Sort is SortExpression sortExpression)
                {
                    var field = sortExpression.Field;
                    var store = this.GetFieldStore(field);

                    documentList = store.Sort(sortExpression, documentResultBuilder.Documents);
                }
                else if (documentScoring.Value.Count > 0)
                {
                    using var sortedDocuments = SharedObjectPools.ScoredDocuments.Lease();

                    var scores = documentScoring.Value;

                    foreach (var documentId in documentResultBuilder.Documents)
                    {
                        scores.TryGetValue(documentId, out var score);

                        sortedDocuments.Instance.Add(new DocumentValue<float>(documentId, score));
                    }

                    sortedDocuments.Instance.Sort(DocumentValueComparer<float>.Descending);

                    var documents = SharedObjectPools.Documents.Lease();

                    foreach (var documentValue in sortedDocuments.Instance)
                    {
                        documents.Instance.Add(documentValue.DocumentId);
                    }

                    documentList = new DocumentList(documents);
                }
                else
                {
                    var documents = SharedObjectPools.Documents.Lease();

                    foreach (var documentId in documentResultBuilder.Documents)
                    {
                        documents.Instance.Add(documentId);
                    }

                    documentList = new DocumentList(documents);
                }
            }

            documentResultBuilder.Dispose();

            return new QueryResult<TDocument>(this.tokenEncoder, this.documentMap, documentScoring, documentList, facetResults);
        }

        internal override FieldStore GetFieldStore(Field field)
        {
            if (!this.fieldStores.TryGetValue(field, out var store))
            {
                throw new InvalidOperationException("Query expression references field not belonging to current index.");
            }

            return store;
        }
    }
}
