using System;
using System.Collections.Generic;
using Clara.Mapping;
using Clara.Querying;

namespace Clara
{
    public class QueryBuilder
    {
        public QueryBuilder(Index index)
        {
            if (index is null)
            {
                throw new ArgumentNullException(nameof(index));
            }

            this.Query = new Query(index);
        }

        public Query Query { get; }

        public QueryBuilder Filter(TextField field, MatchExpression matchExpression)
        {
            this.Query.AddFilter(new TextFilterExpression(field, matchExpression));

            return this;
        }

        public QueryBuilder Filter(KeywordField field, MatchExpression matchExpression)
        {
            this.Query.AddFilter(new KeywordFilterExpression(field, matchExpression));

            return this;
        }

        public QueryBuilder Filter(HierarchyField field, MatchExpression matchExpression)
        {
            this.Query.AddFilter(new HierarchyFilterExpression(field, matchExpression));

            return this;
        }

        public QueryBuilder Filter<TValue>(RangeField<TValue> field, TValue? from, TValue? to)
            where TValue : struct, IComparable<TValue>
        {
            this.Query.AddFilter(new RangeFilterExpression<TValue>(field, from, to));

            return this;
        }

        public QueryBuilder Facet(KeywordField field, IComparer<KeywordValue>? comparer = null)
        {
            this.Query.AddFacet(new KeywordFacetExpression(field, comparer));

            return this;
        }

        public QueryBuilder Facet(HierarchyField field, IComparer<HierarchyValue>? comparer = null)
        {
            this.Query.AddFacet(new HierarchyFacetExpression(field, comparer));

            return this;
        }

        public QueryBuilder Facet<TValue>(RangeField<TValue> field)
            where TValue : struct, IComparable<TValue>
        {
            this.Query.AddFacet(new RangeFacetExpression<TValue>(field));

            return this;
        }

        public QueryBuilder Sort<TValue>(RangeField<TValue> field, SortDirection direction)
            where TValue : struct, IComparable<TValue>
        {
            this.Query.AddSort(new RangeSortExpression<TValue>(field, direction));

            return this;
        }

        public QueryBuilder Include(IEnumerable<string> includeDocuments)
        {
            if (includeDocuments is not null)
            {
                if (this.Query.IncludeDocuments is not null)
                {
                    throw new InvalidOperationException("Include documents already have been set.");
                }
            }

            this.Query.IncludeDocuments = includeDocuments;

            return this;
        }

        public QueryBuilder Exclude(IEnumerable<string> excludeDocuments)
        {
            if (excludeDocuments is not null)
            {
                if (this.Query.ExcludeDocuments is not null)
                {
                    throw new InvalidOperationException("Exclude documents already have been set.");
                }
            }

            this.Query.ExcludeDocuments = excludeDocuments;

            return this;
        }
    }
}
