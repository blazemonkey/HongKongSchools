using HongKongSchools.DataParser.Helpers;
using HongKongSchools.DataParser.Models.Abstracts;
using System;

namespace HongKongSchools.DataParser.Models
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class SchoolBasicInfo : XMLFile
    {
        public string SchoolNameEng { get; set; }
        public string SchoolNameChi { get; set; }
        public uint SchoolNumber { get; set; }
        public byte LocationId { get; set; }
        public string SchoolLevelEng { get; set; }
        public string SchoolLevelChi { get; set; }
        public string SchoolSessionEng { get; set; }
        public string SchoolSessionChi { get; set; }
        public string StudentGenderEng { get; set; }
        public string StudentGenderChi { get; set; }
        public string DistrictEng { get; set; }
        public string DistrictChi { get; set; }
        public string FinanceTypeEng { get; set; }
        public string FinanceTypeChi { get; set; }
        public string TelephoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string SchoolWebSite { get; set; }
        public string SchoolAddressEng { get; set; }
        public string SchoolAddressChi { get; set; }
        public string LocationMapUrl { get; set; }
        public string GeoInfoMapUrl { get; set; }
        public string RegistrationStatusEng { get; set; }
        public string RegistrationStatusChi { get; set; }
        public string SchoolRegistrationNumber { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public DateTime? ProvisionalRegistrationDate { get; set; }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public DateTime? RegistrationDate { get; set; }

        public override string RootElement
        {
            get { return "Schools"; }
        }

        public override string ObjectElement
        {
            get { return "SchoolBasicInfo"; }
        }

        public override void SetProperty(string column, string value)
        {
            switch (column)
            {
                case "SchoolNameEng":
                    SchoolNameEng = PropertyHelper.SetStringProperty(value);
                    break;
                case "SchoolNameChi":
                    SchoolNameChi = PropertyHelper.SetStringProperty(value);
                    break;
                case "SchoolNumber":
                    SchoolNumber = UInt32.Parse(value.Trim());
                    break;
                case "LocationID":
                    LocationId = Byte.Parse(value.Trim());
                    break;
                case "SchoolLevelEng":
                    SchoolLevelEng = PropertyHelper.SetStringProperty(value);
                    break;
                case "SchoolLevelChi":
                    SchoolLevelChi = PropertyHelper.SetStringProperty(value);
                    break;
                case "SchoolSessionEng":
                    SchoolSessionEng = PropertyHelper.SetStringProperty(value);
                    break;
                case "SchoolSessionChi":
                    SchoolSessionChi = PropertyHelper.SetStringProperty(value);
                    break;
                case "StudentGenderEng":
                    StudentGenderEng = PropertyHelper.SetStringProperty(value);
                    break;
                case "StudentGenderChi":
                    StudentGenderChi = PropertyHelper.SetStringProperty(value);
                    break;
                case "DistrictEng":
                    DistrictEng = PropertyHelper.SetStringProperty(value);
                    break;
                case "DistrictChi":
                    DistrictChi = PropertyHelper.SetStringProperty(value);                    
                    break;
                case "FinanceTypeEng":
                    FinanceTypeEng = PropertyHelper.SetStringProperty(value);
                    break;
                case "FinanceTypeChi":
                    FinanceTypeChi = PropertyHelper.SetStringProperty(value);
                    break;
                case "TelephoneNumber":
                    TelephoneNumber = PropertyHelper.SetStringProperty(value);
                    break;
                case "FaxNumber":
                    FaxNumber = PropertyHelper.SetStringProperty(value);
                    break;
                case "SchoolWebSite":
                    SchoolWebSite = PropertyHelper.SetStringProperty(value);
                    break;
                case "SchoolAddressEng":
                    SchoolAddressEng = PropertyHelper.SetStringProperty(value);
                    break;
                case "SchoolAddressChi":
                    SchoolAddressChi = PropertyHelper.SetStringProperty(value);
                    if (!string.IsNullOrEmpty(SchoolAddressChi))
                        SchoolAddressChi = SchoolAddressChi.Substring(1, SchoolAddressChi.Length - 1);
                    break;
                case "LocationMapUrl":
                    LocationMapUrl = PropertyHelper.SetStringProperty(value);
                    break;
                case "GeoInfoMapUrl":
                    GeoInfoMapUrl = PropertyHelper.SetStringProperty(value);
                    break;
                case "RegistrationStatusEng":
                    RegistrationStatusEng = PropertyHelper.SetStringProperty(value);
                    break;
                case "RegistrationStatusChi":
                    RegistrationStatusChi = PropertyHelper.SetStringProperty(value);
                    break;
                case "SchoolRegistrationNumber":
                    SchoolRegistrationNumber = PropertyHelper.SetStringProperty(value);
                    break;
                case "ProvisionalRegistrationDate":
                    ProvisionalRegistrationDate = PropertyHelper.SetDateTimeProperty(value);
                    break;
                case "RegistrationDate":
                    RegistrationDate = PropertyHelper.SetDateTimeProperty(value);
                    break;
            }
        }
    }
}
