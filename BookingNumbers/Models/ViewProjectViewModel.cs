using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using BookingNumbers.Tools;
using DataCore.Models;

namespace BookingNumbers.Models
{
    // Properties of view project
    public class ViewProjectViewModel
    {
        public Projects Project { get; set; }

        public List<TableProjectUser> ProjectUsers { get; set; } // List of users on the project

        [DisplayName("Hours to Book")]
        public int HoursBooked { get; set; }

        [DisplayName("Minutes to Book")]
        public int MinutesBooked { get; set; }
    }
}
