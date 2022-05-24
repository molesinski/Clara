using System;
using System.Collections.Generic;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    internal sealed class EnumerableTokenStream : TokenStream
    {
        private readonly ICharTermAttribute charTermAttribute;
        private readonly IEnumerable<string> tokens;
        private IEnumerator<string>? enumerator;

        public EnumerableTokenStream(IEnumerable<string> tokens)
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

                if (token is null)
                {
                    continue;
                }

                this.charTermAttribute.SetEmpty();
                this.charTermAttribute.Append(token);

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
