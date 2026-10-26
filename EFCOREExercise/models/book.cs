using System;
using System.Collections.Generic;
using System.Text;

namespace EFCOREExercise.models
{
    internal class book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public DateOnly PublishedYear { get; set; }
    }
}
