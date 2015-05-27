using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class Name : ModelBase, ILanguage
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "nameId")]
        public int NameId { get; set; }
        [DataMember(Name = "schoolName")]
        public string SchoolName { get; set; }
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }
    }
}
