using System;
using System.Collections.Generic;

namespace DataCore.Models
{
    public partial class Broadcasts
    {
        public int BroadcastId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }

        public Users User { get; set; }
    }
}
