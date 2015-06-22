using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models
{
    public class School
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("schoolNumber")]
        public uint SchoolNumber { get; set; }
        [JsonProperty("provisionalRegistrationDate")]
        public DateTime? ProvisionalRegistrationDate { get; set; }
        [JsonProperty("registrationDate")]
        public DateTime? RegistrationDate { get; set; }
        [JsonProperty("longitude")]
        public string Longitude { get; set; }
        [JsonProperty("latitude")]
        public string Latitude { get; set; }
        [JsonProperty("easting")]
        public string Easting { get; set; }
        [JsonProperty("northing")]
        public string Northing { get; set; }
        [JsonProperty("telephone")]
        public string Telephone { get; set; }
        [JsonProperty("fax")]
        public string Fax { get; set; }
        [JsonProperty("website")]
        public string Website { get; set; }
        [JsonProperty("imagePath")]
        public string ImagePath { get; set; }
        [JsonProperty("addressId")]
        public int AddressId { get; set; }
        [JsonProperty("nameId")]
        public int NameId { get; set; }
        [JsonProperty("districtId")]
        public int DistrictId { get; set; }
        [JsonProperty("financeTypeId")]
        public int FinanceTypeId { get; set; }
        [JsonProperty("genderId")]
        public int GenderId { get; set; }
        [JsonProperty("levelId")]
        public int LevelId { get; set; }
        [JsonProperty("religionId")]
        public int ReligionId { get; set; }
        [JsonProperty("sessionIds")]
        public List<int> SessionIds { get; set; }

        public School()
        {
            SessionIds = new List<int>();
        }
    }
}
