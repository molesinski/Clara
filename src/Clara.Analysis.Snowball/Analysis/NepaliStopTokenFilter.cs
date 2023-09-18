using System.Text;

namespace Clara.Analysis
{
    public sealed class NepaliStopTokenFilter : ResourceStopTokenFilter
    {
        public NepaliStopTokenFilter()
            : base(
                  typeof(NepaliStopTokenFilter).Assembly,
                  $"{typeof(NepaliStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
