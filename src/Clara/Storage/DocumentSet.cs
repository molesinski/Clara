using System.Collections;
using Clara.Mapping;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class DocumentSet : IDocumentSet
    {
        private readonly Dictionary<Field, BranchResult> branches = new();
        private readonly IReadOnlyCollection<int> allDocuments;
        private ObjectPoolLease<HashSetSlim<int>>? documents;

        public DocumentSet(IReadOnlyCollection<int> allDocuments)
        {
            if (allDocuments is null)
            {
                throw new ArgumentNullException(nameof(allDocuments));
            }

            this.allDocuments = allDocuments;
            this.documents = null;
        }

        public DocumentSet(ObjectPoolLease<HashSetSlim<int>> documents)
        {
            this.allDocuments = Array.Empty<int>();
            this.documents = documents;
        }

        public int Count
        {
            get
            {
                return (this.documents?.Instance ?? this.allDocuments).Count;
            }
        }

        public IReadOnlyCollection<int> GetFacetDocuments(Field field)
        {
            if (this.branches.TryGetValue(field, out var facetDocuments))
            {
                return facetDocuments.Documents;
            }

            return this.documents?.Instance ?? this.allDocuments;
        }

        public void BranchForFaceting(Field field)
        {
            if (this.documents is null)
            {
                this.branches.Add(field, new BranchResult(this.allDocuments));
            }
            else
            {
                this.branches.Add(field, new BranchResult(this.documents.Value.Instance));
            }
        }

        public void Clear()
        {
            if (this.documents is null)
            {
                this.documents = HashSetSlim<int>.ObjectPool.Lease();
            }
            else
            {
                this.documents.Value.Instance.Clear();
            }

            foreach (var pair in this.branches)
            {
                pair.Value.Dispose();
            }

            this.branches.Clear();
        }

        public void IntersectWith(Field field, IEnumerable<int> documents)
        {
            if (this.documents is null)
            {
                this.documents = HashSetSlim<int>.ObjectPool.Lease();
                this.documents.Value.Instance.UnionWith(documents);
            }
            else
            {
                this.documents.Value.Instance.IntersectWith(documents);

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
                this.documents = HashSetSlim<int>.ObjectPool.Lease();
                this.documents.Value.Instance.UnionWith(this.allDocuments);
            }

            this.documents.Value.Instance.ExceptWith(documents);

            foreach (var pair in this.branches)
            {
                pair.Value.ExceptWith(documents);
            }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return (this.documents?.Instance ?? this.allDocuments).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            this.documents?.Dispose();

            foreach (var pair in this.branches)
            {
                pair.Value.Dispose();
            }

            this.branches.Clear();
        }

        private sealed class BranchResult : IDisposable
        {
            private readonly IReadOnlyCollection<int> allDocuments;
            private ObjectPoolLease<HashSetSlim<int>>? branchDocuments;

            public BranchResult(IReadOnlyCollection<int> allDocuments)
            {
                if (allDocuments is null)
                {
                    throw new ArgumentNullException(nameof(allDocuments));
                }

                this.allDocuments = allDocuments;
                this.branchDocuments = null;
            }

            public BranchResult(HashSetSlim<int> branchDocuments)
            {
                this.allDocuments = Array.Empty<int>();

                this.branchDocuments = HashSetSlim<int>.ObjectPool.Lease();
                this.branchDocuments.Value.Instance.UnionWith(branchDocuments);
            }

            public IReadOnlyCollection<int> Documents
            {
                get
                {
                    return this.branchDocuments?.Instance ?? this.allDocuments;
                }
            }

            public void IntersectWith(IEnumerable<int> documents)
            {
                if (this.branchDocuments is null)
                {
                    this.branchDocuments = HashSetSlim<int>.ObjectPool.Lease();
                    this.branchDocuments.Value.Instance.UnionWith(documents);
                }
                else
                {
                    this.branchDocuments.Value.Instance.IntersectWith(documents);
                }
            }

            public void ExceptWith(IEnumerable<int> documents)
            {
                if (this.branchDocuments is null)
                {
                    this.branchDocuments = HashSetSlim<int>.ObjectPool.Lease();
                    this.branchDocuments.Value.Instance.UnionWith(this.allDocuments);
                }

                this.branchDocuments.Value.Instance.ExceptWith(documents);
            }

            public void Dispose()
            {
                this.branchDocuments?.Dispose();
            }
        }
    }
}
