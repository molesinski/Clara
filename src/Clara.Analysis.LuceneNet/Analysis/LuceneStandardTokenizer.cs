using System.Collections;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneStandardTokenizer : ITokenizer
    {
        internal static LuceneStandardTokenizer Instance { get; } = new();

        public ITokenTermSource CreateTokenTermSource()
        {
            return new TokenTermSource();
        }

        public bool Equals(ITokenizer? other)
        {
            return other is LuceneStandardTokenizer;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "By design")]
        private sealed class TokenTermSource : ITokenTermSource, IEnumerable<TokenTerm>, IEnumerator<TokenTerm>
        {
            private readonly ReusableStringReader reader;
            private readonly Tokenizer tokenizer;
            private readonly char[] chars;
            private TokenTerm current;
            private TokenTermStreamEnumerable.Enumerator enumerator;
            private bool isEnumeratorSet;

            public TokenTermSource()
            {
                this.reader = new ReusableStringReader();
                this.tokenizer = new StandardTokenizer(LuceneVersion.LUCENE_48, this.reader);
                this.chars = new char[Token.MaximumLength];
            }

            TokenTerm IEnumerator<TokenTerm>.Current
            {
                get
                {
                    return this.current;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.current;
                }
            }

            public IEnumerable<TokenTerm> GetTerms(string text)
            {
                if (text is null)
                {
                    throw new ArgumentNullException(nameof(text));
                }

                this.reader.SetText(text);
                this.tokenizer.SetReader(this.reader);

                ((IEnumerator)this).Reset();

                return this;
            }

            IEnumerator<TokenTerm> IEnumerable<TokenTerm>.GetEnumerator()
            {
                return this;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this;
            }

            bool IEnumerator.MoveNext()
            {
                if (!this.isEnumeratorSet)
                {
                    this.enumerator = new TokenTermStreamEnumerable(this.tokenizer, this.chars).GetEnumerator();
                    this.isEnumeratorSet = true;
                }

                if (this.enumerator.MoveNext())
                {
                    this.current = this.enumerator.Current;

                    return true;
                }

                this.current = default;

                return false;
            }

            void IEnumerator.Reset()
            {
                this.current = default;

                if (this.isEnumeratorSet)
                {
                    this.enumerator.Dispose();
                    this.enumerator = default;
                    this.isEnumeratorSet = default;
                }
            }

            void IDisposable.Dispose()
            {
                ((IEnumerator)this).Reset();
            }
        }
    }
}
