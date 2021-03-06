using System.Collections;
using System.Collections.Generic;

namespace Clara.Mapping
{
    public readonly struct TokenValue : IEnumerable<string>
    {
        private readonly string? value;
        private readonly IEnumerable<string>? values;

        public TokenValue(string? value)
        {
            this.value = value;
            this.values = default;
        }

        public TokenValue(IEnumerable<string>? values)
        {
            this.value = default;
            this.values = values;
        }

        public static implicit operator TokenValue(string? value)
        {
            return new TokenValue(value);
        }

        public static implicit operator TokenValue(string[]? values)
        {
            return new TokenValue(values);
        }

        public static implicit operator TokenValue(List<string>? values)
        {
            return new TokenValue(values);
        }

        public static implicit operator TokenValue(HashSet<string>? values)
        {
            return new TokenValue(values);
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
            private readonly string? value;
            private readonly IReadOnlyList<string>? listValues;
            private readonly IEnumerable<string>? enumerableValues;
            private bool isEnumerated;
            private int index;
            private IEnumerator<string>? enumerator;
            private string? current;

            public Enumerator(TokenValue source)
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

            public string Current
            {
                get
                {
                    return this.current!;
                }
            }

            object IEnumerator.Current
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
