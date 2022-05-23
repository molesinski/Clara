using System;
using System.Collections.Generic;

namespace Clara.Querying
{
    public sealed class Query
    {
        private readonly Index index;
        private readonly List<FilterExpression> filters = new();
        private readonly List<FacetExpression> facets = new();
        private readonly List<SortExpression> sort = new();

        public Query(Index index)
        {
            if (index is null)
            {
                throw new ArgumentNullException(nameof(index));
            }

            this.index = index;
        }

        public IEnumerable<FilterExpression> Filters
        {
            get
            {
                return this.filters;
            }
        }

        public IEnumerable<FacetExpression> Facets
        {
            get
            {
                return this.facets;
            }
        }

        public IEnumerable<SortExpression> Sort
        {
            get
            {
                return this.sort;
            }
        }

        public IEnumerable<string?>? IncludeDocuments { get; set; }

        public IEnumerable<string?>? ExcludeDocuments { get; set; }

        public void AddFilter(FilterExpression filterExpression)
        {
            if (filterExpression is null)
            {
                throw new ArgumentNullException(nameof(filterExpression));
            }

            if (!this.index.HasField(filterExpression.Field))
            {
                throw new InvalidOperationException("Filter expression references field not belonging to current index.");
            }

            foreach (var item in this.filters)
            {
                if (item.Field == filterExpression.Field)
                {
                    throw new InvalidOperationException("Filter for given field has already been added.");
                }
            }

            this.filters.Add(filterExpression);
        }

        public void AddFacet(FacetExpression facetExpression)
        {
            if (facetExpression is null)
            {
                throw new ArgumentNullException(nameof(facetExpression));
            }

            if (!this.index.HasField(facetExpression.Field))
            {
                throw new InvalidOperationException("Facet expression references field not belonging to current index.");
            }

            foreach (var item in this.facets)
            {
                if (item.Field == facetExpression.Field)
                {
                    throw new InvalidOperationException("Facet for given field has already been added.");
                }
            }

            this.facets.Add(facetExpression);
        }

        public void AddSort(SortExpression sortExpression)
        {
            if (sortExpression is null)
            {
                throw new ArgumentNullException(nameof(sortExpression));
            }

            if (!this.index.HasField(sortExpression.Field))
            {
                throw new InvalidOperationException("Sort expression references field not belonging to current index.");
            }

            foreach (var item in this.sort)
            {
                if (item.Field == sortExpression.Field)
                {
                    throw new InvalidOperationException("Sort for given field has already been added.");
                }
            }

            this.sort.Add(sortExpression);
        }
    }
}
