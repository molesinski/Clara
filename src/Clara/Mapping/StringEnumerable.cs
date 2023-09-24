using System.Collections;

namespace Clara.Mapping
{
    internal readonly record struct StringEnumerable : IEnumerable<string>
    {
        private readonly string? value;
        private readonly IEnumerable<string?>? values;

        public StringEnumerable(string? value)
        {
            this.value = value;
            this.values = default;
        }

        public StringEnumerable(IEnumerable<string?>? values)
        {
            this.value = default;
            this.values = values;
        }

        public readonly Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        readonly IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<string>
        {
            private readonly string? value;
            private readonly IReadOnlyList<string>? listValues;
            private readonly IEnumerable<string?>? enumerableValues;
            private bool isEnumerated;
            private int index;
            private IEnumerator<string?>? enumerator;
            private string? current;

            internal Enumerator(StringEnumerable source)
            {
                this.value = default;
                this.listValues = default;
                this.enumerableValues = default;

                if (source.value is not null)
                {
                    this.value = source.value;
                }
                else if (source.values is IReadOnlyList<string> listValues)
                {
                    this.listValues = listValues;
                }
                else if (source.values is not null)
                {
                    this.enumerableValues = source.values;
                }

                this.isEnumerated = false;
                this.index = 0;
                this.enumerator = default;
                this.current = default;
            }

            public readonly string Current
            {
                get
                {
                    return this.current!;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.current!;
                }
            }

            public bool MoveNext()
            {
                if (!this.isEnumerated)
                {
                    if (this.value is not null)
                    {
                        this.isEnumerated = true;
                        this.current = this.value;

                        return true;
                    }
                    else if (this.listValues is not null)
                    {
                        while (this.index < this.listValues.Count)
                        {
                            this.current = this.listValues[this.index];
                            this.index++;

                            if (this.current is not null)
                            {
                                return true;
                            }
                        }
                    }
                    else if (this.enumerableValues is not null)
                    {
                        this.enumerator ??= this.enumerableValues.GetEnumerator();

                        while (this.enumerator.MoveNext())
                        {
                            this.current = this.enumerator.Current;

                            if (this.current is not null)
                            {
                                return true;
                            }
                        }
                    }
                }

                this.isEnumerated = true;
                this.current = default;

                return false;
            }

            public void Reset()
            {
                this.isEnumerated = false;
                this.index = 0;
                this.enumerator?.Dispose();
                this.enumerator = default;
                this.current = default;
            }

            public void Dispose()
            {
                this.enumerator?.Dispose();
                this.enumerator = default;
                this.current = default;
            }
        }
    }
}
