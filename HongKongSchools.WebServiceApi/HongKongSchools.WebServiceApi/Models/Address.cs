using Newtonsoft.Json;

namespace HongKongSchools.WebServiceApi.Models
{
    public class Address
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("addressId")]
        public int AddressId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }
    }
}