using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingNumbers.Tools
{
    public struct TableProjectUser
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int MinutesBooked { get; set; } // How many minutes the user has booked total on a project
    }
}
