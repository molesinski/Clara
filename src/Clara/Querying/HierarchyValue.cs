using System;
using System.Collections.Generic;

namespace Clara.Querying
{
    public readonly struct HierarchyValue
    {
        public HierarchyValue(string value, int count, IEnumerable<HierarchyValue> children)
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

        public HierarchyValue(string keyword, int count)
            : this(keyword, count, Array.Empty<HierarchyValue>())
        {
        }

        public string Value { get; }

        public int Count { get; }

        public IEnumerable<HierarchyValue> Children { get; }
    }
}
