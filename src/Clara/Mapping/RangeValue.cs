using System;
using System.Collections;
using System.Collections.Generic;

namespace Clara.Mapping
{
    public readonly struct RangeValue<TValue> : IEnumerable<TValue>
        where TValue : struct, IComparable<TValue>
    {
        private readonly TValue? value;
        private readonly IEnumerable<TValue>? values;

        public RangeValue(TValue? value)
        {
            this.value = value;
            this.values = default;
        }

        public RangeValue(IEnumerable<TValue>? values)
        {
            this.value = default;
            this.values = values;
        }

        public static implicit operator RangeValue<TValue>(TValue? value)
        {
            return new RangeValue<TValue>(value);
        }

        public static implicit operator RangeValue<TValue>(TValue[]? values)
        {
            return new RangeValue<TValue>(values);
        }

        public static implicit operator RangeValue<TValue>(List<TValue>? values)
        {
            return new RangeValue<TValue>(values);
        }

        public static implicit operator RangeValue<TValue>(HashSet<TValue>? values)
        {
            return new RangeValue<TValue>(values);
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public struct Enumerator : IEnumerator<TValue>
        {
            private readonly TValue? value;
            private readonly IReadOnlyList<TValue>? listValues;
            private readonly IEnumerable<TValue>? enumerableValues;
            private bool isEnumerated;
            private int index;
            private IEnumerator<TValue>? enumerator;
            private TValue current;

            public Enumerator(RangeValue<TValue> source)
            {
                this.value = default;
                this.listValues = default;
                this.enumerableValues = default;

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

                this.isEnumerated = false;
                this.index = 0;
                this.enumerator = default;
                this.current = default;
            }

            public TValue Current
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
