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
        Task<IEnumerable<Category>> GetCategories();
        Task<Category> GetCategoryById(int id);
        Task<IEnumerable<FinanceType>> GetFinanceTypes();
        Task<FinanceType> GetFinanceTypeById(int id);
        Task<IEnumerable<Gender>> GetGenders();
        Task<Gender> GetGenderById(int id);
        Task<IEnumerable<Level>> GetLevels();
        Task<Level> GetLevelById(int id);
        Task<IEnumerable<District>> GetDistricts();
        Task<District> GetDistrictById(int id);
    }
}
