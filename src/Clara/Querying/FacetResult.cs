using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class FacetResult
    {
        protected internal FacetResult(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            this.Field = field;
        }

        public Field Field { get; }
    }
}
