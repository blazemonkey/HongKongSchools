using SQLite;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class School : ModelBase
    {
        [PrimaryKey]
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "schoolNumber")]
        public int SchoolNumber { get; set; }
        [DataMember(Name = "provisionalRegistrationDate")]
        public DateTime ProvisionalRegistrationDate { get; set; }
        [DataMember(Name = "registrationDate")]
        public DateTime RegistrationDate { get; set; }
        [DataMember(Name = "longitude")]
        public string Longitude { get; set; }
        [DataMember(Name = "latitude")]
        public string Latitude { get; set; }
        [DataMember(Name = "easting")]
        public string Easting { get; set; }
        [DataMember(Name = "northing")]
        public string Northing { get; set; }
        [DataMember(Name = "telephone")]
        public string Telephone { get; set; }
        [DataMember(Name = "fax")]
        public string Fax { get; set; }
        [DataMember(Name = "website")]
        public string Website { get; set; }
        [DataMember(Name = "imagePath")]
        public string ImagePath { get; set; }
        [DataMember(Name = "addressId")]
        public int AddressId { get; set; }
        [DataMember(Name = "address")]
        public Address Address { get; set; }
        [DataMember(Name = "nameId")]
        public int NameId { get; set; }
        [DataMember(Name = "schoolName")]
        public Name SchoolName { get; set; }
        [DataMember(Name = "categoryId")]
        public int CategoryId { get; set; }
        [DataMember(Name = "category")]
        public Category Category { get; set; }
        [DataMember(Name = "districtId")]
        public int DistrictId { get; set; }
        [DataMember(Name = "district")]
        public District District { get; set; }
        [DataMember(Name = "financeTypeId")]
        public int FinanceTypeId { get; set; }
        [DataMember(Name = "financeType")]
        public FinanceType FinanceType { get; set; }
        [DataMember(Name = "genderId")]
        public int GenderId { get; set; }
        [DataMember(Name = "gender")]
        public Gender Gender { get; set; }
        [DataMember(Name = "levelId")]
        public int LevelId { get; set; }
        [DataMember(Name = "level")]
        public Level Level { get; set; }
        [DataMember(Name = "religionId")]
        public int ReligionId { get; set; }
        [DataMember(Name = "religion")]
        public Religion Religion { get; set; }
    }
}
