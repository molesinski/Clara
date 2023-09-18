using System.Text;

namespace Clara.Analysis
{
    public sealed class HungarianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public HungarianStopTokenFilter()
            : base(
                  typeof(HungarianStopTokenFilter).Assembly,
                  $"{typeof(HungarianStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
