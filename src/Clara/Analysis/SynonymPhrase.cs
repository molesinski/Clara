using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "By design")]
    public readonly record struct SynonymPhrase : IReadOnlyCollection<string>
    {
        private readonly ListSlim<string> terms;

        public SynonymPhrase(ListSlim<string> terms)
        {
            if (terms is null)
            {
                throw new ArgumentNullException(nameof(terms));
            }

            this.terms = terms;
        }

        public int Count
        {
            get
            {
                return this.terms.Count;
            }
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this.terms.GetEnumerator());
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<string>
        {
            private ListSlim<string>.Enumerator enumerator;

            internal Enumerator(ListSlim<string>.Enumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            public readonly string Current
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
