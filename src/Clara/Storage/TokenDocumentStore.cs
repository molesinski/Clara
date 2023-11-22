using Clara.Mapping;
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

        public void Filter(Field field, FilterMode mode, HashSetSlim<string> values, ref DocumentResultBuilder documentResultBuilder)
        {
            if (mode == FilterMode.Any)
            {
                if (values.Count == 1)
                {
                    foreach (var token in values)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                            {
                                documentResultBuilder.IntersectWith(field, documents);

                                return;
                            }
                        }
                    }

                    documentResultBuilder.Clear();
                }
                else
                {
                    using var tempSet = SharedObjectPools.DocumentSets.Lease();

                    foreach (var token in values)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                            {
                                tempSet.Instance.UnionWith(documents);
                            }
                        }
                    }

                    documentResultBuilder.IntersectWith(field, tempSet.Instance);
                }
            }
            else if (mode == FilterMode.All)
            {
                foreach (var token in values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                        {
                            documentResultBuilder.IntersectWith(field, documents);

                            continue;
                        }
                    }

                    documentResultBuilder.Clear();

                    break;
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported filter mode enum value encountered.");
            }
        }
    }
}
