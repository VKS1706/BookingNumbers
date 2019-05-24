using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Models;

namespace BookingNumbers.Models
{
    // Stores all the data for the delete user from project view.
    public class DeleteUserFromProjectViewModel
    {
        public List<string> AllProjectNames { get; set; }
        public string SelectedProject { get; set; } // The name of the project the user has selected
        public List<Users> UsersInProject { get; set; } // A list of users in the project
    }
}
