namespace KycAggregatorService.Models
{
    public class ContactDetails
    {
        public List<Address> Address { get; set; }
        public List<PhoneNumber> PhoneNumbers { get; set; }
        public List<Email> Emails { get; set; }
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
    }

    public class PhoneNumber
    {
        public string Number { get; set; }
    }

    public class Email
    {
        public string EmailAddress { get; set; }
    }
}
