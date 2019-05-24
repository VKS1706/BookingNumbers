using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace BookingNumbers.Tools
{
    // Nice class for displaying a booking
    public class FormattedBooking
    {
        public string ProjectBookedTo { get; set; } // The project the minutes will go towards
        public int HoursBooked { get; set; }
        public int MinutesBooked { get; set; }
        public DateTime DateBooked { get; set; } // When the booking was made
    }
}
