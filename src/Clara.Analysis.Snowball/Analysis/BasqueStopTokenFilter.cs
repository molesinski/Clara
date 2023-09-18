using System.Text;

namespace Clara.Analysis
{
    public sealed class BasqueStopTokenFilter : ResourceStopTokenFilter
    {
        public BasqueStopTokenFilter()
            : base(
                  typeof(BasqueStopTokenFilter).Assembly,
                  $"{typeof(BasqueStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
