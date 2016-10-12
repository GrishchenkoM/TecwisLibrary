using System.Data.Entity;
using Core.Entities;

namespace BusinessLogic
{
    public class DbDataContext : DbContext
    {
        public DbDataContext() : base("DefaultConnection")
        { }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
    }
}
