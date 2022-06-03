using System;
using System.Collections.Generic;
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

                if (token.IsEmpty)
                {
                    continue;
                }

                this.charTermAttribute.SetEmpty();
                this.charTermAttribute.Append(token.ToString());

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
