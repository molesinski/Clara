using Clara.Analysis;
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

        internal abstract bool ContainsField(Field field);
    }

    public sealed class Index<TDocument> : Index
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, TDocument> documentMap;
        private readonly Dictionary<Field, FieldStore> fieldStores;
        private readonly HashSetSlim<int> allDocuments;

        internal Index(
            ITokenEncoder tokenEncoder,
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

        public QueryResult<TDocument> Query(Query query)
        {
            if (query is null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var documentResultBuilder = new DocumentResultBuilder(this.allDocuments);
            var documentScoring = default(DocumentScoring);
            var documentList = default(DocumentList);

            if (query.IncludeDocuments is not null)
            {
                using var includedDocuments = SharedObjectPools.DocumentSets.Lease();

                foreach (var includedDocument in query.IncludeDocuments)
                {
                    if (includedDocument is not null)
                    {
                        if (this.tokenEncoder.TryEncode(new Token(includedDocument), out var documentId))
                        {
                            includedDocuments.Instance.Add(documentId);
                        }
                    }
                }

                if (includedDocuments.Instance.Count > 0)
                {
                    documentResultBuilder.IntersectWith(field: null, includedDocuments.Instance);
                }
            }

            if (query.Search is SearchExpression searchExpression)
            {
                if (!searchExpression.IsEmpty)
                {
                    var field = searchExpression.Field;

                    if (!this.fieldStores.TryGetValue(field, out var store))
                    {
                        throw new InvalidOperationException("Search expression references field not belonging to current index.");
                    }

                    documentScoring = store.Search(searchExpression, ref documentResultBuilder);
                }
            }

            if (query.Filters.Count > 0)
            {
                using var facetFields = SharedObjectPools.FieldSets.Lease();

                if (query.Facets.Count > 0)
                {
                    foreach (var facet in (ListSlim<FacetExpression>)query.Facets)
                    {
                        facetFields.Instance.Add(facet.Field);
                    }
                }

                using var filterExpressions = SharedObjectPools.FilterExpressions.Lease();

                foreach (var filterExpression in (ListSlim<FilterExpression>)query.Filters)
                {
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

                        if (!this.fieldStores.TryGetValue(field, out var store))
                        {
                            throw new InvalidOperationException("Filter expression references field not belonging to current index.");
                        }

                        if (filterExpression.IsBranchingRequiredForFaceting)
                        {
                            if (facetFields.Instance.Contains(field))
                            {
                                documentResultBuilder.Facet(field);
                            }
                        }

                        store.Filter(filterExpression, ref documentResultBuilder);
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
                        if (this.tokenEncoder.TryEncode(new Token(excludeDocument), out var documentId))
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

            if (query.Facets.Count > 0)
            {
                foreach (var facetExpression in (ListSlim<FacetExpression>)query.Facets)
                {
                    var field = facetExpression.Field;

                    if (!this.fieldStores.TryGetValue(field, out var store))
                    {
                        throw new InvalidOperationException("Facet expression references field not belonging to current index.");
                    }

                    var fieldFilterExpression = default(FilterExpression);

                    foreach (var filterExpression in (ListSlim<FilterExpression>)query.Filters)
                    {
                        if (!filterExpression.IsEmpty)
                        {
                            if (filterExpression.Field == field)
                            {
                                fieldFilterExpression = filterExpression;
                                break;
                            }
                        }
                    }

                    facetResults.Instance.Add(store.Facet(facetExpression, fieldFilterExpression, ref documentResultBuilder));
                }
            }

            if (documentResultBuilder.Documents.Count > 0)
            {
                if (query.Sort is SortExpression sortExpression)
                {
                    var field = sortExpression.Field;

                    if (!this.fieldStores.TryGetValue(field, out var store))
                    {
                        throw new InvalidOperationException("Sort expression references field not belonging to current index.");
                    }

                    documentList = store.Sort(sortExpression, ref documentResultBuilder);
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

        internal override bool ContainsField(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            return this.fieldStores.ContainsKey(field);
        }
    }
}
