using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class OrMatchExpression : MatchExpression
    {
        internal OrMatchExpression(ScoreAggregation scoreAggregation)
        {
            if (scoreAggregation != ScoreAggregation.Sum && scoreAggregation != ScoreAggregation.Max)
            {
                throw new ArgumentException("Illegal score aggregation enum value.", nameof(scoreAggregation));
            }

            this.ScoreAggregation = scoreAggregation;
        }

        public ScoreAggregation ScoreAggregation { get; }

        public abstract IReadOnlyCollection<MatchExpression> Expressions { get; }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            foreach (var expression in (ListSlim<MatchExpression>)this.Expressions)
            {
                if (expression.IsMatching(tokens))
                {
                    return true;
                }
            }

            return false;
        }

        internal override void ToString(StringBuilder builder)
        {
            builder.Append('(');

            var isFirst = true;

            foreach (var expression in (ListSlim<MatchExpression>)this.Expressions)
            {
                if (!isFirst)
                {
                    builder.Append(" OR ");
                }

                expression.ToString(builder);

                isFirst = false;
            }

            builder.Append(')');
        }

        internal virtual void Discard()
        {
        }
    }

    internal sealed class DisposableOrMatchExpression : OrMatchExpression
    {
        private readonly ObjectPoolLease<ListSlim<MatchExpression>> expressions;
        private bool isDisposed;

        internal DisposableOrMatchExpression(ScoreAggregation scoreAggregation, ObjectPoolLease<ListSlim<MatchExpression>> expressions)
            : base(scoreAggregation)
        {
            this.expressions = expressions;
            this.isDisposed = false;
        }

        public override IReadOnlyCollection<MatchExpression> Expressions
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }

                return this.expressions.Instance;
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
                var expressions = new ListSlim<MatchExpression>();

                foreach (var expression in this.expressions.Instance)
                {
                    expressions.Add(expression.ToPersistent());
                }

                return new PersistentOrMatchExpression(this.ScoreAggregation, expressions);
            }
            finally
            {
                this.Dispose();
            }
        }

        internal override void Discard()
        {
            if (!this.isDisposed)
            {
                this.expressions.Dispose();
                this.isDisposed = true;

                base.Dispose(disposing: true);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                foreach (var expression in this.expressions.Instance)
                {
                    expression.Dispose();
                }

                this.expressions.Dispose();
                this.isDisposed = true;

                base.Dispose(disposing);
            }
        }
    }

    internal sealed class PersistentOrMatchExpression : OrMatchExpression
    {
        private readonly ListSlim<MatchExpression> expressions;

        internal PersistentOrMatchExpression(ScoreAggregation scoreAggregation, ListSlim<MatchExpression> expressions)
            : base(scoreAggregation)
        {
            if (expressions is null)
            {
                throw new ArgumentNullException(nameof(expressions));
            }

            this.expressions = expressions;
        }

        public override IReadOnlyCollection<MatchExpression> Expressions
        {
            get
            {
                return this.expressions;
            }
        }

        public override MatchExpression ToPersistent()
        {
            return this;
        }
    }
}
