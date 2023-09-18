using System.Text;

namespace Clara.Analysis
{
    public sealed class PortugueseStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public PortugueseStopTokenFilter()
            : base(
                  typeof(PortugueseStopTokenFilter).Assembly,
                  $"{typeof(PortugueseStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
