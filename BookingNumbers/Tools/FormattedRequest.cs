using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingNumbers.Tools
{
    // Nice request to be displayed
    public struct FormattedRequest
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public bool Responded { get; set; }
        public bool? Response { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime? DateResponded { get; set; }
        public string RequestType { get; set; } // What kind of request it is
        public int RequestId { get; set; } // The id of the request in the database
    }
}
