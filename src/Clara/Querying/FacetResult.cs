using Clara.Mapping;

namespace Clara.Querying
{
    public abstract class FacetResult : IDisposable
    {
        internal FacetResult(Field field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            this.Field = field;
        }

        public Field Field { get; }

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
