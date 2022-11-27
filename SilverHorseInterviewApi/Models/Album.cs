namespace SilverHorseInterviewApi.Models
{
    [JsonSerializable(typeof(Album))]
    internal partial class Album
    {
        [JsonPropertyName("id")]
        [JsonPropertyOrder(0)]
        public int Id { get; set; } = 0;

        [JsonPropertyName("name")]
        [JsonPropertyOrder(1)]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        [JsonPropertyOrder(2)]
        public string Title { get; set; } = string.Empty;
    }
}
