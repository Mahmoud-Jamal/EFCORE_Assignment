using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EventHub.models
{
    [Table("Attendees")]
    public class Attendee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Street { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("ZipCode")]
        public string PostalCode { get; set; } = string.Empty;

        // Computed property excluded from database generation
        [NotMapped]
        public string FullAddress => $"{Street}, {City}, {Country} {PostalCode}".Trim();

        // Navigation property
        public Badge? Badge { get; set; }
    }
}
