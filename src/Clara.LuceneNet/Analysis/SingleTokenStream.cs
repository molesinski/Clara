using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.TokenAttributes;

namespace Clara.Analysis
{
    public sealed class SingleTokenStream : TokenStream
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

                this.charTermAttribute.SetEmpty();

                if (!this.token.IsReadOnly)
                {
                    this.charTermAttribute.Append(this.token.Chars, this.token.Index, this.token.Length);
                }
                else
                {
                    this.charTermAttribute.Append(this.token.ToString());
                }

                return true;
            }

            return false;
        }

        public override void End()
        {
        }
    }
}
