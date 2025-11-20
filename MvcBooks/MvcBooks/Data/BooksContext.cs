using Microsoft.EntityFrameworkCore;
using MvcBooks.Models;

namespace MvcBooks.Data
{
    public class BooksContext : DbContext
    {
        public BooksContext(DbContextOptions<BooksContext> options)
            : base(options)
        {
        }

        // CORREGIDO: El modelo se llama Book, no Books
        public DbSet<Book> Books { get; set; } = null!;
    }
}
