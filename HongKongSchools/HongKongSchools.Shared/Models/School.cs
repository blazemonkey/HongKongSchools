using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class School : ModelBase
    {
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
    }
}
