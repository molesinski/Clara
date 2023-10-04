using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public static class Match
    {
        public static MatchExpression All(ScoringMode scoringMode, string? token)
        {
            return All(scoringMode, new StringEnumerable(token));
        }

        public static MatchExpression All(ScoringMode scoringMode, IEnumerable<string?>? tokens)
        {
            return All(scoringMode, new StringEnumerable(tokens));
        }

        public static MatchExpression Any(ScoringMode scoringMode, string? token)
        {
            return Any(scoringMode, new StringEnumerable(token));
        }

        public static MatchExpression Any(ScoringMode scoringMode, IEnumerable<string?>? tokens)
        {
            return Any(scoringMode, new StringEnumerable(tokens));
        }

        public static MatchExpression And(ScoringMode scoringMode, IEnumerable<MatchExpression?>? expressions)
        {
            if (expressions is not null)
            {
                var result = new ListSlim<MatchExpression>();
                var queue = new Queue<MatchExpression>();

                foreach (var expression in expressions)
                {
                    if (expression is not null)
                    {
                        queue.Enqueue(expression);
                    }
                }

                while (queue.Count > 0)
                {
                    var expression = queue.Dequeue();

                    if (expression is EmptyMatchExpression)
                    {
                        continue;
                    }

                    if (expression is AndMatchExpression andMatchExpression && andMatchExpression.ScoringMode == scoringMode)
                    {
                        foreach (var expression2 in (ListSlim<MatchExpression>)andMatchExpression.Expressions)
                        {
                            queue.Enqueue(expression2);
                        }

                        continue;
                    }

                    result.Add(expression);
                }

                if (result.Count == 1)
                {
                    return result[0];
                }

                if (result.Count > 1)
                {
                    return new AndMatchExpression(scoringMode, result);
                }
            }

            return EmptyMatchExpression.Instance;
        }

        public static MatchExpression Or(ScoringMode scoringMode, IEnumerable<MatchExpression?>? expressions)
        {
            if (expressions is not null)
            {
                var result = new ListSlim<MatchExpression>();
                var queue = new Queue<MatchExpression>();

                foreach (var expression in expressions)
                {
                    if (expression is not null)
                    {
                        queue.Enqueue(expression);
                    }
                }

                while (queue.Count > 0)
                {
                    var expression = queue.Dequeue();

                    if (expression is EmptyMatchExpression)
                    {
                        continue;
                    }

                    if (expression is OrMatchExpression orMatchExpression && orMatchExpression.ScoringMode == scoringMode)
                    {
                        foreach (var expression2 in (ListSlim<MatchExpression>)orMatchExpression.Expressions)
                        {
                            queue.Enqueue(expression2);
                        }

                        continue;
                    }

                    result.Add(expression);
                }

                if (result.Count == 1)
                {
                    return result[0];
                }

                if (result.Count > 1)
                {
                    return new OrMatchExpression(scoringMode, result);
                }
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression All(ScoringMode scoringMode, StringEnumerable tokens)
        {
            var result = default(ListSlim<string>);

            foreach (var token in tokens)
            {
                result ??= new();
                result.Add(token);
            }

            if (result is not null)
            {
                return new AllTokensMatchExpression(scoringMode, result);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression Any(ScoringMode scoringMode, StringEnumerable tokens)
        {
            var result = default(ListSlim<string>);

            foreach (var token in tokens)
            {
                result ??= new();
                result.Add(token);
            }

            if (result is not null)
            {
                return new AnyTokensMatchExpression(scoringMode, result);
            }

            return EmptyMatchExpression.Instance;
        }
    }
}
