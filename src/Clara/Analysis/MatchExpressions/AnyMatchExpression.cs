﻿using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class AnyMatchExpression : MatchExpression
    {
        internal AnyMatchExpression(ScoreAggregation scoreAggregation)
        {
            if (scoreAggregation != ScoreAggregation.Sum && scoreAggregation != ScoreAggregation.Max)
            {
                throw new ArgumentException("Illegal score aggregation enum value.", nameof(scoreAggregation));
            }

            this.ScoreAggregation = scoreAggregation;
        }

        public ScoreAggregation ScoreAggregation { get; }

        public abstract IReadOnlyCollection<Token> Tokens { get; }

        public override bool IsMatching(IReadOnlyCollection<Token> tokens)
        {
            foreach (var token in (ListSlim<Token>)this.Tokens)
            {
                if (tokens.Contains(token))
                {
                    return true;
                }
            }

            return false;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append("ANY(");

            var isFirst = true;

            foreach (var token in (ListSlim<Token>)this.Tokens)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append('"');
                builder.Append(token.ToString());
                builder.Append('"');

                isFirst = false;
            }

            builder.Append(')');
        }
    }

    internal sealed class IntermittentAnyMatchExpression : AnyMatchExpression
    {
        private readonly ObjectPoolLease<ListSlim<Token>> tokens;
        private bool isDisposed;

        internal IntermittentAnyMatchExpression(ScoreAggregation scoreAggregation, ObjectPoolLease<ListSlim<Token>> tokens)
            : base(scoreAggregation)
        {
            this.tokens = tokens;
            this.isDisposed = false;
        }

        public override IReadOnlyCollection<Token> Tokens
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.tokens.Instance;
            }
        }

        public override MatchExpression ToPersistent()
        {
            if (this.isDisposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }

            try
            {
                var tokens = new ListSlim<Token>(this.tokens.Instance);

                return new PersistentAnyMatchExpression(this.ScoreAggregation, tokens);
            }
            finally
            {
                this.Dispose();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                this.tokens.Dispose();
                this.isDisposed = true;

                base.Dispose(disposing);
            }
        }
    }

    internal sealed class PersistentAnyMatchExpression : AnyMatchExpression
    {
        private readonly ListSlim<Token> tokens;

        internal PersistentAnyMatchExpression(ScoreAggregation scoreAggregation, ListSlim<Token> tokens)
            : base(scoreAggregation)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            this.tokens = tokens;
        }

        public override IReadOnlyCollection<Token> Tokens
        {
            get
            {
                return this.tokens;
            }
        }

        public override MatchExpression ToPersistent()
        {
            return this;
        }
    }
}
