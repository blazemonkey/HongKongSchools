using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using HongKongSchools.WebServiceApi.Models;
using HongKongSchools.WebServiceApi.Services.DatabaseService;

namespace HongKongSchools.WebServiceApi.Controllers
{
    public class AddressesController : ApiController
    {
        private readonly IDatabaseService _db;

        public AddressesController(IDatabaseService db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Address>> Get()
        {
            return await _db.GetAddresses();
        }

        public async Task<Address> Get(int id)
        {
            return await _db.GetAddressById(id);
        }

        [HttpPut]
        public async Task<bool> Update(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            var result = await _db.UpdateAddress(address);
            return result;
        }

        [HttpPost]
        public async Task<bool> Add(Address address)
        {
            if (address == null)
                throw new ArgumentNullException("address");

            var result = await _db.AddAddress(address);
            return result;
        }
    }
}
