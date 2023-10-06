using System.Collections;

namespace Clara.Analysis
{
    public sealed class DisposableEnumerable<TValue> : IDisposableEnumerable<TValue>
    {
        private readonly IEnumerable<TValue> enumerable;

        public DisposableEnumerable(IEnumerable<TValue> enumerable)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            this.enumerable = enumerable;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "By design")]
        public static IDisposableEnumerable<TValue> Empty { get; } = new EmptyDisposableEnumerable();

        public IEnumerator<TValue> GetEnumerator()
        {
            return this.enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.enumerable.GetEnumerator();
        }

        public void Dispose()
        {
        }

        private sealed class EmptyDisposableEnumerable : IDisposableEnumerable<TValue>
        {
            private readonly Enumerator enumerator;

            public EmptyDisposableEnumerable()
            {
                this.enumerator = new Enumerator();
            }

            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
            {
                return this.enumerator;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.enumerator;
            }

            void IDisposable.Dispose()
            {
                this.enumerator.Dispose();
            }

            public sealed class Enumerator : IEnumerator<TValue>
            {
                public TValue Current
                {
                    get
                    {
                        return default!;
                    }
                }

                object IEnumerator.Current
                {
                    get
                    {
                        return default!;
                    }
                }

                public bool MoveNext()
                {
                    return false;
                }

                public void Reset()
                {
                }

                public void Dispose()
                {
                }
            }
        }
    }
}
