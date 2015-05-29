using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;
using System.IO;
using HongKongSchools.DataParser.Models;
using Newtonsoft.Json;

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
                                Console.WriteLine(value);
                                input.SetProperty(y, value);
                            }

                            inputs.Add(input);
                        }
                    }
                }
            }

            var schools = new List<School>();
            var addresses = new List<Address>();
            var names = new List<SchoolName>();
            var categories = new List<Category>();
            var financeTypes = new List<FinanceType>();
            var genders = new List<Gender>();
            PopulateLocationAndInformation(addresses, inputs, x => x.EnglishAddress, x=> x.ChineseAddress);
            PopulateLocationAndInformation(names, inputs, x => x.EnglishName, x=> x.ChineseName);
            PopulateLocationAndInformation(categories, inputs, x => x.EnglishCategory, x => x.ChineseCategory);
            PopulateLocationAndInformation(financeTypes, inputs, x => x.EnglishFinanceType, x => x.ChineseFinanceType);
            PopulateLocationAndInformation(genders, inputs, x => x.EnglishGender, x => x.ChineseGender);

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
                school.CategoryId = categories.First(x => x.Name == input.EnglishCategory).GroupId;
                school.FinanceTypeId = financeTypes.First(x => x.Name == input.EnglishFinanceType).GroupId;
                school.GenderId = genders.First(x => x.Name == input.EnglishGender).GroupId;
                schools.Add(school);
            }

            var addressesJSON = JsonConvert.SerializeObject(addresses);
            File.WriteAllText("addresses.json", addressesJSON, Encoding.Unicode);

            var namesJSON = JsonConvert.SerializeObject(names);
            File.WriteAllText("names.json", namesJSON, Encoding.Unicode);

            var schoolsJSON = JsonConvert.SerializeObject(schools);
            File.WriteAllText("schools.json", schoolsJSON, Encoding.Unicode);
            return true;
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
            return true;
        }
    }
}
