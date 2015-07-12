using HongKongSchools.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HongKongSchools.Services.SqlLiteService
{
    public interface ISqlLiteService
    {
        Task<IEnumerable<School>> GetSchools();
        Task<IEnumerable<School>> GetSchoolsByIds(List<int> schoolIds);
        Task<School> GetSchoolById(int id);
        Task<IEnumerable<Address>> GetAddresses();
        Task<Address> GetAddressById(int id);
        Task<IEnumerable<Name>> GetSchoolNames();
        Task<Name> GetSchoolNameById(int id);
        Task<IEnumerable<FinanceType>> GetFinanceTypes();
        Task<FinanceType> GetFinanceTypeById(int id);
        Task<IEnumerable<Gender>> GetGenders();
        Task<Gender> GetGenderById(int id);
        Task<IEnumerable<Level>> GetLevels();
        Task<Level> GetLevelById(int id);
        Task<IEnumerable<District>> GetDistricts();
        Task<District> GetDistrictById(int id);
        Task<IEnumerable<Session>> GetSessions();
        Task<Session> GetSessionById(int id);
        Task<IEnumerable<Language>> GetLanguages();
        Task<Language> GetLanguageById(int id);
    }
}
