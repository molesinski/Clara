using System.Text;

namespace Clara.Analysis
{
    public sealed class RussianStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public RussianStopTokenFilter()
            : base(
                  typeof(RussianStopTokenFilter).Assembly,
                  $"{typeof(RussianStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
