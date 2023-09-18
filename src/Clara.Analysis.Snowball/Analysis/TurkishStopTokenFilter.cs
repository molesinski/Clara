using System.Text;

namespace Clara.Analysis
{
    public sealed class TurkishStopTokenFilter : ResourceStopTokenFilter
    {
        public TurkishStopTokenFilter()
            : base(
                  typeof(TurkishStopTokenFilter).Assembly,
                  $"{typeof(TurkishStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
