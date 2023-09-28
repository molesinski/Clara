using Clara.Utils;

namespace Clara.Querying
{
    public static class Values
    {
        public static ValuesExpression All(string? value)
        {
            return All(new StringEnumerable(value, trim: true));
        }

        public static ValuesExpression All(params string?[] values)
        {
            return All(new StringEnumerable(values, trim: true));
        }

        public static ValuesExpression All(IEnumerable<string?>? values)
        {
            return All(new StringEnumerable(values, trim: true));
        }

        public static ValuesExpression Any(string? value)
        {
            return Any(new StringEnumerable(value, trim: true));
        }

        public static ValuesExpression Any(params string?[] values)
        {
            return Any(new StringEnumerable(values, trim: true));
        }

        public static ValuesExpression Any(IEnumerable<string?>? values)
        {
            return Any(new StringEnumerable(values, trim: true));
        }

        private static ValuesExpression All(StringEnumerable values)
        {
            var result = default(HashSetSlim<string>);

            foreach (var value in values)
            {
                result ??= new();
                result.Add(value);
            }

            if (result is not null)
            {
                return new AllValuesExpression(result);
            }

            return EmptyValuesExpression.Instance;
        }

        private static ValuesExpression Any(StringEnumerable values)
        {
            var result = default(HashSetSlim<string>);

            foreach (var value in values)
            {
                result ??= new();
                result.Add(value);
            }

            if (result is not null)
            {
                return new AnyValuesExpression(result);
            }

            return EmptyValuesExpression.Instance;
        }
    }
}
