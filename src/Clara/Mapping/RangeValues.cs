﻿using System.Collections;

namespace Clara.Mapping
{
    internal readonly struct RangeValues<TValue> : IEnumerable<TValue>
        where TValue : struct, IComparable<TValue>
    {
        private readonly TValue? value;
        private readonly IEnumerable<TValue>? values;

        public RangeValues(TValue? value)
        {
            this.value = value;
            this.values = default;
        }

        public RangeValues(IEnumerable<TValue>? values)
        {
            this.value = default;
            this.values = values;
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
            private bool isEnumerated;
            private int index;
            private IEnumerator<TValue>? enumerator;
            private TValue current;

            internal Enumerator(RangeValues<TValue> source)
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