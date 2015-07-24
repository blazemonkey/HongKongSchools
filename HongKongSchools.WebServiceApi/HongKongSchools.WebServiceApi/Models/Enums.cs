using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HongKongSchools.WebServiceApi.Models
{
    public enum Tables
    {
        Address,
        District,
        FinanceType,
        Gender,
        Level,
        School,
        Name,
        Session
    }

    public enum Types
    {
        Add,
        Delete,
        Update
    }
}