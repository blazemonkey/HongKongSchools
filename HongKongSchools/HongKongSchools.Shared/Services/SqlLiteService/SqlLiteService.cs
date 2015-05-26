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
            await InsertCategories();
            await InsertFinanceTypes();
            await InsertGenders();
            await InsertLevels();
            await InsertDistricts();
            await InsertLanguages();
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

        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await _conn.Table<Category>().ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            try
            {
                var category = await _conn.Table<Category>().Where(x => x.CategoryId == id).FirstAsync();
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
            return await _conn.Table<FinanceType>().ToListAsync();
        }

        public async Task<FinanceType> GetFinanceTypeById(int id)
        {
            try
            {
                var financeType = await _conn.Table<FinanceType>().Where(x => x.FinanceTypeId == id).FirstAsync();
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
            return await _conn.Table<Gender>().ToListAsync();
        }

        public async Task<Gender> GetGenderById(int id)
        {
            try
            {
                var gender = await _conn.Table<Gender>().Where(x => x.GenderId == id).FirstAsync();
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
            return await _conn.Table<Level>().ToListAsync();
        }

        public async Task<Level> GetLevelById(int id)
        {
            try
            {
                var level = await _conn.Table<Level>().Where(x => x.LevelId == id).FirstAsync();
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
            return await _conn.Table<District>().ToListAsync();
        }

        public async Task<District> GetDistrictById(int id)
        {
            try
            {
                var district = await _conn.Table<District>().Where(x => x.DistrictId == id).FirstAsync();
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
                var language = await _conn.Table<Language>().Where(x => x.LanguageId == id).FirstAsync();
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

        public async Task ClearLocalDb()
        {
            await _conn.DropTableAsync<Address>();                     
            await _conn.DropTableAsync<Religion>();
            await _conn.DropTableAsync<School>();
            await _conn.DropTableAsync<Language>();
            await InitDb();
        }
    }
}
