using System.IO.Compression;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Clara.Analysis;

namespace Clara.Tools
{
    internal static class Program
    {
        private static int Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Usage: SnowballVerification <snowball-data directory> <embedded algorithms directory>");
                return 2;
            }

            var assembly = typeof(EnglishAnalyzer).Assembly;
            long total = 0;
            long differences = 0;
            long corpusDifferences = 0;
            long tokenDifferences = 0;
            long tokenCount = 0;

            var files = Directory.GetFiles(args[1], "*.generated.cs");
            if (files.Length == 0) { throw new InvalidDataException("No embedded algorithms found."); }

            foreach (var file in files.Order())
            {
                var language = Path.GetFileName(file).Replace("Stemmer.generated.cs", string.Empty);
                var className = Regex.Match(File.ReadAllText(file), @"partial class (\w+)").Groups[1].Value;
                var localType = assembly.GetType("Snowball." + className, throwOnError: true);
                var instance = Activator.CreateInstance(localType);
                var stem = Bind<Func<string, string>>(localType.GetMethod("Stem", new[] { typeof(string) }), instance);
                var setToken = Bind<Action<Token>>(localType.GetMethod("SetBufferContents"), instance);
                var stemBuffer = Bind<Func<bool>>(localType.GetMethod("Stem", Type.EmptyTypes), instance);
                var getBuffer = Bind<Func<StringBuilder>>(localType.GetProperty("Buffer").GetMethod, instance);
                var referenceType = typeof(Snowball.Stemmer).Assembly.GetType("Snowball." + className, throwOnError: true);
                var reference = (Snowball.Stemmer)Activator.CreateInstance(referenceType);
                var token = new Token(new char[Token.MaximumLength], 0);
                long count = 0;
                long failures = 0;

                using var words = ReadLines(Path.Combine(args[0], language, "voc.txt")).GetEnumerator();
                using var outputs = ReadLines(Path.Combine(args[0], language, "output.txt")).GetEnumerator();
                while (words.MoveNext())
                {
                    if (!outputs.MoveNext()) { throw new InvalidDataException(language + ": missing expected output."); }
                    var word = words.Current;
                    var expected = reference.Stem(word);
                    var actual = stem(word);
                    var failed = false;
                    if (actual != expected) { differences++; failed = true; }
                    if (expected != outputs.Current) { corpusDifferences++; failed = true; }

                    // Reuse the same stemmer after its string entry point to exercise full state reset.
                    if (word.Length <= Token.MaximumLength)
                    {
                        token.Set(word);
                        setToken(token);
                        stemBuffer();
                        var buffer = getBuffer();
                        var matches = buffer.Length == expected.Length;
                        for (var i = 0; matches && i < buffer.Length; i++) { matches = buffer[i] == expected[i]; }
                        if (!matches) { tokenDifferences++; failed = true; }
                        tokenCount++;
                    }

                    if (failed && failures++ < 5)
                    {
                        Console.Error.WriteLine($"{language}: {word} -> local={actual}, upstream={expected}, corpus={outputs.Current}");
                    }
                    count++;
                }

                if (outputs.MoveNext()) { throw new InvalidDataException(language + ": extra expected output."); }
                total += count;
                Console.WriteLine($"{language}: {count} words, {failures} mismatches");
            }

            Console.WriteLine($"Total: {total} words; string mismatches: {differences}; token mismatches: {tokenDifferences}/{tokenCount}; corpus mismatches: {corpusDifferences}");
            return differences == 0 && tokenDifferences == 0 && corpusDifferences == 0 ? 0 : 1;
        }

        private static T Bind<T>(MethodInfo method, object instance) where T : Delegate
        {
            return (T)method.CreateDelegate(typeof(T), instance);
        }

        private static IEnumerable<string> ReadLines(string path)
        {
            using var file = File.OpenRead(File.Exists(path) ? path : path + ".gz");
            using var stream = File.Exists(path) ? (Stream)file : new GZipStream(file, CompressionMode.Decompress);
            using var reader = new StreamReader(stream);
            string line;
            while ((line = reader.ReadLine()) != null) { yield return line; }
        }
    }
}
