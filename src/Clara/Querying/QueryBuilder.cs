using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class QueryBuilder : IDisposable
    {
        private readonly Query query;
        private bool isDisposed;

        public QueryBuilder(Index index)
        {
            if (index is null)
            {
                throw new ArgumentNullException(nameof(index));
            }

            this.query = new Query(index);
        }

        public Query ToQuery()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            this.isDisposed = true;

            return this.query;
        }

        public QueryBuilder Search(TextField field, string? text, SearchMode mode = SearchMode.All)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.Search is not null)
            {
                throw new InvalidOperationException("Search already has been set.");
            }

            this.query.Search = new SearchExpression(field, text ?? string.Empty, mode);

            return this;
        }

        public QueryBuilder Filter(KeywordField field, ValuesExpression valuesExpression)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (valuesExpression is null)
            {
                throw new ArgumentNullException(nameof(valuesExpression));
            }

            if (this.query.IsFacetAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new KeywordFilterExpression(field, valuesExpression));

            return this;
        }

        public QueryBuilder Filter(HierarchyField field, ValuesExpression valuesExpression)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (valuesExpression is null)
            {
                throw new ArgumentNullException(nameof(valuesExpression));
            }

            if (this.query.IsFacetAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new HierarchyFilterExpression(field, valuesExpression));

            return this;
        }

        public QueryBuilder Filter<TValue>(RangeField<TValue> field, TValue? from, TValue? to)
            where TValue : struct, IComparable<TValue>
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFacetAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new RangeFilterExpression<TValue>(field, from, to));

            return this;
        }

        public QueryBuilder Facet(KeywordField field)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFacetAdded(field))
            {
                throw new InvalidOperationException("Facet for field already has been added.");
            }

            this.query.AddFacet(new KeywordFacetExpression(field));

            return this;
        }

        public QueryBuilder Facet(HierarchyField field)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFacetAdded(field))
            {
                throw new InvalidOperationException("Facet for field already has been added.");
            }

            this.query.AddFacet(new HierarchyFacetExpression(field));

            return this;
        }

        public QueryBuilder Facet<TValue>(RangeField<TValue> field)
            where TValue : struct, IComparable<TValue>
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFacetAdded(field))
            {
                throw new InvalidOperationException("Facet for field already has been added.");
            }

            this.query.AddFacet(new RangeFacetExpression<TValue>(field));

            return this;
        }

        public QueryBuilder Sort<TValue>(RangeField<TValue> field, SortDirection direction = SortDirection.Ascending)
            where TValue : struct, IComparable<TValue>
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.Sort is not null)
            {
                throw new InvalidOperationException("Sort already has been set.");
            }

            this.query.Sort = new RangeSortExpression<TValue>(field, direction);

            return this;
        }

        public QueryBuilder Include(IEnumerable<string?>? includeDocuments)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

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
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

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

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.query.Dispose();

                this.isDisposed = true;
            }
        }
    }
}
