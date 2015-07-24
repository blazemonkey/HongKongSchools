using System;
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

        [HttpPut]
        public async Task<bool> Update(Session session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            var result = await _db.UpdateSession(session);
            return result;
        }

        [HttpPost]
        public async Task<bool> Add(Session session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            var result = await _db.AddSession(session);
            return result;
        }
    }
}
