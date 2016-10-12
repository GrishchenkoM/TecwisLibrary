using Core.Entities;

namespace BusinessLogic.Repositories
{
    public class BookRepository : Repository<Book>
    {
        public BookRepository(DbDataContext context) : base(context)
        { }
    }
}
