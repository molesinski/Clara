using System.Text;

namespace Clara.Analysis
{
    public sealed class HindiStopTokenFilter : ResourceStopTokenFilter
    {
        public HindiStopTokenFilter()
            : base(
                  typeof(HindiStopTokenFilter).Assembly,
                  $"{typeof(HindiStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
