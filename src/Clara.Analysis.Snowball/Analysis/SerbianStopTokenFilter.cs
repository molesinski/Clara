using System.Text;

namespace Clara.Analysis
{
    public sealed class SerbianStopTokenFilter : ResourceStopTokenFilter
    {
        public SerbianStopTokenFilter()
            : base(
                  typeof(SerbianStopTokenFilter).Assembly,
                  $"{typeof(SerbianStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
