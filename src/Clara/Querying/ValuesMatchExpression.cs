using Clara.Utils;

namespace Clara.Querying
{
    public abstract class ValuesMatchExpression : MatchExpression
    {
        private readonly HashSetSlim<string> values;

        internal ValuesMatchExpression(HashSetSlim<string> values)
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
