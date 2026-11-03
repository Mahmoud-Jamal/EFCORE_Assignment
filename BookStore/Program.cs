using Microsoft.EntityFrameworkCore;

namespace BookStore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var bookStoreContext = new bookStoreDbContext();

            var books = bookStoreContext.Books.ToList();

            foreach (var book in books)
            {
                Console.WriteLine($"Title: {book.Title}");
            }

        }
    }
}
