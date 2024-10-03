using System.Collections;

namespace Clara.Analysis.Synonyms
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1214:Readonly fields should appear before non-readonly fields", Justification = "By design")]
    internal sealed class SynonymTermSource : ISynonymTermSource, IEnumerable<SynonymTerm>, IEnumerator<SynonymTerm>
    {
        private readonly ITokenTermSource tokenTermSource;
        private string text = string.Empty;
        private SynonymTerm current;
        private IEnumerator<TokenTerm>? enumerator;
        private bool isEnumerated;

        public SynonymTermSource(ITokenTermSource tokenTermSource)
        {
            if (tokenTermSource is null)
            {
                throw new ArgumentNullException(nameof(tokenTermSource));
            }

            this.tokenTermSource = tokenTermSource;
        }

        SynonymTerm IEnumerator<SynonymTerm>.Current
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

        public IEnumerable<SynonymTerm> GetTerms(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.text = text;

            ((IEnumerator)this).Reset();

            return this;
        }

        IEnumerator<SynonymTerm> IEnumerable<SynonymTerm>.GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        bool IEnumerator.MoveNext()
        {
            if (!this.isEnumerated)
            {
                this.enumerator ??= this.tokenTermSource.GetTerms(this.text).GetEnumerator();

                if (this.enumerator.MoveNext())
                {
                    this.current = new SynonymTerm(this.enumerator.Current.Token, this.enumerator.Current.Position);

                    return true;
                }
            }

            this.isEnumerated = true;
            this.current = default;

            return false;
        }

        void IEnumerator.Reset()
        {
            this.current = default;
            this.enumerator?.Dispose();
            this.enumerator = default;
            this.isEnumerated = default;
        }

        void IDisposable.Dispose()
        {
            ((IEnumerator)this).Reset();

            this.text = string.Empty;
        }
    }
}
