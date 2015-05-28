using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.DataParser.Models
{
    public class LocationAndInformation
    {
        public string EnglishCategory { get; private set; }
        public string ChineseCategory { get; private set; }
        public string EnglishName { get; private set; }
        public string ChineseName { get; private set; }
        public string EnglishAddress { get; private set; }
        public string ChineseAddress { get; private set; }
        public string EnglishLongitude { get; private set; }
        public string ChineseLongitude { get; private set; }
        public string EnglishLatitude { get; private set; }
        public string ChineseLatitude { get; private set; }
        public string EnglishEasting { get; private set; }
        public string ChineseEasting { get; private set; }
        public string EnglishNorthing { get; private set; }
        public string ChineseNorthing { get; private set; }
        public string EnglishGender { get; private set; }
        public string ChineseGender { get; private set; }
        public string EnglishSession { get; private set; }
        public string ChineseSession { get; private set; }
        public string EnglishDistrict { get; private set; }
        public string ChineseDistrict { get; private set; }
        public string EnglishFinanceType { get; private set; }
        public string ChineseFinanceType { get; private set; }
        public string EnglishLevel { get; private set; }
        public string ChineseLevel { get; private set; }
        public string EnglishTelephone { get; private set; }
        public string ChineseTelephone { get; private set; }
        public string EnglishFaxNumber { get; private set; }
        public string ChineseFaxNumber { get; private set; }
        public string EnglishWebsite { get; private set; }
        public string ChineseWebsite { get; private set; }
        public string EnglishReligion { get; private set; }
        public string ChineseReligion { get; private set; }

        public void SetProperty(int column, string value)
        {
            switch (column)
            {
                case 1:
                    EnglishCategory = value;
                    break;
                case 2:
                    ChineseCategory = value;
                    break;
                case 3:
                    EnglishName = value;
                    break;
                case 4:
                    ChineseName = value;
                    break;
                case 5:
                    EnglishAddress = value;
                    break;
                case 6:
                    ChineseAddress = value;
                    break;
                case 7:
                    EnglishLongitude = value;
                    break;
                case 8:
                    ChineseLongitude = value;
                    break;
                case 9:
                    EnglishLatitude = value;
                    break;
                case 10:
                    ChineseLatitude = value;
                    break;
                case 11:
                    EnglishEasting = value;
                    break;
                case 12:
                    ChineseEasting = value;
                    break;
                case 13:
                    EnglishNorthing = value;
                    break;
                case 14:
                    ChineseNorthing = value;
                    break;
                case 15:
                    EnglishGender = value;
                    break;
                case 16:
                    ChineseGender = value;
                    break;
                case 17:
                    EnglishSession = value;
                    break;
                case 18:
                    ChineseSession = value;
                    break;
                case 19:
                    EnglishDistrict = value;
                    break;
                case 20:
                    ChineseDistrict = value;
                    break;
                case 21:
                    EnglishFinanceType = value;
                    break;
                case 22:
                    ChineseFinanceType = value;
                    break;
                case 23:
                    EnglishLevel = value;
                    break;
                case 24:
                    ChineseLevel = value;
                    break;
                case 25:
                    EnglishTelephone = value;
                    break;
                case 26:
                    ChineseTelephone = value;
                    break;
                case 27:
                    EnglishFaxNumber = value;
                    break;
                case 28:
                    ChineseFaxNumber = value;
                    break;
                case 29:
                    EnglishWebsite = value;
                    break;
                case 30:
                    ChineseWebsite = value;
                    break;
                case 31:
                    EnglishReligion = value;
                    break;
                case 32:
                    ChineseReligion = value;
                    break;
            }

        }
    }
}
