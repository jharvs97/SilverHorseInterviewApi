namespace SilverHorseInterviewApi.Models
{
    [JsonSerializable(typeof(User))]
    internal partial class User
    {
        [JsonPropertyName("id")]
        [JsonPropertyOrder(0)]
        public int Id { get; set; } = 0;

        [JsonPropertyName("name")]
        [JsonPropertyOrder(1)]
        public string Name { get; set; } = string.Empty;
        
        [JsonPropertyName("email")]        
        [JsonPropertyOrder(2)]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        [JsonPropertyOrder(3)]
        public Address Address { get; set; } = new Address();
    }
}
