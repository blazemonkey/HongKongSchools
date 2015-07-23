using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HongKongSchools.DataParser.Models;
using HongKongSchools.DataParser.Models.Bases;
using HongKongSchools.DataParser.Services.ExcelReaderService;
using HongKongSchools.DataParser.Services.JSONService;
using HongKongSchools.DataParser.Services.WebClientService;
using HongKongSchools.DataParser.Services.XMLReaderService;

namespace HongKongSchools.DataParser
{
    public class Updater
    {
        private const string OrgBasicInfoFilePath = "Data\\SchoolBasicInfo.xml";
        private const string OrgLocInfoFilePath = "Data\\SCH_LOC_EDB.xlsx";

        private readonly IXMLReaderService _xml;
        private readonly IExcelReaderService _excel;
        private readonly IWebClientService _webClient;
        private readonly IJSONService _json;

        private readonly string _yearMonth = string.Format("{0}{1}", DateTime.UtcNow.Year.ToString("0000"), 
                                                           DateTime.UtcNow.Month.ToString("00"));

        public List<Address> Addresses { get; set; }
        public List<District> Districts { get; set; }
        public List<FinanceType> FinanceTypes { get; set; }
        public List<Gender> Genders { get; set; }
        public List<Level> Levels { get; set; }
        public List<School> Schools { get; set; }
        public List<SchoolName> SchoolNames { get; set; }
        public List<SchoolSession> SchoolSessions { get; set; } 
        public List<Session> Sessions { get; set; }

        public Updater(IWebClientService webClient, IXMLReaderService xml, IExcelReaderService excel, IJSONService json)
        {
            _webClient = webClient;
            _xml = xml;
            _excel = excel;
            _json = json;
        }

        public void BeginUpdate()
        {
            var oldSchoolBasicInfos = new List<SchoolBasicInfo>();
            var newSchoolBasicInfos = new List<SchoolBasicInfo>();
            var oldLocationAndInformations = new List<LocationAndInformation>();
            var newLocationAndInformations = new List<LocationAndInformation>();

            var basicInfoFileName = string.Format("{0}{1}{2}", "..\\..\\Data\\Updates\\SchoolBasicInfo_", _yearMonth, ".xml");
            var locInfoFileName = string.Format("{0}{1}{2}", "..\\..\\Data\\Updates\\SCH_LOC_EDB_", _yearMonth, ".xlsx");

            DownloadNewestFiles(basicInfoFileName, locInfoFileName);
            GetCurrentData();
            ParseFiles(ref oldSchoolBasicInfos, ref oldLocationAndInformations, OrgBasicInfoFilePath, locInfoFileName);
            ParseFiles(ref newSchoolBasicInfos, ref newLocationAndInformations, basicInfoFileName, locInfoFileName);

            CompareSchoolBasicInfos(oldSchoolBasicInfos, newSchoolBasicInfos);
        }

        private void DownloadNewestFiles(string basicInfo, string locInfo)
        {
            _webClient.DownloadFile("http://applications.edb.gov.hk/datagovhk/data/SchoolBasicInfo.xml", basicInfo);
            _webClient.DownloadFile("http://www.edb.gov.hk/attachment/en/student-parents/sch-info/sch-search/sch-location-info/SCH_LOC_EDB.xlsx", locInfo);
        }

        private void GetCurrentData()
        {
            var addresses = File.ReadAllText("..\\..\\Data\\Results\\addresses.json");
            var districts = File.ReadAllText("..\\..\\Data\\Results\\districts.json");
            var financeTypes = File.ReadAllText("..\\..\\Data\\Results\\finance_types.json");
            var genders = File.ReadAllText("..\\..\\Data\\Results\\genders.json");
            var levels = File.ReadAllText("..\\..\\Data\\Results\\levels.json");
            var names = File.ReadAllText("..\\..\\Data\\Results\\names.json");
            var schoolSessions = File.ReadAllText("..\\..\\Data\\Results\\school_sessions.json");
            var schools = File.ReadAllText("..\\..\\Data\\Results\\schools.json");
            var sessions = File.ReadAllText("..\\..\\Data\\Results\\sessions.json");

            Addresses = _json.Deserialize<List<Address>>(addresses);
            Districts = _json.Deserialize<List<District>>(districts);
            FinanceTypes = _json.Deserialize<List<FinanceType>>(financeTypes);
            Genders = _json.Deserialize<List<Gender>>(genders);
            Levels = _json.Deserialize<List<Level>>(levels);
            SchoolNames = _json.Deserialize<List<SchoolName>>(names);
            Schools = _json.Deserialize<List<School>>(schools);
            SchoolSessions = _json.Deserialize<List<SchoolSession>>(schoolSessions);
            Sessions = _json.Deserialize<List<Session>>(sessions);
        }

        private void ParseFiles(ref List<SchoolBasicInfo> basicInfos, ref List<LocationAndInformation> locInfos,
            string basicInfoFileName, string locInfoFileName)
        {
            try
            {
                basicInfos = _xml.Read<SchoolBasicInfo>(basicInfoFileName)
                    .Where(x => x.SchoolLevelEng != "OTHERS" && x.SchoolLevelEng != "POST-SECONDARY")
                    .Where(x => x.RegistrationDate != DateTime.MinValue)
                    .OrderBy(x => x.DistrictEng).ThenBy(x => x.SchoolNameEng).ToList();
                locInfos = _excel.Read<LocationAndInformation>(locInfoFileName).ToList();
                
                Parser.ManualUpdatesBeforeParse(basicInfos, locInfos);
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void CompareSchoolBasicInfos(List<SchoolBasicInfo> oldInfos, List<SchoolBasicInfo> newInfos)
        {
            var add = newInfos.Where(x => !oldInfos.Any(z => z.SchoolNumber == x.SchoolNumber && z.SchoolAddressChi == x.SchoolAddressChi
                                       && z.DistrictChi == x.DistrictChi)).ToList();

            var delete = oldInfos.Where(x => !newInfos.Any(z => z.SchoolNumber == x.SchoolNumber && z.SchoolAddressChi == x.SchoolAddressChi
                                       && z.DistrictChi == x.DistrictChi)).ToList();

            var updates = new List<SchoolBasicInfo>();
            foreach (var o in oldInfos)
            {
                var newInfo =
                    newInfos.SingleOrDefault(
                        x => x.SchoolNumber == o.SchoolNumber && x.SchoolAddressChi == o.SchoolAddressChi
                             && x.DistrictChi == o.DistrictChi && x.SchoolLevelChi == o.SchoolLevelChi 
                             && x.FinanceTypeChi == o.FinanceTypeChi && x.SchoolSessionChi == o.SchoolSessionChi);

                if (newInfo == null)
                    return;

                if (newInfo.TelephoneNumber != o.TelephoneNumber || newInfo.FaxNumber != o.FaxNumber
                    || newInfo.ProvisionalRegistrationDate != o.ProvisionalRegistrationDate
                    || newInfo.RegistrationDate != o.RegistrationDate
                    || newInfo.RegistrationStatusChi != o.RegistrationStatusChi
                    || newInfo.StudentGenderChi != o.StudentGenderChi)
                {
                    updates.Add(newInfo);
                }
            }
        }
    }
}
