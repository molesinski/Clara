using System.Text;

namespace Clara.Analysis
{
    public sealed class IndonesianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public IndonesianStopTokenFilter()
            : base(
                  typeof(IndonesianStopTokenFilter).Assembly,
                  $"{typeof(IndonesianStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
