using System.Text;
using Clara.Utils;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class AllMatchExpression : MatchExpression
    {
        internal AllMatchExpression(ScoreAggregation scoreAggregation)
        {
            if (scoreAggregation != ScoreAggregation.Sum && scoreAggregation != ScoreAggregation.Max)
            {
                throw new ArgumentException("Illegal score aggregation enum value.", nameof(scoreAggregation));
            }

            this.ScoreAggregation = scoreAggregation;
        }

        public ScoreAggregation ScoreAggregation { get; }

        public abstract IReadOnlyCollection<string> Tokens { get; }

        public override bool IsMatching(IReadOnlyCollection<string> tokens)
        {
            foreach (var token in (ListSlim<string>)this.Tokens)
            {
                if (!tokens.Contains(token))
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

            foreach (var token in (ListSlim<string>)this.Tokens)
            {
                if (!isFirst)
                {
                    builder.Append(", ");
                }

                builder.Append('"');
                builder.Append(token);
                builder.Append('"');

                isFirst = false;
            }

            builder.Append(')');
        }
    }

    internal sealed class IntermittentAllMatchExpression : AllMatchExpression
    {
        private readonly ObjectPoolLease<ListSlim<string>> tokens;
        private bool isDisposed;

        internal IntermittentAllMatchExpression(ScoreAggregation scoreAggregation, ObjectPoolLease<ListSlim<string>> tokens)
            : base(scoreAggregation)
        {
            this.tokens = tokens;
            this.isDisposed = false;
        }

        public override IReadOnlyCollection<string> Tokens
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
                var tokens = new ListSlim<string>(this.tokens.Instance);

                return new PersistentAllMatchExpression(this.ScoreAggregation, tokens);
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

    internal sealed class PersistentAllMatchExpression : AllMatchExpression
    {
        private readonly ListSlim<string> tokens;

        internal PersistentAllMatchExpression(ScoreAggregation scoreAggregation, ListSlim<string> tokens)
            : base(scoreAggregation)
        {
            if (tokens is null)
            {
                throw new ArgumentNullException(nameof(tokens));
            }

            this.tokens = tokens;
        }

        public override IReadOnlyCollection<string> Tokens
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
