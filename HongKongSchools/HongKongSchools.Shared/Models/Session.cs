using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class Session : ModelBase, ILanguage
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "sessionId")]
        public int SessionId { get; set; }
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
