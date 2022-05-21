using System.Text;

namespace Clara.Analysis
{
    public sealed class EnglishStopTokenFilter : ResourceStopTokenFilter
    {
        public EnglishStopTokenFilter()
            : base(
                  typeof(EnglishStopTokenFilter).Assembly,
                  $"{typeof(EnglishStopTokenFilter).FullName}.txt",
                  Encoding.UTF8)
        {
        }
    }
}
