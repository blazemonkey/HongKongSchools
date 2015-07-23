using Newtonsoft.Json;

namespace HongKongSchools.WebServiceApi.Models
{
    public class FinanceType
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("financeTypeId")]
        public int FinanceTypeId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }   
    }
}