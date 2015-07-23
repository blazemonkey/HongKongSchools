using Newtonsoft.Json;

namespace HongKongSchools.WebServiceApi.Models
{
    public class District
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("districtId")]
        public int DistrictId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }
    }
}