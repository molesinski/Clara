using System;
using System.Collections.Generic;

namespace Clara.Storage
{
    internal abstract class BufferScope : IDisposable
    {
        public abstract HashSet<int> CreateDocumentSet();

        public abstract HashSet<int> CreateDocumentSet(IReadOnlyCollection<int> collection);

        public void Dispose()
        {
            this.Dispose(disposing: true);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
