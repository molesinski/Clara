using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class FilterExpression
    {
        protected internal FilterExpression(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (!field.IsFilterable)
            {
                throw new ArgumentException("Filtering is not enabled for given field.", nameof(field));
            }

            this.Field = field;
        }

        public Field Field { get; }

        public abstract bool IsEmpty { get; }

        internal abstract bool IsBranchingRequiredForFaceting { get; }
    }
}
