using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class SchoolBasicInfo
    {
        private string schoolNameEngField;
        private string schoolNameChiField;
        private uint schoolNumberField;
        private byte locationIDField;
        private string schoolLevelEngField;
        private string schoolLevelChiField;
        private string schoolSessionEngField;
        private string schoolSessionChiField;
        private string studentGenderEngField;
        private string studentGenderChiField;
        private string districtEngField;
        private string districtChiField;
        private string financeTypeEngField;
        private string financeTypeChiField;
        private string telephoneNumberField;
        private string faxNumberField;
        private string schoolWebSiteField;
        private string schoolAddressEngField;
        private string schoolAddressChiField;
        private string locationMapUrlField;
        private string geoInfoMapUrlField;
        private string registrationStatusEngField;
        private string registrationStatusChiField;
        private string schoolRegistrationNumberField;
        private System.DateTime? provisionalRegistrationDateField;
        private System.DateTime? registrationDateField;

        /// <remarks/>
        public string SchoolNameEng
        {
            get
            {
                return this.schoolNameEngField;
            }
            set
            {
                this.schoolNameEngField = value;
            }
        }

        /// <remarks/>
        public string SchoolNameChi
        {
            get
            {
                return this.schoolNameChiField;
            }
            set
            {
                this.schoolNameChiField = value;
            }
        }

        /// <remarks/>
        public uint SchoolNumber
        {
            get
            {
                return this.schoolNumberField;
            }
            set
            {
                this.schoolNumberField = value;
            }
        }

        /// <remarks/>
        public byte LocationID
        {
            get
            {
                return this.locationIDField;
            }
            set
            {
                this.locationIDField = value;
            }
        }

        /// <remarks/>
        public string SchoolLevelEng
        {
            get
            {
                return this.schoolLevelEngField;
            }
            set
            {
                this.schoolLevelEngField = value;
            }
        }

        /// <remarks/>
        public string SchoolLevelChi
        {
            get
            {
                return this.schoolLevelChiField;
            }
            set
            {
                this.schoolLevelChiField = value;
            }
        }

        /// <remarks/>
        public string SchoolSessionEng
        {
            get
            {
                return this.schoolSessionEngField;
            }
            set
            {
                this.schoolSessionEngField = value;
            }
        }

        /// <remarks/>
        public string SchoolSessionChi
        {
            get
            {
                return this.schoolSessionChiField;
            }
            set
            {
                this.schoolSessionChiField = value;
            }
        }

        /// <remarks/>
        public string StudentGenderEng
        {
            get
            {
                return this.studentGenderEngField;
            }
            set
            {
                this.studentGenderEngField = value;
            }
        }

        /// <remarks/>
        public string StudentGenderChi
        {
            get
            {
                return this.studentGenderChiField;
            }
            set
            {
                this.studentGenderChiField = value;
            }
        }

        /// <remarks/>
        public string DistrictEng
        {
            get
            {
                return this.districtEngField;
            }
            set
            {
                this.districtEngField = value;
            }
        }

        /// <remarks/>
        public string DistrictChi
        {
            get
            {
                return this.districtChiField;
            }
            set
            {
                this.districtChiField = value;
            }
        }

        /// <remarks/>
        public string FinanceTypeEng
        {
            get
            {
                return this.financeTypeEngField;
            }
            set
            {
                this.financeTypeEngField = value;
            }
        }

        /// <remarks/>
        public string FinanceTypeChi
        {
            get
            {
                return this.financeTypeChiField;
            }
            set
            {
                this.financeTypeChiField = value;
            }
        }

        /// <remarks/>
        public string TelephoneNumber
        {
            get
            {
                return this.telephoneNumberField;
            }
            set
            {
                this.telephoneNumberField = value;
            }
        }

        /// <remarks/>
        public string FaxNumber
        {
            get
            {
                return this.faxNumberField;
            }
            set
            {
                this.faxNumberField = value;
            }
        }

        /// <remarks/>
        public string SchoolWebSite
        {
            get
            {
                return this.schoolWebSiteField;
            }
            set
            {
                this.schoolWebSiteField = value;
            }
        }

        /// <remarks/>
        public string SchoolAddressEng
        {
            get
            {
                return this.schoolAddressEngField;
            }
            set
            {
                this.schoolAddressEngField = value;
            }
        }

        /// <remarks/>
        public string SchoolAddressChi
        {
            get
            {
                return this.schoolAddressChiField;
            }
            set
            {
                this.schoolAddressChiField = value;
            }
        }

        /// <remarks/>
        public string LocationMapUrl
        {
            get
            {
                return this.locationMapUrlField;
            }
            set
            {
                this.locationMapUrlField = value;
            }
        }

        /// <remarks/>
        public string GeoInfoMapUrl
        {
            get
            {
                return this.geoInfoMapUrlField;
            }
            set
            {
                this.geoInfoMapUrlField = value;
            }
        }

        /// <remarks/>
        public string RegistrationStatusEng
        {
            get
            {
                return this.registrationStatusEngField;
            }
            set
            {
                this.registrationStatusEngField = value;
            }
        }

        /// <remarks/>
        public string RegistrationStatusChi
        {
            get
            {
                return this.registrationStatusChiField;
            }
            set
            {
                this.registrationStatusChiField = value;
            }
        }

        /// <remarks/>
        public string SchoolRegistrationNumber
        {
            get
            {
                return this.schoolRegistrationNumberField;
            }
            set
            {
                this.schoolRegistrationNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime? ProvisionalRegistrationDate
        {
            get
            {
                return this.provisionalRegistrationDateField;
            }
            set
            {
                this.provisionalRegistrationDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime? RegistrationDate
        {
            get
            {
                return this.registrationDateField;
            }
            set
            {
                this.registrationDateField = value;
            }
        }
    }
}
