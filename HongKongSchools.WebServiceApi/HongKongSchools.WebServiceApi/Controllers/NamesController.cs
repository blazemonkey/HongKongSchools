using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class NamesController : ApiController
    {
        private readonly IDatabaseService _db;

        public NamesController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SchoolName>> Get()
        {
            return await _db.GetNames();
        }

        public async Task<SchoolName> Get(int id)
        {
            return await _db.GetNameById(id);
        }
    }
}
