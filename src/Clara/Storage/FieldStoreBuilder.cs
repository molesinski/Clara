namespace Clara.Storage
{
    internal abstract class FieldStoreBuilder
    {
        internal FieldStoreBuilder()
        {
        }

        public abstract FieldStore Build();
    }

    internal abstract class FieldStoreBuilder<TSource> : FieldStoreBuilder
    {
        internal FieldStoreBuilder()
        {
        }

        public abstract void Index(int documentId, TSource item);
    }
}
