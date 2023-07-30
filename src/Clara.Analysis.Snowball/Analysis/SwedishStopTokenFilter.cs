using System.Text;

namespace Clara.Analysis
{
    public sealed class SwedishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public SwedishStopTokenFilter()
            : base(
                  typeof(SwedishStopTokenFilter).Assembly,
                  $"{typeof(SwedishStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
