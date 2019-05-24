using System;
using System.Collections.Generic;

namespace DataCore.Models
{
    public partial class ProjectMinutesBooked
    {
        public int ProjectMinutesBookedId { get; set; }
        public int AmountOfMinutes { get; set; }
        public DateTime DateOfBooking { get; set; }
        public int UserId { get; set; }
        public int ProjectId { get; set; }

        public Projects Project { get; set; }
        public Users User { get; set; }
    }
}
