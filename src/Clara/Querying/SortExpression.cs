using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class SortExpression
    {
        protected SortExpression(Field field, SortDirection sortDirection)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (!field.IsSortable)
            {
                throw new ArgumentException("Sorting is not enabled.", nameof(field));
            }

            if (sortDirection != SortDirection.Ascending && sortDirection != SortDirection.Descending)
            {
                throw new ArgumentException("Illegal sort direction enum value.", nameof(sortDirection));
            }

            this.Field = field;
            this.SortDirection = sortDirection;
        }

        public Field Field { get; }

        public SortDirection SortDirection { get; }
    }
}
