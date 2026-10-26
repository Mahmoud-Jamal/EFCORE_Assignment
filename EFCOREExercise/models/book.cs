using System;
using System.Collections.Generic;
using System.Text;

namespace EFCOREExercise.models
{
    internal class book
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int NumberOfPages { get; set; }

        public int PublishedYear { get; set; }

        public bool IsInStock { get; set; }

        // Foreign keys
        public int AuthorId { get; set; }

        public int CategoryId { get; set; }

        // Navigation properties
        public author Author { get; set; } = null!;

        public category Category { get; set; } = null!;
    }
}
