using System.Text;

namespace Clara.Analysis
{
    public sealed class ArabicStopTokenFilter : ResourceStopTokenFilter
    {
        public ArabicStopTokenFilter()
            : base(
                  typeof(ArabicStopTokenFilter).Assembly,
                  $"{typeof(ArabicStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
