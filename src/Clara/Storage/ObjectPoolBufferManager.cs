using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Clara.Storage
{
    public sealed class ObjectPoolBufferManager : BufferManager
    {
        private readonly ConcurrentBag<HashSet<int>> documentSets = new();
        private readonly int maximumCount;

        public ObjectPoolBufferManager()
            : this(maximumCount: 32)
        {
        }

        public ObjectPoolBufferManager(int maximumCount)
        {
            if (maximumCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumCount));
            }

            this.maximumCount = maximumCount;
        }

        internal override BufferScope CreateScope()
        {
            return new ObjectPoolBufferScope(this);
        }

        private HashSet<int> CreateDocumentSet()
        {
            if (this.documentSets.TryTake(out var documentSet))
            {
                return documentSet;
            }

            return new HashSet<int>();
        }

        private HashSet<int> CreateDocumentSet(IReadOnlyCollection<int> collection)
        {
            if (this.documentSets.TryTake(out var documentSet))
            {
                foreach (var item in collection)
                {
                    documentSet.Add(item);
                }

                return documentSet;
            }

#if NETSTANDARD2_1_OR_GREATER || NET472_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            if (collection is not HashSet<int>)
            {
                documentSet = new HashSet<int>(capacity: collection.Count);

                foreach (var item in collection)
                {
                    documentSet.Add(item);
                }

                return documentSet;
            }
#endif

            return new HashSet<int>(collection);
        }

        private void Return(HashSet<int> set)
        {
            if (this.documentSets.Count < this.maximumCount)
            {
                set.Clear();

                this.documentSets.Add(set);
            }
        }

        private sealed class ObjectPoolBufferScope : BufferScope
        {
            private readonly ObjectPoolBufferManager bufferManager;
            private readonly List<HashSet<int>> documentSets = new();
            private bool disposed;

            public ObjectPoolBufferScope(ObjectPoolBufferManager bufferManager)
            {
                this.bufferManager = bufferManager;
            }

            public override HashSet<int> CreateDocumentSet()
            {
                var documentSet = this.bufferManager.CreateDocumentSet();

                this.documentSets.Add(documentSet);

                return documentSet;
            }

            public override HashSet<int> CreateDocumentSet(IReadOnlyCollection<int> collection)
            {
                var documentSet = this.bufferManager.CreateDocumentSet(collection);

                this.documentSets.Add(documentSet);

                return documentSet;
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    if (!this.disposed)
                    {
                        foreach (var documentSet in this.documentSets)
                        {
                            this.bufferManager.Return(documentSet);
                        }

                        this.disposed = true;
                    }
                }
            }
        }
    }
}
