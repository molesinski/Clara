#pragma warning disable SA1516 // Elements should be separated by blank line

using System.Text.Json;

namespace Clara.Benchmarks
{
    public sealed class Product
    {
        public static IReadOnlyCollection<Product> Items { get; } = LoadResource(times: 1);

        public static IReadOnlyCollection<Product> Items100 { get; } = LoadResource(times: 100);

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public double? DiscountPercentage { get; set; }
        public double? Rating { get; set; }
        public int? Stock { get; set; }
        public string? Brand { get; set; }
        public string? Category { get; set; }

        private static IReadOnlyCollection<Product> LoadResource(int times = 1)
        {
            var result = new List<Product>();
            var nextId = 1;

            for (var i = 0; i < times; i++)
            {
                using var stream = typeof(Product).Assembly.GetManifestResourceStream($"{typeof(Product).FullName}.json")!;

                var options =
                    new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                    };

                foreach (var item in JsonSerializer.Deserialize<Product[]>(stream, options)!)
                {
                    item.Id = nextId++;

                    result.Add(item);
                }
            }

            return result;
        }
    }
}
