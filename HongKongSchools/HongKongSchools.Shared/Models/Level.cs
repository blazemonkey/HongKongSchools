using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class Level : ModelBase, ILanguage
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "levelId")]
        public int LevelId { get; set; }
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }
    }
}
