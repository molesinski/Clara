using System;
using System.Collections.Generic;
using System.Linq;
using Clara.Collections;
using Clara.Mapping;
using Clara.Querying;
using Clara.Storage;

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
        private readonly TokenEncoder tokenEncoder;
        private readonly PooledDictionary<int, TDocument> documents;
        private readonly Dictionary<Field, FieldStore> fieldStores;
        private readonly PooledSet<int> allDocuments;

        internal Index(
            TokenEncoder tokenEncoder,
            PooledDictionary<int, TDocument> documents,
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
            this.allDocuments = new PooledSet<int>(capacity: documents.Count);

            foreach (var pair in documents)
            {
                this.allDocuments.Add(pair.Key);
            }
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
                var hasValues = false;
                var includedDocuments = new PooledSet<int>();

                foreach (var includedDocument in query.IncludeDocuments)
                {
                    if (includedDocument is not null)
                    {
                        hasValues = true;

                        if (this.tokenEncoder.TryEncode(includedDocument, out var documentId))
                        {
                            includedDocuments.Add(documentId);
                        }
                    }
                }

                if (hasValues)
                {
                    documentSet = new DocumentSet(includedDocuments);
                }
            }

            if (documentSet == default)
            {
                documentSet = new DocumentSet((IReadOnlyCollection<int>)this.allDocuments);
            }

            var facetFields = new HashSet<Field>();

            foreach (var facet in query.Facets)
            {
                facetFields.Add(facet.Field);
            }

            foreach (var filterExpression in query.Filters
                .Where(o => !o.IsEmpty)
                .OrderBy(o => o.IsBranchingRequiredForFaceting && facetFields.Contains(o.Field) ? 1 : 0)
                .ThenBy(o => this.fieldStores.TryGetValue(o.Field, out var store) ? store.FilterOrder : double.MinValue))
            {
                if (documentSet.Documents.Count == 0)
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

            if (query.ExcludeDocuments is not null)
            {
                var hasValues = false;
                using var excludeDocuments = new PooledSet<int>();

                foreach (var excludeDocument in query.ExcludeDocuments)
                {
                    if (excludeDocument is not null)
                    {
                        hasValues = true;

                        if (this.tokenEncoder.TryEncode(excludeDocument, out var documentId))
                        {
                            excludeDocuments.Add(documentId);
                        }
                    }
                }

                if (hasValues)
                {
                    documentSet.ExceptWith(excludeDocuments);
                }
            }

            var facetResults = new List<FieldFacetResult>(capacity: query.Facets.Count);
            var filterExpressions = new List<FilterExpression>(capacity: query.Filters.Count);

            foreach (var facetExpression in query.Facets)
            {
                filterExpressions.Clear();

                var field = facetExpression.Field;
                var facetDocuments = documentSet.GetMatches(field);

                if (facetDocuments.Count > 0)
                {
                    if (!this.fieldStores.TryGetValue(field, out var store))
                    {
                        throw new InvalidOperationException("Facet expression references field not belonging to current index.");
                    }

                    foreach (var filterExpression in query.Filters)
                    {
                        if (filterExpression.Field == field)
                        {
                            filterExpressions.Add(filterExpression);
                        }
                    }

                    var facetResult = store.Facet(facetExpression, filterExpressions, facetDocuments);

                    if (facetResult is not null)
                    {
                        facetResults.Add(facetResult);
                    }
                }
            }

            var documentSort = new DocumentSort(documentSet);

            foreach (var sortExpression in query.Sort)
            {
                var field = sortExpression.Field;

                if (!this.fieldStores.TryGetValue(field, out var store))
                {
                    throw new InvalidOperationException("Sort expression references field not belonging to current index.");
                }

                store.Sort(sortExpression, documentSort);
            }

            return new QueryResult<TDocument>(this.documents, documentSort, facetResults);
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
    }
}
