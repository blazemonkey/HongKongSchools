using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    public class Language : ILanguage
    {
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "culture")]
        public string Culture { get; set; }
    }
}
