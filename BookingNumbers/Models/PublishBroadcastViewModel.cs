using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingNumbers.Models
{
    // Properties for publish broadcast
    public class PublishBroadcastViewModel
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public int AuthorUserId { get; set; } // ID of broadcast author
        public int? UpdateId { get; set; } // ID used when altering previous ID of the same broadcast
    }
}
