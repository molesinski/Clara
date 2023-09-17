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
                if (anyValuesMatchExpression.Values.Count == 1)
                {
                    var token = anyValuesMatchExpression.Values.First();

                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            documentSet.IntersectWith(field, documents);

                            return DocumentScoring.From(documents);
                        }
                    }

                    documentSet.Clear();

                    return DocumentScoring.Empty;
                }
                else
                {
                    var documentScores = DictionarySlim<int, float>.ObjectPool.Lease();

                    foreach (var token in anyValuesMatchExpression.Values)
                    {
                        if (this.tokenEncoder.TryEncode(token, out var tokenId))
                        {
                            if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                            {
                                documentScores.Instance.UnionWith(documents, ValueCombiner.Sum);
                            }
                        }
                    }

                    documentSet.IntersectWith(field, documentScores.Instance);

                    return DocumentScoring.From(documentScores);
                }
            }
            else if (matchExpression is AllMatchExpression allValuesMatchExpression)
            {
                var documentScores = DictionarySlim<int, float>.ObjectPool.Lease();
                var isFirst = true;

                foreach (var token in allValuesMatchExpression.Values)
                {
                    if (this.tokenEncoder.TryEncode(token, out var tokenId))
                    {
                        if (this.tokenDocumentScores.TryGetValue(tokenId, out var documents))
                        {
                            if (isFirst)
                            {
                                documentScores.Instance.UnionWith(documents, ValueCombiner.Sum);
                                isFirst = false;
                            }
                            else
                            {
                                documentScores.Instance.IntersectWith(documents, ValueCombiner.Sum);
                            }

                            continue;
                        }
                    }

                    documentScores.Instance.Clear();

                    break;
                }

                documentSet.IntersectWith(field, documentScores.Instance);

                return DocumentScoring.From(documentScores);
            }
            else if (matchExpression is OrMatchExpression orMatchExpression)
            {
                var documentScores = DictionarySlim<int, float>.ObjectPool.Lease();

                using (var tempScores = DictionarySlim<int, float>.ObjectPool.Lease())
                {
                    foreach (var expression in orMatchExpression.Expressions)
                    {
                        tempScores.Instance.Clear();

                        this.SearchPartial(expression, tempScores.Instance);

                        documentScores.Instance.UnionWith(tempScores.Instance, ValueCombiner.Max);
                    }
                }

                documentSet.IntersectWith(field, documentScores.Instance);

                return DocumentScoring.From(documentScores);
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var documentScores = DictionarySlim<int, float>.ObjectPool.Lease();
                var isFirst = true;

                using (var tempScores = DictionarySlim<int, float>.ObjectPool.Lease())
                {
                    foreach (var expression in andMatchExpression.Expressions)
                    {
                        tempScores.Instance.Clear();

                        this.SearchPartial(expression, tempScores.Instance);

                        if (isFirst)
                        {
                            documentScores.Instance.UnionWith(tempScores.Instance, ValueCombiner.Max);
                            isFirst = false;
                        }
                        else
                        {
                            documentScores.Instance.IntersectWith(tempScores.Instance, ValueCombiner.Max);
                        }
                    }
                }

                documentSet.IntersectWith(field, documentScores.Instance);

                return DocumentScoring.From(documentScores);
            }
            else if (matchExpression is EmptyMatchExpression)
            {
                documentSet.Clear();

                return DocumentScoring.Empty;
            }
            else
            {
                throw new InvalidOperationException("Unsupported match expression type encountered.");
            }
        }

        private void SearchPartial(MatchExpression matchExpression, DictionarySlim<int, float> partialScores)
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
                using var tempScores = DictionarySlim<int, float>.ObjectPool.Lease();

                foreach (var expression in orMatchExpression.Expressions)
                {
                    tempScores.Instance.Clear();

                    this.SearchPartial(expression, tempScores.Instance);

                    partialScores.UnionWith(tempScores.Instance, ValueCombiner.Max);
                }
            }
            else if (matchExpression is AndMatchExpression andMatchExpression)
            {
                var isFirst = true;

                using var tempScores = DictionarySlim<int, float>.ObjectPool.Lease();

                foreach (var expression in andMatchExpression.Expressions)
                {
                    tempScores.Instance.Clear();

                    this.SearchPartial(expression, tempScores.Instance);

                    if (isFirst)
                    {
                        partialScores.UnionWith(tempScores.Instance, ValueCombiner.Max);
                        isFirst = false;
                    }
                    else
                    {
                        partialScores.IntersectWith(tempScores.Instance, ValueCombiner.Max);
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
