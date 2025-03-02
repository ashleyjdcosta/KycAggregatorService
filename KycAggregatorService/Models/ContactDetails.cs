
using System.Text.Json.Serialization;
namespace KycAggregatorService.Models
{
    public class ContactDetails
    {
        //Here I am taking an approach of matching the Json property names that could be case sensitive inorder to give us a perfect match with the json file parameters.
        [JsonPropertyName("addresses")]
        public List<Address>? Address { get; set; } //Nullable list
        //Created Lists to enumerate the values for each of the properties. It was shown as objects in json file given hence selected lists.
        [JsonPropertyName("emails")]
        public List<Email>? Emails { get; set; } //Nullable list

        [JsonPropertyName("phone_numbers")]
        public List<PhoneNumber>? PhoneNumbers { get; set; } //Nullable list
    }

    public class Address
    {
        [JsonPropertyName("street")]
        public string Street { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("postal_code")]
        public string PostalCode { get; set; }

        [JsonPropertyName("country")]
        public string Country { get; set; }
    }

    public class Email
    {
        [JsonPropertyName("preferred")]
        public bool Preferred { get; set; }

        [JsonPropertyName("email_address")]
        public string EmailAddress { get; set; }
    }

    public class PhoneNumber
    {
        [JsonPropertyName("preferred")]
        public bool Preferred { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }
    }

}
