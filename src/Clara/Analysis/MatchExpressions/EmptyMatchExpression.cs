﻿using System.Text;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class EmptyMatchExpression : MatchExpression
    {
        private EmptyMatchExpression()
        {
        }

        internal static MatchExpression Instance { get; } = new EmptyMatchExpression();

        public override bool Matches(IReadOnlyCollection<string> tokens)
        {
            return false;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("EMPTY");
        }
    }
}
