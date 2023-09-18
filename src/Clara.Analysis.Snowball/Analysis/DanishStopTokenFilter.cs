using System.Text;

namespace Clara.Analysis
{
    public sealed class DanishStopTokenFilter : SnowballResourceStopTokenFilter
    {
        public DanishStopTokenFilter()
            : base(
                  typeof(DanishStopTokenFilter).Assembly,
                  $"{typeof(DanishStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
