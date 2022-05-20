using System;
using System.Collections.Generic;
using Clara.Mapping;
using Clara.Querying;

namespace Clara.Storage
{
    internal sealed class TokenDocumentStore
    {
        private readonly TokenEncoder tokenEncoder;
        private readonly Dictionary<int, HashSet<int>> tokenDocuments;

        public TokenDocumentStore(
            TokenEncoder tokenEncoder,
            Dictionary<int, HashSet<int>> tokenDocuments)
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
            var bufferScope = documentSet.BufferScope;

            if (matchExpression is AnyValuesMatchExpression anyValuesMatchExpression)
            {
                if (anyValuesMatchExpression.Values.Count == 1)
                {
                    var token = anyValuesMatchExpression.Values[0];

                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                        {
                            documentSet.IntersectWith(field, (IReadOnlyCollection<int>)documents);

                            return;
                        }
                    }

                    documentSet.Clear();
                }
                else
                {
                    var anyMatches = bufferScope.CreateDocumentSet();

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
                            documentSet.IntersectWith(field, (IReadOnlyCollection<int>)documents);

                            continue;
                        }
                    }

                    documentSet.Clear();

                    break;
                }
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                var anyMatches = bufferScope.CreateDocumentSet();
                var tempSet = bufferScope.CreateDocumentSet();

                foreach (var expression in orMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Filter(expression, tempSet, bufferScope);

                    anyMatches.UnionWith(tempSet);
                }

                documentSet.IntersectWith(field, anyMatches);
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var tempSet = bufferScope.CreateDocumentSet();

                foreach (var expression in andMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Filter(expression, tempSet, bufferScope);

                    documentSet.IntersectWith(field, (IReadOnlyCollection<int>)tempSet);
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported token expression encountered.");
            }
        }

        private void Filter(MatchExpression matchExpression, HashSet<int> resultSet, BufferScope bufferScope)
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
                var tempSet = bufferScope.CreateDocumentSet();

                foreach (var expression in orMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Filter(expression, tempSet, bufferScope);

                    resultSet.UnionWith(tempSet);
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var tempSet = bufferScope.CreateDocumentSet();
                var isFirst = true;

                foreach (var expression in andMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Filter(expression, tempSet, bufferScope);

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
    }
}
