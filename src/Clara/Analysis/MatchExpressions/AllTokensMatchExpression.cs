﻿using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public sealed class AllTokensMatchExpression : TokensMatchExpression
    {
        internal AllTokensMatchExpression(ScoreAggregation scoreAggregation, ListSlim<string> tokens)
            : base(scoreAggregation, tokens)
        {
        }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            for (var i = 0; i < this.Tokens.Count; i++)
            {
                if (!tokens.Contains(this.Tokens[i]))
                {
                    return false;
                }
            }

            return true;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("ALL(");

            var isFirst = true;

            for (var i = 0; i < this.Tokens.Count; i++)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append('"');
                builder.Append(this.Tokens[i]);
                builder.Append('"');

                isFirst = false;
            }

            builder.Append(')');
        }
    }
}