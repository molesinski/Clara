using System.Collections;
using Clara.Utils;

namespace Clara.Querying
{
    public sealed class FilterValueCollection : IReadOnlyCollection<string>, IDisposable
    {
        private readonly ObjectPoolSlimLease<HashSetSlim<string>> lease;
        private bool isDisposed;

        internal FilterValueCollection(string? value)
        {
            this.lease = SharedObjectPools.FilterValues.Lease();

            foreach (var item in new StringEnumerable(value, trim: true))
            {
                this.lease.Instance.Add(item);
            }
        }

        internal FilterValueCollection(IEnumerable<string?>? values)
        {
            this.lease = SharedObjectPools.FilterValues.Lease();

            foreach (var item in new StringEnumerable(values, trim: true))
            {
                this.lease.Instance.Add(item);
            }
        }

        public int Count
        {
            get
            {
                this.ThrowIfDisposed();

                return this.lease.Instance.Count;
            }
        }

        public Enumerator GetEnumerator()
        {
            this.ThrowIfDisposed();

            return new Enumerator(this.lease.Instance.GetEnumerator());
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.lease.Dispose();

                this.isDisposed = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        public struct Enumerator : IEnumerator<string>
        {
            private HashSetSlim<string>.Enumerator enumerator;

            internal Enumerator(HashSetSlim<string>.Enumerator enumerator)
            {
                this.enumerator = enumerator;
            }

            public readonly string Current
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
