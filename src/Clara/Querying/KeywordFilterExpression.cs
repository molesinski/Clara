﻿using Clara.Mapping;

namespace Clara.Querying
{
    public sealed class KeywordFilterExpression : FilterExpression
    {
        private readonly FilterValues values;
        private bool isDisposed;

        public KeywordFilterExpression(KeywordField field, FilterMode filterMode, string? value)
            : base(field)
        {
            if (filterMode != FilterMode.All && filterMode != FilterMode.Any)
            {
                throw new ArgumentException("Illegal filter mode enum value.", nameof(filterMode));
            }

            this.FilterMode = filterMode;
            this.values = new FilterValues(value);
        }

        public KeywordFilterExpression(KeywordField field, FilterMode filterMode, IEnumerable<string?>? values)
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
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().Name);
                }

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
    }
}
