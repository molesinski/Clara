using Clara.Mapping;
using Clara.Utils;

namespace Clara.Querying
{
    internal readonly struct TextSearchFields : IDisposable
    {
        private static readonly ListSlim<TextSearchField> Empty = new();

        private readonly ObjectPoolLease<ListSlim<TextSearchField>>? lease;

        public TextSearchFields(TextField field)
        {
            this.lease = CreateLease(new ObjectEnumerable<TextField>(field));
        }

        public TextSearchFields(IEnumerable<TextField> fields)
        {
            this.lease = CreateLease(new ObjectEnumerable<TextField>(fields));
        }

        public TextSearchFields(IEnumerable<TextSearchField> fields)
        {
            this.lease = CreateLease(new PrimitiveEnumerable<TextSearchField>(fields));
        }

        public readonly ListSlim<TextSearchField> Value
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

        private static ObjectPoolLease<ListSlim<TextSearchField>>? CreateLease(ObjectEnumerable<TextField> fields)
        {
            var lease = default(ObjectPoolLease<ListSlim<TextSearchField>>?);

            foreach (var value in fields)
            {
                lease ??= SharedObjectPools.TextSearchFields.Lease();

                for (var i = 0; i < lease.Value.Instance.Count; i++)
                {
                    if (ReferenceEquals(lease.Value.Instance[i].Field, value))
                    {
                        throw new ArgumentException("Duplicate field detected.", nameof(fields));
                    }
                }

                lease.Value.Instance.Add(new TextSearchField(value));
            }

            return lease;
        }

        private static ObjectPoolLease<ListSlim<TextSearchField>>? CreateLease(PrimitiveEnumerable<TextSearchField> fields)
        {
            var lease = default(ObjectPoolLease<ListSlim<TextSearchField>>?);

            foreach (var value in fields)
            {
                lease ??= SharedObjectPools.TextSearchFields.Lease();

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
