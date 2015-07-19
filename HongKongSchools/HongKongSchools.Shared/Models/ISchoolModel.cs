using System;
using System.Collections.Generic;
using System.Text;

namespace HongKongSchools.Models
{
    public interface ISchoolModel<T>
    {
        T CreateAnyOption();
    }
}
