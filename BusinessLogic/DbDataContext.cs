using System.Data.Entity;
using Core.Entities;

namespace BusinessLogic
{
    public class DbDataContext : DbContext
    {
        public DbDataContext() : base("DefaultConnection")
        { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
