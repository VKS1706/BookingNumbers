using BookingNumbers.Tools;
using DataCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingNumbers.Models
{
    // Mailbox properties
    public class MailboxViewModel
    {
        public List<FormattedRequest> MailList { get; set; } // List of requests
    }
}
