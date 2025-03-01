namespace KycAggregatorService.Models
{
    public class AggKycData
    {
        public string Ssn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string TaxCountry { get; set; }
        public int? Income { get; set; } //Null values are allowed
    }
}
