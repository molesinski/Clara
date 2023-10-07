using System.Text;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class MatchExpression : IDisposable
    {
        internal MatchExpression()
        {
        }

        public abstract MatchExpression ToPersistent();

        public abstract bool IsMatching(IReadOnlyCollection<string> tokens);

        public override string ToString()
        {
            var builder = new StringBuilder();

            this.ToString(builder);

            return builder.ToString();
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        internal abstract void ToString(StringBuilder builder);

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
