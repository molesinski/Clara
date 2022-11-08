using System;

namespace Clara.Storage
{
    internal abstract class FieldStoreBuilder : IDisposable
    {
        protected internal FieldStoreBuilder()
        {
        }

        public abstract FieldStore Build();

        public abstract void Dispose();
    }

    internal abstract class FieldStoreBuilder<TSource> : FieldStoreBuilder
    {
        protected internal FieldStoreBuilder()
        {
        }

        public abstract void Index(int documentId, TSource item);
    }
}
