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
            var errors = new List<School>();

            var id = 1;
            var addressId = 1;

            foreach (var input in inputs)
            {
                if (addresses.Any(x => x.Name == input.EnglishAddress || x.Name == input.ChineseAddress))
                {
                    //var lol = addresses.First(x => x.Name == input.ChineseAddress);
                    continue;
                }

                for (var i = 1; i <= 2; i++)
                {
                    var address = new Address();
                    address.Id = id;
                    address.AddressId = addressId;
                    address.LanguageId = i;
                    if (address.LanguageId == 1)
                        address.Name = input.EnglishAddress;
                    else
                        address.Name = input.ChineseAddress;

                    id++;
                    addresses.Add(address);
                }

                addressId++;                
            }

            id = 1;
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
                try
                {
                    school.AddressId = addresses.First(x => x.Name == input.EnglishAddress).AddressId;
                }
                catch (InvalidOperationException e)
                {
                    errors.Add(school);
                }
                schools.Add(school);
            }

            var addressesJSON = JsonConvert.SerializeObject(addresses);
            File.WriteAllText("addresses.json", addressesJSON, Encoding.Unicode);

            var schoolsJSON = JsonConvert.SerializeObject(schools);
            File.WriteAllText("schools.json", schoolsJSON, Encoding.Unicode);
            return true;
        }

        private static bool ReadBasicInfo()
        {
            return true;
        }
    }
}
