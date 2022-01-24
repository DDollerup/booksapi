namespace MinimalAPI.Models
{
    public class BookContext : DbContext
    {
        public BookContext(DbContextOptions<BookContext> options) : base(options) { }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<CD> CDs => Set<CD>();

    }
}