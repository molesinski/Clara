using System.Text;

namespace Clara.Analysis
{
    public sealed class NorwegianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public NorwegianStopTokenFilter()
            : base(
                  typeof(NorwegianStopTokenFilter).Assembly,
                  $"{typeof(NorwegianStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
