using System;

namespace BookingNumbers.Models
{
    // Properties for error page
    public class ErrorViewModel
    {
        public string RequestId { get; set; } // ID of the request

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); // Don't show ID if null
    }
}