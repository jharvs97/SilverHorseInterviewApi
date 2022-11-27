namespace SilverHorseInterviewApi.Models
{
    [JsonSerializable(typeof(GeoLocation))]
    internal partial class GeoLocation
    {
        [JsonPropertyName("lat")]
        [JsonPropertyOrder(0)]
        public string Lat { get; set; } = string.Empty;

        [JsonPropertyName("lng")]
        [JsonPropertyOrder(1)]
        public string Long { get; set; } = string.Empty;
    }
}
