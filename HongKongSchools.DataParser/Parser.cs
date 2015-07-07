﻿using HongKongSchools.DataParser.Helpers;
using HongKongSchools.DataParser.Models;
using HongKongSchools.DataParser.Models.Bases;
using HongKongSchools.DataParser.Services.ExcelReaderService;
using HongKongSchools.DataParser.Services.JSONService;
using HongKongSchools.DataParser.Services.XMLReaderService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HongKongSchools.DataParser
{
    public class Parser
    {
        private const string BasicInfoFilePath = "Data\\SchoolBasicInfo.xml";
        private const string LocInfoFilePath = "Data\\SCH_LOC_EDB.xlsx";
        private const string ManualUpdatesFilePath = "Data\\ManualUpdates.txt";

        private readonly IXMLReaderService _xml;
        private readonly IExcelReaderService _excel;
        private readonly IJSONService _json;

        public List<Address> Addresses { get; set; }
        public List<District> Districts { get; set; }
        public List<FinanceType> FinanceTypes { get; set; }
        public List<Gender> Genders { get; set; }
        public List<Level> Levels { get; set; }
        public List<SchoolName> SchoolNames { get; set; }
        public List<Session> Sessions { get; set; }

        public Parser(IXMLReaderService xml, IExcelReaderService excel, IJSONService json)
        {
            _xml = xml;
            _excel = excel;
            _json = json;
        }

        public void BeginParse()
        {
            try
            {
                var basicInfos = _xml.Read<SchoolBasicInfo>(BasicInfoFilePath)
                    .Where(x => x.SchoolLevelEng != "OTHERS" && x.SchoolLevelEng != "POST-SECONDARY").ToList();
                var locInfos = _excel.Read<LocationAndInformation>(LocInfoFilePath).ToList();

                AnalyzeBeforeParse(basicInfos, locInfos);
                ManualUpdatesBeforeParse(locInfos);
                OutputBaseJsons(basicInfos);

                var index = 1;
                var schools = new List<School>();

                var groupedInfos = basicInfos
                    .GroupBy(x => new { x.SchoolNumber, x.SchoolAddressEng, x.SchoolLevelEng })
                    .Select(group => new School()
                    {
                        Id = index++,
                        SchoolNumber = group.Key.SchoolNumber,
                        LevelId = Levels.First(x => x.Name == group.Key.SchoolLevelEng).GroupId,
                        AddressId = Addresses.First(x => x.Name == group.Key.SchoolAddressEng).GroupId,
                        NameId = SchoolNames.First(x => x.Name == group.Select(z => z.SchoolNameEng).First()).GroupId,
                        FinanceTypeId = FinanceTypes.First(x => x.Name == group.Select(z => z.FinanceTypeEng).First()).GroupId,
                        DistrictId = Districts.First(x => x.Name == group.Select(z => z.DistrictEng).First()).GroupId,
                        GenderId = Genders.First(x => x.Name == group.Select(z => z.StudentGenderEng).First()).GroupId,
                        RegistrationDate = group.Select(x => x.RegistrationDate).First(),
                        ProvisionalRegistrationDate = group.Select(x => x.ProvisionalRegistrationDate).First(),
                        Telephone = group.Select(x => x.TelephoneNumber).First(),
                        Fax = group.Select(x => x.FaxNumber).First(),
                        Website = group.Select(x => x.SchoolWebSite).First(),
                        SessionIds = group.Select(x => Sessions.First(z => z.Name == x.SchoolSessionEng).GroupId).ToList()
                    }).ToList();


                foreach (var school in groupedInfos)
                {
                    var chiName = SchoolNames.First(x => x.GroupId == school.NameId && x.LanguageId == 2).Name;
                    var engName = SchoolNames.First(x => x.GroupId == school.NameId && x.LanguageId == 1).Name;
                    if (string.IsNullOrEmpty(chiName))
                        chiName = engName;

                    var district = Districts.First(x => x.GroupId == school.DistrictId && x.LanguageId == 2).Name;
                    var level = Levels.First(x => x.GroupId == school.LevelId && x.LanguageId == 2).Name;
                    
                    var locInfo = locInfos.FirstOrDefault(x => (x.ChineseName.StartsWith(chiName)
                        || x.EnglishName.StartsWith(engName)) && x.ChineseDistrict == district
                        && x.ChineseLevel == level);

                    if (locInfo == null)
                    {
                        System.Diagnostics.Trace.WriteLine(string.Format("{0}, {1}, {2}, {3}", level, district, chiName, school.Telephone));
                        continue;
                    }

                    school.Northing = locInfo.EnglishNorthing;
                    school.Easting = locInfo.EnglishEasting;
                    school.Latitude = locInfo.EnglishLatitude;
                    school.Longitude = locInfo.EnglishLongitude;

                    schools.Add(school);
                }

                var json = _json.Serialize(schools);
                File.WriteAllText("schools.json", json, Encoding.Unicode);

                var sessions = (
                    from c in groupedInfos
                    from s in c.SessionIds
                    select new SchoolSession()
                    {
                        SchoolId = c.Id, SessionId = s
                    }).ToList();

                var ssJson = _json.Serialize(sessions);
                File.WriteAllText("school_sessions.json", ssJson, Encoding.Unicode);
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

        private static void AnalyzeBeforeParse(List<SchoolBasicInfo> basicInfos, List<LocationAndInformation> locInfos)
        {
            var sb = new StringBuilder();
            sb.AppendLine("================================================================================");
            sb.AppendLine(string.Format("Number of Schools in SchoolBasicInfo: {0}", basicInfos.Count()));
            sb.AppendLine(string.Format("Number of Schools in Location and Information: {0}", locInfos.Count()));
            sb.AppendLine("");
            sb.AppendLine("================================================================================");
            sb.AppendLine("");

            var existInBasicInfosOnly = basicInfos.Where(x => !locInfos.Any(z => (z.ChineseName.StartsWith(x.SchoolNameChi) 
                || z.EnglishName.StartsWith(x.SchoolNameEng) && z.ChineseDistrict == x.DistrictChi && z.ChineseLevel == x.SchoolLevelChi)))
                .Select(x => new { x.SchoolNameChi, x.SchoolNameEng }).Distinct().ToList();

            sb.AppendLine("================================================================================");
            sb.AppendLine(string.Format("List of Schools that only exist in Basic Infos ({0})", existInBasicInfosOnly.Count()));
            foreach (var bi in existInBasicInfosOnly)
            {
                sb.AppendLine(string.Format("{0}, {1}", bi.SchoolNameChi, bi.SchoolNameEng));
            }

            sb.AppendLine("");
            sb.AppendLine("================================================================================");
            sb.AppendLine("");

            var existInLocInfosOnly = locInfos.Where(x => !basicInfos.Any(z => (z.SchoolNameChi.StartsWith(x.ChineseName)
                || z.SchoolNameEng.StartsWith(x.EnglishName) && z.DistrictChi == x.ChineseDistrict && z.SchoolLevelChi == x.ChineseLevel)))
                .Select(x => new { x.ChineseName, x.EnglishName }).Distinct().ToList();

            sb.AppendLine(string.Format("List of Schools that only exist in Location Infos ({0})", existInLocInfosOnly.Count()));
            foreach (var bi in existInLocInfosOnly)
            {
                sb.AppendLine(string.Format("{0}, {1}", bi.ChineseName, bi.EnglishName));
            }

            File.WriteAllText("analyze.txt", sb.ToString());
        }

        private static void ManualUpdatesBeforeParse(List<LocationAndInformation> locInfos)
        {
            var updates = File.ReadAllLines(ManualUpdatesFilePath);
            var manualUpdates = updates.Select(u => u.Split(',')).Select(split => new ManualUpdate()
            {
                OldName = split[0], NewName = split[1]
            }).ToList();

            foreach (var loc in locInfos.Where(x => manualUpdates.Exists(z => x.ChineseName.StartsWith(z.OldName))))
            {
                loc.ChineseName = manualUpdates.First(x => loc.ChineseName.StartsWith(x.OldName)).NewName;
            }

            foreach (var loc in locInfos.Where(x => string.IsNullOrEmpty(x.ChineseLevel)))
            {
                if (loc.ChineseName.Contains("(小學部)"))
                {
                    loc.ChineseLevel = "小學";
                    loc.EnglishLevel = "PRIMARY";
                }
                else if (loc.ChineseName.Contains("(中學部)"))
                {
                    loc.ChineseLevel = "中學";
                    loc.EnglishLevel = "SECONDARY";
                }
            }
        }

        private void OutputBaseJsons(List<SchoolBasicInfo> basicInfos)
        {
            Addresses = OutputJson<Address>(basicInfos, x => new Tuple<string, string>(x.SchoolAddressEng, x.SchoolAddressChi), "addresses.json");
            Districts = OutputJson<District>(basicInfos, x => new Tuple<string, string>(x.DistrictEng, x.DistrictChi), "districts.json");
            FinanceTypes = OutputJson<FinanceType>(basicInfos, x => new Tuple<string, string>(x.FinanceTypeEng, x.FinanceTypeChi), "finance_types.json");
            Genders = OutputJson<Gender>(basicInfos, x => new Tuple<string, string>(x.StudentGenderEng, x.StudentGenderChi), "genders.json");
            Levels = OutputJson<Level>(basicInfos, x => new Tuple<string, string>(x.SchoolLevelEng, x.SchoolLevelChi), "levels.json");
            SchoolNames = OutputJson<SchoolName>(basicInfos, x => new Tuple<string, string>(x.SchoolNameEng, x.SchoolNameChi), "names.json");
            Sessions = OutputJson<Session>(basicInfos, x => new Tuple<string, string>(x.SchoolSessionEng, x.SchoolSessionChi), "sessions.json");
        }

        private List<T> OutputJson<T>(IEnumerable<SchoolBasicInfo> basicInfos, Func<SchoolBasicInfo, Tuple<string, string>> property, string fileName) where T : IBase, new()
        {
            var bases = basicInfos.GroupBy(property).Select(x => x.Key);
            var list = new List<T>();

            var index = 1;
            var groupId = 1;
            foreach (var b in bases)
            {
                for (var i = 1; i <= 3; i++)
                {
                    var model = new T { Id = index++, LanguageId = i, GroupId = groupId };

                    switch (i)
                    {
                        case 1:
                            model.Name = b.Item1;
                            break;
                        case 2:
                            model.Name = b.Item2;
                            break;
                        case 3:
                            model.Name = ChineseHelper.Trad2Simp(b.Item2);
                            break;
                    }

                    list.Add(model);
                }
                groupId++;
            }

            var json = _json.Serialize(list);
            File.WriteAllText(fileName, json, Encoding.Unicode);

            return list;
        }
    }
}
