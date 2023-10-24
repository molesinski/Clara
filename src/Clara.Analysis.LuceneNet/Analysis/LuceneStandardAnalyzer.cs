using System.Collections;
using Lucene.Net.Util;

namespace Clara.Analysis
{
    public sealed class LuceneStandardAnalyzer : IAnalyzer
    {
        public ITokenizer Tokenizer
        {
            get
            {
                return LuceneStandardTokenizer.Instance;
            }
        }

        public ITokenTermSource CreateTokenTermSource()
        {
            return new TokenTermSource();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "By design")]
        private sealed class TokenTermSource : ITokenTermSource, IEnumerable<TokenTerm>, IEnumerator<TokenTerm>
        {
            private readonly ReusableStringReader reader;
            private readonly Lucene.Net.Analysis.Analyzer analyzer;
            private readonly char[] chars;
            private TokenTerm current;
            private TokenTermStreamEnumerable.Enumerator enumerator;
            private bool isEnumeratorSet;

            public TokenTermSource()
            {
                this.reader = new ReusableStringReader();
                this.analyzer = new Lucene.Net.Analysis.Standard.StandardAnalyzer(LuceneVersion.LUCENE_48);
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
                    this.enumerator = new TokenTermStreamEnumerable(this.analyzer.GetTokenStream(string.Empty, this.reader), this.chars).GetEnumerator();
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
