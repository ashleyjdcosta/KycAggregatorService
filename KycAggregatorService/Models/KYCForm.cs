using System.Text.Json.Serialization;

namespace KycAggregatorService.Models
{
    public class KycForm
    {
        [JsonPropertyName("items")]
        public List<KycItem> Items { get; set; }
    }

    public class KycItem
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
