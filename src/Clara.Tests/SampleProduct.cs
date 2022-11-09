using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Clara.Tests
{
    public sealed class SampleProduct
    {
        public static IEnumerable<SampleProduct> Data { get; } = LoadEmbeddedResource();

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("price")]
        public double? Price { get; set; }

        [JsonPropertyName("discountPercentage")]
        public double? DiscountPercentage { get; set; }

        [JsonPropertyName("rating")]
        public double? Rating { get; set; }

        [JsonPropertyName("stock")]
        public int? Stock { get; set; }

        [JsonPropertyName("brand")]
        public string? Brand { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("thumbnail")]
        public string? Thumbnail { get; set; }

        [JsonPropertyName("images")]
        public string[]? Images { get; set; }

        public string GetText()
        {
            var builder = new StringBuilder();

            builder.AppendFormat(CultureInfo.InvariantCulture, "{0}{1}", this.Id, Environment.NewLine);
            builder.AppendLine(this.Title);
            builder.AppendLine(this.Description);
            builder.AppendLine(this.Brand);
            builder.AppendLine(this.Category);

            return builder.ToString();
        }

        private static IEnumerable<SampleProduct> LoadEmbeddedResource()
        {
            //// Data from https://dummyjson.com/products

            using var stream = typeof(SampleProduct).Assembly.GetManifestResourceStream($"{typeof(SampleProduct).FullName}.json");

            if (stream is not null)
            {
                using var reader = new StreamReader(stream, Encoding.UTF8);

                var data = reader.ReadToEnd();
                var result = JsonSerializer.Deserialize<List<SampleProduct>>(data);

                if (result is not null)
                {
                    return result;
                }
            }

            return Enumerable.Empty<SampleProduct>();
        }
    }
}
