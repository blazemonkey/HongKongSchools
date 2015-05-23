﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class Address : ModelBase, ILanguage
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "addressId")]
        public int AddressId { get; set; }
        [DataMember(Name = "address")]
        public string AddressName { get; set; }
        [DataMember(Name = "languageId")]
        public int LanguageId { get; set; }
    }
}
