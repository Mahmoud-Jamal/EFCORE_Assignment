using System;
using System.Collections.Generic;
using System.Text;

namespace EventHub.models
{
    public class Badge
    {
        public int Id { get; set; }
        public string BadgeNumber { get; set; } = string.Empty;
        public DateTime IssueDate { get; set; }
        public BadgeTier Tier { get; set; }

        // Foreign Key to Attendee
        public int AttendeeId { get; set; }
        public Attendee? Attendee { get; set; }
    }
}
