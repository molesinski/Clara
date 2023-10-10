using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class Query : IDisposable
    {
        private readonly Index index;
        private readonly ObjectPoolLease<ListSlim<FilterExpression>> filters;
        private readonly ObjectPoolLease<ListSlim<FacetExpression>> facets;
        private SearchExpression? search;
        private SortExpression? sort;
        private IEnumerable<string?>? includeDocuments;
        private IEnumerable<string?>? excludeDocuments;
        private bool isDisposed;

        public Query(Index index)
        {
            if (index is null)
            {
                throw new ArgumentNullException(nameof(index));
            }

            this.index = index;
            this.filters = SharedObjectPools.FilterExpressions.Lease();
            this.facets = SharedObjectPools.FacetExpressions.Lease();
        }

        public SearchExpression? Search
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.search;
            }

            set
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                if (value is null)
                {
                    this.search = null;

                    return;
                }

                if (!this.index.ContainsField(value.Field))
                {
                    throw new InvalidOperationException("Search expression references field not belonging to current index.");
                }

                this.search = value;
            }
        }

        public IReadOnlyList<FilterExpression> Filters
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.filters.Instance;
            }
        }

        public IReadOnlyList<FacetExpression> Facets
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.facets.Instance;
            }
        }

        public SortExpression? Sort
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.sort;
            }

            set
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                if (value is null)
                {
                    this.sort = null;

                    return;
                }

                if (!this.index.ContainsField(value.Field))
                {
                    throw new InvalidOperationException("Sort expression references field not belonging to current index.");
                }

                this.sort = value;
            }
        }

        public IEnumerable<string?>? IncludeDocuments
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.includeDocuments;
            }

            set
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                this.includeDocuments = value;
            }
        }

        public IEnumerable<string?>? ExcludeDocuments
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.excludeDocuments;
            }

            set
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                this.excludeDocuments = value;
            }
        }

        internal Index Index
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.index;
            }
        }

        public void AddFilter(FilterExpression filterExpression)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (filterExpression is null)
            {
                throw new ArgumentNullException(nameof(filterExpression));
            }

            if (!this.index.ContainsField(filterExpression.Field))
            {
                throw new InvalidOperationException("Filter expression references field not belonging to current index.");
            }

            var filters = this.filters.Instance;

            for (var i = 0; i < filters.Count; i++)
            {
                if (filters[i].Field == filterExpression.Field)
                {
                    filters[i] = filterExpression;

                    return;
                }
            }

            filters.Add(filterExpression);
        }

        public void AddFacet(FacetExpression facetExpression)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            if (facetExpression is null)
            {
                throw new ArgumentNullException(nameof(facetExpression));
            }

            if (!this.index.ContainsField(facetExpression.Field))
            {
                throw new InvalidOperationException("Facet expression references field not belonging to current index.");
            }

            var facets = this.facets.Instance;

            for (var i = 0; i < facets.Count; i++)
            {
                if (facets[i].Field == facetExpression.Field)
                {
                    facets[i] = facetExpression;

                    return;
                }
            }

            facets.Add(facetExpression);
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                foreach (var filter in this.filters.Instance)
                {
                    filter.Dispose();
                }

                this.facets.Dispose();
                this.filters.Dispose();

                this.isDisposed = true;
            }
        }

        internal bool IsFilterAdded(Field field)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            foreach (var filterExpression in this.filters.Instance)
            {
                if (filterExpression.Field == field)
                {
                    return true;
                }
            }

            return false;
        }

        internal bool IsFacetAdded(Field field)
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            foreach (var facetExpression in this.facets.Instance)
            {
                if (facetExpression.Field == field)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
