using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataCore.Models;

namespace BookingNumbers.Tools
{
    public class SelectUser
    {
        public Users User { get; set; }
        public string Role { get; set; }
        public List<string> MemberOfProjects { get; set; } // A list of project titles the user is a member of
    }
}
