using System.Text;

namespace Clara.Analysis
{
    public sealed class GreekStopTokenFilter : ResourceStopTokenFilter
    {
        public GreekStopTokenFilter()
            : base(
                  typeof(GreekStopTokenFilter).Assembly,
                  $"{typeof(GreekStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
