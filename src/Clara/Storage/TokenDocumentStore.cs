using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TokenDocumentStore
    {
        private readonly TokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, HashSetSlim<int>> tokenDocuments;

        public TokenDocumentStore(
            TokenEncoder tokenEncoder,
            DictionarySlim<int, HashSetSlim<int>> tokenDocuments)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (tokenDocuments is null)
            {
                throw new ArgumentNullException(nameof(tokenDocuments));
            }

            this.tokenEncoder = tokenEncoder;
            this.tokenDocuments = tokenDocuments;

            var sumCount = 0;

            foreach (var pair in this.tokenDocuments)
            {
                var documents = pair.Value;

                sumCount += documents.Count;
            }

            this.FilterOrder = (double)sumCount / (1 + this.tokenDocuments.Count);
        }

        public double FilterOrder { get; }

        public DocumentSet Filter(FilterMode mode, FilterValueCollection values)
        {
            if (values.Count == 1)
            {
                foreach (var token in values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var tokenDocuments))
                        {
                            return new DocumentSet(tokenDocuments);
                        }
                    }
                }

                return default;
            }
            else if (mode == FilterMode.Any)
            {
                var documents = SharedObjectPools.DocumentSets.Lease();

                foreach (var token in values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var tokenDocuments))
                        {
                            documents.Instance.UnionWith(tokenDocuments);
                        }
                    }
                }

                return new DocumentSet(documents);
            }
            else if (mode == FilterMode.All)
            {
                var documents = SharedObjectPools.DocumentSets.Lease();
                var isFirst = true;

                foreach (var token in values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var tokenDocuments))
                        {
                            if (isFirst)
                            {
                                documents.Instance.UnionWith(tokenDocuments);
                                isFirst = false;
                            }
                            else
                            {
                                documents.Instance.IntersectWith(tokenDocuments);
                            }

                            continue;
                        }
                    }

                    documents.Instance.Clear();

                    break;
                }

                return new DocumentSet(documents);
            }
            else
            {
                throw new InvalidOperationException("Unsupported filter mode enum value encountered.");
            }
        }
    }
}
