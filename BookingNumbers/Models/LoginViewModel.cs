using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Models;

namespace BookingNumbers.Models
{
    // Properties for login view
    public class LoginViewModel
    {
        [DisplayName("Username")] // Used to pull the username data in
        public string UserName { get; set; }

        [DisplayName("Password")] // Used to pull the password data in
        public string Password { get; set; }

        public List<Broadcasts> LoginBroadcasts { get; set; } // List of broadcasts available at login

    }
}
