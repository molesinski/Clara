using System.Text;

namespace Clara.Analysis
{
    public sealed class IrishStopTokenFilter : ResourceStopTokenFilter
    {
        public IrishStopTokenFilter()
            : base(
                  typeof(IrishStopTokenFilter).Assembly,
                  $"{typeof(IrishStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
