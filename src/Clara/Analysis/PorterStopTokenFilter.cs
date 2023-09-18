using System.Text;

namespace Clara.Analysis
{
    public sealed class PorterStopTokenFilter : ResourceStopTokenFilter
    {
        public PorterStopTokenFilter()
            : base(
                  typeof(PorterStopTokenFilter).Assembly,
                  $"{typeof(PorterStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
