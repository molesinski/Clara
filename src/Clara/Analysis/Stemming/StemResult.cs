using System;
using System.Collections;
using System.Collections.Generic;

namespace Clara.Analysis.Stemming
{
    public readonly struct StemResult : IEnumerable<string>
    {
        private readonly string? stem;
        private readonly List<string>? stems;

        public StemResult(string stem)
        {
            if (stem is null)
            {
                throw new ArgumentNullException(nameof(stem));
            }

            this.stem = stem;
            this.stems = null;
        }

        public StemResult(List<string> stems)
        {
            if (stems is null)
            {
                throw new ArgumentNullException(nameof(stems));
            }

            this.stem = null;
            this.stems = stems;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<string>
        {
            private readonly string? stem;
            private readonly List<string>? stems;
            private readonly int count;
            private int index;
            private string current;

            public Enumerator(StemResult result)
            {
                this.stem = result.stem;
                this.stems = result.stems;
                this.count =
                    this.stem is not null ? 1 :
                    this.stems is not null ? this.stems.Count :
                    0;

                this.index = 0;
                this.current = default!;
            }

            public string Current
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

            public bool MoveNext()
            {
                if (this.index < this.count)
                {
                    if (this.stem is not null)
                    {
                        this.current = this.stem;
                        this.index++;

                        return true;
                    }
                    else if (this.stems is not null)
                    {
                        this.current = this.stems[this.index];
                        this.index++;

                        return true;
                    }
                }

                this.index = this.count + 1;
                this.current = default!;

                return false;
            }

            public void Reset()
            {
                this.index = 0;
                this.current = default!;
            }

            public void Dispose()
            {
            }
        }
    }
}
