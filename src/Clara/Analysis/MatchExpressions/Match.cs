using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public static class Match
    {
        public static MatchExpression Empty
        {
            get
            {
                return EmptyMatchExpression.Instance;
            }
        }

        public static MatchExpression All(ScoreAggregation scoreAggregation, Token? token)
        {
            return All(scoreAggregation, new PrimitiveEnumerable<Token>(token));
        }

        public static MatchExpression All(ScoreAggregation scoreAggregation, IEnumerable<Token>? tokens)
        {
            return All(scoreAggregation, new PrimitiveEnumerable<Token>(tokens));
        }

        public static MatchExpression Any(ScoreAggregation scoreAggregation, Token? token)
        {
            return Any(scoreAggregation, new PrimitiveEnumerable<Token>(token));
        }

        public static MatchExpression Any(ScoreAggregation scoreAggregation, IEnumerable<Token>? tokens)
        {
            return Any(scoreAggregation, new PrimitiveEnumerable<Token>(tokens));
        }

        public static MatchExpression And(ScoreAggregation scoreAggregation, IEnumerable<MatchExpression?>? expressions)
        {
            return And(scoreAggregation, new ObjectEnumerable<MatchExpression>(expressions));
        }

        public static MatchExpression Or(ScoreAggregation scoreAggregation, IEnumerable<MatchExpression?>? expressions)
        {
            return Or(scoreAggregation, new ObjectEnumerable<MatchExpression>(expressions));
        }

        private static MatchExpression All(ScoreAggregation scoreAggregation, PrimitiveEnumerable<Token> tokens)
        {
            var result = default(ObjectPoolLease<ListSlim<Token>>?);

            foreach (var token in tokens)
            {
                result ??= SharedObjectPools.MatchTokens.Lease();
                result.Value.Instance.Add(token);
            }

            if (result is not null)
            {
                return new IntermittentAllMatchExpression(scoreAggregation, result.Value);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression Any(ScoreAggregation scoreAggregation, PrimitiveEnumerable<Token> tokens)
        {
            var result = default(ObjectPoolLease<ListSlim<Token>>?);

            foreach (var token in tokens)
            {
                result ??= SharedObjectPools.MatchTokens.Lease();
                result.Value.Instance.Add(token);
            }

            if (result is not null)
            {
                return new IntermittentAnyMatchExpression(scoreAggregation, result.Value);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression And(ScoreAggregation scoreAggregation, ObjectEnumerable<MatchExpression> expressions)
        {
            using var queue = SharedObjectPools.MatchExpressionQueues.Lease();
            var result = default(ObjectPoolLease<ListSlim<MatchExpression>>?);

            foreach (var expression in expressions)
            {
                queue.Instance.Enqueue(expression);
            }

            while (queue.Instance.Count > 0)
            {
                var expression = queue.Instance.Dequeue();

                if (expression is EmptyMatchExpression)
                {
                    continue;
                }

                if (expression is AndMatchExpression andMatchExpression && andMatchExpression.ScoreAggregation == scoreAggregation)
                {
                    foreach (var expression2 in (ListSlim<MatchExpression>)andMatchExpression.Expressions)
                    {
                        queue.Instance.Enqueue(expression2);
                    }

                    andMatchExpression.Discard();

                    continue;
                }

                result ??= SharedObjectPools.MatchExpressions.Lease();
                result.Value.Instance.Add(expression);
            }

            if (result is not null)
            {
                if (result.Value.Instance.Count == 1)
                {
                    try
                    {
                        return result.Value.Instance[0];
                    }
                    finally
                    {
                        result.Value.Dispose();
                    }
                }

                return new IntermittentAndMatchExpression(scoreAggregation, result.Value);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression Or(ScoreAggregation scoreAggregation, ObjectEnumerable<MatchExpression> expressions)
        {
            using var queue = SharedObjectPools.MatchExpressionQueues.Lease();
            var result = default(ObjectPoolLease<ListSlim<MatchExpression>>?);

            foreach (var expression in expressions)
            {
                queue.Instance.Enqueue(expression);
            }

            while (queue.Instance.Count > 0)
            {
                var expression = queue.Instance.Dequeue();

                if (expression is EmptyMatchExpression)
                {
                    continue;
                }

                if (expression is OrMatchExpression orMatchExpression && orMatchExpression.ScoreAggregation == scoreAggregation)
                {
                    foreach (var expression2 in (ListSlim<MatchExpression>)orMatchExpression.Expressions)
                    {
                        queue.Instance.Enqueue(expression2);
                    }

                    orMatchExpression.Discard();

                    continue;
                }

                result ??= SharedObjectPools.MatchExpressions.Lease();
                result.Value.Instance.Add(expression);
            }

            if (result is not null)
            {
                if (result.Value.Instance.Count == 1)
                {
                    try
                    {
                        return result.Value.Instance[0];
                    }
                    finally
                    {
                        result.Value.Dispose();
                    }
                }

                return new IntermittentOrMatchExpression(scoreAggregation, result.Value);
            }

            return EmptyMatchExpression.Instance;
        }
    }
}
