using System.Text;

namespace Clara.Analysis
{
    public sealed class SpanishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public SpanishStopTokenFilter()
            : base(
                  typeof(SpanishStopTokenFilter).Assembly,
                  $"{typeof(SpanishStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
