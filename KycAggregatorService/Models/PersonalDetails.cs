using System.Text.Json.Serialization;

namespace KycAggregatorService.Models
{
    public class PersonalDetails
    {
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }

        [JsonPropertyName("sur_name")]
        public string SurName { get; set; }
    }
}
