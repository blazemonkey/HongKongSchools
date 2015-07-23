using System.Collections.Generic;
using System.Data.Entity;
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

        public async Task<IEnumerable<District>> GetDistricts()
        {
            return await _db.Districts.ToListAsync();
        }

        public async Task<District> GetDistrictById(int id)
        {
            return await _db.Districts.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<FinanceType>> GetFinanceTypes()
        {
            return await _db.FinanceTypes.ToListAsync();
        }

        public async Task<FinanceType> GetFinanceTypeById(int id)
        {
            return await _db.FinanceTypes.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Gender>> GetGenders()
        {
            return await _db.Genders.ToListAsync();
        }

        public async Task<Gender> GetGenderById(int id)
        {
            return await _db.Genders.SingleOrDefaultAsync(x => x.Id == id);
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

        public async Task<IEnumerable<School>> GetSchools()
        {
            return await _db.Schools.ToListAsync();
        }

        public async Task<School> GetSchoolById(int id)
        {
            return await _db.Schools.SingleOrDefaultAsync(x => x.Id == id);
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
    }
}