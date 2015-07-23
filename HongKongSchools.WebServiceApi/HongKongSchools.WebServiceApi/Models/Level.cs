using Newtonsoft.Json;

namespace HongKongSchools.WebServiceApi.Models
{
    public class Level
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("levelId")]
        public int LevelId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }
    }
}