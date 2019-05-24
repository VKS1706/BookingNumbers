using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingNumbers.Tools;

namespace BookingNumbers.Models
{
    // Profile View properties
    public class ProfileViewModel
    {
        public string Email { get; set; }
        public List<FormattedBooking> Bookings { get; set; } // List of bookings for user
        public List<FormattedProject> Projects { get; set; } // List of projects for user
    }
}
