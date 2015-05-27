using System;
using System.Collections.Generic;
using System.Text;

namespace HongKongSchools.Models
{
    public class FullSchoolModel
    {
        public School School { get; set; }
        public Address Address { get; set; }
        public Name Name { get; set; }

        public FullSchoolModel()
        {
            School = new School();
            Address = new Address();
            Name = new Name();
        }
    }
}
