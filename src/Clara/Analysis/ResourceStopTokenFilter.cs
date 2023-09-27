using System.Reflection;
using System.Text;

namespace Clara.Analysis
{
    public abstract class ResourceStopTokenFilter : StopTokenFilter
    {
        protected ResourceStopTokenFilter(Type type)
            : base(LoadResource(type))
        {
        }

        protected ResourceStopTokenFilter(Type type, Encoding encoding)
            : base(LoadResource(type, encoding))
        {
        }

        protected ResourceStopTokenFilter(Assembly assembly, string resourceName)
            : base(LoadResource(assembly, resourceName))
        {
        }

        protected ResourceStopTokenFilter(Assembly assembly, string resourceName, Encoding encoding)
            : base(LoadResource(assembly, resourceName, encoding))
        {
        }

        protected static IEnumerable<string> LoadResource(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return LoadResource(type.Assembly, $"{type.FullName}.txt", Encoding.UTF8);
        }

        protected static IEnumerable<string> LoadResource(Type type, Encoding encoding)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            return LoadResource(type.Assembly, $"{type.FullName}.txt", encoding);
        }

        protected static IEnumerable<string> LoadResource(Assembly assembly, string resourceName)
        {
            return LoadResource(assembly, resourceName, Encoding.UTF8);
        }

        protected static IEnumerable<string> LoadResource(Assembly assembly, string resourceName, Encoding encoding)
        {
            if (assembly is null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (resourceName is null)
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            if (encoding is null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream is null)
            {
                throw new InvalidOperationException("Unable to find stopwords resource file in assembly.");
            }

            using var reader = new StreamReader(stream, encoding);

            var stopwords = new HashSet<string>();

            while (reader.ReadLine() is string line)
            {
                if (string.IsNullOrWhiteSpace(line) || line[0] == '#')
                {
                    continue;
                }

                stopwords.Add(line.Trim());
            }

            return stopwords;
        }
    }
}
