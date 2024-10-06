using System.Collections;
using Clara.Utils;

namespace Clara.Analysis
{
    public readonly struct PhraseGroup : IReadOnlyList<Phrase>, IEquatable<PhraseGroup>
    {
        private readonly ListSlim<Phrase> phrases;

        public PhraseGroup(IEnumerable<Phrase> phrases)
        {
            if (phrases is null)
            {
                throw new ArgumentNullException(nameof(phrases));
            }

            this.phrases = new ListSlim<Phrase>(phrases);
        }

        internal PhraseGroup(ListSlim<Phrase> phrases)
        {
            if (phrases is null)
            {
                throw new ArgumentNullException(nameof(phrases));
            }

            this.phrases = phrases;
        }

        public int Count
        {
            get
            {
                return this.phrases.Count;
            }
        }

        public Phrase this[int index]
        {
            get
            {
                return this.phrases[index];
            }
        }

        public static bool operator ==(PhraseGroup left, PhraseGroup right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PhraseGroup left, PhraseGroup right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            return obj is PhraseGroup other && this == other;
        }

        public bool Equals(PhraseGroup other)
        {
            if (this.phrases.Count != other.phrases.Count)
            {
                return false;
            }

            for (var i = 0; i < this.phrases.Count; i++)
            {
                if (!(this.phrases[i] == other.phrases[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            var hash = default(HashCode);

            hash.Add(this.phrases.Count);

            foreach (var phrase in this.phrases)
            {
                hash.Add(phrase);
            }

            return hash.ToHashCode();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this.phrases.GetEnumerator());
        }

        IEnumerator<Phrase> IEnumerable<Phrase>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<Phrase>
        {
            private ListSlim<Phrase>.Enumerator enumerator;

            internal Enumerator(ListSlim<Phrase>.Enumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            public readonly Phrase Current
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
