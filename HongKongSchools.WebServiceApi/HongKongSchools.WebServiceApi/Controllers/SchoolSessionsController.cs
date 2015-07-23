using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class SchoolSessionsController : ApiController
    {
        private readonly IDatabaseService _db;

        public SchoolSessionsController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<SchoolSession>> Get()
        {
            return await _db.GetSchoolSessions();
        }

        public async Task<SchoolSession> Get(int id)
        {
            return await _db.GetSchoolSessionById(id);
        }
    }
}
