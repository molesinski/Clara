using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    public sealed class EnumerableTokenStream : TokenStream
    {
        private readonly ICharTermAttribute charTermAttribute;
        private readonly IEnumerable<Token> tokens;
        private IEnumerator<Token>? enumerator;

        public EnumerableTokenStream(IEnumerable<Token> tokens)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            this.charTermAttribute = this.AddAttribute<ICharTermAttribute>();
            this.tokens = tokens;
            this.enumerator = null;
        }

        public override void Reset()
        {
            base.Reset();

            this.enumerator = this.tokens.GetEnumerator();
        }

        public override bool IncrementToken()
        {
            if (this.enumerator is null)
            {
                throw new InvalidOperationException("Token stream must be reset before incrementing first token.");
            }

            while (this.enumerator.MoveNext())
            {
                var token = this.enumerator.Current;
                var length = token.Length;

                if (length == 0)
                {
                    continue;
                }

                if (length > this.charTermAttribute.Buffer.Length)
                {
                    this.charTermAttribute.ResizeBuffer(length);
                }

                this.charTermAttribute.SetEmpty();
                token.Span.CopyTo(this.charTermAttribute.Buffer.AsSpan());
                this.charTermAttribute.Length = length;

                return true;
            }

            return false;
        }

        public override void End()
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.enumerator?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
