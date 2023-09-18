using System.Text;

namespace Clara.Analysis
{
    public sealed class TamilStopTokenFilter : ResourceStopTokenFilter
    {
        public TamilStopTokenFilter()
            : base(
                  typeof(TamilStopTokenFilter).Assembly,
                  $"{typeof(TamilStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
