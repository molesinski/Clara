using System.Reflection;
using System.Text;

namespace Clara.Analysis
{
    public abstract class ResourceKeywordTokenFilter : KeywordTokenFilter
    {
        protected ResourceKeywordTokenFilter(IEnumerable<string> keywords)
            : base(keywords)
        {
        }

        protected static IReadOnlyCollection<string> LoadResource(Type type, Encoding? encoding = null)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var assembly = type.Assembly;
            var resourceName = $"{type.FullName}.txt";

            return LoadResource(assembly, resourceName, encoding);
        }

        protected static IReadOnlyCollection<string> LoadResource(Assembly assembly, string resourceName, Encoding? encoding = null)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream is null)
            {
                throw new InvalidOperationException($"Unable to find stopwords resource '{resourceName}' in assembly '{assembly.FullName}'.");
            }

            using var reader = new StreamReader(stream, encoding ?? Encoding.UTF8);

            return Parse(reader);
        }
    }
}
