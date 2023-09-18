using System.Text;

namespace Clara.Analysis
{
    public sealed class ArmenianStopTokenFilter : ResourceStopTokenFilter
    {
        public ArmenianStopTokenFilter()
            : base(
                  typeof(ArmenianStopTokenFilter).Assembly,
                  $"{typeof(ArmenianStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
