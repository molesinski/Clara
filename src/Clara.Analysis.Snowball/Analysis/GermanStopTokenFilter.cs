using System.Text;

namespace Clara.Analysis
{
    public sealed class GermanStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public GermanStopTokenFilter()
            : base(
                  typeof(GermanStopTokenFilter).Assembly,
                  $"{typeof(GermanStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
