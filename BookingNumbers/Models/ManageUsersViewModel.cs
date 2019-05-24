using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookingNumbers.Tools;
using DataCore.Models;

namespace BookingNumbers.Models
{
    // Manage users properties
    public class ManageUsersViewModel
    {
        public List<TableProjectUser> AllUsers { get; set; } // List of all users
        public string NewRole { get; set; } // Used for selecting a new role
        public SelectUser SelectedUser { get; set; }

        public List<string> ProjectNames { get; set; }
        public string SelectedProjectName { get; set; }

        public string NewPassword { get; set; }

        public List<string> AllRoles { get; set; } // List of all roles
    }
}
