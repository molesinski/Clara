using System.Text;

namespace Clara.Analysis
{
    public sealed class FinnishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public FinnishStopTokenFilter()
            : base(
                  typeof(FinnishStopTokenFilter).Assembly,
                  $"{typeof(FinnishStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
