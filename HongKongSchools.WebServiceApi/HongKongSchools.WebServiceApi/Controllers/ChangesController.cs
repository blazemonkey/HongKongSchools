using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class ChangesController : ApiController
    {
        private readonly IDatabaseService _db;

        public ChangesController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Change>> Get()
        {
            return await _db.GetChanges();
        }
    }
}
