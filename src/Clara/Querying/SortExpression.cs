using System;
using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class SortExpression
    {
        protected SortExpression(Field field, SortDirection direction)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (!field.IsSortable)
            {
                throw new ArgumentException("Sorting is not enabled for given field.", nameof(field));
            }

            this.Field = field;
            this.Direction = direction;
        }

        public Field Field { get; }

        public SortDirection Direction { get; }
    }
}
