using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models.Bases
{
    public class SchoolSession
    {
        [JsonProperty("schoolId")]
        public int SchoolId { get; set; }
        [JsonProperty("sessionId")]
        public int SessionId { get; set; }
    }
}
