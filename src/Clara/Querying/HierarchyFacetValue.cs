﻿using Clara.Utils;

namespace Clara.Querying
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not intended to be used directly for comparison")]
    public readonly struct HierarchyFacetValue
    {
        private static readonly HierarchyFacetValueChildrenCollection EmptyChildren = new(new ListSlim<HierarchyFacetValue>(), 0, 0);

        public HierarchyFacetValue(string value, int count)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Value = value;
            this.Count = count;
            this.Children = EmptyChildren;
        }

        internal HierarchyFacetValue(string value, int count, HierarchyFacetValueChildrenCollection children)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (children is null)
            {
                throw new ArgumentNullException(nameof(children));
            }

            this.Value = value;
            this.Count = count;
            this.Children = children;
        }

        public string Value { get; }

        public int Count { get; }

        public HierarchyFacetValueChildrenCollection Children { get; }
    }
}
