﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class FinanceType : ModelBase, ILanguage
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "financeTypeId")]
        public int FinanceTypeId { get; set; }
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }
    }
}
