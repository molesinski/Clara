using System;
using System.Collections.Generic;
using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TokenDocumentStore : IDisposable
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

        public void Filter(Field field, MatchExpression matchExpression, DocumentSet documentSet)
        {
            if (matchExpression is AnyValuesMatchExpression anyValuesMatchExpression)
            {
                if (anyValuesMatchExpression.Values.Count == 1)
                {
                    var token = anyValuesMatchExpression.Values[0];

                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                        {
                            documentSet.IntersectWith(field, (IEnumerable<int>)documents);

                            return;
                        }
                    }

                    documentSet.Clear();
                }
                else
                {
                    var anyMatches = new HashSetSlim<int>(Allocator.ArrayPool);

                    foreach (var token in anyValuesMatchExpression.Values)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                            {
                                anyMatches.UnionWith(documents);
                            }
                        }
                    }

                    documentSet.IntersectWith(field, anyMatches);
                }
            }
            else if (matchExpression is AllValuesMatchExpression allValuesMatchExpression)
            {
                foreach (var token in allValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                        {
                            documentSet.IntersectWith(field, (IEnumerable<int>)documents);

                            continue;
                        }
                    }

                    documentSet.Clear();

                    break;
                }
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                var anyMatches = new HashSetSlim<int>(Allocator.ArrayPool);
                using var tempSet = new HashSetSlim<int>(Allocator.ArrayPool);

                foreach (var expression in orMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Filter(expression, tempSet);

                    anyMatches.UnionWith(tempSet);
                }

                documentSet.IntersectWith(field, anyMatches);
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                using var tempSet = new HashSetSlim<int>(Allocator.ArrayPool);

                foreach (var expression in andMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Filter(expression, tempSet);

                    documentSet.IntersectWith(field, (IEnumerable<int>)tempSet);
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported token expression encountered.");
            }
        }

        private void Filter(MatchExpression matchExpression, HashSetSlim<int> resultSet)
        {
            if (matchExpression is AnyValuesMatchExpression anyValuesMatchExpression)
            {
                foreach (var token in anyValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                        {
                            resultSet.UnionWith(documents);
                        }
                    }
                }
            }
            else if (matchExpression is AllValuesMatchExpression allValuesMatchExpression)
            {
                var isFirst = true;

                foreach (var token in allValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                        {
                            if (isFirst)
                            {
                                isFirst = false;
                                resultSet.UnionWith(documents);
                            }
                            else
                            {
                                resultSet.IntersectWith(documents);
                            }

                            continue;
                        }
                    }

                    resultSet.Clear();
                    break;
                }
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                using var tempSet = new HashSetSlim<int>(Allocator.ArrayPool);

                foreach (var expression in orMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Filter(expression, tempSet);

                    resultSet.UnionWith(tempSet);
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                using var tempSet = new HashSetSlim<int>(Allocator.ArrayPool);
                var isFirst = true;

                foreach (var expression in andMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Filter(expression, tempSet);

                    if (isFirst)
                    {
                        isFirst = false;
                        resultSet.UnionWith(tempSet);
                    }
                    else
                    {
                        resultSet.IntersectWith(tempSet);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported token expression encountered.");
            }
        }

        public void Dispose()
        {
            foreach (var pair in this.tokenDocuments)
            {
                pair.Value.Dispose();
            }

            this.tokenDocuments.Dispose();
        }
    }
}
