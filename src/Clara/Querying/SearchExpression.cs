using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class SearchExpression
    {
        internal SearchExpression(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (!field.IsSearchable)
            {
                throw new ArgumentException("Field searching is not enabled.", nameof(field));
            }

            this.Field = field;
        }

        public Field Field { get; }

        internal abstract bool IsEmpty { get; }
    }
}
