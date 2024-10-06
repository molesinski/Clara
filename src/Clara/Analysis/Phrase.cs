using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "By design")]
    public readonly struct Phrase : IReadOnlyList<string>, IEquatable<Phrase>
    {
        private readonly ListSlim<string> terms;

        public Phrase(IEnumerable<string> terms)
        {
            if (terms is null)
            {
                throw new ArgumentNullException(nameof(terms));
            }

            this.terms = new ListSlim<string>(terms);
        }

        internal Phrase(ListSlim<string> terms)
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

        public string this[int index]
        {
            get
            {
                return this.terms[index];
            }
        }

        public static bool operator ==(Phrase left, Phrase right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Phrase left, Phrase right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is Phrase other && this == other;
        }

        public bool Equals(Phrase other)
        {
            if (this.terms.Count != other.terms.Count)
            {
                return false;
            }

            for (var i = 0; i < this.terms.Count; i++)
            {
                if (!StringComparer.Ordinal.Equals(this.terms[i], other.terms[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hash = default(HashCode);

            hash.Add(this.terms.Count);

            foreach (var term in this.terms)
            {
                hash.Add(term, StringComparer.OrdinalIgnoreCase);
            }

            return hash.ToHashCode();
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
