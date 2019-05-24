using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingNumbers.Models
{
    // Properties for create request
    public class CreateRequestViewModel
    {
        public List<string> RequestType { get; set; } // List of requests to be picked from

        public string Reason { get; set; }

        public string SelectedType { get; set; } // Selected type from list
    }
}
