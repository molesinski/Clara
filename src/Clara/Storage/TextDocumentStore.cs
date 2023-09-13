using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextDocumentStore
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, HashSetSlim<int>> tokenDocuments;

        public TextDocumentStore(
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
        }

        public void Search(Field field, MatchExpression matchExpression, DocumentSet documentSet)
        {
            if (matchExpression is AnyMatchExpression anyValuesMatchExpression)
            {
                using var anyMatches = new PooledHashSet<int>(Allocator.ArrayPool);

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
            else if (matchExpression is AllMatchExpression allValuesMatchExpression)
            {
                foreach (var token in allValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocuments.TryGetValue(tokenId, out var documents))
                        {
                            documentSet.IntersectWith(field, documents);

                            continue;
                        }
                    }

                    documentSet.Clear();

                    break;
                }
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                using var anyMatches = new PooledHashSet<int>(Allocator.ArrayPool);
                using var tempSet = new PooledHashSet<int>(Allocator.ArrayPool);

                foreach (var expression in orMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Search(expression, tempSet);

                    anyMatches.UnionWith(tempSet);
                }

                documentSet.IntersectWith(field, anyMatches);
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                using var tempSet = new PooledHashSet<int>(Allocator.ArrayPool);

                foreach (var expression in andMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Search(expression, tempSet);

                    documentSet.IntersectWith(field, tempSet);
                }
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }

        private void Search(MatchExpression matchExpression, PooledHashSet<int> resultSet)
        {
            if (matchExpression is AnyMatchExpression anyValuesMatchExpression)
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
            else if (matchExpression is AllMatchExpression allValuesMatchExpression)
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
                using var tempSet = new PooledHashSet<int>(Allocator.ArrayPool);

                foreach (var expression in orMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Search(expression, tempSet);

                    resultSet.UnionWith(tempSet);
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                using var tempSet = new PooledHashSet<int>(Allocator.ArrayPool);
                var isFirst = true;

                foreach (var expression in andMatchExpression.Expressions)
                {
                    tempSet.Clear();

                    this.Search(expression, tempSet);

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
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }
    }
}
