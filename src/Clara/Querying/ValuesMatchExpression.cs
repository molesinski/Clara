using System;
using System.Collections.Generic;

namespace Clara.Querying
{
    public abstract class ValuesMatchExpression : MatchExpression
    {
        private readonly List<string> values;

        protected internal ValuesMatchExpression(List<string> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            this.values = values;
        }

        public IReadOnlyList<string> Values
        {
            get
            {
                return this.values;
            }
        }
    }
}
