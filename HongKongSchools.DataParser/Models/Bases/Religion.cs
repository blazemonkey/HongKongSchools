using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models.Bases
{
    public class Religion : IBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("religionId")]
        public int GroupId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }
    }
}
