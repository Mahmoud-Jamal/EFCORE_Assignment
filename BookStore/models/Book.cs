using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.models
{
    internal class Book
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? ISBN { get; set; }

        public decimal Price { get; set; }

        public int NumberOfPages { get; set; }

        public int PublishedYear { get; set; }

        public bool IsInStock { get; set; }

    }
}
