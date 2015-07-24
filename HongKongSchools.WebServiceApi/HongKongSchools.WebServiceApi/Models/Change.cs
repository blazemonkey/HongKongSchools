using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HongKongSchools.WebServiceApi.Models
{
    public class Change
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Type { get; set; }
        public int TableId { get; set; }
    }
}