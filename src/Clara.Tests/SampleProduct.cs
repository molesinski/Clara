#pragma warning disable SA1516 // Elements should be separated by blank line

using System.Text.Json;

namespace Clara.Tests
{
    public sealed class SampleProduct
    {
        public static IReadOnlyCollection<SampleProduct> Items { get; } = LoadResource();

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public double? DiscountPercentage { get; set; }
        public double? Rating { get; set; }
        public int? Stock { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }

        private static SampleProduct[] LoadResource()
        {
            using var stream = typeof(SampleProduct).Assembly.GetManifestResourceStream($"{typeof(SampleProduct).FullName}.json")!;

            var options =
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                };

            return JsonSerializer.Deserialize<SampleProduct[]>(stream, options)!;
        }
    }
}
