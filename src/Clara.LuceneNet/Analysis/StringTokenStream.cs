using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    internal sealed class StringTokenStream : TokenStream
    {
        private readonly ICharTermAttribute charTermAttribute;
        private string value;
        private bool isReset;
        private bool isIncremented;

        public StringTokenStream()
        {
            this.charTermAttribute = this.AddAttribute<ICharTermAttribute>();
            this.value = default!;
        }

        public void SetString(string value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.value = value;
            this.isReset = false;
            this.isIncremented = false;
        }

        public override void Reset()
        {
            base.Reset();

            this.isReset = true;
            this.isIncremented = false;
        }

        public override bool IncrementToken()
        {
            if (!this.isReset)
            {
                throw new InvalidOperationException("Token stream must be reset before incrementing first token.");
            }

            if (!this.isIncremented)
            {
                this.isIncremented = true;

                this.charTermAttribute.SetEmpty();
                this.charTermAttribute.Append(this.value);

                return true;
            }

            return false;
        }

        public override void End()
        {
        }
    }
}
