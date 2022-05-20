using System;

namespace Clara.Mapping
{
    internal sealed class HierarchyFieldValue : FieldValue
    {
        public HierarchyFieldValue(HierarchyField field, string[] keywords)
            : base(field)
        {
            if (keywords is null)
            {
                throw new ArgumentNullException(nameof(keywords));
            }

            this.Keywords = keywords;
        }

        public string[] Keywords { get; }
    }
}
