using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EventHub.models
{
    [Table("EventOrganizers")]
    public class Organizer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? CompanyName { get; set; }

        public bool IsVerified { get; set; }

        // Navigation properties configured via Data Annotations
        public OrganizerProfile? Profile { get; set; }

        [InverseProperty(nameof(Event.Organizer))]
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
