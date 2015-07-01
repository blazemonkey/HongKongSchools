using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HongKongSchools.Models
{
    [DataContract]
    public class SchoolSession : ModelBase
    {
        [DataMember(Name = "schoolId")]
        public int SchoolId { get; set; }
        [DataMember(Name = "sessionId")]
        public int SessionId { get; set; }
    }
}
