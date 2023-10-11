﻿using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class AndMatchExpression : ComplexMatchExpression
    {
        internal AndMatchExpression(ScoreAggregation scoreAggregation, ListSlim<MatchExpression> expressions)
            : base(scoreAggregation, expressions)
        {
        }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            for (var i = 0; i < this.Expressions.Count; i++)
            {
                if (!this.Expressions[i].IsMatching(tokens))
                {
                    return false;
                }
            }

            return true;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append('(');

            var isFirst = true;

            for (var i = 0; i < this.Expressions.Count; i++)
            {
                if (!isFirst)
                {
                    builder.Append(" AND ");
                }

                this.Expressions[i].ToString(builder);

                isFirst = false;
            }

            builder.Append(')');
        }
    }
}
