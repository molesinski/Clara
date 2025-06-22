using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFilterExpression : FilterExpression
    {
        private readonly FilterValueCollection values;
        private bool isDisposed;

        public KeywordFilterExpression(KeywordField field, FilterMode filterMode, string? value)
            : base(field)
        {
            if (filterMode != FilterMode.All && filterMode != FilterMode.Any)
            {
                throw new ArgumentException("Illegal filter mode enum value.", nameof(filterMode));
            }

            this.FilterMode = filterMode;
            this.values = new FilterValueCollection(value);
        }

        public KeywordFilterExpression(KeywordField field, FilterMode filterMode, IEnumerable<string?>? values)
            : base(field)
        {
            if (filterMode != FilterMode.All && filterMode != FilterMode.Any)
            {
                throw new ArgumentException("Illegal filter mode enum value.", nameof(filterMode));
            }

            this.FilterMode = filterMode;
            this.values = new FilterValueCollection(values);
        }

        public FilterMode FilterMode { get; }

        public FilterValueCollection Values
        {
            get
            {
                this.ThrowIfDisposed();

                return this.values;
            }
        }

        internal override bool IsEmpty
        {
            get
            {
                return this.Values.Count == 0;
            }
        }

        internal override bool HasPersistedFacets
        {
            get
            {
                return this.FilterMode == FilterMode.Any;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                this.values.Dispose();

                this.isDisposed = true;
            }

            base.Dispose(disposing);
        }

        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }
    }
}
