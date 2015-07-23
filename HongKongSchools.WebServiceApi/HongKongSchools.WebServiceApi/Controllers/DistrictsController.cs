using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class DistrictsController : ApiController
    {
        private readonly IDatabaseService _db;

        public DistrictsController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<District>> Get()
        {
            return await _db.GetDistricts();
        }

        public async Task<District> Get(int id)
        {
            return await _db.GetDistrictById(id);
        }
    }
}
