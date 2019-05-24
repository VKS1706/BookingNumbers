using System;
using System.Collections.Generic;

namespace DataCore.Models
{
    public partial class Users
    {
        public Users()
        {
            AdminRequestsRespondedByUser = new HashSet<AdminRequests>();
            AdminRequestsSentByUser = new HashSet<AdminRequests>();
            Broadcasts = new HashSet<Broadcasts>();
            ProjectMinutesBooked = new HashSet<ProjectMinutesBooked>();
            ProjectUsers = new HashSet<ProjectUsers>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Salt { get; set; }

        public Roles Role { get; set; }
        public ICollection<AdminRequests> AdminRequestsRespondedByUser { get; set; }
        public ICollection<AdminRequests> AdminRequestsSentByUser { get; set; }
        public ICollection<Broadcasts> Broadcasts { get; set; }
        public ICollection<ProjectMinutesBooked> ProjectMinutesBooked { get; set; }
        public ICollection<ProjectUsers> ProjectUsers { get; set; }
    }
}
