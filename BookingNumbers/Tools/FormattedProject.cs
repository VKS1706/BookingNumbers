using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingNumbers.Tools
{
    // Nice class for displayig projects
    public class FormattedProject
    {
        public int ProjectId { get; set; } // The project id from the database
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public int PercentageComplete { get; set; } // The percentage of minutes used from the maximum minutes
        public string LastActivity { get; set; } // When the last booking was
    }
}
