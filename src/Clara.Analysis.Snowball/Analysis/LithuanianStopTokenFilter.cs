using System.Text;

namespace Clara.Analysis
{
    public sealed class LithuanianStopTokenFilter : ResourceStopTokenFilter
    {
        public LithuanianStopTokenFilter()
            : base(
                  typeof(LithuanianStopTokenFilter).Assembly,
                  $"{typeof(LithuanianStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
