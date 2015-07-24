using System;
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

        [HttpPut]
        public async Task<bool> Update(FinanceType financeType)
        {
            if (financeType == null)
                throw new ArgumentNullException("financeType");

            var result = await _db.UpdateFinanceType(financeType);
            return result;
        }

        [HttpPost]
        public async Task<bool> Add(FinanceType financeType)
        {
            if (financeType == null)
                throw new ArgumentNullException("financeType");

            var result = await _db.AddFinanceType(financeType);
            return result;
        }
    }
}
