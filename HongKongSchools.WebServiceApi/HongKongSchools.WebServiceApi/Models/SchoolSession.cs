using Newtonsoft.Json;

namespace HongKongSchools.WebServiceApi.Models
{
    public class SchoolSession
    {
        public int Id { get; set; }
        [JsonProperty("schoolId")]
        public int SchoolId { get; set; }
        [JsonProperty("sessionId")]
        public int SessionId { get; set; }
    }
}