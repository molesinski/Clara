using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
    public sealed class SingleTokenStream : TokenStream
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix
    {
        private readonly ICharTermAttribute charTermAttribute;
        private Token token;
        private bool isReset;
        private bool isIncremented;

        public SingleTokenStream()
        {
            this.charTermAttribute = this.AddAttribute<ICharTermAttribute>();
            this.token = default!;
        }

        public void SetToken(Token token)
        {
            this.token = token;
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

                var length = this.token.Length;

                if (length == 0)
                {
                    return false;
                }

                if (length > this.charTermAttribute.Buffer.Length)
                {
                    this.charTermAttribute.ResizeBuffer(length);
                }

                this.charTermAttribute.SetEmpty();
                this.token.Span.CopyTo(this.charTermAttribute.Buffer.AsSpan());
                this.charTermAttribute.Length = length;

                return true;
            }

            return false;
        }

        public override void End()
        {
        }
    }
}
