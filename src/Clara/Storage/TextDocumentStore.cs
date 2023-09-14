using Clara.Mapping;
using Clara.Querying;
using Clara.Utils;

namespace Clara.Storage
{
    internal sealed class TextDocumentStore
    {
        private readonly ITokenEncoder tokenEncoder;
        private readonly DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores;

        public TextDocumentStore(
            ITokenEncoder tokenEncoder,
            DictionarySlim<int, DictionarySlim<int, float>> tokenDocumentScores,
            DictionarySlim<int, int> documentLengths,
            Weight weight)
        {
            if (tokenEncoder is null)
            {
                throw new ArgumentNullException(nameof(tokenEncoder));
            }

            if (tokenDocumentScores is null)
            {
                throw new ArgumentNullException(nameof(tokenDocumentScores));
            }

            if (weight is null)
            {
                throw new ArgumentNullException(nameof(weight));
            }

            weight.Process(tokenDocumentScores, documentLengths);

            this.tokenEncoder = tokenEncoder;
            this.tokenDocumentScores = tokenDocumentScores;
        }

        public DocumentScoring Search(Field field, MatchExpression matchExpression, DocumentSet documentSet)
        {
            if (matchExpression is AnyMatchExpression anyValuesMatchExpression)
            {
                var documentScores = new PooledDictionary<int, float>(Allocator.ArrayPool);

                foreach (var token in anyValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            documentScores.UnionWith(documents, ValueCombiner.Sum);
                        }
                    }
                }

                documentSet.IntersectWith(field, documentScores);

                return new DocumentScoring(documentScores);
            }
            else if (matchExpression is AllMatchExpression allValuesMatchExpression)
            {
                var documentScores = new PooledDictionary<int, float>(Allocator.ArrayPool);
                var isFirst = true;

                foreach (var token in allValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            if (isFirst)
                            {
                                documentScores.UnionWith(documents, ValueCombiner.Sum);
                                isFirst = false;
                            }
                            else
                            {
                                documentScores.IntersectWith(documents, ValueCombiner.Sum);
                            }

                            continue;
                        }
                    }

                    documentScores.Clear();

                    break;
                }

                documentSet.IntersectWith(field, documentScores);

                return new DocumentScoring(documentScores);
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                var documentScores = new PooledDictionary<int, float>(Allocator.ArrayPool);

                using (var tempScores = new PooledDictionary<int, float>(Allocator.ArrayPool))
                {
                    foreach (var expression in orMatchExpression.Expressions)
                    {
                        tempScores.Clear();

                        this.SearchPartial(expression, tempScores);

                        documentScores.UnionWith(tempScores, ValueCombiner.Max);
                    }
                }

                documentSet.IntersectWith(field, documentScores);

                return new DocumentScoring(documentScores);
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var documentScores = new PooledDictionary<int, float>(Allocator.ArrayPool);
                var isFirst = true;

                using (var tempScores = new PooledDictionary<int, float>(Allocator.ArrayPool))
                {
                    foreach (var expression in andMatchExpression.Expressions)
                    {
                        tempScores.Clear();

                        this.SearchPartial(expression, tempScores);

                        if (isFirst)
                        {
                            documentScores.UnionWith(tempScores, ValueCombiner.Max);
                            isFirst = false;
                        }
                        else
                        {
                            documentScores.IntersectWith(tempScores, ValueCombiner.Max);
                        }
                    }
                }

                documentSet.IntersectWith(field, documentScores);

                return new DocumentScoring(documentScores);
            }
            else if (matchExpression is EmptyMatchExpression)
            {
                documentSet.Clear();

                return new DocumentScoring(new PooledDictionary<int, float>(Allocator.ArrayPool));
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }

        private void SearchPartial(MatchExpression matchExpression, PooledDictionary<int, float> partialScores)
        {
            if (matchExpression is AnyMatchExpression anyValuesMatchExpression)
            {
                foreach (var token in anyValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            partialScores.UnionWith(documents, ValueCombiner.Sum);
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
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            if (isFirst)
                            {
                                partialScores.UnionWith(documents, ValueCombiner.Sum);
                                isFirst = false;
                            }
                            else
                            {
                                partialScores.IntersectWith(documents, ValueCombiner.Sum);
                            }

                            continue;
                        }
                    }

                    partialScores.Clear();

                    break;
                }
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                using var tempScores = new PooledDictionary<int, float>(Allocator.ArrayPool);

                foreach (var expression in orMatchExpression.Expressions)
                {
                    tempScores.Clear();

                    this.SearchPartial(expression, tempScores);

                    partialScores.UnionWith(tempScores, ValueCombiner.Max);
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var isFirst = true;

                using var tempScores = new PooledDictionary<int, float>(Allocator.ArrayPool);

                foreach (var expression in andMatchExpression.Expressions)
                {
                    tempScores.Clear();

                    this.SearchPartial(expression, tempScores);

                    if (isFirst)
                    {
                        partialScores.UnionWith(tempScores, ValueCombiner.Max);
                        isFirst = false;
                    }
                    else
                    {
                        partialScores.IntersectWith(tempScores, ValueCombiner.Max);
                    }
                }
            }
            else if (matchExpression is EmptyMatchExpression)
            {
                partialScores.Clear();
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }
    }
}
