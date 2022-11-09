namespace Clara.Querying
{
    public static class Match
    {
        public static MatchExpression Empty()
        {
            return EmptyMatchExpression.Instance;
        }

        public static MatchExpression And(params MatchExpression?[] expressions)
        {
            return And((IEnumerable<MatchExpression?>)expressions);
        }

        public static MatchExpression And(IEnumerable<MatchExpression?>? expressions)
        {
            if (expressions is not null)
            {
                var result = new List<MatchExpression>();
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

                    if (expression is AndMatchExpression andMatchExpression)
                    {
                        foreach (var expression2 in andMatchExpression.Expressions)
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

            return EmptyMatchExpression.Instance;
        }

        public static MatchExpression Or(params MatchExpression?[] expressions)
        {
            return Or((IEnumerable<MatchExpression?>)expressions);
        }

        public static MatchExpression Or(IEnumerable<MatchExpression?>? expressions)
        {
            if (expressions is not null)
            {
                var result = new List<MatchExpression>();
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

                    if (expression is OrMatchExpression orMatchExpression)
                    {
                        foreach (var expression2 in orMatchExpression.Expressions)
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

            return EmptyMatchExpression.Instance;
        }

        public static MatchExpression All(string? value)
        {
            if (value is not null)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return new AllValuesMatchExpression(new List<string> { value });
                }
            }

            return EmptyMatchExpression.Instance;
        }

        public static MatchExpression All(params string?[] values)
        {
            return All((IEnumerable<string?>)values);
        }

        public static MatchExpression All(IEnumerable<string?>? values)
        {
            if (values is not null)
            {
                var result = new List<string>();

                foreach (var value in values)
                {
                    if (value is not null)
                    {
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            result.Add(value);
                        }
                    }
                }

                if (result.Count > 0)
                {
                    return new AllValuesMatchExpression(result);
                }
            }

            return EmptyMatchExpression.Instance;
        }

        public static MatchExpression Any(string? value)
        {
            if (value is not null)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return new AnyValuesMatchExpression(new List<string> { value });
                }
            }

            return EmptyMatchExpression.Instance;
        }

        public static MatchExpression Any(params string?[] values)
        {
            return Any((IEnumerable<string?>)values);
        }

        public static MatchExpression Any(IEnumerable<string?>? values)
        {
            if (values is not null)
            {
                var result = new List<string>();

                foreach (var value in values)
                {
                    if (value is not null)
                    {
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            result.Add(value);
                        }
                    }
                }

                if (result.Count > 0)
                {
                    return new AnyValuesMatchExpression(result);
                }
            }

            return EmptyMatchExpression.Instance;
        }
    }
}
