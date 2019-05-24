using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingNumbers.Models
{
    // Properties for Create Project
    public class CreateProjectViewModel
    {

        public string ProjectName { get; set; }
        public int ProjectNumber { get; set; }
        public int MaxHours { get; set; } // Hours that can be booked before status is "overbooked"
        public string ProjectDesc { get; set; } // Project Description

    }
}
