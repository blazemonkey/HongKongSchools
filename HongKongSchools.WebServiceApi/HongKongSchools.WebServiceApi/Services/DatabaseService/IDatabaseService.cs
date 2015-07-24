using System.Collections.Generic;
using System.Threading.Tasks;
using HongKongSchools.WebServiceApi.Models;

namespace HongKongSchools.WebServiceApi.Services.DatabaseService
{
    public interface IDatabaseService
    {
        Task<IEnumerable<Address>> GetAddresses();
        Task<Address> GetAddressById(int id);
        Task<bool> UpdateAddress(Address address);
        Task<bool> AddAddress(Address address);

        Task<IEnumerable<District>> GetDistricts();
        Task<District> GetDistrictById(int id);
        Task<bool> UpdateDistrict(District district);
        Task<bool> AddDistrict(District district);

        Task<IEnumerable<FinanceType>> GetFinanceTypes();
        Task<FinanceType> GetFinanceTypeById(int id);
        Task<bool> UpdateFinanceType(FinanceType financeType);
        Task<bool> AddFinanceType(FinanceType financeType);

        Task<IEnumerable<Gender>> GetGenders();
        Task<Gender> GetGenderById(int id);
        Task<bool> UpdateGender(Gender gender);
        Task<bool> AddGender(Gender gender);

        Task<IEnumerable<Level>> GetLevels();
        Task<Level> GetLevelById(int id);
        Task<bool> UpdateLevel(Level level);
        Task<bool> AddLevel(Level level);

        Task<IEnumerable<SchoolName>> GetNames();
        Task<SchoolName> GetNameById(int id);
        Task<bool> UpdateName(SchoolName name);
        Task<bool> AddName(SchoolName name);

        Task<IEnumerable<School>> GetSchools();
        Task<School> GetSchoolById(int id);
        Task<bool> UpdateSchool(School school);
        Task<bool> AddSchool(School school);

        Task<IEnumerable<SchoolSession>> GetSchoolSessions();
        Task<SchoolSession> GetSchoolSessionById(int id);

        Task<IEnumerable<Session>> GetSessions();
        Task<Session> GetSessionById(int id);
        Task<bool> UpdateSession(Session session);
        Task<bool> AddSession(Session session);

        Task<IEnumerable<Change>> GetChanges();
    }
}
