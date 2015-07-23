using Newtonsoft.Json;

namespace HongKongSchools.WebServiceApi.Models
{
    public class SchoolName
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("nameId")]
        public int NameId { get; set; }
        [JsonProperty("schoolName")]
        public string Name { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }
    }
}