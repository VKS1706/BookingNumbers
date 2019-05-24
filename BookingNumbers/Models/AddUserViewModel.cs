using DataCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingNumbers.Models
{
    // Properties for Add User
    public class AddUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; } // Validation for password
        public string RoleName { get; set; } // Admin or staff
        public List<Roles> AllRoles { get; set; } // Should be admin or staff
        public string ErrorMessage { get; set; } // Used when input is invalid
    }
}
