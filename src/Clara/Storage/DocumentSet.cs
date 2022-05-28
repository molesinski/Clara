using System;
using System.Collections;
using System.Collections.Generic;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class DocumentSet : IDocumentSet
    {
        private static readonly PooledHashSetSlim<int> Empty = new();

        private readonly Dictionary<Field, BranchSet> branches = new();
        private readonly IReadOnlyCollection<int> allDocuments;
        private PooledHashSetSlim<int>? documents;

        public DocumentSet(IReadOnlyCollection<int> allDocuments)
        {
            if (allDocuments is null)
            {
                throw new ArgumentNullException(nameof(allDocuments));
            }

            this.allDocuments = allDocuments;
            this.documents = null;
        }

        public DocumentSet(PooledHashSetSlim<int> documents)
        {
            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            this.allDocuments = Empty;
            this.documents = documents;
        }

        public int Count
        {
            get
            {
                return (this.documents ?? this.allDocuments).Count;
            }
        }

        public IReadOnlyCollection<int> GetMatches(Field field)
        {
            if (this.branches.TryGetValue(field, out var facetDocuments))
            {
                return facetDocuments.Documents;
            }

            return this.documents ?? this.allDocuments;
        }

        public void Clear()
        {
            if (this.documents is null)
            {
                this.documents = Empty;
            }
            else
            {
                if (this.documents is not null)
                {
                    this.documents.Dispose();
                    this.documents = Empty;
                }
            }

            foreach (var pair in this.branches)
            {
                pair.Value.Dispose();
            }

            this.branches.Clear();
        }

        public void Branch(Field field)
        {
            if (this.documents is null)
            {
                this.branches.Add(field, new BranchSet(this.allDocuments));
            }
            else
            {
                this.branches.Add(field, new BranchSet(new PooledHashSetSlim<int>(this.documents)));
            }
        }

        public void IntersectWith(Field field, PooledHashSetSlim<int> documents)
        {
            if (this.documents is null)
            {
                this.documents = documents;
            }
            else
            {
                this.documents.IntersectWith(documents);

                foreach (var pair in this.branches)
                {
                    if (pair.Key != field)
                    {
                        pair.Value.IntersectWith(documents);
                    }
                }

                documents.Dispose();
            }
        }

        public void IntersectWith(Field field, IEnumerable<int> documents)
        {
            if (this.documents is null)
            {
                this.documents = new PooledHashSetSlim<int>(documents);
            }
            else
            {
                this.documents.IntersectWith(documents);

                foreach (var pair in this.branches)
                {
                    if (pair.Key != field)
                    {
                        pair.Value.IntersectWith(documents);
                    }
                }
            }
        }

        public void ExceptWith(IEnumerable<int> documents)
        {
            if (this.documents is null)
            {
                this.documents = new PooledHashSetSlim<int>(this.allDocuments);
            }

            this.documents.ExceptWith(documents);

            foreach (var pair in this.branches)
            {
                pair.Value.ExceptWith(documents);
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return (this.documents ?? this.allDocuments).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this.documents ?? this.allDocuments).GetEnumerator();
        }

        public void Dispose()
        {
            if (this.documents is not null)
            {
                this.documents.Dispose();
                this.documents = null;
            }

            foreach (var pair in this.branches)
            {
                pair.Value.Dispose();
            }

            this.branches.Clear();
        }

        private class BranchSet : IDisposable
        {
            private readonly IReadOnlyCollection<int> allDocuments;
            private PooledHashSetSlim<int>? documents;

            public BranchSet(IReadOnlyCollection<int> allDocuments)
            {
                if (allDocuments is null)
                {
                    throw new ArgumentNullException(nameof(allDocuments));
                }

                this.allDocuments = allDocuments;
                this.documents = null;
            }

            public BranchSet(PooledHashSetSlim<int> documents)
            {
                if (documents is null)
                {
                    throw new ArgumentNullException(nameof(documents));
                }

                this.allDocuments = Empty;
                this.documents = documents;
            }

            public IReadOnlyCollection<int> Documents
            {
                get
                {
                    return this.documents ?? this.allDocuments;
                }
            }

            public void Clear()
            {
                if (this.documents is null)
                {
                    this.documents = Empty;
                }
                else
                {
                    if (this.documents is not null)
                    {
                        this.documents.Dispose();
                        this.documents = Empty;
                    }
                }
            }

            public void IntersectWith(IEnumerable<int> documents)
            {
                if (this.documents is null)
                {
                    this.documents = new PooledHashSetSlim<int>(documents);
                }
                else
                {
                    this.documents.IntersectWith(documents);
                }
            }

            public void ExceptWith(IEnumerable<int> documents)
            {
                if (this.documents is null)
                {
                    this.documents = new PooledHashSetSlim<int>(this.allDocuments);
                }

                this.documents.ExceptWith(documents);
            }

            public void Dispose()
            {
                if (this.documents is not null)
                {
                    this.documents.Dispose();
                    this.documents = null;
                }
            }
        }
    }
}
