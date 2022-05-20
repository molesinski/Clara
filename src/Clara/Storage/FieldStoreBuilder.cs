using Clara.Mapping;

namespace Clara.Storage
{
    internal abstract class FieldStoreBuilder
    {
        public abstract void Index(int documentId, FieldValue fieldValue);

        public abstract FieldStore Build();
    }
}
