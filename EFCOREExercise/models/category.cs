using System;
using System.Collections.Generic;
using System.Text;

namespace EFCOREExercise.models
{
    internal class category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        // Navigation property
        public ICollection<book> Books { get; set; } = new List<book>();
    }
}
