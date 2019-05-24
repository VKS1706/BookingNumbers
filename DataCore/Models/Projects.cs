using System;
using System.Collections.Generic;

namespace DataCore.Models
{
    public partial class Projects
    {
        public Projects()
        {
            ProjectMinutesBooked = new HashSet<ProjectMinutesBooked>();
            ProjectUsers = new HashSet<ProjectUsers>();
        }

        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public int BookingNumber { get; set; }
        public long MaximumMinutes { get; set; }
        public long CurrentUsedMinutes { get; set; }
        public bool Locked { get; set; }

        public ICollection<ProjectMinutesBooked> ProjectMinutesBooked { get; set; }
        public ICollection<ProjectUsers> ProjectUsers { get; set; }
    }
}
