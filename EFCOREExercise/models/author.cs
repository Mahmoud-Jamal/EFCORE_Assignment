using System;
using System.Collections.Generic;
using System.Text;

namespace EFCOREExercise.models
{
    internal class author
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Biography { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        // Navigation property
        public ICollection<book> Books { get; set; } = new List<book>();
    }
}
