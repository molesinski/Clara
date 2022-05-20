using System.Text;

namespace Clara.Analysis
{
    public sealed class EnglishStopTokenFilter : ResourceStopTokenFilter
    {
        public EnglishStopTokenFilter()
            : base(
                  typeof(PolishStopTokenFilter).Assembly,
                  $"{typeof(PolishStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
