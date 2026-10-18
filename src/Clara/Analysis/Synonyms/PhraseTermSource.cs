using System.Collections;

namespace Clara.Analysis.Synonyms
{
    internal sealed class PhraseTermSource : IPhraseTermSource, IEnumerable<PhraseTerm>, IEnumerator<PhraseTerm>
    {
        private readonly ITokenTermSource tokenTermSource;
        private string text = string.Empty;
        private PhraseTerm current;
        private IEnumerator<TokenTerm>? enumerator;
        private bool isEnumerated;

        public PhraseTermSource(ITokenTermSource tokenTermSource)
        {
            if (tokenTermSource is null)
            {
                throw new ArgumentNullException(nameof(tokenTermSource));
            }

            this.tokenTermSource = tokenTermSource;
        }

        PhraseTerm IEnumerator<PhraseTerm>.Current
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

        public IEnumerable<PhraseTerm> GetTerms(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            this.text = text;

            ((IEnumerator)this).Reset();

            return this;
        }

        IEnumerator<PhraseTerm> IEnumerable<PhraseTerm>.GetEnumerator()
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
                    this.current = new PhraseTerm(this.enumerator.Current.Token, this.enumerator.Current.Position);

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
