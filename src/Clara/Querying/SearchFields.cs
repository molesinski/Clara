using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    internal readonly struct SearchFields : IDisposable
    {
        private static readonly ListSlim<SearchField> Empty = new();

        private readonly ObjectPoolLease<ListSlim<SearchField>>? lease;

        public SearchFields(TextField field)
        {
            this.lease = CreateLease(new ObjectEnumerable<TextField>(field));
        }

        public SearchFields(IEnumerable<TextField> fields)
        {
            this.lease = CreateLease(new ObjectEnumerable<TextField>(fields));
        }

        public SearchFields(IEnumerable<SearchField> fields)
        {
            this.lease = CreateLease(new PrimitiveEnumerable<SearchField>(fields));
        }

        public readonly ListSlim<SearchField> Value
        {
            get
            {
                return this.lease?.Instance ?? Empty;
            }
        }

        public void Dispose()
        {
            this.lease?.Dispose();
        }

        private static ObjectPoolLease<ListSlim<SearchField>>? CreateLease(ObjectEnumerable<TextField> fields)
        {
            var lease = default(ObjectPoolLease<ListSlim<SearchField>>?);

            foreach (var value in fields)
            {
                lease ??= SharedObjectPools.SearchFields.Lease();

                for (var i = 0; i < lease.Value.Instance.Count; i++)
                {
                    if (ReferenceEquals(lease.Value.Instance[i].Field, value))
                    {
                        throw new ArgumentException("Duplicate field detected.", nameof(fields));
                    }
                }

                lease.Value.Instance.Add(new SearchField(value));
            }

            return lease;
        }

        private static ObjectPoolLease<ListSlim<SearchField>>? CreateLease(PrimitiveEnumerable<SearchField> fields)
        {
            var lease = default(ObjectPoolLease<ListSlim<SearchField>>?);

            foreach (var value in fields)
            {
                lease ??= SharedObjectPools.SearchFields.Lease();

                for (var i = 0; i < lease.Value.Instance.Count; i++)
                {
                    if (ReferenceEquals(lease.Value.Instance[i].Field, value.Field))
                    {
                        throw new ArgumentException("Duplicate field detected.", nameof(fields));
                    }
                }

                lease.Value.Instance.Add(value);
            }

            return lease;
        }
    }
}
