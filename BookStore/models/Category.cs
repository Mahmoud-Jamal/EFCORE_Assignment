using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.models
{
    internal class Category
    {
        public int Id { get; set; }

        public string? Name { get; set; }


        public ICollection<Book> Books { get; set; } = new HashSet<Book>();

    }
}
