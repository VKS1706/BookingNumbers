using System;
using System.Collections.Generic;

namespace DataCore.Models
{
    public partial class RequestTypes
    {
        public RequestTypes()
        {
            AdminRequests = new HashSet<AdminRequests>();
        }

        public int RequestTypeId { get; set; }
        public string RequestName { get; set; }

        public ICollection<AdminRequests> AdminRequests { get; set; }
    }
}
