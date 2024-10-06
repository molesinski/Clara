using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class HierarchyFilterExpression : FilterExpression
    {
        private readonly FilterValues values;
        private bool isDisposed;

        public HierarchyFilterExpression(HierarchyField field, FilterMode filterMode, string? value)
            : base(field)
        {
            if (filterMode != FilterMode.All && filterMode != FilterMode.Any)
            {
                throw new ArgumentException("Illegal filter mode enum value.", nameof(filterMode));
            }

            this.FilterMode = filterMode;
            this.values = new FilterValues(value);
        }

        public HierarchyFilterExpression(HierarchyField field, FilterMode filterMode, IEnumerable<string?>? values)
            : base(field)
        {
            if (filterMode != FilterMode.All && filterMode != FilterMode.Any)
            {
                throw new ArgumentException("Illegal filter mode enum value.", nameof(filterMode));
            }

            this.FilterMode = filterMode;
            this.values = new FilterValues(values);
        }

        public FilterMode FilterMode { get; }

        public IReadOnlyCollection<string> Values
        {
            get
            {
                this.ThrowIfDisposed();

                return this.values.Value;
            }
        }

        internal override bool IsEmpty
        {
            get
            {
                return this.Values.Count == 0;
            }
        }

        internal override bool IsBranchingRequiredForFaceting
        {
            get
            {
                return false;
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
