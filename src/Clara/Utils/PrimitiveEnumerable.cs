using System.Collections;

namespace Clara.Utils
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Value type used for performance optimization")]
    public readonly struct PrimitiveEnumerable<TValue> : IEnumerable<TValue>
        where TValue : struct
    {
        private readonly TValue? value;
        private readonly IEnumerable<TValue>? values;
        private readonly IEnumerable<TValue?>? nullableValues;

        public PrimitiveEnumerable(TValue? value)
        {
            this.value = value;
            this.values = default;
            this.nullableValues = default;
        }

        public PrimitiveEnumerable(IEnumerable<TValue>? values)
        {
            this.value = default;
            this.values = values;
            this.nullableValues = default;
        }

        public PrimitiveEnumerable(IEnumerable<TValue?>? nullableValues)
        {
            this.value = default;
            this.values = default;
            this.nullableValues = nullableValues;
        }

        public readonly Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        readonly IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        readonly IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public struct Enumerator : IEnumerator<TValue>
        {
            private readonly TValue? value;
            private readonly IReadOnlyList<TValue>? listValues;
            private readonly IEnumerable<TValue>? enumerableValues;
            private readonly IReadOnlyList<TValue?>? listNullableValues;
            private readonly IEnumerable<TValue?>? enumerableNullableValues;
            private bool isEnumerated;
            private int index;
            private IEnumerator<TValue>? enumerator;
            private IEnumerator<TValue?>? nullableEnumerator;
            private TValue current;

            internal Enumerator(PrimitiveEnumerable<TValue> source)
            {
                this.value = default;
                this.listValues = default;
                this.enumerableValues = default;
                this.listNullableValues = default;
                this.enumerableNullableValues = default;

                if (source.value is not null)
                {
                    this.value = source.value;
                }
                else if (source.values is IReadOnlyList<TValue> listValues)
                {
                    this.listValues = listValues;
                }
                else if (source.values is not null)
                {
                    this.enumerableValues = source.values;
                }
                else if (source.nullableValues is IReadOnlyList<TValue?> listNullableValues)
                {
                    this.listNullableValues = listNullableValues;
                }
                else if (source.nullableValues is not null)
                {
                    this.enumerableNullableValues = source.nullableValues;
                }

                this.isEnumerated = false;
                this.index = 0;
                this.enumerator = default;
                this.nullableEnumerator = default;
                this.current = default;
            }

            public readonly TValue Current
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
                        this.current = this.value.Value;

                        return true;
                    }
                    else if (this.listValues is not null)
                    {
                        while (this.index < this.listValues.Count)
                        {
                            this.current = this.listValues[this.index];
                            this.index++;

                            return true;
                        }
                    }
                    else if (this.enumerableValues is not null)
                    {
                        this.enumerator ??= this.enumerableValues.GetEnumerator();

                        while (this.enumerator.MoveNext())
                        {
                            this.current = this.enumerator.Current;

                            return true;
                        }
                    }
                    else if (this.listNullableValues is not null)
                    {
                        while (this.index < this.listNullableValues.Count)
                        {
                            var value = this.listNullableValues[this.index];

                            this.index++;

                            if (value is not null)
                            {
                                this.current = value.Value;

                                return true;
                            }
                        }
                    }
                    else if (this.enumerableNullableValues is not null)
                    {
                        this.nullableEnumerator ??= this.enumerableNullableValues.GetEnumerator();

                        while (this.nullableEnumerator.MoveNext())
                        {
                            var value = this.nullableEnumerator.Current;

                            if (value is not null)
                            {
                                this.current = value.Value;

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
                this.nullableEnumerator?.Dispose();
                this.nullableEnumerator = default;
                this.current = default;
            }

            public void Dispose()
            {
                this.enumerator?.Dispose();
                this.enumerator = default;
                this.nullableEnumerator?.Dispose();
                this.nullableEnumerator = default;
                this.current = default;
            }
        }
    }
}
