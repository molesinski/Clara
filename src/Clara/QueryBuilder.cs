using System;
using System.Collections.Generic;
using Clara.Mapping;
using Clara.Querying;

namespace Clara
{
    public class QueryBuilder
    {
        private readonly Query query;

        public QueryBuilder(Index index)
        {
            if (index is null)
            {
                throw new ArgumentNullException(nameof(index));
            }

            this.query = new Query(index);
        }

        public Query Query
        {
            get
            {
                return this.query;
            }
        }

        public QueryBuilder Filter(TextField field, MatchExpression matchExpression)
        {
            this.query.AddFilter(new TextFilterExpression(field, matchExpression));

            return this;
        }

        public QueryBuilder Filter(KeywordField field, MatchExpression matchExpression)
        {
            this.query.AddFilter(new KeywordFilterExpression(field, matchExpression));

            return this;
        }

        public QueryBuilder Filter(HierarchyField field, MatchExpression matchExpression)
        {
            this.query.AddFilter(new HierarchyFilterExpression(field, matchExpression));

            return this;
        }

        public QueryBuilder Filter<TValue>(RangeField<TValue> field, TValue? from, TValue? to)
            where TValue : struct, IComparable<TValue>
        {
            this.query.AddFilter(new RangeFilterExpression<TValue>(field, from, to));

            return this;
        }

        public QueryBuilder Facet(KeywordField field)
        {
            this.query.AddFacet(new KeywordFacetExpression(field));

            return this;
        }

        public QueryBuilder Facet(HierarchyField field)
        {
            this.query.AddFacet(new HierarchyFacetExpression(field));

            return this;
        }

        public QueryBuilder Facet<TValue>(RangeField<TValue> field)
            where TValue : struct, IComparable<TValue>
        {
            this.query.AddFacet(new RangeFacetExpression<TValue>(field));

            return this;
        }

        public QueryBuilder Sort<TValue>(RangeField<TValue> field, SortDirection direction)
            where TValue : struct, IComparable<TValue>
        {
            if (this.query.Sort is not null)
            {
                throw new InvalidOperationException("Sort expression already has been set.");
            }

            this.query.Sort = new RangeSortExpression<TValue>(field, direction);

            return this;
        }

        public QueryBuilder Include(IEnumerable<string?>? includeDocuments)
        {
            if (includeDocuments is not null)
            {
                if (this.query.IncludeDocuments is not null)
                {
                    throw new InvalidOperationException("Include documents already have been set.");
                }
            }

            this.query.IncludeDocuments = includeDocuments;

            return this;
        }

        public QueryBuilder Exclude(IEnumerable<string?>? excludeDocuments)
        {
            if (excludeDocuments is not null)
            {
                if (this.query.ExcludeDocuments is not null)
                {
                    throw new InvalidOperationException("Exclude documents already have been set.");
                }
            }

            this.query.ExcludeDocuments = excludeDocuments;

            return this;
        }
    }
}
