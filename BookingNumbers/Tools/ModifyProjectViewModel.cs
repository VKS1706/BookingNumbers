using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BookingNumbers.Tools
{
    // View model for the modify project view
    public class ModifyProjectViewModel
    {
        [DisplayName("Project Title")]
        public string Title { get; set; }
        [DisplayName("Description")]
        public string Body { get; set; }
        [DisplayName("Booking Number")]
        public int BookingNumber { get; set; }
        
    }
}
