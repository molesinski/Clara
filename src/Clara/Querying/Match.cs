using Clara.Utils;

namespace Clara.Querying
{
    public static class Match
    {
        public static MatchExpression All(string? token)
        {
            return All(new StringEnumerable(token));
        }

        public static MatchExpression All(params string?[] tokens)
        {
            return All(new StringEnumerable(tokens));
        }

        public static MatchExpression All(IEnumerable<string?>? tokens)
        {
            return All(new StringEnumerable(tokens));
        }

        public static MatchExpression Any(string? token)
        {
            return Any(new StringEnumerable(token));
        }

        public static MatchExpression Any(params string?[] tokens)
        {
            return Any(new StringEnumerable(tokens));
        }

        public static MatchExpression Any(IEnumerable<string?>? tokens)
        {
            return Any(new StringEnumerable(tokens));
        }

        public static MatchExpression And(params MatchExpression?[] expressions)
        {
            return And((IEnumerable<MatchExpression?>)expressions);
        }

        public static MatchExpression And(IEnumerable<MatchExpression?>? expressions)
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

                    if (expression is EmptyTokensMatchExpression)
                    {
                        continue;
                    }

                    if (expression is AndMatchExpression andMatchExpression)
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
                    return new AndMatchExpression(result);
                }
            }

            return EmptyTokensMatchExpression.Instance;
        }

        public static MatchExpression Or(params MatchExpression?[] expressions)
        {
            return Or((IEnumerable<MatchExpression?>)expressions);
        }

        public static MatchExpression Or(IEnumerable<MatchExpression?>? expressions)
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

                    if (expression is EmptyTokensMatchExpression)
                    {
                        continue;
                    }

                    if (expression is OrMatchExpression orMatchExpression)
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
                    return new OrMatchExpression(result);
                }
            }

            return EmptyTokensMatchExpression.Instance;
        }

        private static MatchExpression All(StringEnumerable tokens)
        {
            var result = default(ListSlim<string>);

            foreach (var token in tokens)
            {
                result ??= new();
                result.Add(token);
            }

            if (result is not null)
            {
                return new AllTokensMatchExpression(result);
            }

            return EmptyTokensMatchExpression.Instance;
        }

        private static MatchExpression Any(StringEnumerable tokens)
        {
            var result = default(ListSlim<string>);

            foreach (var token in tokens)
            {
                result ??= new();
                result.Add(token);
            }

            if (result is not null)
            {
                return new AnyTokensMatchExpression(result);
            }

            return EmptyTokensMatchExpression.Instance;
        }
    }
}
