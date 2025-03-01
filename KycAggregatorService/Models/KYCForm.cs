namespace KycAggregatorService.Models
{
    public class KycForm
    {
        public List<KycItem> Items { get; set; }
    }

    public class KycItem
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
