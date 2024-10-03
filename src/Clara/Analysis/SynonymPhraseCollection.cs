using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    public readonly record struct SynonymPhraseCollection : IReadOnlyCollection<SynonymPhrase>
    {
        private readonly ListSlim<SynonymPhrase> phrases;

        public SynonymPhraseCollection(ListSlim<SynonymPhrase> phrases)
        {
            this.phrases = phrases;
        }

        public int Count
        {
            get
            {
                return this.phrases.Count;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this.phrases.GetEnumerator());
        }

        IEnumerator<SynonymPhrase> IEnumerable<SynonymPhrase>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<SynonymPhrase>
        {
            private ListSlim<SynonymPhrase>.Enumerator enumerator;

            internal Enumerator(ListSlim<SynonymPhrase>.Enumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            public readonly SynonymPhrase Current
            {
                get
                {
                    return this.enumerator.Current;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.enumerator.Current;
                }
            }

            public bool MoveNext()
            {
                return this.enumerator.MoveNext();
            }

            public void Reset()
            {
                this.enumerator.Reset();
            }

            public void Dispose()
            {
                this.enumerator.Dispose();
            }
        }
    }
}
