using System.Text;

namespace Clara.Analysis.MatchExpressions
{
    public abstract class MatchExpression
    {
        internal MatchExpression()
        {
        }

        public abstract bool IsMatching(IReadOnlyCollection<string> tokens);

        public override string ToString()
        {
            var builder = new StringBuilder();

            this.ToString(builder);

            return builder.ToString();
        }

        internal abstract void ToString(StringBuilder builder);
    }
}
