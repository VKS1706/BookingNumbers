using System;
using System.Collections.Generic;

namespace DataCore.Models
{
    public partial class AdminRequests
    {
        public int RequestId { get; set; }
        public string RequestDescription { get; set; }
        public bool Responded { get; set; }
        public bool? Response { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime? DateResponded { get; set; }
        public int RequestTypeId { get; set; }
        public int SentByUserId { get; set; }
        public int? RespondedByUserId { get; set; }

        public RequestTypes RequestType { get; set; }
        public Users RespondedByUser { get; set; }
        public Users SentByUser { get; set; }
    }
}
