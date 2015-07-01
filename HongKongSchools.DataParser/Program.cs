﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using HongKongSchools.DataParser.Models;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace HongKongSchools.DataParser
{
    class Program
    {
        static readonly string selectMessage = "Select Option:\n\n"
                                                + "Location and Information (XLSX): 1\n"
                                                + "Registration Basic Info (XML): 2\n"
                                                + "Registration Premises (XML): 3\n"
                                                + "Registration Accomodation (XML): 4\n";
        static readonly string selectedMessage = "Selected Option: {0}";
        static readonly string invalidMessage = "Invalid Option, please try again.";
        static readonly string fileNotFoundMessage = "File ({0}) wasn't found, please try again.";

        static void Main(string[] args)
        {
            var valid = false;
            Console.WriteLine(selectMessage);

            while (!valid)
            {                
                var selected = Console.ReadLine();
                Console.WriteLine(string.Format(selectedMessage, Enum.Parse(typeof(FileEnum), selected, true)));

                if (selected == ((int)FileEnum.LocationAndInformation).ToString())
                {                    
                    valid = ReadLocationAndInformation();
                }
                else if (selected == ((int)FileEnum.RegistrationBasicInfo).ToString())
                {
                    valid = ReadBasicInfo();
                }
                else if (selected == ((int)FileEnum.RegistrationPremises).ToString())
                {
                    valid = true;
                }
                else if (selected == ((int)FileEnum.RegistrationAccomodation).ToString())
                {
                    valid = true;
                }
                else
                {
                    Console.WriteLine(invalidMessage);
                    valid = false;
                }
            }

            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static bool ReadLocationAndInformation()
        {
            var filePath = "Data\\SCH_LOC_EDB.xlsx";
            if (!File.Exists(filePath))
            {
                Console.WriteLine(string.Format(fileNotFoundMessage, filePath));
                return false;
            }

            var inputs = ImportLocationAndInformation(filePath);

            var schools = new List<School>();
            var addresses = new List<Address>();
            var names = new List<SchoolName>();
            var financeTypes = new List<FinanceType>();
            var genders = new List<Gender>();
            var districts = new List<District>();
            var levels = new List<Level>();
            var religions = new List<Religion>();
            PopulateLocationAndInformation(addresses, inputs, x => x.EnglishAddress, x=> x.ChineseAddress);
            PopulateLocationAndInformation(names, inputs, x => x.EnglishName, x=> x.ChineseName);
            PopulateLocationAndInformation(financeTypes, inputs, x => x.EnglishFinanceType, x => x.ChineseFinanceType);
            PopulateLocationAndInformation(genders, inputs, x => x.EnglishGender, x => x.ChineseGender);
            PopulateLocationAndInformation(districts, inputs, x => x.EnglishDistrict, x => x.ChineseDistrict);
            PopulateLocationAndInformation(levels, inputs, x => x.EnglishLevel, x => x.ChineseLevel);
            PopulateLocationAndInformation(religions, inputs, x => x.EnglishReligion, x => x.ChineseReligion);

            var id = 1;
            foreach (var input in inputs)
            {
                var school = new School();
                school.Id = id++;
                school.Latitude = input.EnglishLatitude;
                school.Longitude = input.EnglishLongitude;
                school.Northing = input.EnglishNorthing;
                school.Easting = input.EnglishEasting;
                school.Telephone = input.EnglishTelephone;
                school.Fax = input.EnglishFaxNumber;
                school.Website = input.EnglishWebsite;
                school.AddressId = addresses.First(x => x.Name == input.EnglishAddress).GroupId;
                school.NameId = names.First(x => x.Name == input.EnglishName).GroupId;
                school.FinanceTypeId = financeTypes.First(x => x.Name == input.EnglishFinanceType).GroupId;
                school.GenderId = genders.First(x => x.Name == input.EnglishGender).GroupId;
                school.DistrictId = districts.First(x => x.Name == input.EnglishDistrict).GroupId;
                school.LevelId = levels.First(x => x.Name == input.EnglishLevel).GroupId;
                school.ReligionId = religions.First(x => x.Name == input.EnglishReligion).GroupId;
                schools.Add(school);
            }

            //var addressesJSON = JsonConvert.SerializeObject(addresses);
            //File.WriteAllText("addresses.json", addressesJSON, Encoding.Unicode);

            //var namesJSON = JsonConvert.SerializeObject(names);
            //File.WriteAllText("names.json", namesJSON, Encoding.Unicode);

            //var schoolsJSON = JsonConvert.SerializeObject(schools);
            //File.WriteAllText("schools.json", schoolsJSON, Encoding.Unicode);
            return true;
        }

        private static List<LocationAndInformation> ImportLocationAndInformation(string filePath)
        {
            var inputs = new List<LocationAndInformation>();

            var existingFile = new FileInfo(filePath);
            using (var package = new ExcelPackage(existingFile))
            {
                var workBook = package.Workbook;
                if (workBook != null)
                {
                    if (workBook.Worksheets.Count > 0)
                    {
                        var currentWorksheet = workBook.Worksheets.First();
                        var columnCount = 32;

                        for (var x = 2; x <= 10000; x++)
                        {
                            if (currentWorksheet.Cells[x, 1].Value == null)
                                break;

                            var input = new LocationAndInformation();
                            for (var y = 1; y <= columnCount; y++)
                            {
                                var cell = currentWorksheet.Cells[x, y].Value;
                                var value = cell == null ? "" : cell.ToString();
                                input.SetProperty(y, value);
                            }

                            inputs.Add(input);
                        }
                    }
                }
            }

            return inputs;
        }

        private static void PopulateLocationAndInformation<T>(List<T> collection, List<LocationAndInformation> inputs, Func<LocationAndInformation, string> engProp, Func<LocationAndInformation, string> chiProp) where T : IBase, new()
        {
            var id = 1;
            var groupId = 1;
            
            foreach (var input in inputs)
            {
                if (collection.Any(x => x.Name == engProp.Invoke(input) || x.Name == chiProp.Invoke(input)))
                {
                    continue;
                }

                for (var i = 1; i <= 2; i++)
                {                    
                    var model = new T();
                    model.Id = id;
                    model.GroupId = groupId;
                    model.LanguageId = i;
                    if (model.LanguageId == 1)
                        model.Name = engProp.Invoke(input);
                    else
                        model.Name = chiProp.Invoke(input);

                    id++;
                    collection.Add(model);
                }

                groupId++;
            }
        }

        private static bool ReadBasicInfo()
        {
            var filePath = "Data\\SchoolBasicInfo.xml";
            if (!File.Exists(filePath))
            {
                Console.WriteLine(string.Format(fileNotFoundMessage, filePath));
                return false;
            }

            var schoolBasicInfos = ImportSchoolBasicInfo(filePath);

            var filteredList = schoolBasicInfos.Where(x => x.SchoolLevelEng != "OTHERS" && x.SchoolLevelEng != "POST-SECONDARY").ToList();
            var fullSchools = new List<School>();
            var addresses = new List<Address>();
            var names = new List<SchoolName>();
            var financeTypes = new List<FinanceType>();
            var genders = new List<Gender>();
            var districts = new List<District>();
            var levels = new List<Level>();
            var sessions = new List<Session>();
            var schoolSessions = new List<SchoolSession>();
            PopulateSchoolBasicInfo(addresses, filteredList, x => x.SchoolAddressEng, x => x.SchoolAddressChi);
            PopulateSchoolBasicInfo(names, filteredList, x => x.SchoolNameEng, x => x.SchoolNameChi);
            PopulateSchoolBasicInfo(financeTypes, filteredList, x => x.FinanceTypeEng, x => x.FinanceTypeChi);
            PopulateSchoolBasicInfo(genders, filteredList, x => x.StudentGenderEng, x => x.StudentGenderChi);
            PopulateSchoolBasicInfo(districts, filteredList, x => x.DistrictEng, x => x.DistrictChi);
            PopulateSchoolBasicInfo(levels, filteredList, x => x.SchoolLevelEng, x => x.SchoolLevelChi);
            PopulateSchoolBasicInfo(sessions, filteredList, x => x.SchoolSessionEng, x => x.SchoolSessionChi);
            
            OutputJSONFile<Address>(addresses, "addresses.json");
            OutputJSONFile<SchoolName>(names, "names.json");
            OutputJSONFile<District>(districts, "districts.json");
            OutputJSONFile<FinanceType>(financeTypes, "financeTypes.json");
            OutputJSONFile<Gender>(genders, "genders.json");
            OutputJSONFile<Level>(levels, "levels.json");
            OutputJSONFile<Session>(sessions, "sessions.json");

            var id = 1;
            var collection = filteredList
                .GroupBy(x => new { x.SchoolNumber, x.SchoolAddressEng, x.SchoolLevelEng })
                .Select(group => new School()
                {
                    Id = id++,
                    SchoolNumber = group.Key.SchoolNumber,
                    LevelId = levels.First(x => x.Name == group.Key.SchoolLevelEng).GroupId,
                    AddressId = addresses.First(x => x.Name == group.Key.SchoolAddressEng).GroupId,
                    NameId = names.First(x => x.Name == group.Select(z => z.SchoolNameEng).First()).GroupId,
                    FinanceTypeId = financeTypes.First(x => x.Name == group.Select(z => z.FinanceTypeEng).First()).GroupId,
                    DistrictId = districts.First(x => x.Name == group.Select(z => z.DistrictEng).First()).GroupId,
                    GenderId = genders.First(x => x.Name == group.Select(z => z.StudentGenderEng).First()).GroupId,
                    RegistrationDate = group.Select(x => x.RegistrationDate).First(),
                    ProvisionalRegistrationDate = group.Select(x => x.ProvisionalRegistrationDate).First(),
                    Telephone = group.Select(x => x.TelephoneNumber).First(),
                    Fax = group.Select(x => x.FaxNumber).First(),
                    Website = group.Select(x => x.SchoolWebSite).First(),
                    SessionIds = group.Select(x => sessions.First(z => z.Name == x.SchoolSessionEng).GroupId).ToList()
                }).ToList();

            foreach (var c in collection)
            {
                foreach (var s in c.SessionIds)
                {
                    var schoolSession = new SchoolSession()
                    {
                        SchoolId = c.Id,
                        SessionId = s
                    };

                    schoolSessions.Add(schoolSession);
                }
            }

            var filePathLoc = "Data\\SCH_LOC_EDB.xlsx";
            if (!File.Exists(filePathLoc))
            {
                Console.WriteLine(string.Format(fileNotFoundMessage, filePathLoc));
                return false;
            }
            var locationAndInformation = ImportLocationAndInformation(filePathLoc);
            var unmatchedList = filteredList.Where(x => !locationAndInformation.Exists(z => z.ChineseName.StartsWith(x.SchoolNameChi))).Select(x => x.SchoolNameChi);

            foreach (var school in unmatchedList.Distinct())
            {
                System.Diagnostics.Trace.WriteLine(school);
            }

            var matchedList = filteredList.Where(x => locationAndInformation.Exists(z => z.ChineseName.StartsWith(x.SchoolNameChi))).ToList();


            foreach (var school in matchedList.Distinct())
            {
                var locInfo = locationAndInformation.First(x => x.ChineseName.StartsWith(school.SchoolNameChi));
                var nameId = names.First(x => x.Name == school.SchoolNameChi).GroupId;
                var basicInfo = collection.First(x => x.NameId == nameId);
                basicInfo.Latitude = locInfo.EnglishLatitude;
                basicInfo.Longitude = locInfo.EnglishLongitude;
                basicInfo.Northing = locInfo.EnglishNorthing;
                basicInfo.Easting = locInfo.EnglishEasting;
            }

            var ssJSON = JsonConvert.SerializeObject(schoolSessions);
            File.WriteAllText("school_sessions.json", ssJSON, Encoding.Unicode);

            var schoolsJSON = JsonConvert.SerializeObject(collection);
            File.WriteAllText("schools.json", schoolsJSON, Encoding.Unicode);

            return true;
        }

        private static string SetBasicInfoString(XElement property)
        {
            return property != null ? property.Value.Trim().Normalize(NormalizationForm.FormKC) : "";
        }

        private static DateTime? SetBasicInfoDateTime(XElement property)
        {
            return property != null ? DateTime.Parse(property.Value) : default(DateTime);
        }

        private static List<SchoolBasicInfo> ImportSchoolBasicInfo(string filePath)
        {
            var doc = XDocument.Load(filePath);

            var schools = doc.Element("Schools").Elements("SchoolBasicInfo").ToList();
            var schoolBasicInfos = new List<SchoolBasicInfo>();

            foreach (var school in schools)
            {
                var schoolBasicInfo = new SchoolBasicInfo()
                {
                    SchoolNameEng = SetBasicInfoString(school.Element("SchoolNameEng")),
                    SchoolNameChi = SetBasicInfoString(school.Element("SchoolNameChi")),
                    SchoolNumber = UInt32.Parse(school.Element("SchoolNumber").Value.Trim()),
                    LocationID = Byte.Parse(school.Element("LocationID").Value.Trim()),
                    SchoolLevelEng = SetBasicInfoString(school.Element("SchoolLevelEng")),
                    SchoolLevelChi = SetBasicInfoString(school.Element("SchoolLevelChi")),
                    SchoolSessionEng = SetBasicInfoString(school.Element("SchoolSessionEng")),
                    SchoolSessionChi = SetBasicInfoString(school.Element("SchoolSessionChi")),
                    StudentGenderEng = SetBasicInfoString(school.Element("StudentGenderEng")),
                    StudentGenderChi = SetBasicInfoString(school.Element("StudentGenderChi")),
                    DistrictEng = SetBasicInfoString(school.Element("DistrictEng")),
                    DistrictChi = SetBasicInfoString(school.Element("DistrictChi")),
                    FinanceTypeEng = SetBasicInfoString(school.Element("FinanceTypeEng")),
                    FinanceTypeChi = SetBasicInfoString(school.Element("FinanceTypeChi")),
                    TelephoneNumber = SetBasicInfoString(school.Element("TelephoneNumber")),
                    FaxNumber = SetBasicInfoString(school.Element("FaxNumber")),
                    SchoolWebSite = SetBasicInfoString(school.Element("SchoolWebSite")),
                    SchoolAddressEng = SetBasicInfoString(school.Element("SchoolAddressEng")),
                    SchoolAddressChi = SetBasicInfoString(school.Element("SchoolAddressChi")),
                    LocationMapUrl = SetBasicInfoString(school.Element("LocationMapUrl")),
                    GeoInfoMapUrl = SetBasicInfoString(school.Element("GeoInfoMapUrl")),
                    RegistrationStatusEng = SetBasicInfoString(school.Element("RegistrationStatusEng")),
                    RegistrationStatusChi = SetBasicInfoString(school.Element("RegistrationStatusChi")),
                    SchoolRegistrationNumber = SetBasicInfoString(school.Element("SchoolRegistrationNumber")),
                    RegistrationDate = SetBasicInfoDateTime(school.Element("RegistrationDate")),
                    ProvisionalRegistrationDate = SetBasicInfoDateTime(school.Element("ProvisionalRegistrationDate")),
                };

                schoolBasicInfos.Add(schoolBasicInfo);
            }

            return schoolBasicInfos;
        }

        private static void PopulateSchoolBasicInfo<T>(List<T> collection, List<SchoolBasicInfo> inputs, Func<SchoolBasicInfo, string> engProp, Func<SchoolBasicInfo, string> chiProp) where T : IBase, new()
        {
            var id = 1;
            var groupId = 1;

            foreach (var input in inputs)
            {
                var valid = false;

                for (var i = 1; i <= 2; i++)
                {
                    var model = new T();
                    model.Id = id;
                    model.GroupId = groupId;
                    model.LanguageId = i;
                    if (model.LanguageId == 1)
                        model.Name = engProp.Invoke(input);
                    else
                        model.Name = chiProp.Invoke(input);

                    if (collection.Exists(x => x.Name == model.Name))
                        continue;

                    id++;
                    collection.Add(model);
                    valid = true;
                }
                if (valid)
                    groupId++;
            }
        }

        private static void GetStatisticsOfSchoolBasicInfo(IEnumerable<ConsolidatedSchoolBasicInfo> group)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Number of No School Addresses: {0}", group.Where(x => string.IsNullOrEmpty(x.SchoolAddress)).Count()));
        }

        private static void OutputJSONFile<T>(List<T> collection, string fileName) where T : IBase
        {
            var json = JsonConvert.SerializeObject(collection);
            File.WriteAllText(fileName, json, Encoding.Unicode);
        }
    }
}

