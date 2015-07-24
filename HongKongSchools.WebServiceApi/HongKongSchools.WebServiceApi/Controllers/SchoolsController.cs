using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class SchoolsController : ApiController
    {
        private readonly IDatabaseService _db;

        public SchoolsController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<School>> Get()
        {
            return await _db.GetSchools();
        }

        public async Task<School> Get(int id)
        {
            return await _db.GetSchoolById(id);
        }

        [HttpPut]
        public async Task<bool> Update(School school)
        {
            if (school == null)
                throw new ArgumentNullException("school");

            var result = await _db.UpdateSchool(school);
            return result;
        }

        [HttpPost]
        public async Task<bool> Add(School school)
        {
            if (school == null)
                throw new ArgumentNullException("school");

            var result = await _db.AddSchool(school);
            return result;
        }
    }
}
