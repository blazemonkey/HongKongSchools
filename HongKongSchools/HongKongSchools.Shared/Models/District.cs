using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class District : ModelBase, ILanguage
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "districtId")]
        public int DistrictId { get; set; }
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }
    }
}
