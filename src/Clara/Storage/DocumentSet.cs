using System;
using System.Collections.Generic;
using Clara.Collections;
using Clara.Mapping;

namespace Clara.Storage
{
    internal sealed class DocumentSet : IDisposable
    {
        private static readonly PooledSet<int> Empty = new();

        private readonly IReadOnlyCollection<int> allDocuments;
        private readonly Dictionary<Field, PooledSet<int>> facetDocuments = new();
        private PooledSet<int>? documents;

        public DocumentSet(IReadOnlyCollection<int> allDocuments)
        {
            if (allDocuments is null)
            {
                throw new ArgumentNullException(nameof(allDocuments));
            }

            this.allDocuments = allDocuments;
            this.documents = null;
        }

        public DocumentSet(PooledSet<int> documents)
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

        public IReadOnlyCollection<int> Documents
        {
            get
            {
                return this.documents ?? this.allDocuments;
            }
        }

        public IReadOnlyCollection<int> GetMatches(Field field)
        {
            if (this.facetDocuments.TryGetValue(field, out var facetDocuments))
            {
                return facetDocuments;
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
                this.documents.Clear();
                this.facetDocuments.Clear();
            }
        }

        public void Branch(Field field)
        {
            if (this.documents is null)
            {
                this.facetDocuments.Add(field, new PooledSet<int>(this.allDocuments));
            }
            else
            {
                this.facetDocuments.Add(field, new PooledSet<int>(this.documents));
            }
        }

        public void IntersectWith(Field field, PooledSet<int> documents)
        {
            if (this.documents is null)
            {
                this.documents = documents;
            }
            else
            {
                this.documents.IntersectWith(documents);

                foreach (var pair in this.facetDocuments)
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
                this.documents = new PooledSet<int>(documents);
            }
            else
            {
                this.documents.IntersectWith(documents);

                foreach (var pair in this.facetDocuments)
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
                this.documents = new PooledSet<int>(this.allDocuments);

                this.documents.ExceptWith(documents);
            }
            else
            {
                this.documents.ExceptWith(documents);

                foreach (var pair in this.facetDocuments)
                {
                    pair.Value.ExceptWith(documents);
                }
            }
        }

        public void Dispose()
        {
            foreach (var pair in this.facetDocuments)
            {
                pair.Value.Dispose();
            }

            this.documents?.Dispose();

            this.facetDocuments.Clear();
            this.documents = null;
        }
    }
}
