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

        public static MatchExpression All(ScoreAggregation scoreAggregation, string? token)
        {
            return All(scoreAggregation, new StringEnumerable(token));
        }

        public static MatchExpression All(ScoreAggregation scoreAggregation, IEnumerable<string?>? tokens)
        {
            return All(scoreAggregation, new StringEnumerable(tokens));
        }

        public static MatchExpression Any(ScoreAggregation scoreAggregation, string? token)
        {
            return Any(scoreAggregation, new StringEnumerable(token));
        }

        public static MatchExpression Any(ScoreAggregation scoreAggregation, IEnumerable<string?>? tokens)
        {
            return Any(scoreAggregation, new StringEnumerable(tokens));
        }

        public static MatchExpression And(ScoreAggregation scoreAggregation, IEnumerable<MatchExpression?>? expressions)
        {
            return And(scoreAggregation, new ObjectEnumerable<MatchExpression>(expressions));
        }

        public static MatchExpression Or(ScoreAggregation scoreAggregation, IEnumerable<MatchExpression?>? expressions)
        {
            return Or(scoreAggregation, new ObjectEnumerable<MatchExpression>(expressions));
        }

        private static MatchExpression All(ScoreAggregation scoreAggregation, StringEnumerable tokens)
        {
            var result = default(ObjectPoolLease<ListSlim<string>>?);

            foreach (var token in tokens)
            {
                result ??= SharedObjectPools.MatchTokens.Lease();
                result.Value.Instance.Add(token);
            }

            if (result is not null)
            {
                return new DisposableAllMatchExpression(scoreAggregation, result.Value);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression Any(ScoreAggregation scoreAggregation, StringEnumerable tokens)
        {
            var result = default(ObjectPoolLease<ListSlim<string>>?);

            foreach (var token in tokens)
            {
                result ??= SharedObjectPools.MatchTokens.Lease();
                result.Value.Instance.Add(token);
            }

            if (result is not null)
            {
                return new DisposableAnyMatchExpression(scoreAggregation, result.Value);
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

                return new DisposableAndMatchExpression(scoreAggregation, result.Value);
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

                return new DisposableOrMatchExpression(scoreAggregation, result.Value);
            }

            return EmptyMatchExpression.Instance;
        }
    }
}
