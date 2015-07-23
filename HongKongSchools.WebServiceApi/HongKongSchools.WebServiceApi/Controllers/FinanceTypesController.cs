using HongKongSchools.WebServiceApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class FinanceTypesController : ApiController
    {
        private readonly IDatabaseService _db;

        public FinanceTypesController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<FinanceType>> Get()
        {
            return await _db.GetFinanceTypes();
        }

        public async Task<FinanceType> Get(int id)
        {
            return await _db.GetFinanceTypeById(id);
        }
    }
}
