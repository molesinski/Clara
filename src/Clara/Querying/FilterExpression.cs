using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class FilterExpression : IDisposable
    {
        internal FilterExpression(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            if (!field.IsFilterable)
            {
                throw new ArgumentException("Field filtering is not enabled.", nameof(field));
            }

            this.Field = field;
        }

        public Field Field { get; }

        internal abstract bool IsEmpty { get; }

        internal abstract bool HasPersistedFacets { get; }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
