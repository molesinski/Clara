using System;
using System.Collections.Generic;

namespace Clara.Querying
{
    public readonly record struct HierarchyFacetValue
    {
        public HierarchyFacetValue(string value, int count, IEnumerable<HierarchyFacetValue> children)
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

        public HierarchyFacetValue(string keyword, int count)
            : this(keyword, count, Array.Empty<HierarchyFacetValue>())
        {
        }

        public string Value { get; }

        public int Count { get; }

        public IEnumerable<HierarchyFacetValue> Children { get; }
    }
}
