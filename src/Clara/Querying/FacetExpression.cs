using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class FacetExpression
    {
        internal FacetExpression(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (!field.IsFacetable)
            {
                throw new ArgumentException("Field faceting is not enabled.", nameof(field));
            }

            this.Field = field;
        }

        public Field Field { get; }
    }
}
