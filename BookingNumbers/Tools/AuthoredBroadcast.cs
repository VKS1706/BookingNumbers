using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Models;

namespace BookingNumbers.Tools
{
    // Broadcast with 'nice' fields for the view model
    public class AuthoredBroadcast
    {
        public Broadcasts Broadcast { get; set; }
        public string Username { get; set; }
    }
}
