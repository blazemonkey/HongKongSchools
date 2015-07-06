using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models.Bases
{
    public class SchoolName : IBase
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("nameId")]
        public int GroupId { get; set; }
        [JsonProperty("schoolName")]
        public string Name { get; set; }
        [JsonProperty("languageId")]
        public int LanguageId { get; set; }
    }
}
