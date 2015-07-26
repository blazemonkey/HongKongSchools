using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using HongKongSchools.WebServiceApi.DAL;
using HongKongSchools.WebServiceApi.Models;

namespace HongKongSchools.WebServiceApi.Services.DatabaseService
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SchoolContext _db;

        public DatabaseService()
        {
            _db = new SchoolContext();
        }

        public async Task<IEnumerable<Address>> GetAddresses()
        {
            return await _db.Addresses.ToListAsync();
        }

        public async Task<Address> GetAddressById(int id)
        {
            return await _db.Addresses.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateAddress(Address address)
        {
            var find = await _db.Addresses.FindAsync(address.Id);
            if (find == null)
                return false;

            find.Name = address.Name;
            AddChanges(Tables.Address, Types.Update, address.Id);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddAddress(Address address)
        {
            var find = await _db.Addresses.FindAsync(address.Id);
            if (find != null)
                return false;

            _db.Addresses.Add(address);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<District>> GetDistricts()
        {
            return await _db.Districts.ToListAsync();
        }

        public async Task<District> GetDistrictById(int id)
        {
            return await _db.Districts.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateDistrict(District district)
        {
            var find = await _db.Districts.FindAsync(district.Id);
            if (find == null)
                return false;

            find.Name = district.Name;
            AddChanges(Tables.District, Types.Update, district.Id);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddDistrict(District district)
        {
            var find = await _db.Districts.FindAsync(district.Id);
            if (find != null)
                return false;

            _db.Districts.Add(district);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<FinanceType>> GetFinanceTypes()
        {
            return await _db.FinanceTypes.ToListAsync();
        }

        public async Task<FinanceType> GetFinanceTypeById(int id)
        {
            return await _db.FinanceTypes.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateFinanceType(FinanceType financeType)
        {
            var find = await _db.FinanceTypes.FindAsync(financeType.Id);
            if (find == null)
                return false;

            find.Name = financeType.Name;
            AddChanges(Tables.FinanceType, Types.Update, financeType.Id);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddFinanceType(FinanceType financeType)
        {
            var find = await _db.FinanceTypes.FindAsync(financeType.Id);
            if (find != null)
                return false;

            _db.FinanceTypes.Add(financeType);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Gender>> GetGenders()
        {
            return await _db.Genders.ToListAsync();
        }

        public async Task<Gender> GetGenderById(int id)
        {
            return await _db.Genders.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateGender(Gender gender)
        {
            var find = await _db.Genders.FindAsync(gender.Id);
            if (find == null)
                return false;

            find.Name = gender.Name;
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddGender(Gender gender)
        {
            var find = await _db.Genders.FindAsync(gender.Id);
            if (find != null)
                return false;

            _db.Genders.Add(gender);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Level>> GetLevels()
        {
            return await _db.Levels.ToListAsync();
        }

        public async Task<Level> GetLevelById(int id)
        {
            return await _db.Levels.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateLevel(Level level)
        {
            var find = await _db.Levels.FindAsync(level.Id);
            if (find == null)
                return false;

            find.Name = level.Name;
            AddChanges(Tables.Level, Types.Update, level.Id);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddLevel(Level level)
        {
            var find = await _db.Levels.FindAsync(level.Id);
            if (find != null)
                return false;

            _db.Levels.Add(level);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SchoolName>> GetNames()
        {
            return await _db.Names.ToListAsync();
        }

        public async Task<SchoolName> GetNameById(int id)
        {
            return await _db.Names.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateName(SchoolName name)
        {
            var find = await _db.Names.FindAsync(name.Id);
            if (find == null)
                return false;

            find.Name = name.Name;
            AddChanges(Tables.Name, Types.Update, name.Id);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddName(SchoolName name)
        {
            var find = await _db.Names.FindAsync(name.Id);
            if (find != null)
                return false;

            _db.Names.Add(name);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<School>> GetSchools()
        {
            return await _db.Schools.ToListAsync();
        }

        public async Task<School> GetSchoolById(int id)
        {
            return await _db.Schools.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateSchool(School school)
        {
            var find = await _db.Schools.FindAsync(school.Id);
            if (find == null)
                return false;

            find.AddressId = school.AddressId;
            find.DistrictId = school.DistrictId;
            find.Easting = school.Easting;
            find.Fax = school.Fax;
            find.FinanceTypeId = school.FinanceTypeId;
            find.GenderId = school.GenderId;
            find.ImagePath = school.ImagePath;
            find.Latitude = school.Latitude;
            find.LevelId = school.LevelId;
            find.Longitude = school.Longitude;
            find.NameId = school.NameId;
            find.Northing = school.Northing;
            find.ProvisionalRegistrationDate = school.ProvisionalRegistrationDate;
            find.RegistrationDate = school.RegistrationDate;
            find.ReligionId = school.ReligionId;
            find.SchoolNumber = school.SchoolNumber;
            find.SessionIds = school.SessionIds;
            find.Telephone = school.Telephone;
            find.Website = school.Website;

            AddChanges(Tables.School, Types.Update, school.Id);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddSchool(School school)
        {
            var find = await _db.Schools.FindAsync(school.Id);
            if (find != null)
                return false;

            _db.Schools.Add(school);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<SchoolSession>> GetSchoolSessions()
        {
            return await _db.SchoolSessions.ToListAsync();
        }

        public async Task<SchoolSession> GetSchoolSessionById(int id)
        {
            return await _db.SchoolSessions.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Session>> GetSessions()
        {
            return await _db.Sessions.ToListAsync();
        }

        public async Task<Session> GetSessionById(int id)
        {
            return await _db.Sessions.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> UpdateSession(Session session)
        {
            var find = await _db.Sessions.FindAsync(session.Id);
            if (find == null)
                return false;

            find.Name = session.Name;
            AddChanges(Tables.Session, Types.Update, session.Id);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddSession(Session session)
        {
            var find = await _db.Sessions.FindAsync(session.Id);
            if (find != null)
                return false;
            
            _db.Sessions.Add(session);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Change>> GetChanges()
        {
            return await _db.Changes.ToListAsync();
        }

        private void AddChanges(Tables tableName, Types type, int tableId)
        {
            var newChange = new Change()
            {
                TableId = tableId,
                TableName = tableName.ToString(),
                Type = type.ToString()
            };

            _db.Changes.Add(newChange);
        }
    }
}