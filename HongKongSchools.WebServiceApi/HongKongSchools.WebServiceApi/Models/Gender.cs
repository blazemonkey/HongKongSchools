using Newtonsoft.Json;

namespace HongKongSchools.WebServiceApi.Models
{
    public class Gender
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("genderId")]
        public int GenderId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }  
    }
}