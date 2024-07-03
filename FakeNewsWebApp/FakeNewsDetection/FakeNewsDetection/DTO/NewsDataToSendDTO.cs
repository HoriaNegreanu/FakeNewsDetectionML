using FakeNewsDetection.Entities;
using Newtonsoft.Json;

namespace FakeNewsDetection.DTO
{
    public class NewsDataToSendDTO
    {
        [JsonProperty(PropertyName = "title")]
        public string? Title { get; set; }

        [JsonProperty(PropertyName = "text")]
        public string? Text { get; set; }
    }
}
