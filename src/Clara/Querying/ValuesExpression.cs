using Clara.Utils;

namespace Clara.Querying
{
    public abstract class ValuesExpression
    {
        private readonly HashSetSlim<string> values;

        internal ValuesExpression(HashSetSlim<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            this.values = values;
        }

        public IReadOnlyCollection<string> Values
        {
            get
            {
                return this.values;
            }
        }
    }
}
