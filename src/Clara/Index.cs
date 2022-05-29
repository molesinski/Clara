using System;
using System.Collections.Generic;
using System.Linq;
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

    public sealed class Index<TDocument> : Index, IDisposable
    {
        private static readonly HashSet<Field> EmptyFacetFields = new();
        private static readonly List<FieldFacetResult> EmptyFacetResults = new();

        private readonly TokenEncoder tokenEncoder;
        private readonly PooledDictionarySlim<int, TDocument> documents;
        private readonly Dictionary<Field, FieldStore> fieldStores;
        private readonly PooledHashSetSlim<int> allDocuments;

        internal Index(
            TokenEncoder tokenEncoder,
            PooledDictionarySlim<int, TDocument> documents,
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
            this.allDocuments = new PooledHashSetSlim<int>(capacity: documents.Count);

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
                var includedDocuments = default(PooledHashSetSlim<int>);

                foreach (var includedDocument in query.IncludeDocuments)
                {
                    if (includedDocument is not null)
                    {
                        includedDocuments ??= new();

                        if (this.tokenEncoder.TryEncode(includedDocument, out var documentId))
                        {
                            includedDocuments.Add(documentId);
                        }
                    }
                }

                if (includedDocuments is not null)
                {
                    documentSet = new DocumentSet(includedDocuments);
                }
            }

            if (documentSet is null)
            {
                documentSet = new DocumentSet((IReadOnlyCollection<int>)this.allDocuments);
            }

            if (query.Filters.Count > 0)
            {
                var facetFields = EmptyFacetFields;

                if (query.Facets.Count > 0)
                {
                    facetFields = new HashSet<Field>();

                    foreach (var facet in query.Facets)
                    {
                        facetFields.Add(facet.Field);
                    }
                }

                var filterExpressions = query.Filters.ToArray();

                Array.Sort(filterExpressions, new FilterExpressionComparer(facetFields, this.fieldStores));

                foreach (var filterExpression in filterExpressions)
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
                        if (facetFields.Contains(field))
                        {
                            documentSet.Branch(field);
                        }
                    }

                    store.Filter(filterExpression, documentSet);
                }
            }

            if (query.ExcludeDocuments is not null)
            {
                var excludeDocuments = default(PooledHashSetSlim<int>);

                foreach (var excludeDocument in query.ExcludeDocuments)
                {
                    if (excludeDocument is not null)
                    {
                        excludeDocuments ??= new();

                        if (this.tokenEncoder.TryEncode(excludeDocument, out var documentId))
                        {
                            excludeDocuments.Add(documentId);
                        }
                    }
                }

                if (excludeDocuments is not null)
                {
                    documentSet.ExceptWith(excludeDocuments);

                    excludeDocuments.Dispose();
                }
            }

            var facetResults = EmptyFacetResults;

            if (query.Facets.Count > 0)
            {
                facetResults = new List<FieldFacetResult>(capacity: query.Facets.Count);

                foreach (var facetExpression in query.Facets)
                {
                    var field = facetExpression.Field;
                    var facetDocuments = documentSet.GetMatches(field);

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
                            facetResults.Add(facetResult);
                        }
                    }
                }
            }

            var documentResult = (IDocumentSet)documentSet;

            if (query.Sort is SortExpression sortExpression)
            {
                var field = sortExpression.Field;

                if (!this.fieldStores.TryGetValue(field, out var store))
                {
                    throw new InvalidOperationException("Sort expression references field not belonging to current index.");
                }

                documentResult = store.Sort(sortExpression, documentSet);
            }

            return new QueryResult<TDocument>(documentResult, facetResults, this.documents);
        }

        public void Dispose()
        {
            this.tokenEncoder.Dispose();
            this.documents.Dispose();
            this.allDocuments.Dispose();

            foreach (var pair in this.fieldStores)
            {
                pair.Value.Dispose();
            }

            this.fieldStores.Clear();
        }

        internal override bool HasField(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            return this.fieldStores.ContainsKey(field);
        }

        private sealed class FilterExpressionComparer : IComparer<FilterExpression>
        {
            private readonly HashSet<Field> facetFields;
            private readonly Dictionary<Field, FieldStore> fieldStores;

            public FilterExpressionComparer(HashSet<Field> facetFields, Dictionary<Field, FieldStore> fieldStores)
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
