namespace Clara.Querying
{
    public static class Values
    {
        public static ValuesExpression All(string? value)
        {
            if (value is not null)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return new AllValuesExpression(new[] { value });
                }
            }

            return EmptyValuesExpression.Instance;
        }

        public static ValuesExpression All(params string?[] values)
        {
            return All((IEnumerable<string?>)values);
        }

        public static ValuesExpression All(IEnumerable<string?>? values)
        {
            if (values is not null)
            {
                var result = new HashSet<string>();

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
                    return new AllValuesExpression(result);
                }
            }

            return EmptyValuesExpression.Instance;
        }

        public static ValuesExpression Any(string? value)
        {
            if (value is not null)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return new AnyValuesExpression(new[] { value });
                }
            }

            return EmptyValuesExpression.Instance;
        }

        public static ValuesExpression Any(params string?[] values)
        {
            return Any((IEnumerable<string?>)values);
        }

        public static ValuesExpression Any(IEnumerable<string?>? values)
        {
            if (values is not null)
            {
                var result = new HashSet<string>();

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
                    return new AnyValuesExpression(result);
                }
            }

            return EmptyValuesExpression.Instance;
        }
    }
}
