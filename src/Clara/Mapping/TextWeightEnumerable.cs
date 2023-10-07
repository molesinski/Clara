using System.Collections;

namespace Clara.Mapping
{
    internal readonly struct TextWeightEnumerable : IEnumerable<TextWeight>
    {
        private readonly string? value;
        private readonly IEnumerable<string?>? values;
        private readonly IEnumerable<TextWeight>? structValues;

        public TextWeightEnumerable(string? value)
        {
            this.value = value;
            this.values = default;
            this.structValues = default;
        }

        public TextWeightEnumerable(IEnumerable<string?>? values)
        {
            this.value = default;
            this.values = values;
            this.structValues = default;
        }

        public TextWeightEnumerable(IEnumerable<TextWeight>? values)
        {
            this.value = default;
            this.values = default;
            this.structValues = values;
        }

        public readonly Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        readonly IEnumerator<TextWeight> IEnumerable<TextWeight>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<TextWeight>
        {
            private readonly string? value;
            private readonly IReadOnlyList<string>? listValues;
            private readonly IEnumerable<string?>? enumerableValues;
            private readonly IReadOnlyList<TextWeight>? structListValues;
            private readonly IEnumerable<TextWeight>? structEnumerableValues;
            private bool isEnumerated;
            private int index;
            private IEnumerator<string?>? enumerator;
            private IEnumerator<TextWeight>? structEnumerator;
            private TextWeight current;

            internal Enumerator(TextWeightEnumerable source)
            {
                this.value = default;
                this.listValues = default;
                this.enumerableValues = default;
                this.structListValues = default;
                this.structEnumerableValues = default;

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
                else if (source.structValues is IReadOnlyList<TextWeight> structListValues)
                {
                    this.structListValues = structListValues;
                }
                else if (source.structValues is not null)
                {
                    this.structEnumerableValues = source.structValues;
                }

                this.isEnumerated = false;
                this.index = 0;
                this.enumerator = default;
                this.structEnumerator = default;
                this.current = default;
            }

            public readonly TextWeight Current
            {
                get
                {
                    return this.current;
                }
            }

            readonly object IEnumerator.Current
            {
                get
                {
                    return this.current;
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
                            this.current = new TextWeight(value);

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
                                this.current = new TextWeight(value);

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
                                this.current = new TextWeight(value);
#else
                                this.current = new TextWeight(value!);
#endif

                                return true;
                            }
                        }
                    }
                    else if (this.structListValues is not null)
                    {
                        while (this.index < this.structListValues.Count)
                        {
                            this.current = this.structListValues[this.index];
                            this.index++;

                            return true;
                        }
                    }
                    else if (this.structEnumerableValues is not null)
                    {
                        this.structEnumerator ??= this.structEnumerableValues.GetEnumerator();

                        while (this.structEnumerator.MoveNext())
                        {
                            this.current = this.structEnumerator.Current;

                            return true;
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
                this.structEnumerator?.Dispose();
                this.structEnumerator = default;
                this.current = default;
            }

            public void Dispose()
            {
                this.Reset();
            }
        }
    }
}
