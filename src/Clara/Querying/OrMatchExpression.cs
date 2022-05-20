using System;
using System.Collections.Generic;

namespace Clara.Querying
{
    public sealed class OrMatchExpression : MatchExpression
    {
        private readonly List<MatchExpression> expressions;

        internal OrMatchExpression(List<MatchExpression> expressions)
        {
            if (expressions is null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            this.expressions = expressions;
        }

        public IEnumerable<MatchExpression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }
    }
}
