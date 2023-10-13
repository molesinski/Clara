using Clara.Querying;
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

        public static MatchExpression All(ScoreAggregation scoreAggregation, IEnumerable<string>? tokens)
        {
            return All(scoreAggregation, new StringEnumerable(tokens));
        }

        public static MatchExpression Any(ScoreAggregation scoreAggregation, IEnumerable<string>? tokens, bool isLazy)
        {
            return Any(scoreAggregation, new StringEnumerable(tokens), isLazy);
        }

        public static MatchExpression And(ScoreAggregation scoreAggregation, IEnumerable<MatchExpression?>? expressions)
        {
            return And(scoreAggregation, new ObjectEnumerable<MatchExpression>(expressions));
        }

        public static MatchExpression Or(ScoreAggregation scoreAggregation, IEnumerable<MatchExpression?>? expressions, bool isLazy)
        {
            return Or(scoreAggregation, new ObjectEnumerable<MatchExpression>(expressions), isLazy);
        }

        public static MatchExpression Search(SearchMode mode, IEnumerable<SearchTerm> terms)
        {
            if (terms is null)
            {
                throw new ArgumentNullException(nameof(terms));
            }

            var tokens = default(ListSlim<string>?);
            var expressions = default(ListSlim<MatchExpression>?);

            foreach (var term in new PrimitiveEnumerable<SearchTerm>(terms))
            {
                if (term.Token is string token)
                {
                    tokens ??= new();
                    tokens.Add(token);
                }
                else if (term.Expression is MatchExpression expression)
                {
                    expressions ??= new();
                    expressions.Add(expression);
                }
            }

            if (tokens is not null)
            {
                MatchExpression tokenExpression =
                    mode == SearchMode.All
                        ? new AllTokensMatchExpression(ScoreAggregation.Sum, tokens)
                        : new AnyTokensMatchExpression(ScoreAggregation.Sum, tokens, isLazy: false);

                if (expressions is null)
                {
                    return tokenExpression;
                }
                else
                {
                    expressions.Insert(0, tokenExpression);
                }
            }

            if (expressions is null)
            {
                return EmptyMatchExpression.Instance;
            }
            else if (expressions.Count == 1)
            {
                return expressions[0];
            }
            else
            {
                if (mode == SearchMode.All)
                {
                    return new AndMatchExpression(ScoreAggregation.Sum, expressions);
                }
                else
                {
                    return new OrMatchExpression(ScoreAggregation.Sum, expressions, isLazy: false);
                }
            }
        }

        private static MatchExpression All(ScoreAggregation scoreAggregation, StringEnumerable tokens)
        {
            var result = default(ListSlim<string>?);

            foreach (var token in tokens)
            {
                result ??= new();
                result.Add(token);
            }

            if (result is not null)
            {
                return new AllTokensMatchExpression(scoreAggregation, result);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression Any(ScoreAggregation scoreAggregation, StringEnumerable tokens, bool isLazy)
        {
            var result = default(ListSlim<string>?);

            foreach (var token in tokens)
            {
                result ??= new();
                result.Add(token);
            }

            if (result is not null)
            {
                return new AnyTokensMatchExpression(scoreAggregation, result, isLazy);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression And(ScoreAggregation scoreAggregation, ObjectEnumerable<MatchExpression> expressions)
        {
            var queue = new Queue<MatchExpression>();
            var result = default(ListSlim<MatchExpression>?);

            foreach (var expression in expressions)
            {
                queue.Enqueue(expression);
            }

            while (queue.Count > 0)
            {
                var expression = queue.Dequeue();

                if (expression is EmptyMatchExpression)
                {
                    continue;
                }

                if (expression is AndMatchExpression andMatchExpression && andMatchExpression.ScoreAggregation == scoreAggregation)
                {
                    for (var i = 0; i < andMatchExpression.Expressions.Count; i++)
                    {
                        queue.Enqueue(andMatchExpression.Expressions[i]);
                    }

                    continue;
                }

                result ??= new();
                result.Add(expression);
            }

            if (result is not null)
            {
                if (result.Count == 1)
                {
                    return result[0];
                }

                return new AndMatchExpression(scoreAggregation, result);
            }

            return EmptyMatchExpression.Instance;
        }

        private static MatchExpression Or(ScoreAggregation scoreAggregation, ObjectEnumerable<MatchExpression> expressions, bool isLazy)
        {
            var queue = new Queue<MatchExpression>();
            var result = default(ListSlim<MatchExpression>?);

            foreach (var expression in expressions)
            {
                queue.Enqueue(expression);
            }

            while (queue.Count > 0)
            {
                var expression = queue.Dequeue();

                if (expression is EmptyMatchExpression)
                {
                    continue;
                }

                if (expression is OrMatchExpression orMatchExpression && orMatchExpression.ScoreAggregation == scoreAggregation)
                {
                    for (var i = 0; i < orMatchExpression.Expressions.Count; i++)
                    {
                        queue.Enqueue(orMatchExpression.Expressions[i]);
                    }

                    continue;
                }

                result ??= new();
                result.Add(expression);
            }

            if (result is not null)
            {
                if (result.Count == 1)
                {
                    return result[0];
                }

                return new OrMatchExpression(scoreAggregation, result, isLazy);
            }

            return EmptyMatchExpression.Instance;
        }
    }
}
