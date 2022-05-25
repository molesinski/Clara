namespace Clara.Storage
{
    internal abstract class FieldStoreBuilder
    {
        protected internal FieldStoreBuilder()
        {
        }

        public abstract FieldStore Build();
    }

    internal abstract class FieldStoreBuilder<TSource> : FieldStoreBuilder
    {
        protected internal FieldStoreBuilder()
        {
        }

        public abstract void Index(int documentId, TSource item);
    }
}
