using System.Collections;

namespace Clara.Utils
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Value type used for performance optimization")]
    public readonly struct StringEnumerable : IEnumerable<string>
    {
        private readonly string? value;
        private readonly IEnumerable<string?>? values;
        private readonly bool trim;

        public StringEnumerable(string? value, bool trim = false)
        {
            this.value = value;
            this.values = default;
            this.trim = trim;
        }

        public StringEnumerable(IEnumerable<string?>? values, bool trim = false)
        {
            this.value = default;
            this.values = values;
            this.trim = trim;
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
            private readonly bool trim;
            private bool isEnumerated;
            private int index;
            private IEnumerator<string?>? enumerator;
            private string? current;

            internal Enumerator(StringEnumerable source)
            {
                this.value = default;
                this.listValues = default;
                this.enumerableValues = default;
                this.trim = source.trim;

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

                        var value = this.value;

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            this.current = this.trim ? value.Trim() : value;

                            return true;
                        }
                    }
                    else if (this.listValues is not null)
                    {
                        while (this.index < this.listValues.Count)
                        {
                            var value = this.listValues[this.index];

                            this.index++;

                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                this.current = this.trim ? value.Trim() : value;

                                return true;
                            }
                        }
                    }
                    else if (this.enumerableValues is not null)
                    {
                        this.enumerator ??= this.enumerableValues.GetEnumerator();

                        while (this.enumerator.MoveNext())
                        {
                            var value = this.enumerator.Current;

                            if (!string.IsNullOrWhiteSpace(value))
                            {
#if NET6_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                                this.current = this.trim ? value.Trim() : value;
#else
                                this.current = this.trim ? value!.Trim() : value;
#endif

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
