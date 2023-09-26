using Clara.Utils;

namespace Clara.Querying
{
    public static class Match
    {
        public static MatchExpression All(string? token)
        {
            if (token is not null)
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    return new AllTokensMatchExpression(new ListSlim<string> { token });
                }
            }

            return EmptyTokensMatchExpression.Instance;
        }

        public static MatchExpression All(params string?[] tokens)
        {
            return All((IEnumerable<string?>)tokens);
        }

        public static MatchExpression All(IEnumerable<string?>? tokens)
        {
            if (tokens is not null)
            {
                var result = new ListSlim<string>();

                foreach (var token in tokens)
                {
                    if (token is not null)
                    {
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            result.Add(token);
                        }
                    }
                }

                if (result.Count > 0)
                {
                    return new AllTokensMatchExpression(result);
                }
            }

            return EmptyTokensMatchExpression.Instance;
        }

        public static MatchExpression Any(string? token)
        {
            if (token is not null)
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    return new AnyTokensMatchExpression(new ListSlim<string> { token });
                }
            }

            return EmptyTokensMatchExpression.Instance;
        }

        public static MatchExpression Any(params string?[] tokens)
        {
            return Any((IEnumerable<string?>)tokens);
        }

        public static MatchExpression Any(IEnumerable<string?>? tokens)
        {
            if (tokens is not null)
            {
                var result = new ListSlim<string>();

                foreach (var token in tokens)
                {
                    if (token is not null)
                    {
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            result.Add(token);
                        }
                    }
                }

                if (result.Count > 0)
                {
                    return new AnyTokensMatchExpression(result);
                }
            }

            return EmptyTokensMatchExpression.Instance;
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
    }
}
