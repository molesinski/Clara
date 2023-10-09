namespace Clara.Storage
{
    internal abstract class FieldStoreBuilder
    {
        public abstract FieldStore Build(TokenEncoder tokenEncoder);
    }

    internal abstract class FieldStoreBuilder<TSource> : FieldStoreBuilder
    {
        public abstract void Index(int documentId, TSource item);
    }
}
