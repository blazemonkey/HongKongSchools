using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class GendersController : ApiController
    {
        private readonly IDatabaseService _db;

        public GendersController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Gender>> Get()
        {
            return await _db.GetGenders();
        }

        public async Task<Gender> Get(int id)
        {
            return await _db.GetGenderById(id);
        }

        [HttpPut]
        public async Task<bool> Update(Gender gender)
        {
            if (gender == null)
                throw new ArgumentNullException("gender");

            var result = await _db.UpdateGender(gender);
            return result;
        }

        [HttpPost]
        public async Task<bool> Add(Gender gender)
        {
            if (gender == null)
                throw new ArgumentNullException("gender");

            var result = await _db.AddGender(gender);
            return result;
        }
    }
}
