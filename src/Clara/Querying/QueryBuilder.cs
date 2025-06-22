using Clara.Analysis;
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
            this.ThrowIfDisposed();

            this.isDisposed = true;

            return this.query;
        }

        public QueryBuilder Search(TextField field, SearchMode searchMode, string? text, Func<Position, float>? positionBoost = null)
        {
            this.ThrowIfDisposed();

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.Search is not null)
            {
                throw new InvalidOperationException("Search already has been set.");
            }

            this.query.Search = new TextScoringSearchExpression(field, searchMode, text ?? string.Empty, positionBoost);

            return this;
        }

        public QueryBuilder Filter(KeywordField field, FilterMode filterMode, string? value)
        {
            this.ThrowIfDisposed();

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFilterAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new KeywordFilterExpression(field, filterMode, value));

            return this;
        }

        public QueryBuilder Filter(KeywordField field, FilterMode filterMode, params string?[] values)
        {
            this.ThrowIfDisposed();

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFilterAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new KeywordFilterExpression(field, filterMode, values));

            return this;
        }

        public QueryBuilder Filter(KeywordField field, FilterMode filterMode, IEnumerable<string?>? values)
        {
            this.ThrowIfDisposed();

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFilterAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new KeywordFilterExpression(field, filterMode, values));

            return this;
        }

        public QueryBuilder Filter(HierarchyField field, FilterMode filterMode, string? value)
        {
            this.ThrowIfDisposed();

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFilterAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new HierarchyFilterExpression(field, filterMode, value));

            return this;
        }

        public QueryBuilder Filter(HierarchyField field, FilterMode filterMode, params string?[] values)
        {
            this.ThrowIfDisposed();

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFilterAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new HierarchyFilterExpression(field, filterMode, values));

            return this;
        }

        public QueryBuilder Filter(HierarchyField field, FilterMode filterMode, IEnumerable<string?>? values)
        {
            this.ThrowIfDisposed();

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFilterAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new HierarchyFilterExpression(field, filterMode, values));

            return this;
        }

        public QueryBuilder Filter<TValue>(RangeField<TValue> field, TValue? from, TValue? to)
            where TValue : struct, IComparable<TValue>
        {
            this.ThrowIfDisposed();

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.IsFilterAdded(field))
            {
                throw new InvalidOperationException("Filter for field already has been added.");
            }

            this.query.AddFilter(new RangeFilterExpression<TValue>(field, from, to));

            return this;
        }

        public QueryBuilder Facet(KeywordField field)
        {
            this.ThrowIfDisposed();

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
            this.ThrowIfDisposed();

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
            this.ThrowIfDisposed();

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

        public QueryBuilder Sort<TValue>(RangeField<TValue> field, SortDirection sortDirection)
            where TValue : struct, IComparable<TValue>
        {
            this.ThrowIfDisposed();

            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (this.query.Sort is not null)
            {
                throw new InvalidOperationException("Sort already has been set.");
            }

            this.query.Sort = new RangeSortExpression<TValue>(field, sortDirection);

            return this;
        }

        public QueryBuilder Include(IEnumerable<string?>? includeDocuments)
        {
            this.ThrowIfDisposed();

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
            this.ThrowIfDisposed();

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

        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
