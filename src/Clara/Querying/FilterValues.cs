using Clara.Utils;

namespace Clara.Querying
{
    internal static class FilterValues
    {
        private static readonly HashSetSlim<string> Empty = new();

        public static HashSetSlim<string> Get(string? value)
        {
            return Get(new StringEnumerable(value, trim: true));
        }

        public static HashSetSlim<string> Get(IEnumerable<string?>? values)
        {
            return Get(new StringEnumerable(values, trim: true));
        }

        private static HashSetSlim<string> Get(StringEnumerable values)
        {
            var result = default(HashSetSlim<string>);

            foreach (var value in values)
            {
                result ??= new();
                result.Add(value);
            }

            if (result is not null)
            {
                return result;
            }

            return Empty;
        }
    }
}
