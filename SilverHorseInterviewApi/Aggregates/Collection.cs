using SilverHorseInterviewApi.Models;

namespace SilverHorseInterviewApi.Aggregates
{
    [JsonSerializable(typeof(Collection))]
    internal partial class Collection : IAggregate
    {
        [JsonPropertyName("post")]
        [JsonPropertyOrder(0)]
        public Post[] Posts { get; set; } = new Post[] { };

        [JsonPropertyName("album")]
        [JsonPropertyOrder(1)]
        public Album[] Albums { get; set; } = new Album[] { };

        [JsonPropertyName("user")]
        [JsonPropertyOrder(2)]
        public User[] Users { get; set; } = new User[] { };
    }
}
