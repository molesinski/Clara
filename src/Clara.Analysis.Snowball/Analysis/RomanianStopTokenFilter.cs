using System.Text;

namespace Clara.Analysis
{
    public sealed class RomanianStopTokenFilter : ResourceStopTokenFilter
    {
        public RomanianStopTokenFilter()
            : base(
                  typeof(RomanianStopTokenFilter).Assembly,
                  $"{typeof(RomanianStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
