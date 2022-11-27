namespace SilverHorseInterviewApi.Models
{
    [JsonSerializable(typeof(Post))]
    internal partial class Post
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

        [JsonPropertyName("body")]
        [JsonPropertyOrder(3)]
        public string Body { get; set; } = string.Empty;
    }
}
