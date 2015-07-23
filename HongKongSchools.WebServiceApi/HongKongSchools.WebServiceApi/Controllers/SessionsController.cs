using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class SessionsController : ApiController
    {
        private readonly IDatabaseService _db;

        public SessionsController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Session>> Get()
        {
            return await _db.GetSessions();
        }

        public async Task<Session> Get(int id)
        {
            return await _db.GetSessionById(id);
        }
    }
}
