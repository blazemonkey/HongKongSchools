using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class Category : ModelBase, ILanguage
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "categoryId")]
        public int CategoryId { get; set; }
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }
    }
}
