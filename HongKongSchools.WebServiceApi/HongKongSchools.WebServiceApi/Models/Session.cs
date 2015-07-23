using Newtonsoft.Json;

namespace HongKongSchools.WebServiceApi.Models
{
    public class Session
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("sessionId")]
        public int SessionId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }
    }
}