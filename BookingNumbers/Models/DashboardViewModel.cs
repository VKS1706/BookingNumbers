using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Models;

namespace BookingNumbers.Models
{
    // Properties for dashboard
    public class DashboardViewModel
    {
        public List<Projects> UsersProjects { get; set; } // List of projects the user is assigned to
    }
}
