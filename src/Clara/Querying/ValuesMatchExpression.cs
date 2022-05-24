using System;
using System.Collections.Generic;

namespace Clara.Querying
{
    public abstract class ValuesMatchExpression : MatchExpression
    {
        private readonly HashSet<string> values;

        protected internal ValuesMatchExpression(HashSet<string> values)
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
