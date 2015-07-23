using System.Collections.Generic;
using System.Threading.Tasks;
using HongKongSchools.WebServiceApi.Models;

namespace HongKongSchools.WebServiceApi.Services.DatabaseService
{
    public interface IDatabaseService
    {
        Task<IEnumerable<Address>> GetAddresses();
        Task<Address> GetAddressById(int id);

        Task<IEnumerable<District>> GetDistricts();
        Task<District> GetDistrictById(int id);

        Task<IEnumerable<FinanceType>> GetFinanceTypes();
        Task<FinanceType> GetFinanceTypeById(int id);

        Task<IEnumerable<Gender>> GetGenders();
        Task<Gender> GetGenderById(int id);

        Task<IEnumerable<Level>> GetLevels();
        Task<Level> GetLevelById(int id);
        Task<bool> UpdateLevel(Level level);

        Task<IEnumerable<SchoolName>> GetNames();
        Task<SchoolName> GetNameById(int id);

        Task<IEnumerable<School>> GetSchools();
        Task<School> GetSchoolById(int id);

        Task<IEnumerable<SchoolSession>> GetSchoolSessions();
        Task<SchoolSession> GetSchoolSessionById(int id);

        Task<IEnumerable<Session>> GetSessions();
        Task<Session> GetSessionById(int id);
    }
}
