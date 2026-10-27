using Clara.Mapping;

namespace Clara.Storage
{
    internal abstract class FieldStoreBuilder
    {
        public virtual bool RequiresFieldStores
        {
            get
            {
                return false;
            }
        }

        public abstract FieldStore Build(TokenEncoder tokenEncoder);

        public virtual FieldStore Build(TokenEncoder tokenEncoder, IReadOnlyDictionary<Field, FieldStore> fieldStores)
        {
            return this.Build(tokenEncoder);
        }
    }

    internal abstract class FieldStoreBuilder<TSource> : FieldStoreBuilder
    {
        public abstract void Index(int documentId, TSource item);
    }
}
