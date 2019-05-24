using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingNumbers.Tools;
using DataCore.Models;

namespace BookingNumbers.Models
{
    // Properties for BroadcastList
    public class BroadcastListViewModel
    {
        public List<AuthoredBroadcast> BroadcastsList { get; set; } // List of broadcasts
    }
}
