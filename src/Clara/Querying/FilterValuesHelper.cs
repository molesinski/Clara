using Clara.Utils;

namespace Clara.Querying
{
    internal static class FilterValuesHelper
    {
        private static readonly HashSetSlim<string> Empty = new();

        public static HashSetSlim<string> GetValues(string? value)
        {
            return GetValues(new StringEnumerable(value, trim: true));
        }

        public static HashSetSlim<string> GetValues(IEnumerable<string?>? values)
        {
            return GetValues(new StringEnumerable(values, trim: true));
        }

        private static HashSetSlim<string> GetValues(StringEnumerable values)
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
