using System;
using System.Collections.Generic;
using Clara.Mapping;

namespace Clara.Storage
{
    internal sealed class DocumentSet
    {
        private static readonly HashSet<int> Empty = new HashSet<int>();

        private readonly IReadOnlyCollection<int> allDocuments;
        private readonly BufferScope bufferScope;
        private readonly Dictionary<Field, HashSet<int>> facetDocuments = new();
        private HashSet<int>? documents;

        public DocumentSet(
            IReadOnlyCollection<int> allDocuments,
            BufferScope bufferScope)
        {
            if (allDocuments is null)
            {
                throw new ArgumentNullException(nameof(allDocuments));
            }

            if (bufferScope is null)
            {
                throw new ArgumentNullException(nameof(bufferScope));
            }

            this.allDocuments = allDocuments;
            this.bufferScope = bufferScope;
            this.documents = null;
        }

        public DocumentSet(
            HashSet<int> documents,
            BufferScope bufferScope)
        {
            if (documents is null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            this.allDocuments = Empty;
            this.documents = documents;
            this.bufferScope = bufferScope;
        }

        public IReadOnlyCollection<int> Documents
        {
            get
            {
                return this.documents ?? this.allDocuments;
            }
        }

        public BufferScope BufferScope
        {
            get
            {
                return this.bufferScope;
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
                this.facetDocuments.Add(field, this.bufferScope.CreateDocumentSet(this.allDocuments));
            }
            else
            {
                this.facetDocuments.Add(field, this.bufferScope.CreateDocumentSet(this.documents));
            }
        }

        public void IntersectWith(Field field, HashSet<int> documents)
        {
            if (this.documents is null)
            {
                this.documents = documents;
            }
            else
            {
                this.documents.IntersectWith(documents);
            }

            foreach (var pair in this.facetDocuments)
            {
                if (pair.Key != field)
                {
                    pair.Value.IntersectWith(documents);
                }
            }
        }

        public void IntersectWith(Field field, IReadOnlyCollection<int> documents)
        {
            if (this.documents is null)
            {
                this.documents = this.bufferScope.CreateDocumentSet(documents);
            }
            else
            {
                this.documents.IntersectWith(documents);
            }

            foreach (var pair in this.facetDocuments)
            {
                if (pair.Key != field)
                {
                    pair.Value.IntersectWith(documents);
                }
            }
        }

        public void ExceptWith(IReadOnlyCollection<int> documents)
        {
            this.documents ??= this.bufferScope.CreateDocumentSet(this.allDocuments);
            this.documents.ExceptWith(documents);

            foreach (var pair in this.facetDocuments)
            {
                pair.Value.ExceptWith(documents);
            }
        }
    }
}
