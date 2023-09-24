using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TokenDocumentStore
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, HashSetSlim<int>> tokenDocuments;

        public TokenDocumentStore(
            ITokenEncoder tokenEncoder,
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

            var averageItemsPerToken = (double)sumCount / (1 + this.tokenDocuments.Count);

            this.FilterOrder = averageItemsPerToken;
        }

        public double FilterOrder { get; }

        public void Filter(Field field, ValuesExpression valuesExpression, ref DocumentResultBuilder documentResultBuilder)
        {
            if (valuesExpression is AnyValuesExpression anyValuesExpression)
            {
                if (anyValuesExpression.Values.Count == 1)
                {
                    foreach (var token in (HashSetSlim<string>)anyValuesExpression.Values)
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

                    foreach (var token in (HashSetSlim<string>)anyValuesExpression.Values)
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
            else if (valuesExpression is AllValuesExpression allValuesExpression)
            {
                foreach (var token in (HashSetSlim<string>)allValuesExpression.Values)
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
                throw new InvalidOperationException("Unsupported values expression encountered.");
            }
        }
    }
}
