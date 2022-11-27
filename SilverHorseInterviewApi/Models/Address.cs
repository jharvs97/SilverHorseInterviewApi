namespace SilverHorseInterviewApi.Models
{
    [JsonSerializable(typeof(Address))]
    internal partial class Address
    {
        [JsonPropertyName("street")]
        [JsonPropertyOrder(0)]
        public string Street { get; set; } = string.Empty;

        [JsonPropertyName("suite")]
        [JsonPropertyOrder(1)]
        public string Suite { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        [JsonPropertyOrder(2)]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("zipcode")]
        [JsonPropertyOrder(3)]
        public string ZipCode { get; set; } = string.Empty;

        [JsonPropertyName("geo")]
        [JsonPropertyOrder(4)]
        public GeoLocation GeoLocation { get; set; } = new GeoLocation();
    }
}
