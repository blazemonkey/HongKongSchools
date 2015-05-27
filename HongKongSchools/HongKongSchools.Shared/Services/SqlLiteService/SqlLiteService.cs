using HongKongSchools.Models;
using HongKongSchools.Services.FileReaderService;
using HongKongSchools.Services.JSONService;
using HongKongSchools.Services.SqlLiteService;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.ApplicationModel;
using System.Diagnostics;
using System.Globalization;

namespace HongKongSchools.Services.SqlLiteService
{
    public class SqlLiteService : ISqlLiteService
    {
        private readonly SQLiteAsyncConnection _conn;
        private readonly IFileReaderService _fileReader;
        private readonly IJSONService _json;

        public SqlLiteService(IFileReaderService fileReader, IJSONService json)
        {
            _conn = new SQLiteAsyncConnection("hkschools.db");
            _fileReader = fileReader;
            _json = json;
        }

        public async Task InitDb()
        {
            var createTasks = new Task[]
            {
                _conn.CreateTableAsync<Address>(),
                _conn.CreateTableAsync<Name>(),
                _conn.CreateTableAsync<Category>(),
                _conn.CreateTableAsync<District>(),
                _conn.CreateTableAsync<FinanceType>(),
                _conn.CreateTableAsync<Gender>(),
                _conn.CreateTableAsync<Level>(),
                _conn.CreateTableAsync<Religion>(),
                _conn.CreateTableAsync<School>(),
                _conn.CreateTableAsync<Language>()
            };

            Task.WaitAll(createTasks);
            await InsertDataAsync();
        }

        private async Task InsertDataAsync()
        {
            await InsertSchools();
            await InsertNames();
            await InsertAddresses();
            await InsertCategories();
            await InsertFinanceTypes();
            await InsertGenders();
            await InsertLevels();
            await InsertDistricts();
            await InsertLanguages();
        }

        private async Task InsertSchools()
        {
            if (await _conn.Table<School>().CountAsync() == 0)
            {
                var school = new School
                {
                    Id = 1,
                    NameId = 1,
                    AddressId = 1,
                    CategoryId = 1,
                    ImagePath = "CHAN SUI KI (LA SALLE) PRIMARY SCHOOL.png",
                };

                await _conn.InsertAsync(school);
            }
        }

        private async Task InsertAddresses()
        {
            if (await _conn.Table<Address>().CountAsync() == 0)
            {
                var address = new Address
                {
                    Id = 1,
                    AddressId = 1,
                    LanguageId = 2,
                    Name = "九龍九龍城常盛街22號"
                };

                await _conn.InsertAsync(address);
            }
        }

        private async Task InsertNames()
        {
            if (await _conn.Table<Name>().CountAsync() == 0)
            {
                var name = new Name
                {
                    Id = 1,
                    NameId = 1,
                    LanguageId = 2,
                    SchoolName = "陳瑞祺(喇沙)小學"
                };

                await _conn.InsertAsync(name);
            }
        }

        private async Task InsertCategories()
        {
            if (await _conn.Table<Category>().CountAsync() == 0)
            {
                var categoryJSON = await _fileReader.ReadFile(Package.Current.InstalledLocation, "categories.json");
                var categories = _json.Deserialize<List<Category>>(categoryJSON);
                await _conn.InsertAllAsync(categories);
            }
        }

        private async Task InsertFinanceTypes()
        {
            if (await _conn.Table<FinanceType>().CountAsync() == 0)
            {
                var financeTypeJSON = await _fileReader.ReadFile(Package.Current.InstalledLocation, "finance_types.json");
                var financeTypes = _json.Deserialize<List<FinanceType>>(financeTypeJSON);
                await _conn.InsertAllAsync(financeTypes);
            }
        }

        private async Task InsertGenders()
        {
            if (await _conn.Table<Gender>().CountAsync() == 0)
            {
                var genderJSON = await _fileReader.ReadFile(Package.Current.InstalledLocation, "genders.json");
                var genders = _json.Deserialize<List<Gender>>(genderJSON);
                await _conn.InsertAllAsync(genders);
            }
        }

        private async Task InsertLevels()
        {
            if (await _conn.Table<Level>().CountAsync() == 0)
            {
                var levelJSON = await _fileReader.ReadFile(Package.Current.InstalledLocation, "levels.json");
                var levels = _json.Deserialize<List<Level>>(levelJSON);
                await _conn.InsertAllAsync(levels);
            }
        }

        private async Task InsertDistricts()
        {
            if (await _conn.Table<District>().CountAsync() == 0)
            {
                var districtJSON = await _fileReader.ReadFile(Package.Current.InstalledLocation, "districts.json");
                var districts = _json.Deserialize<List<District>>(districtJSON);
                await _conn.InsertAllAsync(districts);
            }
        }

        private async Task InsertLanguages()
        {
            if (await _conn.Table<Language>().CountAsync() == 0)
            {
                var english = new Language { LanguageId = 1, Name = "English", Culture = "en" };
                var cht = new Language { LanguageId = 2, Name = "繁體中文", Culture = "zh-Hant" };
                var chs = new Language { LanguageId = 3, Name = "简体中文", Culture = "zh-Hans" };

                await _conn.InsertAsync(english);
                await _conn.InsertAsync(cht);
                await _conn.InsertAsync(chs);
            }
        }

        private async Task DoInsertDataAsync<T>()
        {
            var type = typeof(SQLiteAsyncConnection);
            var category = typeof(Category);
            var methods = type.GetRuntimeMethods();
            
            foreach (var method in methods)
            {
                if (method.Name == "Table")
                {
                    var tableMethod = method.MakeGenericMethod(new Type[] { typeof(T) });
                    var a = tableMethod.GetType();
                }
            }
        }

        public async Task<IEnumerable<School>> GetSchools()
        {
            var languageId = await GetCurrentLanguageId();
            var schools =  await _conn.Table<School>().ToListAsync();

            foreach (var school in schools)
            {
                await SetSchoolProperties(school);
            }

            return schools;
        }

        public async Task<School> GetSchoolById(int id)
        {
            try
            {
                var languageId = await GetCurrentLanguageId();
                var school = await _conn.Table<School>().Where(x => x.Id == id).FirstAsync();
                await SetSchoolProperties(school);
                return school;
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine(string.Format("School Id ({0}) could not be found.", id));
                throw ioe;
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("GetSchoolById Error", id));
                throw e;
            }
        }

        private async Task SetSchoolProperties(School school)
        {
            school.Category = await GetCategoryById(school.CategoryId);
            school.Address = await GetAddressById(school.AddressId);
            school.SchoolName = await GetSchoolNameById(school.NameId);            
        }

        public async Task<IEnumerable<Address>> GetAddresses()
        {
            var languageId = await GetCurrentLanguageId();
            return await _conn.Table<Address>().Where(x => x.LanguageId == languageId).ToListAsync();
        }

        public async Task<Address> GetAddressById(int id)
        {
            try
            {
                var languageId = await GetCurrentLanguageId();
                var address = await _conn.Table<Address>().Where(x => x.LanguageId == languageId)
                                                          .Where(x => x.AddressId == id)
                                                          .FirstAsync();
                return address;
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine(string.Format("Address Id ({0}) could not be found.", id));
                throw ioe;
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("GetAddressById Error", id));
                throw e;
            }
        }

        public async Task<IEnumerable<Name>> GetSchoolNames()
        {
            var languageId = await GetCurrentLanguageId();
            return await _conn.Table<Name>().Where(x => x.LanguageId == languageId).ToListAsync();
        }

        public async Task<Name> GetSchoolNameById(int id)
        {
            try
            {
                var languageId = await GetCurrentLanguageId();
                var name = await _conn.Table<Name>().Where(x => x.LanguageId == languageId)
                                                    .Where(x => x.NameId == id)
                                                    .FirstAsync();
                return name;
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine(string.Format("Name Id ({0}) could not be found.", id));
                throw ioe;
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("GetSchoolNameById Error", id));
                throw e;
            }
        }

        public async Task<IEnumerable<Category>> GetCategories()
        {
            var languageId = await GetCurrentLanguageId();
            return await _conn.Table<Category>().Where(x => x.LanguageId == languageId).ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                var languageId = await GetCurrentLanguageId();
                var category = await _conn.Table<Category>().Where(x => x.LanguageId == languageId)
                                                            .Where(x => x.CategoryId == id)
                                                            .FirstAsync();
                return category;
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine(string.Format("Category Id ({0}) could not be found.", id));
                throw ioe;
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("GetCategoryById Error", id));
                throw e;
            }
        }

        public async Task<IEnumerable<FinanceType>> GetFinanceTypes()
        {
            var languageId = await GetCurrentLanguageId();
            return await _conn.Table<FinanceType>().Where(x => x.LanguageId == languageId).ToListAsync();
        }

        public async Task<FinanceType> GetFinanceTypeById(int id)
        {
            try
            {
                var languageId = await GetCurrentLanguageId();
                var financeType = await _conn.Table<FinanceType>().Where(x => x.LanguageId == languageId)
                                                                  .Where(x => x.FinanceTypeId == id)
                                                                  .FirstAsync();
                return financeType;
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine(string.Format("Finance Type Id ({0}) could not be found.", id));
                throw ioe;
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("GetFinanceTypeById Error", id));
                throw e;
            }
        }

        public async Task<IEnumerable<Gender>> GetGenders()
        {
            var languageId = await GetCurrentLanguageId();
            return await _conn.Table<Gender>().Where(x => x.LanguageId == languageId).ToListAsync();
        }

        public async Task<Gender> GetGenderById(int id)
        {
            try
            {
                var languageId = await GetCurrentLanguageId();
                var gender = await _conn.Table<Gender>().Where(x => x.LanguageId == languageId)
                                                        .Where(x => x.GenderId == id)
                                                        .FirstAsync();
                return gender;
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine(string.Format("Gender Id ({0}) could not be found.", id));
                throw ioe;
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("GetGenderById Error", id));
                throw e;
            }
        }

        public async Task<IEnumerable<Level>> GetLevels()
        {
            var languageId = await GetCurrentLanguageId();
            return await _conn.Table<Level>().Where(x => x.LanguageId == languageId).ToListAsync();
        }

        public async Task<Level> GetLevelById(int id)
        {
            try
            {
                var languageId = await GetCurrentLanguageId();
                var level = await _conn.Table<Level>().Where(x => x.LanguageId == languageId)
                                                      .Where(x => x.LevelId == id)
                                                      .FirstAsync();
                return level;
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine(string.Format("Level Id ({0}) could not be found.", id));
                throw ioe;
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("GetLevelById Error", id));
                throw e;
            }
        }

        public async Task<IEnumerable<District>> GetDistricts()
        {
            var languageId = await GetCurrentLanguageId();
            return await _conn.Table<District>().Where(x => x.LanguageId == languageId).ToListAsync();
        }

        public async Task<District> GetDistrictById(int id)
        {
            try
            {
                var languageId = await GetCurrentLanguageId();
                var district = await _conn.Table<District>().Where(x => x.LanguageId == languageId)
                                                            .Where(x => x.DistrictId == id)
                                                            .FirstAsync();
                return district;
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine(string.Format("District Id ({0}) could not be found.", id));
                throw ioe;
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("GetDistrictById Error", id));
                throw e;
            }
        }

        public async Task<IEnumerable<Language>> GetLanguages()
        {
            return await _conn.Table<Language>().ToListAsync();
        }

        public async Task<Language> GetLanguageById(int id)
        {
            try
            {
                var languageId = await GetCurrentLanguageId();
                var language = await _conn.Table<Language>().Where(x => x.LanguageId == languageId)
                                                            .Where(x => x.LanguageId == id)
                                                            .FirstAsync();
                return language;
            }
            catch (InvalidOperationException ioe)
            {
                Debug.WriteLine(string.Format("Language Id ({0}) could not be found.", id));
                throw ioe;
            }
            catch (Exception e)
            {
                Debug.WriteLine(string.Format("GetLanguageById Error", id));
                throw e;
            }
        }

        private async Task<int> GetCurrentLanguageId()
        {
            var languages = await GetLanguages();
            var current = CultureInfo.CurrentCulture.Name;

            foreach (var lang in languages)
            {
                if (lang.Culture == current)
                    return lang.LanguageId;
            }

            // English
            return 1;
        }

        public async Task ClearLocalDb()
        {
            await _conn.DropTableAsync<Address>();                     
            await _conn.DropTableAsync<Religion>();
            await _conn.DropTableAsync<School>();
            await _conn.DropTableAsync<Name>();            
            await InitDb();
        }
    }
}
