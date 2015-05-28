using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models
{
    public class School
    {
        public int Id { get; set; }
        public int SchoolNumber { get; set; }
        public DateTime ProvisionalRegistrationDate { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Easting { get; set; }
        public string Northing { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string Website { get; set; }
        public string ImagePath { get; set; }
        public int AddressId { get; set; }
        public int NameId { get; set; }
        public int CategoryId { get; set; }
        public int DistrictId { get; set; }
        public int FinanceTypeId { get; set; }
        public int GenderId { get; set; }
        public int LevelId { get; set; }
        public int ReligionId { get; set; }
    }
}
