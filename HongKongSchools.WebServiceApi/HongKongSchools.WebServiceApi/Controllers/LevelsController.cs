using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class LevelsController : ApiController
    {
        private readonly IDatabaseService _db;

        public LevelsController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Level>> Get()
        {
            return await _db.GetLevels();
        }

        public async Task<Level> Get(int id)
        {
            return await _db.GetLevelById(id);
        }

        public bool Update(Level level)
        {
            if (level == null)
                throw new ArgumentNullException("level");

            return true;
        }
    }
}
